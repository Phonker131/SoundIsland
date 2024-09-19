using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using static Community.CsharpSqlite.Sqlite3;
using System.Net;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Security.Cryptography.X509Certificates;
using WMPLib;
using System.Numerics;

namespace Halfyearproject
{
    public partial class SerchingResultPage : Form
    {
        public SerchingResultPage(string text, string path)
        {
            InitializeComponent();
            this.path = path;
            player = new WMPLib.WindowsMediaPlayer();
            changeVolume(null, null);
            pauseButton = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\" + "Pause Button.png");
            continueButton = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\" + "Continue Button.png");
            MakeRequest(text);
            oldSize = new Size(Width, Height);
            label1.Text = $"Search result by query: {text}";
            CreateNewPlayer();
        }

        // Global variables
        string path; // Variable that stores path to python.exe
        Image pauseButton; // Initialization of variable that stores pause button image.
        Image continueButton; // Initialization of variable that stores continue button image.
        List<System.Windows.Forms.Button> buttons = new List<System.Windows.Forms.Button>(); // Initialization of list that stores buttons that used as player background.
        List<System.Windows.Forms.Button> controlButtons = new List<System.Windows.Forms.Button>(); // Initialization of list that stores control buttons.
        WMPLib.WindowsMediaPlayer player; // Initialization music player.
        List<DeezerTrack> tracks = new List<DeezerTrack>(); // Initialization of list that stores links of songs.
        Dictionary<System.Windows.Forms.Button, DeezerTrack> matchID = new Dictionary<System.Windows.Forms.Button, DeezerTrack>(); // Initialization of dict that stores as key buttons and as value song links.
        private Dictionary<System.Windows.Forms.Button, SerchingResultPage.DeezerTrack> matchIDControls = new Dictionary<System.Windows.Forms.Button, SerchingResultPage.DeezerTrack>(); // Initialization of dict that stores as key control buttons and as value song links.
        const int numberOfPlayers = 5; // Initialization of constant that stores max number of players.
        private Size oldSize; // Initialization of variable that stores old size of frame.

        /// <summary>
        /// Function used to make request to Deezer API, recive data and store it.
        /// It gets the text from searching bar on previous frame and in result makes call to another function.
        /// </summary>
        /// <param name="text"></param>
        public async void MakeRequest(string text)
        {
            string name = text;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://deezerdevs-deezer.p.rapidapi.com/search?q=" + name + "&limit=5"),
                    Headers =
            {
                { "X-RapidAPI-Key", "dfe3ecdb68msh351d604eceaf701p12a457jsn1996d326a887" },
                { "X-RapidAPI-Host", "deezerdevs-deezer.p.rapidapi.com" },
            },
                };
                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(body);
                        tracks = new List<DeezerTrack>();
                        if (result != null)
                        {
                            foreach (var item in result.data)
                            {
                                DeezerTrack track = new DeezerTrack()
                                {
                                    Title = item.title,
                                    ArtistName = item.artist.name,
                                    AlbumName = item.album.title,
                                    Duration = item.duration,
                                    PreviewUrl = item.preview,
                                    DeezerUrl = item.link,
                                    Cover = item.album.cover,
                                    CoverMid = item.album.coverMid,
                                    CoverSmall = item.album.coverSmall,
                                    CoverXL = item.album.coverXL,
                                };
                                tracks.Add(track);
                            }
                        }
                    }

                }
                catch (System.NullReferenceException)
                {
                    MakeRequest(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + "please make a screan and send me this error! /// my contact @mail");
                }
            }

            getSong();

        }
        /// <summary>
        /// Function used to rewrite all the links to Song link.txt file that stores all the song links.
        /// It gets the nothing and in result calls another functions run_cmd and checkSongEnable.
        /// </summary>
        private void getSong()
        {
            string[] song_url = new string[tracks.Count];
            for (int i = 0; i < tracks.Count; i++)
            {
                song_url[i] = tracks[i].DeezerUrl.ToString();
            }
            if (File.Exists("Song links.txt"))
            {
                File.WriteAllText("Song links.txt", string.Empty);
            }

            using (StreamWriter writer = new StreamWriter("Song links.txt"))
            {
                foreach (string url in song_url)
                {
                    writer.WriteLine(url);
                }
            }
            run_cmd("downloadSong.py");
            checkSongEnable();
        }
        /// <summary>
        /// Function used to run Python file that used to download all the tracks using their links.
        /// It gets the param cmd(Name of the Python file) and returns nothing.
        /// </summary>
        /// <param name="cmd"></param>
        private void run_cmd(string cmd)
        {
            Action action = () =>
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = path;
                start.Arguments = string.Format("{0}", cmd);
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;


                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                    }
                }
            };

            Thread myThread = new Thread(new ThreadStart(action));
            myThread.Start();

        }
        /// <summary>
        /// Function used to check if song mp3 file is dowloaded. We compare name of directory(As names we use tracks ID) with track ID to check attendance of song.
        /// It gets the nothing and in result makes call to function ManagePlayer.
        /// </summary>
        private void checkSongEnable()
        {
            Action action = () =>
            {
                int count = 0;
                Dictionary<DeezerTrack, System.Windows.Forms.Button> checkSongs = new Dictionary<DeezerTrack, System.Windows.Forms.Button>();
                Dictionary<DeezerTrack, System.Windows.Forms.Button> checkSongsControls = new Dictionary<DeezerTrack, System.Windows.Forms.Button>();
                foreach (DeezerTrack track in tracks)
                {
                    checkSongs[track] = buttons[count];
                    checkSongsControls[track] = this.controlButtons[count];
                    count++;

                }
                matchID = checkSongs.ToDictionary(entry => entry.Value, entry => entry.Key);
                matchIDControls = checkSongsControls.ToDictionary(entry => entry.Value, entry => entry.Key);
                count = 0;
                while (true)
                {
                    if (count < numberOfPlayers)
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo("Song Cache");
                        foreach (KeyValuePair<DeezerTrack, System.Windows.Forms.Button> track in checkSongs)
                        {
                            string folderName = track.Key.DeezerUrl.Split("/")[^1];
                            if (di.GetDirectories().Any(dir => dir.Name == folderName))
                            {
                                ManagePlayer(track.Key, track.Value, checkSongsControls[track.Key]);
                                checkSongs.Remove(track.Key);
                                count++;

                            }
                        }
                    }
                    else if (count == numberOfPlayers)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            };

            Thread myThread = new Thread(new ThreadStart(action));
            myThread.Start();
        }
        /// <summary>
        /// Function used to set desing and functional of player.
        /// It gets the 3 params track(list that stores song ID's) button(list that stores buttons) and controlButton(list that stores control buttons).
        /// </summary>
        /// <param name="track"></param>
        /// <param name="button"></param>
        /// <param name="controlButton"></param>
        public async void ManagePlayer(DeezerTrack track, System.Windows.Forms.Button button, System.Windows.Forms.Button controlButton)
        {
            string imageUrl = track.Cover;
            Image image;

            using (var webClient = new System.Net.WebClient())
            {
                byte[] imageData = webClient.DownloadData(imageUrl);
                using (var stream = new System.IO.MemoryStream(imageData))
                {
                    image = Image.FromStream(stream);
                }
            }
            button.Enabled = true;
            controlButton.Enabled = true;
            controlButton.BackColor = Color.Black;
            button.BackColor = Color.Black;
            button.ForeColor = Color.White;
            button.TextAlign = ContentAlignment.TopRight;
            button.Text = track.ArtistName + " - " + track.Title;
            ImageList imageList = new ImageList();
            imageList.Images.Add(image);
            imageList.Images.Add(continueButton);
            imageList.Images.Add(pauseButton);
            imageList.ImageSize = new Size(75, 75);
            button.ImageList = imageList;
            button.ImageIndex = 0;
            button.ImageAlign = ContentAlignment.MiddleLeft;
            controlButton.ImageList = imageList;
            controlButton.ImageIndex = 1;


            controlButton.MouseClick += (sender, args) =>
            {
                System.IO.DirectoryInfo di = new DirectoryInfo("Song Cache" + "\\" + track.DeezerUrl.Split("/")[^1]);
                string songName = "";
                foreach (FileInfo name in di.GetFiles())
                {
                    if (name.Name.EndsWith("mp3"))
                    {
                        songName = name.Name;
                    }
                }
                string path = System.IO.Directory.GetCurrentDirectory() + "\\" + "Song Cache" + "\\" + track.DeezerUrl.Split("/")[^1] + "\\" + songName;
                if (path == player.URL && player.playState == WMPPlayState.wmppsPlaying)
                {
                    volumeBar.Visible = false;
                    player.controls.pause();
                    controlButton.ImageIndex = 1;
                }
                else
                {
                    volumeBar.Location = new(button.Location.X + 375, button.Location.Y + 25);
                    volumeBar.Visible = true;
                    foreach (System.Windows.Forms.Button b in controlButtons)
                    {
                        b.ImageIndex = 1;
                    }
                    player.URL = path;
                    player.controls.play();
                    controlButton.ImageIndex = 2;
                }
            };

        }
        /// <summary>
        /// Function connected to volume bar and used to change volume of track.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeVolume(object sender, EventArgs e)
        {
            player.settings.volume = volumeBar.Value * 10;
        }

        /// <summary>
        /// Function used to generate all buttons used to make backgroud of player and control buttons and set all the buttons sizes.
        /// It gets the nothing and returns nothing.
        /// </summary>
        public async void CreateNewPlayer()
        {

            buttons = new List<System.Windows.Forms.Button>();
            controlButtons = new List<System.Windows.Forms.Button>();
            int buttonWidth = 500;
            int buttonHeight = 70;
            int x = 35;
            int y = 50;

            for (int i = 0; i < numberOfPlayers; i++)
            {
                System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                System.Windows.Forms.Button controlButton = new System.Windows.Forms.Button();
                this.buttons.Add(button);
                this.controlButtons.Add(controlButton);
                controlButton.Size = new Size(70, 70);
                button.Size = new Size(buttonWidth, buttonHeight);
                controlButton.Location = new Point(x + 85, y + 80 * i);
                button.Location = new Point(x, y + 80 * i);
                Controls.Add(controlButton);
                Controls.Add(button);
                controlButton.Name = "controlButton";
                controlButton.BackColor = Color.White;
                controlButton.FlatStyle = FlatStyle.Flat;
                controlButton.FlatAppearance.BorderSize = 0;
                controlButton.Enabled = false;
                button.Name = "player";
                button.BackColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Enabled = false;

                button.Click += (object sender, EventArgs e) =>
                {
                    string playerName = ((System.Windows.Forms.Button)sender).Name;
                };
            }
        }


        /// <summary>
        /// *FUNCTION IS UNFINISHED!
        /// Function conected to frame and used to remember old size of window and in result method makes call to another method ResizeAll.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerchingResultPage_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }
        /// <summary>
        /// *FUNCTION IS UNFINISHED!
        /// Function used to resize all objects and buttons on window.
        /// It gets the 2 args control(used to resize objects) and newSize(used to calculate new sizes of objects).
        /// </summary>
        /// <param name="control"></param>
        /// <param name="newSize"></param>
        private void ResizeAll(Control control, Size newSize)
        {
            int width = newSize.Width - oldSize.Width;
            control.Left += (control.Left * width) / oldSize.Width;
            control.Width += (control.Width * width) / oldSize.Width;

            int height = newSize.Height - oldSize.Height;
            control.Top += (control.Top * height) / oldSize.Height;
            control.Height += (control.Height * height) / oldSize.Height;

        }
        /// <summary>
        /// Function connetcted to frame and used to kill the app's proces when form is closing, stop player and clean all it data and clean all the cache.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerchingResultPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo("Song Cache");

            if (player.playState == WMPPlayState.wmppsPlaying)
            {
                player.controls.stop();
            }
            player.close();
            player = null;

            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }
        /// <summary>
        /// Function conected back to menu button and used to stop player and clean all it data and clean all the cache. and return to manu frame.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMenu_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo("Song Cache");

            if (player.playState == WMPPlayState.wmppsPlaying)
            {
                player.controls.stop();
            }
            player.close();
            player = null;

            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }


            Menu taskWindow = new Menu();
            taskWindow.Show();
            this.Hide();
        }
        /// <summary>
        /// Class used to store all the data we get from Deezer API.
        /// </summary>
        public class DeezerTrack
        {
            public string Title { get; set; }
            public string ArtistName { get; set; }
            public string AlbumName { get; set; }
            public string Cover { get; set; }
            public string CoverSmall { get; set; }
            public string CoverMid { get; set; }
            public string CoverXL { get; set; }
            public int Duration { get; set; }
            public string PreviewUrl { get; set; }
            public string DeezerUrl { get; set; }

            /// <summary>
            /// Method used to convert all the data to string.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Cover.ToString();
            }
        }

    }
}

