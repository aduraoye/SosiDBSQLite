using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite; 

namespace SosiDB
{
    public partial class FmEmailPad : Form
    {
        FmLogin fmLogin;
        OpenFileDialog UnsentEmail;
        List<string> ltAttachmentList;
        bool diffEmail, isNewEmail = false;
        private int AttachmentMaxNo = 10, tscbItemNo = 0, AttachmentNo;
        private string FName, LName, Title; 
        private string Schedule, strDetail, strNumber, EmailMessage, ImgFilename;
        private string DataRangex, RCCGYearx, ReportFileNamex, RemitReportx, OtherReportFileNamex, ReportEmailTox, strChurchAcronym;

        private string strRegId = "", strType = "", strTitle = "", strFName = "", strMName = "", strLName = "", strPara1 = "", strPara2 = "", strPara3 = "", DutyTime, DutyHeader, strMsgType;
        private string strAttendees = "", strDept = "", strService = "", strSex = "", strMarital = "", strAgeGroup = "", strResidency = "";
//********************* Start methods ****************************
        public FmEmailPad()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmLogin.niBackground.Visible = false;
            strChurchAcronym = fmLogin.strChurchAcronym;
        }
        private void FmEmailPad_Load(object sender, EventArgs e)
        {
           ltAttachmentList = new List<string> ();
           chEmail.Checked = true; 
           chSMS.Checked = false; chSMS_CheckedChanged(sender, e);
           ImgFilename = fmLogin.NoneNull;
           LoadArrayCombo();
           if (isNewEmail == true)EmailNewMessage();
        }
        private void LoadArrayCombo()
        {
            cbEmailName.Items.Clear();
            for (int i = 1; i <= fmLogin.ArrLeng; i++)
            {
                string[] cbArr = fmLogin.EmailDetailsArray[i].Split(new[] { fmLogin.Pass }, StringSplitOptions.None);//cbArr[0]
                cbEmailName.Items.Add(cbArr[0]);
            }
            cbEmailName.SelectedIndex = 0;
        }
                
        private void tsbEmailCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tssbEmailAttachment_ButtonClick(object sender, EventArgs e)
        {
            this.openFileDialog1.Title = "Load Attachment Files";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tscbItemNo = tscbAttachmentList.Items.Count;
                if (openFileDialog1.FileNames.Length + tscbItemNo <= AttachmentMaxNo) AttachmentNo = openFileDialog1.FileNames.Length + tscbItemNo;
                else { AttachmentNo = AttachmentMaxNo; MessageBox.Show("You can only send 10 attachments per e-mail!"); }
                for (int i = tscbItemNo; i <= AttachmentNo - 1; i++)
                {
                    tscbAttachmentList.Items.Add(Path.GetFileName(openFileDialog1.FileNames[i - tscbItemNo]));
                    ltAttachmentList.Add(openFileDialog1.FileNames[i - tscbItemNo]);
                }
                tscbAttachmentList.Text = tscbAttachmentList.Items.Count.ToString() + " Attachment(s)";
            }
            else return;
        }
        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tscbAttachmentList.Items.Clear();
            ltAttachmentList.Clear();// YesNo = false;
            tscbAttachmentList.Text = tscbAttachmentList.Items.Count.ToString() + " Attachment(s)";
        }
        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tscbAttachmentList.SelectedIndex <= 0) MessageBox.Show("You have to Select an attachment you want deleted.");
            else
            {
                int Seld = tscbAttachmentList.SelectedIndex;
                tscbAttachmentList.Items.RemoveAt(Seld);
                ltAttachmentList.RemoveAt(Seld);
                AttachmentNo = AttachmentNo - 1;
                tscbAttachmentList.Text = tscbAttachmentList.Items.Count.ToString() +  " Attachment(s)";
            }
        }

        private void tsbEmailSend_Click(object sender, EventArgs e)
        {
            if (chImage.Checked == true)
            {
                if (ImgFilename == fmLogin.NoneNull)
                {
                    DialogResult ImgInsert = MessageBox.Show("Would you like to insert image with your email(s)?", "Insert Image", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (ImgInsert == DialogResult.Yes) return;
                }
            }

            ReplaceString();
            tsbEmailSend.Enabled = false; 
            if (txEmailCC.Text == "E-mail Address") txEmailCC.Text = "";
            if (diffEmail == true)
            {
                if (chEmail.Checked == true) fmLogin.SendEmailDt((cbEmailName.SelectedIndex + 1).ToString(), txEmailTo.Text, txEmailCC.Text, txEmailSubject.Text, rtEmailBody.Text,
                    Schedule, ltAttachmentList, ImgFilename, strTitle, strFName, strMName, strLName, strPara1, strPara2, strPara3);
                if (chSMS.Checked == true) fmLogin.SendSMSDt(txMPhone.Text, rtSmsBody.Text, Schedule, strTitle, strFName,
                    strMName, strLName, strPara1, strPara2, strPara3, fmLogin.InternetPhoneSMS); //SMS
            }
            else
            {
                if (chEmail.Checked == true) fmLogin.SendEmailSt((cbEmailName.SelectedIndex + 1).ToString(), txEmailTo.Text, txEmailCC.Text, txEmailSubject.Text, rtEmailBody.Text,
                        Schedule, ltAttachmentList, ImgFilename);
                if (chSMS.Checked == true) fmLogin.SendSMSSt(txMPhone.Text, rtSmsBody.Text, Schedule, fmLogin.InternetPhoneSMS); //SMS                        
            }
            tsbEmailSend.Enabled = true;
            DialogResult MsgDigEPadClose = MessageBox.Show("Messages successfully sent ...\nWould like to close the EmailPad window?", "Email status", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (MsgDigEPadClose == DialogResult.No) return;
            else this.Close();
            tslbEmail.Text = "Messages successfully sent...";
        }       
       
        private void btEmailCC_Click_1(object sender, EventArgs e)
        {
            if (txEmailCC.Enabled == false) txEmailCC.Enabled = true;
            else txEmailCC.Enabled = false;
        }
        public void  EditMessage()
        {
            this.Size = new System.Drawing.Size(625, 470);//625, 560
            this.gb2.Location = new System.Drawing.Point(2, 0);
            tsbtSaveMessage.Visible = true;
            tsbEmailSend.Visible = false;
            tssbEmailMessagx.DropDownButtonWidth = 30;
            this.Text = fmLogin.AppName + "[Email Pad] - Edit Email&SMS Msg";
            this.ShowDialog();
        }
        private void tssbEmailMessagx_ButtonClick(object sender, EventArgs e)
        {
            if (tsbEmailSend.Visible == true)
            {
                if (EmailMessage == "EmailErrorMsg") ErrorMsgResend();
                else
                {
                    if (EmailMessage == "1stTimer") F1stTimerMessage();                  
                    else if (EmailMessage == "User") LoginUserMessage();
                    else if (EmailMessage == "Report") ReportMessage();
                    else if (EmailMessage == "EmailSMS") EmailSMSMessage();
                    else if (EmailMessage == "EmailSMSPlus") EmailSMSPlusMessage();
                    else if (EmailMessage == "EmailSMSAll") EmailSMSAllMessage();
                    else if (EmailMessage == "EmailRegMembUpdate") EmailRegMembUpdateMsgEmail();
                    else if (EmailMessage == "Roster") RosterEmail();
                    else if (EmailMessage == "Reminder") ReminderEmail();
                    else { };//EmailMessage == "New" //else if (EmailMessage == "Offering") OfferingMessage();
                    SmsMessage();
                }
                tsbEmailSend.Enabled = true;
            }
        }
        private void SmsMessage()
        {
            if (txMPhone.Enabled == true)
            {
                if (EmailMessage == "1stTimer") F1stTimerSms();
                else if (EmailMessage == "EmailSMS") EmailSMSSmsMessag();
                else if (EmailMessage == "EmailSMSPlus") EmailSMSPlusSmsMessag();
                else if (EmailMessage == "EmailSMSAll") EmailSMSAllSmsMessag();
                else if (EmailMessage == "Roster") RosterSms();
                else if (EmailMessage == "Reminder") { }
                else { txMPhone.Enabled = false; SMSPad(); }
            }
        }
        private void ReplaceString()
        {
            if (EmailMessage == "Roster") RosterReplace();
        }
        private void SMSPad()
        {
            if(chSMS.Checked == false)
            {
                gb2.Location = new System.Drawing.Point(2, 27);
                gb2.Size = new System.Drawing.Size(600, 402);
                rtEmailBody.Size = new System.Drawing.Size(583, 330);
                txMPhone.Enabled = false; //rtSmsBody.Visible = true;
            }
            else
            {
                gb2.Location = new System.Drawing.Point(2, 103);
                gb2.Size = new System.Drawing.Size(600, 326);
                rtEmailBody.Size = new System.Drawing.Size(583, 254);
                txMPhone.Enabled = true; //rtSmsBody.Visible = false; 
                txMPhone.Text = "MPhone Number";               
            }
        }
        private void chSMS_CheckedChanged(object sender, EventArgs e)
        {
            SMSPad();
        } 
        //+++++++++++++++++++++++++++++++++ Email Operations ===========================
        public void EmailNew()
        {  
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;   
            tsbEmailSend.Enabled = true;
            isNewEmail = true;//txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            EmailMessage = "New";
            this.Text = fmLogin.AppName + "[Email Pad]New -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void EmailNewMessage()
        {
            tssbEmailMessagx.Visible = false;
            diffEmail = false; chEmail.Checked = true; //chWhatsApp.Checked = true;                
            txEmailTo.Text = "Email Address";
            txEmailSubject.Text = "Email subject";
            rtEmailBody.Text = "*** Welcome to SosiDB E-mailPad.**** \n\n***Type your email message here.***";
            Schedule = "";
        }

        public void EmailErrorMsg()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "EmailErrorMsg";
            this.Text = fmLogin.AppName + "[Email Pad]Error -" + strChurchAcronym;           
            this.ShowDialog();
        }
        private void ErrorMsgResend()
        {
            try
            {
                chSMS.Checked = true; chEmail.Checked = true; chWhatsApp.Checked = true; //cbEmailSMS.Enabled = false;
                btEmailTo.Enabled = false; btEmailCC.Enabled = false; txEmailSubject.Enabled = false; 
                txMPhone.Enabled = false;  rtEmailBody.ReadOnly = true; rtSmsBody.ReadOnly = true;
                tscbAttachmentList.Enabled = false; tssbEmailAttachment.Enabled = false;

                UnsentEmail = new OpenFileDialog();
                UnsentEmail.Filter = "Unsent Email|*.err";
                UnsentEmail.Title = "Load Unsent Email File";
                UnsentEmail.InitialDirectory = fmLogin.PathErrorMsg;
                if (UnsentEmail.ShowDialog() == DialogResult.OK) { }
                else return;

                string subrow = "**";
                FileStream fs = new FileStream(UnsentEmail.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                sr.ReadLine();//Filename
                diffEmail = bool.Parse(sr.ReadLine());
                chSMS.Checked = bool.Parse(sr.ReadLine()); //cbEmailSMS.SelectedIndex = int.Parse(sr.ReadLine());
                Schedule = sr.ReadLine();
                sr.ReadLine(); //"***SMS***"
                txMPhone.Text = sr.ReadLine();
                rtSmsBody.Text = sr.ReadLine();
                sr.ReadLine(); //"***Email***"
                txEmailTo.Text = sr.ReadLine();
                txEmailCC.Text = sr.ReadLine();
                txEmailSubject.Text = sr.ReadLine();
                sr.ReadLine();//"***Email Body Start***"
                rtEmailBody.Clear();
                while (subrow != "***Email Body End***")
                {
                    subrow = sr.ReadLine();
                    if (subrow == "***Email Body End***") break;
                    rtEmailBody.AppendText("\n" + subrow);
                }               
                fs.Close(); sr.Close();
                System.IO.File.Delete(UnsentEmail.FileName);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        
        public void EmailLoginUser()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            EmailMessage = "User";
            this.Text = fmLogin.AppName + "[Email Pad]Login User -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void LoginUserMessage()
        {
            try
            {
                diffEmail = false; chEmail.Checked = true; //chWhatsApp.Checked = true;
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                Title = sr.ReadLine();                
                FName = sr.ReadLine();
                LName = sr.ReadLine();
                string UserName = sr.ReadLine();
                txEmailTo.Text = sr.ReadLine();
                txEmailSubject.Text = "Account creation on SosiDB Software of " + fmLogin.strChurchAcronym + " Church";

                strDetail = "Beloved " + Title + " " + FName + " " + LName + ","
                       + "\n\n"
                       + " A user account has been created for you on SosiDB Software of " + fmLogin.strChurchAcronym 
                       + ". Your usernme is " + UserName + "."
                       + "\n\nPlease, send a reply email if you did not authorize the creation or do not want the account."
                       + "\n\n"
                       + "May the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                fs.Close(); sr.Close();
                rtEmailBody.Text = strDetail;
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        public void EmailReport(string DataRange, string RCCGYear, string ReportFileName, string RemitReport, 
                                string OtherReportFileName, string ReportEmailTo, string ReportBody)
       {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            EmailMessage = "Report";
            DataRangex = DataRange;
            RCCGYearx = RCCGYear;
            ReportFileNamex = ReportFileName;
            RemitReportx = RemitReport;
            OtherReportFileNamex = OtherReportFileName;
            ReportEmailTox = ReportEmailTo;
            strDetail = ReportBody;
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad] - Region Report";
            this.ShowDialog();
        }
        private void ReportMessage()
        {
            tscbAttachmentList.Items.Add(Path.GetFileName(ReportFileNamex)); ltAttachmentList.Add(ReportFileNamex);
            tscbAttachmentList.Items.Add(Path.GetFileName(RemitReportx)); ltAttachmentList.Add(RemitReportx);
            tscbAttachmentList.Items.Add(Path.GetFileName(OtherReportFileNamex)); ltAttachmentList.Add(OtherReportFileNamex);           
            tscbAttachmentList.Text = tscbAttachmentList.Items.Count.ToString() + " Attachment(s)";

            try
            {
                chEmail.Checked = true; // chWhatsApp.Checked = true;
                diffEmail = false;
                txEmailTo.Text = ReportEmailTox; 
                txEmailSubject.Text = DataRangex + " Report";
                rtEmailBody.Text = strDetail;
                Schedule = "";
                cbEmailName.SelectedIndex = 3;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
                
        public void Email1stTimer()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;  tsbEmailSend.Enabled = false;
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            EmailMessage = "1stTimer";
            this.Text = fmLogin.AppName + "[Email Pad]First Timer -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void F1stTimerMessage()
        {
            try
            {
                chSMS.Checked = true; chEmail.Checked = true; chWhatsApp.Checked = true; //cbEmailSMS.SelectedIndex = 2;
                diffEmail = true;
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                strType = sr.ReadLine();
                strTitle = sr.ReadLine();
                strFName = sr.ReadLine();
                strMName = sr.ReadLine();
                strLName = sr.ReadLine();
                strNumber = sr.ReadLine();
                txEmailTo.Text = sr.ReadLine();
                fs.Close(); sr.Close();

                FileStream fsx = new FileStream(fmLogin.PathMsg + strType + "EmailMessage.msgx", FileMode.Open);
                StreamReader srx = new StreamReader(fsx);
                txEmailSubject.Text = srx.ReadLine();
                srx.ReadLine();
                strDetail = srx.ReadToEnd();
                fsx.Close(); srx.Close();
                rtEmailBody.Text = strDetail;
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void F1stTimerSms()
        {
            txMPhone.Text = strNumber;
            rtSmsBody.LoadFile(fmLogin.PathMsg + strType + "SMSMessage.msgx", RichTextBoxStreamType.PlainText);            
        }
             
        public void EmailSMS()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "EmailSMS";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad]Email&SMS -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void EmailSMSMessage()
        {
            try
            {
                chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true; 
                diffEmail = false;
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);                
                txEmailSubject.Text = sr.ReadLine();
                txEmailTo.Text = sr.ReadLine();
                strNumber = sr.ReadLine();//Mobile Phone

                strDetail = "Dear brethren,"
                       + "\n"
                       + "\n The Redeemed Christain Chuch of God (RCCG), "
                       + "Fountain of Life Parish (FOLP), Canberra. Please, send a reply email if there is any mistake.";
                fs.Close(); sr.Close();                 
                rtEmailBody.Text = strDetail;
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }           
        }
        private void EmailSMSSmsMessag()
        {
            txMPhone.Text = strNumber;
            string Messag =  "Dear brethren, We wish you happy birthday and many happy returns on your birthday."
                           + "May the Almighty God favour you and provide for all your needs, Amen. RCCG Church";
            rtSmsBody.Text = Messag;
        }

        public void EmailSMSPlus()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "EmailSMSPlus";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad]Email&SMS Plus -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void EmailSMSPlusMessage()
        {
            try
            {
                diffEmail = true; chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true; 
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txEmailSubject.Text = sr.ReadLine();
                string strRead = sr.ReadLine();
                char[] sep = new char[] { '?' }; string[] strSplit;
                strSplit = strRead.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                strTitle = strSplit[0]; /*Title*/ strFName = strSplit[1]; /*First name*/
                strMName = strSplit[2]; /*Middle name*/ strLName = strSplit[3]; // Last Name
                txEmailTo.Text = strSplit[4]; /*Email*/ txMPhone.Text = strSplit[5]; //Mobile Phone
                strPara1 = strSplit[6]; /*Birthdate*/ strPara2 = strSplit[7]; /*Address*/
                strPara3 = strSplit[8]; /*EmailSex & marital status*/
                strDetail = "Dear All,"
                        + "\n Title: #Title."
                        + "\n First Name: #FName."
                        + "\n Last Name: #LName."
                        + "\n Middle Name: #MName."
                        + "\n Birthdate: #Para1."
                        + "\n Adress: #Para2."
                        + "\n #Para3."
                        + "\n\n The Redeemed Christian Church of God (RCCG), "
                        + "Fountain of Life Parish (FOLP), Canberra. Please, send a reply email if there is any mistake.";
                rtEmailBody.Text = strDetail;
                fs.Close(); sr.Close();
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void EmailSMSPlusSmsMessag()
        {
            rtSmsBody.Text = "Dear Title: #Title; First Name: #FName; Last Name: #LName; Para1: #Para1; Para2: #Para2. RCCG Church";
        }
        
        public void EmailRegMembUpdate()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "EmailRegMembUpdate";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad]RegMembUpdate -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void EmailRegMembUpdateMsgEmail()
        {
            try
            {
                diffEmail = true; chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true; 
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txEmailSubject.Text = sr.ReadLine();
                string strRead = sr.ReadLine();
                char[] sep = new char[] { '?' }; string[] strSplit;
                strSplit = strRead.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                strPara3 = strSplit[0]; /*RegId*/ strTitle = strSplit[1]; /*Title*/
                strFName = strSplit[2]; /*First & Last Name*/ txEmailTo.Text = strSplit[3]; /*Email*/ 
                txMPhone.Text = strSplit[4]; /*Mobile Phone */ strPara1 = strSplit[5]; /*Birthdate*/ 
                strPara2 = strSplit[6]; /*Address*/ strMName = strSplit[7]; /*Sex*/
                strLName = strSplit[4]; // Mobile Phone  //strAttendees = strSplit[8]; /*Attendees*/ 
                        
                strDetail = "Dearly Beloved,"
                       + "\nCalvary greetings in Jesus' name."
                       + "\nFind below are your details in the church database. Please, send a reply email if you find any error."
                       + "\n\n Registration ID(RegId): #Para3" 
                       + "\n Title: #Title"
                       + "\n Name: #FName"
                       + "\n Mobile No: #LName"
                       + "\n Birth Date: #Para1"
                       + "\n Address: #Para2"
                       + "\n #MName"

                       + "\n\n<b>N.B.:</b> If you are interested in getting an account statement of your tithe to process your tax return, please write your RegId as part of your reference for online tithe payment "
                       + "or on a piece of paper which should be put in your tithe envelope when you pay your tithe in cash during any of our services.";
                rtEmailBody.Text = strDetail;
                fs.Close(); sr.Close();
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }           
        }
                      
        public void EmailSMSAll()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "EmailSMSAll";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.ShowDialog();
        }
        private void EmailSMSAllMessage()
        {
            try
            {
                diffEmail = true; chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true; 
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txEmailSubject.Text = sr.ReadLine();
                string strRead = sr.ReadLine();
                char[] sep = new char[] { '?' }; string[] strSplit;
                strSplit = strRead.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                strRegId = strSplit[0]; /*RegId*/ strTitle = strSplit[1]; /*Title*/ 
                strFName = strSplit[2]; /*First name*/ strLName = strSplit[3]; // Last Name
                txEmailTo.Text = strSplit[4]; /*Email*/ txMPhone.Text = strSplit[5]; //Mobile Phone
                strPara1 = strSplit[6]; /*Birthdate*/ strPara2 = strSplit[7]; /*Address*/
                strAttendees = strSplit[8]; /*Email*/ strDept = strSplit[9]; /*Dept*/
                strService = strSplit[10]; /*Email*/ strSex = strSplit[11]; /*Email*/
                strMarital = strSplit[12]; /*Email*/ strAgeGroup = strSplit[13]; /*Email*/
                strResidency = strSplit[14]; /*Email*/

                strMName = "-"; /*Middle name*/

                strDetail = "Dear All,"
                       + "\nCalvary greetings in Jesus name."
                       + "\nAs I announced on Sunady, below are your details in the church database. Send back a reply email for any correction you notice."
                       + "\n\n Registration Identity(RegId): " + strSplit[0]
                       + "\n Title: " + strSplit[1]
                       + "\n First Name: " + strSplit[2]
                       + "\n Last  Name: " + strSplit[3]
                       + "\n Email: " + strSplit[4]
                       + "\n Mobile Phone: " + strSplit[5]
                       + "\n Birthdate: " + strSplit[6]
                       + "\n Address: " + strSplit[7]
                       + "\n Sex: " + strSplit[11]

                       + "\n\nTake special note of your Registration Identity(RegId)and write it as part of your reference for online tithe payment "
                       + "or on a piece of paper which should be put in your tithe envelope when you pay your tithe in cash during any of our services. "
                       + "This would make it easy to prepare account statement for tax return if it would be needed." 
                       
                       + "\n\nI wish you a Merry Christmas and a prosperous New Year with floodgate of haeven in Jesus' name."; 
                rtEmailBody.Text = strDetail;
                fs.Close(); sr.Close();
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }                   
        }
        private void EmailSMSAllSmsMessag()
        {
            rtSmsBody.Text = "Dear " + Title + " " + FName + ", We wish you happy birthday and many happy returns on your birthday."
                           + "May the Almighty God favour you and provide for all your needs, Amen. RCCG Church";
        }

        public void RosterEmaSms()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "Roster";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad]Roster -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void RosterEmail()
        {
            try
            {
            chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true;       
            diffEmail = true;
            FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            strMsgType = sr.ReadLine();
            strTitle = sr.ReadLine();
            strFName = sr.ReadLine();
            strMName = sr.ReadLine();
            strLName = sr.ReadLine();
            strNumber = sr.ReadLine();
            txEmailTo.Text = sr.ReadLine();
            DutyTime = sr.ReadLine();
            DutyHeader = sr.ReadLine();
            string DutySubject = sr.ReadLine();
            fs.Close(); sr.Close();

            FileStream fsx = new FileStream(fmLogin.PathMsg + "RosterEmailMessage.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine().Replace("#DutyHeader", DutySubject);
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();                    
            rtEmailBody.Text = strDetail;
            Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void RosterSms()
        {
            txMPhone.Text = strNumber;
            rtSmsBody.LoadFile(fmLogin.PathMsg + "RosterSMSMessage.msgx", RichTextBoxStreamType.PlainText);            
        }
        private void RosterReplace()
        {
            rtEmailBody.Text = rtEmailBody.Text.Replace("#DutyTime", "#Para1");
            rtEmailBody.Text = rtEmailBody.Text.Replace("#DutyHeader", "#Para2");

            rtSmsBody.Text = rtSmsBody.Text.Replace("#DutyTime", "#Para1");
            rtSmsBody.Text = rtSmsBody.Text.Replace("#DutyHeader", "#Para2");

            strPara1 = DutyTime; strPara2 = DutyHeader;
        }

        public void ReminderEmaSms()
        {
            tsbtSaveMessage.Visible = false;
            tsbEmailSend.Visible = true;
            tsbEmailSend.Enabled = false;
            EmailMessage = "Reminder";
            txEmailTo.ReadOnly = true; txMPhone.ReadOnly = true;
            this.Text = fmLogin.AppName + "[Email Pad]Reminder -" + strChurchAcronym;
            this.ShowDialog();
        }
        private void ReminderEmail()
        {
            try
            {
                chEmail.Checked = true; //chWhatsApp.Checked = true; chSMS.Checked = true;    
                diffEmail = false; // true;
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txMPhone.Text = sr.ReadLine();
                txEmailTo.Text = sr.ReadLine();
                txEmailSubject.Text = sr.ReadLine();
                rtSmsBody.Text = sr.ReadLine();
                rtEmailBody.Text = sr.ReadToEnd();
                fs.Close(); sr.Close();               
                Schedule = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }        
        //+++++++++++++++++++++++++++++++++ Message editing Operations ===========================
        string SMSMsg, EmailMsg;
        private void mnBirthdayMessage_Click(object sender, EventArgs e)
        {
            rtSmsBody.LoadFile(fmLogin.PathMsg + "BirthdateSMSMessage.msgx", RichTextBoxStreamType.PlainText);
            FileStream fsx = new FileStream(fmLogin.PathMsg + "BirthdateEmailMessage.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            SMSMsg = fmLogin.PathMsg + "BirthdateSMSMessage.msgx";
            EmailMsg = fmLogin.PathMsg + "BirthdateEmailMessage.msgx";
            tscbAttachmentList.Text = "Birthdate Message";
        }
        private void mnRosterMessage_Click(object sender, EventArgs e)
        {
            rtSmsBody.LoadFile(fmLogin.PathMsg + "RosterSMSMessage.msgx", RichTextBoxStreamType.PlainText);

            FileStream fsx = new FileStream(fmLogin.PathMsg + "RosterEmailMessage.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            SMSMsg = fmLogin.PathMsg + "RosterSMSMessage.msgx";
            EmailMsg = fmLogin.PathMsg + "RosterEmailMessage.msgx";
            tscbAttachmentList.Text = "Roster Message";
        }
        private void mn1stTimerMessage_Click(object sender, EventArgs e)
        {
            rtSmsBody.LoadFile(fmLogin.PathMsg + "FirstTimerSMSMessage.msgx", RichTextBoxStreamType.PlainText);
            FileStream fsx = new FileStream(fmLogin.PathMsg + "FirstTimerEmailMessage.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            SMSMsg = fmLogin.PathMsg + "FirstTimerSMSMessage.msgx";
            EmailMsg = fmLogin.PathMsg + "FirstTimerEmailMessage.msgx";
            tscbAttachmentList.Text = "FirstTimer Message";
        }
        private void mnConvertMessage_Click(object sender, EventArgs e)
        {   
            rtSmsBody.LoadFile(fmLogin.PathMsg + "ConvertSMSMessage.msgx", RichTextBoxStreamType.PlainText);
            FileStream fsx = new FileStream(fmLogin.PathMsg + "ConvertEmailMessage.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            SMSMsg = fmLogin.PathMsg + "ConvertSMSMessage.msgx";
            EmailMsg = fmLogin.PathMsg + "ConvertEmailMessage.msgx";
            tscbAttachmentList.Text = "Convert Message";
        }
        private void mnAnyService_Click(object sender, EventArgs e)
        { 
            rtSmsBody.LoadFile(fmLogin.PathMsg + "ServiceReminderSMS.msgx", RichTextBoxStreamType.PlainText);
            FileStream fsx = new FileStream(fmLogin.PathMsg + "ServiceReminderMsg.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            SMSMsg = fmLogin.PathMsg + "ServiceReminderSMS.msgx";
            EmailMsg = fmLogin.PathMsg + "ServiceReminderMsg.msgx";
            tscbAttachmentList.Text = "Service Reminder Message";
        }
        private void MnSundayService_Click(object sender, EventArgs e)
        {    
            FileStream fsx;        SMSMsg = "Yes";
            if (System.IO.File.Exists(fmLogin.PathMsg + "SundayReminderSMS.msgx")) rtSmsBody.LoadFile(fmLogin.PathMsg + "SundayReminderSMS.msgx", RichTextBoxStreamType.PlainText);
            else rtSmsBody.LoadFile(fmLogin.PathMsg + "ServiceReminderSMS.msgx", RichTextBoxStreamType.PlainText);
            
            if (System.IO.File.Exists(fmLogin.PathMsg + "SundayReminderMsg.msgx")) fsx = new FileStream(fmLogin.PathMsg + "SundayReminderMsg.msgx", FileMode.Open);
            else fsx = new FileStream(fmLogin.PathMsg + "ServiceReminderMsg.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            EmailMsg = fmLogin.PathMsg + "SundayReminderMsg.msgx";
            tscbAttachmentList.Text = "Sunday Service Reminder Message";
        }
        private void mnFridayService_Click(object sender, EventArgs e)
        {
            FileStream fsx;     SMSMsg = "Yes";
            if (System.IO.File.Exists(fmLogin.PathMsg + "FridayReminderSMS.msgx")) rtSmsBody.LoadFile(fmLogin.PathMsg + "FridayReminderSMS.msgx", RichTextBoxStreamType.PlainText);
            else rtSmsBody.LoadFile(fmLogin.PathMsg + "ServiceReminderSMS.msgx", RichTextBoxStreamType.PlainText);
            
            if (System.IO.File.Exists(fmLogin.PathMsg + "FridayReminderMsg.msgx")) fsx = new FileStream(fmLogin.PathMsg + "FridayReminderMsg.msgx", FileMode.Open);
            else fsx = new FileStream(fmLogin.PathMsg + "ServiceReminderMsg.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            txEmailSubject.Text = srx.ReadLine();
            srx.ReadLine();
            strDetail = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            rtEmailBody.Text = strDetail;

            EmailMsg = fmLogin.PathMsg + "FridayReminderMsg.msgx";
            tscbAttachmentList.Text = "Friday Service Reminder Message";
        }
        private void mnEmailSignature_Click(object sender, EventArgs e)
        {
            SMSMsg = "";
            FileStream fsx = new FileStream(fmLogin.PathMsg + "EmailSignature.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            rtEmailBody.Text = srx.ReadToEnd();
            fsx.Close(); srx.Close();

            EmailMsg = fmLogin.PathMsg + "EmailSignature.msgx";
            tscbAttachmentList.Text = "Email Signature";
        }
        private void mnEmailFootnote_Click(object sender, EventArgs e)
        {
            SMSMsg = "";
            FileStream fsx = new FileStream(fmLogin.PathMsg + "EmailFootnote.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            rtEmailBody.Text = srx.ReadToEnd();
            fsx.Close(); srx.Close();
                       
            EmailMsg = fmLogin.PathMsg + "EmailFootnote.msgx";
            tscbAttachmentList.Text = "Email Footnote";
        }
        private void tsbtSaveMessage_Click(object sender, EventArgs e)
        {
            if (SMSMsg != "")
            {
                FileStream fBirthdateMsg = new FileStream(SMSMsg, FileMode.Create, FileAccess.Write);
                StreamWriter wBirthdateMsg = new StreamWriter(fBirthdateMsg);
                wBirthdateMsg.WriteLine(rtSmsBody.Text);
                wBirthdateMsg.Flush(); wBirthdateMsg.Close(); fBirthdateMsg.Close();
            }//*/

            FileStream feBirthdateMsg = new FileStream(EmailMsg, FileMode.Create, FileAccess.Write);
            StreamWriter weBirthdateMsg = new StreamWriter(feBirthdateMsg);
            weBirthdateMsg.WriteLine(txEmailSubject.Text);
            weBirthdateMsg.WriteLine("");
            weBirthdateMsg.WriteLine(rtEmailBody.Text);
            weBirthdateMsg.Flush(); weBirthdateMsg.Close(); feBirthdateMsg.Close();
        }
        
        private void mnInsertImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog Imagez = new OpenFileDialog();
            Imagez.Filter = "Pix Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff|All Files|*.*";
            Imagez.Title = "Load Image";
            if (Imagez.ShowDialog() == DialogResult.OK)
            {
                ImgFilename = Imagez.FileName;
            }
            else return;
        }
        private void mnSMSLength_Click(object sender, EventArgs e)
        {
            //if(rtSmsBody.MaxLength == 160) { rtSmsBody.MaxLength = 306; mnSMSLength.Text = "SMS Length -> 160";}
           // else if(rtSmsBody.MaxLength == 306) { rtSmsBody.MaxLength = 160; mnSMSLength.Text = "SMS Length -> 306";}
        }                
        // end
    }
}
 