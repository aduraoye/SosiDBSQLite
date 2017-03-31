using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Diagnostics;

namespace SosiDB
{
    public partial class FmRecords : Form
    {
        FmLogin fmLogin;
        public string strConRec;
        string BackupPath, Semiannual; bool IsSemiannual = false;
        public FmRecords()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            strConRec = fmLogin.strConReg; 
            fmLogin.niBackground.Visible = false;
            Semiannual = fmLogin.AxivDir + "Backup\\" + fmLogin.AppName + "-" + Environment.MachineName + DateTime.Now.ToString("_MMM-yyyy") + "_Semiannual-Backup";
        }
        private void FmRecords_Load(object sender, EventArgs e)
        {
          this.Text = fmLogin.AppName + "[Records]-" + fmLogin.strChurchAcronym;
          tsslbDB.Text = "  | Loaded Database: " + fmLogin.database;
          CreateTables();
          ReadUserGroup();
             
         // Update when DB is modified by other systems
          string readUpdateAppFile = ReadUpdateAppFile();
          if (readUpdateAppFile == "CheckUpdate" || readUpdateAppFile == "AlreadyUpdated") { }
          else
          {
              DialogResult mgUpdate = MessageBox.Show("The database has been modified in another computer. \nWould like to update it?",
                  "Database Update", MessageBoxButtons.YesNo);
              if (mgUpdate == DialogResult.Yes) fmLogin.PreRestore(readUpdateAppFile, fmLogin.NoStartFile); 
              else return;
          }

          //Semiannual backups
          if (DateTime.Now.ToString("MM") == "01" || DateTime.Now.ToString("MM") == "07") 
          { 
              IsSemiannual = true;
              if (!Directory.Exists(Semiannual)) mnBackup_Click(sender, e); 
              IsSemiannual = false; 
          }
        }        
        private void ReadUserGroup()
        {
            FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string UserGroup = sr.ReadLine();
            string RegId = sr.ReadLine();
            fs.Close(); sr.Close();

            if (UserGroup == fmLogin.Rootgroup)
            {
                mnControlPanel.Visible = true; mnModTableData.Visible = true;
                IsRoot = true;
            }
            else
            {
                mnControlPanel.Visible = false; mnModTableData.Visible = false;
                IsRoot = false;
            }
        }
        private void CreateTables()
        {
            // Create "RegisterInfo" Table
            string strDollarCent = "Title VARCHAR(7), First_Name VARCHAR(20), MName VARCHAR(20), Last_Name VARCHAR(20), BirthDay SMALLINT, BirthMonth VARCHAR(10), Age_Group VARCHAR(20), "
                    + "Sex VARCHAR(7), Marital_Status VARCHAR(10), Email VARCHAR(50), MPhone VARCHAR(15), HAdd VARCHAR(50), City VARCHAR(25), State VARCHAR(15), "
                    + "PCode VARCHAR(7), Country VARCHAR(15), Residency VARCHAR(15), Attendees VARCHAR(10), Department1 VARCHAR(20), Department2 VARCHAR(20), Department3 VARCHAR(20), "
                    + "Service VARCHAR(25), Status VARCHAR(15), MoreDetails VARCHAR(5)";//true or false
            string strTbl = "CREATE TABLE IF NOT EXISTS  " + fmLogin.TableReg + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date DATE, " + strDollarCent + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }

            // Create OfferingInfo Table
            string strOffering = "NormOffering DECIMAL(8,2), Tithe DECIMAL(8,2), F1stfruit DECIMAL(8,2), Thanksgiving DECIMAL(8,2), Missions DECIMAL(8,2), "
                               + "Building DECIMAL(8,2), Pledges DECIMAL(8,2), Special DECIMAL(8,2), OtherOffering DECIMAL(8,2), ";
            string strAttendance = "NoMale SMALLINT, NoFemale SMALLINT, Nochild SMALLINT, NoTotal SMALLINT, No1stTimer SMALLINT, NoNewConvert SMALLINT, NoSunSch SMALLINT, ";
            string strTblOff = "CREATE TABLE IF NOT EXISTS " + fmLogin.TableOff + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date_Time DATETIME, Service VARCHAR(35), "
                                + strOffering + strAttendance + "TotalDeposit DECIMAL(8,2), TotalAmount DECIMAL(8,2), Preacher VARCHAR(45));";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTblOff, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }

            // Create AccountInfo Table           
            string strTblAcct = "CREATE TABLE IF NOT EXISTS " + fmLogin.TableAcct + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date_Time DATETIME, "
                          + "Custodian VARCHAR(15), CustDetails VARCHAR(95), Account VARCHAR(15), Transact VARCHAR(15), Amount DECIMAL(8,2), GSTAmt DECIMAL(8,2), "
                          + "GSTtype VARCHAR(5), Descript VARCHAR(100), OtherTable VARCHAR(20), OtherId INTEGER, FileName VARCHAR(20), FileExt VARCHAR(50));";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTblAcct, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }

            // Create "BankLodgeInfo" Table
            string strTblBL = "CREATE TABLE IF NOT EXISTS " + fmLogin.TableBL + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date_Time DATETIME, OfferingId SMALLINT, RegId SMALLINT, Offerings VARCHAR(35), Amount DECIMAL(8,2), OtherId INTEGER, PayMethod VARCHAR(5));";  //VARCHAR);";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTblBL, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }

            // Create ForeignCurrencyInfo Table// Date_Time, OfferingId, Custodian, CustDetails, Currency, Amount, Rate, AUD$, Descript
            string strTblFC = "CREATE TABLE IF NOT EXISTS " + fmLogin.TableFC + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date_Time DATETIME, OfferingId SMALLINT, Custodian VARCHAR(15), CustDetails VARCHAR(50), Currency VARCHAR(10), "
                            + "Transact VARCHAR(15), Amount DECIMAL(8,2), Rate DECIMAL(8,2), Comm DECIMAL(8,2), AUD$ DECIMAL(8,2), Descript VARCHAR(100));";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTblFC, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void WriteGoto()
        {
            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("2"); //StrUpdate
                swop.WriteLine("0"); // Record No not needed
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private string ReadUpdateAppFile()
        {
            string strUpdate = "";
            try
            {
                FileStream fs = new FileStream(fmLogin.UpdateFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                strUpdate = sr.ReadLine();
                fs.Close(); sr.Close(); fs.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return strUpdate;
        }

        #region // %%%%%%%%%%%%% Modifying or Recreating a database table %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        private void mnModTableData_Click(object sender, EventArgs e)
        {
            //ChangePassword();
            //DeleteData(4);
            DeleteDBTable(fmLogin.TableFC);
        }
        private void ModUpdate()
        {
            string ModTable = "BankLodgeInfo";
            string ODTFrom = "2016-07-01" + " 00:00:00";
            string ODTTo = "2016-08-16" + " 23:59:59";
            //Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "'";
            string query = "UPDATE " + ModTable + " SET PayMethod = 'Bank' WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }

            string queryx = "UPDATE " + ModTable + " SET PayMethod = 'Cash' WHERE RegId = '1' OR RegId = '2' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(queryx, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void ModDBTable()
        {
            string ModTable = "BankLodgeInfo";
            string OldPara = "Id, Date_Time, OfferingId, RegId, Offerings, Amount, OtherId";

            string NewPara = "Date_Time DATETIME, OfferingId SMALLINT, RegId SMALLINT, Offerings VARCHAR(35), Amount DECIMAL(8,2), OtherId INTEGER, PayMethod VARCHAR(5)";

            string NewInsert = "Date_Time, OfferingId, RegId, Offerings, Amount, OtherId, PayMethod";

            // Import data to datatable;
            DataTable dTableV = new DataTable();
            string OldQuery = "SELECT " + OldPara + " FROM " + ModTable;
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(OldQuery, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTableV);
            }

            //*/delete the old table 
            string query = "DROP TABLE IF EXISTS " + ModTable;
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }

            // Create a new table 
            string strTbl = "CREATE TABLE IF NOT EXISTS " + ModTable + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, " + NewPara + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }

            //Insert data from datatable to the new table
            string NewQuery, NewValue, strTitle = "None"; string Datex = "";

            for (int i = 0; i < dTableV.Rows.Count; i++)
            {//"RegId, Date, Title, First_Name, Last_Name, UserName, PassWord, Email, UserGroup";
                /*
                valRetry = true;
                while (valRetry == true)
                {
                    if (dialogDescript(ref Answer, ref valRetry, dTableV.Rows[i][5].ToString()) == DialogResult.Yes) strTitle = Answer;
                    else valRetry = false;
                }*/

                //NewInsert = "Date_Time, OfferingId, RegId, Offerings, Amount, OtherId, PayMethod"; 
                Datex = DateTime.Parse(dTableV.Rows[i][1].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                NewValue = Datex + "', '" +
                           dTableV.Rows[i][2].ToString() + "', '" + dTableV.Rows[i][3].ToString() + "', '" +
                           dTableV.Rows[i][4].ToString() + "', '" + dTableV.Rows[i][5].ToString() + "', '" +
                           dTableV.Rows[i][6].ToString() + "', '" + strTitle;

                NewQuery = "INSERT INTO " + ModTable + " (" + NewInsert + ") VALUES ('" + NewValue + "');";
                using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOffering = new SQLiteCommand(NewQuery, conTbly);
                    conTbly.Open();
                    cmdOffering.ExecuteNonQuery();
                }
            }//*/
        }
        //bool valRetry; string Answer;// QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ
        public static DialogResult dialogDescript(ref string Answer, ref bool valRetry, string User)
        {
            Form form = new Form();
            Label lbInfo = new Label();
            TextBox txEmailTo = new TextBox();
            Button buttonYes = new Button();
            Button buttonNo = new Button();

            form.Text = " Special Service Description";
            lbInfo.Text = User + " = Type in the description for the special service.";
            txEmailTo.Text = "Description";
            buttonYes.Text = "Yes"; buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.Text = "No"; buttonNo.DialogResult = DialogResult.No;
            lbInfo.SetBounds(5, 10, 250, 25);
            txEmailTo.SetBounds(20, 50, 180, 20);
            buttonYes.SetBounds(20, 85, 80, 23); buttonNo.SetBounds(120, 85, 80, 23);
            buttonYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(250, 120);
            form.Controls.AddRange(new Control[] { lbInfo, txEmailTo, buttonYes, buttonNo });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonYes;
            form.CancelButton = buttonNo;

            DialogResult dialogResult = form.ShowDialog();
            if (txEmailTo.Text != "Description")
            {
                if (form.DialogResult == DialogResult.Yes) Answer = txEmailTo.Text;
                valRetry = false;
                return dialogResult;
            }
            else
            {
                valRetry = true;
                if (dialogResult == DialogResult.Yes) MessageBox.Show("You have not typed in a description.");
                return dialogResult;
            }
        }

        private int Count(string DelTable)
        {
            string strCount = "SELECT COUNT(*) FROM " + DelTable;
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
        private void DeleteData(int k)
        {
            string queryx = "DELETE FROM " + fmLogin.TableReg + " WHERE Id > '" + k.ToString() + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }
            MessageBox.Show("All deleted.");
        }
        private void ChangePassword()
        {
            string newPassword = fmLogin.Pass + fmLogin.strChurchAcronym + fmLogin.Pass;
            SQLiteConnection connection = new SQLiteConnection(fmLogin.strConReg);
            connection.Open();
            connection.ChangePassword(newPassword);
            connection.Close();
        }
        private void DeleteDBTable(string TableDel)
        { 
            string query = "DROP TABLE IF EXISTS " + TableDel;
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void CultureNames()
        {
            List<string> list = new List<string>();
            foreach (System.Globalization.CultureInfo ci in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures))
            {
                string specName = "(none)";
                try { specName = System.Globalization.CultureInfo.CreateSpecificCulture(ci.Name).Name; }
                catch { }
                list.Add(String.Format("{0,-12}{1,-12}{2}", ci.Name, specName, ci.EnglishName));
            }

            list.Sort();  // sort by name

            // write to console
            Console.WriteLine("CULTURE   SPEC.CULTURE  ENGLISH NAME");
            Console.WriteLine("--------------------------------------------------------------");
            foreach (string str in list) Console.WriteLine(str);
            //format.Culture = new CultureInfo("en-AU"); //Using English(Australia) cultureinfo
        }

        /*/ *******************  Load Excel to Data table ****************************
        public DataTable GetTableFromExcel(string PathFilename, bool hasHeader)
        {
            using (var pck = new ExcelPackage())
            {
                #region Fast function for CSV files conversion to Excel files
                /* My suggestion here is to read the file by yourself and then use the library to create the file. The code to read the CSV could be as simple as:
                List<String> lines = new List<String>();
                using (StreamReader reader = new StreamReader("file.csv"))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.add(line);
                    }
                }
                //Now you got all lines of your CSV. Create your file with EPPLUS
                foreach (String line in lines)
                {
                    var values = line.Split(',');
                    foreach (String value in values)
                    {
                        //use EPPLUS library to fill your file
                    }
                }

                List<Engagement> QueryResult = PMService.GetRequestedEngagments(test);
                var filename = yourfilename;
                var path = @"C:\Users\er4505\Downloads" + filename;
                var excelFileInfo = new FileInfo(path);

                //try {
                using (ExcelPackage csvToExcel = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = csvToExcel.Workbook.Worksheets.Add(filename);
                    worksheet.Cells["A1"].LoadFromCollection(QueryResult, true, OfficeOpenXml.Table.TableStyles.Medium25);
                    csvToExcel.SaveAs(excelFileInfo);
                }
               // }
                
                #endregion

                string FileExt = Path.GetExtension(PathFilename);
                ExcelWorksheet ws;
                if (FileExt == ".csv")
                {
                    List<String> lines = new List<String>();
                    using (StreamReader reader = new StreamReader(PathFilename))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                    var format = new ExcelTextFormat(); // var format = new StringFormat
                    format.Delimiter = ',';  //format.Encoding = new UTF7Encoding();
                    format.Culture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                    format.Culture.DateTimeFormat.ShortDatePattern = "dd/mm/yyyy";
                    format.EOL = "\r";  // DEFAULT IS "\r\n";             
                    format.TextQualifier = '"';
                    ws = pck.Workbook.Worksheets.Add("Dummy");
                    ws.Cells["A1"].LoadFromCollection(lines, hasHeader, OfficeOpenXml.Table.TableStyles.Medium25);
                    //ws.Cells["A1"].LoadFromText(new FileInfo(PathFilename), format, OfficeOpenXml.Table.TableStyles.Medium27, hasHeader);
                }
                else
                {
                    using (var stream = File.OpenRead(PathFilename)) { pck.Load(stream); }
                    ws = pck.Workbook.Worksheets.First();
                }

                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    //Date Details Amount Balance
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }//*/

        #endregion//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%       
        
        private void btRegister_Click(object sender, EventArgs e)
        {
            FmRegNew fmRegNew = new FmRegNew();
            fmRegNew.ShowDialog();
        }
        private void btRoster_Click(object sender, EventArgs e)
        {
            FmRoster fmRoster = new FmRoster();
            fmRoster.ShowDialog();        
        }
        private void btOfferingInput_Click(object sender, EventArgs e)
        {
            FmOfferingInput fmOfferingInput = new FmOfferingInput();
            fmOfferingInput.ShowDialog(); 
        }
        private void btOtheAcct_Click(object sender, EventArgs e)
        {
            FmAccounts fmImprest = new FmAccounts();
            fmImprest.ShowDialog();
        }
        private void btBanlLodge_Click(object sender, EventArgs e)
        {
            WriteGoto();
            FmBankLodge fmBankLodge = new FmBankLodge();
            fmBankLodge.BankLodgeStatement();
        }        
        private void btForeignCurr_Click(object sender, EventArgs e)
        {
            WriteGoto();
            FmForeignCurrency fmForeignCurrency = new FmForeignCurrency();
            fmForeignCurrency.ForeignView();
        }
              
        private void mnRestore_Click(object sender, EventArgs e)
        {
            DialogResult mgUpdate = dialogRestore(ref SqlFilename, IsRoot);
            if (mgUpdate == DialogResult.OK) fmLogin.PreRestore(SqlFilename, fmLogin.NoStartFile);
        }
        private void mnControlPanel_Click(object sender, EventArgs e)
        {
            FmInsatllLogin fmInsatllLogin = new FmInsatllLogin();
            fmInsatllLogin.ShowDialog();
        }
        private void mnLateRoster_Click(object sender, EventArgs e)
        {
            fmLogin.RunLateRoster();
        }
        private void mnBackup_Click(object sender, EventArgs e)
        {            
            if(IsSemiannual == false) BackupPath = fmLogin.AxivDir + "Backup\\" + fmLogin.AppName + "-" + Environment.MachineName + DateTime.Now.ToString("-ddd_dd-MMM-yyyy_HHmmssff");
            else BackupPath = Semiannual;

            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
            FileStream fRestore = new FileStream(BackupPath + "\\Restore.dbax", FileMode.Create, FileAccess.Write);
            if (!Directory.Exists(BackupPath + "\\MainPath")) Directory.CreateDirectory(BackupPath + "\\MainPath");
            DirCopy(fmLogin.MainPath, BackupPath + "\\MainPath", true);
            if (!Directory.Exists(BackupPath + "\\MainDir")) Directory.CreateDirectory(BackupPath + "\\MainDir");
            DirCopy(fmLogin.MainDir, BackupPath + "\\MainDir", true);
            fRestore.Close();
        }
        private void mnEditMsg_Click(object sender, EventArgs e)
        {
            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.EditMessage();
        }
        private void mnErrorMsg_Click(object sender, EventArgs e)
        {
            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.EmailErrorMsg();
        }
        private void mnEmailPad_Click(object sender, EventArgs e)
        {
            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.EmailNew();
        }
        private void mnDuplicate_Click(object sender, EventArgs e)
        {
            string Username = Path.GetFileName(System.Environment.GetEnvironmentVariable("USERPROFILE"));
            SaveFileDialog PixOpen = new SaveFileDialog();
            PixOpen.Filter = "Setup Duplicate File|*.sudf";
            PixOpen.Title = "Save Setup Duplicate File";
            PixOpen.FileName = Username + ".sudf";
            if (PixOpen.ShowDialog() == DialogResult.OK) WriteDuplicateSetupFile(PixOpen.FileName);
            else return;
        }

        private void WriteDuplicateSetupFile(string SaveFilename)
        {
            try
            {
                FileStream fs = new FileStream(fmLogin.EtoFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                FileStream fsop = new FileStream(SaveFilename, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);

                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());

                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());

                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());

                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());

                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());
                swop.WriteLine(sr.ReadLine());

                fs.Close(); sr.Close();
                swop.Flush(); swop.Close();

                File.Copy(fmLogin.SqlFile, Path.GetDirectoryName(SaveFilename) + "\\" + Path.GetFileNameWithoutExtension(SaveFilename) + ".sudb", true);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
        private void FmClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }       
        // QQQQQQQQQ- Restore InputBox  -QQQQQQQQQQQQQ
        string SqlFilename; bool IsRoot;
        public static DialogResult dialogRestore(ref string SqlFilename, bool IsRoot)
        {
            Form form = new Form();
            ComboBox cbBackup = new ComboBox(); ComboBox cbMonth = new ComboBox();
            Button btCancel = new Button(); Button btRestore = new Button();
                        
            btRestore.Text = "Restore"; btRestore.DialogResult = DialogResult.OK;
            btCancel.Text = "Cancel"; btCancel.DialogResult = DialogResult.Ignore;
            cbBackup.SetBounds(20, 10, 80, 25); cbMonth.SetBounds(120, 10, 80, 25);
            btRestore.SetBounds(20, 40, 80, 23); btCancel.SetBounds(120, 40, 80, 23);
            btRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.Text = "Restore DB";
            cbBackup.Text = "SqlAcct";  cbBackup.Items.Add("SqlAcct"); cbBackup.Items.Add("SqlBkl"); cbBackup.Items.Add("SqlCurr"); 
            cbBackup.Items.Add("SqlOffer"); cbBackup.Items.Add("SqlRos"); cbBackup.Items.Add("SqlReg"); cbBackup.Items.Add("SqlRec"); 
            cbBackup.Items.Add("SqlSave"); cbBackup.Items.Add("SqlMonth");
            if (IsRoot == true)
            {
                cbBackup.Items.Add("SqlAll");
                cbBackup.Items.Add("Sql~DB");
            }

            cbMonth.Text = DateTime.Now.ToString("MMM");
            cbMonth.Items.Add("Jan"); cbMonth.Items.Add("Feb"); cbMonth.Items.Add("Mar"); cbMonth.Items.Add("Apr");
            cbMonth.Items.Add("May"); cbMonth.Items.Add("Jun"); cbMonth.Items.Add("Jul"); cbMonth.Items.Add("Aug");
            cbMonth.Items.Add("Sep"); cbMonth.Items.Add("Oct"); cbMonth.Items.Add("Nov"); cbMonth.Items.Add("Dec");

            form.ClientSize = new Size(220, 80);
            form.Controls.AddRange(new Control[] { cbBackup, cbMonth, btCancel, btRestore });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false; form.MaximizeBox = false;

            /*/cbMonth.SelectedIndexChanged += new System.EventHandler(cbMonth_SelectedIndexChanged);
            private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)   
            {
                if (cbMonth.SelectedIndex == 5) { cbMonth.Text = "Months"; cbMonth.Enabled = false; }
                else { cbMonth.Text = DateTime.Now.ToString("MMM"); cbMonth.Enabled = true; }
            }//*/

            DialogResult dialogResult = form.ShowDialog();
            if (cbBackup.Text == "SqlMonth") SqlFilename = "Sql" + cbMonth.Text;
            else SqlFilename = cbBackup.Text;
            return dialogResult;
        }        
         //888888888 End 88888888888888888888      
    }
}