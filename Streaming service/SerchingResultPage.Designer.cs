namespace Halfyearproject
{
    partial class SerchingResultPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button1 = new Button();
            volumeBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)volumeBar).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(176, -1);
            label1.Name = "label1";
            label1.Size = new Size(317, 37);
            label1.TabIndex = 0;
            label1.Text = "Search result by query: ";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(12, 9);
            button1.Name = "button1";
            button1.Size = new Size(141, 27);
            button1.TabIndex = 1;
            button1.Text = "Back To Menu";
            button1.UseVisualStyleBackColor = true;
            button1.Click += BackToMenu_Click;
            // 
            // volumeBar
            // 
            volumeBar.BackColor = SystemColors.ActiveCaptionText;
            volumeBar.Location = new Point(623, 137);
            volumeBar.Name = "volumeBar";
            volumeBar.Size = new Size(104, 45);
            volumeBar.TabIndex = 2;
            volumeBar.Value = 3;
            volumeBar.Visible = false;
            volumeBar.Scroll += changeVolume;
            // 
            // SerchingResultPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(volumeBar);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "SerchingResultPage";
            Text = "SerchingResultPage";
            FormClosing += SerchingResultPage_FormClosing;
            SizeChanged += SerchingResultPage_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)volumeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private TrackBar volumeBar;
    }
}