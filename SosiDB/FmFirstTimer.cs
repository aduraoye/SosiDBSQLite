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

namespace SosiDB
{
    public partial class FmFirstTimer : Form
    {
        FmLogin fmLogin;
        AutoCompleteStringCollection FNsource;
        string Table = "RegisterInfo";
        string EmailTo = "", MphoneNo = "", Title = "", FName = "", MName = "", LName = "", Para1 = "", Para2 = "", Para3 = "", strType;
        bool SendEmail = false;
        int NoF1st = 0;

        public FmFirstTimer()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
        }
        private void FmFirstTimer_Load(object sender, EventArgs e)
        {
            CreateTable();
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                strType = sr.ReadLine();
                fs.Close(); sr.Close();

                if (strType == "FirstTimer")
                {
                    this.Text = "SosiDB [First-Timer] -" + fmLogin.strChurchAcronym;
                    gbConvert.Visible = false;
                    this.MinimumSize = new System.Drawing.Size(448, 380);
                }
                else
                {
                    this.Text = "SosiDB [New Convert]";
                    gb1stTimer.Visible = false;
                    cbSearch2.SelectedIndex = 0; cbSearch2_SelectedIndexChanged(sender, e); 
                    cbStatus2.SelectedIndex = 0;
                    this.Size = new System.Drawing.Size(375, 230); this.MaximumSize = new System.Drawing.Size(375, 230);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        
        private void CreateTable()
        {   // Create Table
            string strDollarCent = "Title VARCHAR(7), First_Name VARCHAR(20), MName VARCHAR(20), Last_Name VARCHAR(20), BirthDay SMALLINT, BirthMonth VARCHAR(10), Age_Group VARCHAR(20), "
                    + "Sex VARCHAR(7), Marital_Status VARCHAR(10), Email VARCHAR(50), MPhone VARCHAR(15), HAdd VARCHAR(50), City VARCHAR(25), State VARCHAR(15), "
                    + "PCode VARCHAR(7), Country VARCHAR(15), Residency VARCHAR(15), Attendees VARCHAR(10), Department VARCHAR(20), Service VARCHAR(25), Status VARCHAR(15), HasFamily VARCHAR(5)";
            string strTbl = "CREATE TABLE IF NOT EXISTS " + Table + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date DATE, " + strDollarCent + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void InsertDB()
        {   //********** INSERT
            string Today = DateTime.Today.ToString("yyyy-MM-dd");
            string strDollarCent = "Title, First_Name, MName, Last_Name, BirthDay, BirthMonth, Age_Group, Sex, Marital_Status, Email, "
                                 + "MPhone, HAdd, City, State, PCode, Country, Residency, Attendees, Department, Service, Status, HasFamily";
            string strtxDollarCent = cbTitle.Text + "', '" + txFName.Text + "', '" + txMName.Text + "', '" + txLName.Text + "', '" 
                                   + cbDay.Text + "', '" + cbMonth.Text + "', '" + cbAgeGroup.Text + "', '" + cbSex.Text + "', '" 
                                   + cbMatrital.Text + "', '" + txEmail.Text + "', '" + txMobileNo.Text + "', '" + txHAdd.Text + "', '" 
                                   + txCity.Text + "', '" + cbState.Text + "', '" + txPostCode.Text + "', '" + cbCountry.Text 
                                   + "', 'Other Visa', '" + cbAttendees.Text + "', 'None', 'None', 'Unknown', 'No' ";
            string query = "INSERT INTO RegisterInfo (Date, " + strDollarCent + ") VALUES ('" + Today + "', '" + strtxDollarCent + ");";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            File.Copy(fmLogin.PathImage + "Pix-0", fmLogin.PathImage + "Pix-" + MaxRegInfo().ToString());
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            if (CheckInput() == 0) return;
            string strCheckDuplic = CheckDuplicate();
            if (strCheckDuplic != "null")
            {
                DialogResult mgSaveDuplicate = MessageBox.Show(strCheckDuplic + "\n\nWould still you like to continue with saving the record?", "Record Duplicating Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgSaveDuplicate == DialogResult.No) return;
            }

            DialogResult DgResult = MessageBox.Show("Are the details provided correct and would you like to send an email to the person?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DgResult == DialogResult.Yes)
            {
                InsertDB();
                Title = Title + ";" + cbTitle.Text; //Title
                FName = FName + ";" + txFName.Text;//First name
                MName = MName + ";" + txMName.Text;// Middle Name
                LName = LName + ";" + txLName.Text;// Last Name
                MphoneNo = MphoneNo + ";" + txMobileNo.Text;//Mobile Phone
                EmailTo = EmailTo + ";" + txEmail.Text;//Email
                Para1 = Para1 + ";" + "";
                Para2 = Para2 + ";" + "";
                Para3 = Para3 + ";" + "";
                SendEmail = true;
                NoF1st++;
            }
            else { return; }

            DialogResult mgUpdate = MessageBox.Show("Data Saved. Do you want to start a new record?", "Record Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgUpdate == DialogResult.Yes) Clear();
        }
        private int CheckInput()
        {
            if (cbTitle.Text == "Nonex") { MessageBox.Show("You have not selected any value for title."); cbTitle.Focus(); return 0; }
            if (txFName.Text == "First Name") { MessageBox.Show("You have not typed in your first name."); txFName.Focus(); return 0; }
            if (txMName.Text == "Middle Name") { MessageBox.Show("You have not typed in your middle name."); txMName.Focus(); return 0; }
            if (txLName.Text == "Surname Name") { MessageBox.Show("You have not typed in your surname(last) name."); txLName.Focus(); return 0; }

            if (cbDay.Text == "dd") { MessageBox.Show("You have not selected your birthday."); cbDay.Focus(); return 0; }
            if (cbMonth.Text == "mm") { MessageBox.Show("You have not selected your birthmonth."); cbMonth.Focus(); return 0; }
            if (cbSex.Text == "Sex") { MessageBox.Show("You have not selected your sex."); cbSex.Focus(); return 0; }
            if (cbMatrital.Text == "Marital Status") { MessageBox.Show("You have not selected your marital status."); cbMatrital.Focus(); return 0; }

            if (txEmail.Text == "E-mail Address") { MessageBox.Show("You have not typed in your email address."); txEmail.Text = "No E-mail"; txEmail.Focus(); return 0; }
            if (txMobileNo.Text == "Mobile No") { MessageBox.Show("You have not not typed in your mobile phone number."); txMobileNo.Focus(); return 0; }
            //if (txHomeNo.Text == "Home Phone No") { MessageBox.Show("You have not typed in your home phone number."); txHomeNo.Focus(); return 0; }
            if (txHAdd.Text == "House No & Street Name") { MessageBox.Show("You have not typed in your home address."); txHAdd.Focus(); return 0; }
            if (txCity.Text == "City/Suburb") { MessageBox.Show("You have not typed in your city."); txCity.Focus(); return 0; }
            if (cbState.Text == "State") { MessageBox.Show("You have not selected your state."); cbState.Focus(); return 0; }
            if (txPostCode.Text == "Post Code") { MessageBox.Show("You have not typed in your post code."); txPostCode.Focus(); return 0; }
            if (cbCountry.Text == "Country") { MessageBox.Show("You have not selected your country."); cbCountry.Focus(); return 0; }

            if (cbAgeGroup.Text == "Age Group") { MessageBox.Show("You have not selected your age group."); cbAgeGroup.Focus(); return 0; }
            if (cbAttendees.Text == "Attendees") { MessageBox.Show("You have not selected your Attendee class."); cbAttendees.Focus(); return 0; }
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
        
        private int MaxRegInfo()
       {
           string strPixId = "SELECT Id FROM RegisterInfo ORDER BY Id DESC LIMIT 1";
           int intMaxId;
           using (var conPix = new SQLiteConnection(fmLogin.strConReg))
           {
               conPix.Open();
               using (var cmd = new SQLiteCommand(strPixId, conPix))
               {
                   intMaxId = Convert.ToInt32(cmd.ExecuteScalar());
               }
           }
           return intMaxId;
       }
        private void Clear()
        {
            cbTitle.Text = "Nonex"; txFName.Text = "First Name"; txMName.Text = "Middle Name"; txLName.Text = "Surname Name";
            cbDay.Text = "Day"; cbMonth.Text = "Month"; cbSex.Text = "Sex"; cbMatrital.Text = "Marital Status"; txEmail.Text = "E-mail Address";
            txMobileNo.Text = "Mobile No"; txHAdd.Text = "House No & Street Name"; txCity.Text = "City/Suburb"; cbState.Text = "State";
            txPostCode.Text = "Post Code"; cbCountry.Text = "Country"; cbAgeGroup.Text = "Age Group"; cbAttendees.Text = "Attendees";
        }
        private void FmFirstTimer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SendEmail == true)
            { 
                DialogResult DialogMsg = MessageBox.Show("Would you like to send email and SMS messages to the firstimers? "
                             + "\nClick \"Yes\" without editing default message, \n\"No\" if you want to edit the default message and "
                             + "\n\"Cancel\" if you don't want to send message.", "Message Inforamtion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (DialogMsg == DialogResult.Cancel) { return; }
                 else if (DialogMsg == DialogResult.Yes)
                {
                    FileStream fs = new FileStream(fmLogin.PathMsg + strType + "EmailMessage.msgx", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string EmailSubject = sr.ReadLine();
                    sr.ReadLine();
                    string EmailBody = sr.ReadToEnd();
                    fs.Close(); sr.Close();
                    //EmailBody += fmLogin.EmailDefaultSignature();
                    fmLogin.SendEmailDt("5", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull, Title, FName, MName, LName, "", "", "");

                    FileStream fsx = new FileStream(fmLogin.PathMsg + strType + "SMSMessage.msgx", FileMode.Open);
                    StreamReader srx = new StreamReader(fsx);
                    string Messag = srx.ReadToEnd();
                    fsx.Close(); srx.Close();
                    fmLogin.SendSMSDt(MphoneNo, Messag, "", Title, FName, MName, LName, "", "", "", fmLogin.InternetPhoneSMS);
                }
                else
                {
                    FileStream fsop = new FileStream(fmLogin.MainPath + "temp.dbax", FileMode.Create, FileAccess.Write);
                    StreamWriter swop = new StreamWriter(fsop);
                    swop.WriteLine(strType); //Type of Msg either 1stTimer or NewConvert
                    swop.WriteLine(Title); //Title
                    swop.WriteLine(FName);//First name
                    swop.WriteLine(MName);//Middle name
                    swop.WriteLine(LName);// Last Name
                    swop.WriteLine(MphoneNo);//Mobile Phone
                    swop.WriteLine(EmailTo);//Email
                    swop.Flush(); swop.Close();

                    FmEmailPad fmEmailPad = new FmEmailPad();
                    fmEmailPad.Email1stTimer();
                }
            }

            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(NoF1st.ToString());
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            NoF1st = 0;
            SendEmail = false;
            Close();
        }
//*********************** gbConvert    ************************************
        string strTitle, strFName, strLName, Gap = "    ";
        private void cbSearch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Autocomplete();
            Clear2();
            txFName2.Text = "";
            txFName2.Enabled = true;
            if (cbSearch2.SelectedIndex == 0) { lb1Name2.Text = "First Name"; lb2Name2.Text = "Last Name"; }
            else { lb1Name2.Text = "Last Name"; lb2Name2.Text = "First Name"; }
        }
        private void btSaveData2_Click(object sender, EventArgs e)
        { 
            DialogResult DgResult = MessageBox.Show("Are these details correct and would you like to save or update it?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DgResult == DialogResult.Yes)
            {
                string subString = "";
                if (cbSearch2.SelectedIndex == 0) subString = "' WHERE First_Name || '" + Gap + "' || Last_Name = '" + txFName2.Text + "';";
                else subString = "' WHERE Last_Name || '" + Gap + "' || First_Name = '" + txFName2.Text + "';";

                string query = "UPDATE " + Table + " SET  Status = '" + cbStatus2.Text + subString;
                using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                    conTbly.Open();
                    cmdOffering.ExecuteNonQuery();
                }
                
                Title = Title + ";" + strTitle; //Title
                FName = FName + ";" + strFName;//First name
                MName = MName + ";" + "None";// Middle Name
                LName = LName + ";" + strLName;// Last Name
                MphoneNo = MphoneNo + ";" + txMobileNo2.Text;//Mobile Phone
                EmailTo = EmailTo + ";" + txEmail2.Text;//Email
                Para1 = Para1 + ";" + "";
                Para2 = Para2 + ";" + "";
                Para3 = Para3 + ";" + "";
                SendEmail = true;
                NoF1st++;
            }
            else { return; }

            MessageBox.Show("Data Saved!", "Convert Information ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btCancel2_Click(object sender, EventArgs e)
        {
            btCancel_Click(sender, e);
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
        private void Clear2()
        {
            txBirthdate2.Text = "Birthdate"; txEmail.Text = "E-mail Address";
            txMobileNo.Text = "Mobile No";
        }
        private void Autocomplete()
        {
            int CountNo = Count();
            string queryx = "SELECT First_Name, Last_Name FROM " + Table; //All RegisterInfo data
            DataTable dTable = new DataTable();
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }
            #region Set Autocomplete for txFName & txLName
            txFName2.AutoCompleteMode = AutoCompleteMode.Suggest;
            txFName2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            FNsource = new AutoCompleteStringCollection(); //source. 
            if (cbSearch2.SelectedIndex == 0) for (int i = 0; i < CountNo; i++) FNsource.Add(dTable.Rows[i][0].ToString() + Gap + dTable.Rows[i][1].ToString());
            else for (int i = 0; i < CountNo; i++) FNsource.Add(dTable.Rows[i][1].ToString() + Gap + dTable.Rows[i][0].ToString());
            txFName2.AutoCompleteCustomSource = FNsource;            
            #endregion
        }
        private void txFName2_TextChanged(object sender, EventArgs e)
        {
            if (FNsource.Contains(txFName2.Text) == true)
            {
                string subString = "";
                if (cbSearch2.SelectedIndex == 0) subString = " WHERE First_Name || '" + Gap + "' || Last_Name = '" + txFName2.Text + "';";
                else subString = " WHERE Last_Name || '" + Gap + "' || First_Name = '" + txFName2.Text + "';";

                string queryx = "SELECT BirthDay || BirthMonth AS Birthdate, MPhone, Email, Title, First_Name, Last_Name FROM " + Table + subString;
                 
                DataTable dTable = new DataTable();
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    MyAdapter.SelectCommand = cmdRegister;
                    MyAdapter.Fill(dTable);
                }
                txBirthdate2.Text = dTable.Rows[0][0].ToString();
                txMobileNo2.Text = dTable.Rows[0][1].ToString();    txEmail2.Text = dTable.Rows[0][2].ToString();
                strTitle = dTable.Rows[0][3].ToString(); 
                strFName = dTable.Rows[0][4].ToString(); strLName = dTable.Rows[0][5].ToString();
            }
        }
        // """"""""""""""""""""""""""""""""""" End
    }
}
