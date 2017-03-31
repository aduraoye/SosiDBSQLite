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
    public partial class FmAccounts : Form
    {
        FmLogin fmLogin;
        FmRecords fmRecords;
        PrintPreviewXDialog PreviewXDialog;
        OpenFileDialog LoadFiles;
        AutoCompleteStringCollection source; 
        DataTable tbOnline; DateTime dtOnline;
        string Table = "AccountInfo", OtherTable = "AccountInfo", OtherId, strIdSel, strId0, Filename, FileExt = "";
        string EmailTo = "", FName = "", LName = "", EmailTox = "", FNamex = "", LNamex = "", Title = "", Titlex = "", Amount, Date_Time, Descript, Descripx;
        string OnlineDate, OnlineAmount, OnlineOfferText, OfferingDateTime;
        string[] splitCol;  char[] sep = new char[] { ';' };
        bool IsSel = false, OnlineLoad = false, IsNewAcct = false, DoUpdate = false; 

        public FmAccounts()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmRecords = new FmRecords();
            PreviewXDialog = new PrintPreviewXDialog();
            fmLogin.niBackground.Visible = false;  
        }
        private void FmAccounts_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Accounts] -" + fmLogin.strChurchAcronym;
            dgvEdit.SendToBack(); cbFileNames.SendToBack();
            mnUpdate.Enabled = false;// dtpAccountTo.Enabled = false;
            dtpAccountFrom.Value = dtpAccountTo.Value.AddMonths(-1);
            cbCustodianDetails.SendToBack();
            cbHandOverTo.SendToBack(); cbHandOverTo.Text = "Behind"; cbHandOverToDetails.SendToBack();
            tscbFileNames.Items.Clear(); cbFileNames.Items.Clear();
            tscbAccounts.SelectedIndex = 0;    cbHours.SelectedIndex = 12;

            this.MinimumSize = new System.Drawing.Size(551, 480);
            this.Size = new System.Drawing.Size(551, 480);
            gbOnline.Visible = false;
            LoadCustodian();
            BankStartAmt();
            fmLogin.SavingUpdate("SqlAcct", "Loading");//Backup
        } 
        private void InsertDB()
        { //Insert
            if (cbTransaction.Text == "Deposit") OtherId = (MaxId() + 2).ToString();
            else OtherId = (MaxId() + 1).ToString();

            if (cbTransaction.Text == "Reconcil" || cbTransaction.Text == "Debit") Amount = "-" + txAmount.Text;
            else Amount = txAmount.Text;

            if (cbTransaction.Text == "Deposit") txDescript.Text = tscbAccounts.Text + " account: " + txDescript.Text;
            Date_Time = dtpAccountTo.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO " + Table + " (Date_Time, Custodian, CustDetails, Account, Transact, Amount, GSTAmt, GSTtype, Descript, OtherTable, OtherId, FileName, FileExt) VALUES ('"
                         + Date_Time + "', '" + cbCustodian.Text + "', '" + cbCustodianDetails.Text + "', '" + tscbAccounts.Text + "', '" + cbTransaction.Text + "', '" + Amount + "', '" + txGST.Text + "', '"
                         + chGST.Checked.ToString() + "', '" + txDescript.Text + "', '" + OtherTable + "', '" + OtherId + "', '" + Filename + "', '" + FileExt + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }

            if (tscbAccounts.SelectedIndex != 0) SendEmail();
                
            if (cbTransaction.Text == "Deposit") InsertToBank(Date_Time, true);
            if (cbTransaction.Text == "Reconcil")
            {
                DialogResult DgResult = MessageBox.Show("Would you like to insert this transaction into bank account?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DgResult == DialogResult.Yes) InsertToBank(Date_Time, false);
            }
            cbTransaction.SelectedIndex = 0; cbTransaction.Enabled = true;
            txDescript.Enabled = true;   tscbAccounts.Enabled = true; tsbFiles.Enabled = true;
        }
        private void InsertToBank(string Date_Time, bool IsDebit)
        {
            OtherId = MaxId().ToString();
            string DebitCredit = "";
            if (IsDebit == true)
            {
                DebitCredit = "Debit";
                Amount = "-" + txAmount.Text;
            }
            else
            {
                DebitCredit = "Credit"; 
                Amount = txAmount.Text;
            }
            string query = "INSERT INTO " + Table + " (Date_Time, Custodian, Account, Transact, Amount, GSTAmt, GSTtype, Descript, OtherTable, OtherId, FileName, FileExt) VALUES ('"
                         + Date_Time + "', '" + cbCustodian.Text + "', 'CBA_Bank', '" + DebitCredit + "', '" + Amount + "', '" + txGST.Text + "', '" + chGST.Checked.ToString() + "', '"
                         + txDescript.Text + "', '" + tscbAccounts.Text + "', '" + OtherId + "', '" + Filename + "', '" + FileExt + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }  
        private void SelectDB()
        { // Select
            if (IsSel == true)
            {
                DataTable dTable;
                string ODTFrom = dtpAccountFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                string ODTTo = dtpAccountTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                string queryx = "";
                if(cbTransaction.SelectedIndex > 1) queryx = "SELECT Id, Date_Time As Date, Amount As Amount_AU$, Transact As Balance, Descript As Description, FileName, FileExt FROM "
                              + Table + " WHERE Account = '" + tscbAccounts.Text + "' AND ( Transact = 'Take Over' OR Transact = 'Hand Over') ORDER BY Date_Time DESC, Id DESC";
                else queryx = "SELECT Id, Date_Time As Date, Amount As Amount_AU$, Transact As Balance, Descript As Description, FileName, FileExt FROM "
                               + Table + " WHERE Account = '" + tscbAccounts.Text + "' AND ( Transact != 'Take Over' AND Transact != 'Hand Over') ORDER BY Date_Time DESC, Id DESC";
               
                using (SQLiteConnection conTble = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdImprest = new SQLiteCommand(queryx, conTble);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    dTable = new DataTable();
                    MyAdapter.SelectCommand = cmdImprest;
                    MyAdapter.Fill(dTable);
                }
                // For calaculating the balance column
                dTable.Rows[dTable.Rows.Count - 1][3] = dTable.Rows[dTable.Rows.Count - 1][2]; // The amount becomes the the 1st balance
                for (int i = dTable.Rows.Count - 2; i >= 0; i--)
                {
                    dTable.Rows[i][3] = (float.Parse(dTable.Rows[i + 1][3].ToString()) + float.Parse(dTable.Rows[i][2].ToString())).ToString("F2");
                }
                // To the display only the selected date range
                for (int i = dTable.Rows.Count - 2; i >= 0; i--)//(int i = 0; i < 7; i++)
                {
                    if(DateTime.Parse(dTable.Rows[i][1].ToString()) < DateTime.Parse(ODTFrom) || DateTime.Parse(dTable.Rows[i][1].ToString()) > DateTime.Parse(ODTTo))
                    {
                        dTable.Rows.RemoveAt(i);
                    }
                    dTable.Rows[i][1] = DateTime.Parse(dTable.Rows[i][1].ToString()).ToString("dd/MM/yyyy");
                }
                if (DateTime.Parse(dTable.Rows[dTable.Rows.Count - 1][1].ToString()) < DateTime.Parse(ODTFrom)) dTable.Rows.RemoveAt(dTable.Rows.Count - 1);

                dgvAccounts.DataSource = dTable;
                dgvAccounts.Columns[0].Visible = false;
                dgvAccounts.Columns[5].Visible = false;
                dgvAccounts.Columns[6].Visible = false;
                dgvAccounts.Columns[1].Width = 100;
                dgvAccounts.Columns[2].Width = 80;  dgvAccounts.Columns[2].DefaultCellStyle.Format = "#,###,##0.00";
                dgvAccounts.Columns[3].Width = 100; 
                dgvAccounts.Columns[4].Width = 400;
            }
            else
            {
                DataTable dTable = new DataTable();
                dgvAccounts.DataSource = dTable;
            }

        }       
        private void UpdateDB()
        {//UPDATE  
            if (cbTransaction.Text == "Reconcile" || cbTransaction.Text == "Debit") Amount = "-" + txAmount.Text;
            else Amount = txAmount.Text;
            string Date_Time = dtpAccountTo.Value.ToString("yyyy-MM-dd HH:mm:ss"); 
            string query = "UPDATE " + Table + " SET Date_Time = '" + Date_Time + "', Amount = '" + Amount
                   + "', Descript ='" + txDescript.Text + "', FileName = '" + Filename + "', FileExt = '" + FileExt
                   + "' WHERE Id = '" + strId0 + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            if (cbTransaction.Text == "Deposit") UpdateDepositToBank(Date_Time);
        }
        private void UpdateDepositToBank(string Date_Time)
        {//UPDATE  
            Amount = "-" + txAmount.Text;
            string query = "UPDATE " + Table + " SET Date_Time = '" + Date_Time + "', Amount = '" + Amount
                   + "', Descript ='" + txDescript.Text + "', FileName = '" + Filename + "', FileExt = '" + FileExt
                   + "' WHERE Id = '" + OtherId + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }

        private void mnAdd_Click(object sender, EventArgs e)
        {
            if (cbCustodian.Text == "Custodian") 
            {
                MessageBox.Show("You have not selected a custodian for the account.");
                cbCustodian.Focus();    return; 
            }

            if (cbTransaction.SelectedIndex > 1 && cbHandOverTo.Text == "Custodian")
            {
                MessageBox.Show("You have not selected a custodian to hand over the account to.");
                cbHandOverTo.Focus(); return;
            }

            DialogResult DgResult = MessageBox.Show("Are the details provided correct?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DgResult == DialogResult.Yes)
            {
                if (cbTransaction.SelectedIndex > 1 && IsNewAcct == false)
                {
                    string[] splitDesx = cbHandOverToDetails.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    FNamex = splitDesx[0];//First name
                    LNamex = splitDesx[1];// Last Name
                    EmailTox = splitDesx[2];//Email
                    Titlex = splitDesx[3];//Title

                    Filename = "0"; FileExt = fmLogin.NoneNull;
                    txDescript.Text = "Handling Over the " + tscbAccounts.Text + " account from " + cbCustodian.Text + " to " + cbHandOverTo.Text;
                    cbCustodian.Text = cbHandOverTo.Text;
                    InsertDB();
                }
                else
                {
                    if (IsNewAcct == true)
                    {
                        Filename = "0"; FileExt = fmLogin.NoneNull;
                        txDescript.Text = "CBA CheqNo:002xx - Opening the " + tscbAccounts.Text + " account with " + cbCustodian.Text + " as the custodian";
                        InsertDB();
                        cbTransaction.SelectedIndex = 1;
                        IsNewAcct = false;
                    }

                    if (tscbFileNames.Items.Count > 0)
                    {
                        Filename = tscbAccounts.Text + "-" + (MaxId() + 1).ToString();
                        FileExt = "";
                        for (int i = 0; i < tscbFileNames.Items.Count; i++)
                        {
                            FileExt = FileExt + ";" + Path.GetExtension(cbFileNames.Items[i].ToString());
                            File.Copy(cbFileNames.Items[i].ToString(), fmLogin.PathReceipt + Filename + "-" + (i + 1).ToString() + Path.GetExtension(cbFileNames.Items[i].ToString()));
                        }
                    }
                    else
                    {
                        Filename = "0";
                        FileExt = fmLogin.NoneNull;
                    }
                    InsertDB();
                } 
                cbTransaction.SelectedIndex = 1; cbTransaction.SelectedIndex = 0;
                IsSel = true;           SelectDB();
                txAmount.Text = "0.00"; txGST.Text = "0.00";
                tscbFileNames.Items.Clear(); cbFileNames.Items.Clear();
                tscbAccounts.Enabled = true; tsbFiles.Enabled = true; cbTransaction.Enabled = true;
                fmLogin.SavingUpdate("SqlAcct", "Saving");
            }
            else { return; }
            dtpAccountTo.Value = DateTime.Now;
            if (OnlineLoad == true) { gbOnline.Enabled = true; dtpAccountTo.Enabled = true; mnEdit.Enabled = true; tscbFileNames.Enabled = true; }
            tscbFileNames.Items.Clear(); cbFileNames.Items.Clear(); tscbFileNames.Text = "Receipt Files";
        }        
        private void mnEdit_ButtonClick(object sender, EventArgs e)
        {
            if (dgvAccounts.CurrentRow == null) return;
            else
            {
                mnAdd.Enabled = false; mnUpdate.Enabled = true; //mnNew.Enabled = true;
                tscbFileNames.Items.Clear(); cbFileNames.Items.Clear();
                tscbFileNames.Text = ""; cbFileNames.Text = ""; 
                int SelIndex = dgvAccounts.CurrentRow.Index;
                strId0 = dgvAccounts.Rows[SelIndex].Cells[0].FormattedValue.ToString();
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
                dtpAccountTo.Value = DateTime.Parse(dgvEdit.Rows[0].Cells[1].FormattedValue.ToString());
                cbCustodian.Text = dgvEdit.Rows[0].Cells[2].FormattedValue.ToString();
                cbCustodianDetails.Text = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString();
                tscbAccounts.Text  = dgvEdit.Rows[0].Cells[4].FormattedValue.ToString();
                cbTransaction.Text  = dgvEdit.Rows[0].Cells[5].FormattedValue.ToString();
                txAmount.Text  = Math.Abs(float.Parse(dgvEdit.Rows[0].Cells[6].FormattedValue.ToString())).ToString();
                txGST.Text  = dgvEdit.Rows[0].Cells[7].FormattedValue.ToString();
                chGST.Checked = bool.Parse(dgvEdit.Rows[0].Cells[8].FormattedValue.ToString());
                txDescript.Text = dgvEdit.Rows[0].Cells[9].FormattedValue.ToString();
                OtherTable = dgvEdit.Rows[0].Cells[10].FormattedValue.ToString();
                OtherId = dgvEdit.Rows[0].Cells[11].FormattedValue.ToString();
                Filename = dgvEdit.Rows[0].Cells[12].FormattedValue.ToString();
                FileExt = dgvEdit.Rows[0].Cells[13].FormattedValue.ToString();

                if (FileExt != fmLogin.NoneNull && FileExt != "0")
                {
                    char[] sep = new char[] { ';' };
                    string[] splitDesc = FileExt.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (splitDesc.Length > 0)
                    {
                        for (int i = 0; i <= splitDesc.Length - 1; i++)
                        {
                            if (File.Exists(fmLogin.PathReceipt + Filename + "-" + (i + 1).ToString() + splitDesc[i]))
                            {
                                tscbFileNames.Items.Add(Filename + "-" + (i + 1).ToString() + "~ts");
                                cbFileNames.Items.Add(fmLogin.PathReceipt + Filename + "-" + (i + 1).ToString() + splitDesc[i]);
                            }
                        }
                        tscbFileNames.SelectedIndex = 0;
                    }
                }
                else { tscbFileNames.Items.Clear(); cbFileNames.Items.Clear(); }			
                dtpAccountTo.Enabled = false;
            }
        }
        private void mnNew_Click(object sender, EventArgs e)
        {
            mnAdd.Enabled = true;
            dtpAccountTo.Value = DateTime.Now;
        }
        private void mnEditDate_Click(object sender, EventArgs e)
        {
            dtpAccountTo.Enabled = true;
        }
        private void mnUpdate_Click(object sender, EventArgs e)
        {//*
            if (tscbAccounts.Text == "CBA_Bank" && cbCustodian.Text != "Bank")
            {
                string Expla;
                if (OtherTable == "BankLodgeInfo" || OtherTable == "ForeignCurrencyInfo") Expla = cbCustodian.Text + " form.";
                else Expla = OtherTable + " account.";
                MessageBox.Show("You cannot edit this data from this account go to " + Expla);
                return;
            }
            else
            {
                DialogResult mgUpdate = MessageBox.Show("Do you want to continue?", "Data update", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes)
                {
                    //Deleting files met for deletion
                    if (FileExt != fmLogin.NoneNull && FileExt != "0")
                    {
                        int psn = 0; char[] sep = new char[] { '-' }; string[] splitFile; string strpsn = "";
                        FileExt = "";
                        foreach (var strFile in tscbFileNames.Items)
                        {
                            splitFile = strFile.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            if (splitFile[0] == "**Del") strpsn = strpsn + psn.ToString() + ";";
                            else FileExt = FileExt + ";" + Path.GetExtension(cbFileNames.Items[psn].ToString());
                            psn++;
                        }

                        char[] sepn = new char[] { ';' }; string[] splitpsn;
                        splitpsn = strpsn.Split(sepn, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var intpsn in splitpsn)
                        {
                            tscbFileNames.Items.RemoveAt(int.Parse(intpsn));
                            File.Delete(cbFileNames.Items[int.Parse(intpsn)].ToString());
                            cbFileNames.Items.RemoveAt(int.Parse(intpsn)); //psn--;
                        }
                    }
                    Filename = tscbAccounts.Text + "-" + strId0;
                    UpdateDB();
                   // txdBalance.Text = Balance().ToString();
                    if (FileExt != fmLogin.NoneNull && FileExt != "0")
                    {
                        if (tscbFileNames.Items.Count > 0)
                        {
                            string strPathi, strPathj; int j = 0;
                            char[] sepx = new char[] { ';' };
                            string[] splitDesc = FileExt.Split(sepx, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < tscbFileNames.Items.Count; i++)
                            {
                                if (!tscbFileNames.Items[i].ToString().Contains("~ts"))
                                {
                                    strPathi = fmLogin.PathReceipt + Filename + "-" + (i + 1).ToString() + splitDesc[i];
                                    strPathj = fmLogin.PathReceipt + Filename + "-" + (j + 1).ToString() + splitDesc[i];
                                    if (!File.Exists(strPathi)) File.Copy(cbFileNames.Items[i].ToString(), strPathi, true); 
                                    if (!File.Exists(strPathj)) File.Copy(cbFileNames.Items[i].ToString(), strPathj, true); 
                                    j++;
                                }
                            }
                        }
                    }

                    mnAdd.Enabled = true; mnUpdate.Enabled = false; dtpAccountTo.Enabled = false;
                    SelectDB();
                    fmLogin.SavingUpdate("SqlAcct", "Saving");
                }
                else return;
            }
            tscbFileNames.Items.Clear(); cbFileNames.Items.Clear(); tscbFileNames.Text = "Receipt Files";
        }
        private void mnReorder_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.CurrentRow == null) return;
            else
            {
                int ReorderIndex = dgvAccounts.CurrentRow.Index;
                string ReorderId = dgvAccounts.Rows[ReorderIndex].Cells[0].FormattedValue.ToString();
                DateTime ReorderDate;
                string querySelect = "SELECT Date_Time FROM " + Table + " WHERE Id = '" + ReorderId + "';";
                using (SQLiteConnection conSelect = new SQLiteConnection(fmLogin.strConReg))
                {
                    conSelect.Open();
                    using (var cmdSelect = new SQLiteCommand(querySelect, conSelect)) ReorderDate = Convert.ToDateTime(cmdSelect.ExecuteScalar());
                }
                string strDate;
                if (dtpAccountTo.Enabled == true) strDate = dtpAccountTo.Value.ToString("yyyy-MM-dd ") + cbHours.Text + ":22:22";
                else strDate = ReorderDate.ToString("yyyy-MM-dd ") + cbHours.Text + ":22:22";

                string queryInsert = "UPDATE " + Table + " SET Date_Time = '" + strDate + "' WHERE Id = '" + ReorderId + "'";
                using (SQLiteConnection conInsert = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdInsert = new SQLiteCommand(queryInsert, conInsert);
                    conInsert.Open();
                    cmdInsert.ExecuteNonQuery();
                }
                SelectDB();
            }

        } 
        private void mnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvAccounts.CurrentRow.Index;
                string strId = dgvAccounts.Rows[SelIndex].Cells[0].FormattedValue.ToString();

                DialogResult mgUpdate = MessageBox.Show("Do you want to delete the selected row data?", "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes)
                {
                    string strFilename = "SELECT FileName, FileExt FROM " + Table + " WHERE Id = '" + strId + "';";
                    using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdOfferingx = new SQLiteCommand(strFilename, conTblx);
                        SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                        DataTable dTable = new DataTable();
                        MyAdapter.SelectCommand = cmdOfferingx;
                        MyAdapter.Fill(dTable);
                        dgvEdit.DataSource = dTable;
                    }
                    Filename = dgvEdit.Rows[0].Cells[0].FormattedValue.ToString();
                    /*FileExt = dgvEdit.Rows[0].Cells[1].FormattedValue.ToString();

                    char[] sep = new char[] { ';' };
                    string[] splitDesc = FileExt.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (splitDesc.Length > 0) for (int i = 0; i <= splitDesc.Length - 1; i++) File.Delete(fmLogin.PathReceipt + Filename + "-" + (i+1).ToString() + splitDesc[i]);
                    */
                    if(File.Exists(fmLogin.PathReceipt + Filename + "*")) File.Delete(fmLogin.PathReceipt + Filename + "*");
                    string queryx = "DELETE FROM " + Table + " WHERE Id = '" + strId + "';";
                    using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                        conTblx.Open();
                        cmdOfferingx.ExecuteNonQuery();
                    }
                    dgvAccounts.Rows.RemoveAt(SelIndex);
                    dgvAccounts.Refresh();

                   // txdBalance.Text = Balance().ToString();
                    string queBank = "DELETE FROM " + Table + " WHERE Id = '" + (int.Parse(strId) + 1).ToString() + "';";
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
        private double Balance()
        {
            string ODTFrom = dtpAccountFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";// ("Reconcil"); cbTransaction.Items.Add("Deposit");  
            string ODTTo = dtpAccountTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
            string strDisburse = "SELECT SUM(Amount) FROM " + Table + " WHERE Account = '" + tscbAccounts.Text + "' AND ( Transact = 'Reconcil' OR Transact = 'Deposit')";
            double dBalance;
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strDisburse, conPix))
                {
                    if (!Convert.IsDBNull(cmd.ExecuteScalar())) dBalance = Convert.ToDouble(cmd.ExecuteScalar());
                   else dBalance = 0.00;
                }
            }
            return dBalance;
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
       
        private void mnLoadFiles_Click(object sender, EventArgs e)
        {
            LoadFiles = new OpenFileDialog();
            LoadFiles.Filter = "Receipt Files|*.pdf;*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*";
            LoadFiles.Title = "Load Receipts";
            LoadFiles.Multiselect = true;
            if (LoadFiles.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i <= LoadFiles.FileNames.Count() - 1; i++)
                {
                    if (i < 7)
                    {
                        tscbFileNames.Items.Add(Path.GetFileNameWithoutExtension(LoadFiles.FileNames[i]));
                        cbFileNames.Items.Add((LoadFiles.FileNames[i]));
                    }
                    else { MessageBox.Show("You cann only load 7 files at once."); break; }
                }
                tscbFileNames.SelectedIndex = 0;
            }
            else return; 
        }
        private void mnOpenFiles_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvAccounts.CurrentRow.Index;
                string Ind0 = dgvAccounts.Rows[SelIndex].Cells[0].FormattedValue.ToString();


                LoadFiles = new OpenFileDialog();
                LoadFiles.Filter = "Receipt Files|" + tscbAccounts.Text + "-" + Ind0 + "*.*";
                LoadFiles.Title = "Open Receipts";
                LoadFiles.Multiselect = true;
                LoadFiles.InitialDirectory = fmLogin.PathReceipt;
                ProcessStartInfo pi = new ProcessStartInfo();
                if (LoadFiles.ShowDialog() == DialogResult.OK)
                {
                    if (LoadFiles.FileNames.Length > 0)
                    {
                        for (int i = 0; i <= LoadFiles.FileNames.Length - 1; i++)
                        {
                            pi.FileName = LoadFiles.FileNames[i];
                            Process.Start(pi);
                        }
                    }
                }
                else return;
            }
        }
        private void mnDeleteFile_Click(object sender, EventArgs e)
        {
            if (tscbFileNames.Items.Count > 0)
            {
                int psn = tscbFileNames.SelectedIndex; string strpsn = tscbFileNames.Text;
                if (mnAdd.Enabled == true)
                {
                    tscbFileNames.Items.RemoveAt(psn); tscbFileNames.SelectedIndex = 0;
                    cbFileNames.Items.RemoveAt(psn); cbFileNames.SelectedIndex = 0;
                }
                else
                {
                    tscbFileNames.Items.RemoveAt(psn); tscbFileNames.Items.Insert(psn, "**Del-" + strpsn);
                    tscbFileNames.SelectedIndex = 0;
                }
            }
        }

        private void BankStartAmt()
        {
            DataTable dTable = new DataTable();
            string queryx = "SELECT Custodian FROM " + Table + " WHERE Account = '" + tscbAccounts.Text + "' ORDER BY Date_Time DESC LIMIT 1";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                if (dTable.Rows.Count == 0)//Account yet to be taken over or created
                {
                    IsSel = false;
                    cbTransaction.SelectedIndex = 1; cbTransaction.Enabled = false;
                    txDescript.Text = tscbAccounts.Text + " account: starting amount."; txDescript.Enabled = false;
                    tscbAccounts.Enabled = false; tsbFiles.Enabled = false;
                }
                else IsSel = true;
            }
        }
        private void HaveCustodian()
        {
            DataTable dTable = new DataTable();
            string queryx = "SELECT Custodian, Transact, CustDetails FROM " + Table + " WHERE Account = '" + tscbAccounts.Text
                          + "' AND ( Transact = 'Take Over' OR Transact = 'Hand Over') ORDER BY Id DESC, Date_Time DESC LIMIT 1";            
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                MyAdapter.SelectCommand = cmdRegister; 
                MyAdapter.Fill(dTable);
                if (dTable.Rows.Count != 0)//Account has been taken over once
                {
                    IsSel = true; IsNewAcct = false;
                    //dTable.Rows[0][1].ToString() == "Take Over" //Account has a custodian
                    cbCustodian.Text = dTable.Rows[0][0].ToString(); cbCustodian.Enabled = false;
                    cbCustodianDetails.Text = dTable.Rows[0][2].ToString();
                    cbTransaction.SelectedIndex = 0;
                    cbTransaction.Enabled = true;
                }
                else//Account yet to be taken over or created
                {
                    IsSel = false; IsNewAcct = true;
                    LoadCustodian();
                    cbCustodian.Text = "Custodian"; cbCustodian.Enabled = true;
                    cbTransaction.SelectedIndex = 2; cbTransaction.Enabled = false;
                    chGST.Checked = false; chGST.Enabled = false; txAmount.Enabled = true;
                    txDescript.Text = "CBA CheqNo:002xx - Opening the " + tscbAccounts.Text + " account with " + cbCustodian.Text + " as the custodian";                        
                }
            }
        }        
        private void LoadCustodian()
        {
            if (tscbAccounts.SelectedIndex != 0)
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
                    cbCustodian.Items.Add(splitCol[0]);     cbCustodianDetails.Items.Add(splitCol[1]);
                }
                cbCustodian.Items.Add("All"); cbCustodianDetails.Items.Add("All;All;All;All");
            }
            else {cbCustodian.Text = "Bank"; cbCustodian.Enabled = false;}
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
        private void SendEmail()// email for custodian
        {
            string[] splitDesc = cbCustodianDetails.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            FName = splitDesc[0];//First name
            LName = splitDesc[1];// Last Name
            EmailTo = splitDesc[2];//Email
            Title = splitDesc[3];//Title
            
            DialogResult DialogMsg = MessageBox.Show("Would you like to send an email message to the account custodian?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogMsg == DialogResult.No) { return; }
            else
            {
                if (cbTransaction.Text == "Take Over" || cbTransaction.Text == "Hand Over")
                {
                    if (IsNewAcct == false)
                    {
                        string EmailSubject = fmLogin.strChurchAcronym + " Church: Confirmation for handing over the custody of church " + tscbAccounts.Text + " account.";
                        string EmailBody = "Beloved " + Title + " " + FName + " " + LName + ","
                               + "\n\n"
                               + "We appreciate your contributions to the kingdom of God in your capacity as the custodian of " + tscbAccounts.Text + " account for sometime. "
                               + "\n\n We confirm the handing over and the return of the balance (AUD$" + Balance().ToString() + ") of the account in your possession to " + FNamex + " " + LNamex + " today (" + DateTime.Now.ToLongDateString() + ")."
                               + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                        fmLogin.SendEmailSt("4", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull);
                    }
                   
                    string EmailSubjectx = fmLogin.strChurchAcronym + " Church: Confirmation for taking over the custody of church " + tscbAccounts.Text + " account.";
                    string EmailBodyx = "Beloved " + Titlex + " " + FNamex + " " + LNamex + ","
                           + "\n\n"
                           + "We appreciate your acceptance to be the custodian of " + tscbAccounts.Text + " account. "
                           + "\n\n The balance of this account as of this time (" + dtpAccountTo.Value.ToLongDateString() + ") of your acceptance is AUD$" + Balance().ToString() + "."
                           + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                    fmLogin.SendEmailSt("4", EmailTox, "", EmailSubjectx, EmailBodyx, "", null, fmLogin.NoneNull);
                }
                else
                {
                    string EndTx; if (cbTransaction.Text == "Reconcil") EndTx = "iation"; else EndTx = "";
                    string EmailSubject = fmLogin.strChurchAcronym + " Church: " + tscbAccounts.Text + " Account " + cbTransaction.Text + EndTx + " Confirmation";
                    string EmailBody = "Beloved " + Title + " " + FName + " " + LName + ","
                           + "\n\nThe following amount was " + cbTransaction.Text.ToLowerInvariant() + "ed into the " + tscbAccounts.Text.ToLowerInvariant() + " account "
                           + "under your care as follows: \nDate: " + dtpAccountTo.Value.ToLongDateString() + "\nAmount: AUD$" + txAmount.Text
                           + "\n\nPlease send a reply email to confirm this statement. Thank you Sir/Ma."
                           + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' Name, Amen.";
                    fmLogin.SendEmailSt("4", EmailTo, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull);
                }
            }
        }
        private void cbTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTransaction.SelectedIndex > 1)
            {
                txAmount.Text = Balance().ToString(); txAmount.Enabled = false;
                tscbFileNames.Items.Clear(); tscbFileNames.Enabled = false;
                tsbFiles.Enabled = false; chGST.Checked = false;
                cbHandOverTo.Visible = true; cbHandOverTo.BringToFront();
                cbHandOverTo.Text = "Custodian"; LoadCustodianHandOver();
                if(IsSel==true) dgvAccounts.Columns[3].Visible = false;
            }
            else
            {
                if (OnlineLoad == false)
                {
                    //txAmount.Text = "0.00"; txAmount.Enabled = true; txGST.Text = "0.00";
                   // tscbFileNames.Items.Clear(); tscbFileNames.Enabled = true;
                   // tsbFiles.Enabled = true; txDescript.Enabled = true;
                }

                if (tscbAccounts.Text == "CBA_Bank")
                {
                    if (cbTransaction.SelectedIndex == 1) { chGST.Checked = false; txDescript.Text = ""; chGST.Enabled = false; }
                    else { chGST.Checked = true; txDescript.Text = "CheqNo:002xx -"; chGST.Enabled = true; } //  == 0 Debit
                }
                else
                {
                    if (cbTransaction.SelectedIndex == 0) { chGST.Checked = true; txDescript.Text = ""; chGST.Enabled = true; }
                    else // == 1 Deposit
                    {
                        chGST.Checked = false; chGST.Enabled = false;
                        if (IsSel == true) txDescript.Text = "CheqNo:002xx -";
                        else txDescript.Text = "(CBA CheqNo:002xx) Deposited starting amount.";
                    }
                }
                cbHandOverTo.Visible = false; txAmount.Enabled = true;
                if (IsSel == true) dgvAccounts.Columns[3].Visible = true;
                if(IsNewAcct==false) txAmount.Text = "0.00";
            }
            SelectDB();
        }
        private void tscbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscbAccounts.SelectedIndex == 0)
            {
                cbTransaction.Items.Clear();
                cbTransaction.Items.Add("Debit");
                cbTransaction.Items.Add("Credit");
                cbCustodian.Text = "Bank"; cbCustodian.Enabled = false;
                cbTransaction.SelectedIndex = 0; cbTransaction.Enabled = true;
                BankStartAmt();
            }
            else
            {
                cbTransaction.Items.Clear();
                cbTransaction.Items.Add("Reconcil");
                cbTransaction.Items.Add("Deposit");     //cbTransaction.Items.Add("Take Over");
                cbTransaction.Items.Add("Hand Over");
                HaveCustodian();
            }
            SelectDB();
            cbHandOverTo.Visible = false; 
        }               
        private void chGST_CheckedChanged(object sender, EventArgs e)
        {
            if (chGST.Checked == true) txGST.Enabled = true;
            else { txGST.Enabled = false; txGST.Text = "0.00"; }
        }       
        private void mnPrintData_Click(object sender, EventArgs e)
        {
            string DocName = tscbAccounts.Text + " Account Statement-" + DateTime.Now.ToString("ddMMyy"); 
            string AdHeader = fmLogin.strChurchAcronym + "-" + DocName;
            PreviewXDialog.DGVPdfPrint(dgvAccounts, AdHeader, DocName, false);
            PreviewXDialog.PrintDGVX(dgvAccounts, AdHeader, DocName, false);
        }
        private void cbCustodian_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCustodianDetails.SelectedIndex = cbCustodian.SelectedIndex;
            cbHandOverTo.Text = cbCustodian.Text;
            SelectDB();
        }
        private void cbHandOverTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbHandOverToDetails.SelectedIndex = cbHandOverTo.SelectedIndex;
        }

        #region // ************ Load Banklodge *******************************
        private void mnOnline_Click(object sender, EventArgs e)
        {
            dtOnline = dtpAccountTo.Value;// DateTime.Parse("24/07/2016"); // delete
            cbPreLoad.SelectedIndex = 0;
            if (dtOnline.DayOfWeek == DayOfWeek.Sunday)
            {
                if (DateTime.Now.Date > dtOnline.Date) { } // .AddDays(2) delete
                else
                {
                    MessageBox.Show("This process must be done after the selected day.", "Selected Day", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("The day of the week must be Sunday.", "Day of the Week", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenFileDialog PixOpen = new OpenFileDialog();
            PixOpen.Filter = "Report Files|*.csv;*.xlsx";
            PixOpen.Title = "Load Other Statistical Report File";
            if (PixOpen.ShowDialog() == DialogResult.OK)
            {
                this.MinimumSize = new System.Drawing.Size(997, 480);
                this.Size = new System.Drawing.Size(997, 480);
                gbOnline.Visible = true;
                cbOnlineAcct.SelectedIndex = 0;

                ExcelReport excelReport = new ExcelReport();
                tbOnline = new DataTable();
                tbOnline = excelReport.GetTableFromExcel(PixOpen.FileName, dtOnline.AddDays(-6), dtOnline, true);

                dgvOnlineHeader.Columns.Add("Date", "Date"); dgvOnlineHeader.Columns.Add("Amount", "Amount");
                dgvOnlineHeader.Columns.Add("Balance", "Balance"); dgvOnlineHeader.Columns.Add("Descriptions", "Descriptions");
                dgvOnlineHeader.Rows.Insert(0, tbOnline.Rows[tbOnline.Rows.Count - 1][0].ToString(), tbOnline.Rows[tbOnline.Rows.Count - 1][1].ToString(),
                                               tbOnline.Rows[tbOnline.Rows.Count - 1][2].ToString(), tbOnline.Rows[tbOnline.Rows.Count - 1][3].ToString());
                dgvOnlineHeader.Columns[0].Width = 85;
                dgvOnlineHeader.Columns[1].Width = 80; dgvOnlineHeader.Columns[1].DefaultCellStyle.Format = "#,###,##0.00";
                dgvOnlineHeader.Columns[2].Width = 80;
                dgvOnlineHeader.Columns[3].Width = 175;

                tbOnline.Rows.RemoveAt(tbOnline.Rows.Count - 1);
                dgvOnline.DataSource = ReverseRowsInDataTable(tbOnline);
                dgvOnline.Columns[0].Width = 85;
                dgvOnline.Columns[1].Width = 80; dgvOnline.Columns[1].DefaultCellStyle.Format = "#,###,##0.00";
                dgvOnline.Columns[2].Width = 80;
                dgvOnline.Columns[3].Width = 600;

                cbOnlineAcct.Items.Clear(); cbOnlineAcct.Items.Add("Offeings"); cbOnlineAcct.Items.Add("Deposit");
                object[] obj = new object[tscbAccounts.Items.Count]; tscbAccounts.Items.CopyTo(obj, 0);
                cbOnlineAcct.Items.AddRange(obj);
                AutoComplete();
                OnlineLoad = true;
                lbDate.Text = "From " + dtOnline.AddDays(-6).ToString("ddd - dd/MM/yy") + " To " + dtOnline.ToString("ddd - dd/MM/yy");
            }
            else return;
        }
        private DataTable ReverseRowsInDataTable(DataTable inputTable)
        {
            DataTable outputTable = inputTable.Clone();

            for (int i = inputTable.Rows.Count - 1; i >= 0; i--)
            {
                outputTable.ImportRow(inputTable.Rows[i]);
            }

            return outputTable;
        }
        private void btOnlineProcess_Click(object sender, EventArgs e) 
        {
            if (cbOnlineAcct.Text == "All Offerings") { MessageBox.Show("You have not selected any account type."); return; }

            if (dgvOnline.CurrentRow == null || dgvOnline.CurrentRow.Index != 0) { MessageBox.Show("You have to select the first row!"); return; }
            else
            {
                OnlineDate = dgvOnline.Rows[0].Cells[0].Value.ToString();
                OnlineAmount = dgvOnline.Rows[0].Cells[1].Value.ToString().Replace("+","");
                OnlineOfferText = cbOnlineOfferings.Text;
                if (cbOnlineAcct.SelectedIndex == 0)//Offerings
                {
                    if (cbNameSearch.SelectedIndex < 3)
                    {
                        if (source.Contains(txNameSearch.Text) == false || cbOnlineOfferings.Text == "Select Offerings")
                        {MessageBox.Show("You have not selected appropriate person's details or offering type."); return;}
                    }
                    else if(cbOnlineOfferings.Text == "Select Offerings"){MessageBox.Show("You have not selected offering type."); return;}
                    AddOffering();
                    
                    dgvOnlineHeader.Rows.Insert(0, dgvOnline.Rows[0].Cells[0].Value, dgvOnline.Rows[0].Cells[1].Value, dgvOnline.Rows[0].Cells[2].Value, dgvOnline.Rows[0].Cells[3].Value);
                    dgvOnline.Rows.RemoveAt(dgvOnline.CurrentRow.Index);
                    dgvOnline.Refresh(); dgvOnline.Refresh();
                    tscbAccounts.SelectedIndex = 0;
                    SelectDB();
                }
                else if (cbOnlineAcct.SelectedIndex == 1) // Deposit
                {
                    int SelIndex = 0;
                    if (dgvDeposit.CurrentRow == null) { MessageBox.Show("You have not selected any row!"); return; }
                    else SelIndex = dgvDeposit.CurrentRow.Index;

                    if (DateTime.Parse(dgvDeposit.Rows[SelIndex].Cells[0].Value.ToString()).Date != dtOnline.AddDays(-7).Date
                     || float.Parse(dgvDeposit.Rows[SelIndex].Cells[1].Value.ToString()) != float.Parse(dgvOnline.CurrentRow.Cells[1].Value.ToString()))
                    {
                        FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
                        StreamReader sr = new StreamReader(fs);
                        sr.ReadLine();
                        string RegIdx = sr.ReadLine();
                        fs.Close(); sr.Close();

                        if (RegIdx == fmLogin.RootNo)
                        {
                            DialogResult DiaMsg = MessageBox.Show("Would you like to continue online insertion?", "Deposit Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (DiaMsg == DialogResult.Yes)
                            {
                                string Descriptn = dgvDeposit.Rows[SelIndex].Cells[3].Value.ToString();
                                OnlineInsertBank(dgvOnline.CurrentRow.Cells[0].Value.ToString(), Descriptn, MaxId().ToString(), "DepositInfo");
                            }
                        }
                        else { MessageBox.Show("The date or amount of the offering deposit doesn't agree!"); return; }                        
                    }
                    else
                    {
                        string Descriptn = dgvDeposit.Rows[SelIndex].Cells[3].Value.ToString();
                        OnlineInsertBank(dgvOnline.CurrentRow.Cells[0].Value.ToString(), Descriptn, MaxId().ToString(), "DepositInfo");
                    }

                    DialogResult DialogMsg = MessageBox.Show("Would you like to send email messages to the offering recorders?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogMsg == DialogResult.Yes)
                    {
                        string strDateStart = DateTime.Parse(dgvDeposit.Rows[SelIndex].Cells[0].Value.ToString()).AddDays(-6).ToString("dd/MM/yyyy");
                        string strDateEnd = DateTime.Parse(dgvDeposit.Rows[SelIndex].Cells[0].Value.ToString()).ToString("dd/MM/yyyy");
                        string strSubject = "Acknowledgement of the lodgement of church offerings for " + strDateStart + " to " + strDateEnd;
                        string strDescript = "Dear All,"
                               + "\n"
                               + "\nI acknowledege that the " + dgvDeposit.Rows[SelIndex].Cells[2].Value.ToString() + " has deposited AUD$"
                               + dgvDeposit.Rows[SelIndex].Cells[1].Value.ToString() + " as the church offerings for "
                               + strDateStart + " to " + strDateEnd + " into the church account."
                               + "\n\n May your service be acceptable by the Almighty God. \nRemain blessed and rapturable.";
                        fmLogin.SendEmailSt("4", dgvDeposit.Rows[SelIndex].Cells[4].Value.ToString(), "", strSubject, strDescript, "", null, fmLogin.NoneNull);
                    }
                    dgvOnlineHeader.Rows.Insert(0, dgvOnline.Rows[0].Cells[0].Value, dgvOnline.Rows[0].Cells[1].Value,
                                              dgvOnline.Rows[0].Cells[2].Value, dgvOnline.Rows[0].Cells[3].Value);
                    dgvOnline.Rows.RemoveAt(dgvOnline.CurrentRow.Index);
                    dgvOnline.Refresh(); dgvOnline.Refresh();
                    tscbAccounts.SelectedIndex = 0;
                    SelectDB();
                }
                else
                {
                    tscbAccounts.SelectedIndex = cbOnlineAcct.SelectedIndex - 2; tscbAccounts.Enabled = false;
                    cbTransaction.Enabled = false;
                    if (tscbAccounts.SelectedIndex == 0)
                    {
                        if (dgvOnline.Rows[0].Cells[1].Value.ToString().Contains("-")) cbTransaction.SelectedIndex = 0;
                        else cbTransaction.SelectedIndex = 1;
 
                        if (cbPreLoad.SelectedIndex == 1) txDescript.Text = dgvOnline.CurrentRow.Cells[3].Value.ToString();
                        else if (cbPreLoad.SelectedIndex == 2) txDescript.Text = "CheqNo:00xxx - " + DateTime.Now.ToString("MMMM") + " Remittance to the Region";
                    }
                    else
                    {
                        if (dgvOnline.Rows[0].Cells[1].Value.ToString().Contains("-")) cbTransaction.SelectedIndex = 1;
                        else cbTransaction.SelectedIndex = 0;

                        if (cbPreLoad.SelectedIndex == 1) txDescript.Text = dgvOnline.CurrentRow.Cells[3].Value.ToString();
                    }

                    dtpAccountTo.Value = DateTime.Parse(dgvOnline.CurrentRow.Cells[0].Value.ToString()); dtpAccountTo.Enabled = false;
                    txAmount.Text = Math.Abs(float.Parse(dgvOnline.CurrentRow.Cells[1].Value.ToString())).ToString(); txAmount.Enabled = false;
                    mnEdit.Enabled = false; gbOnline.Enabled = false;
                    
                    dgvOnlineHeader.Rows.Insert(0, dgvOnline.Rows[0].Cells[0].Value, dgvOnline.Rows[0].Cells[1].Value, dgvOnline.Rows[0].Cells[2].Value, dgvOnline.Rows[0].Cells[3].Value);
                    dgvOnline.Rows.RemoveAt(dgvOnline.CurrentRow.Index);
                    dgvOnline.Refresh(); dgvOnline.Refresh();
                    SelectDB();
                }
                mnAdd.Enabled = true;
                DoUpdate = true;
            }
        }
        private void btOnlineClose_Click(object sender, EventArgs e)
        {
            string OfferId = OfferingId();
            for (int i = 0; i <= 8; i++)
            {
                if(cbOnlineAmt.Items[i].ToString() != "0.00")
                    UpdateBankLodgeDB(cbOnlineAmt.Items[i].ToString(), OfferingTable(i), Offeringx(i), OfferingCb(i), OfferingId());
            }
            this.MinimumSize = new System.Drawing.Size(551, 480);
            this.Size = new System.Drawing.Size(551, 480);
            gbOnline.Visible = false; gbDeposit.Visible = false;
            DoUpdate = false;
        }
        private void UpdateBankLodgeDB(string strBankLodge, string OfferingTable, string Offering, string OfferingCb, string strIdup)
        {//UPDATE
            float New_TotalAmount, Old_TotalAmount, Old_BankLodgeAmt, New_BankLodgeAmt, Old_Offering, New_Offering;

            string strCount = "SELECT BankLodge FROM " + OfferingTable + "  WHERE Id = '" + strIdup + "'";
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix)) { Old_BankLodgeAmt = (float)Convert.ToDouble(cmd.ExecuteScalar()); }
            }
            string queryBankAmt = "SELECT total(Amount) FROM BankLodgeInfo WHERE OfferingId = '" + strIdup + "' AND Offerings = '" + OfferingCb + "' AND PayMethod = 'Bank'";
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(queryBankAmt, conPix)) { New_BankLodgeAmt = (float)Convert.ToDouble(cmd.ExecuteScalar()); }
            }
            
            string query = "UPDATE " + OfferingTable + " SET BankLodge = '" + New_BankLodgeAmt.ToString() + "' WHERE Id = '" + strIdup + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }

            string queryoff = "SELECT " + Offering + " FROM OfferingInfo WHERE Id = '" + strIdup + "'";
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(queryoff, conPix)) { Old_Offering = (float)Convert.ToDouble(cmd.ExecuteScalar()); }
            }
            New_Offering = Old_Offering - Old_BankLodgeAmt + New_BankLodgeAmt;
            string querzoff = "UPDATE OfferingInfo SET " + Offering + " = '" + New_Offering.ToString() + "' WHERE Id = '" + strIdup + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(querzoff, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }

            string queryt = "SELECT TotalAmount FROM OfferingInfo WHERE Id = '" + strIdup + "'";
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(queryt, conPix)) { Old_TotalAmount = (float)Convert.ToDouble(cmd.ExecuteScalar()); }
            }
            New_TotalAmount = Old_TotalAmount - Old_BankLodgeAmt + New_BankLodgeAmt;
            string queryz = "UPDATE OfferingInfo SET TotalAmount = '" + New_TotalAmount.ToString() + "' WHERE Id = '" + strIdup + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(queryz, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private string OfferingTable(int Idx)
        {
            string OfferingTable = "";
            if (Idx == 0) OfferingTable = "NormOfferingInfo";
            if (Idx == 1) OfferingTable = "TitheInfo";
            if (Idx == 2) OfferingTable = "F1stFruitInfo";
            if (Idx == 3) OfferingTable = "ThanksgivingInfo";
            if (Idx == 4) OfferingTable = "MissionsInfo";
            if (Idx == 5) OfferingTable = "BuildingFundInfo";
            if (Idx == 6) OfferingTable = "PledgesInfo";
            if (Idx == 7) OfferingTable = "SpecialInfo";
            if (Idx == 8) OfferingTable = "OtherOfferingInfo";
            return OfferingTable;
        }
        private string Offeringx(int Idx)
        {
            string Offering = "";     
            if (Idx == 0) Offering = "NormOffering";
            if (Idx == 1) Offering = "Tithe";
            if (Idx == 2) Offering = "F1stFruit";
            if (Idx == 3) Offering = "Thanksgiving";
            if (Idx == 4) Offering = "Missions";
            if (Idx == 5) Offering = "Building";
            if (Idx == 6) Offering = "Pledges";
            if (Idx == 7) Offering = "Special";
            if (Idx == 8) Offering = "OtherOffering";
            return Offering;
        }
        private string OfferingCb(int Idx)
        {
            string Offering = "";
            if (Idx == 0) Offering = "Normal Offering";
            if (Idx == 1) Offering = "Tithes";
            if (Idx == 2) Offering = "First Fruit";
            if (Idx == 3) Offering = "Thanksgiving Offering";
            if (Idx == 4) Offering = "Missions Offering";
            if (Idx == 5) Offering = "Building Funds";
            if (Idx == 6) Offering = "Pledges";
            if (Idx == 7) Offering = "Special Offering";
            if (Idx == 8) Offering = "Other Offerings/Funds";
            return Offering;
        } 

      private void cbOnlineAcct_SelectedIndexChanged(object sender, EventArgs e)
      {
          if (cbOnlineAcct.SelectedIndex == 0) 
          {
              cbNameSearch.Enabled = true; txNameSearch.Enabled = true;
              cbOnlineOfferings.Enabled = true; cbOnlineAmt.Enabled = true;
              gbDeposit.Visible = false;
          }
          else
          {
              cbNameSearch.Enabled = false; txNameSearch.Enabled = false;
              cbOnlineOfferings.Enabled = false; cbOnlineAmt.Enabled = false;
              gbDeposit.Visible = false;
              if (cbOnlineAcct.SelectedIndex == 1) DepositShow();
          }
      }
      private void cbOnlineOfferings_SelectedIndexChanged(object sender, EventArgs e)
        {            
           cbOnlineAmt.SelectedIndex = cbOnlineOfferings.SelectedIndex;
        }        
      private void AddOffering()
      {          
          DialogResult DgResult = MessageBox.Show("Are the details provided correct and would you like to continue?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
          if (DgResult == DialogResult.Yes)
          {
              OnlineInsertBankLodge();
              cbOnlineAmt.Items[cbOnlineOfferings.SelectedIndex] = (float.Parse(cbOnlineAmt.Text) + float.Parse(OnlineAmount)).ToString("F2");
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

                  string Title1 = dTable.Rows[0][0].ToString(); //Title
                  string FName1 = dTable.Rows[0][1].ToString();//First name
                  string LName1 = dTable.Rows[0][2].ToString();// Last Name
                  string PayDate = DateTime.Parse(OnlineDate).ToString("dddd, MMMM d, yyyy"); // Middle Name
                  string EmailTo1 = dTable.Rows[0][4].ToString();//Email

                  DialogResult DialogMsg = MessageBox.Show("Would you like to send email messages to the offering payers?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                  if (DialogMsg == DialogResult.Yes)
                  {
                      string EmailSubject = fmLogin.strChurchAcronym + " Church: Payment Acknowledgment";                     
                      string EmailBody = "Dear " + Title1 + " " + FName1 + " " + LName1 + " (RegID: " + RegIdSplit() + "),"
                             + "\n\nWe acknowledge the payment of AUD$" + OnlineAmount + " on " + PayDate + " as your " + cbOnlineOfferings.Text + " into the church account."
                             //+ "\n\nPlease, accept my apologies for the wrong offering acknowledgement email sent to you earlier on. We were updating the software App that sends the emails. "
                             //+ "I have been working assiduously to rectify this mistake and I am happy to inform you that it has been corrected. We will do everything within our ability to ensure that it does repeat itself. Thank you."
                             + "\n\nMay the all sufficient God favour you and provide for all your needs in Jesus' name, Amen. Remain blessed and rapturable.";
                      fmLogin.SendEmailSt("4", EmailTo1, "", EmailSubject, EmailBody, "", null, fmLogin.NoneNull);
                  }
              } 
              string Descriptn = cbOnlineOfferings.Text + " lodgement for RegId = " + RegIdSplit();
              OnlineInsertBank(OfferingDateTime, Descriptn, MaxId().ToString(), "BankLodgeInfo"); 
          }
          else return;
          txNameSearch.Text = "";
      }
      private void OnlineInsertBankLodge()  //Insert into  BankLodgeInfo table
      {
          OtherId = (BankMaxId() + 1).ToString();
          OfferingDateTime = DateTime.Parse(OnlineDate).ToString("yyyy-MM-dd HH:mm:ss");
          string query = "INSERT INTO BankLodgeInfo (Date_Time, OfferingId, RegId, Offerings, Amount, OtherId, PayMethod) VALUES ('" + OfferingDateTime
                       + "', '" + OfferingId() + "', '" + RegIdSplit() + "', '" + OnlineOfferText + "', '" + OnlineAmount + "', '" + OtherId + "', 'Bank');";
          using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
          {
              SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
              conTbly.Open();
              cmdOffering.ExecuteNonQuery();
          }
      }
      private string OfferingId()
      {
          string ODTFrom = dtOnline.ToString("yyyy-MM-dd") + " 00:00:00"; 
          string ODTTo =   dtOnline.ToString("yyyy-MM-dd") + " 23:59:59";
          string strCount = "SELECT Id FROM OfferingInfo WHERE Service = 'Sunday Service' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "'";
          using (var conPix = new SQLiteConnection(fmLogin.strConReg))
          {
              conPix.Open();
              using (var cmd = new SQLiteCommand(strCount, conPix))
              {
                  return Convert.ToString(cmd.ExecuteScalar());
              }
          }
      } 
      private void OnlineInsertBank(string OfferingDateTimex, string Descriptx, string OtherIdx, string OtherTablex)
      {
          OfferingDateTimex = DateTime.Parse(OfferingDateTimex).ToString("yyyy-MM-dd HH:mm:ss");
          string query = "INSERT INTO AccountInfo (Date_Time, Custodian, Account, Transact, Amount, GSTAmt, GSTtype, Descript, OtherTable, OtherId, FileName, FileExt) VALUES ('" + OfferingDateTimex
                       + "', 'BankLodge', 'CBA_Bank', 'Credit', '" + OnlineAmount + "', '0.00', 'false', '" + Descriptx + "', '" + OtherTablex + "', '" + OtherIdx + "', ' ', '0');";
          using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
          {
              SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
              conTbly.Open();
              cmdOffering.ExecuteNonQuery();
          }
      }
      private string RegIdSplit()
      {
          char[] sep = new char[] { ' ' }; string[] splitColm;
          splitColm = txNameSearch.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
          if (cbNameSearch.SelectedIndex == 0) return splitColm[0];
          else if (cbNameSearch.SelectedIndex == 1 || cbNameSearch.SelectedIndex == 2) return splitColm[2];
          else return "0";// 
      }
      private int OnlineMaxId()
      {
          string strPixId = "SELECT Id FROM BankLodgeInfo ORDER BY Id DESC LIMIT 1";
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
      
      private void AutoComplete()
       {
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
        
       private void DepositShow()
       {
           gbDeposit.Visible = true;
           string ODTTo = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";          
           string ODTFrom = DateTime.Now.AddDays(-84).ToString("yyyy-MM-dd") + " 23:59:59";
           string queryDeposit = "SELECT DepositDate As Date, DepositAmount AS Amount, Depositor, Descript As Details, EmailTo, Id FROM DepositInfo WHERE DepositDate BETWEEN '"
                         + ODTFrom + "' AND '" + ODTTo + "' ORDER BY DepositDate DESC, Id DESC";
           using (SQLiteConnection conDeposit = new SQLiteConnection(fmLogin.strConReg))
           {
               SQLiteCommand cmdDeposit = new SQLiteCommand(queryDeposit, conDeposit);
               SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
               DataTable tbDeposit = new DataTable();
               MyAdapter.SelectCommand = cmdDeposit;
               MyAdapter.Fill(tbDeposit);
               dgvDeposit.DataSource = tbDeposit;
               dgvDeposit.Columns[0].Width = 85;
               dgvDeposit.Columns[1].Width = 80; dgvDeposit.Columns[1].DefaultCellStyle.Format = "#,###,##0.00";
               dgvDeposit.Columns[2].Width = 80; 
               dgvDeposit.Columns[3].Width = 240;
               dgvDeposit.Columns[4].Visible = false;
           }           
       }
       private void btEditDeposit_Click(object sender, EventArgs e)
       {
           if (dgvDeposit.CurrentRow == null) { MessageBox.Show("You have not selected any row!"); return; }
           else
           {
               int SelIndex = dgvDeposit.CurrentRow.Index;
               strIdSel = dgvDeposit.Rows[SelIndex].Cells[5].FormattedValue.ToString();

               dtpEditDeposit.Value = DateTime.Parse(dgvDeposit.Rows[SelIndex].Cells[0].FormattedValue.ToString());
               txAmountDeposit.Text = dgvDeposit.Rows[SelIndex].Cells[1].FormattedValue.ToString();
               txDepositorDeposit.Text = dgvDeposit.Rows[SelIndex].Cells[2].FormattedValue.ToString();
               txDescriptDeposit.Text = dgvDeposit.Rows[SelIndex].Cells[3].FormattedValue.ToString();
               txEmailsDeposit.Text = dgvDeposit.Rows[SelIndex].Cells[4].FormattedValue.ToString();
           }
           gbEditDeposit.Visible = true;
       }
       private void btUpdateDeposit_Click(object sender, EventArgs e)
       {
           string querydp = "UPDATE DepositInfo SET DepositDate = '" + dtpEditDeposit.Value.ToString() + "', DepositAmount = '" + txAmountDeposit.Text
                       + "', Depositor = '" + txDepositorDeposit.Text + "', Descript = '" + txDescriptDeposit.Text
                       + "', EmailTo = '" + txEmailsDeposit.Text + "' WHERE Id = '" + strIdSel + "'";
           using (SQLiteConnection conTble = new SQLiteConnection(fmLogin.strConReg))
           {
               SQLiteCommand cmdOfferingx = new SQLiteCommand(querydp, conTble);
               conTble.Open();
               cmdOfferingx.ExecuteNonQuery();
           }
           DepositShow();    MessageBox.Show("Here");
       }
       private void btDeleteDeposit_Click(object sender, EventArgs e)
       {
           string query = "DROP TABLE IF EXISTS DepositInfo";
           using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
           {
               SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
               conTbly.Open();
               cmdOffering.ExecuteNonQuery();
           }
       }
       private void btCloseDeposit_Click(object sender, EventArgs e)
       {
           gbEditDeposit.Visible = false;
       }

       private void btMoveUp_Click(object sender, EventArgs e)
       {
           dgvOnlineHeader.Rows.Insert(0, dgvOnline.Rows[0].Cells[0].Value, dgvOnline.Rows[0].Cells[1].Value, dgvOnline.Rows[0].Cells[2].Value, dgvOnline.Rows[0].Cells[3].Value);
           dgvOnline.Rows.RemoveAt(dgvOnline.CurrentRow.Index);
           dgvOnline.Refresh(); dgvOnlineHeader.Refresh();
           SelectDB();
       }
       private void btMoveDown_Click(object sender, EventArgs e)
       {
           dgvOnline.Rows.Insert(0, dgvOnlineHeader.Rows[0].Cells[0].Value, dgvOnlineHeader.Rows[0].Cells[1].Value, dgvOnlineHeader.Rows[0].Cells[2].Value, dgvOnlineHeader.Rows[0].Cells[3].Value);
           dgvOnlineHeader.Rows.RemoveAt(dgvOnlineHeader.CurrentRow.Index);
           dgvOnline.Refresh(); dgvOnlineHeader.Refresh();
           SelectDB();
       }
       #endregion  // Deposit

       private void FmAccounts_FormClosing(object sender, FormClosingEventArgs e)
       {
           if (DoUpdate == true) btOnlineClose_Click(sender, e);
       }
        //Eneddd =============================================================================================
    }
}
