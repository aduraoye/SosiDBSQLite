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
    public partial class FmBankLodge : Form
    {
        FmLogin fmLogin;
        FmRecords fmRecords;
        AutoCompleteStringCollection source;
        PrintPreviewXDialog PreviewXDialog;
        string Table = "BankLodgeInfo", Desription, strId0, strOfferingId, OfferingDateTime, NoneNull, OtherId;
        string EmailTo = "", Title = "", FName = "", PayDate = "", LName = "", RegID = "", Amount = "", OfferingDesc = "", Paymethod;
        bool IsStmnt, SendEmail; 
        public FmBankLodge()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmRecords = new FmRecords();
            PreviewXDialog = new PrintPreviewXDialog();
            fmLogin.niBackground.Visible = false;
        }

        private void BankLodge_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Bank Lodge] -" + fmLogin.strChurchAcronym;
            NoneNull = fmLogin.NoneNull;
            dtpOfferingFrom.Value = dtpOffering.Value.AddMonths(-5);
            if (IsStmnt == true) SelectStmntDB(); else SelectDB();
            tscbPayMethod.SelectedIndex = 2;
            Calculate();
            fmLogin.SavingUpdate("SqlBkl", "Loading");
        }
        public void BankLodgeStatement()
        {
            btStatement.Visible = true; dtpOfferingFrom.Visible = true;
            cbOfferingStmnt.Visible = true;    IsStmnt = true;// SelectStmntDB();
            btAdd.Visible = false; lbAUD.Visible = false;
            lbTotal.Visible = false; txBLTotal.Visible = false;
            cbNameSearch.Enabled = false; txNameSearch.Visible = false;
            lbOfferingDescript.Visible = false;
            SendEmail = false;
            AutoComplete();
            cbRegId.SelectedIndex = 0;
            cbOfferingStmnt.Enabled = true; cbOfferingStmnt.SelectedIndex = 2;
            this.ShowDialog();
        }
        public void BankLodgeAdd(string payMethod, string OfferingId, string Desript)//, ref string OfferingTotalAmt)//Cash or Bank lodged Offerings
        {
            btStatement.Visible = false; dtpOfferingFrom.Visible =false;
            cbOfferingStmnt.Visible = false;
            btAdd.Visible = true; lbAUD.Visible = true; 
            lbTotal.Visible = true; txBLTotal.Visible = true;
            cbNameSearch.Enabled = true; txNameSearch.Visible = true;
            lbOfferingDescript.Visible = true;
            if (payMethod == "Bank") SendEmail = false; else SendEmail = true;
            Paymethod = payMethod;
            strOfferingId = OfferingId;
            lbOfferingDescript.Text = Desription = Desript;
            IsStmnt = false;// SelectDB();
            AutoComplete();
            cbNameSearch.SelectedIndex = 0;
            this.ShowDialog();
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        private void InsertDB()
        { //Insert BankLodge details PayMethod VARCHAR(5)
            OtherId = (BankMaxId() + 1).ToString();
            OfferingDateTime = dtpOffering.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO " + Table + " (Date_Time, OfferingId, RegId, Offerings, Amount, OtherId, PayMethod) VALUES ('" + OfferingDateTime
                         + "', '" + strOfferingId + "', '" + RegIdSplit() + "', '" + lbOfferingDescript.Text + "', '" + txBLAmount.Text + "', '" + OtherId + "', '" + Paymethod + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            btAdd.Enabled = false;
        }        
        private void SelectDB() 
        {  // Select 
            string queryx = "SELECT Id, OfferingId, Date_Time, RegId, Amount, Offerings, PayMethod, OtherId FROM " + Table + " WHERE OfferingId = '" 
                          + strOfferingId + "' AND Offerings = '" + lbOfferingDescript.Text + "' ORDER BY Date_Time DESC";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvBankLodge.DataSource = dTable;
            }
            dgvBankLodge.Columns[0].Visible = false;
            dgvBankLodge.Columns[1].Visible = false;
            dgvBankLodge.Columns[6].Visible = false;
            dgvBankLodge.Columns[7].Visible = false;
            dgvBankLodge.Columns[2].Width = 85; dgvBankLodge.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvBankLodge.Columns[3].Width = 55;
            dgvBankLodge.Columns[4].Width = 80;
            dgvBankLodge.Columns[5].Width = 140;
            Calculate();
        }
        private void SelectStmntDB()
        { // Select 
            string queryx = "";
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string ODTTo = dtpOffering.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            char[] sep = new char[] { ' ' };
            string[] splitColx;
            splitColx = cbRegId.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);// txRegId.Text = splitCol[0];  
            if (cbOfferingStmnt.Text == "All Offerings")
            {
                if (cbRegId.SelectedIndex == 0) queryx = "SELECT Id, OfferingId, Date_Time, RegId, Amount, Offerings, PayMethod, OtherId FROM " + Table + " WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
                else queryx = "SELECT Id, OfferingId, Date_Time, RegId, Amount, Offerings, PayMethod FROM " + Table + " WHERE RegId = '" + splitColx[0] + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            }
            else
            {
                //cbRegIdStmnt.SelectedIndex = 2;
                if (cbRegId.SelectedIndex == 0) queryx = "SELECT Id, OfferingId, Date_Time, RegId, Amount, Offerings, PayMethod FROM " + Table + " WHERE  Offerings = '" + cbOfferingStmnt.Text + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
                else queryx = "SELECT Id, OfferingId, Date_Time, RegId, Amount, Offerings, PayMethod FROM " + Table + " WHERE RegId = '" + splitColx[0] + "' AND Offerings = '" + cbOfferingStmnt.Text + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            }
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvBankLodge.DataSource = dTable;
            }
            dgvBankLodge.Columns[0].Visible = false;
            dgvBankLodge.Columns[1].Visible = false;
            dgvBankLodge.Columns[6].Visible = false;
            //dgvBankLodge.Columns[7].Visible = false;
            dgvBankLodge.Columns[2].Width = 85; dgvBankLodge.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvBankLodge.Columns[3].Width = 55;
            dgvBankLodge.Columns[4].Width = 80;
            dgvBankLodge.Columns[5].Width = 140;
        }       
        private void UpdateDB()
        {//UPDATE
            string OfferingDateTime = dtpOffering.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "UPDATE " + Table + " SET Date_Time = '" + OfferingDateTime + "', RegId = '" + RegIdSplit()
                         + "', Amount = '" + txBLAmount.Text + "', OfferingId   = '" + tstxAmount.Text + "' WHERE Id = '" + strId0 + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            SelectDB();
            UpdateBank(OfferingDateTime);
            MessageBox.Show("Data updated"); 
        }
        private void UpdateBank(string Date_Time)
        {//UPDATE  
            string Descriptx = lbOfferingDescript.Text + " lodgement for RegId = " + RegIdSplit();
            string query = "UPDATE AccountInfo  SET Date_Time = '" + Date_Time + "', Amount = '" + txBLAmount.Text + "', Descript ='" + Descriptx + "' WHERE Id = '" + OtherId + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if(cbNameSearch.SelectedIndex < 3)
                if (source.Contains(txNameSearch.Text) == false && btAdd.Text == "Add") { MessageBox.Show("You have not selected appropriate person's details."); return; }

            DialogResult DgResult = MessageBox.Show("Are the details provided correct and would you like to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DgResult == DialogResult.Yes)
            {
                if (btAdd.Text == "Add")
                {
                    InsertDB();
                    SelectDB();
                    txBLTotal.Text = (float.Parse(txBLTotal.Text) + float.Parse(txBLAmount.Text)).ToString("F2");
                    if (RegIdSplit() != "0")
                    {
                        DataTable dTable;
                        string queryx = "SELECT Title, First_Name, Last_Name, MPhone, Email FROM RegisterInfo WHERE Id = '" + RegIdSplit() + "';";
                        using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                        {
                            SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                            SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                            dTable = new DataTable();
                            MyAdapter.SelectCommand = cmdOfferingx;
                            MyAdapter.Fill(dTable);
                        }

                        Title = Title + ";" + dTable.Rows[0][0].ToString(); //Title
                        FName = FName + ";" + dTable.Rows[0][1].ToString();//First name
                        LName = LName + ";" + dTable.Rows[0][2].ToString();// Last Name
                        PayDate = PayDate + ";" + dtpOffering.Value.ToString("dddd, MMMM d, yyyy"); // Middle Name
                        //MphoneNo = MphoneNo + ";" + dTable.Rows[0][3].ToString();//Mobile Phone
                        EmailTo = EmailTo + ";" + dTable.Rows[0][4].ToString();//Email
                        Amount = Amount + ";" + txBLAmount.Text; //Para1
                        RegID = RegID + ";" + RegIdSplit();//Para2
                        OfferingDesc = OfferingDesc + ";" + lbOfferingDescript.Text; //Para3
                    }
                }
                else UpdateDB();
                fmLogin.SavingUpdate("SqlBkl", "Saving");
            }
            else { return; }
            btAdd.Enabled = true; txNameSearch.Text = ""; 
        }              
        private void mnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBankLodge.CurrentRow == null) { MessageBox.Show("You have not selected any row!"); return; }
                else
                {
                    int SelIndex = dgvBankLodge.CurrentRow.Index;
                    strId0 = dgvBankLodge.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                    string queryx = "SELECT * FROM " + Table + " WHERE Id = '" + strId0 + "';";
                    using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                        SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                        DataTable dTable = new DataTable();
                        MyAdapter.SelectCommand = cmdOfferingx;
                        MyAdapter.Fill(dTable);
                        
                        dtpOffering.Value = DateTime.Parse(dTable.Rows[0][1].ToString());
                        tstxAmount.Text = dTable.Rows[0][2].ToString();
                        txNameSearch.Text = RegIdEdit(dTable.Rows[0][3].ToString()); 
                        txBLAmount.Text = dTable.Rows[0][5].ToString();
                        OtherId = dTable.Rows[0][6].ToString();
                        tscbPayMethod.Text = dTable.Rows[0][7].ToString();
                    }

                    if(IsStmnt == false) Calculate();
                    btAdd.Text = "Update";
                    btAdd.Enabled = true;
                }
        }       
        private int MaxId()
        {
            string strPixId = "SELECT Id FROM " + Table + " ORDER BY Id DESC LIMIT 1";
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
        private int BankMaxId()
        {
            string strPixId = "SELECT Id FROM AccountInfo ORDER BY Id DESC LIMIT 1";
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
        
        private void Clear()
        {
            txBLAmount.Text = "0.00";
            cbRegId.SelectedIndex = 0; 
        }
        private void Calculate()
        {
            float Curr = 0.0f;
            for (int j = 0; j <= dgvBankLodge.Rows.Count - 1; j++)
            {
               Curr = Curr + float.Parse(dgvBankLodge.Rows[j].Cells[4].FormattedValue.ToString());
            }
            txBLTotal.Text = Curr.ToString("F2");
        }
        private void BankLodge_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                float Curr = 0.0f;
                for (int j = 0; j <= dgvBankLodge.Rows.Count - 1; j++)
                {
                    if (dgvBankLodge.Rows[j].Cells[6].FormattedValue.ToString() != "Cash") Curr = Curr + float.Parse(dgvBankLodge.Rows[j].Cells[4].FormattedValue.ToString());
                }
                txBLTotal.Text = Curr.ToString("F2");

                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(txBLTotal.Text);
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            
            if (SendEmail == true)
            {
                DialogResult DialogMsg = MessageBox.Show("Would you like to send email messages to the offering payers?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogMsg == DialogResult.No) { return; }
                else
                {
                    string EmailSubject = fmLogin.strChurchAcronym + " Church: Payment Acknowledgment";
                    string EmailBody = "Dear #Title #FName #LName (RegID: #Para2),"
                           + "\n\n"
                           + "We acknowledge the payment of AUD$#Para1 on #MName as your #Para3 into the church account."
                           + "\n\n"
                           + "May the all sufficient God favour you and provide for all your needs in Jesus' name, Amen.";
                    fmLogin.SendEmailDt("4", EmailTo, "", EmailSubject, EmailBody, "", null, NoneNull, Title, FName, PayDate, LName, Amount, RegID, OfferingDesc);
                }
            }
        }
        private void AutoComplete()
        {// Select
            if (IsStmnt == false)
            {
                //cbRegId.Items.Add("Other Deposit"); cbRegId.Items.Add("00 No Name");
                string queryx = "SELECT Id, First_Name, Last_Name FROM RegisterInfo ";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    DataTable dTable = new DataTable();
                    MyAdapter.SelectCommand = cmdRegister;
                    MyAdapter.Fill(dTable);

                    txNameSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txNameSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    source = new AutoCompleteStringCollection(); //source.

                    if (cbNameSearch.SelectedIndex == 0) // = RegId 
                        for (int i = 0; i < dTable.Rows.Count; i++) source.Add(dTable.Rows[i][0].ToString() + " " + dTable.Rows[i][1].ToString() + " " + dTable.Rows[i][2].ToString());
                    else if (cbNameSearch.SelectedIndex == 1) // = First name 
                        for (int i = 0; i < dTable.Rows.Count; i++) source.Add(dTable.Rows[i][1].ToString() + " " + dTable.Rows[i][2].ToString() + " " + dTable.Rows[i][0].ToString());
                    else if (cbNameSearch.SelectedIndex == 2) // = Last name 
                        for (int i = 0; i < dTable.Rows.Count; i++) source.Add(dTable.Rows[i][2].ToString() + " " + dTable.Rows[i][1].ToString() + " " + dTable.Rows[i][0].ToString());
                    else txNameSearch.Text = "0 No Name";  //if (cbNameSearch.SelectedIndex == 3) // = No Name
                      
                    txNameSearch.AutoCompleteCustomSource = source;
                }
            }
            else
            {
                string queryx = "SELECT Id || ' ' || First_Name || ' ' || Last_Name FROM RegisterInfo ";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    DataTable dTable = new DataTable();
                    MyAdapter.SelectCommand = cmdRegister;
                    MyAdapter.Fill(dTable);

                    cbRegId.Items.Add("All RegID");
                    for (int i = 0; i < dTable.Rows.Count; i++) cbRegId.Items.Add(dTable.Rows[i][0].ToString());
                }
            }            
        }       
        
        private void cbRegId_TextChanged(object sender, EventArgs e)
        {
            if (cbRegId.SelectedIndex != 0) lbOfferingDescript.Text = Desription; 
            txBLAmount.Focus();
        }
        private string RegIdSplit()
        {
            char[] sep = new char[] { ' ' }; 
            string[] splitColm;

            if (IsStmnt == true) //BankLodge for statements
            {
                splitColm = cbRegId.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                return splitColm[0];
            }
            else // IsStmnt == false - BankLodge for Tithe and othe offerings
            {
                splitColm = txNameSearch.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if(cbNameSearch.SelectedIndex == 0) return splitColm[0];
                else if (cbNameSearch.SelectedIndex == 1 || cbNameSearch.SelectedIndex == 2) return splitColm[2];
                //else if (cbNameSearch.SelectedIndex == 2) return splitColm[2];
                else return "0";
            }
        }
        private string RegIdEdit(string RegIdx)
        {
            string strPixId = "SELECT Id || ' ' || First_Name || ' ' || Last_Name FROM RegisterInfo WHERE Id = '" + RegIdx + "';";
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strPixId, conPix))
                {
                    if (cmd.ExecuteScalar() != DBNull.Value) return Convert.ToString(cmd.ExecuteScalar());
                    else return "0 No Name";
                }
            }
        }

        private void cbOfferingStmnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsStmnt == true) SelectStmntDB();
           //if(IsStmnt == false) SelectStmntDB(); else SelectStmntDB();
        }       
        private void mnPrint_Click(object sender, EventArgs e)
        {
            string DocName = "Bank Lodgement Statement-" + DateTime.Now.ToString("ddMMyy");
            string AdHeader = fmLogin.strChurchAcronym + "-" + DocName;
            PreviewXDialog.DGVPdfPrint(dgvBankLodge, AdHeader, DocName, false);
            PreviewXDialog.PrintDGVX(dgvBankLodge, AdHeader, DocName, false);
        }

        private void mnNew_Click(object sender, EventArgs e)
        {
            btAdd.Enabled = true;
            btAdd.Text = "Add";
        }
        private void mnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBankLodge.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvBankLodge.CurrentRow.Index;
                string strId = dgvBankLodge.Rows[SelIndex].Cells[0].FormattedValue.ToString();

                DialogResult mgUpdate = MessageBox.Show("Do you want to delete the selected row data?", "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes)
                {
                    string queryx = "DELETE FROM " + Table + " WHERE Id = '" + strId + "';";
                    using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                        conTblx.Open();
                        cmdOfferingx.ExecuteNonQuery();
                    }

                    dgvBankLodge.Rows.RemoveAt(SelIndex);
                    dgvBankLodge.Refresh();
                    Calculate();

                    string queBank = "DELETE FROM AccountInfo WHERE Id = '" + (int.Parse(strId) + 1).ToString() + "';";
                    using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdBank = new SQLiteCommand(queBank, conTbly);
                        conTbly.Open();
                        cmdBank.ExecuteNonQuery();
                    }
                }
                else return; 
            }
        
        }

        private void cbRegId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsStmnt == true) SelectStmntDB();
        }
        private void cbNameSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbNameSearch.SelectedIndex > 2) txNameSearch.Enabled = false;
            else { txNameSearch.Enabled = true; txNameSearch.Text = ""; }
            AutoComplete(); 
        }
        private void tsbtCalAmount_Click(object sender, EventArgs e)
        {
            float Curr = 0.0f;
            for (int j = 0; j <= dgvBankLodge.Rows.Count - 1; j++)
            {
                if (tscbPayMethod.SelectedIndex == 0)
                { if (dgvBankLodge.Rows[j].Cells[6].FormattedValue.ToString() != "Cash") Curr = Curr + float.Parse(dgvBankLodge.Rows[j].Cells[4].FormattedValue.ToString()); }
                else if (tscbPayMethod.SelectedIndex == 1)
                { if (dgvBankLodge.Rows[j].Cells[6].FormattedValue.ToString() == "Cash") Curr = Curr + float.Parse(dgvBankLodge.Rows[j].Cells[4].FormattedValue.ToString()); }
                else Curr = Curr + float.Parse(dgvBankLodge.Rows[j].Cells[4].FormattedValue.ToString());         
            }
            tstxAmount.Text = Curr.ToString();
        }               
        // .......;;;; End
    }
}
