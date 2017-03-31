using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Data.SQLite;
using System.Diagnostics;

namespace SosiDB
{
    public partial class FmInsatllLogin : Form
    {
        FmLogin fmLogin;
        string DropboxDir = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Dropbox";
        string FileExt = ".-None2", MainDir = "SosiDB_Installation", AxivDir = "SosiDB_Axiv_Installation";
        private string EtoFile, Dummy, PathMsg, SqlSetupFile = "Sql~SetupFile", nextReportDate = "NoDate"; 
        public string MainPath, strChurchAcronym, TempFile;
        int ArrLeng = 0;
        string[] DirArray = new string[3]; string[] EmailDetailsArray = new string[10];

        public FmInsatllLogin()
        {
            InitializeComponent();
            timer1.Start();
            fmLogin = new FmLogin();
            fmLogin.niBackground.Visible = false;
            if (System.IO.File.Exists(fmLogin.ChurchList)) Dummy = ReadChurchAcronym(fmLogin.ChurchList);
            TempFile = fmLogin.AppDir + "rootTemp.dbax";
        }
        private void FmInsatllLogin_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Insatll Setup]-" + txChurchAcronym.Text;
            if (cbChurchList.SelectedIndex == 0)
            {
                btCancel.Visible = false;
                txChurchAcronym.Enabled = true; txNewRootNo.Enabled = true;
                cbTime1.SelectedIndex = 3; cbTime2.SelectedIndex = 6; cbTime3.SelectedIndex = 10;

                DialogResult mgUpdate = MessageBox.Show("Do you want to duplicate a church database on this system?", "Database Duplication Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes)
                {
                    DuplicateDB();
                    btSaveDetails_Click(sender, e);
                }
            }
            else ReadEtoFile(fmLogin.EtoFile);

            cbDir.SelectedIndex = 0;
            DirArray[0] = MainDir;
            DirArray[1] = AxivDir;
            DirArray[2] = fmLogin.MainPath;

            if (nextReportDate != "NoDate")
            {
                dtpReportDate.Value = DateTime.Parse(nextReportDate); 
                WriteEtoFile();
                this.Close();
            }
        }
        public void LoadReport(string NextReportDate)
        {
            nextReportDate = NextReportDate;
            this.ShowDialog();
        }
        private void DuplicateDB() // Duplicate DB for a new installation on another PC
        {
            OpenFileDialog OpenReadEto = new OpenFileDialog();
            OpenReadEto.Filter = "Setup Duplicate File|*.sudf";
            OpenReadEto.Title = "Setup Duplicate File";
            if (OpenReadEto.ShowDialog() == DialogResult.OK)
            {
                ReadEtoFile(OpenReadEto.FileName);
                string OldUsername = Path.GetFileNameWithoutExtension(OpenReadEto.FileName);
                string NewUsername = Path.GetFileName(System.Environment.GetEnvironmentVariable("USERPROFILE"));
                MainDir = MainDir.Replace(OldUsername, NewUsername);
                AxivDir = AxivDir.Replace(OldUsername, NewUsername);
                SqlSetupFile = Path.GetDirectoryName(OpenReadEto.FileName) + "\\" + OldUsername + ".sudb";
            }
            else return;
        }
        private void ReadEtoFile(string EtoFilex)
        {
            if (File.Exists(EtoFilex))
            {
                FileStream fs = new FileStream(EtoFilex, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                txChurchName.Text = sr.ReadLine();
                txChurchAcronym.Text = sr.ReadLine();
                txNewRootNo.Text = sr.ReadLine();

                FileExt = sr.ReadLine();
                MainDir = sr.ReadLine();
                AxivDir = sr.ReadLine();
                txTimezoneOffset.Text = sr.ReadLine();
                chSchBkg.Checked = bool.Parse(sr.ReadLine());
                lbMasterPc.Text = sr.ReadLine();
                cbTime1.Text = sr.ReadLine();
                cbTime2.Text = sr.ReadLine();
                cbTime3.Text = sr.ReadLine();
                chBirthdayRun.Checked = bool.Parse(sr.ReadLine());
                if (sr.ReadLine() == rbPhone.Text) rbPhone.Checked = true; else rbInternet.Checked = true;

                dtpReportDate.Value = DateTime.Parse(sr.ReadLine()); 
                txTithePercent.Text = sr.ReadLine();
                txThanksPercent1.Text = sr.ReadLine();
                txThanksPercent2.Text = sr.ReadLine();
                txThanksPercent3.Text = sr.ReadLine();
                tx1stFruitPercent.Text = sr.ReadLine();
                txOtherPercent.Text = sr.ReadLine();

                txReportEmail1.Text = sr.ReadLine();
                txReportEmail2.Text = sr.ReadLine();
                txReportEmail3.Text = sr.ReadLine();

                txBccEmailAddr.Text = sr.ReadLine();
                ArrLeng = int.Parse(sr.ReadLine());  
                for (int i = 1; i <= ArrLeng; i++) EmailDetailsArray[i] = sr.ReadLine();               
                fs.Close(); sr.Close();

                LoadArrayCombo(); 
                strChurchAcronym = txChurchAcronym.Text;
                MainPath = fmLogin.AppDir + strChurchAcronym + "\\";
                EtoFile = MainPath + fmLogin.Eto;
            }
            else { MessageBox.Show("Corrupted! An important file is missing."); }
        }
        private void WriteChurchList(string ChurchListxx)
        {
            FileStream fList = new FileStream(ChurchListxx, FileMode.Create, FileAccess.Write);
            StreamWriter sList = new StreamWriter(fList);
            sList.WriteLine(cbChurchList.Items.Count);
            sList.WriteLine("****SOF****");
            for (int i = 0; i < cbChurchList.Items.Count; i++) sList.WriteLine(cbChurchList.Items[i].ToString());
            sList.WriteLine(txChurchAcronym.Text);
            sList.WriteLine("***EOF***");
            sList.Flush(); sList.Close(); fList.Close();
        }
        private string ReadChurchAcronym(string ChuList)
        {
            cbChurchList.Items.Clear();
            FileStream fs = new FileStream(ChuList, FileMode.Open);
            StreamReader sr = new StreamReader(fs); string subrow = "**"; 
            int AcronymIndex = int.Parse(sr.ReadLine());

            sr.ReadLine();//"****SOF****"
            while (subrow != "***EOF***") { subrow = sr.ReadLine(); cbChurchList.Items.Add(subrow); }
            string strAcronym = cbChurchList.Items[AcronymIndex].ToString();
            cbChurchList.SelectedIndex = AcronymIndex;
            cbChurchList.Items.RemoveAt(cbChurchList.Items.Count - 1);

            sr.Close(); fs.Close();
            return strAcronym;
        }       
        private void WriteUserGroup()
        {
            string UserGroupFile = fmLogin.AppDir + txChurchAcronym.Text + "\\" + "usergroup.dbax";
            FileStream fso = new FileStream(UserGroupFile, FileMode.Create, FileAccess.Write);
            StreamWriter swo = new StreamWriter(fso);
            swo.WriteLine(fmLogin.Rootgroup);
            swo.WriteLine("SetupLogin");
            swo.Flush(); swo.Close();
        }        
        private void btSaveDetails_Click(object sender, EventArgs e)
        {
            if (cbChurchList.SelectedIndex == 0)
            {
                strChurchAcronym = txChurchAcronym.Text;
                MainPath = fmLogin.AppDir + strChurchAcronym + "\\";
                Directory.CreateDirectory(MainPath);
                EtoFile = MainPath + fmLogin.Eto;
                WriteEtoFile();

                string database = txChurchAcronym.Text + ".dbx";
                string password = fmLogin.Pass + txChurchAcronym.Text + fmLogin.Pass;
                string strConReg = " Data Source=" + MainPath + database + ";Version=3;Password=" + password + ";";

                using (FileStream fsop = new FileStream(TempFile, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine(MainPath);
                    swop.WriteLine(txChurchAcronym.Text);
                    swop.WriteLine(fmLogin.TableReg);
                    swop.WriteLine(strConReg);
                    swop.WriteLine(MainPath + fmLogin.Edit);
                    swop.WriteLine(MainPath + fmLogin.Temp);
                    swop.Flush(); swop.Close();
                }

                FmRegNew fmRegNew;
                if (SqlSetupFile == "Sql~SetupFile")
                {
                    this.Hide();
                    fmRegNew = new FmRegNew(); fmRegNew.CreateLoginRootUser();
                    this.Show();
                    if (!Directory.Exists(MainDir)) MainDirectory();
                    if (!Directory.Exists(AxivDir)) AxivDirectory();
                }
                else
                {
                    File.Copy(SqlSetupFile, MainPath + strChurchAcronym + ".dbx", true);
                    if (!Directory.Exists(MainDir)) MainDirectory();
                    if (!Directory.Exists(AxivDir)) AxivDirectory();
                }
                
                WriteUserGroup();
                txNewRootNo.Text = "1";
                WriteChurchList(fmLogin.ChurchList);
                WriteEtoFile();
            }
            else WriteEtoFile();

            this.Close();
        }

        private void LoadArrayCombo()
        {
            cbEmailName.Items.Clear();
            cbEmailName.Items.Add("Create new Email"); 
            for (int i = 1; i <= ArrLeng; i++)
            {
                string[] cbArr = EmailDetailsArray[i].Split(new[]{fmLogin.Pass}, StringSplitOptions.None);//cbArr[0]
                cbEmailName.Items.Add(cbArr[0]);
            } 
            cbEmailName.SelectedIndex = 1;
        }
        private void cbEmailName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEmailName.SelectedIndex == 0) btCreateEditEmail.Text = "Create";
            else
            {
                btCreateEditEmail.Text = "Edit";
                string[] cbArr = EmailDetailsArray[cbEmailName.SelectedIndex].Split(new[] { fmLogin.Pass }, StringSplitOptions.None);
                txEmailName.Text = cbArr[0]; txEmailAddr.Text = cbArr[1]; txsmtpAddress.Text = cbArr[2];
                txPortNo.Text = cbArr[3]; txEmailPass.Text = cbArr[4]; rtEmailSignature.Text = cbArr[5];
            }
        }
        private void btCreateEditEmail_Click(object sender, EventArgs e)
        {
            if (btCreateEditEmail.Text == "Create" || btCreateEditEmail.Text == "Edit")
            {
                txEmailName.BringToFront();
                btCreateEditEmail.Text = "Apply";
            }
            else
            {
                if (cbEmailName.SelectedIndex == 0)
                {
                    ArrLeng += 1;
                    EmailDetailsArray[ArrLeng] = txEmailName.Text + fmLogin.Pass + txEmailAddr.Text + fmLogin.Pass + txsmtpAddress.Text
                        + fmLogin.Pass + txPortNo.Text + fmLogin.Pass + txEmailPass.Text + fmLogin.Pass + rtEmailSignature.Text;
                    cbEmailName.BringToFront();
                    cbEmailName.Items.Add(txEmailName.Text); cbEmailName.SelectedIndex = ArrLeng;
                }
                else
                {
                    EmailDetailsArray[cbEmailName.SelectedIndex] = txEmailName.Text + fmLogin.Pass + txEmailAddr.Text + fmLogin.Pass + txsmtpAddress.Text
                                    + fmLogin.Pass + txPortNo.Text + fmLogin.Pass + txEmailPass.Text + fmLogin.Pass + rtEmailSignature.Text;
                    cbEmailName.BringToFront();
                }
                btCreateEditEmail.Text = "Edit";
            }
        }
        private void WriteEtoFile()
        {
            try
            {
                FileStream fsop = new FileStream(EtoFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(txChurchName.Text);
                swop.WriteLine(txChurchAcronym.Text);
                swop.WriteLine(txNewRootNo.Text);
                swop.WriteLine(FileExt);
                swop.WriteLine(MainDir);
                swop.WriteLine(AxivDir); 
                swop.WriteLine(txTimezoneOffset.Text);
                swop.WriteLine(chSchBkg.Checked.ToString());
                swop.WriteLine(lbMasterPc.Text);
                swop.WriteLine(cbTime1.Text);
                swop.WriteLine(cbTime2.Text);
                swop.WriteLine(cbTime3.Text);
                swop.WriteLine(chBirthdayRun.Checked.ToString());
                if (rbPhone.Checked) swop.WriteLine(rbPhone.Text); else swop.WriteLine(rbInternet.Text);
                
                swop.WriteLine(dtpReportDate.Value.ToString("dd/MM/yyyy"));               
                swop.WriteLine(txTithePercent.Text);
                swop.WriteLine(txThanksPercent1.Text);
                swop.WriteLine(txThanksPercent2.Text);
                swop.WriteLine(txThanksPercent3.Text);
                swop.WriteLine(tx1stFruitPercent.Text);
                swop.WriteLine(txOtherPercent.Text);

                swop.WriteLine(txReportEmail1.Text);
                swop.WriteLine(txReportEmail2.Text);
                swop.WriteLine(txReportEmail3.Text);
                
                swop.WriteLine(txBccEmailAddr.Text);
                swop.WriteLine(ArrLeng);
                for (int i = 1; i <= ArrLeng; i++) swop.WriteLine(EmailDetailsArray[i]);
                
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void btLoadReportFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog PixOpen = new OpenFileDialog();
            PixOpen.Filter = "Report Files|*.xlsx;*.txt;*.dat|All Files|*.*";
            PixOpen.Title = "Load Report File";
            if (PixOpen.ShowDialog() == DialogResult.OK)
            {
                FileExt = Path.GetExtension(PixOpen.FileName);
                if (File.Exists(fmLogin.PathResource + " ReportDate" + FileExt)) File.Delete(fmLogin.PathResource + " ReportDate" + FileExt);
                File.Copy(PixOpen.FileName, fmLogin.PathResource + " ReportDate" + FileExt);
            }
            else return;
        }

        private void EmailMessageAtSetup()
        {
            FileStream fBirthdateMsg = new FileStream(PathMsg + "BirthdateEmailMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wBirthdateMsg = new StreamWriter(fBirthdateMsg);
            wBirthdateMsg.WriteLine("Happy Birthday from RCCG FOLP"); //EmailSubject
            wBirthdateMsg.WriteLine("");
            wBirthdateMsg.WriteLine("Dear #Title #FName,"
                       + "\n\n"
                       + "The Pastor and every member of The Redeemed Christian Church of God (RCCG), "
                       + "Fountain of Life Parish (FOLP), Canberra wish you happy birthday and "
                       + "many happy returns on your birthday #Birthmonth #Birthday."
                       + "\n\nMay the all sufficient God favour you and provide for all your needs, Amen.");
            wBirthdateMsg.Flush(); wBirthdateMsg.Close(); fBirthdateMsg.Close();

            FileStream fRosterMsg = new FileStream(PathMsg + "RosterEmailMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wRosterMsg = new StreamWriter(fRosterMsg);
            wRosterMsg.WriteLine("Service Roster from RCCG FOLP for #DutyHeader"); //EmailSubject
            wRosterMsg.WriteLine("");
            wRosterMsg.WriteLine("Dear #Title #FName,"
                   + "\n\n"
                   + "Find below the service duty details:"
                   + "\nService description and Date: #DutyHeader"
                   + "\nService duty and time: #DutyTime"
                   + "\n\nPlease, let me know if you would not be available for this responsibility."
                   + "\nMay the all sufficient God remember your labour of love in His kingdom and reward you accordingly in Jesus' name , Amen.");
            wRosterMsg.Flush(); wRosterMsg.Close(); fRosterMsg.Close();

            FileStream fServiceReminderMsg = new FileStream(PathMsg + "ServiceReminderMsg.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wServiceReminderMsg = new StreamWriter(fServiceReminderMsg);
            wServiceReminderMsg.WriteLine("RCCG FOLP Service Reminder: #DutyHeader"); //EmailSubject
            wServiceReminderMsg.WriteLine("");
            wServiceReminderMsg.WriteLine("Dearly Beloved,"
                   + "\n\n"
                   + "Just a friendly service reminder as follows:"
                   + "\nService description and Date: #DutyHeader"
                   + "\nService time: #ServiceTime"
                   + "\n\nGod bless you and remain rapturable as you attend in Jesus' name , Amen.");
            wServiceReminderMsg.Flush(); wServiceReminderMsg.Close(); fServiceReminderMsg.Close();

            FileStream fFirstTimerMsg = new FileStream(PathMsg + "FirstTimerEmailMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wFirstTimerMsg = new StreamWriter(fFirstTimerMsg);
            wFirstTimerMsg.WriteLine("Greetings from RCCG FOLP Church"); //EmailSubject
            wFirstTimerMsg.WriteLine("");
            wFirstTimerMsg.WriteLine("Dear #Title #FName #LName,"
                       + "\n\n"
                       + "The Pastor and every member of The Redeemed Christian Church of God (RCCG), "
                       + "Fountain of Life Parish (FOLP), Canberra are glad to have you around to "
                       + "worship the all sufficient God with us. We hope you had a nice time in the presence of God this morning."
                       + "\n\nMay the all sufficient God favour you and provide for all your needs, Amen.");
            wFirstTimerMsg.Flush(); wFirstTimerMsg.Close(); fFirstTimerMsg.Close();

            FileStream fConvertMsg = new FileStream(PathMsg + "ConvertEmailMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wConvertMsg = new StreamWriter(fConvertMsg);
            wConvertMsg.WriteLine("Greetings from RCCG FOLP Church"); //EmailSubject
            wConvertMsg.WriteLine("");
            wConvertMsg.WriteLine("Dear #Title #FName #LName,"
                       + "\n\n"
                       + "The Pastor and every member of The Redeemed Christian Church of God (RCCG), "
                       + "Fountain of Life Parish (FOLP), Canberra are glad to have you around to "
                       + "worship the all sufficient God with us. We hope you had a nice time in the presence of God this morning."
                       + "\n\nMay the all sufficient God favour you and provide for all your needs, Amen.");
            wConvertMsg.Flush(); wConvertMsg.Close(); fConvertMsg.Close();
        }
        private void SMSMessageAtSetup()
        {
            FileStream fBirthdateMsg = new FileStream(PathMsg + "BirthdateSMSMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wBirthdateMsg = new StreamWriter(fBirthdateMsg);
            wBirthdateMsg.WriteLine("Dear #Title #FName, We wish you happy birthday and many happy returns today. "
                           + "May the Almighty God favour you and provide for all your needs, Amen. RCCG Church");
            wBirthdateMsg.Flush(); wBirthdateMsg.Close(); fBirthdateMsg.Close();

            FileStream fRosterMsg = new FileStream(PathMsg + "RosterSMSMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wRosterMsg = new StreamWriter(fRosterMsg);
            wRosterMsg.WriteLine("Dear #Title #FName, Just a roster reminder. #DutyHeader for #DutyTime. God bless you.");
            wRosterMsg.Flush(); wRosterMsg.Close(); fRosterMsg.Close();

            FileStream fServiceReminderMsg = new FileStream(PathMsg + "ServiceReminderSMS.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wServiceReminderMsg = new StreamWriter(fServiceReminderMsg);
            wServiceReminderMsg.WriteLine("Dearly Beloved, Just a friendly service reminder. #DutyHeader for #ServiceTime. God bless you as you attend in Jesus' name , Amen.");
            wServiceReminderMsg.Flush(); wServiceReminderMsg.Close(); fServiceReminderMsg.Close();

            FileStream fFirstTimerMsg = new FileStream(PathMsg + "FirstTimerSMSMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wFirstTimerMsg = new StreamWriter(fFirstTimerMsg);
            wFirstTimerMsg.WriteLine("Dear #Title #FName, We are happy to have you in our midst today. Hope you are blessed. "
                                     + "May the Almighty God favour you and provide for all your needs, Amen. RCCG Church");
            wFirstTimerMsg.Flush(); wFirstTimerMsg.Close(); fFirstTimerMsg.Close();

            FileStream fConvertMsg = new FileStream(PathMsg + "ConvertSMSMessage.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wConvertMsg = new StreamWriter(fConvertMsg);
            wConvertMsg.WriteLine("Dear #Title #FName, We are happy to have you in our midst today. Hope you are blessed. "
                                     + "May the Almighty God favour you and provide for all your needs, Amen. RCCG Church");
            wConvertMsg.Flush(); wConvertMsg.Close(); fConvertMsg.Close();
        }
        private void EmailSignatureAtSetup()
        {
            FileStream fEmailSignature = new FileStream(PathMsg + "EmailSignature.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wEmailSignature = new StreamWriter(fEmailSignature);
            wEmailSignature.WriteLine(
                    "\n<br>\n<br><font color=\"green\">Yours in His service\n<br>Dr Adura Abiona (Pastor-In-Charge)</font>"
                //+ "\n<br><font color=\"green\">With Affectionate Love\n<br>The Abionas - Adura, Ranti & Iyin</font>"
                //+ "\n<br><font color=\"green\">Regards\n<br>Sis. Ranti</font>"                  
                  ); //*/
            wEmailSignature.Flush(); wEmailSignature.Close(); fEmailSignature.Close();
        }
        private void EmailFootnoteAtSetup()
        {
            FileStream fEmailFootnote = new FileStream(PathMsg + "EmailFootnote.msgx", FileMode.Create, FileAccess.Write);
            StreamWriter wEmailFootnote = new StreamWriter(fEmailFootnote);
            wEmailFootnote.WriteLine(
                    "\n<br><font color=\"brown\"><b>The Redeemed Christian Church of God " + txChurchName.Text + "</b></font>"
                  + "\n<br><font color=\"red\"><b>Church Programmes</b></font> @ Florey Primary School, Ratcliffe Crescent, Florey ACT 2615"
                  + "\n<br><b>Sunday Service:</b> Sunday School 9:45-10:45am; Sunday Worship Service 10:45am-12:30pm"
                  + "\n<br><b>Weekly Service:</b> Bible/Prayer Meeting 7:00-8:30pm Every Friday(Except the Last Friday)"
                  + "\n<br><b>Monthly Vigil:</b>  Every Last Friday 10:00pm-1:00am"
                  + "\n<br><b>Believers' Class:</b> 3:00-4:00pm Every Saturday"

                  + "\n<br><font color=\"red\"><b>Campus Fellowship: </b></font> <b>The Redeemed Christian Fellowship (RCF)</b>"
                  + "\n<br><b>Centre 1:</b> Multi-Faith Centre, University of Canberra(UC) 6:00-7:30pm Every Monday "

                  + "\n<br><font color=\"red\"><b>House Fellowship:</b></font> <b>Every First Sunday of the Month:</b> 2:00-3:00pm"
                  + "\n<br><b>Centre 1:</b> Unit 83, 15 John Cleland Crescent, Florey ACT 2615"

                  + "\n<br>\n<br><font color=\"red\"><b>Church bank details for payment of tithes, offerings and donations</b></font>"
                  + "\n<br><b>Bank: </b><font color=\"blue\">Commonwealth Bank of Australia</font>"
                  + "\n<br><b>BSB: </b><font color=\"blue\">062900</font> <b>Acct No: </b><font color=\"blue\">10703234</font>"
                  + "\n<br><b>Acct Name: </b><font color=\"blue\">The Redeemed Christian Church of God Incorp.</font>"

                  + "\n<br>\n<br><b>For more information check</b> <font color=\"blue\">http://www.rccgcanberra.org.au/</font> "
                  + "\n<br>\n<br><b>For prayer and counselling call <font color=\"blue\">0416874435</font>"
                  ); //*/
            wEmailFootnote.Flush(); wEmailFootnote.Close(); fEmailFootnote.Close();
        }
        private void MainDirectory()
        {
            if (Directory.Exists(DropboxDir))
            {
                MainDir = DropboxDir + fmLogin.AppFolder + txChurchAcronym.Text + "\\";
                if (!Directory.Exists(MainDir))Directory.CreateDirectory(MainDir);
            }
            else
            {
                FolderBrowserDialog MainFolder = new FolderBrowserDialog();
                MainFolder.ShowNewFolderButton = false;
                MainFolder.Description = "Main Directory";
                MainFolder.SelectedPath = System.Environment.GetEnvironmentVariable("USERPROFILE");
                if (MainFolder.ShowDialog() == DialogResult.OK)
                {
                    MainDir = MainFolder.SelectedPath + fmLogin.AppFolder + txChurchAcronym.Text + "\\";
                    if (!Directory.Exists(MainDir)) Directory.CreateDirectory(MainDir);
                }
                else return;
            }
            Directory.CreateDirectory(Directory.GetParent(Directory.GetParent(Directory.GetParent(MainDir).FullName).FullName).FullName + "\\PhoneSMS");
            Directory.CreateDirectory(MainDir + "Schedule");
            Directory.CreateDirectory(MainDir + "AppBackup");
            Directory.CreateDirectory(MainDir + "Resource");
            Directory.CreateDirectory(MainDir + "OutBox");
            File.Create(MainDir + "Resource\\" + "Deletedfilenames.dbax");
            
            SMSMessageAtSetup();
            EmailMessageAtSetup();
            EmailSignatureAtSetup();
            EmailFootnoteAtSetup();
        }
        private void AxivDirectory()
        {
            FolderBrowserDialog AxivFolder = new FolderBrowserDialog();
            AxivFolder.ShowNewFolderButton = false;
            AxivFolder.Description = "Achive Directory";
            AxivFolder.SelectedPath = Environment.SpecialFolder.MyDocuments.ToString();
            if (AxivFolder.ShowDialog() == DialogResult.OK)
            {
                AxivDir = AxivFolder.SelectedPath + fmLogin.AppFolder + txChurchAcronym.Text + "_Axiv\\";
                if (!Directory.Exists(AxivDir)) Directory.CreateDirectory(AxivDir);
            }
            else return;

            Directory.CreateDirectory(AxivDir + "Messagex"); PathMsg = MainDir + "Messagex\\";
            Directory.CreateDirectory(AxivDir + "ImagesPath");
            Directory.CreateDirectory(AxivDir + "WorkersDoc");
            Directory.CreateDirectory(AxivDir + "ReceiptFile");
            Directory.CreateDirectory(AxivDir + "ErrorMsg");
            Directory.CreateDirectory(AxivDir + "EmailCopy");
            if (!File.Exists(MainDir + "ImagesPath\\" + "Pix-0"))
            {
                File.Copy(fmLogin.PathExec + "Pix-0.bmp", AxivDir + "ImagesPath\\" + "Pix-0");
                File.Copy(fmLogin.PathExec + "Pix-0.bmp", AxivDir + "ImagesPath\\" + "Pix-1");
            }
        }
        private void btOpenDir_Click(object sender, EventArgs e)
        {
            string strDir = DirArray[cbDir.SelectedIndex];//.Substring(0, DirArray[cbDir.SelectedIndex].Length -1);
            if (Directory.Exists(strDir)) System.Diagnostics.Process.Start("explorer.exe", strDir);
            else MessageBox.Show(strDir + " doesn't exist.");
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int Count()
        {
            string strCount = "SELECT COUNT(*) FROM LoginInfo;";
            int intCount;
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix))
                {
                    intCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return intCount;
        }

        private void btUpdateRootNo_Click(object sender, EventArgs e)
        {
            string Oldquery = "UPDATE LoginInfo SET UserGroup = '" + fmLogin.Admingroup + "' WHERE RegId = '" + txOldRootNo.Text + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(Oldquery, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }

            string Newquery = "UPDATE LoginInfo SET UserGroup = '" + fmLogin.Rootgroup + "' WHERE RegId = '" + txNewRootNo.Text + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(Newquery, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void btMasterPc_Click(object sender, EventArgs e)
        {
            if (lbMasterPc.Text != Environment.MachineName) lbMasterPc.Text = Environment.MachineName;
            else lbMasterPc.Text = "NotMasterPc";
            WriteEtoFile();
        }
        //// End of methods 
    }
}
