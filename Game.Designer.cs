namespace Damka
{
    partial class Game
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.משחקחדשToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.הוראותToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.יציאהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.התחלמוזיקהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עצורמוזיקהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.משחקחדשToolStripMenuItem,
            this.הוראותToolStripMenuItem,
            this.יציאהToolStripMenuItem,
            this.התחלמוזיקהToolStripMenuItem,
            this.עצורמוזיקהToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(16, 5, 0, 5);
            this.menuStrip1.Size = new System.Drawing.Size(3844, 55);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // משחקחדשToolStripMenuItem
            // 
            this.משחקחדשToolStripMenuItem.Name = "משחקחדשToolStripMenuItem";
            this.משחקחדשToolStripMenuItem.Size = new System.Drawing.Size(180, 45);
            this.משחקחדשToolStripMenuItem.Text = "משחק חדש";
            this.משחקחדשToolStripMenuItem.Click += new System.EventHandler(this.משחקחדשToolStripMenuItem_Click);
            // 
            // הוראותToolStripMenuItem
            // 
            this.הוראותToolStripMenuItem.Name = "הוראותToolStripMenuItem";
            this.הוראותToolStripMenuItem.Size = new System.Drawing.Size(124, 45);
            this.הוראותToolStripMenuItem.Text = "הוראות";
            this.הוראותToolStripMenuItem.Click += new System.EventHandler(this.הוראותToolStripMenuItem_Click);
            // 
            // יציאהToolStripMenuItem
            // 
            this.יציאהToolStripMenuItem.Name = "יציאהToolStripMenuItem";
            this.יציאהToolStripMenuItem.Size = new System.Drawing.Size(103, 45);
            this.יציאהToolStripMenuItem.Text = "יציאה";
            this.יציאהToolStripMenuItem.Click += new System.EventHandler(this.יציאהToolStripMenuItem_Click);
            // 
            // התחלמוזיקהToolStripMenuItem
            // 
            this.התחלמוזיקהToolStripMenuItem.Name = "התחלמוזיקהToolStripMenuItem";
            this.התחלמוזיקהToolStripMenuItem.Size = new System.Drawing.Size(203, 45);
            this.התחלמוזיקהToolStripMenuItem.Text = "התחל מוזיקה";
            this.התחלמוזיקהToolStripMenuItem.Click += new System.EventHandler(this.התחלמוזיקהToolStripMenuItem_Click);
            // 
            // עצורמוזיקהToolStripMenuItem
            // 
            this.עצורמוזיקהToolStripMenuItem.Name = "עצורמוזיקהToolStripMenuItem";
            this.עצורמוזיקהToolStripMenuItem.Size = new System.Drawing.Size(185, 45);
            this.עצורמוזיקהToolStripMenuItem.Text = "עצור מוזיקה";
            this.עצורמוזיקהToolStripMenuItem.Click += new System.EventHandler(this.עצורמוזיקהToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(549, 136);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 59);
            this.label1.TabIndex = 2;
            this.label1.Text = "Black Turn";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OrangeRed;
            this.button1.Location = new System.Drawing.Point(0, 943);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(253, 152);
            this.button1.TabIndex = 3;
            this.button1.Text = "STOP";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.OrangeRed;
            this.label2.Font = new System.Drawing.Font("Showcard Gothic", 15.9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(506, 487);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 235);
            this.label2.TabIndex = 4;
            this.label2.Text = "BLACK BURNT";
            this.label2.Visible = false;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 3500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DodgerBlue;
            this.label3.Font = new System.Drawing.Font("Showcard Gothic", 15.9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(605, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(265, 235);
            this.label3.TabIndex = 5;
            this.label3.Text = "RED BURNT";
            this.label3.Visible = false;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 3500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(3844, 1674);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Game";
            this.Text = "Game";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Game_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Game_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Game_MouseUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem משחקחדשToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem הוראותToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem יציאהToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem התחלמוזיקהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem עצורמוזיקהToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer2;
    }
}

