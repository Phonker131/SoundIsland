namespace Halfyearproject
{
    partial class Menu
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
            Search = new Button();
            SearchingBar = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // Search
            // 
            Search.Enabled = false;
            Search.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            Search.Location = new Point(316, 307);
            Search.Name = "Search";
            Search.Size = new Size(136, 55);
            Search.TabIndex = 0;
            Search.Text = "Search";
            Search.UseVisualStyleBackColor = true;
            Search.Click += Search_Click;
            // 
            // SearchingBar
            // 
            SearchingBar.Location = new Point(274, 223);
            SearchingBar.Name = "SearchingBar";
            SearchingBar.Size = new Size(227, 23);
            SearchingBar.TabIndex = 2;
            SearchingBar.Text = "Type your song here";
            SearchingBar.TextAlign = HorizontalAlignment.Center;
            SearchingBar.Click += SearchingBar_Click;
            SearchingBar.TextChanged += SearchingBar_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Sitka Display", 48F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(202, 88);
            label1.Name = "label1";
            label1.Size = new Size(377, 92);
            label1.TabIndex = 3;
            label1.Text = "SoundIsland";
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(SearchingBar);
            Controls.Add(Search);
            Name = "Menu";
            Text = "Menu";
            FormClosing += Menu_FormClosing;
            SizeChanged += Menu_SizeChanged;
            Click += Menu_Click;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Search;
        private TextBox SearchingBar;
        private Label label1;
    }
}