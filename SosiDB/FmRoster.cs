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
    public partial class FmRoster : Form
    {
        FmLogin fmLogin; TextBox autoText;
        AutoCompleteStringCollection source;
        DataGridViewCell clickedCell; DataGridView.HitTestInfo hit;
        PrintPreviewXDialog PreviewXDialog;

        int _2ndProgCol = -1; // day of service in the week = col of the 2nd program list
        int NoOfCol;  //  // no of weekday e.g. Sundays or Fridays in a month        
        bool DoLoad_dgvEdit = true;
        string Table = "RosterInfo";
        string CellValue, ServiceDay = "Sunday", AdHeader, DocName, NoneNull;
        string[] ServiceTime = new string[10];

        public FmRoster()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            PreviewXDialog = new PrintPreviewXDialog();
        }
        private void FmRoster_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Roster]";
            btCreate.Visible = false; btUpdate.Visible = false;
            CreateTable();
            cbService.SelectedIndex = 5;
            NoneNull = fmLogin.NoneNull;
            dgvEdit.SendToBack();
            dgvAutoComplete.SendToBack();
            cbService.SelectedIndex = 0;
            cbDayMinus.SelectedIndex = 0;
            fmLogin.SavingUpdate("SqlRos", "Loading"); 
        }
        private void btNew_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < dgvRoster.Columns.Count; j++)
            {
                for (int i = 0; i < dgvRoster.Rows.Count; i++)
                {
                    dgvRoster.Rows[i].Cells[j].Value = "";
                    dgvEdit.Rows[i].Cells[j].Value = "";
                }
            }
            if (ServiceDay == "Sunday") AddSundayProgram();
            else if (ServiceDay == "Friday") AddFridayProgram();
            else AddOtherdayProgram();
            dgvRoster.Columns[0].Width = 125; dgvRoster.Columns[1].Width = 125;
            dgvRoster.Columns[2].Width = 125; dgvRoster.Columns[3].Width = 125;
            dgvRoster.Columns[4].Width = 125; dgvRoster.Columns[5].Width = 125;
            dgvRoster.Columns[6].Width = 125;
            loadDgvHeader();
            btNew.Visible = false; btCreate.Visible = true;
        }
        private void btCreate_Click(object sender, EventArgs e)
        {
            loadDgvHeader();
            InsertDB();
            btCreate.Visible = false; btEdit.Visible = true; //btNew.Visible = true;
            RosterSchedule();

            EmailType = "Roster"; valRetry = true;//"Roster - another version";           
            while (valRetry == true)
            {
                if (dialogEmail(EmailType, cbService.Text, dgvRoster, dtpMonth, ref valRetry) == DialogResult.Yes) { }
                else valRetry = false;
            }

            if (cbService.Text == "Sunday")
            {
                EmailType = "Birthdate"; valRetry = true;
                while (valRetry == true)
                {
                    if (dialogEmail(EmailType, cbService.Text, dgvAutoComplete, dtpMonth, ref valRetry) == DialogResult.Yes) { }
                    else valRetry = false;
                }

                ExhRetry = true;
                while (ExhRetry == true)
                {
                    if (dialogExhort(NoOfCol, dgvRoster, dgvEdit, ref ExhRetry) == DialogResult.Yes) { }
                    else valRetry = false;
                }
            }
            fmLogin.SavingUpdate("SqlRos", "Saving");
        }
        private void btEdit_Click(object sender, EventArgs e)
        {
            string MonthYear = ServiceDay + dtpMonth.Value.ToString("/MM/yyyy");
            string queryx = queryx = "SELECT Colm0, Colm1  || ' , ' || Colx1, Colm2  || ' , ' || Colx2, Colm3  || ' , ' || Colx3, "
                          + "Colm4  || ' , ' || Colx4, Colm5  || ' , ' || Colx5, Colm6   || ' , ' || Colx6, Id FROM " + Table + " WHERE Date = '" + MonthYear + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                dgvRoster.DataSource = dTable;
            }
            dgvRoster.Columns[0].Width = 125; dgvRoster.Columns[1].Width = 125;
            dgvRoster.Columns[2].Width = 125; dgvRoster.Columns[3].Width = 125;
            dgvRoster.Columns[4].Width = 125; dgvRoster.Columns[5].Width = 125;
            dgvRoster.Columns[6].Width = 125; dgvRoster.Columns[7].Visible = false;
            loadDgvHeader();
            btEdit.Visible = false; btUpdate.Visible = true;
        }
        private void btUpdate_Click(object sender, EventArgs e)
        {//*UPDATE 
            loadDgvHeader();
            char[] sep = new char[] { ',' }; string[] splitCol;
            // Splitting the cell values into colm and colx
            for (int j = 1; j < 7; j++) // j == col or cell j starts from 1 bcos j = 0 is the colm0 which does not need separation
            {
                for (int i = 0; i < 11; i++) // i == row
                {
                    if (dgvRoster.Rows[i].Cells[j].Value.ToString() == "")
                    {
                        dgvRoster.Rows[i].Cells[j].Value = "";
                        dgvEdit.Rows[i].Cells[j].Value = "";
                    }
                    else
                    {
                        splitCol = dgvRoster.Rows[i].Cells[j].FormattedValue.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        dgvRoster.Rows[i].Cells[j].Value = splitCol[0];
                        dgvEdit.Rows[i].Cells[j].Value = splitCol[1];
                    }
                }
            }

            string MonthYear = ServiceDay + dtpMonth.Value.ToString("/MM/yyyy");
            string query = "", strtxDollarCent = "";
            string strId0 = dgvRoster.Rows[0].Cells[7].FormattedValue.ToString();
            int indx = int.Parse(strId0);
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                for (int i = 0; i < 11; i++)
                {
                    strtxDollarCent = "Colm0 = '" + dgvRoster.Rows[i].Cells[0].FormattedValue.ToString() + "', Colm1 = '" + dgvRoster.Rows[i].Cells[1].FormattedValue.ToString()
                                        + "', Colm2 = '" + dgvRoster.Rows[i].Cells[2].FormattedValue.ToString() + "', Colm3 = '" + dgvRoster.Rows[i].Cells[3].FormattedValue.ToString()
                                        + "', Colm4 = '" + dgvRoster.Rows[i].Cells[4].FormattedValue.ToString() + "', Colm5 = '" + dgvRoster.Rows[i].Cells[5].FormattedValue.ToString()
                                        + "', Colm6 = '" + dgvRoster.Rows[i].Cells[6].FormattedValue.ToString() + "', Colx1 = '" + dgvEdit.Rows[i].Cells[1].FormattedValue.ToString()
                                        + "', Colx2 = '" + dgvEdit.Rows[i].Cells[2].FormattedValue.ToString() + "', Colx3 = '" + dgvEdit.Rows[i].Cells[3].FormattedValue.ToString()
                                        + "', Colx4 = '" + dgvEdit.Rows[i].Cells[4].FormattedValue.ToString() + "', Colx5 = '" + dgvEdit.Rows[i].Cells[5].FormattedValue.ToString()
                                        + "', Colx6 = '" + dgvEdit.Rows[i].Cells[6].FormattedValue.ToString() + "'";
                    query = "UPDATE " + Table + " SET  " + strtxDollarCent + " WHERE Id = '" + indx.ToString() + "'";
                    SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                    conTbly.Open();
                    cmdOffering.ExecuteNonQuery();
                    conTbly.Close();
                    indx++;
                }
            }
            SelectDB();
            btEdit.Visible = true; btUpdate.Visible = false;
            RosterSchedule();

            EmailType = "Roster - another version"; valRetry = true;
            while (valRetry == true)
            {
                if (dialogEmail(EmailType, cbService.Text, dgvRoster, dtpMonth, ref valRetry) == DialogResult.Yes) { }
                else valRetry = false;
            }

            if (cbService.Text == "Sunday" && chUpdateOtherEmail.Checked == true)
            {
                EmailType = "Birthdate"; valRetry = true;
                while (valRetry == true)
                {
                    if (dialogEmail(EmailType, cbService.Text, dgvAutoComplete, dtpMonth, ref valRetry) == DialogResult.Yes) { }
                    else valRetry = false;
                }

                ExhRetry = true;
                while (ExhRetry == true)
                {
                    if (dialogExhort(NoOfCol, dgvRoster, dgvEdit, ref ExhRetry) == DialogResult.Yes) { }
                    else valRetry = false;
                }
            }
            fmLogin.SavingUpdate("SqlRos", "Saving");
        }

        private void CreateTable()
        {   // Create Table
            string strDollarCent = "Colm0 VARCHAR(50), Colm1 VARCHAR(50), Colm2 VARCHAR(50), Colm3 VARCHAR(50), Colm4 VARCHAR(50), Colm5 VARCHAR(50), Colm6 VARCHAR(50), "
                                                    + "Colx1 VARCHAR(50), Colx2 VARCHAR(50), Colx3 VARCHAR(50), Colx4 VARCHAR(50), Colx5 VARCHAR(50), Colx6 VARCHAR(50)";
            string strTbl = "CREATE TABLE IF NOT EXISTS " + Table + " (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date VARCHAR(20), " + strDollarCent + ");";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void DeleteDBTable()
        {    //Insert
            string query = "DROP TABLE IF EXISTS " + Table;
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void InsertDB()
        {    //Insert
            char[] sep = new char[] { ',' }; string[] splitCol;
            for (int j = 1; j < 7; j++)
            {
                //MessageBox.Show("Here j = " + j.ToString());
                if (_2ndProgCol != j)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        if (dgvRoster.Rows[i].Cells[j].Value.ToString() == "")
                        {
                            dgvRoster.Rows[i].Cells[j].Value = "";
                            dgvEdit.Rows[i].Cells[j].Value = "";
                        }
                        else
                        {
                            splitCol = dgvRoster.Rows[i].Cells[j].FormattedValue.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            dgvRoster.Rows[i].Cells[j].Value = splitCol[0];
                            dgvEdit.Rows[i].Cells[j].Value = splitCol[1];
                        }
                    }
                }
            }
            //MessageBox.Show("Here");
            string MonthYear = ServiceDay + dtpMonth.Value.ToString("/MM/yyyy");
            string strDollarCent = "Colm0, Colm1, Colm2, Colm3, Colm4, Colm5, Colm6, Colx1, Colx2, Colx3, Colx4, Colx5, Colx6";
            string query = "", strtxDollarCent = "";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                for (int i = 0; i < 11; i++)
                {
                    strtxDollarCent = dgvRoster.Rows[i].Cells[0].FormattedValue.ToString() + "', '" + dgvRoster.Rows[i].Cells[1].FormattedValue.ToString() + "', '"
                                    + dgvRoster.Rows[i].Cells[2].FormattedValue.ToString() + "', '" + dgvRoster.Rows[i].Cells[3].FormattedValue.ToString() + "', '"
                                    + dgvRoster.Rows[i].Cells[4].FormattedValue.ToString() + "', '" + dgvRoster.Rows[i].Cells[5].FormattedValue.ToString() + "', '"
                                    + dgvRoster.Rows[i].Cells[6].FormattedValue.ToString() + "', '" + dgvEdit.Rows[i].Cells[1].FormattedValue.ToString() + "', '"
                                    + dgvEdit.Rows[i].Cells[2].FormattedValue.ToString() + "', '" + dgvEdit.Rows[i].Cells[3].FormattedValue.ToString() + "', '"
                                    + dgvEdit.Rows[i].Cells[4].FormattedValue.ToString() + "', '" + dgvEdit.Rows[i].Cells[5].FormattedValue.ToString() + "', '"
                                    + dgvEdit.Rows[i].Cells[6].FormattedValue.ToString();
                    query = "INSERT INTO " + Table + " (Date, " + strDollarCent + ") VALUES ('" + MonthYear + "', '" + strtxDollarCent + "');";

                    SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                    conTbly.Open();
                    cmdOffering.ExecuteNonQuery();
                    conTbly.Close();
                }
            }
        }
        private void SelectDB()
        { // Select  
            string MonthYear = ServiceDay + dtpMonth.Value.ToString("/MM/yyyy");
            string queryx = queryx = "SELECT Colm0, Colm1, Colm2, Colm3, Colm4, Colm5, Colm6 FROM " + Table + " WHERE Date = '" + MonthYear + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                if (Count() > 0)
                {
                    MyAdapter.SelectCommand = cmdRegister;
                    MyAdapter.Fill(dTable);
                }
                else
                {
                    for (int i = 0; i < 7; i++) dTable.Columns.Add("Colm" + i.ToString(), typeof(string));
                    for (int i = 0; i < 11; i++) dTable.Rows.Add("", "", "", "", "", "", "");
                }
                dgvRoster.DataSource = dTable;
                if (DoLoad_dgvEdit == true) { dgvEdit.DataSource = dTable; DoLoad_dgvEdit = false; }
            }
            dgvRoster.Columns[0].Width = 125; dgvRoster.Columns[1].Width = 125;
            dgvRoster.Columns[2].Width = 125; dgvRoster.Columns[3].Width = 125;
            dgvRoster.Columns[4].Width = 125; dgvRoster.Columns[5].Width = 125;
            dgvRoster.Columns[6].Width = 125;
            loadDgvHeader();
        }
        private void RosterSchedule()
        {
            int HeaderCol = 0, HourMinus;
            if (cbDayMinus.SelectedIndex == 0) HourMinus = -0;
            else if (cbDayMinus.SelectedIndex == 1) HourMinus = -6 * 24;
            else if (cbDayMinus.SelectedIndex == 2) HourMinus = -13 * 24;
            else HourMinus = -30 * 24;

            char[] sep = new char[] { '\n' }; char[] secom = new char[] { ',' };

            for (int j = 0; j <= NoOfCol; j++)//Columns
            {
                string[] splitHeader = dgvRoster.Columns[j].HeaderText.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (splitHeader[0] == "Program")
                {
                    HeaderCol = j;
                }
                else
                {
                    try
                    {
                        DateTime MonB4; string RosterFile;
                        if (ServiceDay == "Sunday") MonB4 = DateTime.Parse(splitHeader[3] + " 06:00:00").AddDays(-6);
                        else MonB4 = DateTime.Parse(splitHeader[3] + " 06:00:00").AddDays(-4);
                        if (long.Parse(MonB4.ToString("yyyyMMddHHmmss")) > long.Parse(DateTime.Now.AddHours(HourMinus).ToString("yyyyMMddHHmmss")))
                        {
                            RosterFile = fmLogin.PathSchedule + "~" + ServiceDay + "-" + MonB4.ToString("yyyyMMddHHmm") + "-" + "Email-" + dgvRoster.Columns[j].Index.ToString() + ".sch";
                            if (File.Exists(RosterFile)) File.Delete(RosterFile);
                            FileStream fsop = new FileStream(RosterFile, FileMode.Create, FileAccess.Write);
                            StreamWriter swop = new StreamWriter(fsop);
                            swop.WriteLine("Roster");
                            swop.WriteLine("Email");
                            swop.WriteLine(ServiceDay + dtpMonth.Value.ToString("/MM/yyyy"));
                            swop.WriteLine(ServiceTime[j]);
                            swop.WriteLine(HeaderCol.ToString());
                            swop.WriteLine(j.ToString());
                            swop.WriteLine(dgvRoster.Columns[j].HeaderText.Replace("\n", " "));
                            swop.WriteLine(NoOfRows(j, ServiceDay).ToString());
                            swop.Flush(); swop.Close();
                        }

                        #region SMS for Sunday Service
                        /* DateTime MonB4Sms;
                        if (ServiceDay == "Sunday") MonB4Sms = DateTime.Parse(splitHeader[3] + " 15:00:00").AddDays(-6);
                        else MonB4Sms = DateTime.Parse(splitHeader[3] + " 15:00:00").AddDays(-4);
                        if (long.Parse(MonB4Sms.ToString("yyyyMMddHHmmss")) > long.Parse(DateTime.Now.AddHours(HourMinus).ToString("yyyyMMddHHmmss")))
                        {
                            RosterFile = fmLogin.PathSchedule + "~" + ServiceDay + "-" + MonB4Sms.ToString("yyyyMMddHHmm") + "-" + "SMS-" + dgvRoster.Columns[j].Index.ToString() + ".sch";
                            if (File.Exists(RosterFile)) File.Delete(RosterFile);
                            FileStream fsoq = new FileStream(RosterFile, FileMode.Create, FileAccess.Write);
                            StreamWriter swoq = new StreamWriter(fsoq);
                            swoq.WriteLine("Roster");
                            swoq.WriteLine("SMS");
                            swoq.WriteLine(ServiceDay + dtpMonth.Value.ToString("/MM/yyyy"));
                            swoq.WriteLine(ServiceTime[j]);
                            swoq.WriteLine(HeaderCol.ToString());
                            swoq.WriteLine(j.ToString());
                            swoq.WriteLine(dgvRoster.Columns[j].HeaderText.Replace("\n", " "));
                            swoq.WriteLine(NoOfRows(j, ServiceDay).ToString());
                            swoq.Flush(); swoq.Close();
                        }*/
                        # endregion

                        DateTime FriSatB4;
                        if (ServiceDay == "Sunday") FriSatB4 = DateTime.Parse(splitHeader[3] + " 10:00:00").AddDays(-1);
                        else FriSatB4 = DateTime.Parse(splitHeader[3] + " 12:00:00");
                        if (long.Parse(FriSatB4.ToString("yyyyMMddHHmmss")) > long.Parse(DateTime.Now.AddHours(HourMinus).ToString("yyyyMMddHHmmss")))
                        {
                            RosterFile = fmLogin.PathSchedule + "~" + ServiceDay + "-" + FriSatB4.ToString("yyyyMMddHHmm") + "-" + "SMS-" + dgvRoster.Columns[j].Index.ToString() + ".sch";
                            if (File.Exists(RosterFile)) File.Delete(RosterFile);
                            FileStream fsopx = new FileStream(RosterFile, FileMode.Create, FileAccess.Write);
                            StreamWriter swopx = new StreamWriter(fsopx);
                            swopx.WriteLine("Roster");
                            swopx.WriteLine("SMSService");
                            swopx.WriteLine(ServiceDay + dtpMonth.Value.ToString("/MM/yyyy"));
                            swopx.WriteLine(ServiceTime[j]);
                            swopx.WriteLine(HeaderCol.ToString());
                            swopx.WriteLine(j.ToString());
                            swopx.WriteLine(dgvRoster.Columns[j].HeaderText.Replace("\n", " "));
                            swopx.WriteLine(NoOfRows(j, ServiceDay).ToString());
                            swopx.Flush(); swopx.Close();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
            }
        }

        private int NoOfRows(int Colmx, string DayWeek)
        {
            int Rowx = 0;
            if (DayWeek == "Sunday") Rowx = 11;
            else //(DayWeek == "Friday")
            {
                if (Colmx < GetNoOfWeekDayInMonth(dtpMonth.Value, DayWeek)) Rowx = 6;
                else if (Colmx > GetNoOfWeekDayInMonth(dtpMonth.Value, DayWeek)) Rowx = 9;
            }
            return Rowx;
        }
        private void loadDgvHeader()
        {
            string datex, WhichDay, posn; int Ind = 0; int ix = 0;
            int IntDay = GetFirstDayOfWeekOfTheMonth(dtpMonth.Value, ServiceDay);
            int VarIndex;
            if (GetNoOfWeekDayInMonth(dtpMonth.Value, ServiceDay) == 4) VarIndex = 5; else VarIndex = 4;
            foreach (DataGridViewColumn column in dgvRoster.Columns)
            {
                #region Sunday service
                if (cbService.SelectedIndex == 0)// Sunday
                {
                    if (column.Index == 1 || column.Index == 3 || column.Index == 4 || column.Index == 5 || column.Index == 6)
                    {
                        if (column.Index == 1) { Ind = 1; ix = 0; } else { Ind = 2; ix = 1; }
                        if ((IntDay + 7 * (column.Index - Ind)) <= DateTime.DaysInMonth(dtpMonth.Value.Year, dtpMonth.Value.Month))
                        {
                            if (column.Index == 1) { WhichDay = "Thanksgiving\nService\n"; posn = "st"; }
                            else if (column.Index == 3) { WhichDay = "Evangelism\nService\n"; posn = "nd"; }
                            else if (column.Index == 4) { WhichDay = "Youth\nService\n"; posn = "rd"; }
                            else if (column.Index == 5) { WhichDay = "Glorious\nService\n"; posn = "th"; }
                            else { WhichDay = "Glorious\nService\n"; posn = "th"; }
                            if (column.Index == 6) { dgvRoster.Columns[6].Visible = true; NoOfCol = 6; } else NoOfCol = 5;
                            datex = (IntDay + 7 * (column.Index - Ind)).ToString() + dtpMonth.Value.ToString("/MM/yyyy");
                            column.HeaderText = String.Concat(WhichDay, (column.Index - ix).ToString(), posn, " Sunday\n", datex);
                        }
                        else { column.HeaderText = ""; dgvRoster.Columns[6].Visible = false; }
                        ServiceTime[column.Index] = "09:45-12:30";
                    }
                    else
                    {
                        if (column.Index == 0) { column.HeaderText = String.Concat("Program", "\n1st Sunday"); }
                        if (column.Index == 2) { column.HeaderText = String.Concat("Program", "\nother Sundays"); _2ndProgCol = 2; }
                    }
                }
                #endregion

                #region Friday service
                if (cbService.SelectedIndex == 5) //Friday VarIndex = 4 or 5 
                {
                    if (column.Index == 1 || column.Index == 2 || column.Index == 3 || column.Index == VarIndex || column.Index == 6)
                    {
                        if (column.Index < 4) { Ind = 1; ix = 0; }
                        else
                        {
                            if (column.Index == 4) { Ind = 1; ix = 0; } else { Ind = 2; ix = 1; }
                        }

                        if ((IntDay + 7 * (column.Index - Ind)) <= DateTime.DaysInMonth(dtpMonth.Value.Year, dtpMonth.Value.Month))
                        {
                            if (column.Index == 1) { WhichDay = "Bible/Prayer\nMeeting\n"; posn = "st"; }
                            else if (column.Index == 2) { WhichDay = "Bible/Prayer\nMeeting\n"; posn = "nd"; }
                            else if (column.Index == 3) { WhichDay = "Bible/Prayer\nMeeting\n"; posn = "rd"; }

                            else if (column.Index == 4) { WhichDay = "Bible/Prayer\nMeeting\n"; posn = "th"; }
                            else if (column.Index == 5) { WhichDay = "Vigil\nService\n"; posn = "th"; }
                            else { WhichDay = "Vigil\nService\n"; posn = "th"; }

                            if (column.Index == 6) { dgvRoster.Columns[6].Visible = true; NoOfCol = 6; } else NoOfCol = 5;
                            datex = (IntDay + 7 * (column.Index - Ind)).ToString() + dtpMonth.Value.ToString("/MM/yyyy");
                            column.HeaderText = String.Concat(WhichDay, (column.Index - ix).ToString(), posn, " Friday\n", datex);
                        }
                        else { column.HeaderText = ""; dgvRoster.Columns[6].Visible = false; }

                        if (column.Index < 5) ServiceTime[column.Index] = "19:00-20:30";
                        else ServiceTime[column.Index] = "22:15-01:00";
                    }
                    else
                    {
                        if (column.Index == 0) { column.HeaderText = String.Concat("Program", "\nOther Friday"); }
                        if (column.Index == 4) { column.HeaderText = String.Concat("Program", "\nLast Friday"); _2ndProgCol = 4; }
                        if (column.Index == 5) { column.HeaderText = String.Concat("Program", "\nLast Friday"); _2ndProgCol = 5; }
                    }
                }
                #endregion
            }
        }
        private void AddSundayProgram()
        {   // Do not include "," in the strings
            // First Sunday
            dgvRoster.Rows[0].Cells[0].Value = "Workers Meeting\n09:15-09:45 (30 min)";
            dgvRoster.Rows[1].Cells[0].Value = "Sunday School\n09:45-10:45 (60 min)";
            dgvRoster.Rows[2].Cells[0].Value = "Opening Prayer\n10:45-10:50 (5 min)";
            dgvRoster.Rows[3].Cells[0].Value = "Praise & Worship\n10:50-11:10 (20 min)";
            dgvRoster.Rows[4].Cells[0].Value = "Intercessory Prayer\n11:10-11:20 (10 min)";
            dgvRoster.Rows[5].Cells[0].Value = "Bible Reading\n11:20-11:25 (5 min)";
            dgvRoster.Rows[6].Cells[0].Value = "Tithe&offering/Hymn\n11:25-11:35 (10 min)";
            dgvRoster.Rows[7].Cells[0].Value = "Testimony\n11:35-11:50 (15 min)";
            dgvRoster.Rows[8].Cells[0].Value = "Thanksgiving\n11:50-12:15 (25 min)";
            dgvRoster.Rows[9].Cells[0].Value = "Announcement\n12:15-12:25 (10 min)";
            dgvRoster.Rows[10].Cells[0].Value = "Closing Prayer\n12:25-12:30 (5 min)";
            //Other Sundays
            dgvRoster.Rows[0].Cells[2].Value = "Workers Meeting\n09:15-09:45 (30 min)";
            dgvRoster.Rows[1].Cells[2].Value = "Sunday School\n09:45-10:45 (60 min)";
            dgvRoster.Rows[2].Cells[2].Value = "Opening Prayer\n10:45-10:50 (5 min)";
            dgvRoster.Rows[3].Cells[2].Value = "Praise & Worship\n10:50-11:10 (20 min)";
            dgvRoster.Rows[4].Cells[2].Value = "Intercessory Prayer\n11:10-11:20 (10 min)";
            dgvRoster.Rows[5].Cells[2].Value = "Bible Reading\n11:20-11:25 (5 min)";
            dgvRoster.Rows[6].Cells[2].Value = "Choir Min/Hymns\n11:25-11:35 (10 min)";
            dgvRoster.Rows[7].Cells[2].Value = "Exhortation\n11:35-12:05 (30 min)";
            dgvRoster.Rows[8].Cells[2].Value = "Tithes & offerings\n12:05-12:15 (10 min)";
            dgvRoster.Rows[9].Cells[2].Value = "Announcement\n12:15-12:25 (10 min)";
            dgvRoster.Rows[10].Cells[2].Value = "Closing Prayer\n12:25-12:30 (5 min)";
        }
        private void AddFridayProgram()
        {      //Do not include "," in the strings 
            // Other Friday
            dgvRoster.Rows[0].Cells[0].Value = "Opening Prayer\n19:00-19:05 (05 min)";
            dgvRoster.Rows[1].Cells[0].Value = "Praise & Worship\n19:05-19:20 (15 min)";
            dgvRoster.Rows[2].Cells[0].Value = "Intercessory Prayer\n19:20-19:40 (20 min)";
            dgvRoster.Rows[3].Cells[0].Value = "Bible Studying\n19:40-20:15 (35 min)";
            dgvRoster.Rows[4].Cells[0].Value = "offerings\n20:15-20:20 (5 min)";
            dgvRoster.Rows[5].Cells[0].Value = "Closing Prayer\n20:20-20:25 (5 min)";
            //Last Friday Vigil
            int VarIndex = GetNoOfWeekDayInMonth(dtpMonth.Value, ServiceDay);
            dgvRoster.Rows[0].Cells[VarIndex].Value = "Opening Prayer\n22:30-22:35 (5 min)";
            dgvRoster.Rows[1].Cells[VarIndex].Value = "Praise & Worship\n22:35-23:00 (25 min)";
            dgvRoster.Rows[2].Cells[VarIndex].Value = "Intercessory Prayer\n23:00-23:15 (15 min)";
            dgvRoster.Rows[3].Cells[VarIndex].Value = "Bible Reading\n23:15-23:20 (5 min)";
            dgvRoster.Rows[4].Cells[VarIndex].Value = "Testimony\n23:20-23:40 (20 min)";
            dgvRoster.Rows[5].Cells[VarIndex].Value = "Exhortation\n23:40-00:30 (50 min)";
            dgvRoster.Rows[6].Cells[VarIndex].Value = "offerings\n00:30-00:40 (10 min)";
            dgvRoster.Rows[7].Cells[VarIndex].Value = "Announcement\n00:40-00:50 (10 min)";
            dgvRoster.Rows[8].Cells[VarIndex].Value = "Closing Prayer\n00:50-00:55 (5 min)";
        }
        private void AddOtherdayProgram()
        {      // Do not include "," in the strings
            // First Sunday
        }
        public static int GetFirstDayOfWeekOfTheMonth(DateTime date, string WeekDay)
        {
            date = DateTime.Parse("1" + "/" + date.Month.ToString() + "/" + date.Year.ToString());
            while (date.DayOfWeek.ToString() != WeekDay)
            {
                date = date.AddDays(1);
            }
            return int.Parse(date.Day.ToString());
        }
        public static int GetNoOfWeekDayInMonth(DateTime date, string WeekDay)
        {
            int i = 0;
            int LastDayMonth = System.DateTime.DaysInMonth(date.Year, date.Month);
            int F1stWeekDay = GetFirstDayOfWeekOfTheMonth(date, WeekDay);
            for (int k = F1stWeekDay; k <= LastDayMonth; k = k + 7) i++;
            return i;
        }

        private void AutoComplete()
        {
            string strCount = "SELECT COUNT(*) FROM RegisterInfo WHERE Service = 'Believers' OR Service = 'Baptismal' OR "
                            + "Service = 'Trainee' OR Service = 'Worker' OR Service = 'Associate' OR Service = 'Minister' OR Service = 'Leader'";
            int intCount;
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix))
                {
                    intCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Select
            string queryx = "SELECT First_Name || ' ' || Last_Name || ' , ' || Title  || ' ; ' || Email || ' ; ' || MPhone FROM RegisterInfo "
                          + "WHERE Service = 'Believers' OR Service = 'Baptismal' OR Service = 'Trainee' OR Service = 'Worker' OR "
                          + "Service = 'Associate' OR Service = 'Minister' OR Service = 'Leader'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                dgvAutoComplete.DataSource = dTable;
            }
            source = new AutoCompleteStringCollection();
            for (int i = 0; i < intCount; i++) source.Add(dgvAutoComplete.Rows[i].Cells[0].FormattedValue.ToString());
            source.Add("Praise Team , None ; None ; 0000");
            source.Add("Invited Minister , None ; None ; 0000");
            source.Add("No Service , None ; None ; 0000");
        }
        private void dgvRoster_MouseDown(object sender, MouseEventArgs e)
        {
            hit = dgvRoster.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell) clickedCell = dgvRoster.Rows[hit.RowIndex].Cells[hit.ColumnIndex];

        }
        private void dgvRoster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clickedCell.ColumnIndex != 0 && clickedCell.ColumnIndex != _2ndProgCol)
            {
                if (btCreate.Visible == true || btUpdate.Visible == true) dgvRoster.EditMode = DataGridViewEditMode.EditOnKeystroke;
                else dgvRoster.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            else dgvRoster.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void dgvRoster_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (clickedCell.ColumnIndex != 0 && clickedCell.ColumnIndex != _2ndProgCol)
            {
                autoText = e.Control as TextBox;
                if (autoText != null)
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoComplete();
                    autoText.AutoCompleteCustomSource = source;
                }
            }
        }
        private void dgvRoster_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            CellValue = dgvRoster.CurrentCell.FormattedValue.ToString();
        }
        private void dgvRoster_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (source.Contains(autoText.Text) == false) dgvRoster.CurrentCell.Value = CellValue;
        }
        private void dgvRoster_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dgvRoster.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            SelectDB();
            if (Count() > 0) { btNew.Visible = false; btCreate.Visible = false; btEdit.Visible = true; btUpdate.Visible = false; }
            else { btNew.Visible = true; btCreate.Visible = false; btEdit.Visible = false; btUpdate.Visible = false; }
            dgvRoster.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceDay = cbService.Text;
            SelectDB();
            if (Count() > 0) { btNew.Visible = false; btCreate.Visible = false; btEdit.Visible = true; btUpdate.Visible = false; }
            else { btNew.Visible = true; btCreate.Visible = false; btEdit.Visible = false; btUpdate.Visible = false; }
            dgvRoster.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private int Count()
        {
            string MonthYear = ServiceDay + dtpMonth.Value.ToString("/MM/yyyy");
            string strCount = "SELECT COUNT(*) FROM " + Table + " WHERE Date = '" + MonthYear + "'";
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

        bool valRetry, ExhRetry; string EmailType;// QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ // QQQQQQQQQ- dialogExhort -QQQQQQQQQQQQQ
        public static DialogResult dialogEmail(string EmailType, string ServiceDay, DataGridView dgvAutoComplete, DateTimePicker dtpMonth, ref bool valRetry)
        {
            Form form = new Form();
            Label lbInfo = new Label();
            TextBox txEmailTo = new TextBox();
            Button buttonYes = new Button();
            Button buttonNo = new Button();

            form.Text = " Email " + EmailType;
            if (EmailType == "Birthdate") lbInfo.Text = "Would like to send a PDF file of the month \nbirthdate details to the birthdate coordinator?";
            else lbInfo.Text = "Would like to send a copy of the month roster to church program coordinator?";
            txEmailTo.Text = "Coord email";
            buttonYes.Text = "Yes"; buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.Text = "No"; buttonNo.DialogResult = DialogResult.No;
            lbInfo.SetBounds(5, 10, 250, 25);
            txEmailTo.SetBounds(20, 50, 180, 20);
            buttonYes.SetBounds(20, 85, 80, 23); buttonNo.SetBounds(120, 85, 80, 23);
            buttonYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;


            FmLogin fmLoginx = new FmLogin();
            string NoneNull = fmLoginx.NoneNull;
            PrintPreviewXDialog PreviewXDialog = new PrintPreviewXDialog();
            List<string> AttachmentList = new List<string>();
            #region Set autocomplete
            txEmailTo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txEmailTo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            string strCount = "SELECT COUNT(*) FROM RegisterInfo WHERE Service = 'Trainee' OR Service = 'Worker' OR Service = 'Minister' OR Service = 'Leader'";
            int intCount;
            using (var conPix = new SQLiteConnection(fmLoginx.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix))
                {
                    intCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            string queryk = "SELECT First_Name || ' ' || Last_Name || ' ; ' || Email FROM RegisterInfo WHERE Service = 'Trainee' OR Service = 'Worker' OR Service = 'Minister' OR Service = 'Leader'";
            DataTable dTablex = new DataTable();
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLoginx.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryk, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTablex);
            }
            AutoCompleteStringCollection source = new AutoCompleteStringCollection();
            for (int i = 0; i < intCount; i++) source.Add(dTablex.Rows[i][0].ToString());
            txEmailTo.AutoCompleteCustomSource = source;
            #endregion

            form.ClientSize = new Size(250, 120);
            form.Controls.AddRange(new Control[] { lbInfo, txEmailTo, buttonYes, buttonNo });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonYes;
            form.CancelButton = buttonNo;

            DialogResult dialogResult = form.ShowDialog();
            if (txEmailTo.Text != "Coord email")
            {
                if (form.DialogResult == DialogResult.Yes)
                {
                    char[] sep = new char[] { ';' };
                    string[] splitCol = txEmailTo.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    txEmailTo.Text = splitCol[1];

                    if (EmailType == "Birthdate")
                    {
                        DataTable dTable;
                        string queryx = "SELECT Title, First_Name, Last_Name, BirthDay || BirthMonth AS Birthdate FROM RegisterInfo WHERE BirthMonth = '" + dtpMonth.Value.ToString(" MMMM") + "';";
                        using (SQLiteConnection conTblx = new SQLiteConnection(fmLoginx.strConReg))
                        {
                            SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                            SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                            dTable = new DataTable();
                            MyAdapter.SelectCommand = cmdRegister;
                            MyAdapter.Fill(dTable);
                        }
                        string DocName = dtpMonth.Value.ToString("MMMM yyyy") + " Birthdate";
                        string AdHeader = fmLoginx.strChurchAcronym + "-" + DocName;
                        string strBirthday = "\n";
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            strBirthday = strBirthday + "\n" + dTable.Rows[i][0].ToString() + "  \t" + dTable.Rows[i][1].ToString()
                                                    + "  \t" + dTable.Rows[i][2].ToString() + "  \t" + dTable.Rows[i][3].ToString();
                        }
                        string strDetail = "Dear Church Birthdate Coordinator,"
                               + "\n\nFind below are the church members birthdate details for this month. Please, send a reply email if there is any mistake. "
                               + "\n\nMay the all sufficient God remember your labour of love in His kingdom and reward you accordingly in Jesus' name , Amen."
                               + strBirthday;

                        if (txEmailTo.Text.Length > 6 && txEmailTo.Text.Contains('@') == true)
                            fmLoginx.SendEmailSt("2", txEmailTo.Text, "", AdHeader, strDetail, "", null, NoneNull);
                        MessageBox.Show(" The month birthdate detail is successfully created and emailed.");
                        dgvAutoComplete.SendToBack();
                    }
                    else
                    {
                        string BaseName = ServiceDay + " " + dtpMonth.Value.ToString("MMMM yyyy");
                        string DocName = BaseName + " Roster";
                        string AdHeader = fmLoginx.strChurchAcronym + "-" + BaseName + " " + EmailType;
                        PreviewXDialog.DGVPdfPrint(dgvAutoComplete, AdHeader, DocName, true);
                        AttachmentList.Clear(); AttachmentList.Add(fmLoginx.PathResource + DocName + ".pdf");
                        string strDetail = "Dear Church Program Coordinator,"
                              + "\n\nFind attached is the church roster for this month. Please, send a reply email if there is any mistake. "
                              + "\n\nMay the all sufficient God remember your labour of love in His kingdom and reward you accordingly in Jesus' name , Amen.";
                        if (txEmailTo.Text.Length > 6 && txEmailTo.Text.Contains('@') == true)
                            fmLoginx.SendEmailSt("2", txEmailTo.Text, "", AdHeader, strDetail, "", AttachmentList, NoneNull);
                        File.Delete(fmLoginx.PathResource + ServiceDay + " " + dtpMonth.Value.AddMonths(-3).ToString("MMMM yyyy") + ".pdf");
                        MessageBox.Show(" The month roster is successfully created and emailed.");
                    }
                }
                valRetry = false;
                return dialogResult;
            }
            else
            {
                valRetry = true;
                if (dialogResult == DialogResult.Yes) MessageBox.Show("You have not typed in Coordinator's email.");
                return dialogResult;
            }
        }
        public static DialogResult dialogExhort(int NoOfCol, DataGridView dgvRoster, DataGridView dgvEdit, ref bool ExhRetry)
        {
            #region
            Form form = new Form();
            Label lbInfo = new Label();
            TextBox txEmailTo = new TextBox();
            Button buttonYes = new Button();
            Button buttonNo = new Button();

            form.Text = " Email Exhortation";
            lbInfo.Text = "Would like to send a copy of this month roster to the ministers that would be taking exhortations?";
            txEmailTo.Text = " Email Exhortation";
            buttonYes.Text = "Yes"; buttonYes.DialogResult = DialogResult.Yes;
            buttonNo.Text = "No"; buttonNo.DialogResult = DialogResult.No;
            lbInfo.SetBounds(5, 10, 250, 25);
            txEmailTo.SetBounds(20, 50, 180, 20);
            buttonYes.SetBounds(20, 85, 80, 23); buttonNo.SetBounds(120, 85, 80, 23);
            buttonYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            FmLogin fmLoginx = new FmLogin();
            string NoneNull = fmLoginx.NoneNull;
            form.ClientSize = new Size(250, 120);
            form.Controls.AddRange(new Control[] { lbInfo, txEmailTo, buttonYes, buttonNo });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonYes;
            form.CancelButton = buttonNo;
            #endregion

            DialogResult dialogResult = form.ShowDialog();
            if (form.DialogResult == DialogResult.Yes)
            {
                string Title = "", FName = "", MName = "", LName = "", EmailTo = "", MphoneNo = "", DutyTime = "", DutyHeader = "";
                char[] sep = new char[] { ';' }; char[] sepNamex = new char[] { ' ' };
                string[] splitName, splitCon;
                for (int j = 0; j <= NoOfCol; j++)//Columns
                {
                    if (j != 0 && j != 2)
                    {
                        int rowxx = 0;
                        if (j == 1)
                        {
                            rowxx = 8;
                            DutyTime = DutyTime + ";" + dgvRoster.Rows[rowxx].Cells[0].FormattedValue.ToString();
                        }
                        else
                        {
                            rowxx = 7;
                            DutyTime = DutyTime + ";" + dgvRoster.Rows[rowxx].Cells[2].FormattedValue.ToString();
                        }

                        DutyHeader = DutyHeader + ";" + dgvRoster.Columns[j].HeaderText.Replace("\n", " "); //ProgramCol

                        splitName = dgvRoster.Rows[rowxx].Cells[j].FormattedValue.ToString().Split(sepNamex, StringSplitOptions.RemoveEmptyEntries);
                        FName = FName + ";" + splitName[0];
                        MName = MName + ";" + "None";
                        LName = LName + ";" + splitName[1];

                        splitCon = dgvEdit.Rows[rowxx].Cells[j].FormattedValue.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        Title = Title + ";" + splitCon[0];
                        EmailTo = EmailTo + ";" + splitCon[1];
                        MphoneNo = MphoneNo + ";" + splitCon[2];
                    }
                }
                FileStream fsx = new FileStream(fmLoginx.PathMsg + "RosterEmailMessage.msgx", FileMode.Open);
                StreamReader srx = new StreamReader(fsx);
                string EmailSubject = srx.ReadLine();
                srx.ReadLine();
                string EmailBody = srx.ReadToEnd();
                fsx.Close(); srx.Close();
                EmailSubject = "Service Roster from RCCG FOLP for preaching"; // EmailSubject.Replace("#DutyHeader", DutyHeader);
                EmailBody = EmailBody.Replace("#DutyTime", "#Para1");
                EmailBody = EmailBody.Replace("#DutyHeader", "#Para2");
                fmLoginx.SendEmailDt("2", EmailTo, "", EmailSubject, EmailBody, "", null, NoneNull, Title, FName, MName, LName, DutyTime, DutyHeader, "");

                FileStream fsz = new FileStream(fmLoginx.PathMsg + "RosterSMSMessage.msgx", FileMode.Open);
                StreamReader srz = new StreamReader(fsz);
                string SMSMessage = srz.ReadToEnd();
                fsz.Close(); srz.Close();
                SMSMessage = SMSMessage.Replace("#DutyTime", "#Para1");
                SMSMessage = SMSMessage.Replace("#DutyHeader", "#Para2");
                fmLoginx.SendSMSDt(MphoneNo, SMSMessage, "", Title, FName, MName, LName, DutyTime, DutyHeader, "", fmLoginx.InternetPhoneSMS);

                MessageBox.Show(" This month roster is successfully sent to ministers who will be preaching.");
            }
            ExhRetry = false;
            return dialogResult;
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            DocName = dtpMonth.Value.ToString("MMMM yyyy ") + ServiceDay + " Roster";
            AdHeader = fmLogin.strChurchAcronym + "-" + DocName;
            PreviewXDialog.DGVPdfPrint(dgvRoster, AdHeader, DocName, true);
            PreviewXDialog.PrintDGVX(dgvRoster, AdHeader, DocName, true);
        }
        //+++++++++++++++++++++++++++++ End End  End ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
}
