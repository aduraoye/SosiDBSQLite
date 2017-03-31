namespace SosiDB
{
    partial class FmUsers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmUsers));
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btCreate = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.dgvUserLogin = new System.Windows.Forms.DataGridView();
            this.lbTitle = new System.Windows.Forms.Label();
            this.txPWConfirmation = new SosiDB.TextBox_Ad();
            this.cbUserGroup = new SosiDB.ComboBox_Ad();
            this.txUsername = new SosiDB.TextBox_Ad();
            this.txPassword = new SosiDB.TextBox_Ad();
            this.txEmail = new SosiDB.TextBox_Ad();
            this.txFName = new SosiDB.TextBox_Ad();
            this.txLName = new SosiDB.TextBox_Ad();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserLogin)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(329, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 16);
            this.label9.TabIndex = 112;
            this.label9.Text = "E-mail ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(178, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 109;
            this.label1.Text = "Last Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(72, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 16);
            this.label2.TabIndex = 110;
            this.label2.Text = "First Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(233, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 115;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(127, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 16);
            this.label4.TabIndex = 116;
            this.label4.Text = "Username";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 16);
            this.label5.TabIndex = 117;
            this.label5.Text = "User Group";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(335, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 120;
            this.label7.Text = "Confirm PW";
            // 
            // btCreate
            // 
            this.btCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCreate.Location = new System.Drawing.Point(5, 111);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(104, 32);
            this.btCreate.TabIndex = 5;
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // btCancel
            // 
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancel.Location = new System.Drawing.Point(332, 111);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(103, 32);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btDelete
            // 
            this.btDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDelete.Location = new System.Drawing.Point(167, 111);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(104, 32);
            this.btDelete.TabIndex = 7;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // dgvUserLogin
            // 
            this.dgvUserLogin.AllowUserToAddRows = false;
            this.dgvUserLogin.AllowUserToDeleteRows = false;
            this.dgvUserLogin.AllowUserToResizeColumns = false;
            this.dgvUserLogin.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.NullValue = null;
            this.dgvUserLogin.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUserLogin.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvUserLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserLogin.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUserLogin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserLogin.Location = new System.Drawing.Point(5, 149);
            this.dgvUserLogin.MultiSelect = false;
            this.dgvUserLogin.Name = "dgvUserLogin";
            this.dgvUserLogin.ReadOnly = true;
            this.dgvUserLogin.RowHeadersVisible = false;
            this.dgvUserLogin.RowHeadersWidth = 5;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvUserLogin.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUserLogin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUserLogin.Size = new System.Drawing.Size(430, 250);
            this.dgvUserLogin.TabIndex = 259;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(9, 29);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(39, 16);
            this.lbTitle.TabIndex = 260;
            this.lbTitle.Text = "Title";
            // 
            // txPWConfirmation
            // 
            this.txPWConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txPWConfirmation.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txPWConfirmation.Init_Text = "Yes";
            this.txPWConfirmation.Location = new System.Drawing.Point(332, 76);
            this.txPWConfirmation.MaxLength = 15;
            this.txPWConfirmation.Name = "txPWConfirmation";
            this.txPWConfirmation.PasswordChar = '•';
            this.txPWConfirmation.Size = new System.Drawing.Size(103, 22);
            this.txPWConfirmation.TabIndex = 4;
            this.txPWConfirmation.Text = "Password Confirmation";
            this.txPWConfirmation.Valu_Type = "String";
            this.txPWConfirmation.WordWrap = false;
            // 
            // cbUserGroup
            // 
            this.cbUserGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUserGroup.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.cbUserGroup.FormattingEnabled = true;
            this.cbUserGroup.Init_Text = "Yes";
            this.cbUserGroup.Items.AddRange(new object[] {
            "Adminstrator",
            "Standard"});
            this.cbUserGroup.Location = new System.Drawing.Point(5, 75);
            this.cbUserGroup.MaxDropDownItems = 2;
            this.cbUserGroup.Name = "cbUserGroup";
            this.cbUserGroup.Size = new System.Drawing.Size(104, 24);
            this.cbUserGroup.TabIndex = 1;
            this.cbUserGroup.Text = "Usergroup";
            // 
            // txUsername
            // 
            this.txUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txUsername.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txUsername.Init_Text = "Yes";
            this.txUsername.Location = new System.Drawing.Point(114, 76);
            this.txUsername.MaxLength = 15;
            this.txUsername.Name = "txUsername";
            this.txUsername.Size = new System.Drawing.Size(103, 22);
            this.txUsername.TabIndex = 2;
            this.txUsername.Text = "Username";
            this.txUsername.Valu_Type = "String";
            this.txUsername.WordWrap = false;
            // 
            // txPassword
            // 
            this.txPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txPassword.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txPassword.Init_Text = "Yes";
            this.txPassword.Location = new System.Drawing.Point(223, 76);
            this.txPassword.MaxLength = 15;
            this.txPassword.Name = "txPassword";
            this.txPassword.PasswordChar = '•';
            this.txPassword.Size = new System.Drawing.Size(103, 22);
            this.txPassword.TabIndex = 3;
            this.txPassword.Text = "Password";
            this.txPassword.Valu_Type = "String";
            this.txPassword.WordWrap = false;
            // 
            // txEmail
            // 
            this.txEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txEmail.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txEmail.Init_Text = "Yes";
            this.txEmail.Location = new System.Drawing.Point(289, 25);
            this.txEmail.MaxLength = 70;
            this.txEmail.Name = "txEmail";
            this.txEmail.ReadOnly = true;
            this.txEmail.Size = new System.Drawing.Size(146, 22);
            this.txEmail.TabIndex = 103;
            this.txEmail.Text = "E-mail Address";
            this.txEmail.Valu_Type = "String";
            this.txEmail.WordWrap = false;
            // 
            // txFName
            // 
            this.txFName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txFName.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txFName.Init_Text = "Yes";
            this.txFName.Location = new System.Drawing.Point(59, 25);
            this.txFName.MaxLength = 15;
            this.txFName.Name = "txFName";
            this.txFName.ReadOnly = true;
            this.txFName.Size = new System.Drawing.Size(103, 22);
            this.txFName.TabIndex = 100;
            this.txFName.Text = "First Name";
            this.txFName.Valu_Type = "String";
            this.txFName.WordWrap = false;
            // 
            // txLName
            // 
            this.txLName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txLName.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txLName.Init_Text = "Yes";
            this.txLName.Location = new System.Drawing.Point(168, 25);
            this.txLName.MaxLength = 15;
            this.txLName.Name = "txLName";
            this.txLName.ReadOnly = true;
            this.txLName.Size = new System.Drawing.Size(103, 22);
            this.txLName.TabIndex = 101;
            this.txLName.Text = "Surname";
            this.txLName.Valu_Type = "String";
            this.txLName.WordWrap = false;
            // 
            // FmUsers
            // 
            this.AcceptButton = this.btCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(442, 404);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.dgvUserLogin);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txPWConfirmation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbUserGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txEmail);
            this.Controls.Add(this.txFName);
            this.Controls.Add(this.txLName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(458, 443);
            this.MinimumSize = new System.Drawing.Size(458, 443);
            this.Name = "FmUsers";
            this.Text = "SosiDB [Users]";
            this.Load += new System.EventHandler(this.FmUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserLogin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox_Ad txEmail;
        private TextBox_Ad txFName;
        private TextBox_Ad txLName;
        private ComboBox_Ad cbUserGroup;
        private System.Windows.Forms.Label label9;
        private TextBox_Ad txUsername;
        private System.Windows.Forms.Label label1;
        private TextBox_Ad txPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private TextBox_Ad txPWConfirmation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.DataGridView dgvUserLogin;
        private System.Windows.Forms.Label lbTitle;
    }
}