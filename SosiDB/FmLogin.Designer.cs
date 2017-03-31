namespace SosiDB
{
    partial class FmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txUsername = new System.Windows.Forms.TextBox();
            this.txPassword = new System.Windows.Forms.TextBox();
            this.btLogin = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbChurchList = new System.Windows.Forms.ComboBox();
            this.txValidation = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbParishName = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cmsBackground = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSchedulerMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.niBackground = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbTime = new System.Windows.Forms.Label();
            this.rtSmsMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.cmsBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // txUsername
            // 
            this.txUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txUsername.Location = new System.Drawing.Point(117, 16);
            this.txUsername.Name = "txUsername";
            this.txUsername.Size = new System.Drawing.Size(114, 26);
            this.txUsername.TabIndex = 2;
            this.txUsername.Text = "Admin";
            // 
            // txPassword
            // 
            this.txPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txPassword.Location = new System.Drawing.Point(117, 50);
            this.txPassword.Name = "txPassword";
            this.txPassword.PasswordChar = '•';
            this.txPassword.Size = new System.Drawing.Size(114, 26);
            this.txPassword.TabIndex = 3;
            this.txPassword.Text = "adura004";
            // 
            // btLogin
            // 
            this.btLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btLogin.Location = new System.Drawing.Point(117, 80);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(114, 27);
            this.btLogin.TabIndex = 4;
            this.btLogin.Text = "  Login";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbChurchList);
            this.groupBox1.Controls.Add(this.btLogin);
            this.groupBox1.Controls.Add(this.txValidation);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txPassword);
            this.groupBox1.Controls.Add(this.txUsername);
            this.groupBox1.Location = new System.Drawing.Point(114, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 142);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Admin";
            // 
            // cbChurchList
            // 
            this.cbChurchList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbChurchList.FormattingEnabled = true;
            this.cbChurchList.Location = new System.Drawing.Point(11, 82);
            this.cbChurchList.Name = "cbChurchList";
            this.cbChurchList.Size = new System.Drawing.Size(99, 24);
            this.cbChurchList.TabIndex = 56;
            this.cbChurchList.Text = "NewChurch";
            this.cbChurchList.SelectedIndexChanged += new System.EventHandler(this.cbChurchList_SelectedIndexChanged);
            // 
            // txValidation
            // 
            this.txValidation.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txValidation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txValidation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txValidation.ForeColor = System.Drawing.Color.Red;
            this.txValidation.Location = new System.Drawing.Point(11, 107);
            this.txValidation.Multiline = true;
            this.txValidation.Name = "txValidation";
            this.txValidation.Size = new System.Drawing.Size(220, 31);
            this.txValidation.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(365, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Monotype Corsiva", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(12, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(425, 33);
            this.label3.TabIndex = 7;
            this.label3.Text = "The Redeemed Christian Church of God";
            // 
            // lbParishName
            // 
            this.lbParishName.AutoSize = true;
            this.lbParishName.Font = new System.Drawing.Font("Monotype Corsiva", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbParishName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbParishName.Location = new System.Drawing.Point(31, 38);
            this.lbParishName.Name = "lbParishName";
            this.lbParishName.Size = new System.Drawing.Size(394, 25);
            this.lbParishName.TabIndex = 8;
            this.lbParishName.Text = "(Fountain of Life Parish Canberra) Database";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(2, 110);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(108, 113);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // cmsBackground
            // 
            this.cmsBackground.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnRestore,
            this.mnSchedulerMonitor,
            this.mnClose});
            this.cmsBackground.Name = "contextMenuStrip1";
            this.cmsBackground.Size = new System.Drawing.Size(170, 70);
            // 
            // mnRestore
            // 
            this.mnRestore.Name = "mnRestore";
            this.mnRestore.Size = new System.Drawing.Size(169, 22);
            this.mnRestore.Text = "Restore";
            this.mnRestore.Click += new System.EventHandler(this.mnRestore_Click);
            // 
            // mnSchedulerMonitor
            // 
            this.mnSchedulerMonitor.Name = "mnSchedulerMonitor";
            this.mnSchedulerMonitor.Size = new System.Drawing.Size(169, 22);
            this.mnSchedulerMonitor.Text = "SchedulerMonitor";
            this.mnSchedulerMonitor.Click += new System.EventHandler(this.mnSchedulerMonitor_Click);
            // 
            // mnClose
            // 
            this.mnClose.Name = "mnClose";
            this.mnClose.Size = new System.Drawing.Size(169, 22);
            this.mnClose.Text = "Close";
            this.mnClose.Click += new System.EventHandler(this.mnClose_Click);
            // 
            // niBackground
            // 
            this.niBackground.ContextMenuStrip = this.cmsBackground;
            this.niBackground.Icon = ((System.Drawing.Icon)(resources.GetObject("niBackground.Icon")));
            this.niBackground.Text = "SosiDB";
            this.niBackground.Visible = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.BackColor = System.Drawing.SystemColors.ControlText;
            this.lbTime.Font = new System.Drawing.Font("Lucida Sans Typewriter", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbTime.Location = new System.Drawing.Point(2, 84);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(76, 22);
            this.lbTime.TabIndex = 54;
            this.lbTime.Text = "label7";
            // 
            // rtSmsMsg
            // 
            this.rtSmsMsg.Location = new System.Drawing.Point(421, 166);
            this.rtSmsMsg.Name = "rtSmsMsg";
            this.rtSmsMsg.Size = new System.Drawing.Size(44, 40);
            this.rtSmsMsg.TabIndex = 55;
            this.rtSmsMsg.Text = "";
            // 
            // FmLogin
            // 
            this.AcceptButton = this.btLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(477, 240);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.rtSmsMsg);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lbParishName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(493, 279);
            this.MinimumSize = new System.Drawing.Size(493, 279);
            this.Name = "FmLogin";
            this.Text = "SosiDB [Login]";
            this.Load += new System.EventHandler(this.FmLogin_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FmLogin_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.cmsBackground.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txUsername;
        private System.Windows.Forms.TextBox txPassword;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbParishName;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ContextMenuStrip cmsBackground;
        private System.Windows.Forms.ToolStripMenuItem mnRestore;
        private System.Windows.Forms.ToolStripMenuItem mnSchedulerMonitor;
        private System.Windows.Forms.ToolStripMenuItem mnClose;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbTime;
        public System.Windows.Forms.NotifyIcon niBackground;
        private System.Windows.Forms.TextBox txValidation;
        private System.Windows.Forms.ComboBox cbChurchList;
        private System.Windows.Forms.RichTextBox rtSmsMsg;
    }
}

