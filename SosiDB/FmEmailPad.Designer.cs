namespace SosiDB
{
    partial class FmEmailPad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmEmailPad));
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbEmailSend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbAttachmentList = new System.Windows.Forms.ToolStripComboBox();
            this.tssbEmailAttachment = new System.Windows.Forms.ToolStripSplitButton();
            this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbEmailMessagx = new System.Windows.Forms.ToolStripSplitButton();
            this.mnBirthdayMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRosterMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.mn1stTimerMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnConvertMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnServiceReminder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSundayService = new System.Windows.Forms.ToolStripMenuItem();
            this.mnFridayService = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAnyService = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEmailSignature = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEmailFootnote = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtSaveMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.mnInsertImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbEmailCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tslbEmail = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btEmailCC = new System.Windows.Forms.Button();
            this.btEmailTo = new System.Windows.Forms.Button();
            this.gb2 = new System.Windows.Forms.GroupBox();
            this.chSignature = new System.Windows.Forms.CheckBox();
            this.chImage = new System.Windows.Forms.CheckBox();
            this.rtEmailBody = new System.Windows.Forms.RichTextBox();
            this.chEmail = new System.Windows.Forms.CheckBox();
            this.chWhatsApp = new System.Windows.Forms.CheckBox();
            this.chSMS = new System.Windows.Forms.CheckBox();
            this.rtSmsBody = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbEmailName = new System.Windows.Forms.ComboBox();
            this.txEmailSubject = new SosiDB.TextBox_Ad();
            this.txEmailCC = new SosiDB.TextBox_Ad();
            this.txEmailTo = new SosiDB.TextBox_Ad();
            this.txMPhone = new SosiDB.TextBox_Ad();
            this.toolStrip1.SuspendLayout();
            this.gb2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Subject :";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEmailSend,
            this.toolStripSeparator4,
            this.tscbAttachmentList,
            this.tssbEmailAttachment,
            this.toolStripSeparator2,
            this.tssbEmailMessagx,
            this.tsbtSaveMessage,
            this.toolStripSeparator5,
            this.toolStripSplitButton1,
            this.toolStripSeparator1,
            this.tsbEmailCancel,
            this.toolStripSeparator7,
            this.tslbEmail,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 456);
            this.toolStrip1.MinimumSize = new System.Drawing.Size(0, 35);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(607, 35);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbEmailSend
            // 
            this.tsbEmailSend.Enabled = false;
            this.tsbEmailSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbEmailSend.Image = ((System.Drawing.Image)(resources.GetObject("tsbEmailSend.Image")));
            this.tsbEmailSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmailSend.Name = "tsbEmailSend";
            this.tsbEmailSend.Size = new System.Drawing.Size(55, 32);
            this.tsbEmailSend.Text = "Send";
            this.tsbEmailSend.Click += new System.EventHandler(this.tsbEmailSend_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // tscbAttachmentList
            // 
            this.tscbAttachmentList.Name = "tscbAttachmentList";
            this.tscbAttachmentList.Size = new System.Drawing.Size(100, 35);
            this.tscbAttachmentList.Text = "Attachment List";
            // 
            // tssbEmailAttachment
            // 
            this.tssbEmailAttachment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
            this.tssbEmailAttachment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssbEmailAttachment.Image = ((System.Drawing.Image)(resources.GetObject("tssbEmailAttachment.Image")));
            this.tssbEmailAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbEmailAttachment.Name = "tssbEmailAttachment";
            this.tssbEmailAttachment.Size = new System.Drawing.Size(106, 32);
            this.tssbEmailAttachment.Text = "Attachment";
            this.tssbEmailAttachment.ButtonClick += new System.EventHandler(this.tssbEmailAttachment_ButtonClick);
            // 
            // deleteSelectedToolStripMenuItem
            // 
            this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
            this.deleteSelectedToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteSelectedToolStripMenuItem.Text = "Delete Selected";
            this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // tssbEmailMessagx
            // 
            this.tssbEmailMessagx.DropDownButtonWidth = 0;
            this.tssbEmailMessagx.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnBirthdayMessage,
            this.mnRosterMessage,
            this.mn1stTimerMessage,
            this.mnConvertMessage,
            this.mnServiceReminder,
            this.mnEmailSignature,
            this.mnEmailFootnote});
            this.tssbEmailMessagx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssbEmailMessagx.Image = ((System.Drawing.Image)(resources.GetObject("tssbEmailMessagx.Image")));
            this.tssbEmailMessagx.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbEmailMessagx.Name = "tssbEmailMessagx";
            this.tssbEmailMessagx.Size = new System.Drawing.Size(76, 32);
            this.tssbEmailMessagx.Text = "Message";
            this.tssbEmailMessagx.ButtonClick += new System.EventHandler(this.tssbEmailMessagx_ButtonClick);
            // 
            // mnBirthdayMessage
            // 
            this.mnBirthdayMessage.Name = "mnBirthdayMessage";
            this.mnBirthdayMessage.Size = new System.Drawing.Size(171, 22);
            this.mnBirthdayMessage.Text = "Birthday";
            this.mnBirthdayMessage.Click += new System.EventHandler(this.mnBirthdayMessage_Click);
            // 
            // mnRosterMessage
            // 
            this.mnRosterMessage.Name = "mnRosterMessage";
            this.mnRosterMessage.Size = new System.Drawing.Size(171, 22);
            this.mnRosterMessage.Text = "Roster";
            this.mnRosterMessage.Click += new System.EventHandler(this.mnRosterMessage_Click);
            // 
            // mn1stTimerMessage
            // 
            this.mn1stTimerMessage.Name = "mn1stTimerMessage";
            this.mn1stTimerMessage.Size = new System.Drawing.Size(171, 22);
            this.mn1stTimerMessage.Text = "1stTimer";
            this.mn1stTimerMessage.Click += new System.EventHandler(this.mn1stTimerMessage_Click);
            // 
            // mnConvertMessage
            // 
            this.mnConvertMessage.Name = "mnConvertMessage";
            this.mnConvertMessage.Size = new System.Drawing.Size(171, 22);
            this.mnConvertMessage.Text = "Convert";
            this.mnConvertMessage.Click += new System.EventHandler(this.mnConvertMessage_Click);
            // 
            // mnServiceReminder
            // 
            this.mnServiceReminder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnSundayService,
            this.mnFridayService,
            this.mnAnyService});
            this.mnServiceReminder.Name = "mnServiceReminder";
            this.mnServiceReminder.Size = new System.Drawing.Size(171, 22);
            this.mnServiceReminder.Text = "ServiceReminder";
            // 
            // mnSundayService
            // 
            this.mnSundayService.Name = "mnSundayService";
            this.mnSundayService.Size = new System.Drawing.Size(159, 22);
            this.mnSundayService.Text = "Sunday Service";
            this.mnSundayService.Click += new System.EventHandler(this.MnSundayService_Click);
            // 
            // mnFridayService
            // 
            this.mnFridayService.Name = "mnFridayService";
            this.mnFridayService.Size = new System.Drawing.Size(159, 22);
            this.mnFridayService.Text = "Friday Service";
            this.mnFridayService.Click += new System.EventHandler(this.mnFridayService_Click);
            // 
            // mnAnyService
            // 
            this.mnAnyService.Name = "mnAnyService";
            this.mnAnyService.Size = new System.Drawing.Size(159, 22);
            this.mnAnyService.Text = "Any Service";
            this.mnAnyService.Click += new System.EventHandler(this.mnAnyService_Click);
            // 
            // mnEmailSignature
            // 
            this.mnEmailSignature.Name = "mnEmailSignature";
            this.mnEmailSignature.Size = new System.Drawing.Size(171, 22);
            this.mnEmailSignature.Text = "EmailSignature";
            this.mnEmailSignature.Click += new System.EventHandler(this.mnEmailSignature_Click);
            // 
            // mnEmailFootnote
            // 
            this.mnEmailFootnote.Name = "mnEmailFootnote";
            this.mnEmailFootnote.Size = new System.Drawing.Size(171, 22);
            this.mnEmailFootnote.Text = "EmailFootnote";
            this.mnEmailFootnote.Click += new System.EventHandler(this.mnEmailFootnote_Click);
            // 
            // tsbtSaveMessage
            // 
            this.tsbtSaveMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbtSaveMessage.Image = ((System.Drawing.Image)(resources.GetObject("tsbtSaveMessage.Image")));
            this.tsbtSaveMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSaveMessage.Name = "tsbtSaveMessage";
            this.tsbtSaveMessage.Size = new System.Drawing.Size(54, 32);
            this.tsbtSaveMessage.Text = "Save";
            this.tsbtSaveMessage.Click += new System.EventHandler(this.tsbtSaveMessage_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 35);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnInsertImage});
            this.toolStripSplitButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(62, 32);
            this.toolStripSplitButton1.Text = "More...";
            this.toolStripSplitButton1.ToolTipText = "More Actions";
            // 
            // mnInsertImage
            // 
            this.mnInsertImage.Name = "mnInsertImage";
            this.mnInsertImage.Size = new System.Drawing.Size(145, 22);
            this.mnInsertImage.Text = "Insert Image";
            this.mnInsertImage.Click += new System.EventHandler(this.mnInsertImage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // tsbEmailCancel
            // 
            this.tsbEmailCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbEmailCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsbEmailCancel.Image")));
            this.tsbEmailCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmailCancel.Name = "tsbEmailCancel";
            this.tsbEmailCancel.Size = new System.Drawing.Size(63, 32);
            this.tsbEmailCancel.Text = "Cancel";
            this.tsbEmailCancel.Click += new System.EventHandler(this.tsbEmailCancel_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 35);
            // 
            // tslbEmail
            // 
            this.tslbEmail.Name = "tslbEmail";
            this.tslbEmail.Size = new System.Drawing.Size(95, 15);
            this.tslbEmail.Text = "Ready to attach..";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // btEmailCC
            // 
            this.btEmailCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEmailCC.Location = new System.Drawing.Point(383, 18);
            this.btEmailCC.Name = "btEmailCC";
            this.btEmailCC.Size = new System.Drawing.Size(53, 23);
            this.btEmailCC.TabIndex = 293;
            this.btEmailCC.Text = "CC ...";
            this.btEmailCC.UseVisualStyleBackColor = true;
            this.btEmailCC.Click += new System.EventHandler(this.btEmailCC_Click_1);
            // 
            // btEmailTo
            // 
            this.btEmailTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEmailTo.Location = new System.Drawing.Point(8, 18);
            this.btEmailTo.Name = "btEmailTo";
            this.btEmailTo.Size = new System.Drawing.Size(53, 23);
            this.btEmailTo.TabIndex = 290;
            this.btEmailTo.Text = "To ...";
            this.btEmailTo.UseVisualStyleBackColor = true;
            // 
            // gb2
            // 
            this.gb2.Controls.Add(this.cbEmailName);
            this.gb2.Controls.Add(this.chImage);
            this.gb2.Controls.Add(this.btEmailCC);
            this.gb2.Controls.Add(this.txEmailSubject);
            this.gb2.Controls.Add(this.txEmailCC);
            this.gb2.Controls.Add(this.label1);
            this.gb2.Controls.Add(this.txEmailTo);
            this.gb2.Controls.Add(this.btEmailTo);
            this.gb2.Controls.Add(this.rtEmailBody);
            this.gb2.Location = new System.Drawing.Point(2, 103);
            this.gb2.Name = "gb2";
            this.gb2.Size = new System.Drawing.Size(600, 326);
            this.gb2.TabIndex = 294;
            this.gb2.TabStop = false;
            this.gb2.Text = " ";
            // 
            // chSignature
            // 
            this.chSignature.AutoSize = true;
            this.chSignature.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chSignature.Location = new System.Drawing.Point(489, 5);
            this.chSignature.Name = "chSignature";
            this.chSignature.Size = new System.Drawing.Size(93, 20);
            this.chSignature.TabIndex = 304;
            this.chSignature.Text = "Signature";
            this.chSignature.UseVisualStyleBackColor = true;
            // 
            // chImage
            // 
            this.chImage.AutoSize = true;
            this.chImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chImage.Location = new System.Drawing.Point(383, 49);
            this.chImage.Name = "chImage";
            this.chImage.Size = new System.Drawing.Size(112, 20);
            this.chImage.TabIndex = 303;
            this.chImage.Text = "Insert Image";
            this.chImage.UseVisualStyleBackColor = true;
            // 
            // rtEmailBody
            // 
            this.rtEmailBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtEmailBody.Location = new System.Drawing.Point(8, 75);
            this.rtEmailBody.Name = "rtEmailBody";
            this.rtEmailBody.Size = new System.Drawing.Size(583, 236);
            this.rtEmailBody.TabIndex = 20;
            this.rtEmailBody.Text = "*** Welcome to SosiDB E-mailPad. ***";
            // 
            // chEmail
            // 
            this.chEmail.AutoSize = true;
            this.chEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chEmail.Location = new System.Drawing.Point(299, 2);
            this.chEmail.Name = "chEmail";
            this.chEmail.Size = new System.Drawing.Size(66, 20);
            this.chEmail.TabIndex = 302;
            this.chEmail.Text = "Email";
            this.chEmail.UseVisualStyleBackColor = true;
            // 
            // chWhatsApp
            // 
            this.chWhatsApp.AutoSize = true;
            this.chWhatsApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chWhatsApp.Location = new System.Drawing.Point(385, 4);
            this.chWhatsApp.Name = "chWhatsApp";
            this.chWhatsApp.Size = new System.Drawing.Size(98, 20);
            this.chWhatsApp.TabIndex = 301;
            this.chWhatsApp.Text = "WhatsApp";
            this.chWhatsApp.UseVisualStyleBackColor = true;
            // 
            // chSMS
            // 
            this.chSMS.AutoSize = true;
            this.chSMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chSMS.Location = new System.Drawing.Point(10, 4);
            this.chSMS.Name = "chSMS";
            this.chSMS.Size = new System.Drawing.Size(59, 20);
            this.chSMS.TabIndex = 300;
            this.chSMS.Text = "SMS";
            this.chSMS.UseVisualStyleBackColor = true;
            this.chSMS.CheckedChanged += new System.EventHandler(this.chSMS_CheckedChanged);
            // 
            // rtSmsBody
            // 
            this.rtSmsBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtSmsBody.Location = new System.Drawing.Point(10, 30);
            this.rtSmsBody.MaxLength = 900;
            this.rtSmsBody.Name = "rtSmsBody";
            this.rtSmsBody.Size = new System.Drawing.Size(583, 77);
            this.rtSmsBody.TabIndex = 297;
            this.rtSmsBody.Text = "*** Welcome to SosiDB SMSPad. ***";
            // 
            // cbEmailName
            // 
            this.cbEmailName.FormattingEnabled = true;
            this.cbEmailName.Location = new System.Drawing.Point(496, 47);
            this.cbEmailName.Name = "cbEmailName";
            this.cbEmailName.Size = new System.Drawing.Size(95, 21);
            this.cbEmailName.TabIndex = 303;
            this.cbEmailName.Text = "Email Name";
            // 
            // txEmailSubject
            // 
            this.txEmailSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txEmailSubject.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txEmailSubject.Init_Text = "No";
            this.txEmailSubject.Location = new System.Drawing.Point(75, 47);
            this.txEmailSubject.MaxLength = 150;
            this.txEmailSubject.Name = "txEmailSubject";
            this.txEmailSubject.Size = new System.Drawing.Size(302, 22);
            this.txEmailSubject.TabIndex = 19;
            this.txEmailSubject.Text = "Subject of your Email";
            this.txEmailSubject.Valu_Type = "String";
            this.txEmailSubject.WordWrap = false;
            // 
            // txEmailCC
            // 
            this.txEmailCC.AutoCompleteCustomSource.AddRange(new string[] {
            "adurafimihan.abiona@gmail.com ;",
            "adeyinkaadebule@yahoo.com ;",
            "CHIGSMITH90@yahoo.com;",
            "oluranti.abiona@gmail.com;"});
            this.txEmailCC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txEmailCC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txEmailCC.Enabled = false;
            this.txEmailCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txEmailCC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txEmailCC.Init_Text = "No";
            this.txEmailCC.Location = new System.Drawing.Point(438, 18);
            this.txEmailCC.MaxLength = 100;
            this.txEmailCC.Name = "txEmailCC";
            this.txEmailCC.Size = new System.Drawing.Size(153, 22);
            this.txEmailCC.TabIndex = 292;
            this.txEmailCC.Text = "E-mail Address";
            this.txEmailCC.Valu_Type = "String";
            this.txEmailCC.WordWrap = false;
            // 
            // txEmailTo
            // 
            this.txEmailTo.AutoCompleteCustomSource.AddRange(new string[] {
            "adurafimihan.abiona@gmail.com ;",
            "adeyinkaadebule@yahoo.com ;",
            "CHIGSMITH90@yahoo.com;",
            "oluranti.abiona@gmail.com;"});
            this.txEmailTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txEmailTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txEmailTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txEmailTo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txEmailTo.Init_Text = "No";
            this.txEmailTo.Location = new System.Drawing.Point(75, 19);
            this.txEmailTo.MaxLength = 3276799;
            this.txEmailTo.Name = "txEmailTo";
            this.txEmailTo.Size = new System.Drawing.Size(302, 22);
            this.txEmailTo.TabIndex = 291;
            this.txEmailTo.Text = "Recepient\'s E-mail Adress(es)";
            this.txEmailTo.Valu_Type = "String";
            this.txEmailTo.WordWrap = false;
            // 
            // txMPhone
            // 
            this.txMPhone.Enabled = false;
            this.txMPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txMPhone.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txMPhone.Init_Text = "No";
            this.txMPhone.Location = new System.Drawing.Point(67, 3);
            this.txMPhone.MaxLength = 100;
            this.txMPhone.Name = "txMPhone";
            this.txMPhone.Size = new System.Drawing.Size(189, 22);
            this.txMPhone.TabIndex = 296;
            this.txMPhone.Text = "MPhone Number";
            this.txMPhone.Valu_Type = "String";
            this.txMPhone.WordWrap = false;
            // 
            // FmEmailPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(607, 491);
            this.Controls.Add(this.chEmail);
            this.Controls.Add(this.chSignature);
            this.Controls.Add(this.gb2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.rtSmsBody);
            this.Controls.Add(this.chWhatsApp);
            this.Controls.Add(this.txMPhone);
            this.Controls.Add(this.chSMS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(623, 530);
            this.MinimumSize = new System.Drawing.Size(623, 530);
            this.Name = "FmEmailPad";
            this.Text = "SosiDB [EmailPad]";
            this.Load += new System.EventHandler(this.FmEmailPad_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gb2.ResumeLayout(false);
            this.gb2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox_Ad txEmailSubject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbEmailSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbEmailCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox tscbAttachmentList;
        private System.Windows.Forms.ToolStripSplitButton tssbEmailAttachment;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel tslbEmail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Button btEmailCC;
        private TextBox_Ad txEmailCC;
        private TextBox_Ad txEmailTo;
        private System.Windows.Forms.Button btEmailTo;
        private System.Windows.Forms.GroupBox gb2;
        private TextBox_Ad txMPhone;
        private System.Windows.Forms.ToolStripSplitButton tssbEmailMessagx;
        private System.Windows.Forms.ToolStripMenuItem mnBirthdayMessage;
        private System.Windows.Forms.ToolStripMenuItem mnRosterMessage;
        private System.Windows.Forms.ToolStripMenuItem mn1stTimerMessage;
        private System.Windows.Forms.ToolStripMenuItem mnConvertMessage;
        private System.Windows.Forms.ToolStripButton tsbtSaveMessage;
        private System.Windows.Forms.ToolStripMenuItem mnServiceReminder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem mnInsertImage;
        private System.Windows.Forms.ToolStripMenuItem mnSundayService;
        private System.Windows.Forms.ToolStripMenuItem mnFridayService;
        private System.Windows.Forms.ToolStripMenuItem mnAnyService;
        private System.Windows.Forms.ToolStripMenuItem mnEmailFootnote;
        private System.Windows.Forms.ToolStripMenuItem mnEmailSignature;
        private System.Windows.Forms.RichTextBox rtEmailBody;
        private System.Windows.Forms.CheckBox chEmail;
        private System.Windows.Forms.CheckBox chWhatsApp;
        private System.Windows.Forms.CheckBox chSMS;
        private System.Windows.Forms.RichTextBox rtSmsBody;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chImage;
        private System.Windows.Forms.CheckBox chSignature;
        private System.Windows.Forms.ComboBox cbEmailName;
    }
}