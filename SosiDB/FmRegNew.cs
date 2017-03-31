using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
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
    public partial class FmRegNew : Form
    {
        FmLogin fmLogin; FmUsers fmUsers;
        OpenFileDialog PixOpen;
        PrintPreviewXDialog PreviewXDialog;
        FmInsatllLogin fmInsatllLogin;
        AutoCompleteStringCollection source;
        private string Table = "RegisterInfo",PixName = "None", HasFamily = "No", UserGroup, RegId, PixPath;
        public string strId0; int SelIndex_;
        
        public FmRegNew()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            PreviewXDialog = new PrintPreviewXDialog();
            fmInsatllLogin = new FmInsatllLogin();
            PixPath = fmLogin.PathImage;
            fmLogin.niBackground.Visible = false;
        }
        private void FmRegNew_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Register]-" + fmLogin.strChurchAcronym;
            if (isRoot == false)
            {
                cb1stSelect.SelectedIndex = 0; cb1stSelect_SelectedIndexChanged(sender, e);
                SelectDB();
                dgvEdit.SendToBack();
                dtpDetails.Enabled = false;
                btSaveData.Enabled = false;
                cb2ndSelect.SelectedIndex = 7; cb2ndSelect_SelectedIndexChanged(sender, e);
                tsslbDB.Text = "  | Loaded Database: " + fmLogin.strChurchAcronym + "_DB";
                fmLogin.SavingUpdate("SqlReg", "Loading"); 
            }
            else
            {
                this.Size = new System.Drawing.Size(430, 190);
                this.MinimumSize = new System.Drawing.Size(430, 190);
                this.MaximumSize = new System.Drawing.Size(430, 190);
                gbRootAdmin.Visible = true; btRootSave.Visible = true;
                toolStrip1.Enabled = false;
            }            
        }
        #region // ********** Root User Setup *************************************
        string RootMainPath, RootChurchAcronym, RootConReg, RootTableReg, RootTempFile, RootEditFile, strIdRoot = "1"; bool isRoot = false;
        public void CreateLoginRootUser()
        {
            gbRootAdmin.Visible = true; btRootSave.Visible = true;
            isRoot = true;
            this.ShowDialog();
        }
        private void btValidate_Click(object sender, EventArgs e)
        {
            if (txUsername.Text == "Username") { MessageBox.Show("You have not typed in username!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }            
            if (txPassword.Text != txPWConfirmation.Text) { MessageBox.Show("The Passwords do not match!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            gbRootAdmin.Visible = false;
            this.MinimumSize = new System.Drawing.Size(915, 604);
            this.Size = new System.Drawing.Size(915, 604);

            using (FileStream fsrd = new FileStream(fmInsatllLogin.TempFile, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fsrd);
                RootMainPath = sr.ReadLine();
                RootChurchAcronym = sr.ReadLine();
                RootTableReg = sr.ReadLine();
                RootConReg = sr.ReadLine();
                RootEditFile = sr.ReadLine();
                RootTempFile = sr.ReadLine();
                sr.Close();
            }
            File.Delete(fmInsatllLogin.TempFile);

            // Create "RegisterInfo" Table
            string strDollarCent = "Title VARCHAR(7), First_Name VARCHAR(20), MName VARCHAR(20), Last_Name VARCHAR(20), BirthDay SMALLINT, BirthMonth VARCHAR(10), Age_Group VARCHAR(20), "
                    + "Sex VARCHAR(7), Marital_Status VARCHAR(10), Email VARCHAR(50), MPhone VARCHAR(15), HAdd VARCHAR(50), City VARCHAR(25), State VARCHAR(15), "
                    + "PCode VARCHAR(7), Country VARCHAR(15), Residency VARCHAR(15), Attendees VARCHAR(10), Department VARCHAR(20), Service VARCHAR(25), Status VARCHAR(15), HasFamily VARCHAR(5)";
            string strTbl = "CREATE TABLE IF NOT EXISTS  " + RootTableReg + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date DATE, " + strDollarCent + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(RootConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void btRootSave_Click(object sender, EventArgs e)
        {
            if (CheckInput() == 0) return;
            //Insert Personl/church details
            string Today = dtpDetails.Value.ToString("yyyy-MM-dd");
            string strDollarCent = "Title, First_Name, MName, Last_Name, BirthDay, BirthMonth, Age_Group, Sex, Marital_Status, Email, MPhone, HAdd, City, State, PCode, Country, Residency, Attendees, Department, Service, Status, HasFamily";
            string strtxDollarCent = cbTitle.Text + "', '" + txFName.Text + "', '" + txMName.Text + "', '" + txLName.Text + "', '" + cbDay.Text + "', '" + cbMonth.Text
                                     + "', '" + cbAgeGroup.Text + "', '" + cbSex.Text + "', '" + cbMatrital.Text + "', '" + txEmail.Text + "', '" + txMobileNo.Text
                                     + "', '" + txHAdd.Text + "', '" + txCity.Text + "', '" + cbState.Text + "', '" + txPostCode.Text + "', '" + cbCountry.Text
                                     + "', '" + cbResidency.Text + "', '" + cbAttendees.Text + "', '" + cbDepartment.Text + "', '" + cbService.Text + "', '" + cbStatus.Text + "', '" + HasFamily;
            string query = "INSERT INTO " + Table + " (Date, " + strDollarCent + ") VALUES ('" + Today + "', '" + strtxDollarCent + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(RootConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }            

            FileStream fsop = new FileStream(RootEditFile, FileMode.Append, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + strIdRoot + ") Record created and saved. ");
            swop.Flush(); swop.Close();
            CreateRootUser();
        }                       
        private void CreateRootUser()
        {
            CreateMainLoginTable();
            CreateUser();
            using (FileStream fsopx = new FileStream(RootEditFile, FileMode.Append, FileAccess.Write))
            {
                StreamWriter swopx = new StreamWriter(fsopx);
                swopx.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + strIdRoot + ") Root UserLogin record created.");
                swopx.Flush(); swopx.Close();
            }
            #region // Send RootUser Creation Email
            /*
            string Subject = "Account creation on SosiDB Software of " + RootChurchAcronym + " Church";
            string strDetail = "Beloved " + cbTitle.Text + " " + txFName.Text + " " + txLName.Text + ","
                   + "\n\n"
                   + " A user account has been created for you on SosiDB Software of " + RootChurchAcronym
                   + ". Your usernme is " + txUsername.Text + "."
                   + "\n\nPlease, send a reply email if you did not authorize the creation or do not want the account."
                   + "\n\n"
                   + "May the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
            strDetail += fmLogin.EmailDefaultSignature();
            fmLogin.SendEmailSt(txEmail.Text, "", Subject, strDetail, "", null, "NoneNull"); */
            #endregion 
            //isRoot = false;
            MessageBox.Show("User created successfully", "SosiDB - Root User", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void CreateMainLoginTable()
        {  // Create Table
            string strTbl = "CREATE TABLE IF NOT EXISTS LoginInfo(Id INTEGER PRIMARY KEY AUTOINCREMENT, RegId SMALLINT, Date DATE, Title VARCHAR(7), "
                   + " First_Name VARCHAR(20), Last_Name VARCHAR(20), UserName VARCHAR(10), PassWord VARCHAR(10), Email VARCHAR(50), UserGroup VARCHAR(15));";
            using (SQLiteConnection conTbl = new SQLiteConnection(RootConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void CreateUser()
        {  // Create or insert User
            string Today = DateTime.Today.ToString("yyyy-MM-dd");
            string strDollarCent = "RegId, Date, Title, First_Name, Last_Name, Username, Password, Email, UserGroup";
            string strtxDollarCent = strIdRoot + "', '" + Today + "', '" + cbTitle.Text + "', '" + txFName.Text + "', '" + txLName.Text
                   + "', '" + txUsername.Text + "', '" + txPassword.Text + "', '" + txEmail.Text + "', '" + fmLogin.Rootgroup;
            string query = "INSERT INTO LoginInfo(" + strDollarCent + ") VALUES ('" + strtxDollarCent + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(RootConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        #endregion ***************************************************************************
               
        private void btLoadPix_Click(object sender, EventArgs e)
        {
            PixOpen = new OpenFileDialog();
            PixOpen.Filter = "Pix Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tiff|All Files|*.*";
            PixOpen.Title = "Load Picture";
            if (PixOpen.ShowDialog() == DialogResult.OK) 
            {
                PixName = PixOpen.FileName; pxPix.Load(PixName); 
            }
            else return;
            btLoadPix.Text = "Pix Loaded";
        }
        private void mnLoadDoc_Click(object sender, EventArgs e)
        {
            if (dgvRegister.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvRegister.CurrentRow.Index;
                string Ind0 = dgvRegister.Rows[SelIndex].Cells[0].FormattedValue.ToString();

                PixOpen = new OpenFileDialog();
                PixOpen.Filter = "Doc Files|*.pdf;*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*";
                PixOpen.Title = "Load Worker's Document";
                if (PixOpen.ShowDialog() == DialogResult.OK)
                {
                    string FileExt = Path.GetExtension(PixOpen.FileName);
                    valRetry = true;
                    while (valRetry == true)
                    {
                        if (dialogDoc(ref Answer, ref valRetry) == DialogResult.Yes)
                        {
                            if (File.Exists(fmLogin.PathWorkersDoc + "Doc" + Ind0 + "-" + Answer + FileExt)) 
                            {
                                MessageBox.Show("Filename exists! Change the filename."); 
                                valRetry = true;  
                            }
                        }
                        else valRetry = false;
                    }
                    File.Copy(PixOpen.FileName, fmLogin.PathWorkersDoc + "Doc" + Ind0 + "-" + Answer + FileExt);
                }
                else return;
            }
        }
        private void mnOpenDoc_Click(object sender, EventArgs e)
        {
            if (dgvRegister.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvRegister.CurrentRow.Index;
                string Ind0 = dgvRegister.Rows[SelIndex].Cells[0].FormattedValue.ToString();

                PixOpen = new OpenFileDialog();
                PixOpen.InitialDirectory = fmLogin.PathWorkersDoc;
                PixOpen.Filter = "Doc Files|Doc" + Ind0 + "-*.*";
                PixOpen.Title = "Open Document";
                if (PixOpen.ShowDialog() == DialogResult.OK)
                {
                    ProcessStartInfo pi = new ProcessStartInfo(PixOpen.FileName);
                    Process.Start(pi);
                }
                else return;
            }
        }        
                
        private void InsertDB()
        {    //Insert Personl/church details
            string Today = dtpDetails.Value.ToString("yyyy-MM-dd");
            string strDollarCent = "Title, First_Name, MName, Last_Name, BirthDay, BirthMonth, Age_Group, Sex, Marital_Status, Email, MPhone, HAdd, City, State, PCode, Country, Residency, Attendees, Department, Service, Status, HasFamily";
            string strtxDollarCent = cbTitle.Text + "', '" + txFName.Text + "', '" + txMName.Text + "', '" + txLName.Text + "', '" + cbDay.Text + "', '" + cbMonth.Text 
                                     + "', '" + cbAgeGroup.Text + "', '" + cbSex.Text + "', '" + cbMatrital.Text + "', '" + txEmail.Text + "', '" + txMobileNo.Text 
                                     + "', '" + txHAdd.Text  + "', '" + txCity.Text  + "', '" + cbState.Text  + "', '" + txPostCode.Text + "', '" + cbCountry.Text
                                     + "', '" + cbResidency.Text + "', '" + cbAttendees.Text + "', '" + cbDepartment.Text + "', '" + cbService.Text + "', '" + cbStatus.Text + "', '" + HasFamily;
            string query = "INSERT INTO " + Table + " (Date, " + strDollarCent + ") VALUES ('" + Today + "', '" + strtxDollarCent + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            btSaveData.Enabled = false;
            if (btLoadPix.Text == "Pix Loaded") File.Copy(PixOpen.FileName, PixPath + "Pix-" + MaxId());
            else File.Copy(PixPath + "Pix-0", PixPath + "Pix-" + MaxId());
        }
        private void UpdateDB()
        {//UPDATE 
            string strtxDollarCent = "Title = '" + cbTitle.Text + "', First_Name = '" + txFName.Text + "', MName = '" + txMName.Text 
                   + "', Last_Name = '" + txLName.Text + "', BirthDay = '" + cbDay.Text + "', BirthMonth = '" + cbMonth.Text 
                   + "', Age_Group = '" + cbAgeGroup.Text + "', Sex = '" + cbSex.Text + "', Marital_Status = '" + cbMatrital.Text 
                   + "', Email = '" + txEmail.Text + "',  MPhone = '" + txMobileNo.Text + "', HAdd = '" + txHAdd.Text + "', City = '" + txCity.Text 
                   + "', State = '" + cbState.Text + "', PCode = '" + txPostCode.Text + "', Country = '" + cbCountry.Text + "', Residency = '" + cbResidency.Text 
                   + "', Attendees = '" + cbAttendees.Text + "', Department = '" + cbDepartment.Text + "', Service = '" + cbService.Text + "',  Status = '" + cbStatus.Text+ "',  HasFamily = '" + HasFamily + "'"; 
            string query = "UPDATE " + Table + " SET  " + strtxDollarCent + " WHERE Id = '" + strId0 + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
                       
            if (PixName != "None")
            {
                if(File.Exists(PixPath + "Pix-" + strId0))File.Delete(PixPath + "Pix-" + strId0);
                File.Copy(PixOpen.FileName, PixPath + "Pix-" + strId0);
            }
            SelectDB();
        }
        private void UpdateBank(string Idm, string Idt)
        {//UPDATE  
            string query = "UPDATE AccountInfo SET Custodian = 'BankLodge', OtherTable = 'BankLodgeInfo', OtherId = '" + Idt + "' WHERE Id = '" + Idm + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void SelectDB()
        { // Select 
            string queryx = "";  
            if (cb1stSelect.SelectedIndex == 1) queryx = "SELECT Id, Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate FROM " + Table + " WHERE BirthMonth = '" + cb2ndSelect.Text + "';";
            else 
            {//New-Comer, Regular, Member, ChildMember, Visitor, Associate, Former, RegMemb, RegMemb+1st, All
                if (cb1stSelect.SelectedIndex == 0 && cb2ndSelect.SelectedIndex == 7) queryx = "SELECT Id, Date, First_Name, Last_Name, " + cb1stSelect.Text + ", BirthDay || BirthMonth AS Birthdate, MPhone, Email FROM " + Table + " WHERE " + cb1stSelect.Text + " = 'Regular' OR " + cb1stSelect.Text + " = 'Member';"; //RegMemb
                else if (cb1stSelect.SelectedIndex == 0 && cb2ndSelect.SelectedIndex == 8) queryx = "SELECT Id, Date, First_Name, Last_Name, " + cb1stSelect.Text + ", BirthDay || BirthMonth AS Birthdate FROM " + Table + " WHERE " + cb1stSelect.Text + " = 'Regular' OR " + cb1stSelect.Text + " = 'Member' OR "
                        + cb1stSelect.Text + " = 'New-Comer';"; // RegMemb+1st = RegMemb + new-comer
                     else if (cb1stSelect.SelectedIndex == 0 && cb2ndSelect.SelectedIndex == 9) queryx = "SELECT Id, Date, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate, MPhone, Email FROM " + Table; // all i.e all RegisterInfo data
                     else queryx = "SELECT Id, Date, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate FROM " + Table + " WHERE " + cb1stSelect.Text + " = '" + cb2ndSelect.Text + "';";// New-comer, Rg , Memb, ChildMemb, former & vistor
            }
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                dgvRegister.DataSource = dTable;
            }
            dgvRegister.Columns[0].Visible = false; 
            dgvRegister.Columns[1].Visible = false;
            dgvRegister.Columns[1].Width = 85;
            dgvRegister.Columns[2].Width = 115;
            dgvRegister.Columns[3].Width = 115;
            dgvRegister.Columns[4].Width = 120;
            
            #region Set Autocomplete
            txNameSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            txNameSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            source = new AutoCompleteStringCollection(); //source. 
            if (cbNameSearch.SelectedIndex == 0) for (int i = 0; i < dgvRegister.Rows.Count; i++) source.Add(dgvRegister.Rows[i].Cells[2].FormattedValue.ToString());
            else if (cbNameSearch.SelectedIndex == 1) for (int i = 0; i < dgvRegister.Rows.Count; i++) source.Add(dgvRegister.Rows[i].Cells[3].FormattedValue.ToString());
            else if (cbNameSearch.SelectedIndex == 2) for (int i = 0; i < dgvRegister.Rows.Count; i++) source.Add(dgvRegister.Rows[i].Cells[5].FormattedValue.ToString());
            else for (int i = 0; i < dgvRegister.Rows.Count; i++) source.Add(dgvRegister.Rows[i].Cells[6].FormattedValue.ToString());
                        
            txNameSearch.AutoCompleteCustomSource = source;
            #endregion

            tsslb1.Text = "Dgv Records: " + dgvRegister.Rows.Count + " | Total records: " + MaxId() + " | Deleted records: " + (MaxId()-Count()).ToString() + " | Saved records: " + Count().ToString();
        }
        private int MaxId ()
        {
           string strPixId = "SELECT Id FROM "  + Table + " ORDER BY Id DESC LIMIT 1";
            int intMaxId;
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strPixId, conPix))
                {
                    if (cmd.ExecuteScalar() != DBNull.Value) intMaxId = Convert.ToInt32(cmd.ExecuteScalar());
                    else intMaxId = 0;
                }
            }
            return intMaxId;
        }
        private int Count()
        {
            string strCount = "SELECT COUNT(*) FROM " + Table;
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
        private void cb1stSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb1stSelect.SelectedIndex == 0)
            {
                object[] obj = new object[cbAttendees.Items.Count]; cbAttendees.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj);
                cb2ndSelect.Items.Add("RegMemb"); cb2ndSelect.Items.Add("RegMemb+1st"); cb2ndSelect.Items.Add("All");//regular 
                cb2ndSelect.SelectedIndex = 7;
            }
            if (cb1stSelect.SelectedIndex == 1)
            {
                object[] obj = new object[cbMonth.Items.Count]; cbMonth.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = int.Parse(DateTime.Now.ToString("MM"))-1;
            }
            if (cb1stSelect.SelectedIndex == 2)
            {
                object[] obj = new object[cbAgeGroup.Items.Count]; cbAgeGroup.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }
            if (cb1stSelect.SelectedIndex == 3)
            {
                object[] obj = new object[cbSex.Items.Count]; cbSex.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }
            if (cb1stSelect.SelectedIndex == 4)
            {
                object[] obj = new object[cbMatrital.Items.Count]; cbMatrital.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }
            if (cb1stSelect.SelectedIndex == 5)
            {
                object[] obj = new object[cbResidency.Items.Count]; cbResidency.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }
            if (cb1stSelect.SelectedIndex == 6)
            {
                object[] obj = new object[cbDepartment.Items.Count]; cbDepartment.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }
            if (cb1stSelect.SelectedIndex == 7)
            {
                object[] obj = new object[cbService.Items.Count]; cbService.Items.CopyTo(obj, 0);
                cb2ndSelect.Items.Clear(); cb2ndSelect.Items.AddRange(obj); cb2ndSelect.SelectedIndex = 0;
            }

            if (cb1stSelect.SelectedIndex == 1) mnBirthdayEmail.Visible = true; else mnBirthdayEmail.Visible = false;          
        }
        private void cb2ndSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectDB();
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            if (CheckInput() == 0) return;
            if (Count() > 0)
            {
                string strCheckDuplic = CheckDuplicate();
                if (strCheckDuplic != "null" && btSaveData.Text == "Save Data")
                {
                    DialogResult mgSaveDuplicate = MessageBox.Show(strCheckDuplic + "\n\nWould still you like to continue with saving the record?", "Record Duplicating Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (mgSaveDuplicate == DialogResult.No) return;
                }
            }

            DialogResult mgSaveUpdate = MessageBox.Show("Are the input records correct and do you want to continue?", "Record Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgSaveUpdate == DialogResult.No) { btSaveData.Enabled = true; return; }
            else btSaveData.Enabled = false;

            if (btSaveData.Text == "Save Data")
            {
                InsertDB();
                SelectDB();

                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + MaxId().ToString() + ") Record created and saved. ");
                swop.Flush(); swop.Close();


                DialogResult mgUpdate = MessageBox.Show("Data Saved. Do you want to start a new record?", "Record Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes) Clear();
                cb1stSelect.SelectedIndex = 0; cb2ndSelect.SelectedIndex = 8;
                SelIndex_ = dgvRegister.Rows.Count - 1;
                dgvRegister.CurrentCell = dgvRegister.Rows[SelIndex_].Cells[2];
            }
            else
            {
                UpdateDB();
                SelectDB();

                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + strId0 + ") Record updated, ");
                swop.Flush(); swop.Close();

                DialogResult mgUpdate = MessageBox.Show("Data updated. Do you want to start a new record?", "Record Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes) Clear();
                dgvRegister.CurrentCell = dgvRegister.Rows[SelIndex_].Cells[2];// SelIndex_;
            }
            dtpDetails.Enabled = false;
            tsslb1.Text = " Total records: " + MaxId() + " | Deleted records: " + (MaxId() - Count()).ToString() + " | Saved records: " + Count().ToString();
            fmLogin.SavingUpdate("SqlReg", "Saving");
        }
        private void tssbtEdit_ButtonClick(object sender, EventArgs e)
        {
            PixName = "None"; btLoadPix.Text = "Load  Pix";
            if (dgvRegister.CurrentRow == null) { MessageBox.Show("Create or select a person's record you want to get the RegID!"); return; }           
            else
            {
                SelIndex_ = dgvRegister.CurrentRow.Index;
                strId0 = dgvRegister.Rows[SelIndex_].Cells[0].FormattedValue.ToString();
                string queryx = "SELECT * FROM " + Table + " WHERE Id = '" + strId0 + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    DataTable dTable = new DataTable();
                    MyAdapter.SelectCommand = cmdOfferingx;
                    MyAdapter.Fill(dTable);
                    dgvEdit.DataSource = dTable;
                }

                cbTitle.Text = dgvEdit.Rows[0].Cells[2].FormattedValue.ToString();
                txFName.Text = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString();
                txMName.Text = dgvEdit.Rows[0].Cells[4].FormattedValue.ToString();
                txLName.Text = dgvEdit.Rows[0].Cells[5].FormattedValue.ToString();

                cbDay.Text = dgvEdit.Rows[0].Cells[6].FormattedValue.ToString();
                cbMonth.Text = dgvEdit.Rows[0].Cells[7].FormattedValue.ToString();
                cbAgeGroup.Text = dgvEdit.Rows[0].Cells[8].FormattedValue.ToString();
                cbSex.Text = dgvEdit.Rows[0].Cells[9].FormattedValue.ToString();
                cbMatrital.Text = dgvEdit.Rows[0].Cells[10].FormattedValue.ToString();

                txEmail.Text = dgvEdit.Rows[0].Cells[11].FormattedValue.ToString();
                txMobileNo.Text = dgvEdit.Rows[0].Cells[12].FormattedValue.ToString();
                txHAdd.Text = dgvEdit.Rows[0].Cells[13].FormattedValue.ToString();
                txCity.Text = dgvEdit.Rows[0].Cells[14].FormattedValue.ToString();

                cbState.Text = dgvEdit.Rows[0].Cells[15].FormattedValue.ToString();
                txPostCode.Text = dgvEdit.Rows[0].Cells[16].FormattedValue.ToString();
                cbCountry.Text = dgvEdit.Rows[0].Cells[17].FormattedValue.ToString();
                cbResidency.Text = dgvEdit.Rows[0].Cells[18].FormattedValue.ToString();

                cbAttendees.Text = dgvEdit.Rows[0].Cells[19].FormattedValue.ToString();
                cbDepartment.Text = dgvEdit.Rows[0].Cells[20].FormattedValue.ToString();
                cbService.Text = dgvEdit.Rows[0].Cells[21].FormattedValue.ToString();
                cbStatus.Text = dgvEdit.Rows[0].Cells[22].FormattedValue.ToString();
                HasFamily = dgvEdit.Rows[0].Cells[23].FormattedValue.ToString();

                pxPix.Load(PixPath + "Pix-" + strId0);
                btSaveData.Enabled = false;
                gbPersonalDetails.Enabled = false; gbChurchDeatails.Enabled = false;
                btSaveData.Enabled = false; btCancel.Enabled = false; btClear.Enabled = false; btLoadPix.Enabled = false;
            }
        }
        private void mnEditData_Click(object sender, EventArgs e)
        {
            tssbtEdit_ButtonClick(sender, e);
            btSaveData.Text = "Update Data"; btSaveData.Enabled = true;
            btCancel.Enabled = true; btClear.Enabled = true; btLoadPix.Enabled = true;
            gbPersonalDetails.Enabled = true; gbChurchDeatails.Enabled = true;
        }
        private void tsbtNew_Click(object sender, EventArgs e)
        {
            btSaveData.Text = "Save Data"; btSaveData.Enabled = true;
            btCancel.Enabled = true; btClear.Enabled = true; btLoadPix.Enabled = true;
            gbPersonalDetails.Enabled = true; gbChurchDeatails.Enabled = true;
        }
        private void mnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRegister.CurrentRow == null) return;
            else
            {
                DialogResult mgSaveUpdate = MessageBox.Show("Are sure you want to delete this data from the DB?", "Delete Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgSaveUpdate == DialogResult.No) return;

                int SelIndex = dgvRegister.CurrentRow.Index;
                string strId = dgvRegister.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                string queryx = "DELETE FROM " + Table + " WHERE Id = '" + strId + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    conTblx.Open();
                    cmdOfferingx.ExecuteNonQuery();
                }
                dgvRegister.Rows.RemoveAt(SelIndex);
                dgvRegister.Refresh();

                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + SelIndex + ") Offering record deleted");
                swop.Flush(); swop.Close();
            }
        }        
        private void btCancel_Click(object sender, EventArgs e)
        {
           //ModUpdate(); 
           this.Close();
        }
        private void Clear()
        {
            cbTitle.Text = "Nonex"; txFName.Text = "First Name"; txMName.Text = "Middle Name"; txLName.Text  = "Surname Name";
            cbDay.Text = "Day"; cbMonth.Text = "Month";  cbSex.Text = "Sex"; cbMatrital.Text = "Marital Status"; txEmail.Text = "E-mail Address"; 
            txMobileNo.Text = "Mobile No"; txHAdd.Text = "House No & Street Name"; txCity.Text = "City/Suburb"; cbState.Text = "State"; 
            txPostCode.Text = "Post Code"; cbCountry.Text = "Country"; cbResidency.Text = "Resident Status"; cbAgeGroup.Text = "Age Group"; 
            cbAttendees.Text = "Attendees"; cbDepartment.Text = "Department"; cbService.Text = "Service"; cbStatus.Text = "Status"; HasFamily = "No";
            pxPix.Load(PixPath + "Pix-0");
        }
        private void btClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void mnCreateUser_Click(object sender, EventArgs e)
        {
            if (dgvRegister.CurrentRow == null) { MessageBox.Show("Create or select a person's record to create userLogin account for!"); return; }
            else
            {
                tssbtEdit_ButtonClick(sender, e);
                try
                {
                    FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine(strId0);
                    swop.WriteLine(cbTitle.Text);
                    swop.WriteLine(txFName.Text);
                    swop.WriteLine(txLName.Text);
                    swop.WriteLine(txEmail.Text);
                    swop.Flush(); swop.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                btSaveData.Text = "Save Data"; btSaveData.Enabled = false;
                fmUsers = new FmUsers();
                fmUsers.CreateLoginUser();
            }
        }
        private void mnChangeLoginUser_Click(object sender, EventArgs e)
        {
            FmChangeLogin fmChangeLogin = new FmChangeLogin();
            fmChangeLogin.ShowDialog();
        }
        private void mnViewUser_Click(object sender, EventArgs e)
        {
            fmUsers = new FmUsers();
            fmUsers.ViewUser(); 
        }
        private void mnRegId_Click(object sender, EventArgs e)
        {
            if (dgvRegister.CurrentRow == null) { MessageBox.Show("Create or select a person's record you want to get the RegID!"); return; }
            else if (dgvRegister.Rows[dgvRegister.CurrentRow.Index].Cells[4].FormattedValue.ToString() == "New-Comer" ||
                     dgvRegister.Rows[dgvRegister.CurrentRow.Index].Cells[4].FormattedValue.ToString() == "Visitor")
            {
                FmFirstTimer fmFirstTimer = new FmFirstTimer();
                fmFirstTimer.ShowDialog();
            }
            else
            {
                int SelIndex = dgvRegister.CurrentRow.Index;
                string strRegId = dgvRegister.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                MessageBox.Show("RegID: " + strRegId);
            }
        }
        private void FmClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void mnFirstTimer_Click(object sender, EventArgs e)
        {
            FmFirstTimer fmFirstTimer = new FmFirstTimer();
            fmFirstTimer.ShowDialog();

            /*/cb1stSelect.SelectedIndex = 0; cb2ndSelect.SelectedIndex = 0;
            SelectDB();
            int index = dgvRegister.Rows.Count - 1;
            dgvRegister.FirstDisplayedScrollingRowIndex = index;
            dgvRegister.Refresh();
            dgvRegister.CurrentCell = dgvRegister.Rows[index].Cells[1];
            dgvRegister.Rows[index].Selected = true;
            mnEditData_Click(sender, e);//*/
        }       
        private void ReadUserGroup()
        {
            FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            UserGroup = sr.ReadLine();
            RegId = sr.ReadLine();
            fs.Close(); sr.Close();
        }
        private void FmRegNew_Activated(object sender, EventArgs e)
        {
           if(isRoot == false) ReadUserGroup();
           if (UserGroup == fmLogin.Admingroup || UserGroup == fmLogin.Rootgroup)
            {
                mnEditData.Visible = true;
                mnDelete.Visible = true;
                mnCreateUser.Visible = true;
                mnViewUser.Visible = true;
                mnEditMessage.Visible = true;
            }
            else
            {
                mnEditData.Visible = false;
                mnDelete.Visible = false;
                mnCreateUser.Visible = false;
                mnViewUser.Visible = false;
                mnEditMessage.Visible = false;
            }
        }
        private int CheckInput()
        {
            if (cbTitle.Text == "Nonex") { MessageBox.Show("You have not selected any value for title."); cbTitle.Focus(); return 0; }
            if (txFName.Text == "First Name") { MessageBox.Show("You have not typed in your first name."); txFName.Focus(); return 0; }
            if (txMName.Text == "Middle Name") { MessageBox.Show("You have not typed in your middle name."); txMName.Text = "***"; txMName.Focus(); return 0; }
            if (txLName.Text == "Surname Name") { MessageBox.Show("You have not typed in your surname(last) name."); txLName.Focus(); return 0; }

            if (cbDay.Text == "Day") { MessageBox.Show("You have not selected your birthday."); cbDay.Focus(); return 0; }
            if (cbMonth.Text == "Month") { MessageBox.Show("You have not selected your birthmonth."); cbMonth.Focus(); return 0; }
            if (cbAgeGroup.Text == "Age Group") { MessageBox.Show("You have not selected your age group."); cbAgeGroup.Focus(); return 0; }
            if (cbSex.Text == "Sex") { MessageBox.Show("You have not selected your sex."); cbSex.Focus(); return 0; }            
           
            if (txEmail.Text == "E-mail Address") { MessageBox.Show("You have not typed in your email address."); txEmail.Text = "No E-mail"; txEmail.Focus(); return 0; }
            if (txMobileNo.Text == "Mobile No") { MessageBox.Show("You have not not typed in your mobile phone number."); txMobileNo.Focus(); return 0; } 
            if (cbMatrital.Text == "Marital Status") { MessageBox.Show("You have not selected your marital status."); cbMatrital.Focus(); return 0; }
            if (txHAdd.Text == "House No & Street Name") { MessageBox.Show("You have not typed in your home address."); txHAdd.Focus(); return 0; }
            if (txCity.Text == "City/Suburb") { MessageBox.Show("You have not typed in your city."); txCity.Focus(); return 0; }
            if (cbState.Text == "State") { MessageBox.Show("You have not selected your state."); cbState.Focus(); return 0; }
            if (txPostCode.Text == "Post Code") { MessageBox.Show("You have not typed in your post code."); txPostCode.Text = "000"; txPostCode.Focus(); return 0; }
            if (cbCountry.Text == "Country") { MessageBox.Show("You have not selected your country."); cbCountry.Focus(); return 0; }
            if (cbResidency.Text == "Resident Status") { MessageBox.Show("You have not selected your resident status."); cbResidency.Focus(); return 0; }
           
            if (cbAttendees.Text == "Attendees") { MessageBox.Show("You have not selected your Attendee class."); cbAttendees.Focus(); return 0; }
            if (cbDepartment.Text == "Department") { MessageBox.Show("You have not selected your department."); cbDepartment.Focus(); return 0; }
            if (cbService.Text == "Service") { MessageBox.Show("You have not selected your service."); cbService.Focus(); return 0; }
            if (cbStatus.Text == "Status") { MessageBox.Show("You have not selected your spiritual status."); cbStatus.Focus(); return 0; }
            
            if (btLoadPix.Text == "Load Pix")
            {
                MessageBox.Show("You have not loaded your pix."); pxPix.Load(PixPath + "Pix-0");
                btLoadPix.Text = "Load  Pix"; btLoadPix.Focus(); return 0;
            }
            return 1;
        }
        private string CheckDuplicate()
        {   // Select 
            string queryx = "SELECT First_Name, Last_Name, MPhone, Email FROM " + Table;
            string strReport = "";
            DataTable dTable = new DataTable();
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }
            if (txFName.Text == dTable.Rows[0][0].ToString()) strReport = strReport + ", First Name: " + dTable.Rows[0][0].ToString();
            if (txLName.Text == dTable.Rows[0][1].ToString()) strReport = strReport + ", Last Name: " + dTable.Rows[0][1].ToString();
            if (txMobileNo.Text == dTable.Rows[0][2].ToString()) strReport = strReport + ", Mobile: " + dTable.Rows[0][2].ToString();
            if (txEmail.Text == dTable.Rows[0][3].ToString()) strReport = strReport + ", Email: " + dTable.Rows[0][3].ToString();

            if (strReport == "") { return "null"; }
            else
            {
                strReport = strReport.Substring(2, strReport.Length - 2);
                strReport = "There is a record with th(is/ese) field(s) [" + strReport + "] already in the database.";
                return strReport;
            }
        }
        
     private void mnEmailSMS_Click(object sender, EventArgs e)
        {
            try
            {
                int Reg = 0;
                string condn = "",  Email = "", Phone = "";

                for (int i = 0; i < dgvRegister.Rows.Count; i++)
                {
                    Reg++;
                    if (Reg == 1) condn = "SELECT GROUP_CONCAT(Email, ';'), GROUP_CONCAT(MPhone, ';') FROM RegisterInfo WHERE ";
                    if (Reg < dgvRegister.Rows.Count) condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "' OR ";
                    else condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "';";
                }

                if (dgvRegister.Rows.Count > 0)
                {
                    DataTable dTable = new DataTable();
                    string queryk = condn;
                    using (SQLiteConnection conTblk = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdRegister = new SQLiteCommand(queryk, conTblk);
                        SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                        MyAdapter.SelectCommand = cmdRegister;
                        MyAdapter.Fill(dTable);
                    }
                    Email = dTable.Rows[0][0].ToString();
                    Phone = dTable.Rows[0][1].ToString();
                }

                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("Other Email for " + cb2ndSelect.Text);
                swop.WriteLine(Email);
                swop.WriteLine(Phone);
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); } //*/
            FmEmailPad fmEmailPad = new FmEmailPad();
            fmEmailPad.EmailSMS();
        }
     private void mnEmailSMSPlus_Click(object sender, EventArgs e)
     {
         try
         {
             int Reg = 0;
             string condn = "", value = "";

             for (int i = 0; i < dgvRegister.Rows.Count; i++)
             {                 
                     Reg++;
                     if (Reg == 1) condn = "SELECT GROUP_CONCAT(Title, ';'), GROUP_CONCAT(First_Name, ';'), GROUP_CONCAT(MName, ';'), GROUP_CONCAT(Last_Name, ';'), "
                          + "GROUP_CONCAT(Email, ';'), GROUP_CONCAT(MPhone, ';'), GROUP_CONCAT(BirthDay || BirthMonth || ', Age group: ' ||  Age_Group, ';'), "
                          + "GROUP_CONCAT(HAdd || ' ' || City || ' ' || State || ' ' || PCode || ' ' || Country, ';'), "
                          + "GROUP_CONCAT('Sex: ' || Sex || ', Marital Status: ' || Marital_Status, ';'), GROUP_CONCAT(Attendees, ';') FROM RegisterInfo WHERE ";
                     if (Reg < dgvRegister.Rows.Count) condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "' OR ";
                     else condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "';";
                
             }

             if (dgvRegister.Rows.Count > 0)
             {
                 DataTable dTable = new DataTable();
                 string queryk = condn;
                 using (SQLiteConnection conTblk = new SQLiteConnection(fmLogin.strConReg))
                 {
                     SQLiteCommand cmdRegister = new SQLiteCommand(queryk, conTblk);
                     SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                     MyAdapter.SelectCommand = cmdRegister;
                     MyAdapter.Fill(dTable);
                 }
                 value =
                      dTable.Rows[0][0].ToString() + "?" + dTable.Rows[0][1].ToString() + "?" + dTable.Rows[0][2].ToString() + "?" +
                      dTable.Rows[0][3].ToString() + "?" + dTable.Rows[0][4].ToString() + "?" + dTable.Rows[0][5].ToString() + "?" +
                      dTable.Rows[0][6].ToString() + "?" + dTable.Rows[0][7].ToString() + "?" + dTable.Rows[0][8].ToString();
             }
                      
             FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
             StreamWriter swop = new StreamWriter(fsop);
             swop.WriteLine("Other Email for " + cb2ndSelect.Text);
             swop.WriteLine(value);
             swop.Flush(); swop.Close();
         }
         catch (Exception ex) { MessageBox.Show(ex.ToString()); }

         FmEmailPad fmEmailPad = new FmEmailPad();
         fmEmailPad.EmailSMSPlus();
     }
     private void mnRegMembUpdate_Click(object sender, EventArgs e)
     {
         try
         {
             int Reg = 0;
             string condn = "", value = "";

             for (int i = 0; i < dgvRegister.Rows.Count; i++)
             {
                 Reg++;
                 if (Reg == 1) condn = "SELECT GROUP_CONCAT(Id, ';'), GROUP_CONCAT(Title, ';'), GROUP_CONCAT(First_Name || ' ' || Last_Name, ';'), "
                      + "GROUP_CONCAT(Email, ';'), GROUP_CONCAT(MPhone, ';'), GROUP_CONCAT(BirthDay || BirthMonth || ', Age group: ' ||  Age_Group, ';'), "
                      + "GROUP_CONCAT(HAdd || ' ' || City || ' ' || State || ' ' || PCode || ' ' || Country, ';'), "
                      + "GROUP_CONCAT('Sex: ' || Sex || ', Marital Status: ' || Marital_Status, ';') FROM RegisterInfo WHERE ";
                 if (Reg < dgvRegister.Rows.Count) condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "' OR ";
                 else condn = condn + "Id = '" + dgvRegister.Rows[i].Cells[0].FormattedValue.ToString() + "';";
             }

             if (dgvRegister.Rows.Count > 0)
             {
                 DataTable dTable = new DataTable();
                 string queryk = condn;
                 using (SQLiteConnection conTblk = new SQLiteConnection(fmLogin.strConReg))
                 {
                     SQLiteCommand cmdRegister = new SQLiteCommand(queryk, conTblk);
                     SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                     MyAdapter.SelectCommand = cmdRegister;
                     MyAdapter.Fill(dTable);
                 }
                 value =
                      dTable.Rows[0][0].ToString() + "?" + dTable.Rows[0][1].ToString() + "?" + dTable.Rows[0][2].ToString() + "?" +
                      dTable.Rows[0][3].ToString() + "?" + dTable.Rows[0][4].ToString() + "?" + dTable.Rows[0][5].ToString() + "?" +
                      dTable.Rows[0][6].ToString() + "?" + dTable.Rows[0][7].ToString();
             }

             FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
             StreamWriter swop = new StreamWriter(fsop);
             swop.WriteLine("Other Email for " + cb2ndSelect.Text);
             swop.WriteLine(value);
             swop.Flush(); swop.Close();
         }
         catch (Exception ex) { MessageBox.Show(ex.ToString()); }

         FmEmailPad fmEmailPad = new FmEmailPad();
         fmEmailPad.EmailRegMembUpdate();
     }
     private void mnBirthdayEmail_Click(object sender, EventArgs e)
     {
         try
         {
             string EmailTo = "";
             string Birthmonth = " " + DateTime.Now.ToString("MMMM"), Birthday = DateTime.Now.Day.ToString();
             string strTitle = "", strFName = "", strMName = "", strLName = "", MphoneNo = "", SMSMessage;
             string queryx = "SELECT GROUP_CONCAT(Title, ';'), GROUP_CONCAT(First_Name, ';'), GROUP_CONCAT(Last_Name, ';'), "
                           + "GROUP_CONCAT(Email, ';'), GROUP_CONCAT(MPhone, ';') FROM RegisterInfo WHERE BirthMonth = '" + Birthmonth + "' AND BirthDay = '" + Birthday + "';";
             using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
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

             FileStream fs = new FileStream(fmLogin.PathMsg + "BirthdateEmailMessage.msgx", FileMode.Open);
             StreamReader sr = new StreamReader(fs);
             string EmailSubject = sr.ReadLine();
             sr.ReadLine();
             string EmailBody = sr.ReadToEnd();
             fs.Close(); sr.Close();
             EmailBody = EmailBody.Replace("#Birthmonth", "#Para1");
             EmailBody = EmailBody.Replace("#Birthday", "#Para2");
             //EmailBody += fmLogin.EmailDefaultSignature();
             fmLogin.SendEmailDt("2", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull, strTitle, strFName, strMName, strLName, Birthmonth, Birthday, "");

             FileStream fsx = new FileStream(fmLogin.PathMsg + "BirthdateSMSMessage.msgx", FileMode.Open);
             StreamReader srx = new StreamReader(fsx);
             SMSMessage = srx.ReadToEnd();
             fsx.Close(); srx.Close();
             fmLogin.SendSMSDt(MphoneNo, SMSMessage, "", strTitle, strFName, strMName, strLName, Birthday, "", "", fmLogin.InternetPhoneSMS);

             MessageBox.Show("Birthday email and SMS sent.");
         }
         catch (Exception ex) { MessageBox.Show(ex.ToString()); }
     }

     private void mnEditMessage_Click(object sender, EventArgs e)
     {
         FmEmailPad fmEmailPad = new FmEmailPad();
         fmEmailPad.EditMessage();
     }
     private void mnCheckCreditSms_Click(object sender, EventArgs e)
     {
         if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
         {
             try
             {
                 string userName = "rccgfolp"; // put your user name 	
                 string Key = "95BFFA59-7F75-20C3-348A-74FF83FCB728";  // unique key here
                 // attempt to send message
                 CreditRespondsData _Result = ClickSendSMS.CheckCredit(userName, Key);
                 // load result
                 MessageBox.Show(" ClickSend SMS prepaid balance = " + _Result.Value);// ResultBalance = _Data.Value; ResultType 
             }
             catch (Exception ex) { MessageBox.Show(ex.Message); }
         }
         else MessageBox.Show("Internet connection is not available. \nCheck back when the internet is available?", "Internet Availability", MessageBoxButtons.OK, MessageBoxIcon.Information);
     }
     private void mnPrintData_Click(object sender, EventArgs e)
     {
         dgvRegister.Columns[1].Visible = false;
         SelectDB();

         string DocName = cb1stSelect.Text + "(" + cb2ndSelect.Text + ") Register-" + DateTime.Now.ToString("ddMMyy");
         string AdHeader = fmLogin.strChurchAcronym + "-" + DocName;
         PreviewXDialog.DGVPdfPrint(dgvRegister, AdHeader, DocName, false);
         PreviewXDialog.PrintDGVX(dgvRegister, AdHeader, DocName, false);

         dgvRegister.Columns[1].Visible = true;
         SelectDB();
     }    
    
     bool valRetry; string Answer;// QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ
     public static DialogResult dialogDoc(ref string Answer, ref bool valRetry)
     {
         Form form = new Form();
         Label lbInfo = new Label();
         TextBox txEmailTo = new TextBox();
         Button buttonYes = new Button();
         Button buttonNo = new Button();

         form.Text = " Filename";
         lbInfo.Text = "Type in the filename here.";
         txEmailTo.Text = "Filename";
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
         if (txEmailTo.Text != "Filename")
         {
             if (form.DialogResult == DialogResult.Yes) Answer = txEmailTo.Text;
             valRetry = false;
             return dialogResult;
         }
         else
         {
             valRetry = true;
             if (dialogResult == DialogResult.Yes) MessageBox.Show("You have not typed in a filename.");
             return dialogResult;
         }
     }
     private void mnRemove_Click(object sender, EventArgs e)
     {
         if (dgvRegister.CurrentRow == null) return;
         else
         {
             DialogResult mgSaveUpdate = MessageBox.Show("Are sure you want to remove this data from the list?", "Data Removal Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
             if (mgSaveUpdate == DialogResult.No) return; 

             int SelIndex = dgvRegister.CurrentRow.Index;
             string strId = dgvRegister.Rows[SelIndex].Cells[0].FormattedValue.ToString();
            
             dgvRegister.Rows.RemoveAt(SelIndex);
             dgvRegister.Refresh();
         }
     }
     private void cbNameSearch_SelectedIndexChanged(object sender, EventArgs e)
     {
         cb1stSelect.SelectedIndex = 0;
         cb2ndSelect.SelectedIndex = 9;
         SelectDB();
     }    
     private void txNameSearch_TextChanged(object sender, EventArgs e)
     {
         if (source.Contains(txNameSearch.Text) == true)
         { 
             string queryx = "";
             if (cbNameSearch.SelectedIndex == 0) queryx = "SELECT Id, Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate,  MPhone, Email FROM " + Table + " WHERE First_Name = '" + txNameSearch.Text + "';";
             else if (cbNameSearch.SelectedIndex == 1) queryx = "SELECT Id, Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate, MPhone, Email FROM " + Table + " WHERE Last_Name = '" + txNameSearch.Text + "';";
             else if (cbNameSearch.SelectedIndex == 2) queryx = "SELECT Id, Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate, MPhone, Email FROM " + Table + " WHERE MPhone = '" + txNameSearch.Text + "';";
             else queryx = "SELECT Id, Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate, MPhone, Email FROM " + Table + " WHERE Email = '" + txNameSearch.Text + "';";
             

             using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
             {
                 SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                 SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                 DataTable dTable = new DataTable();
                 MyAdapter.SelectCommand = cmdRegister;
                 MyAdapter.Fill(dTable);
                 dgvRegister.DataSource = dTable;
             }
             dgvRegister.Columns[0].Visible = false;
             dgvRegister.Columns[1].Width = 85;
             dgvRegister.Columns[2].Width = 115;
             dgvRegister.Columns[3].Width = 115;
             dgvRegister.Columns[4].Width = 120;
         }
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
     private void udDepartment_ValueChanged(object sender, EventArgs e)
     {
         lbDepartment.Text = "Department " + udDepartment.Value.ToString();
         if (udDepartment.Value == 1) lbDepartment1.Text = cbDepartment.Text;
         else if (udDepartment.Value == 2) lbDepartment2.Text = cbDepartment.Text;
         else lbDepartment3.Text = cbDepartment.Text; //udDepartment.Value == 3
     }
     private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
     {
         if (udDepartment.Value == 1) lbDepartment1.Text = cbDepartment.Text;
         else if (udDepartment.Value == 2) lbDepartment2.Text = cbDepartment.Text;
         else lbDepartment3.Text = cbDepartment.Text; //udDepartment.Value == 3
     }

     private void btMoreDetails_Click(object sender, EventArgs e)
     {
         if (gbMoreDetails.Visible == false)
         {
             btMoreDetails.Text = "Less Info...";
             gbMoreDetails.Visible = true;
             cb1stSelect.Visible = false; cb2ndSelect.Visible = false;
             cbNameSearch.Visible = false; txNameSearch.Visible = false;
             gbMoreDetails.Location = new System.Drawing.Point(8, 16);
             dgvRegister.Location = new System.Drawing.Point(8, 275);
             dgvRegister.Size = new System.Drawing.Size(445, 225);//444, 250
         }
         else
         {
             btMoreDetails.Text = "More Info...";
             gbMoreDetails.Visible = false;
             cb1stSelect.Visible = true; cb2ndSelect.Visible = true;
             cbNameSearch.Visible = true; txNameSearch.Visible = true;
             //gbMoreDetails.Location = new System.Drawing.Point(8, 16);
             dgvRegister.Location = new System.Drawing.Point(8, 46);
             dgvRegister.Size = new System.Drawing.Size(445, 452);
         }
     }
     private void MoreDeatails()
     {
     }            
  //******************** End  ***************************************************************
    }
}