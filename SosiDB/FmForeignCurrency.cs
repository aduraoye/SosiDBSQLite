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
    public partial class FmForeignCurrency : Form
    {
        FmLogin fmLogin;
        FmRecords fmRecords;
        string Table = "ForeignCurrencyInfo", strId0, strOfferingId;
        string EmailTo = "", FName = "", LName = "", Title = "", EmailTox = "", FNamex = "", LNamex = "", Titlex = "", NoneNull;
        string[] splitCol; char[] sep = new char[] { ';' };
        bool IsNewAcct = false;

        public FmForeignCurrency()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmRecords = new FmRecords();
            fmLogin.niBackground.Visible = false;
        }

        private void ForeignCurrency_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Foreign Currency] -" + fmLogin.strChurchAcronym;
            dgvEdit.SendToBack(); cbCustodianDetails.SendToBack(); cbHandOverToDetails.SendToBack(); cbCurrSymbLogic.SendToBack();
            SelectDB();
            cbCurrencySymbol.SelectedIndex = 0; tscbTransact.SelectedIndex = 0; 
            NoneNull = fmLogin.NoneNull;
            cbHandOverTo.Enabled = false;
            HaveCustodian();
            fmLogin.SavingUpdate("SqlCurr", "Loading");
        }
        public void ForeignLodge()
        {
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                strOfferingId = sr.ReadLine();
                fs.Close(); sr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            this.ShowDialog();
        }
        public void ForeignView()
        {
          strOfferingId = "00";
          this.ShowDialog();
        }
        private void cbCurrencySymbol_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            lbOtherCurrency.Text = lbForeignCurrency.Text = cbCurrencySymbol.Text;
            mnAdd.Enabled = true;
            SelectDB();
        }

        private void InsertDB()
        { //Insert Foreign currency details    strOfferingId = "0";
            string Amount, AUDAmt;
            if (tscbTransact.Text == "Convert") { Amount = "-" + txCurrencyAmount.Text; AUDAmt = "-" + txCurrencyAUD.Text; }
            else { Amount = txCurrencyAmount.Text; AUDAmt = txCurrencyAUD.Text; }

            string OfferingDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO " + Table + " (Date_Time, OfferingId, Custodian, CustDetails, Currency, Transact, Amount, Rate, Comm, AUD$, Descript) VALUES ('"
                         + OfferingDateTime + "', '" + strOfferingId + "', '" + cbCustodian.Text + "', '" + cbCustodianDetails.Text
                         + "', '" + cbCurrencySymbol.Text + "', '" + tscbTransact.Text + "', '" + Amount + "', '" + txCurrencyRate.Text + "', '"
                         + txCommission.Text + "', '" + AUDAmt + "', '" + txDescript.Text + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            mnAdd.Enabled = false;
        }
        private void SelectDB()
        { // Select 
            string queryx = "";
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string ODTTo = dtpOfferingTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            if (cbCurrencySymbol.Text == "All")
            {
                if (cbCustodian.Text == "All") queryx = "SELECT Id, Date_Time, Currency, Amount, Rate, AUD$, Descript FROM " + Table + " WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time";
                else queryx = "SELECT Id, Date_Time, Currency, Amount, Rate, AUD$, Descript FROM " + Table + " WHERE Custodian = '" + cbCustodian.Text + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time";
            }
            else
            {
                if (cbCustodian.Text == "All") queryx = "SELECT Id, Date_Time, Currency, Amount, Rate, AUD$, Descript FROM " + Table + " WHERE Currency = '" + cbCurrencySymbol.Text + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time";
                else queryx = "SELECT Id, Date_Time, Currency, Amount, Rate, AUD$, Descript FROM " + Table + " WHERE Custodian = '" + cbCustodian.Text + "' AND Currency = '" + cbCurrencySymbol.Text + "' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time";
            }
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvForeignCurr.DataSource = dTable;
            }
            dgvForeignCurr.Columns[0].Visible = false;
            dgvForeignCurr.Columns[1].Width = 85; dgvForeignCurr.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvForeignCurr.Columns[2].Width = 73;
            dgvForeignCurr.Columns[3].Width = 73;
            dgvForeignCurr.Columns[4].Width = 73;
            dgvForeignCurr.Columns[5].Width = 73;
            dgvForeignCurr.Columns[6].Width = 170;
            Calculate();
        }
        private void UpdateDB()
        {//UPDATE  ForeignCurrency
            AUDz = 0.0f;
            string query = "UPDATE " + Table + " SET Currency = '" + cbCurrencySymbol.Text + "', Amount = '" + txCurrencyAmount.Text + "', Rate = '"
                            + txCurrencyAUD.Text + "', AUD$ = '" + txCurrencyAUD.Text + "' WHERE Id = '" + strId0 + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            SelectDB();
            DialogResult mgUpdate = MessageBox.Show("Do you want to continue?", "Data updated", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgUpdate == DialogResult.Yes) { Clear(); mnAdd.Enabled = false; }
            else { mnAdd.Enabled = true; return; }
            //UpdateBank();
        }        

        private void mnEdit_ButtonClick(object sender, EventArgs e)
        {
            if (dgvForeignCurr.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvForeignCurr.CurrentRow.Index;
                strId0 = dgvForeignCurr.Rows[SelIndex].Cells[0].FormattedValue.ToString();
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
                dtpOfferingTo.Value = DateTime.Parse(dgvEdit.Rows[0].Cells[1].FormattedValue.ToString());
                strOfferingId = dgvEdit.Rows[0].Cells[2].FormattedValue.ToString();
                cbCustodian.Text = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString();
                cbCustodianDetails.Text = dgvEdit.Rows[0].Cells[4].FormattedValue.ToString();
                cbCurrencySymbol.Text = dgvEdit.Rows[0].Cells[5].FormattedValue.ToString();
                tscbTransact.Text = dgvEdit.Rows[0].Cells[6].FormattedValue.ToString();
                txCurrencyAmount.Text = dgvEdit.Rows[0].Cells[7].FormattedValue.ToString();
                txCurrencyRate.Text = dgvEdit.Rows[0].Cells[8].FormattedValue.ToString();
                txCommission.Text = dgvEdit.Rows[0].Cells[9].FormattedValue.ToString();
                txCurrencyAUD.Text = dgvEdit.Rows[0].Cells[10].FormattedValue.ToString();
                txDescript.Text = dgvEdit.Rows[0].Cells[11].FormattedValue.ToString();
                AUDz = 0.0f; Calculate();

                mnUpdate.Enabled = true;
            }
        }
        private void mnDelete_Click(object sender, EventArgs e)
        {
            if (dgvForeignCurr.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvForeignCurr.CurrentRow.Index;
                string strId = dgvForeignCurr.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                string queryx = "DELETE FROM " + Table + " WHERE Id = '" + strId + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    conTblx.Open();
                    cmdOfferingx.ExecuteNonQuery();
                }

                dgvForeignCurr.Rows.RemoveAt(SelIndex);
                dgvForeignCurr.Refresh();
                Calculate();
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
       
        private void HaveCustodian()
        {
            DataTable dTable = new DataTable();
            string queryx = "SELECT Custodian, CustDetails FROM " + Table + " ORDER BY Id DESC, Date_Time DESC LIMIT 1";

            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                if (dTable.Rows.Count != 0)//Account has been taken over once
                {
                    IsNewAcct = false;
                    cbCustodian.Text = dTable.Rows[0][0].ToString(); cbCustodian.Enabled = false;
                    cbCustodianDetails.Text = dTable.Rows[0][1].ToString(); //MessageBox.Show(cbCustodianDetails.Text);
                }
                else//Account yet to be taken over or created
                {
                    IsNewAcct = true;
                    LoadCustodian();
                    cbCustodian.Text = "Custodian"; cbCustodian.Enabled = true;
                }
            }
        }
        private void LoadCustodian()
        {
            DataTable dTable;
            string queryx = "SELECT UserName || ':' || First_Name || ';' || Last_Name || ';' || Email || ';' || Title FROM LoginInfo";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }

            cbCustodian.Items.Clear(); cbCustodianDetails.Items.Clear();
            char[] sep = new char[] { ':' };
            for (int i = 0; i <= Count() - 1; i++)
            {
                splitCol = dTable.Rows[i][0].ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                cbCustodian.Items.Add(splitCol[0]); cbCustodianDetails.Items.Add(splitCol[1]);
            }
            cbCustodian.Items.Add("All"); cbCustodianDetails.Items.Add("All;All;All;All");
        }
        private void LoadCustodianHandOver()
        {
            DataTable dTable;
            string queryx = "SELECT UserName || ':' || First_Name || ';' || Last_Name || ';' || Email || ';' || Title FROM LoginInfo";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }

            cbHandOverTo.Items.Clear(); cbHandOverToDetails.Items.Clear();
            char[] sep = new char[] { ':' }; string[] splitColx;
            for (int i = 0; i <= Count() - 1; i++)
            {
                splitColx = dTable.Rows[i][0].ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                cbHandOverTo.Items.Add(splitColx[0]); cbHandOverToDetails.Items.Add(splitColx[1]);
            }
        }
        private void tscbTransact_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscbTransact.SelectedIndex == 2)
            {
                cbHandOverTo.Enabled = true;
                LoadCustodianHandOver();
            }
            else cbHandOverTo.Enabled = false;
        }
        private void cbCustodian_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCustodianDetails.SelectedIndex = cbCustodian.SelectedIndex;
            cbCurrSymbLogic.SelectedIndex = cbCustodian.SelectedIndex;
            //cbHandOverTo.Text = cbCustodian.Text;
        }
        private void cbHandOverTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbHandOverToDetails.SelectedIndex = cbHandOverTo.SelectedIndex;
        } 
        private void SendEmailFtn()// email for custodian
        {
            DialogResult DialogMsg = MessageBox.Show("Would you like to send an email message to the account custodian?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogMsg == DialogResult.No) { return; }
            else
            {
                string[] splitDesc = cbCustodianDetails.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                FName = splitDesc[0];//First name
                LName = splitDesc[1];// Last Name
                EmailTo = splitDesc[2];//Email
                Title = splitDesc[3];//Title
                               
                if (tscbTransact.SelectedIndex == 2)
                {
                    string strInfo = "\n";
                    for (int i = 0; i < cbCurrencySymbol.Items.Count-1; i++)
                    {
                        float Curr = 0.0f; cbCurrencySymbol.SelectedIndex = i;
                        for (int j = 0; j <= dgvForeignCurr.Rows.Count - 1; j++)
                        {
                            Curr = Curr + float.Parse(dgvForeignCurr.Rows[j].Cells[3].FormattedValue.ToString());
                        }
                        if (Curr != 0.0f) strInfo = strInfo + cbCurrencySymbol.Items[i].ToString() + Curr.ToString() + "\n";
                    }

                    if (IsNewAcct == false)
                    {
                        string EmailSubject = fmLogin.strChurchAcronym + " Church: Confirmation for handing over the custody of the church foreign currency account.";
                        string EmailBody = "Beloved " + Title + " " + FName + " " + LName + ","
                               + "\n\n"
                               + "We appreciate your service to the kingdom of God in your capacity as the custodian of the church foreign currency account for sometime."
                               + "\n\n We confirm the handing over of this account with details of its balance as of " + dtpOfferingTo.Value.ToLongDateString() + " as follows:" + strInfo
                               + "\nPlease send a reply email to confirm this statement. Thank you Sir/Ma."
                               + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                        fmLogin.SendEmailSt("4", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull);
                    }

                    string EmailSubjectx = fmLogin.strChurchAcronym + " Church: Confirmation for taking over the custody of church the church foreign currency account.";
                    string EmailBodyx = "Beloved " + Titlex + " " + FNamex + " " + LNamex + ","
                           + "\n\n"
                           + "We appreciate your acceptance to be the custodian of the church foreign currency account. "
                           + "\n\n The details of its balance as of " + dtpOfferingTo.Value.ToLongDateString() + " are as follows:" + strInfo
                           + "\nPlease send a reply email to confirm this statement. Thank you Sir/Ma."
                           + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                    fmLogin.SendEmailSt("4", EmailTox, "", EmailSubjectx, EmailBodyx, "", null, fmLogin.NoneNull);
                }
                else
                {
                    string strInfo = "\n";
                    for (int i = 0; i < cbCurrencySymbol.Items.Count; i++)
                    {
                        if (cbCurrSymbLogic.Items[i].ToString() != "0.00") strInfo = strInfo + cbCurrencySymbol.Items[i].ToString() + cbCurrSymbLogic.Items[i].ToString() + "\n";
                    }

                    string EmailSubject = fmLogin.strChurchAcronym + " Church: Foreign currency account " + tscbTransact.Text + " Confirmation";
                    string EmailBody = "Beloved " + Title + " " + FName + " " + LName + ","
                           + "\n\nThe following amount was " + tscbTransact.Text.ToLowerInvariant() + "ed into the church foreign currency account "
                           + "under your care as follows: \nDate: " + dtpOfferingTo.Value.ToLongDateString() + strInfo
                           + "\nPlease send a reply email to confirm this statement. Thank you Sir/Ma."
                           + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                    fmLogin.SendEmailSt("4", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull);
                }
            }
        }

        private void mnAdd_Click(object sender, EventArgs e)
        {
            if (cbCustodian.Text == "Custodian")
            {
                MessageBox.Show("You have not selected a custodian for the account.");
                cbCustodian.Focus(); return;
            }

            if (tscbTransact.SelectedIndex == 2 && cbHandOverTo.Text == "Custodian")
            {
                MessageBox.Show("You have not selected a custodian to hand over the account to.");
                cbHandOverTo.Focus(); return;
            }

            txCurrencyAmount.Text = float.Parse(txCurrencyAmount.Text).ToString("F2");
            txCommission.Text = float.Parse(txCommission.Text).ToString("F2");
            txCurrencyRate.Text = float.Parse(txCurrencyRate.Text).ToString("F4");
            DialogResult DgResult = MessageBox.Show("Are the details provided correct?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DgResult == DialogResult.Yes)
            {         
                if (tscbTransact.SelectedIndex == 2)
                {
                    string[] splitDesx = cbHandOverToDetails.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    FNamex = splitDesx[0];//First name
                    LNamex = splitDesx[1];// Last Name
                    EmailTox = splitDesx[2];//Email
                    Titlex = splitDesx[3];//Title

                    txDescript.Text = "Handling Over the foreign currency account from " + cbCustodian.Text + " to " + cbHandOverTo.Text;
                    cbCustodian.Text = cbHandOverTo.Text;
                    InsertDB();
                    SendEmailFtn();
                }
                else
                {
                    if (cbCurrencySymbol.Text != "All")
                    {
                        txCurrencyAUD.Text = (float.Parse(txCurrencyAmount.Text) * float.Parse(txCurrencyRate.Text) + float.Parse(txCommission.Text)).ToString("F2");
                        InsertDB();
                        cbCurrSymbLogic.Items[cbCurrencySymbol.SelectedIndex] = (float.Parse(cbCurrSymbLogic.Items[cbCurrencySymbol.SelectedIndex].ToString()) + float.Parse(txCurrencyAmount.Text)).ToString("F2");
                        SelectDB();
                    }
                    else
                    {
                        MessageBox.Show("You have not selected any currency!");
                        cbCurrencySymbol.SelectedIndex = 0;
                        return;
                    }
                }
                fmLogin.SavingUpdate("SqlCurr", "Saving");
            }
        }
        private void mnUpdate_Click(object sender, EventArgs e)
        {
            UpdateDB();
            fmLogin.SavingUpdate("SqlCurr", "Saving");
        }        
        private void Clear()
        {
            cbCurrencySymbol.SelectedIndex = 0; txCurrencyAmount.Text = "0.00";
            txCurrencyRate.Text = "0.00"; txCurrencyAUD.Text = "0.00";
        }
        float AUDz = 0.0f;
        private void Calculate()
        {
            float Curr = 0.0f;
            for (int j = 0; j <= dgvForeignCurr.Rows.Count - 1; j++)
            {
                Curr = Curr + float.Parse(dgvForeignCurr.Rows[j].Cells[3].FormattedValue.ToString());
                AUDz = AUDz + float.Parse(dgvForeignCurr.Rows[j].Cells[5].FormattedValue.ToString());
            }
            txCurrTotal.Text = Curr.ToString("F2");
            txTotalAUD.Text = AUDz.ToString("F2");
        }
        private void ForeignCurrency_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(txTotalAUD.Text);
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            SendEmailFtn();
        }   
        // .'........... End
    }
}
