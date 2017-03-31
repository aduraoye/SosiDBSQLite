
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
using System.Net;
using System.Net.Mail;
using System.Threading;
using IWshRuntimeLibrary;
using System.Diagnostics;

namespace SosiDB
{
    public partial class FmLogin : Form
    {
        /*
         First name has been changed to Preferred Name and Middle Name is now first name.
        */
        // ***************** Change these details for final software ***********************************
        private string strUsername = "", strPassword = "";
        public bool InstallLogin = false, EmailInternet = true, doWrite = false;
        private string schTime = "";//"201812121000";      
        // **************************************************************************************
        public string Company_Name = "AduraX", AppName = "SosiDB-1.2", Rootgroup = "RootAdmin", Admingroup = "Adminstrator", Pass = "*$#@~", NoneNull = "NoneNull";
        public string TableReg = "RegisterInfo", TableOff = "OfferingInfo", TableAcct = "AccountInfo", 
                      TableBL = "BankLodgeInfo", TableFC = "ForeignCurrencyInfo";          
        public string PathExec = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public string PathData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public string AppFolder, AppDir, MainPath, PathSchedule, PathMsg, PathImage, PathWorkersDoc, PathReceipt, PathOutBox,
                      PathErrorMsg, PathPhoneSMS, MainDir, AxivDir, SqlPath, PathBackup, PathResource, PathEmailCopy; //Other Directory Paths
        public string SetupRestart, EtoFile, SqlFile, RestartFile, BackupFile, TempFile, UserGroupFile, EditFile, OfferingFile, NoStartFile, UpdateFile; //File Names
        public string RootNo, Eto, Temp, Usergroup, Edit,  Title1 = "", FName1 = "";//File Names        
        public string NewChurch, ChurchList, strChurchName, strChurchAcronym, password, InternetPhoneSMS, ReportEmail1, ReportEmail2, ReportEmail3, ReportEmail4, ReportEmail5;
        public string database, strConReg, RegId, UserGroup, YesNo, strRuntime, queryz, MasterPc, strTime1, strTime2, strTime3;
        public string TithePercent, ThanksPercent1, ThanksPercent2, ThanksPercent3, F1stFruitPercent, OtherPercent, ReportDatex;
        private bool Sched, BirthdayRun, DoTimer = true, CheckUpdate = false;
        private string EmailCC, EmailTo, EmailSubject, EmailBody, StartFlagFile, RescueStartFile, DoSchFile, TimezoneOffset = "0", UpdateFlag = "CheckUpdate";
        DateTime FormTime = DateTime.Parse("1/1/2000"); int intLogin = 0;
        public string[] EmailDetailsArray = new string[10]; public int ArrLeng = 0;

        public FmLogin()
        {
            InitializeComponent();

            AppFolder = "\\" + Company_Name + "\\" + AppName + "\\";
            AppDir = PathData + AppFolder;
            NewChurch = "NewChurch";
            SetupRestart = AppDir + "restart.bat";
            NoStartFile =  AppDir + "Nostart.dbax";
            ChurchList = AppDir + "churchlist.dbax";
            
            Eto = "eto.dbax";
            Temp = "temp.dbax";
            Usergroup = "usergroup.dbax";
            Edit = "EditInfo.dbax";

            if (!System.IO.File.Exists(ChurchList)) Write1stChurchList(ChurchList);
            if (System.IO.File.Exists(ChurchList)) strChurchAcronym = ReadChurchAcronym(ChurchList);
              
            MainPath = AppDir + strChurchAcronym + "\\";  
            SqlPath = MainPath;
            EtoFile = MainPath + Eto;
            TempFile = MainPath + Temp;
            UserGroupFile = MainPath + Usergroup;
            EditFile = MainPath + Edit;
            OfferingFile = MainPath + "offering.dbax";
            UpdateFile = MainPath + "update.dbax";
            RestartFile = MainPath + "restart.bat";
            DoSchFile = MainPath + "DoSch.dbax";
            BackupFile = AppDir + "backupfile.dbax";

            if (System.IO.File.Exists(EtoFile)) OpenInstallLogin();
            database = strChurchAcronym + ".dbx";
            password = Pass + strChurchAcronym + Pass;
            strConReg = " Data Source=" + MainPath + database + ";Version=3;Password=" + password + ";";
            timer1.Start();
            if (Sched == true) niBackground.Visible = true; else niBackground.Visible = false;

            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }       
        private void OpenInstallLogin()
        {
            FileStream fs = new FileStream(EtoFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            strChurchName = sr.ReadLine();
            strChurchAcronym = sr.ReadLine();
            RootNo = sr.ReadLine();

            sr.ReadLine(); //FileExt = sr.ReadLine();
            MainDir = sr.ReadLine();
            AxivDir = sr.ReadLine();
            TimezoneOffset = sr.ReadLine();

            Sched = bool.Parse(sr.ReadLine());
            MasterPc = sr.ReadLine(); 
            strTime1 = sr.ReadLine();
            strTime2 = sr.ReadLine();
            strTime3 = sr.ReadLine();
            BirthdayRun = bool.Parse(sr.ReadLine());
            InternetPhoneSMS = sr.ReadLine();

            ReportDatex = sr.ReadLine();
            TithePercent = sr.ReadLine();
            ThanksPercent1 = sr.ReadLine();
            ThanksPercent2 = sr.ReadLine();
            ThanksPercent3 = sr.ReadLine();
            F1stFruitPercent = sr.ReadLine();
            OtherPercent = sr.ReadLine();

            ReportEmail1 = sr.ReadLine();
            ReportEmail2 = sr.ReadLine();
            ReportEmail3 = sr.ReadLine();

            emailBcc = sr.ReadLine();
            ArrLeng = int.Parse(sr.ReadLine()); 
            for (int i = 1; i <= ArrLeng; i++) EmailDetailsArray[i] = sr.ReadLine();
            fs.Close(); sr.Close();

            PreInitialization();
        }
        private void PreInitialization()
        {
            PathSchedule = MainDir + "Schedule\\";
            PathOutBox = MainDir + "OutBox\\";
            PathPhoneSMS = Directory.GetParent(Directory.GetParent(Directory.GetParent(MainDir).FullName).FullName).FullName + "\\PhoneSMS\\";
            PathBackup = MainDir + "AppBackup\\";
            PathResource = MainDir + "Resource\\";

            PathMsg = AxivDir + "Messagex\\";
            PathImage = AxivDir + "ImagesPath\\";
            PathWorkersDoc = AxivDir + "WorkersDoc\\";
            PathReceipt = AxivDir + "ReceiptFile\\";
            PathErrorMsg = AxivDir + "ErrorMsg\\";
            PathEmailCopy = AxivDir + "EmailCopy\\";

            SqlFile = MainPath + strChurchAcronym + ".dbx"; //"RCCGFOLP.dbx";
            StartFlagFile = PathResource + "startupdate.dbax";
            RescueStartFile = PathResource + Environment.MachineName + "-Rescuestart.dbax";
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (niBackground != null)
                {
                    niBackground.Visible = false;
                    niBackground.Icon = null;
                    niBackground.Dispose();
                    niBackground = null;
                }
            }
            catch { }
        }

        private void FmLogin_Load(object sender, EventArgs e)
        {
            this.Text = AppName + "[Login]-" + strChurchAcronym;
            rtSmsMsg.SendToBack(); 
            if (cbChurchList.SelectedIndex == 0)
            {
                strChurchName = NewChurch; lbParishName.Text = "(" + strChurchName + ") Database";
                txUsername.Text = "Admin"; txPassword.Text = "adminstration";
                InstallLogin = true; btLogin.Text = "Create DB";
            }
            else
            {
                    lbParishName.Text = "(" + strChurchName + ") Database";
                    if (MasterPc == Environment.MachineName) { strUsername = "Adura"; strPassword = "adura004"; }// MasterPc
                    txUsername.Text = strUsername; txPassword.Text = strPassword;
                    InstallLogin = false; btLogin.Text = "Login";
                    emailFootnote = EmailFootnote();

                    if (System.IO.File.Exists(SetupRestart))
                    {
                        if (UpdateFlag == "CheckUpdate") UpdateDB();
                        System.IO.File.Delete(NoStartFile);
                        System.IO.File.Delete(SetupRestart);
                        FormTime = DateTime.Now;//This time delays the App from starting to allow for proper restoring of the App files. 
                    }

                    if (System.IO.File.Exists(RestartFile))
                    {
                        btLogin.Enabled = false;
                        Restore();
                    }
            }
            doWrite = true;
        }
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (cbChurchList.SelectedIndex == 0)
            {
                WriteChurchList(ChurchList);
                this.Hide();
                FmInsatllLogin fmInsatllLogin = new FmInsatllLogin();
                fmInsatllLogin.ShowDialog();
                AppClose(NoStartFile, SetupRestart);
            }
            else
            {
                if (ValidateLogin(txUsername.Text, txPassword.Text) != "Wrong")
                {
                    ReportAlert();
                    if (RegId == RootNo) intLogin++;
                    if (System.IO.File.Exists(StartFlagFile) || System.IO.File.Exists(RescueStartFile) || intLogin == 7)
                    {
                        intLogin = 0;
                        if (RegId == RootNo) { }// Create close files to close the apps before use.
                        if (System.IO.File.Exists(StartFlagFile)) System.IO.File.Delete(StartFlagFile);
                        if (!System.IO.File.Exists(RescueStartFile)) System.IO.File.Create(RescueStartFile);
                        if (UpdateFlag == "CheckUpdate") UpdateDB();
                        SendBwMsg();

                        FmRecords fmRecords = new FmRecords();
                        fmRecords.Show(); this.Hide(); 
                        fmRecords.FormClosed += new FormClosedEventHandler(FmClosed);
                    }
                    else { MessageBox.Show("The App is being used!", "Under Usage", MessageBoxButtons.OK); CheckUpdate = true; }
                }
                else txValidation.Text = "Error! Username or password \nis incorrect. Try again.";
            }
        }
        
        #region ######## Restore & InputBox ########
        public void PreRestore(string SqlFilenamex, string NoStartUpdateFilex)
        {
            FileStream fsop = new FileStream(BackupFile, FileMode.Create, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            if (SqlFilenamex == "SqlAll")
            {
                OpenFileDialog RestoreDir = new OpenFileDialog();
                RestoreDir.InitialDirectory = System.Environment.SpecialFolder.Desktop.ToString();// fmLogin.MainDir + @"Backup";
                RestoreDir.RestoreDirectory = true;
                RestoreDir.Filter = "DB Main File|*.dbax";
                RestoreDir.Title = "Restore DB";

                if (RestoreDir.ShowDialog() == DialogResult.OK && Path.GetExtension(RestoreDir.FileName) == Path.GetExtension("Restore.dbax"))
                {
                    swop.WriteLine(SqlFilenamex);
                    swop.WriteLine(Path.GetDirectoryName(RestoreDir.FileName));
                }
                else return;
            }
            else if (SqlFilenamex == "SqlSave" || SqlFilenamex == "Sql~DB")
            {
                OpenFileDialog RestoreDir = new OpenFileDialog();
                RestoreDir.InitialDirectory = PathBackup;
                RestoreDir.Filter = "DB Main File|" + System.Environment.MachineName + "~*";
                RestoreDir.Title = "Restore DB";

                if (RestoreDir.ShowDialog() == DialogResult.OK)
                {
                    swop.WriteLine(SqlFilenamex);
                    swop.WriteLine(RestoreDir.FileName);
                }
                else return;
            }
            else
            {
                swop.WriteLine(SqlFilenamex);
                swop.WriteLine("dummy");
            }
            swop.Flush(); swop.Close();
            
            SavingUpdate("SqlPre", "Loading"); ;
            AppClose(NoStartUpdateFilex, RestartFile);
        }
        private void AppClose(string NoStartFilexx, string RestartFilexx)
        {
            //Creates NoStartFile that flags no creation of startupFile when the Appp closes for restoring DB         
            System.IO.File.Create(NoStartFilexx);
            // Creates the restart bat file when the App closes for restoring DB
            FileStream fBat = new FileStream(RestartFilexx, FileMode.Create, FileAccess.Write);
            StreamWriter sBat = new StreamWriter(fBat);
            sBat.WriteLine("sleep 20");
            sBat.WriteLine("start \"\" \"" + PathExec + "SosiDB.exe\"");
            sBat.Flush(); sBat.Close(); fBat.Close();
            Process.Start(RestartFilexx);

            Application.Exit();
        }
        private void Write1stChurchList(string ChurchListxx)
        {
            Directory.CreateDirectory(AppDir);
            FileStream fList = new FileStream(ChurchListxx, FileMode.Create, FileAccess.Write);
            StreamWriter sList = new StreamWriter(fList);
            sList.WriteLine("0");
            sList.WriteLine("****SOF****");
            sList.WriteLine(NewChurch);
            sList.WriteLine("***EOF***");
            sList.Flush(); sList.Close(); fList.Close();
        }
        private void WriteChurchList(string ChurchListxx)
        {
            FileStream fList = new FileStream(ChurchListxx, FileMode.Create, FileAccess.Write);
            StreamWriter sList = new StreamWriter(fList);
            sList.WriteLine(cbChurchList.SelectedIndex);
            sList.WriteLine("****SOF****");
            for (int i = 0; i < cbChurchList.Items.Count; i++) sList.WriteLine(cbChurchList.Items[i].ToString());
            sList.WriteLine("***EOF***");
            sList.Flush(); sList.Close(); fList.Close();
        }
        private void cbChurchList_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(doWrite == true) WriteChurchList(ChurchList);
        }
        private string ReadChurchAcronym(string ChuList)
        {
            cbChurchList.Items.Clear();
            FileStream fs = new FileStream(ChuList, FileMode.Open);
            StreamReader sr = new StreamReader(fs); string subrow = "**"; 
            int AcronymIndex = int.Parse(sr.ReadLine()); 
            sr.ReadLine();//"****SOF****"
            while (subrow != "***EOF***") { subrow = sr.ReadLine(); cbChurchList.Items.Add(subrow); }
            cbChurchList.Items.RemoveAt(cbChurchList.Items.Count - 1);

            string strAcronym = cbChurchList.Items[AcronymIndex].ToString();
            cbChurchList.SelectedIndex = AcronymIndex;

            sr.Close(); fs.Close();
            return strAcronym;
        }
        private static void DirCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }        
        private void UpdateDB()
        {
            DirectoryInfo apple = new DirectoryInfo(@PathBackup);
            int i = 0, j = 0, l = 0, Max = 0, Maxx = 0; long MaxValue = 0, MaxValuex = 0;
            foreach (var file in apple.GetFiles("*~*")) i++;
            char[] sep = new char[] { '~' }; string[] splitFile, Filename = new string[i];
            foreach (var file in apple.GetFiles("*~*"))
            {
                splitFile = file.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (System.Environment.MachineName != splitFile[0])
                {
                    Filename[j] = file.ToString();
                    if (long.Parse(splitFile[1]) > MaxValue) { MaxValue = long.Parse(splitFile[1]); Max = j; }
                    j++;
                }
                else
                {
                    if (long.Parse(splitFile[1]) > MaxValuex) { MaxValuex = long.Parse(splitFile[1]); Maxx = l; }
                    l++;
                }
            }

            if (MaxValue > MaxValuex) UpdateFlag = Filename[Max];
            else UpdateFlag = "AlreadyUpdated";
            WriteUpdateFile(UpdateFlag);
        }
        public void SavingUpdate(string extName, string LoadSave)
        {
            if (LoadSave == "Saving") System.IO.File.Copy(SqlFile, PathBackup + System.Environment.MachineName + DateTime.Now.ToString("~yyMMddHHmmss~") + extName, true);//Backup                                   
            System.IO.File.Copy(SqlFile, SqlPath + extName + DateTime.Now.ToString("~yyMMddHHmmss~") + LoadSave, true);//Backup
        }
        private void WriteUpdateFile(string UpdateFlagx)
        {
            try
            {
                FileStream fsop = new FileStream(UpdateFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(UpdateFlagx);
                swop.Flush(); swop.Close(); fsop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void Restore()
        {
            // Delete NoStartFile  after restoring DB. 
            //It is not needed bcos it wont allow StartupFile to be created which is needed for starting a new App session 
            System.IO.File.Delete(NoStartFile);

            FileStream fs = new FileStream(BackupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string SqlFilename = sr.ReadLine();
            string RestorePath = sr.ReadLine();
            fs.Close(); sr.Close();

            if (SqlFilename == "SqlAll")
            {
                System.IO.File.Copy(EtoFile, AppDir + Path.GetFileName(EtoFile), true);
                System.IO.File.Copy(EditFile, AppDir + Path.GetFileName(EditFile), true);
                Directory.Delete(MainPath, true);
                Directory.Delete(MainDir, true);
                MessageBox.Show("The process of restoring your DB is already initiated. Click OK to continue.", "SosiDB Inforamtion",MessageBoxButtons.OK,MessageBoxIcon.Information);
                DirCopy(RestorePath + "\\MainPath", MainPath, true); 
                MessageBox.Show("Copying the main execute path of your DB... \nClick OK to continue.", "SosiDB Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DirCopy(RestorePath + "\\MainDir", MainDir, true); 
                MessageBox.Show("Copying the main file directory of your DB... \nClick OK to continue.", "SosiDB Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);                                
                System.IO.File.Copy(AppDir + Path.GetFileName(EtoFile), EtoFile, true);
                System.IO.File.Copy(AppDir + Path.GetFileName(EditFile), EditFile, true);
                System.IO.File.Delete(AppDir + Path.GetFileName(EtoFile));
                System.IO.File.Delete(AppDir + Path.GetFileName(EditFile));
                MessageBox.Show("The process of restoring your DB is about to complete. Click OK to continue.", "SosiDB Inforamtion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (SqlFilename == "SqlSave") System.IO.File.Copy(RestorePath, MainPath + database, true);
            else if (SqlFilename == "Sql~DB") { System.IO.File.Copy(RestorePath, MainPath + database, true); ChangePassword("RCCGFOLP", 4);}
            else if (SqlFilename.Contains("Sql")) System.IO.File.Copy(SqlPath + SqlFilename, MainPath + database, true);
            else System.IO.File.Copy(PathBackup + SqlFilename, MainPath + database, true); //Automated updating the App   
            UpdateFlag = "AlreadyUpdated"; WriteUpdateFile(UpdateFlag);
            System.IO.File.Delete(RestartFile);//System.IO.File.Delete(BackupFile);

            FormTime = DateTime.Now;//This time delays the App from starting to allow for proper restoring of the App files.         
        }
        private void ChangePassword(string OldstrChurchAcronym, int k) //"RCCGFOLP";
        {
            string OldstrConDB = " Data Source=" + MainPath + database + ";Version=3;Password=" + Pass + OldstrChurchAcronym + Pass + ";";
            SQLiteConnection connection = new SQLiteConnection(OldstrConDB);
            connection.Open();
            connection.ChangePassword(password);
            connection.Close();

            string queryx = "DELETE FROM " + TableReg + " WHERE Id > '" + k.ToString() + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }//MessageBox.Show("All deleted.");
        }
        private void HideNSowForm()
        {
            this.Hide(); // The main form (login)
            if (System.IO.File.Exists(DoSchFile)){timerSch(); System.IO.File.Delete(DoSchFile); }
            else
            {
                FmRecords fmRecords = new FmRecords();
                fmRecords.Show();
                fmRecords.FormClosed += new FormClosedEventHandler(FmClosed);
            }
        }
        #endregion

        private void FmClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private void writeUserGroup()
        {
            FileStream fsop = new FileStream(UserGroupFile, FileMode.Create, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine(UserGroup); ;
            swop.WriteLine(RegId);
            swop.Flush(); swop.Close();
        }
        public string ValidateLogin(string username, string password)
        {
            int output;
            string query = "SELECT RegId, UserName, Password, UserGroup FROM LoginInfo WHERE UserName = '" + username + "' AND Password = '" + password + "';";
            if (InstallLogin == true)
            {
                RegId = "SetupLogin";
                FileStream fsop = new FileStream(EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ***************** Login by User RegId(" + RegId + ") *****************");
                swop.Flush(); swop.Close();
                return RegId;
            }
            else
            {
                DataTable dTable = new DataTable();
                using (SQLiteConnection conLogin = new SQLiteConnection(strConReg))
                {
                    SQLiteCommand cmdUserPass = new SQLiteCommand(query, conLogin);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    MyAdapter.SelectCommand = cmdUserPass;
                    output = MyAdapter.Fill(dTable);
                }

                if (output == 1)
                {
                    RegId = dTable.Rows[0][0].ToString();
                    UserGroup = dTable.Rows[0][3].ToString();
                    writeUserGroup();
                    FileStream fsop = new FileStream(EditFile, FileMode.Append, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ***************** Login by User RegId(" + RegId + ") **********************");
                    swop.Flush(); swop.Close();

                    return RegId;
                }
                else return "Wrong";
            }
        }
        private void FmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Deleting backup file that is more than 3 month old & creating Monthly Backups
            if (btLogin.Text == "Login")
            {
                if (System.IO.File.Exists(SqlPath + "Sql" + DateTime.Now.AddMonths(-3).ToString("MMM"))) System.IO.File.Delete(SqlPath + "Sql" + DateTime.Now.AddMonths(-3).ToString("MMM"));
                if (!System.IO.File.Exists(SqlPath + "Sql" + DateTime.Now.ToString("MMM"))) System.IO.File.Copy(SqlFile, SqlPath + "Sql" + DateTime.Now.ToString("MMM"), true);//Monthly Backup
                DelFile3dayB4();
            }

            if (!System.IO.File.Exists(NoStartFile))
            {
                if (this.Visible == true) e.Cancel = false;
                else
                { //OpenInstallLogin();
                    if (Sched == true)
                    {
                        DialogResult BkgClose = MessageBox.Show("Would you like the program to run in the background instead of closing?", "Background/Close", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (BkgClose == DialogResult.Yes) e.Cancel = true;
                        else e.Cancel = false;
                    }
                }
                if (!System.IO.File.Exists(StartFlagFile)) System.IO.File.Create(StartFlagFile);
                if (System.IO.File.Exists(RescueStartFile)) System.IO.File.Delete(RescueStartFile);
            }
        }       
        private void DelFile3dayB4()
        {
            DirectoryInfo apple = new DirectoryInfo(@PathBackup);
            char[] sep = new char[] { '~' }; string[] splitFile;
            long DaysB3 = long.Parse(DateTime.Now.AddDays(-3).ToString("yyMMddHHmmss"));
            foreach (var file in apple.GetFiles("*~*"))
            {
                splitFile = file.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (long.Parse(splitFile[1]) < DaysB3) System.IO.File.Delete(PathBackup + file.ToString());
            }
        }
        
        #region %%%%%%%%%%%%%%%%% Begin Email&SMS Scheduler %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        string DateTimeFormat = "yyyyMMdd-HH:mm:ss";   string DateFormat = "yyyyMMdd-";
        double MinAdd = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Checking if the App is being released by the other user for use after trying to use it. 
            // It means that the StartupFile is now available to allow the App to be used
            if (CheckUpdate == true && System.IO.File.Exists(StartFlagFile))// && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                CheckUpdate = false;
                MessageBox.Show("The App is ready for use!", "Ready for Use", MessageBoxButtons.OK);                
            }

            if (DateTime.Now.ToString("dd:MM:yy-HH:mm:ss") == FormTime.AddSeconds(1).ToString("dd:MM:yy-HH:mm:ss")) HideNSowForm();           
            lbTime.Text = DateTime.Now.ToString("HH:mm:ss");
            MinAdd = int.Parse(TimezoneOffset);
            if (Sched == true && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                if (DateTime.Now.AddMinutes(MinAdd).ToString(DateTimeFormat) == DateTime.Now.AddMinutes(MinAdd).ToString(DateFormat) + strTime1 + ":00"
                 || DateTime.Now.AddMinutes(MinAdd).ToString(DateTimeFormat) == DateTime.Now.AddMinutes(MinAdd).ToString(DateFormat) + strTime2 + ":00"
                 || DateTime.Now.AddMinutes(MinAdd).ToString(DateTimeFormat) == DateTime.Now.AddMinutes(MinAdd).ToString(DateFormat) + strTime3 + ":00")
                {
                    if (UpdateFlag == "CheckUpdate")
                    {
                        UpdateDB();
                        if (UpdateFlag == "CheckUpdate" || UpdateFlag == "AlreadyUpdated") { timerSch(); }
                        else
                        {
                            System.IO.File.Create(DoSchFile);//Create DoScheduleFile that flags timerSch() running   
                            PreRestore(UpdateFlag, NoStartFile);
                        }
                    }
                    else timerSch();
                }
            }

        }
        private void timerSch()
        {
            if (DateTime.Now.AddMinutes(MinAdd).ToString(DateTimeFormat) == DateTime.Now.AddMinutes(MinAdd).ToString(DateFormat) + strTime1 + ":00")
            {
                if (BirthdayRun == true) BirthdateSchedule();
                DelDayReminderFile();
                ReportSchedule();
            }
            RunSchedule();
        }
        private void RunSchedule()
        {
            DirectoryInfo apple = new DirectoryInfo(@PathSchedule);
            int i = 0, j = 0, k = 0; ;
            foreach (var file in apple.GetFiles("*.sch")) { i++; }

            char[] sep = new char[] { '-' }; string[] splitFile, Filename = new string[i], ArrSch = new string[i];
            foreach (var file in apple.GetFiles("*.sch"))
            {
                Filename[j] = Path.GetFileNameWithoutExtension(file.ToString());
                splitFile = Path.GetFileNameWithoutExtension(file.ToString()).Split(sep, StringSplitOptions.RemoveEmptyEntries);
                ArrSch[j] = splitFile[1];
                j++;
            }

            Array.Sort(ArrSch, Filename);
            long RunTime = long.Parse(DateTime.Now.AddMinutes(MinAdd).ToString("yyyyMMddHHmm"));
            if (i > 0)
            {
                while (long.Parse(ArrSch[k]) < RunTime)
                {
                    SchMessage(PathSchedule + Filename[k] + ".sch"); // Main function here
                    System.IO.File.Delete(PathSchedule + Filename[k] + ".sch");

                    FileStream fsop = new FileStream(PathResource + "Deletedfilenames.dbax", FileMode.Append);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine(Filename[k] + "\t\t" + DateTime.Now.ToString("ddMMyy-HHmmss"));
                    swop.Flush(); swop.Close(); fsop.Close();
                    k++;
                    if (k >= i) break;
                }
            }
            SchNotifyEmail();
        }
        private void SchMessage(string filenamex)
        {
            EmailBody = ""; 
            string subrow = "**", chek ="";
            FileStream fs = new FileStream(filenamex, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            chek = sr.ReadLine();
            if (chek == "Schedule")
            {
                EmailTo = sr.ReadLine();
                EmailCC = sr.ReadLine();
                EmailSubject = sr.ReadLine();
                sr.ReadLine();//"***Email Body Start***"
                while (subrow != "***Email Body End***")
                {
                    subrow = sr.ReadLine();
                    if (subrow == "***Email Body End***") break;
                    EmailBody = EmailBody + "\n" + subrow;
                }
                //EmailBody += EmailDefaultSignature();
                SendEmailSt("0", EmailTo, EmailCC, EmailSubject, EmailBody, "", null, NoneNull);
            }
            else if (chek == "Roster")
            {
                string strEmailSMS, DateES, HeaderCol, DutyCol, DutyHeader, ServiceTime; int Rowx;
                strEmailSMS = sr.ReadLine();
                DateES = sr.ReadLine();
                ServiceTime = sr.ReadLine();
                HeaderCol = sr.ReadLine();
                DutyCol = sr.ReadLine();
                DutyHeader = sr.ReadLine();
                Rowx =  int.Parse(sr.ReadLine());
                SelectRosterDB(strEmailSMS, DateES, HeaderCol, DutyCol, DutyHeader, ServiceTime, Rowx);
            }
            else { }
            fs.Close(); sr.Close();
        }
        private void SelectRosterDB(string strEmailSMS, string DateES, string HeaderCol, string DutyCol, string DutyHeader, string ServiceTime, int Rowx)
        {
            DataTable dTable = new DataTable();
            queryz = "SELECT GROUP_CONCAT(Colm" + HeaderCol + ", ';'), GROUP_CONCAT(Colm" + DutyCol + ",  ';'), GROUP_CONCAT(Colx" 
                                                + DutyCol + ",  '?') FROM RosterInfo WHERE Date = '" + DateES + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryz, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }

            string Title = "", FName = "", MName = "", LName = "", DutyTime = "", MphoneNo = "", DutyHeaderx = "", SMSMessage; EmailTo = "";
            char[] sep = new char[] { ';' }; char[] sepNamex = new char[] { ' ' }; char[] sepCon = new char[] { '?' };
            string[] splitName; string[] splitCon;

            DutyTime = dTable.Rows[0][0].ToString().Replace("\n", " "); //ProgramCol
            string[] splitNamex = dTable.Rows[0][1].ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string[] splitContact = dTable.Rows[0][2].ToString().Split(sepCon, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < Rowx; i++)
            {
                splitName = splitNamex[i].Split(sepNamex, StringSplitOptions.RemoveEmptyEntries);
                FName = FName + ";" + splitName[0];
                MName = MName + ";" + "None";
                LName = LName + ";" + splitName[1];

                splitCon = splitContact[i].Split(sep, StringSplitOptions.RemoveEmptyEntries);
                Title = Title + ";" + splitCon[0];
                EmailTo = EmailTo + ";" + splitCon[1];
                MphoneNo = MphoneNo + ";" + splitCon[2];
            
                DutyHeaderx = DutyHeaderx + ";" + DutyHeader.Replace("\n",""); 
            }

            Title = Title.Substring(1, Title.Length - 1);
            FName = FName.Substring(1, FName.Length - 1);
            LName = LName.Substring(1, LName.Length - 1);
            EmailTo = EmailTo.Substring(1, EmailTo.Length - 1);
            MphoneNo = MphoneNo.Substring(1, MphoneNo.Length - 1);
            DutyHeaderx = DutyHeaderx.Substring(1, DutyHeaderx.Length - 1);

            #region automated & manual roster msg
            if (DoTimer == true)//automated roster msg
            {
                if (strEmailSMS == "Email")
                {
                    FileStream fsx = new FileStream(PathMsg + "RosterEmailMessage.msgx", FileMode.Open);
                    StreamReader srx = new StreamReader(fsx);
                    EmailSubject = srx.ReadLine();
                    srx.ReadLine();
                    EmailBody = srx.ReadToEnd();
                    fsx.Close(); srx.Close();
                    EmailSubject = EmailSubject.Replace("#DutyHeader", DutyHeader);
                    EmailBody = EmailBody.Replace("#DutyTime", "#Para1");
                    EmailBody = EmailBody.Replace("#DutyHeader", "#Para2");
                    //EmailBody += EmailDefaultSignature();
                    SendEmailDt("5", EmailTo, "", EmailSubject, EmailBody, schTime, null, NoneNull, Title, FName, MName, LName, DutyTime, DutyHeaderx, "");
                }
                else //SMS & Service
                {
                    FileStream fsz = new FileStream(PathMsg + "RosterSMSMessage.msgx", FileMode.Open);
                    StreamReader srz = new StreamReader(fsz);
                    SMSMessage = srz.ReadToEnd();
                    fsz.Close(); srz.Close();
                    SMSMessage = SMSMessage.Replace("#DutyTime", "#Para1");
                    SMSMessage = SMSMessage.Replace("#DutyHeader", "#Para2");
                    rtSmsMsg.Text = SMSMessage;
                    SendSMSDt(MphoneNo, rtSmsMsg.Text, schTime, Title, FName, MName, LName, DutyTime, DutyHeaderx, "", InternetPhoneSMS);

                    #region Service reminder email Message for all members, regulars and 1stTimers
                    string Reminderfile = "ServiceReminderMsg.msgx"; bool DelFile = false;
                    if (System.IO.File.Exists(PathMsg + "SundayReminderMsg.msgx") && Rowx > 10) { Reminderfile = "SundayReminderMsg.msgx"; DelFile = true; }
                    else if (System.IO.File.Exists(PathMsg + "FridayReminderMsg.msgx") && Rowx < 10) { Reminderfile = "FridayReminderMsg.msgx"; DelFile = true; }
                    
                    FileStream fsk = new FileStream(PathMsg + Reminderfile, FileMode.Open);
                    StreamReader srk = new StreamReader(fsk);
                    string xEmailSubject = srk.ReadLine();
                    srk.ReadLine();
                    string xEmailBody = srk.ReadToEnd();
                    fsk.Close(); srk.Close();
                    xEmailSubject = xEmailSubject.Replace("#DutyHeader", DutyHeader);
                    xEmailBody = xEmailBody.Replace("#DutyHeader", DutyHeader);
                    xEmailBody = xEmailBody.Replace("#ServiceTime", ServiceTime);
                    //xEmailBody += EmailDefaultSignature();
                    SendEmailSt("5", ServiceReminderRegMemEmail("Email"), "", xEmailSubject, xEmailBody, schTime, null, NoneNull);
                    if (DelFile == true) System.IO.File.Delete(PathMsg + Reminderfile);

                    string ReminderSMSFile = "ServiceReminderSMS.msgx"; bool DelSMSFile = false;
                    if (System.IO.File.Exists(PathMsg + "SundayReminderSMS.msgx") && Rowx > 10) { ReminderSMSFile = "SundayReminderSMS.msgx"; DelSMSFile = true; }
                    else if (System.IO.File.Exists(PathMsg + "FridayReminderSMS.msgx") && Rowx < 10) { ReminderSMSFile = "FridayReminderSMS.msgx"; DelSMSFile = true; }
                    
                    FileStream fsx = new FileStream(PathMsg + ReminderSMSFile, FileMode.Open);
                    StreamReader srx = new StreamReader(fsx);
                    string SMSBody= srx.ReadToEnd();
                    fsx.Close(); srx.Close();
                    SMSBody = SMSBody.Replace("#DutyHeader", DutyHeader);
                    SMSBody = SMSBody.Replace("#ServiceTime", ServiceTime);
                    SendSMSSt(ServiceReminderRegMemEmail("PhoneNo"), SMSBody, schTime, "Phone");
                    if (DelSMSFile == true) System.IO.File.Delete(PathMsg + ReminderSMSFile);
                    #endregion
                }
            }
            else//DoTimer == false; manual roster msg
            {   //Roster
                ManualRosterEmaSms(EmailTo, MphoneNo, Title, FName, MName, LName, DutyTime, DutyHeaderx, DutyHeader);
                // Email SMS Service Reminder
                string FileType = "ServiceReminderMsg.msgx", SMSType = "ServiceReminderSMS.msgx";
                if (System.IO.File.Exists(PathMsg + "SundayReminderMsg.msgx") && Rowx > 10) { FileType = "SundayReminderMsg.msgx"; SMSType = "SundayReminderSMS.msgx"; }
                else if (System.IO.File.Exists(PathMsg + "FridayReminderMsg.msgx") && Rowx < 10) { FileType = "FridayReminderMsg.msgx"; SMSType = "FridayReminderSMS.msgx"; }
                
                ManualReminderEmaSms(FileType, SMSType, ServiceReminderRegMemEmail("Email"), ServiceReminderRegMemEmail("PhoneNo"), ServiceTime, DutyHeader);
                if (FileType == "SundayReminderMsg.msgx") { System.IO.File.Delete(PathMsg + "SundayReminderMsg.msgx"); System.IO.File.Delete(PathMsg + "SundayReminderSMS.msgx"); }
                if (FileType == "FridayReminderMsg.msgx") {System.IO.File.Delete(PathMsg + "FridayReminderMsg.msgx");   System.IO.File.Delete(PathMsg + "FridayReminderSMS.msgx");}
                               
            }
            #endregion
    }
       
        private void ManualRosterEmaSms(string EmailTo, string MphoneNo, string Title, string FName, string MName, string LName, string DutyTimex, string DutyHeaderx, string DutySubject)
        {
            FileStream fsop = new FileStream(TempFile, FileMode.Create, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine("Roster");
            swop.WriteLine(Title);
            swop.WriteLine(FName);
            swop.WriteLine(MName);
            swop.WriteLine(LName);
            swop.WriteLine(MphoneNo);
            swop.WriteLine(EmailTo);
            swop.WriteLine(DutyTimex);
            swop.WriteLine(DutyHeaderx);
            swop.WriteLine(DutySubject);//DutySubject = DutyHeader
            swop.Flush(); swop.Close();

            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.RosterEmaSms();
        }
        private void ManualReminderEmaSms(string FileType, string SMSType, string EmailTo, string MphoneNo, string ServiceTime, string DutyHeader)
        {
            FileStream fsk = new FileStream(PathMsg + FileType, FileMode.Open);
            StreamReader srk = new StreamReader(fsk);
            string yEmailSubject = srk.ReadLine();
            srk.ReadLine();
            string yEmailBody = srk.ReadToEnd();
            fsk.Close(); srk.Close();
            yEmailSubject = yEmailSubject.Replace("#DutyHeader", DutyHeader);
            yEmailBody = yEmailBody.Replace("#DutyHeader", DutyHeader);
            yEmailBody = yEmailBody.Replace("#ServiceTime", ServiceTime);

            FileStream fsx = new FileStream(PathMsg + SMSType, FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            string SMSBody = srx.ReadLine();
            fsx.Close(); srx.Close();
            SMSBody = SMSBody.Replace("#DutyHeader", DutyHeader);
            SMSBody = SMSBody.Replace("#ServiceTime", ServiceTime);
            
            FileStream fsop = new FileStream(TempFile, FileMode.Create, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine(MphoneNo);
            swop.WriteLine(EmailTo);
            swop.WriteLine(yEmailSubject);
            swop.WriteLine(SMSBody);
            swop.WriteLine(yEmailBody);
            swop.Flush(); swop.Close();

            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.ReminderEmaSms();
        }
        
        private string ServiceReminderRegMemEmail(string strEmailSMS)
        {
            string Email = "", Phone = "";
            try
            {
                DataTable dTable = new DataTable();
                string queryk = "SELECT GROUP_CONCAT(Email, ';') AS Emails, GROUP_CONCAT(MPhone, ';') AS MPhone, GROUP_CONCAT(Title, ';') AS Title, GROUP_CONCAT(First_Name, ';') AS FName FROM RegisterInfo WHERE Attendees = 'Regular' OR Attendees = 'Member' OR Attendees = 'New-Comer';";
                using (SQLiteConnection conTblk = new SQLiteConnection(strConReg))
                {
                    SQLiteCommand cmdRegister = new SQLiteCommand(queryk, conTblk);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    MyAdapter.SelectCommand = cmdRegister;
                    MyAdapter.Fill(dTable);
                }
                Email = dTable.Rows[0][0].ToString();
                Phone = dTable.Rows[0][1].ToString();
                Title1 = dTable.Rows[0][2].ToString();
                FName1 = dTable.Rows[0][3].ToString();                
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            if (strEmailSMS == "Email") return Email;
            else return Phone;
        }
        private void BirthdateSchedule()
        {
                try
                {
                    string Birthmonth = " " + DateTime.Now.ToString("MMMM"), Birthday = DateTime.Now.AddMinutes(MinAdd).Day.ToString();
                    string strTitle = "", strFName = "", strMName = "", strLName = "", MphoneNo = "", SMSMessage;
                    string queryx = "SELECT GROUP_CONCAT(Title, ';'), GROUP_CONCAT(First_Name, ';'), GROUP_CONCAT(Last_Name, ';'), "
                                  + "GROUP_CONCAT(Email, ';'), GROUP_CONCAT(MPhone, ';') FROM RegisterInfo WHERE BirthMonth = '" + Birthmonth + "' AND BirthDay = '" + Birthday + "';";
                    using (SQLiteConnection conTblx = new SQLiteConnection
                        (strConReg))
                    {
                        SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                        SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                        DataTable dTable = new DataTable();
                        MyAdapter.SelectCommand = cmdOfferingx;
                        MyAdapter.Fill(dTable);
                        strTitle = dTable.Rows[0][0].ToString(); //Title
                        strFName = dTable.Rows[0][1].ToString();//First name
                        strMName = "None";                      // Middle Name
                        strLName = dTable.Rows[0][2].ToString();// Last Name
                        EmailTo = dTable.Rows[0][3].ToString();//Email
                        MphoneNo = dTable.Rows[0][4].ToString();//Mobile Phone
                    }
                                    
                FileStream fs = new FileStream(PathMsg + "BirthdateEmailMessage.msgx", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                EmailSubject = sr.ReadLine();
                sr.ReadLine();
                EmailBody = sr.ReadToEnd();
                fs.Close(); sr.Close();
                EmailBody = EmailBody.Replace("#Birthmonth", "#Para1");
                EmailBody = EmailBody.Replace("#Birthday", "#Para2");
                //EmailBody += EmailDefaultSignature();
                SendEmailDt("2", EmailTo, "", EmailSubject, EmailBody, "", null, NoneNull, strTitle, strFName, strMName, strLName, Birthmonth, Birthday, "");

                FileStream fsx = new FileStream(PathMsg + "BirthdateSMSMessage.msgx", FileMode.Open);
                StreamReader srx = new StreamReader(fsx);
                SMSMessage = srx.ReadToEnd();
                fsx.Close(); srx.Close();
                SendSMSDt(MphoneNo, SMSMessage, "", strTitle, strFName, strMName, strLName, Birthday, "", "", InternetPhoneSMS);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        public void RunLateRoster()
        {
            DoTimer = false;
            timerSch();
            DoTimer = true;
        }
        private void DelDayReminderFile()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                if (System.IO.File.Exists(PathMsg + "SundayReminderMsg.msgx")) System.IO.File.Delete(PathMsg + "SundayReminderMsg.msgx");
                if (System.IO.File.Exists(PathMsg + "SundayReminderSMS.msgx")) System.IO.File.Delete(PathMsg + "SundayReminderSMS.msgx");
                if (System.IO.File.Exists(PathMsg + "FridayReminderMsg.msgx")) System.IO.File.Delete(PathMsg + "FridayReminderMsg.msgx");
                if (System.IO.File.Exists(PathMsg + "FridayReminderSMS.msgx")) System.IO.File.Delete(PathMsg + "FridayReminderSMS.msgx");
            }
        }
        private void Create1stTimerTable()// FirstTimerInfo DB Table
        {
            string strDollarCent = "Title VARCHAR(7), First_Name VARCHAR(20), MName VARCHAR(20), Last_Name VARCHAR(20), BirthDay SMALLINT, BirthMonth VARCHAR(10), Age_Group VARCHAR(20), "
                    + "Sex VARCHAR(7), Marital_Status VARCHAR(10), Email VARCHAR(50), MPhone VARCHAR(15), HPhone VARCHAR(15), HAdd VARCHAR(50), City VARCHAR(25), State VARCHAR(15), "
                    + "PCode VARCHAR(7), Country VARCHAR(15), Attendees VARCHAR(10), OfferingId SMALLINT";
            string strTbl = "CREATE TABLE IF NOT EXISTS FirstTimerInfo (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date DATE, " + strDollarCent + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private static long ToUnixTimestamp(DateTime value)
        {
            var unixTimestamp = System.Convert.ToInt64((value - new DateTime(1970, 1, 1, 0, 0, 0, value.Kind)).TotalSeconds);
            return (unixTimestamp - 36000);
        }
        
        private void mnRestore_Click(object sender, EventArgs e)
        {
            FmRecords fmRecords = new FmRecords();
            fmRecords.Show();
            fmRecords.FormClosed += new FormClosedEventHandler(FmClosed);
        }
        private void mnSchedulerMonitor_Click(object sender, EventArgs e)
        {
            FmSchedulerMonitor fmSchedulerMonitor = new FmSchedulerMonitor();
            fmSchedulerMonitor.Show();
        }
        private void mnClose_Click(object sender, EventArgs e)
        {
            Sched = false; this.Close();
        }

        private void ReportAlert()
        {
            string DayDescript = "";
            string Datex4 = DateTime.Parse(ReportDatex).AddDays(-3).ToString("dd/MM/yyyy");
            string Datex1 = DateTime.Parse(ReportDatex).AddDays(-1).ToString("dd/MM/yyyy");
            if (Datex4 == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                DayDescript = "in three day's time";
                MessageBox.Show("The Report is due for submission " + DayDescript + " - " + DateTime.Parse(ReportDatex).ToLongDateString() 
                            + ".", "    Report Submisssion Reminder: ",MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            if (Datex1 == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                DayDescript = "tommorrow";
                MessageBox.Show("Reminder: The Report is due for submission " + DayDescript + " - " + DateTime.Parse(ReportDatex).ToLongDateString()
                            + ".", "Report Submisssion Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ReportSchedule()
        {
            string DayDescript = "";
            string Datex4 = DateTime.Parse(ReportDatex).AddDays(-4).ToString("dd/MM/yyyy");
            string Datex1 = DateTime.Parse(ReportDatex).AddDays(-1).ToString("dd/MM/yyyy");
            if (Datex4 == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                DayDescript = "in four day's time";
                ReportEmail(DateTime.Parse(ReportDatex).ToLongDateString(), DayDescript);
            }
            if (Datex1 == DateTime.Now.ToString("dd/MM/yyyy"))
            {
                DayDescript = "tommorrow";
                ReportEmail(DateTime.Parse(ReportDatex).ToLongDateString(), DayDescript);
            }
        }
        public string EmailSignature(string EmailFromPson)
        {
            /*FileStream fsgn = new FileStream(PathMsg + "EmailSignature.msgx", FileMode.Open);
            StreamReader sgn = new StreamReader(fsgn);
            string Signature = sgn.ReadToEnd();
            fsgn.Close(); sgn.Close();
            return Signature;//*/

            string[] cbArr = EmailDetailsArray[int.Parse(EmailFromPson)].Split(new[] { Pass }, StringSplitOptions.None);
            return cbArr[5]; //emailSign
        }
        public string EmailFootnote()
        {        
            FileStream fsx = new FileStream(PathMsg + "EmailFootnote.msgx", FileMode.Open);
            StreamReader srx = new StreamReader(fsx);
            string Footnote = srx.ReadToEnd();
            fsx.Close(); srx.Close();
            return "\n<br>\n<br><font color=\"brown\"><b>*************************************************************************************************************************</b></font>"
                   + Footnote
                   + "\n<br><font color=\"brown\"><b>*************************************************************************************************************************</b></font>"
                   + "<br><font color=\"blue\"><b>SosiDB Software © 2015 AduraX Corp.</b></font>";
        }               
        #endregion                 

        #region ************* Email Sending Thru Background worker ***********************************

        BackgroundWorker bwDoSendMsg; MailMessage mail; 
        bool doReadMsgFile = true; 
        private void SendBwMsg()
        {
            bwDoSendMsg = new BackgroundWorker();
            bwDoSendMsg.WorkerReportsProgress = true;  // To report progress from the background worker we need to set this property
            bwDoSendMsg.DoWork += new DoWorkEventHandler(RunBwJob); // This event will be raised on the worker thread when the worker starts
           // bwDoSendMsg.ProgressChanged += new ProgressChangedEventHandler(bwDoSendMsg_ProgressChanged); // This event will be raised when we call ReportProgress
            bwDoSendMsg.RunWorkerAsync();  //Start the background worker 
        }
        public void RunBwJob(object sender, DoWorkEventArgs e)
        {
            while (0 < 1)if (doReadMsgFile == true && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                         if (MasterPc == Environment.MachineName)ReadSavedMsgFiles();
        }

        private string smtpAddress, emailFrom, emailName, emailPass, emailBcc, emailFootnote, emailSign; 
        string Axiv, AxivTitle, AxivFName, AxivMName, AxivLName, AxivPara1, AxivPara2, AxivPara3;        
        private int portNumber, j;
        string userName = "rccgfolp", Key = "95BFFA59-7F75-20C3-348A-74FF83FCB728", SenderId = "RCCGFOLP", PreMsg = "<Subject: From RCCG FOLP Canberra Church> ";
        string[] splitEmailTo, splitEmailCC, splitTitle, splitFName, splitMName, splitLName, splitPara1, splitPara2, splitPara3, splitNumber;
               
        public void SendEmailSt(string cnEmailFrom, string cnEmailTo, string cnEmailCC, string cnEmailSubject, string cnEmailBody, string cnSchedule,
                              List<string> cnAttachmentList, string cnImgName)
        {
            try
            {
                string Date_time = "Outgoing_" + DateTime.Now.ToString("ddMMyy_HHmmssff");//ddMM
                FileStream fsop = new FileStream(PathOutBox + Date_time + ".ema", FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("false");
                swop.WriteLine(cnSchedule);
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine("N/A");
                swop.WriteLine(cnImgName);
                swop.WriteLine(AttachmentList(cnAttachmentList));
                swop.WriteLine(cnEmailFrom);
                swop.WriteLine(cnEmailTo);
                swop.WriteLine(cnEmailCC);
                swop.WriteLine(cnEmailSubject);
                swop.WriteLine("***Email Body Start***");
                swop.WriteLine(cnEmailBody);
                swop.WriteLine("***Email Body End***");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        public void SendEmailDt(string cnEmailFrom, string cnEmailTo, string cnEmailCC, string cnEmailSubject, string cnEmailBody, string cnSchedule, List<string> cnAttachmentList, 
            string cnImgName, string cnTitlex, string cnFNamex, string cnMNamex, string cnLNamex, string cnPara1, string cnPara2, string cnPara3)
        {
            try
            {
                string Date_time = "Outgoing_" + DateTime.Now.ToString("ddMMyy_HHmmssff");//ddMM
                FileStream fsop = new FileStream(PathOutBox + Date_time + ".ema", FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("true");
                swop.WriteLine(cnSchedule);
                swop.WriteLine(cnTitlex);
                swop.WriteLine(cnFNamex);
                swop.WriteLine(cnMNamex);
                swop.WriteLine(cnLNamex);
                swop.WriteLine(cnPara1);
                swop.WriteLine(cnPara2);
                swop.WriteLine(cnPara3);
                swop.WriteLine(cnImgName);
                swop.WriteLine(AttachmentList(cnAttachmentList));
                swop.WriteLine(cnEmailFrom);
                swop.WriteLine(cnEmailTo);
                swop.WriteLine(cnEmailCC);
                swop.WriteLine(cnEmailSubject);
                swop.WriteLine("***Email Body Start***");
                swop.WriteLine(cnEmailBody);
                swop.WriteLine("***Email Body End***");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }           
        }

        public void SendSMSSt(string cnNumber, string cnMessage, string cnSchedule, string cnInternetPhoneSMS)
        {
            if (cnInternetPhoneSMS == "Phone")
            {
                try
                {
                    string Name = "SD_" + DateTime.Now.ToString("yyMMddHHmmssff");
                    FileStream fsop = new FileStream(PathPhoneSMS + Name + ".ssdb", FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine("DiffSms=No");
                    swop.WriteLine(cnNumber);
                    swop.WriteLine(PreMsg + cnMessage);
                    swop.Flush(); swop.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
            else
            {
                #region //InternetPhoneSMS == "Internet"
                try
                {
                    string Date_time = "Outgoing_" + DateTime.Now.ToString("ddMMyy_HHmmssff");//ddMM
                    FileStream fsop = new FileStream(PathOutBox + Date_time + ".sms", FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine("false");
                    swop.WriteLine(cnNumber);
                    swop.WriteLine(cnMessage);
                    swop.WriteLine(cnSchedule);
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.WriteLine("N/A");
                    swop.Flush(); swop.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }    
                #endregion
            }
        }
        public void SendSMSDt(string cnNumber, string cnMessage, string cnSchedule, string cnTitlex, string cnFNamex, string cnMNamex, 
                              string cnLNamex, string cnPara1, string cnPara2, string cnPara3, string cnInternetPhoneSMS)
        {
            if (cnInternetPhoneSMS == "Phone")
            {
                #region //InternetPhoneSMS == "Phone"
                try
                {
                    string Name = "SD_" + DateTime.Now.ToString("yyMMddHHmmssff");
                    FileStream fsop = new FileStream(PathPhoneSMS + Name + ".ssdb", FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine("DiffSms=Yes");
                    swop.WriteLine(cnNumber);
                    swop.WriteLine(cnTitlex);
                    swop.WriteLine(cnFNamex);
                    swop.WriteLine(cnLNamex);
                    swop.WriteLine(cnPara1);
                    swop.WriteLine(cnPara2);
                    swop.WriteLine(PreMsg + cnMessage);
                    swop.Flush(); swop.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                #endregion
            }
            else
            {
                #region //InternetPhoneSMS == "Internet"
                try
                {
                    string Date_time = "Outgoing_" + DateTime.Now.ToString("ddMMyy_HHmmssff");//ddMM
                    FileStream fsop = new FileStream(PathOutBox + Date_time + ".sms", FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine("true");
                    swop.WriteLine(cnNumber);
                    swop.WriteLine(cnMessage);
                    swop.WriteLine(cnSchedule);
                    swop.WriteLine(cnTitlex);
                    swop.WriteLine(cnFNamex);
                    swop.WriteLine(cnMNamex);
                    swop.WriteLine(cnLNamex);
                    swop.WriteLine(cnPara1);
                    swop.WriteLine(cnPara2);
                    swop.WriteLine(cnPara3);
                    swop.WriteLine(cnInternetPhoneSMS);
                    swop.Flush(); swop.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                #endregion
            }
        }

        private void ReadEmailFrom(string EmailFromPson)
        {
            string[] cbArr = EmailDetailsArray[int.Parse(EmailFromPson)].Split(new[] { Pass }, StringSplitOptions.None);
            emailName = cbArr[0]; emailFrom = cbArr[1]; smtpAddress = cbArr[2];
            portNumber = int.Parse(cbArr[3]); emailPass = cbArr[4]; emailSign = cbArr[5];
        }
        private void ReadSavedMsgFiles()
        {
            bool DiffMsg; string Schedulex, Titlex, FNamex, MNamex, LNamex, Para1x, Para2x, Para3x;
            
            doReadMsgFile = false;
            DirectoryInfo apple = new DirectoryInfo(@PathOutBox);
            foreach (var file in apple.GetFiles())
            {
              try
                {
                    using (StreamReader sr = new StreamReader(PathOutBox + file.ToString()))
                    {
                        if (Path.GetExtension(PathOutBox + file.ToString()) == ".sms")
                        {
                            string PhoneNox, Msgx;
                            DiffMsg = bool.Parse(sr.ReadLine());
                            PhoneNox = sr.ReadLine();
                            Msgx = sr.ReadLine();
                            Schedulex = sr.ReadLine();
                            Titlex = sr.ReadLine();
                            FNamex = sr.ReadLine();
                            MNamex = sr.ReadLine();
                            LNamex = sr.ReadLine();
                            Para1x = sr.ReadLine();
                            Para2x = sr.ReadLine();
                            Para3x = sr.ReadLine();
                            SendSavedSms(DiffMsg, PhoneNox, Msgx, Schedulex, Titlex, FNamex, MNamex, LNamex, Para1x, Para2x, Para3x);
                        }
                        else if (Path.GetExtension(PathOutBox + file.ToString()) == ".ema")
                        {
                            string subrow = "**"; 
                            string ImgNamex, EmailFromx, EmailTox, EmailCCx, EmailSubjectx, EmailBodyx = ""; List<string> ltAttachmentList;
                            DiffMsg = bool.Parse(sr.ReadLine()); 
                            Schedulex = sr.ReadLine();
                            Titlex = sr.ReadLine();
                            FNamex = sr.ReadLine();
                            MNamex = sr.ReadLine();
                            LNamex = sr.ReadLine();
                            Para1x = sr.ReadLine();
                            Para2x = sr.ReadLine();
                            Para3x = sr.ReadLine();
                            ImgNamex = sr.ReadLine();
                            ltAttachmentList = RestoreAttList(sr.ReadLine());
                            EmailFromx = sr.ReadLine();
                            EmailTox = sr.ReadLine(); 
                            EmailCCx = sr.ReadLine();
                            EmailSubjectx = sr.ReadLine();
                            sr.ReadLine();//"***Email Body Start***"
                            while (subrow != "***Email Body End***")
                            {
                                subrow = sr.ReadLine();
                                if (subrow == "***Email Body End***") break;
                                EmailBodyx += "\n" + subrow;
                            }
                            SendSavedEmail(DiffMsg, Schedulex, Titlex, FNamex, MNamex, LNamex, Para1x, Para2x, Para3x,
                                           ImgNamex, ltAttachmentList, EmailFromx, EmailTox, EmailCCx, EmailSubjectx, EmailBodyx);
                        }
                    }
                    System.IO.File.Delete(PathOutBox + file.ToString());
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }

            DateTime ReadMsgFileTimeLast = DateTime.Now.AddSeconds(60);
            while (ReadMsgFileTimeLast > DateTime.Now) { }
            doReadMsgFile = true;
        }
        private void SendSavedSms(bool DiffMsg, string PhoneNox, string Msgx, string Schedulex, 
                                  string Titlex, string FNamex, string MNamex, string LNamex, string Para1x, string Para2x, string Para3x)
        {
            string Messagex = "", AxivSMS = "";
            try
            {
                CreditRespondsData _Result; double Resultvalue = 0.0;
                _Result = ClickSendSMS.CheckCredit(userName, Key);
                Resultvalue = (double)_Result.Value;

                if (Resultvalue > 0.50)
                {
                    //  attempt to send message
                    char[] sep = new char[] { ';' }; string MbNo; int iError = 0;
                    splitNumber = PhoneNox.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (DiffMsg == true)
                    {
                        splitTitle = Titlex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitFName = FNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitMName = MNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitLName = LNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitPara1 = Para1x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitPara2 = Para2x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        splitPara3 = Para3x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    }
                    string strDetail = "", AxivSMSNo = ""; j = 0;
                    foreach (string Num in splitNumber)
                    {
                        strDetail = Msgx;
                        if (DiffMsg == true)
                        {
                            if (splitTitle[j] != "") { strDetail = strDetail.Replace("#Title", splitTitle[j]); AxivTitle = AxivTitle + ";" + splitTitle[j]; }
                            if (splitFName[j] != "") { strDetail = strDetail.Replace("#FName", splitFName[j]); AxivFName = AxivFName + ";" + splitFName[j]; }
                            if (splitMName[j] != "") { strDetail = strDetail.Replace("#MName", splitMName[j]); AxivMName = AxivMName + ";" + splitMName[j]; }
                            if (splitLName[j] != "") { strDetail = strDetail.Replace("#LName", splitLName[j]); AxivLName = AxivLName + ";" + splitLName[j]; }
                            if (Para1x != "") { strDetail = strDetail.Replace("#Para1", splitPara1[j]); AxivPara1 = AxivPara1 + ";" + splitPara1[j]; }
                            if (Para2x != "") { strDetail = strDetail.Replace("#Para2", splitPara2[j]); AxivPara2 = AxivPara2 + ";" + splitPara2[j]; }
                            if (Para3x != "") { strDetail = strDetail.Replace("#Para3", splitPara3[j]); AxivPara3 = AxivPara3 + ";" + splitPara3[j]; }
                        }

                        MbNo = Num.Trim(); SMSRespondsData _Data = null;
                        strDetail = strDetail.Replace("\n", " "); strDetail = strDetail.Replace("   ", " "); strDetail = strDetail.Replace("  ", " ");

                        if (MbNo.Length == 10) _Data = ClickSendSMS.SendSms(userName, Key, MbNo, strDetail, SenderId, Schedulex);
                        else if (MbNo.Length != 10 || _Data.RespondsCode != 0)
                        {
                            if (splitNumber[j].Contains("None") == false && splitNumber[j].Contains("none") == false)
                            {
                                iError++;
                                ErrorSMSEmail("InvalidSMS", Num, strDetail, "", "", "", "", Schedulex, 1, false);
                            }
                        }
                        j++; 
                        AxivSMSNo = Num + ";" + AxivSMSNo;
                    }

                    if (iError > 0)
                    {
                        Messagex = AxivSMS + "\n***SMSBody starts *****\n" + Msgx + "\n\nRan @ " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
                        SendEmail(emailBcc, "BCC-Error(s) in Sending SMS(s)", Messagex);
                    }
                }
                else
                {
                    string EmailxTo = "adurafimihan.abiona@gmail.com";
                    string EmailxSubject = "Insufficient SMS credit";
                    string EmailxBody = "SMS credit would not be sufficient to send the SMS. The balance = " + Resultvalue
                                      + "\nRan @ " + DateTime.Now.ToString("HH:mm:ss-dd/MM/yy");
                    SendEmail(EmailxTo, EmailxSubject, EmailxBody);
                }
            }
            catch (Exception ex)
            {
                Messagex = AxivSMS + "\n***SMSBody starts *****\n" + Messagex + "\n\nRan @ " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss")
                         + ".\n\n Withe this exception: " + ex;
                SendEmail(emailBcc, "BCC-Error(s) in Sending SMS(s)", Messagex);
            }
        }
        private void SendSavedEmail(bool DiffMsg, string Schedulex, string Titlex, string FNamex, string MNamex, string LNamex,
                                    string Para1x, string Para2x, string Para3x, string ImgNamex, List<string> ltAttachmentList,
                                    string EmailFromx, string EmailTox, string EmailCCx, string EmailSubjectx, string EmailBodyx)
        {
            char[] sep = new char[] { ';' }; string AxivEmailTo = "";
            ReadEmailFrom(EmailFromx);
            try
            {
                splitEmailTo = EmailTox.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                splitEmailCC = EmailCCx.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                Axiv = j.ToString() + " out of " + splitEmailTo.Length.ToString();

                if (DiffMsg == true)
                {
                    splitTitle = Titlex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitFName = FNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitMName = MNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitLName = LNamex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitPara1 = Para1x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitPara2 = Para2x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    splitPara3 = Para3x.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                }

                using (mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom, emailName, Encoding.UTF8);
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.SubjectEncoding = Encoding.UTF8;

                    SmtpClient smtp = new SmtpClient(smtpAddress, portNumber);
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(emailFrom, emailPass);
                        smtp.EnableSsl = true; //if (EmailInternet == true) smtp.EnableSsl = true;
                        //else { smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory; smtp.PickupDirectoryLocation = PathOutBox; }

                        if (ltAttachmentList != null) attach(ltAttachmentList);

                        j = 0; int iError = 0; string strDetail = "";
                        mail.Subject = EmailSubjectx;
                        string strBody = EmailBodyx.Replace("\n", "\n<br>");

                        foreach (string ml in splitEmailTo)
                        {
                            strDetail = strBody;
                            if (DiffMsg == true)
                            {
                                if (splitTitle[j] != "") { strDetail = strDetail.Replace("#Title", splitTitle[j]); AxivTitle = AxivTitle + ";" + splitTitle[j]; }
                                if (splitFName[j] != "") { strDetail = strDetail.Replace("#FName", splitFName[j]); AxivFName = AxivFName + ";" + splitFName[j]; }
                                if (splitMName[j] != "") { strDetail = strDetail.Replace("#MName", splitMName[j]); AxivMName = AxivMName + ";" + splitMName[j]; }
                                if (splitLName[j] != "") { strDetail = strDetail.Replace("#LName", splitLName[j]); AxivLName = AxivLName + ";" + splitLName[j]; }
                                if (Para1x != "") { strDetail = strDetail.Replace("#Para1", splitPara1[j]); AxivPara1 = AxivPara1 + ";" + splitPara1[j]; }
                                if (Para2x != "") { strDetail = strDetail.Replace("#Para2", splitPara2[j]); AxivPara2 = AxivPara2 + ";" + splitPara2[j]; }
                                if (Para3x != "") { strDetail = strDetail.Replace("#Para3", splitPara3[j]); AxivPara3 = AxivPara3 + ";" + splitPara3[j]; }

                                //if (ltAttachmentList != null) attach(ltAttachmentList);  
                                //if (ImgName != fmLogin.NoneNull)
                            }

                            if (ImgNamex != NoneNull)
                            {
                                LinkedResource LinkedImage = new LinkedResource(ImgNamex);//NewYear.gif, Xmas.gif, jpg
                                LinkedImage.ContentId = "MyPic";
                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strDetail + "\n<br>\n<br><img src=cid:MyPic>\n<br>" + EmailSignature(EmailFromx) + emailFootnote, null, "text/html");
                                htmlView.LinkedResources.Add(LinkedImage);
                                mail.AlternateViews.Add(htmlView);
                            }
                            else mail.Body = strDetail + EmailSignature(EmailFromx) + emailFootnote;

                            if (ml.Length > 6 && ml.Contains('@') == true)
                            {
                                if (j == 0) if (EmailCC != "") foreach (string mcc in splitEmailCC) mail.CC.Add(mcc);
                                mail.To.Clear(); mail.To.Add(splitEmailTo[j]);
                                smtp.Send(mail);
                                mail.CC.Clear();
                            }
                            else
                            {
                                if (splitEmailTo[j].Contains("None") == false && splitEmailTo[j].Contains("none") == false && splitEmailTo[j].Contains("") == false)
                                {
                                    iError++;
                                    ErrorSMSEmail("InvalidEmailAddress", "", "", splitEmailTo[j], "", EmailSubjectx, strDetail, Schedulex, 0, false);
                                }
                            }
                            j++;
                            AxivEmailTo = AxivEmailTo + ";" + ml;
                        }

                        Axiv = j.ToString() + " out of " + splitEmailTo.Length.ToString() + "\n" + AxivEmailTo + "\n" + AxivTitle + "\n" + AxivFName + "\n" + AxivMName + "\n"
                                            + AxivLName + "\n" + AxivPara1 + "\n" + AxivPara2 + "\n" + AxivPara3;
                        string EmailSubjecty;
                        if (iError > 0) EmailSubjecty = "BCC-Error(s) in Sending Email(s): " + EmailSubjectx;
                        else EmailSubjecty = "BCC: " + EmailSubjectx;
                        string EmailBodyy = Axiv + "\n***EmailBoady starts *****\n" + EmailBodyx + "\n***EmailBoady ends *****\n"
                                          + "\n\nRan @ " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
                        //SendEmail(emailBcc, EmailSubjecty, EmailBodyy);
                        mail.To.Clear(); mail.CC.Clear();
                        mail.Subject = EmailSubjecty;
                        mail.Body = EmailBodyy.Replace("\n", "\n<br>");
                        mail.To.Add(emailBcc); 
                        smtp.Send(mail);
                        
                        SaveEmailCopy(PathEmailCopy, DiffMsg.ToString(), EmailTox, "", AxivTitle, AxivFName, AxivMName, AxivLName,
                                      AxivPara1, AxivPara2, AxivPara3, ImgNamex, ltAttachmentList, EmailFromx, AxivEmailTo, EmailCCx, EmailSubjectx, EmailBodyx);
                        Axiv = ""; AxivEmailTo = ""; AxivTitle = ""; AxivFName = ""; AxivMName = ""; AxivLName = ""; AxivPara1 = ""; AxivPara2 = ""; AxivPara3 = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string EmailSubjecty = "Catched Error(s) in Sending Email(s): " + EmailSubjectx;
                string EmailBodyy = Axiv + "\n~~~~~~~~ Error:\n" + ex.ToString() + "\n***EmailBoady starts *****\n" + EmailBodyx + "\n***EmailBoady ends *****\n"
                                    + "\n\nRan @ " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
                SendEmail(emailBcc, EmailSubjecty, EmailBodyy);

                SaveEmailCopy(PathEmailCopy, DiffMsg.ToString(), EmailTox, "", AxivTitle, AxivFName, AxivMName, AxivLName,
                              AxivPara1, AxivPara2, AxivPara3, ImgNamex, ltAttachmentList, EmailFromx, AxivEmailTo, EmailCCx, EmailSubjectx, EmailBodyx);
                Axiv = ""; AxivEmailTo = ""; AxivTitle = ""; AxivFName = ""; AxivMName = ""; AxivLName = ""; AxivPara1 = ""; AxivPara2 = ""; AxivPara3 = "";
            }
        }
        
        private void SaveEmailCopy(string PathEmailCopy, string diffEmail, string AllEmailTo, string Schedulex, string Titlex, string FNamex, string MNamex, string LNamex, string Para1,
                                string Para2, string Para3, string ImgName, List<string> ltAttachmentList, string EmailFrom, string EmailTo, string EmailCC, string EmailSubject, string EmailBody)
        {
            try
            {
                string Namex = DateTime.Now.ToString("ddMMyy_HHmmssff");//ddMM
                FileStream fsop = new FileStream(PathEmailCopy + "SeCopy_" + Namex + ".sec", FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(diffEmail);
                swop.WriteLine(AllEmailTo);
                swop.WriteLine(Schedulex);
                swop.WriteLine(Titlex);
                swop.WriteLine(FNamex);
                swop.WriteLine(MNamex);
                swop.WriteLine(LNamex);
                swop.WriteLine(Para1);
                swop.WriteLine(Para2);
                swop.WriteLine(Para3);
                swop.WriteLine(ImgName);
                swop.WriteLine(AttachmentList(ltAttachmentList));
                swop.WriteLine(EmailFrom);
                swop.WriteLine(EmailTo);
                swop.WriteLine(EmailCC);
                swop.WriteLine(EmailSubject);
                swop.WriteLine("***Email Body Start***");
                swop.WriteLine(EmailBody);
                swop.WriteLine("***Email Body End***");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }


        private void ErrorSMSEmail(string FileName, string SMSNumber, string SMSBody, string EmailTo, string EmailCC,
                   string EmailSubject, string EmailBody, string Schedule, int EmailSMSSel, bool diffEmail)
        {
            try
            {
                string Name = FileName + "_" + DateTime.Now.ToString("ddMMyyyy-HHmmssff");
                FileStream fsop = new FileStream(PathErrorMsg + Name + ".err", FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(FileName);
                swop.WriteLine(diffEmail.ToString());
                if (EmailSMSSel == 0) swop.WriteLine("0"); //Email
                else if (EmailSMSSel == 1) swop.WriteLine("1"); //SMS
                else swop.WriteLine("2"); //SMSEmail
                swop.WriteLine(Schedule);
                swop.WriteLine("***SMS***");
                swop.WriteLine(SMSNumber);
                swop.WriteLine(SMSBody);
                swop.WriteLine("***Email***");
                swop.WriteLine(EmailTo);
                swop.WriteLine(EmailCC);
                swop.WriteLine(EmailSubject);
                swop.WriteLine("***Email Body Start***");
                swop.WriteLine(EmailBody);
                swop.WriteLine("***Email Body End***");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
       
        public void SendEmail(string bwEmailTo, string bwEmailSubject, string bwEmailBody)
        {
            MailMessage mailx;
            using (mailx = new MailMessage())
            {
                mailx.From = new MailAddress(emailFrom, emailName, Encoding.UTF8);
                mailx.IsBodyHtml = true;
                mailx.BodyEncoding = Encoding.UTF8;
                mailx.SubjectEncoding = Encoding.UTF8;

                SmtpClient smtp = new SmtpClient(smtpAddress, portNumber);
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailFrom, emailPass);
                    smtp.EnableSsl = true; 

                    mailx.Subject = bwEmailSubject;   
                    bwEmailBody = bwEmailBody.Replace("\n", "\n<br>");
                    mailx.Body = bwEmailBody; // + EmailSignatureFootnote();
                    mailx.To.Add(bwEmailTo);
                    smtp.Send(mailx);
                }
            }
        }
        public void SchNotifyEmail()
        {
            string EmailxTo = "rccgfolpcanberra@gmail.com";
            string EmailxSubject = "Scheduler: " + DateTime.Now.ToString("HH:mm:ss-dd/MM/yy");
            string EmailxBody = "Scheduler Monitor Ran @ " + DateTime.Now.ToString("HH:mm:ss-dd/MM/yy");
            SendEmail(EmailxTo, EmailxSubject, EmailxBody);
        }
        public void ReportEmail(string date, string DayDescript)
        {
            string EmailxTo = ReportEmail3 + ";" + ReportEmail4 + ";" + ReportEmail5;
            string EmailxSubject = "Reminder: Report Submission on " + date;
            string EmailxBody = "The Report is due for submission " + DayDescript + ", " + date + ".";
            SendEmail(EmailxTo, EmailxSubject, EmailxBody);
        }

        private List<string> RestoreAttList(string AttList)
        {
            if (AttList == "null") return null;
            else
            {
                List<string> AttachmentList2 = new List<string>(); char[] sep = new char[] { ';' };
                string[] splitList = AttList.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ml in splitList) AttachmentList2.Add(ml);
                return AttachmentList2;
            }
        }       
        private string AttachmentList(List<string> AttList)
        {
            string AList = "";
            if (AttList == null) return "null";
            else
            {
                foreach (string ml in AttList) AList = AList + ";" + ml;
                return AList;
            }
        }
        private void attach(List<string> ltAttachmentList)
        {
            for (int k = 0; k <= ltAttachmentList.Count - 1; k++)
            {
                mail.Attachments.Add(new Attachment(ltAttachmentList[k]));
            }
        }
        #endregion       
        //// End of methods 
    }
}
