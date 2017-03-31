namespace SosiDB
{
    partial class FmRecords
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmRecords));
            this.btForeignCurr = new System.Windows.Forms.Button();
            this.btOfferingInput = new System.Windows.Forms.Button();
            this.btBanlLodge = new System.Windows.Forms.Button();
            this.btOtheAcct = new System.Windows.Forms.Button();
            this.btRegister = new System.Windows.Forms.Button();
            this.ssRegNew = new System.Windows.Forms.StatusStrip();
            this.tsslbDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsRoster = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.mnControlPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnLateRoster = new System.Windows.Forms.ToolStripMenuItem();
            this.mnBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEditMsg = new System.Windows.Forms.ToolStripMenuItem();
            this.mnErrorMsg = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEmailPad = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.btRoster = new System.Windows.Forms.Button();
            this.mnModTableData = new System.Windows.Forms.ToolStripMenuItem();
            this.ssRegNew.SuspendLayout();
            this.cmsRoster.SuspendLayout();
            this.SuspendLayout();
            // 
            // btForeignCurr
            // 
            this.btForeignCurr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btForeignCurr.Location = new System.Drawing.Point(188, 155);
            this.btForeignCurr.Name = "btForeignCurr";
            this.btForeignCurr.Size = new System.Drawing.Size(156, 42);
            this.btForeignCurr.TabIndex = 5;
            this.btForeignCurr.Text = "Foreign Currency";
            this.btForeignCurr.UseVisualStyleBackColor = true;
            this.btForeignCurr.Click += new System.EventHandler(this.btForeignCurr_Click);
            // 
            // btOfferingInput
            // 
            this.btOfferingInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOfferingInput.Location = new System.Drawing.Point(12, 77);
            this.btOfferingInput.Name = "btOfferingInput";
            this.btOfferingInput.Size = new System.Drawing.Size(157, 44);
            this.btOfferingInput.TabIndex = 2;
            this.btOfferingInput.Text = "Offering Input";
            this.btOfferingInput.UseVisualStyleBackColor = true;
            this.btOfferingInput.Click += new System.EventHandler(this.btOfferingInput_Click);
            // 
            // btBanlLodge
            // 
            this.btBanlLodge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBanlLodge.Location = new System.Drawing.Point(13, 155);
            this.btBanlLodge.Name = "btBanlLodge";
            this.btBanlLodge.Size = new System.Drawing.Size(156, 42);
            this.btBanlLodge.TabIndex = 4;
            this.btBanlLodge.Text = "Bank Lodge";
            this.btBanlLodge.UseVisualStyleBackColor = true;
            this.btBanlLodge.Click += new System.EventHandler(this.btBanlLodge_Click);
            // 
            // btOtheAcct
            // 
            this.btOtheAcct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOtheAcct.Location = new System.Drawing.Point(188, 77);
            this.btOtheAcct.Name = "btOtheAcct";
            this.btOtheAcct.Size = new System.Drawing.Size(156, 42);
            this.btOtheAcct.TabIndex = 3;
            this.btOtheAcct.Text = "Accounts";
            this.btOtheAcct.UseVisualStyleBackColor = true;
            this.btOtheAcct.Click += new System.EventHandler(this.btOtheAcct_Click);
            // 
            // btRegister
            // 
            this.btRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRegister.Location = new System.Drawing.Point(13, 12);
            this.btRegister.Name = "btRegister";
            this.btRegister.Size = new System.Drawing.Size(156, 44);
            this.btRegister.TabIndex = 1;
            this.btRegister.Text = "Register";
            this.btRegister.UseVisualStyleBackColor = true;
            this.btRegister.Click += new System.EventHandler(this.btRegister_Click);
            // 
            // ssRegNew
            // 
            this.ssRegNew.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslbDB});
            this.ssRegNew.Location = new System.Drawing.Point(0, 214);
            this.ssRegNew.Name = "ssRegNew";
            this.ssRegNew.Size = new System.Drawing.Size(358, 22);
            this.ssRegNew.TabIndex = 271;
            this.ssRegNew.Text = "statusStrip1";
            // 
            // tsslbDB
            // 
            this.tsslbDB.Name = "tsslbDB";
            this.tsslbDB.Size = new System.Drawing.Size(40, 17);
            this.tsslbDB.Text = "| DB ...";
            // 
            // cmsRoster
            // 
            this.cmsRoster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnRestore,
            this.mnControlPanel,
            this.mnLateRoster,
            this.mnBackup,
            this.mnEditMsg,
            this.mnErrorMsg,
            this.mnEmailPad,
            this.mnDuplicate,
            this.mnModTableData});
            this.cmsRoster.Name = "MnsRoster";
            this.cmsRoster.Size = new System.Drawing.Size(153, 224);
            // 
            // mnRestore
            // 
            this.mnRestore.Name = "mnRestore";
            this.mnRestore.Size = new System.Drawing.Size(152, 22);
            this.mnRestore.Text = "Restore";
            this.mnRestore.Click += new System.EventHandler(this.mnRestore_Click);
            // 
            // mnControlPanel
            // 
            this.mnControlPanel.Name = "mnControlPanel";
            this.mnControlPanel.Size = new System.Drawing.Size(152, 22);
            this.mnControlPanel.Text = "Control Panel ";
            this.mnControlPanel.Click += new System.EventHandler(this.mnControlPanel_Click);
            // 
            // mnLateRoster
            // 
            this.mnLateRoster.Name = "mnLateRoster";
            this.mnLateRoster.Size = new System.Drawing.Size(152, 22);
            this.mnLateRoster.Text = "Late Roster";
            this.mnLateRoster.Click += new System.EventHandler(this.mnLateRoster_Click);
            // 
            // mnBackup
            // 
            this.mnBackup.Name = "mnBackup";
            this.mnBackup.Size = new System.Drawing.Size(152, 22);
            this.mnBackup.Text = "Backup";
            this.mnBackup.Click += new System.EventHandler(this.mnBackup_Click);
            // 
            // mnEditMsg
            // 
            this.mnEditMsg.Name = "mnEditMsg";
            this.mnEditMsg.Size = new System.Drawing.Size(152, 22);
            this.mnEditMsg.Text = "EditMsg";
            this.mnEditMsg.Click += new System.EventHandler(this.mnEditMsg_Click);
            // 
            // mnErrorMsg
            // 
            this.mnErrorMsg.Name = "mnErrorMsg";
            this.mnErrorMsg.Size = new System.Drawing.Size(152, 22);
            this.mnErrorMsg.Text = "Error Msg";
            this.mnErrorMsg.Click += new System.EventHandler(this.mnErrorMsg_Click);
            // 
            // mnEmailPad
            // 
            this.mnEmailPad.Name = "mnEmailPad";
            this.mnEmailPad.Size = new System.Drawing.Size(152, 22);
            this.mnEmailPad.Text = "EmailPad";
            this.mnEmailPad.Click += new System.EventHandler(this.mnEmailPad_Click);
            // 
            // mnDuplicate
            // 
            this.mnDuplicate.Name = "mnDuplicate";
            this.mnDuplicate.Size = new System.Drawing.Size(152, 22);
            this.mnDuplicate.Text = "DB Duplicate";
            this.mnDuplicate.Click += new System.EventHandler(this.mnDuplicate_Click);
            // 
            // btRoster
            // 
            this.btRoster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRoster.Location = new System.Drawing.Point(188, 12);
            this.btRoster.Name = "btRoster";
            this.btRoster.Size = new System.Drawing.Size(156, 44);
            this.btRoster.TabIndex = 272;
            this.btRoster.Text = "Roster";
            this.btRoster.UseVisualStyleBackColor = true;
            this.btRoster.Click += new System.EventHandler(this.btRoster_Click);
            // 
            // mnModTableData
            // 
            this.mnModTableData.Name = "mnModTableData";
            this.mnModTableData.Size = new System.Drawing.Size(152, 22);
            this.mnModTableData.Text = "ModTableData";
            this.mnModTableData.Click += new System.EventHandler(this.mnModTableData_Click);
            // 
            // FmRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(358, 236);
            this.ContextMenuStrip = this.cmsRoster;
            this.Controls.Add(this.btRoster);
            this.Controls.Add(this.ssRegNew);
            this.Controls.Add(this.btRegister);
            this.Controls.Add(this.btOtheAcct);
            this.Controls.Add(this.btBanlLodge);
            this.Controls.Add(this.btOfferingInput);
            this.Controls.Add(this.btForeignCurr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(374, 275);
            this.MinimumSize = new System.Drawing.Size(374, 275);
            this.Name = "FmRecords";
            this.Text = "SosiDB [Records]";
            this.Load += new System.EventHandler(this.FmRecords_Load);
            this.ssRegNew.ResumeLayout(false);
            this.ssRegNew.PerformLayout();
            this.cmsRoster.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btForeignCurr;
        private System.Windows.Forms.Button btOfferingInput;
        private System.Windows.Forms.Button btBanlLodge;
        private System.Windows.Forms.Button btOtheAcct;
        private System.Windows.Forms.Button btRegister;
        private System.Windows.Forms.StatusStrip ssRegNew;
        private System.Windows.Forms.ToolStripStatusLabel tsslbDB;
        private System.Windows.Forms.ContextMenuStrip cmsRoster;
        private System.Windows.Forms.ToolStripMenuItem mnLateRoster;
        private System.Windows.Forms.ToolStripMenuItem mnBackup;
        private System.Windows.Forms.ToolStripMenuItem mnEditMsg;
        private System.Windows.Forms.ToolStripMenuItem mnErrorMsg;
        private System.Windows.Forms.Button btRoster;
        private System.Windows.Forms.ToolStripMenuItem mnRestore;
        private System.Windows.Forms.ToolStripMenuItem mnEmailPad;
        private System.Windows.Forms.ToolStripMenuItem mnControlPanel;
        private System.Windows.Forms.ToolStripMenuItem mnDuplicate;
        private System.Windows.Forms.ToolStripMenuItem mnModTableData;

    }
}