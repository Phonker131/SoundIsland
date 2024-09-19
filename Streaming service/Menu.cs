using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.DirectoryServices;
using Microsoft.Win32;

namespace Halfyearproject
{
    public partial class Menu : Form
    {



        public Menu()
        {
            InitializeComponent();
            CheckPythonEnable();
            oldSize = new Size(Width, Height);

        }

        // Global Variables
        private bool isClickedOnSearchingBar = false; // Bool variable that indicates that user clicked on searching bar
        private Size oldSize; // Variable used to remember previous size of the window
        string path; // Variable that stores path to python.exe

        /// <summary>
        /// Function used to find python.exe path in register, if it exists.
        /// It gets the nothing and returns nothing.
        /// </summary>
        public void CheckPythonEnable()
        {
            try
            {
                string version = FindVersion(Registry.LocalMachine);
                using (RegistryKey? key = Registry.LocalMachine.OpenSubKey($"SOFTWARE\\Python\\PythonCore\\{version}\\InstallPath", RegistryKeyPermissionCheck.ReadSubTree))
                {
                    if (key != null)
                    {
                        path = key.GetValue("ExecutablePath").ToString();

                    }
                    else
                    {
                        throw new Exception("no key");
                    }
                }

            }
            catch (Exception ex)
            {
                string version = FindVersion(Registry.CurrentUser);
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\Python\\PythonCore\\{version}\\InstallPath", RegistryKeyPermissionCheck.ReadSubTree))
                {
                    if (key != null)
                    {
                        path = key.GetValue("ExecutablePath").ToString();

                    }
                }

            }


        }

        /// <summary>
        /// Function used to find python version in register.
        /// It gets the keyDirectory(Key directory in windows register) and returns string version(Version of python downloaded on users computer), if it exists.
        /// </summary>
        /// <param name="keyDirectory"></param>
        /// <returns></returns>
        public string FindVersion(RegistryKey keyDirectory)
        {
            using (RegistryKey? pythonKey = keyDirectory.OpenSubKey("SOFTWARE\\Python\\PythonCore", RegistryKeyPermissionCheck.ReadSubTree))
            {
                if (pythonKey != null)
                {
                    string[] subKeyNames = pythonKey.GetSubKeyNames();

                    foreach (string subKeyName in subKeyNames)
                    {
                        using (RegistryKey subKey = pythonKey.OpenSubKey(subKeyName))
                        {
                            if (subKey != null)
                            {
                                string version = subKey.GetValue("SysVersion").ToString();

                                if (!string.IsNullOrEmpty(version))
                                {
                                    return version;
                                }
                            }
                        }
                    }
                }
            }
            return null;

        }

        /// <summary>
        /// Function conecnted to button search on Menu frame. 
        /// It gets the obj sender and EventArg e and in result of working sends text from searching bar to next frame and opens SearchingResultPage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Search_Click(object sender, EventArgs e)
        {
            Search.Enabled = true;
            SerchingResultPage taskWindow = new SerchingResultPage(SearchingBar.Text, path);
            taskWindow.Show();
            this.Hide();
        }
        /// <summary>
        /// Function conected to searching bar and used to change style of searching bar when user clicks on it. 
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchingBar_Click(object sender, EventArgs e)
        {
            if (!isClickedOnSearchingBar)
            {
                SearchingBar.Text = string.Empty;
                SearchingBar.BackColor = Color.LightGray;
                isClickedOnSearchingBar = true;
                SearchingBar.Select(0, 0);
            }
        }
        /// <summary>
        /// Function conected to frame and used to change style of searching bar and delete text from searching bar if user doesn't use it.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Click(object sender, EventArgs e)
        {
            if (isClickedOnSearchingBar)
            {
                SearchingBar.BackColor = SystemColors.Control;
                SearchingBar.PlaceholderText = " ";
                isClickedOnSearchingBar = false;
                SearchingBar.Select(0, 0);
            }
        }
        /// <summary>
        /// Function conected to frame and used to kill the proces of app and clean cache after app is closed 
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo("Song Cache");

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
        /// Function conected to frame and used to remember old size of window and in result method makes call to another method ResizeAll.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }
        /// <summary>
        /// Function used to resize all objects and buttons on window.
        /// It gets the 2 args control(used to resize objects) and newSize(used to calculate new sizes of objects)
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
        /// Function conected to searching bar and used to change param Enabled on text.
        /// It gets the obj sender and EventArg e and returns nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchingBar_TextChanged(object sender, EventArgs e)
        {
            string searchText = SearchingBar.Text;

            if (searchText == "")
            {
                Search.Enabled = false;
            }
            else
            {
                Search.Enabled = true;
            }
        }
    }

}