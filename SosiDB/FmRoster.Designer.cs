namespace SosiDB
{
    partial class FmRoster
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmRoster));
            this.btNew = new System.Windows.Forms.Button();
            this.dgvRoster = new System.Windows.Forms.DataGridView();
            this.cbService = new System.Windows.Forms.ComboBox();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.dgvEdit = new System.Windows.Forms.DataGridView();
            this.btCreate = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btUpdate = new System.Windows.Forms.Button();
            this.btPrint = new System.Windows.Forms.Button();
            this.chDayMinus = new System.Windows.Forms.CheckBox();
            this.chUpdateOtherEmail = new System.Windows.Forms.CheckBox();
            this.cbDayMinus = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRoster = new System.Windows.Forms.TabPage();
            this.dgvAutoComplete = new System.Windows.Forms.DataGridView();
            this.tpReminder = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdit)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpRoster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutoComplete)).BeginInit();
            this.SuspendLayout();
            // 
            // btNew
            // 
            this.btNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNew.Location = new System.Drawing.Point(466, 5);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(79, 28);
            this.btNew.TabIndex = 5;
            this.btNew.Text = "New";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // dgvRoster
            // 
            this.dgvRoster.AllowUserToAddRows = false;
            this.dgvRoster.AllowUserToDeleteRows = false;
            this.dgvRoster.AllowUserToResizeColumns = false;
            this.dgvRoster.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.NullValue = null;
            this.dgvRoster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRoster.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRoster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRoster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRoster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvRoster.Location = new System.Drawing.Point(8, 36);
            this.dgvRoster.MultiSelect = false;
            this.dgvRoster.Name = "dgvRoster";
            this.dgvRoster.RowHeadersVisible = false;
            this.dgvRoster.RowHeadersWidth = 5;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRoster.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRoster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoster.Size = new System.Drawing.Size(879, 338);
            this.dgvRoster.TabIndex = 2;
            this.dgvRoster.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvRoster_MouseDown);
            this.dgvRoster.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvRoster_CellBeginEdit);
            this.dgvRoster.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoster_CellEndEdit);
            this.dgvRoster.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoster_CellClick);
            this.dgvRoster.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvRoster_EditingControlShowing);
            this.dgvRoster.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvRoster_ColumnAdded);
            // 
            // cbService
            // 
            this.cbService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbService.FormattingEnabled = true;
            this.cbService.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.cbService.Location = new System.Drawing.Point(156, 8);
            this.cbService.Name = "cbService";
            this.cbService.Size = new System.Drawing.Size(124, 24);
            this.cbService.TabIndex = 3;
            this.cbService.Text = "Services";
            this.cbService.SelectedIndexChanged += new System.EventHandler(this.cbService_SelectedIndexChanged);
            // 
            // dtpMonth
            // 
            this.dtpMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpMonth.Location = new System.Drawing.Point(9, 11);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dtpMonth.RightToLeftLayout = true;
            this.dtpMonth.Size = new System.Drawing.Size(134, 22);
            this.dtpMonth.TabIndex = 4;
            this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
            // 
            // dgvEdit
            // 
            this.dgvEdit.AllowUserToAddRows = false;
            this.dgvEdit.AllowUserToDeleteRows = false;
            this.dgvEdit.AllowUserToResizeColumns = false;
            this.dgvEdit.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.NullValue = null;
            this.dgvEdit.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvEdit.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.dgvEdit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEdit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvEdit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEdit.Location = new System.Drawing.Point(46, 56);
            this.dgvEdit.MultiSelect = false;
            this.dgvEdit.Name = "dgvEdit";
            this.dgvEdit.ReadOnly = true;
            this.dgvEdit.RowHeadersVisible = false;
            this.dgvEdit.RowHeadersWidth = 5;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvEdit.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvEdit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEdit.Size = new System.Drawing.Size(375, 193);
            this.dgvEdit.TabIndex = 272;
            // 
            // btCreate
            // 
            this.btCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCreate.Location = new System.Drawing.Point(466, 4);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(79, 28);
            this.btCreate.TabIndex = 275;
            this.btCreate.Text = "Create Roster";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Visible = false;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // btEdit
            // 
            this.btEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEdit.Location = new System.Drawing.Point(561, 5);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(79, 28);
            this.btEdit.TabIndex = 276;
            this.btEdit.Text = "Edit";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btUpdate
            // 
            this.btUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btUpdate.Location = new System.Drawing.Point(561, 5);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(79, 28);
            this.btUpdate.TabIndex = 277;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = true;
            this.btUpdate.Visible = false;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // btPrint
            // 
            this.btPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPrint.Location = new System.Drawing.Point(753, 3);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(79, 28);
            this.btPrint.TabIndex = 278;
            this.btPrint.Text = "Print";
            this.btPrint.UseVisualStyleBackColor = true;
            this.btPrint.Click += new System.EventHandler(this.btPrint_Click);
            // 
            // chDayMinus
            // 
            this.chDayMinus.AutoSize = true;
            this.chDayMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chDayMinus.Location = new System.Drawing.Point(646, 10);
            this.chDayMinus.Name = "chDayMinus";
            this.chDayMinus.Size = new System.Drawing.Size(78, 19);
            this.chDayMinus.TabIndex = 282;
            this.chDayMinus.Text = "Days B4";
            this.chDayMinus.UseVisualStyleBackColor = true;
            // 
            // chUpdateOtherEmail
            // 
            this.chUpdateOtherEmail.AutoSize = true;
            this.chUpdateOtherEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chUpdateOtherEmail.Location = new System.Drawing.Point(373, 11);
            this.chUpdateOtherEmail.Name = "chUpdateOtherEmail";
            this.chUpdateOtherEmail.Size = new System.Drawing.Size(81, 19);
            this.chUpdateOtherEmail.TabIndex = 283;
            this.chUpdateOtherEmail.Text = "UdEmail";
            this.chUpdateOtherEmail.UseVisualStyleBackColor = true;
            // 
            // cbDayMinus
            // 
            this.cbDayMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDayMinus.FormattingEnabled = true;
            this.cbDayMinus.Items.AddRange(new object[] {
            "-0  Days",
            "-6  Days",
            "-13 Days",
            "-30 Days"});
            this.cbDayMinus.Location = new System.Drawing.Point(287, 8);
            this.cbDayMinus.Name = "cbDayMinus";
            this.cbDayMinus.Size = new System.Drawing.Size(73, 24);
            this.cbDayMinus.TabIndex = 284;
            this.cbDayMinus.Text = "Services";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpRoster);
            this.tabControl1.Controls.Add(this.tpReminder);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(2, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(903, 410);
            this.tabControl1.TabIndex = 285;
            // 
            // tpRoster
            // 
            this.tpRoster.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tpRoster.Controls.Add(this.dgvAutoComplete);
            this.tpRoster.Controls.Add(this.btPrint);
            this.tpRoster.Controls.Add(this.cbDayMinus);
            this.tpRoster.Controls.Add(this.btCreate);
            this.tpRoster.Controls.Add(this.chUpdateOtherEmail);
            this.tpRoster.Controls.Add(this.btUpdate);
            this.tpRoster.Controls.Add(this.chDayMinus);
            this.tpRoster.Controls.Add(this.dtpMonth);
            this.tpRoster.Controls.Add(this.btNew);
            this.tpRoster.Controls.Add(this.btEdit);
            this.tpRoster.Controls.Add(this.cbService);
            this.tpRoster.Controls.Add(this.dgvEdit);
            this.tpRoster.Controls.Add(this.dgvRoster);
            this.tpRoster.Location = new System.Drawing.Point(4, 25);
            this.tpRoster.Name = "tpRoster";
            this.tpRoster.Padding = new System.Windows.Forms.Padding(3);
            this.tpRoster.Size = new System.Drawing.Size(895, 381);
            this.tpRoster.TabIndex = 0;
            this.tpRoster.Text = "Roster";
            // 
            // dgvAutoComplete
            // 
            this.dgvAutoComplete.AllowUserToAddRows = false;
            this.dgvAutoComplete.AllowUserToDeleteRows = false;
            this.dgvAutoComplete.AllowUserToResizeColumns = false;
            this.dgvAutoComplete.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle7.NullValue = null;
            this.dgvAutoComplete.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvAutoComplete.BackgroundColor = System.Drawing.SystemColors.ButtonShadow;
            this.dgvAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAutoComplete.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvAutoComplete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAutoComplete.Location = new System.Drawing.Point(506, 56);
            this.dgvAutoComplete.MultiSelect = false;
            this.dgvAutoComplete.Name = "dgvAutoComplete";
            this.dgvAutoComplete.ReadOnly = true;
            this.dgvAutoComplete.RowHeadersVisible = false;
            this.dgvAutoComplete.RowHeadersWidth = 5;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAutoComplete.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvAutoComplete.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAutoComplete.Size = new System.Drawing.Size(218, 92);
            this.dgvAutoComplete.TabIndex = 285;
            // 
            // tpReminder
            // 
            this.tpReminder.Location = new System.Drawing.Point(4, 25);
            this.tpReminder.Name = "tpReminder";
            this.tpReminder.Padding = new System.Windows.Forms.Padding(3);
            this.tpReminder.Size = new System.Drawing.Size(895, 381);
            this.tpReminder.TabIndex = 1;
            this.tpReminder.Text = "Reminder";
            this.tpReminder.UseVisualStyleBackColor = true;
            // 
            // FmRoster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(906, 423);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(922, 462);
            this.MinimumSize = new System.Drawing.Size(922, 462);
            this.Name = "FmRoster";
            this.Text = "SosiDB[Roster]";
            this.Load += new System.EventHandler(this.FmRoster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEdit)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpRoster.ResumeLayout(false);
            this.tpRoster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutoComplete)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.DataGridView dgvRoster;
        private System.Windows.Forms.ComboBox cbService;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.DataGridView dgvEdit;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.Button btUpdate;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.CheckBox chDayMinus;
        private System.Windows.Forms.CheckBox chUpdateOtherEmail;
        private System.Windows.Forms.ComboBox cbDayMinus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpRoster;
        private System.Windows.Forms.TabPage tpReminder;
        private System.Windows.Forms.DataGridView dgvAutoComplete;
    }
}

