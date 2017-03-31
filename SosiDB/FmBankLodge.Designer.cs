namespace SosiDB
{
    partial class FmBankLodge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmBankLodge));
            this.lbOfferingDescript = new System.Windows.Forms.Label();
            this.dgvBankLodge = new System.Windows.Forms.DataGridView();
            this.btAdd = new System.Windows.Forms.Button();
            this.lbTotal = new System.Windows.Forms.Label();
            this.gbAdd = new System.Windows.Forms.GroupBox();
            this.cbNameSearch = new SosiDB.ComboBox_Ad();
            this.dtpOffering = new System.Windows.Forms.DateTimePicker();
            this.btStatement = new System.Windows.Forms.Button();
            this.cbOfferingStmnt = new System.Windows.Forms.ComboBox();
            this.txBLTotal = new SosiDB.TextBox_Ad();
            this.dtpOfferingFrom = new System.Windows.Forms.DateTimePicker();
            this.txBLAmount = new SosiDB.TextBox_Ad();
            this.txNameSearch = new SosiDB.TextBox_Ad();
            this.cbRegId = new SosiDB.ComboBox_Ad();
            this.lbAUD = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.mnEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbPayMethod = new System.Windows.Forms.ToolStripComboBox();
            this.tstxAmount = new System.Windows.Forms.ToolStripTextBox();
            this.tsbtCalAmount = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankLodge)).BeginInit();
            this.gbAdd.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbOfferingDescript
            // 
            this.lbOfferingDescript.AutoSize = true;
            this.lbOfferingDescript.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOfferingDescript.Location = new System.Drawing.Point(225, 40);
            this.lbOfferingDescript.Name = "lbOfferingDescript";
            this.lbOfferingDescript.Size = new System.Drawing.Size(59, 16);
            this.lbOfferingDescript.TabIndex = 94;
            this.lbOfferingDescript.Text = "Amount";
            // 
            // dgvBankLodge
            // 
            this.dgvBankLodge.AllowUserToAddRows = false;
            this.dgvBankLodge.AllowUserToDeleteRows = false;
            this.dgvBankLodge.AllowUserToResizeColumns = false;
            this.dgvBankLodge.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.NullValue = null;
            this.dgvBankLodge.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBankLodge.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvBankLodge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Monotype Corsiva", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBankLodge.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBankLodge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBankLodge.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBankLodge.Location = new System.Drawing.Point(5, 132);
            this.dgvBankLodge.MultiSelect = false;
            this.dgvBankLodge.Name = "dgvBankLodge";
            this.dgvBankLodge.ReadOnly = true;
            this.dgvBankLodge.RowHeadersVisible = false;
            this.dgvBankLodge.RowHeadersWidth = 5;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBankLodge.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBankLodge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBankLodge.Size = new System.Drawing.Size(452, 287);
            this.dgvBankLodge.TabIndex = 117;
            // 
            // btAdd
            // 
            this.btAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAdd.Location = new System.Drawing.Point(367, 52);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(78, 29);
            this.btAdd.TabIndex = 109;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // lbTotal
            // 
            this.lbTotal.AutoSize = true;
            this.lbTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotal.Location = new System.Drawing.Point(6, 14);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(98, 16);
            this.lbTotal.TabIndex = 100;
            this.lbTotal.Text = "Total (AUD$)";
            // 
            // gbAdd
            // 
            this.gbAdd.Controls.Add(this.cbNameSearch);
            this.gbAdd.Controls.Add(this.lbOfferingDescript);
            this.gbAdd.Controls.Add(this.dtpOffering);
            this.gbAdd.Controls.Add(this.btStatement);
            this.gbAdd.Controls.Add(this.cbOfferingStmnt);
            this.gbAdd.Controls.Add(this.txBLTotal);
            this.gbAdd.Controls.Add(this.lbTotal);
            this.gbAdd.Controls.Add(this.btAdd);
            this.gbAdd.Controls.Add(this.dtpOfferingFrom);
            this.gbAdd.Controls.Add(this.txBLAmount);
            this.gbAdd.Controls.Add(this.txNameSearch);
            this.gbAdd.Controls.Add(this.cbRegId);
            this.gbAdd.Controls.Add(this.lbAUD);
            this.gbAdd.Location = new System.Drawing.Point(5, 37);
            this.gbAdd.Name = "gbAdd";
            this.gbAdd.Size = new System.Drawing.Size(452, 89);
            this.gbAdd.TabIndex = 122;
            this.gbAdd.TabStop = false;
            // 
            // cbNameSearch
            // 
            this.cbNameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNameSearch.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbNameSearch.FormattingEnabled = true;
            this.cbNameSearch.Init_Text = "No";
            this.cbNameSearch.Items.AddRange(new object[] {
            "RegID",
            "First Name",
            "Last Name",
            "No Name"});
            this.cbNameSearch.Location = new System.Drawing.Point(6, 57);
            this.cbNameSearch.MaxDropDownItems = 2;
            this.cbNameSearch.Name = "cbNameSearch";
            this.cbNameSearch.Size = new System.Drawing.Size(82, 21);
            this.cbNameSearch.TabIndex = 276;
            this.cbNameSearch.Text = "First Name";
            this.cbNameSearch.SelectedIndexChanged += new System.EventHandler(this.cbNameSearch_SelectedIndexChanged);
            // 
            // dtpOffering
            // 
            this.dtpOffering.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpOffering.Location = new System.Drawing.Point(223, 13);
            this.dtpOffering.Name = "dtpOffering";
            this.dtpOffering.Size = new System.Drawing.Size(222, 20);
            this.dtpOffering.TabIndex = 122;
            // 
            // btStatement
            // 
            this.btStatement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btStatement.Location = new System.Drawing.Point(368, 51);
            this.btStatement.Name = "btStatement";
            this.btStatement.Size = new System.Drawing.Size(78, 30);
            this.btStatement.TabIndex = 132;
            this.btStatement.Text = "Statement";
            this.btStatement.UseVisualStyleBackColor = true;
            // 
            // cbOfferingStmnt
            // 
            this.cbOfferingStmnt.FormattingEnabled = true;
            this.cbOfferingStmnt.Items.AddRange(new object[] {
            "All Offerings",
            "Normal Offering",
            "Tithes",
            "First Fruit",
            "Thanksgiving Offering",
            "Missions Offering",
            "Building Funds",
            "Pledges",
            "Special Offering",
            "Other Offerings/Funds"});
            this.cbOfferingStmnt.Location = new System.Drawing.Point(223, 56);
            this.cbOfferingStmnt.Name = "cbOfferingStmnt";
            this.cbOfferingStmnt.Size = new System.Drawing.Size(101, 21);
            this.cbOfferingStmnt.TabIndex = 135;
            this.cbOfferingStmnt.SelectedIndexChanged += new System.EventHandler(this.cbOfferingStmnt_SelectedIndexChanged);
            // 
            // txBLTotal
            // 
            this.txBLTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txBLTotal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txBLTotal.Init_Text = "No";
            this.txBLTotal.Location = new System.Drawing.Point(99, 11);
            this.txBLTotal.Name = "txBLTotal";
            this.txBLTotal.ReadOnly = true;
            this.txBLTotal.Size = new System.Drawing.Size(113, 22);
            this.txBLTotal.TabIndex = 99;
            this.txBLTotal.Text = "0.00";
            this.txBLTotal.Valu_Type = "Int";
            // 
            // dtpOfferingFrom
            // 
            this.dtpOfferingFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpOfferingFrom.Location = new System.Drawing.Point(6, 12);
            this.dtpOfferingFrom.Name = "dtpOfferingFrom";
            this.dtpOfferingFrom.Size = new System.Drawing.Size(206, 20);
            this.dtpOfferingFrom.TabIndex = 128;
            // 
            // txBLAmount
            // 
            this.txBLAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txBLAmount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txBLAmount.Init_Text = "No";
            this.txBLAmount.Location = new System.Drawing.Point(223, 56);
            this.txBLAmount.Name = "txBLAmount";
            this.txBLAmount.Size = new System.Drawing.Size(101, 22);
            this.txBLAmount.TabIndex = 3;
            this.txBLAmount.Text = "0.00";
            this.txBLAmount.Valu_Type = "Float";
            // 
            // txNameSearch
            // 
            this.txNameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txNameSearch.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txNameSearch.Init_Text = "Yes";
            this.txNameSearch.Location = new System.Drawing.Point(94, 57);
            this.txNameSearch.MaxLength = 15;
            this.txNameSearch.Name = "txNameSearch";
            this.txNameSearch.Size = new System.Drawing.Size(118, 20);
            this.txNameSearch.TabIndex = 275;
            this.txNameSearch.Text = "First Name";
            this.txNameSearch.Valu_Type = "String";
            this.txNameSearch.WordWrap = false;
            // 
            // cbRegId
            // 
            this.cbRegId.DropDownWidth = 150;
            this.cbRegId.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbRegId.Init_Text = "No";
            this.cbRegId.Location = new System.Drawing.Point(94, 56);
            this.cbRegId.Name = "cbRegId";
            this.cbRegId.Size = new System.Drawing.Size(118, 21);
            this.cbRegId.TabIndex = 125;
            this.cbRegId.Text = "Other Deposit";
            this.cbRegId.SelectedIndexChanged += new System.EventHandler(this.cbRegId_SelectedIndexChanged);
            this.cbRegId.TextChanged += new System.EventHandler(this.cbRegId_TextChanged);
            // 
            // lbAUD
            // 
            this.lbAUD.AutoSize = true;
            this.lbAUD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAUD.Location = new System.Drawing.Point(322, 61);
            this.lbAUD.Name = "lbAUD";
            this.lbAUD.Size = new System.Drawing.Size(48, 16);
            this.lbAUD.TabIndex = 277;
            this.lbAUD.Text = "AUD$";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.mnEdit,
            this.toolStripSeparator3,
            this.mnDelete,
            this.toolStripSeparator1,
            this.MnPrint,
            this.toolStripSeparator4,
            this.tscbPayMethod,
            this.tstxAmount,
            this.tsbtCalAmount,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(5, 11);
            this.toolStrip1.MinimumSize = new System.Drawing.Size(452, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(452, 25);
            this.toolStrip1.TabIndex = 262;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(10, 22);
            this.toolStripLabel1.Text = " ";
            // 
            // mnEdit
            // 
            this.mnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnEdit.Image = ((System.Drawing.Image)(resources.GetObject("mnEdit.Image")));
            this.mnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnEdit.Name = "mnEdit";
            this.mnEdit.Size = new System.Drawing.Size(48, 22);
            this.mnEdit.Text = "Edit";
            this.mnEdit.Click += new System.EventHandler(this.mnEdit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // mnDelete
            // 
            this.mnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnDelete.Image = ((System.Drawing.Image)(resources.GetObject("mnDelete.Image")));
            this.mnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnDelete.Name = "mnDelete";
            this.mnDelete.Size = new System.Drawing.Size(65, 22);
            this.mnDelete.Text = "Delete";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MnPrint
            // 
            this.MnPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MnPrint.Image = ((System.Drawing.Image)(resources.GetObject("MnPrint.Image")));
            this.MnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MnPrint.Name = "MnPrint";
            this.MnPrint.Size = new System.Drawing.Size(54, 22);
            this.MnPrint.Text = "Print";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbPayMethod
            // 
            this.tscbPayMethod.Items.AddRange(new object[] {
            "Bank",
            "Cash",
            "Both"});
            this.tscbPayMethod.Name = "tscbPayMethod";
            this.tscbPayMethod.Size = new System.Drawing.Size(75, 25);
            // 
            // tstxAmount
            // 
            this.tstxAmount.Name = "tstxAmount";
            this.tstxAmount.Size = new System.Drawing.Size(70, 25);
            // 
            // tsbtCalAmount
            // 
            this.tsbtCalAmount.Image = ((System.Drawing.Image)(resources.GetObject("tsbtCalAmount.Image")));
            this.tsbtCalAmount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtCalAmount.Name = "tsbtCalAmount";
            this.tsbtCalAmount.Size = new System.Drawing.Size(44, 22);
            this.tsbtCalAmount.Text = "Cal";
            this.tsbtCalAmount.Click += new System.EventHandler(this.tsbtCalAmount_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // FmBankLodge
            // 
            this.AcceptButton = this.btAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(466, 429);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbAdd);
            this.Controls.Add(this.dgvBankLodge);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(482, 468);
            this.MinimumSize = new System.Drawing.Size(482, 468);
            this.Name = "FmBankLodge";
            this.Text = "SosiDB[BankLodge]";
            this.Load += new System.EventHandler(this.BankLodge_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BankLodge_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankLodge)).EndInit();
            this.gbAdd.ResumeLayout(false);
            this.gbAdd.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbOfferingDescript;
        private System.Windows.Forms.DataGridView dgvBankLodge;
        private System.Windows.Forms.Button btAdd;
        private TextBox_Ad txBLAmount;
        private TextBox_Ad txBLTotal;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.GroupBox gbAdd;
        private System.Windows.Forms.DateTimePicker dtpOffering;
        private System.Windows.Forms.Button btStatement;
        private System.Windows.Forms.ComboBox cbOfferingStmnt;
        private System.Windows.Forms.DateTimePicker dtpOfferingFrom;
        private ComboBox_Ad cbRegId;
        private ComboBox_Ad cbNameSearch;
        private TextBox_Ad txNameSearch;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton mnEdit;
        private System.Windows.Forms.ToolStripButton mnDelete;
        private System.Windows.Forms.ToolStripButton MnPrint;
        private System.Windows.Forms.Label lbAUD;
        private System.Windows.Forms.ToolStripTextBox tstxAmount;
        private System.Windows.Forms.ToolStripButton tsbtCalAmount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox tscbPayMethod;
    }
}