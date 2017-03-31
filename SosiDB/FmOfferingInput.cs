using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite; 

namespace SosiDB
{
    public partial class FmOfferingInput : Form
    {
        FmLogin fmLogin; FmChangeLogin fmChangeLogin;   FmRecords fmRecords; 
        PrintPreviewXDialog PreviewXDialog;
        public string ReportFileName, RemittanceFileName;//, RCCGYear;
        private string Table = "OfferingInfo", UserGroup, RegId, strId0, TotalDeposit, DataRange, Service;
        private float DollCent;

        public FmOfferingInput()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmChangeLogin = new FmChangeLogin();
            fmRecords = new FmRecords();
            PreviewXDialog = new PrintPreviewXDialog();
            fmLogin.niBackground.Visible = false;
        }
        private void FmOfferingInput_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Offerings] -" + fmLogin.strChurchAcronym;
            cbService.SelectedIndex = 0;
            tscbService.SelectedIndex = 4;
            cbOffering.SelectedIndex = 0; 
            btNew_Click(sender, e);
            SelectDB();
            dtpOfferingFrom.Value = dtpOfferingTo.Value.AddMonths(-1);  //dtpOffering.Enabled = false;
            dgvEdit.SendToBack();

            gbOfferings.Enabled = false; gbAttendance.Enabled = false;
            btSaveData.Enabled = false; btClear.Enabled = false; btAddOffering.Enabled = false;
            fmLogin.SavingUpdate("SqlOffer", "Loading");
        }
        private void ReadUserGroup()
        {
            FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            UserGroup = sr.ReadLine();
            RegId = sr.ReadLine();
            fs.Close(); sr.Close();
        }       
        private void InsertDB()
        { //Insert offering details  cbOfferTotalAmt.Items[0].ToString()       
            string strOffering   = "NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering, ";
            string strAttendance = "NoMale, NoFemale, Nochild, NoTotal, No1stTimer, NoNewConvert, NoSunSch";
            string strtxOffering = cbOfferTotalAmt.Items[0].ToString() + "', '" + cbOfferTotalAmt.Items[1].ToString() + "', '"
                                 + cbOfferTotalAmt.Items[2].ToString() + "', '" + cbOfferTotalAmt.Items[3].ToString() + "', '"
                                 + cbOfferTotalAmt.Items[4].ToString() + "', '" + cbOfferTotalAmt.Items[5].ToString() + "', '"
                                 + cbOfferTotalAmt.Items[6].ToString() + "', '" + cbOfferTotalAmt.Items[7].ToString() + "', '"
                                 + cbOfferTotalAmt.Items[8].ToString(); 
            string strtxAttendance = txNoMale.Text + "', '" + txNoFemale.Text + "', '" + txNoChild.Text + "', '" + txNoTotal.Text
                                     + "', '" + txNo1stTimer.Text + "', '" + txNoNewConvert.Text + "', '" + txSundaySch.Text;            
            string OfferingDateTime = dtpOffering.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO " + Table + " (Date_Time, Service, " + strOffering + strAttendance + ", TotalDeposit, TotalAmount, Preacher) VALUES ('"
                             + OfferingDateTime + "', '" + Service + "', '" + strtxOffering + "', '" + strtxAttendance + "', '"
                             + txTotalDeposit.Text + "', '" + txTotalAmount.Text + "', '" + txPreacher.Text + "');";

            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            btSaveData.Enabled = false;
        }
        private void SelectDB()
        { // Select 
            string queryx = "";
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string ODTTo = dtpOfferingTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
            if (tscbService.SelectedIndex == 0) queryx = "SELECT Id, Date_Time, Service, TotalAmount, TotalDeposit, NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering FROM " + Table + " WHERE Service = 'Sunday Service' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            if (tscbService.SelectedIndex == 1) queryx = "SELECT Id, Date_Time, Service, TotalAmount, TotalDeposit, NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering FROM " + Table + " WHERE Service = 'Bible/Prayer Service' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            if (tscbService.SelectedIndex == 2) queryx = "SELECT Id, Date_Time, Service, TotalAmount, TotalDeposit, NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering FROM " + Table + " WHERE Service = 'Monthly Vigil' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            if (tscbService.SelectedIndex == 3) queryx = "SELECT Id, Date_Time, Service, TotalAmount, TotalDeposit, NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering FROM " + Table + " WHERE Service = 'Special Service' AND Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            if (tscbService.SelectedIndex == 4) queryx = "SELECT Id, Date_Time, Service, TotalAmount, TotalDeposit, NormOffering, Tithe, F1stfruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering FROM " + Table + " WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvOffering.DataSource = dTable;
            }
            dgvOffering.Columns[0].Visible = false;
            dgvOffering.Columns[1].Width = 90; dgvOffering.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvOffering.Columns[2].Width = 90;
            dgvOffering.Columns[3].Width = 80;
            dgvOffering.Columns[4].Width = 80;
            dgvOffering.Columns[5].Width = 80;
            dgvOffering.Columns[6].Width = 80;
        }
        private void UpdateDB()
        {//UPDATE 
            string OfferingDateTime = dtpOffering.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string strtxOffering = "', NormOffering = '" + cbOfferTotalAmt.Items[0].ToString() + "', Tithe = '" + cbOfferTotalAmt.Items[1].ToString()
                   + "', F1stfruit = '" + cbOfferTotalAmt.Items[2].ToString() + "', Thanksgiving = '" + cbOfferTotalAmt.Items[3].ToString()
                   + "', Missions = '" + cbOfferTotalAmt.Items[4].ToString() + "', Building = '" + cbOfferTotalAmt.Items[5].ToString()
                   + "', Pledges = '" + cbOfferTotalAmt.Items[6].ToString() + "', Special = '" + cbOfferTotalAmt.Items[7].ToString()
                   + "', OtherOffering = '" + cbOfferTotalAmt.Items[8].ToString();  
            string strtxAttendance = "', NoMale = '" + txNoMale.Text + "', NoFemale = '" + txNoFemale.Text + "', Nochild = '" + txNoChild.Text 
                   + "', NoTotal =  '" + txNoTotal.Text + "', No1stTimer = '" + txNo1stTimer.Text 
                   + "', NoNewConvert = '" + txNoNewConvert.Text + "', NoSunSch = '" + txSundaySch.Text; 

            string query = "UPDATE " + Table + " SET Date_Time = '" + OfferingDateTime + "', Service = '" + cbService.Text 
                   + strtxOffering + strtxAttendance + "', TotalDeposit = '" + txTotalDeposit.Text + "', TotalAmount = '" 
                   + txTotalAmount.Text + "', Preacher = '" + txPreacher.Text + "' WHERE Id = '" + strId0 + "'";
            //MessageBox.Show(txTotalDeposit.Text);
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
            //UpdateBank();  + " SET Date_Time = '" + Date_Time + 
        }
        private void UpdateBank(string DateUpdate)
        {//UPDATE  
            string Descript = "Weekly offerings lodgement " + DateRange;
            string query = "UPDATE AccountInfo SET Date_Time = '" + DateUpdate + "', Amount = '" + TotalDeposit + "', Descript ='" + Descript + "' WHERE Id = '" + (int.Parse(strId0) + 1).ToString() + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void Calculate()
        {
            txTotalDeposit.Text =(float.Parse(cbAddCash.Items[0].ToString()) + float.Parse(cbAddCash.Items[1].ToString()) + float.Parse(cbAddCash.Items[2].ToString())
                                + float.Parse(cbAddCash.Items[3].ToString()) + float.Parse(cbAddCash.Items[4].ToString()) + float.Parse(cbAddCash.Items[5].ToString())
                                + float.Parse(cbAddCash.Items[6].ToString()) + float.Parse(cbAddCash.Items[7].ToString()) + float.Parse(cbAddCash.Items[8].ToString())).ToString("F2");
          
            txTotalAmount.Text =(float.Parse(cbOfferTotalAmt.Items[0].ToString()) + float.Parse(cbOfferTotalAmt.Items[1].ToString()) + float.Parse(cbOfferTotalAmt.Items[2].ToString())
                               + float.Parse(cbOfferTotalAmt.Items[3].ToString()) + float.Parse(cbOfferTotalAmt.Items[4].ToString()) + float.Parse(cbOfferTotalAmt.Items[5].ToString())
                               + float.Parse(cbOfferTotalAmt.Items[6].ToString()) + float.Parse(cbOfferTotalAmt.Items[7].ToString()) + float.Parse(cbOfferTotalAmt.Items[8].ToString())).ToString("F2");

            txNoTotal.Text     = (int.Parse(txNoMale.Text) + int.Parse(txNoFemale.Text) + int.Parse(txNoChild.Text)).ToString();
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            DialogResult mgSaveUpdate = MessageBox.Show("Are the input records correct and do you want to continue?", "Record Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgSaveUpdate == DialogResult.No) { btSaveData.Enabled = true; return; }
            else btSaveData.Enabled = false;
            
            Calculate();
            if (btSaveData.Text == "Save Data")
            {
                InsertOfferingDB_NotYet();
                InsertDB();
                SelectDB();
                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + MaxId().ToString() + ") Offering record created and saved. ");
                swop.Flush(); swop.Close();

                DialogResult mgUpdate = MessageBox.Show("Data Saved. Do you want to start a new record?", "Record Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes) Clear();
            }
            else
            {
                UpdateDB();
                SelectDB();

                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + strId0 + ") Offering record updated. ");
                swop.Flush(); swop.Close();

                DialogResult mgUpdate = MessageBox.Show("Data updated. Do you want to start a new record?", "Record Information ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgUpdate == DialogResult.Yes) Clear();
            }
            btSaveData.Enabled = false;

            gbOfferings.Enabled = false; gbAttendance.Enabled = false;
            btSaveData.Enabled = false; btClear.Enabled = false; btAddOffering.Enabled = false;
            cbAddForeignCurrency.Enabled = true; cbAddBankLodge.Enabled = true;
            fmLogin.SavingUpdate("SqlOffer", "Saving");             
        }        
        private void Clear()
        {
            for (int i = 0; i < cbOffering.Items.Count; i++)
            {
                cbAddForeignCurrency.Items[i] = "0.00";     cbAddBankLodge.Items[i] = "0.00";
                cbAddCash.Items[i] = "0.00";                cbOfferTotalAmt.Items[i] = "0.00";
            }          
            txTotalDeposit.Text = "0.00"; txTotalAmount.Text = "0.00";

            txNoMale.Text = "0"; txNoFemale.Text = "0"; txNoChild.Text = "0"; txNoTotal.Text = "0";

            txNo1stTimer.Text = "0"; btAddFirstTimer.Text = "Add 1st Timer";
            txNoNewConvert.Text = "0"; btAddConvert.Text = "Add Convert";
            txSundaySch.Text = "0";   txPreacher.Text = "Preacher";
            ClearOffering();

            btSaveData.Text = "Save Data"; 
        }
        private void btClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void tscbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectDB();
        }
        private void dtpOfferingFrom_ValueChanged(object sender, EventArgs e)
        {
            SelectDB();
        }
        private void dtpOfferingTo_ValueChanged(object sender, EventArgs e)
        {
            SelectDB();
        }
        private void btNew_Click(object sender, EventArgs e)
        {
            cbService.Enabled = true; btSaveData.Enabled = true; 
            btSaveData.Text = "Save Data"; btAddOffering.Text = "Add: " + cbOffering.Text;
            btAddBankLodge.Text = "Add: Bank"; btAddForeignCurrency.Text = "Add: Foriegn";
            btAddConvert.Text = "Add Convert"; btAddFirstTimer.Text = "Add 1st Timer";

            gbOfferings.Enabled = true; btClear.Enabled = true; mnEditRecord.Visible = false;
            gbAttendance.Enabled = true; btAddOffering.Enabled = true;
            btAddForeignCurrency.Enabled = true; btAddBankLodge.Enabled = true;
            btClear_Click(sender, e);
            cbService.SelectedIndex = 0;  cbOffering.SelectedIndex = 0;
        }
        private void mnDelete_Click(object sender, EventArgs e)
        {
            if (dgvOffering.CurrentRow == null) return; 
            DialogResult mgSaveUpdate = MessageBox.Show("Are you sure you want to delete the data?", "delete Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgSaveUpdate == DialogResult.No) return;
            else
            {
                int SelIndex = dgvOffering.CurrentRow.Index;
                string strId = dgvOffering.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                string queryx = "DELETE FROM " + Table + " WHERE Id = '" + strId + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    conTblx.Open();
                    cmdOfferingx.ExecuteNonQuery();
                }
                Clear();
                dgvOffering.Rows.RemoveAt(SelIndex);
                dgvOffering.Refresh();


                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Offering record with RegId(" + SelIndex + ") deleted by User RegId(" + fmLogin.RegId + ")");
                swop.Flush(); swop.Close();
            }
        }
        private void mnEditDate_Click(object sender, EventArgs e)
        {
            dtpOffering.Enabled = true;
        }
        private void mnEnableTextbox_Click(object sender, EventArgs e)
        {
            cbAddBankLodge.Enabled = false; cbAddForeignCurrency.Enabled = false;
            txTotalDeposit.ReadOnly = false;
            txTotalAmount.ReadOnly = false;
        }  
        private void EditRecord()
        {
            int SelIndex = dgvOffering.CurrentRow.Index;
            strId0 = dgvOffering.Rows[SelIndex].Cells[0].FormattedValue.ToString();
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
            dtpOffering.Value = DateTime.Parse(dgvEdit.Rows[0].Cells[1].FormattedValue.ToString());
            cbService.SelectedItem = dgvEdit.Rows[0].Cells[2].FormattedValue.ToString();

            cbOfferTotalAmt.Items[0] = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString(); //dTable.Rows[0][0].ToString()
            cbOfferTotalAmt.Items[1] = dgvEdit.Rows[0].Cells[4].FormattedValue.ToString();
            cbOfferTotalAmt.Items[2] = dgvEdit.Rows[0].Cells[5].FormattedValue.ToString();
            cbOfferTotalAmt.Items[3] = dgvEdit.Rows[0].Cells[6].FormattedValue.ToString();
            cbOfferTotalAmt.Items[4] = dgvEdit.Rows[0].Cells[7].FormattedValue.ToString();
            cbOfferTotalAmt.Items[5] = dgvEdit.Rows[0].Cells[8].FormattedValue.ToString();
            cbOfferTotalAmt.Items[6] = dgvEdit.Rows[0].Cells[9].FormattedValue.ToString();
            cbOfferTotalAmt.Items[7] = dgvEdit.Rows[0].Cells[10].FormattedValue.ToString();
            cbOfferTotalAmt.Items[8] = dgvEdit.Rows[0].Cells[11].FormattedValue.ToString();

            txNoMale.Text = dgvEdit.Rows[0].Cells[12].FormattedValue.ToString();
            txNoFemale.Text = dgvEdit.Rows[0].Cells[13].FormattedValue.ToString();
            txNoChild.Text = dgvEdit.Rows[0].Cells[14].FormattedValue.ToString();
            txNoTotal.Text = dgvEdit.Rows[0].Cells[15].FormattedValue.ToString();
            txNo1stTimer.Text = dgvEdit.Rows[0].Cells[16].FormattedValue.ToString();
            txNoNewConvert.Text = dgvEdit.Rows[0].Cells[17].FormattedValue.ToString();
            txSundaySch.Text = dgvEdit.Rows[0].Cells[18].FormattedValue.ToString();

            txTotalDeposit.Text = dgvEdit.Rows[0].Cells[19].FormattedValue.ToString();
            txTotalAmount.Text = dgvEdit.Rows[0].Cells[20].FormattedValue.ToString();
            txPreacher.Text = dgvEdit.Rows[0].Cells[21].FormattedValue.ToString();

            cbService.Enabled = false;   cbOffering.SelectedIndex = 0;
        }
        private void tssbtViewRecord_ButtonClick(object sender, EventArgs e)
        {
            if (dgvOffering.CurrentRow == null) return;
            else
            {
                EditRecord();
                for (int i = 0; i < cbOffering.Items.Count; i++)
                {
                    cbOffering.SelectedIndex = i;
                    EditOffering(); 
                }
                cbOffering.SelectedIndex = 0;
                gbOfferings.Enabled = false; gbAttendance.Enabled = false;
                btSaveData.Enabled = false; btClear.Enabled = false; btAddOffering.Enabled = false;
                mnEditRecord.Visible = true;
            }
        }
        private void mnEditRecord_Click(object sender, EventArgs e)
        {
            if (dgvOffering.CurrentRow == null) return;
            else
            {
                EditRecord();
                btSaveData.Text = "Update Data"; btSaveData.Enabled = true;
                btAddOffering.Text = "Edit: " + cbOffering.Text;
                btAddForeignCurrency.Text = "Edit: Foriegn"; btAddForeignCurrency.Enabled = true;
                btAddBankLodge.Text = "Edit: Bank"; btAddBankLodge.Enabled = true;
                btAddConvert.Text = "Edit Convert"; btAddFirstTimer.Text = "Edit 1st Timer";

                gbOfferings.Enabled = true; gbAttendance.Enabled = true;
                btClear.Enabled = true; btAddOffering.Enabled = true; cbService.Enabled = true;
            }
        }
        private int  MaxId()
        {
            string strPixId = "SELECT Id FROM " + Table + " ORDER BY Id DESC LIMIT 1";
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
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btAddOffering_Click(object sender, EventArgs e)
        {
            CreateOfferingTable();
            CalculateDollarCent();
            DialogResult mgSaveUpdate = MessageBox.Show("Are the input records correct and do you want to continue and \nis this amount AUD$" + DollCent.ToString() + " correct for the total cash?", "Dollar-Cent Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mgSaveUpdate == DialogResult.Yes)
            {
                cbAddCash.Items[cbAddCash.SelectedIndex] = DollCent.ToString("F2");
                cbOfferTotalAmt.Items[cbOffering.SelectedIndex] = (float.Parse(ForeignValue()) + float.Parse(BankValue()) + float.Parse(CashValue())).ToString("F2");        
            }
            else return;

            if (btAddOffering.Text == "Add: " + cbOffering.Text)
            {
                InsertOfferingDB(); btAddOffering.Text = "Saved: " + cbOffering.Text;
                if (cbOffering.SelectedIndex == 1)
                {
                    DialogResult mgAddTithe = MessageBox.Show("Do you want to record tithes for the tithers?", "Tithe Recording ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (mgAddTithe == DialogResult.Yes) AddOfferingTitheOnly();
                }
            }
            else//(btAddOffering.Text = "Edit: " + cbOffering.Text)
            {
                UpdateOfferingDB(); btAddOffering.Text = "Updated: " + cbOffering.Text;
                if (cbOffering.SelectedIndex == 1)
                {
                    DialogResult mgAddTithe = MessageBox.Show("Do you want to update tithes for the tithers?", "Tithe Recording ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (mgAddTithe == DialogResult.Yes) AddOfferingTitheOnly();
                }
            }
            if (cbOffering.SelectedIndex < 8) cbOffering.SelectedIndex = cbOffering.SelectedIndex + 1;
            else cbOffering.SelectedIndex = 0;
        }
        private void cbOffering_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbOfferTotalAmt.SelectedIndex = cbOffering.SelectedIndex;
            cbAddCash.SelectedIndex = cbOffering.SelectedIndex;
            cbAddForeignCurrency.SelectedIndex = cbOffering.SelectedIndex;
            cbAddBankLodge.SelectedIndex = cbOffering.SelectedIndex;
            if (btSaveData.Text == "Save Data" && float.Parse(OfferingValue()) == 0.0f)
            {
                btAddOffering.Text = "Add: " + cbOffering.Text;
                ClearOffering();
            }
            else
            {
                btAddOffering.Text = "Edit: " + cbOffering.Text;
                EditOffering();
            }
        }
        private void CalculateDollarCent()
        {
            txDollar100Amt.Text = (100 * int.Parse(txDollar100Qty.Text)).ToString();
            txDollar50Amt.Text = (50 * int.Parse(txDollar50Qty.Text)).ToString();
            txDollar20Amt.Text = (20 * int.Parse(txDollar20Qty.Text)).ToString();
            txDollar10Amt.Text = (10 * int.Parse(txDollar10Qty.Text)).ToString();
            txDollar5Amt.Text = (5 * int.Parse(txDollar5Qty.Text)).ToString();
            txDollar2Amt.Text = (2 * int.Parse(txDollar2Qty.Text)).ToString();
            txDollar1Amt.Text = (1 * int.Parse(txDollar1Qty.Text)).ToString();
            int DollarTotal = int.Parse(txDollar100Amt.Text) + int.Parse(txDollar50Amt.Text) + int.Parse(txDollar20Amt.Text) +
                              int.Parse(txDollar10Amt.Text) + int.Parse(txDollar5Amt.Text) + int.Parse(txDollar2Amt.Text) + int.Parse(txDollar1Amt.Text);

            txCent50Amt.Text = (50 * int.Parse(txCent50Qty.Text)).ToString();
            txCent20Amt.Text = (20 * int.Parse(txCent20Qty.Text)).ToString();
            txCent10Amt.Text = (10 * int.Parse(txCent10Qty.Text)).ToString();
            txCent5Amt.Text = (5 * int.Parse(txCent5Qty.Text)).ToString();
            int CentTotal = int.Parse(txCent50Amt.Text) + int.Parse(txCent20Amt.Text) + int.Parse(txCent10Amt.Text) + int.Parse(txCent5Amt.Text);

            DollCent = DollarTotal + CentTotal / 100.00f; 
        }
        private string OfferingTable()
        {
            string OfferingTable = "";
            if (cbOffering.SelectedIndex == 0) OfferingTable = "NormOfferingInfo";
            if (cbOffering.SelectedIndex == 1) OfferingTable = "TitheInfo";
            if (cbOffering.SelectedIndex == 2) OfferingTable = "F1stFruitInfo";
            if (cbOffering.SelectedIndex == 3) OfferingTable = "ThanksgivingInfo";
            if (cbOffering.SelectedIndex == 4) OfferingTable = "MissionsInfo";
            if (cbOffering.SelectedIndex == 5) OfferingTable = "BuildingFundInfo";
            if (cbOffering.SelectedIndex == 6) OfferingTable = "PledgesInfo";
            if (cbOffering.SelectedIndex == 7) OfferingTable = "SpecialInfo";
            if (cbOffering.SelectedIndex == 8) OfferingTable = "OtherOfferingInfo";
            return OfferingTable;
        }
        private void CreateOfferingTable()
        {      // Create Table 
            string strDollarCent = "Cent5Qty SMALLINT, Cent10Qty SMALLINT, Cent20Qty SMALLINT, Cent50Qty SMALLINT, "
                   + "Dollar1Qty SMALLINT, Dollar2Qty SMALLINT, Dollar5Qty SMALLINT, Dollar10Qty SMALLINT, "
                   + "Dollar20Qty SMALLINT, Dollar50Qty SMALLINT, Dollar100Qty SMALLINT, ";
            string strTbl = "CREATE TABLE IF NOT EXISTS " + OfferingTable() + " (Id SMALLINT, " + strDollarCent + "ForeignCurrency DECIMAL(8,2), BankLodge DECIMAL(8,2));";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTbl, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }
        }
        private void InsertOfferingDB()
        { //Insert 
            strId0 = (MaxId() + 1).ToString();
            // pre delete if exist
            string queryx = "DELETE FROM " + OfferingTable() + " WHERE Id = '" + strId0 + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }
            // Insert
            string strDollarCent = "Cent5Qty, Cent10Qty, Cent20Qty, Cent50Qty, Dollar1Qty, Dollar2Qty, Dollar5Qty, Dollar10Qty, "
                                 + "Dollar20Qty, Dollar50Qty, Dollar100Qty, ";
            string strtxDollarCent = txCent50Qty.Text + "', '" + txCent20Qty.Text + "', '" + txCent10Qty.Text + "', '" + txCent5Qty.Text + "', '"
                                     + txDollar100Qty.Text + "', '" + txDollar50Qty.Text + "', '" + txDollar20Qty.Text + "', '" + txDollar10Qty.Text
                                     + "', '" + txDollar5Qty.Text + "', '" + txDollar2Qty.Text + "', '" + txDollar1Qty.Text;

            string query = "INSERT INTO " + OfferingTable() + " (Id, " + strDollarCent + "ForeignCurrency, BankLodge) VALUES ('" + strId0
                         + "', '" + strtxDollarCent + "', '" + ForeignValue() + "', '" + BankValue() + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void InsertOfferingDB_NotYet()
        { //Insert 
            for (int i = 0; i <= cbOffering.Items.Count - 1; i++)
            {
                cbOffering.SelectedIndex = i;
                if (OfferingValue() == "0.00")
                {
                    CreateOfferingTable();
                    strId0 = (MaxId() + 1).ToString();
                    string strDollarCent = "Cent5Qty, Cent10Qty, Cent20Qty, Cent50Qty, Dollar1Qty, Dollar2Qty, Dollar5Qty, Dollar10Qty, "
                                         + "Dollar20Qty, Dollar50Qty, Dollar100Qty, ";
                    string strtxDollarCent = "0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0";
                    string query = "INSERT INTO " + OfferingTable() + " (Id, " + strDollarCent + "ForeignCurrency, BankLodge) "
                                 + "VALUES ('" + strId0 + "', '" + strtxDollarCent + "', '0.00', '0.00');";
                    using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
                    {
                        SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                        conTbly.Open();
                        cmdOffering.ExecuteNonQuery();
                    }
                }
            }
        }
        private void UpdateOfferingDB()
        {//UPDATE
            string strtxDollarCent = "Cent5Qty = '" + txCent50Qty.Text + "', Cent10Qty = '" + txCent20Qty.Text
                   + "', Cent20Qty = '" + txCent10Qty.Text + "', Cent50Qty = '" + txCent5Qty.Text + "', Dollar1Qty = '" + txDollar100Qty.Text
                   + "', Dollar2Qty = '" + txDollar50Qty.Text + "', Dollar5Qty = '" + txDollar20Qty.Text + "',  Dollar10Qty = '" + txDollar10Qty.Text
                   + "', Dollar20Qty = '" + txDollar5Qty.Text + "', Dollar50Qty = '" + txDollar2Qty.Text + "', Dollar100Qty = '" + txDollar1Qty.Text + "'";
            string query = "UPDATE " + OfferingTable() + " SET  " + strtxDollarCent + ", ForeignCurrency = '" + ForeignValue()
                                     + "', BankLodge = '" + BankValue() + "' WHERE Id = '" + strId0 + "'";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private string OfferingValue()
        {
            return cbOfferTotalAmt.Items[cbOffering.SelectedIndex].ToString();
        }
        private string CashValue()
        {
            return cbAddCash.Items[cbAddCash.SelectedIndex].ToString();
        }
        private string ForeignValue()
        {
            return cbAddForeignCurrency.Items[cbAddForeignCurrency.SelectedIndex].ToString();
        }
        private string BankValue()
        {
            return cbAddBankLodge.Items[cbAddBankLodge.SelectedIndex].ToString();
        }
        private void EditOffering()
        {
            string queryx = "";
            queryx = "SELECT * FROM " + OfferingTable() + " WHERE Id = '" + strId0 + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvEdit.DataSource = dTable;
            }
            txCent50Qty.Text = dgvEdit.Rows[0].Cells[1].FormattedValue.ToString();
            txCent20Qty.Text = dgvEdit.Rows[0].Cells[2].FormattedValue.ToString();
            txCent10Qty.Text = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString();
            txCent5Qty.Text = dgvEdit.Rows[0].Cells[4].FormattedValue.ToString();

            txDollar100Qty.Text = dgvEdit.Rows[0].Cells[5].FormattedValue.ToString();
            txDollar50Qty.Text = dgvEdit.Rows[0].Cells[6].FormattedValue.ToString();
            txDollar20Qty.Text = dgvEdit.Rows[0].Cells[7].FormattedValue.ToString();
            txDollar10Qty.Text = dgvEdit.Rows[0].Cells[8].FormattedValue.ToString();
            txDollar5Qty.Text = dgvEdit.Rows[0].Cells[9].FormattedValue.ToString();
            txDollar2Qty.Text = dgvEdit.Rows[0].Cells[10].FormattedValue.ToString();
            txDollar1Qty.Text = dgvEdit.Rows[0].Cells[11].FormattedValue.ToString();
            
            cbAddForeignCurrency.Items[cbOffering.SelectedIndex] = float.Parse(dgvEdit.Rows[0].Cells[12].FormattedValue.ToString()).ToString("F2");
            cbAddBankLodge.Items[cbOffering.SelectedIndex] = float.Parse(dgvEdit.Rows[0].Cells[13].FormattedValue.ToString()).ToString("F2");
            CalculateDollarCent(); cbAddCash.Items[cbOffering.SelectedIndex] = DollCent.ToString("F2");
            cbOfferTotalAmt.Items[cbOffering.SelectedIndex] = (float.Parse(ForeignValue()) + float.Parse(BankValue()) + float.Parse(CashValue())).ToString("F2");                  
        }
        private void ClearOffering()
        {
            txDollar100Amt.Text = "0"; txDollar100Qty.Text = "0";
            txDollar50Amt.Text = "0"; txDollar50Qty.Text = "0";
            txDollar20Amt.Text = "0"; txDollar20Qty.Text = "0";
            txDollar10Amt.Text = "0"; txDollar10Qty.Text = "0";
            txDollar5Amt.Text = "0"; txDollar5Qty.Text = "0";
            txDollar2Amt.Text = "0"; txDollar2Qty.Text = "0";
            txDollar1Amt.Text = "0"; txDollar1Qty.Text = "0";

            txCent50Amt.Text = "0"; txCent50Qty.Text = "0";
            txCent20Amt.Text = "0"; txCent20Qty.Text = "0";
            txCent10Amt.Text = "0"; txCent10Qty.Text = "0";
            txCent5Amt.Text = "0"; txCent5Qty.Text = "0";

            cbAddForeignCurrency.Items[cbAddForeignCurrency.SelectedIndex] = "0.00";
            cbAddBankLodge.Items[cbAddBankLodge.SelectedIndex] = "0.00";
            cbAddCash.Items[cbAddCash.SelectedIndex] = "0.00";
            btAddBankLodge.Text = "Add: Bank"; btAddForeignCurrency.Text = "Add: Foriegn";
            btAddOffering.Text = "Add: " + cbOffering.Text;
        }

        private void btAddCash_Click(object sender, EventArgs e)
        {

        }
        private void btAddBankLodge_Click(object sender, EventArgs e)
        {
            string strOfferingId = ""; 
            if (btAddBankLodge.Text == "Edit: Bank") strOfferingId = strId0; else strOfferingId = (MaxId() + 1).ToString();
            FmBankLodge fmBankLodge = new FmBankLodge();
            fmBankLodge.BankLodgeAdd("Bank", strOfferingId, cbOffering.Text);
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                cbAddBankLodge.Items[cbAddBankLodge.SelectedIndex] = sr.ReadLine();
                fs.Close(); sr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void btAddForeignCurrency_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);

                if (btAddForeignCurrency.Text == "Edit: Foreign") swop.WriteLine(strId0);
                else swop.WriteLine((MaxId() + 1).ToString());
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            FmForeignCurrency fbl = new FmForeignCurrency();
            fbl.ForeignLodge();

            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                cbAddForeignCurrency.Items[cbAddForeignCurrency.SelectedIndex] = float.Parse(sr.ReadLine()).ToString("F2");
                fs.Close(); sr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }       
        private void AddOfferingTitheOnly()
        {
            string strOfferingId = "";
            if (btAddBankLodge.Text == "Edit: Bank") strOfferingId = strId0; else strOfferingId = (MaxId() + 1).ToString();
            FmBankLodge fmBankLodge = new FmBankLodge();
            fmBankLodge.BankLodgeAdd("Cash", strOfferingId, cbOffering.Text);
        }
        private void btAddFirstTimer_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("FirstTimer");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            int NoF1st = int.Parse(txNo1stTimer.Text);
            FmFirstTimer fmFirstTimer = new FmFirstTimer();
            fmFirstTimer.ShowDialog();
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                NoF1st += int.Parse(sr.ReadLine());
                txNo1stTimer.Text = NoF1st.ToString();
                fs.Close(); sr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void btAddConvert_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine("Convert");
                swop.Flush(); swop.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            int NoNewConvert = int.Parse(txNoNewConvert.Text);
            FmFirstTimer fmFirstTimer = new FmFirstTimer();
            fmFirstTimer.ShowDialog();
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                NoNewConvert += int.Parse(sr.ReadLine());
                txNoNewConvert.Text = NoNewConvert.ToString();
                fs.Close(); sr.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private string RCCGYEAR()
        {
            if (DateTime.Now.Month > 8) return "-RCCG Year " + DateTime.Now.ToString("yyyy") + "_" + DateTime.Now.AddYears(1).ToString("yyyy");
            else return "-RCCG Year " + DateTime.Now.AddYears(-1).ToString("yyyy") + "_" + DateTime.Now.ToString("yyyy");            
        }
        private void mnReport_Click(object sender, EventArgs e)
        {
            int Mnt = 0, Monthx = 0;  DoRetry = true;
            while (DoRetry == true)
            {
                if (dialogReportDate(ref AnsMonth, ref DoRetry) == DialogResult.OK)
                {
                    Monthx = DateTime.Now.Month - AnsMonth;
                    if (Monthx < 1) Monthx = 12 + Monthx;
                }
                else return;
            }

            using (StreamReader sr = new StreamReader(fmLogin.PathResource + "ReportDate" + RCCGYEAR() + ".dat"))
            {
                if (Monthx > 8) Mnt = Monthx - 8; else Mnt = Monthx + 4;
                char[] sep = new char[] { ' ' };
                for (int i = 1; i < Mnt; i++) sr.ReadLine(); //Read off
                string[] splitCurrMonthDate = sr.ReadLine().Split(sep, StringSplitOptions.RemoveEmptyEntries);

                int Monthy = 0, Yeary = 0;
                if (Monthx == 1) { Monthy = 12; Yeary = DateTime.Now.AddYears(-1).Year; } else { Monthy = Monthx - 1; Yeary = DateTime.Now.Year; }
                //MessageBox.Show("Monthx = " + Monthx.ToString() + ";  " + splitCurrMonthDate[3].Substring(0, 2) + "/" + Monthy.ToString() + "/" + Yeary.ToString());
                dtpOfferingFrom.Value = DateTime.Parse(splitCurrMonthDate[3].Substring(0, 2) + "/" + Monthy.ToString() + "/" + Yeary.ToString());
                dtpOfferingTo.Value = DateTime.Parse(splitCurrMonthDate[7].Substring(0, 2) + "/" + Monthx.ToString() + "/" + DateTime.Now.Year.ToString());

                string[] splitNextMonthDate = sr.ReadLine().Split(sep, StringSplitOptions.RemoveEmptyEntries); string nextReportDate = "";
                if (Monthx == 12) nextReportDate = splitNextMonthDate[10].Substring(0, 2) + "/1/" + DateTime.Now.AddYears(1).Year.ToString();
                else nextReportDate = splitNextMonthDate[10].Substring(0, 2) + "/" + (Monthx + 1).ToString() + "/" + DateTime.Now.Year.ToString();
                DialogResult DialogReport = MessageBox.Show("The next report date: " + nextReportDate, "Next Report Date", MessageBoxButtons.OKCancel);
                if (DialogReport == DialogResult.OK)
                {
                    FmInsatllLogin fmInsatllLogin = new FmInsatllLogin();
                    fmInsatllLogin.LoadReport(nextReportDate);
                    Report();
                }
                else return;
            }// sr.Close(); fs.Close(); fs.Dispose(); 
        }       
        private void Report()
        {   // Set next report date first!          
            string RemitAmount = "", Tithe5Percent = "";
            DataRange = dtpOfferingTo.Value.ToString("MMMM yyyy") + " (" + dtpOfferingFrom.Value.ToString("ddMMyyyy") + "-" + dtpOfferingTo.Value.ToString("ddMMyyyy") + ")";
            ReportFileName = fmLogin.PathResource + "Financial Report" + RCCGYEAR() + ".xlsx";
            try
            {
                ExcelReport excelReport = new ExcelReport();
                string strReturn = excelReport.ExportToExcel(MonthlyIncome(), MonthlyExpenses(), ReportFileName, dtpOfferingTo.Value.ToString("MMMM, yyyy"), DataRange, RCCGYEAR() + ".xlsx");
                MessageBox.Show("Inside Offering: " + strReturn);
                string[] ReturnArr = strReturn.Split(new[] {'?'}, StringSplitOptions.None);
                Tithe5Percent = (float.Parse(ReturnArr[0]) * 0.05f).ToString("#,###,##0.00"); 
                RemitAmount = float.Parse(ReturnArr[1]).ToString("#,###,##0.00"); 
                }
            catch (Exception ex) { if (ex.Message != "Part does not exist.") MessageBox.Show("\r\nException #: " + ex.Message); }

            #region //Openning the created excel files using MS Excel Application
            string RemitReport = fmLogin.PathResource + "Remittance Report" + RCCGYEAR() + ".xlsx";
            string OtherReportFileName = fmLogin.PathResource + "Other Statistical Report" + RCCGYEAR() + ".xlsx";
            //*
            Process ProcOtherReportFileName;
            if (File.Exists(OtherReportFileName))ProcOtherReportFileName = Process.Start(OtherReportFileName);
            else
            {
                OpenFileDialog PixOpen = new OpenFileDialog();
                PixOpen.Filter = "Report Files|*.xlsx";
                PixOpen.Title = "Load Other Statistical Report File";
                if (PixOpen.ShowDialog() == DialogResult.OK)
                { 
                    File.Copy(PixOpen.FileName, OtherReportFileName);
                    ProcOtherReportFileName = Process.Start(OtherReportFileName);
                }
                else return;
            }
            Process ProcReportFileName = Process.Start(ReportFileName);
            Process ProcRemitReport = Process.Start(RemitReport); //*/               
            #endregion 

            DialogResult DialogOpenReport = MessageBox.Show("Check the report files and edit as appropriate then click OK to continue?", "Info on Report Files", MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
            if (DialogOpenReport == DialogResult.OK)
            {
                //ProcOtherReportFileName.Kill(); //ProcReportFileName.Kill(); //ProcRemitReport.Kill(); --//Closes the excel files

                string ReportBody = "Dear Daddy,"
                     + "\n\nGood morning to you sir in Jesus' name."
                     + "\nEku ise Oluwa sir, the Lord will continue to strengthen and multiply His grace upon your life, family "
                     + "and ministry in Jesus' name. Amen."
                     + "\n\nFind attached are this month reports, sir."
                     + "\n\nGreetings to Mummy.";
                FmEmailPad fmEmailPad = new FmEmailPad();
                fmEmailPad.EmailReport(DataRange, RCCGYEAR() + ".xlsx", ReportFileName, RemitReport, OtherReportFileName, fmLogin.ReportEmail1, ReportBody);

                DialogResult mgSendEmail = MessageBox.Show("Would like to send emails to signatories?", "Signatory Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgSendEmail == DialogResult.Yes)
                {
                    string strDetailxz = "Dear All,"
                    + "\n\nFind attached copies of this month report sent to the regional office and details of remittance to be deposited:"
                    + "\n\n ****** Details for depositing the remittance *******"
                    + "\n Bank Name: CBA \n Account Name: RCCG Inc."
                    + "\n Account Number: 10703234; \n BSB: 062900"
                    + "\n Reference:  ACT " + dtpOfferingTo.Value.ToString("MMM") + ". Remit"
                    + "\n Deposit Amount = AUD$ " + RemitAmount
                    + "\n\n ****** Details for contribution to Redemption Compassion Purse *******"
                    + "\n Contribution = AUD$ " + Tithe5Percent                    
                    + "\n\n May your service be acceptable by the Almighty God. Remain blessed and rapturable.\n";
                    FmEmailPad fmEmailPadx = new FmEmailPad(); 
                    fmEmailPadx.EmailReport(DataRange, RCCGYEAR() + ".xlsx", ReportFileName, RemitReport, OtherReportFileName, fmLogin.ReportEmail2 + ";" + fmLogin.ReportEmail3, strDetailxz);
                }
            }
            //else ProcOtherReportFileName.Kill(); //Closes the excel files 
        }
        private DataTable MonthlyIncome()
        {
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string ODTTo = dtpOfferingTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
            string queryx = "SELECT Date_Time, Service, Preacher, NoMale AS Men, NoFemale AS Women, NoChild AS Children, NoTotal AS Total, "
                   + "NoSunSch AS Sunday_School, NormOffering AS Offering, Tithe, F1stfruit AS First_Fruit, Thanksgiving, Missions, Building, Pledges, Special, OtherOffering, "
                   + "TotalAmount FROM OfferingInfo WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Service DESC;";

            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvOffering.DataSource = dTable;
                dgvOffering.Columns[0].Width = 80;
                dgvOffering.Columns[1].Width = 80;
                dgvOffering.Columns[2].Width = 80;
                dgvOffering.Columns[3].Width = 80;
                dgvOffering.Columns[4].Width = 80;
                return dTable;
            }
        }
        private DataTable MonthlyExpenses()
        { 
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd");
            string ODTTo = dtpOfferingTo.Value.ToString("yyyy-MM-dd");
            string queryx = "SELECT Date_Time, Descript, Amount FROM AccountInfo WHERE Account = 'CBA_Bank' AND Transact = 'Debit' AND "
                          + "Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "' ORDER BY Date_Time DESC;";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);

                dTable.Columns.Add("xx2").SetOrdinal(2); dTable.Columns.Add("xx3").SetOrdinal(3);
                dTable.Columns.Add("xx4").SetOrdinal(4); dTable.Columns.Add("xx5").SetOrdinal(5);
                dTable.Columns.Add("xx6").SetOrdinal(6);
                return dTable;
            }
        }

        public static string firstDayOfTheMonth(DateTime date, DayOfWeek day)
        {
            date = DateTime.Parse("1" + "/" + date.Month.ToString() + "/" + date.Year.ToString());
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }
            return date.ToString("yyyy-MM-dd");
        }
        
        #region // Offering Confirmation ********************************************
        bool NoPassword = false;
        private string DateRange, DepositDate, Username, FName, LName, Email, Title, RegIdx, strConfirmer, strDepositor, strVerifier, strEmailTo;        
        private void readUserGroup()
        {
            FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            sr.ReadLine();
            RegIdx = sr.ReadLine();
            fs.Close(); sr.Close();

            if (RegIdx == fmLogin.RootNo) { NoPassword = true; txConfirmerUser.ReadOnly = false; }
            else { NoPassword = false; txConfirmerUser.ReadOnly = true; } 
        }       
        private void SelectWeek()
        {
            string ODTFrom = dtpOfferingFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string ODTTo =   dtpOfferingTo.Value.ToString("yyyy-MM-dd") + " 23:59:59";
            string query1 = "SELECT SUM(TotalDeposit) AS TotalDeposit, SUM(TotalAmount) AS TotalAmount, SUM(NoMale) AS Male, "
                   + "SUM(NoFemale) AS Female, SUM(NoChild) AS Child, SUM(NoTotal) AS Attendance, SUM(NoSunSch) AS SunSch, "
                   + "SUM(No1stTimer) AS F1stTimer, SUM(NoNewConvert) AS Converts, GROUP_CONCAT(Preacher, '; ') AS Preacher FROM " + Table
                   + " WHERE Date_Time BETWEEN '" + ODTFrom + "' AND '" + ODTTo + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(query1, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdOfferingx;
                MyAdapter.Fill(dTable);
                dgvOffering.DataSource = dTable;
            }
            dgvOffering.Columns[0].Width = 80;
            dgvOffering.Columns[1].Width = 80;
            dgvOffering.Columns[2].Width = 80;
            dgvOffering.Columns[3].Width = 80;
            dgvOffering.Columns[4].Width = 80;
        }
        private void tsbtConfirm_Click(object sender, EventArgs e)
        {
            if (dtpOfferingTo.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                /*
                if (DateTime.Now.Date > dtpOfferingTo.Value.Date) { }
                else
                {
                    MessageBox.Show("This process must be done after the selected day.", "Selected Day", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }*/
                gb1.Visible = true;
                tsMain.Enabled = false; dtpOfferingFrom.Enabled = false; dtpOfferingTo.Enabled = false;
            }
            else
            {
                MessageBox.Show("The day of the week must be Sunday.", "Day of the Week", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dtpOfferingFrom.Value = dtpOfferingTo.Value.AddDays(-6);
            DateRange = dtpOfferingTo.Value.AddDays(-6).ToString("dd/MM/yyyy") + "-" + dtpOfferingTo.Value.ToString("dd/MM/yyyy");
            DepositDate = dtpOfferingTo.Value.ToString("yyyy-MM-dd HH:mm:ss");
            SelectWeek();
            lbDepositDate.Text = "Offerings for Mon " + dtpOfferingTo.Value.AddDays(-6).ToString("dd/MM/yyyy") + " - Sun " + dtpOfferingTo.Value.ToString("dd/MM/yyyy"); 
            TotalDeposit = dgvOffering.Rows[0].Cells[0].FormattedValue.ToString();
            dgvOffering.Columns[0].DefaultCellStyle.Format = "#,###,##0.00";
            dgvOffering.Columns[1].DefaultCellStyle.Format = "#,###,##0.00";
                        
            readUserGroup();
            string strCount = "SELECT UserName FROM LoginInfo WHERE RegId = '" + RegIdx + "';";// For confirmer
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix))
                {
                    txConfirmerUser.Text = Convert.ToString(cmd.ExecuteScalar());
                }
            }

            string queryx = "SELECT UserName FROM LoginInfo"; // For other usernames
            DataTable dTable;
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }
            for (int i = 0; i <= Countx() - 1; i++)
            {
                cbDepositor.Items.Add(dTable.Rows[i][0].ToString());
                cbVerifier.Items.Add(dTable.Rows[i][0].ToString());
            }           
        }
        private int Countx()
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

        private void btConfirm_Click(object sender, EventArgs e)
        {           
            if (GetDetail() == 0) return;
            else
            {
                string EmailSubject = "", strDetail = "";
                DialogResult DialogMsg = MessageBox.Show("Would you like to send email messages to the offering recorders?", "Email Inforamtion", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogMsg == DialogResult.Yes)
                {
                    try
                    {
                        EmailSubject = "Offering records for " + DateRange;
                        strDetail = "Dear All,"
                        + "\n\nFind below is a summary of the records of this week offerings for " + DateRange + "."
                        + "\nPlease, the depositor must ensure that the REFERENCE given below is used when depositing the moneny."
                        + "\n\nPlease, send a reply email if there is any discrepancy."
                        + "\n\n ****** Recorders' Details ******"
                        + "\n" + strConfirmer
                        + "\n" + strDepositor
                        + "\n" + strVerifier
                        + "\n\n ****** Details for depositing the offerings *******"
                        + "\n Bank Name: CBA \n Account Name: RCCG Inc."
                        + "\n Account Number: 10703234; \n BSB: 062900"
                        + "\n Reference: Dpt-" + dtpOfferingTo.Value.ToString("dd/MM/yyyy")
                        + "\n Deposit Amount = AUD$" + float.Parse(TotalDeposit).ToString("#,###,##0.00")
                        + "\n\n May your service be acceptable by the Almighty God. Remain blessed and rapturable.";

                        fmLogin.SendEmailSt("4", strEmailTo, "", EmailSubject, strDetail, "", null, fmLogin.NoneNull);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
                
                DialogResult mgSaveUpdate = MessageBox.Show("Are the amounts correct and would like to log it into bank account?", "Logging into bank account", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mgSaveUpdate == DialogResult.Yes) InsertOfferingDeposit();
                gb1.Visible = false; tsMain.Enabled = true; dtpOfferingFrom.Enabled = true;
                dtpOfferingTo.Enabled = true; strEmailTo = "";
                cbDepositor.Text = "Username"; cbVerifier.Text = "Username";
                txOtherRegId.Text = txOtherRegId.Init_Text;

                dtpOfferingTo.Value = DateTime.Now;
                dtpOfferingFrom.Value = dtpOfferingTo.Value.AddMonths(-1);
                SelectDB();
                fmLogin.SavingUpdate("SqlConfr", "Saving");
            }
        }
        private int  GetDetail()
        {
            if (txConfirmerUser.Text == cbDepositor.Text) { MessageBox.Show("Error! The confirmer and the depositor cannot be the same person. Try again."); return 0; }
            if (cbDepositor.Text == "Username") { MessageBox.Show("Error! You have not selected a depositor? Try again."); return 0; }
            if (cbVerifier.Text == "Username" && txOtherRegId.Text == "RegId") { MessageBox.Show("Error! You have not selected a verifier? Try again."); return 0; }

            //Confirmer
            Username = txConfirmerUser.Text;
            SelectLogin();
            strConfirmer = "Confirmer: " + Title + " " + FName + " " + LName;
            strEmailTo = Email + ";";
            if (ValidateUser(cbDepositor.Text, txPwDepositor.Text) == "Wrong") { MessageBox.Show("Error! Password of " + cbDepositor.Text + " is incorrect. Try again."); return 0; }
            else
            {
                Username = cbDepositor.Text;
                SelectLogin();
                strDepositor = "Depositor: " + Title + " " + FName + " " + LName;
                strEmailTo = strEmailTo + Email + ";";
            }

            if (rbVerifier.Checked == true)
            {
                if (ValidateUser(cbVerifier.Text, txPwVerifier.Text) == "Wrong") { MessageBox.Show("Error! Password of " + cbVerifier.Text + " is incorrect. Try again."); return 0; }
                else
                {
                    Username = cbVerifier.Text;
                    SelectLogin();
                    strVerifier = "Verifier: " + Title + " " + FName + " " + LName;
                    strEmailTo = strEmailTo + Email + ";";
                    return 1;
                }
            }
            else
            {//non-Verifier 
                RegId = txOtherRegId.Text;
                SelectRegInfo();
                strVerifier = "Verifier: " + Title + " " + FName + " " + LName;
                strEmailTo = strEmailTo + Email + "; ";
                return 1;
            }
        }
        private void SelectLogin()
        {
            DataTable dTable;
            string queryx = "SELECT First_Name, Last_Name, Email, Title FROM LoginInfo WHERE UserName = '" + Username + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }

            FName = dTable.Rows[0][0].ToString();
            LName = dTable.Rows[0][1].ToString();
            Email = dTable.Rows[0][2].ToString();
            Title = dTable.Rows[0][3].ToString();
        }
        private void SelectRegInfo()
        {
            DataTable dTable;
            string queryx = "SELECT First_Name, Last_Name, Email, Title FROM RegisterInfo WHERE Id = '" + RegId + "';";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
            }

            FName = dTable.Rows[0][0].ToString();
            LName = dTable.Rows[0][1].ToString();
            Email = dTable.Rows[0][2].ToString();
            Title = dTable.Rows[0][3].ToString();
        }
        private void InsertOfferingDeposit() 
        {// Create "DepositInfo" Table
            string strTblBL = "CREATE TABLE IF NOT EXISTS DepositInfo (Id INTEGER PRIMARY KEY AUTOINCREMENT, DepositDate DATETIME, DepositAmount DECIMAL(8,2), Depositor VARCHAR(50), Descript VARCHAR(100), EmailTo  VARCHAR(100));";  //VARCHAR);";
            using (SQLiteConnection conTbl = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdTbl = new SQLiteCommand(strTblBL, conTbl);
                conTbl.Open();
                cmdTbl.ExecuteNonQuery();
            }


            string strNewDepositor = strDepositor.Replace(": ", "(") + ")";
            string Descript = "Weekly offerings for the period " + DateRange + ". " + strDepositor + ", " + strConfirmer + ", " + strVerifier;
            string query2 = "INSERT INTO DepositInfo (DepositDate, DepositAmount, Depositor, Descript, EmailTo) VALUES ('"
                          + DepositDate + "', '" + TotalDeposit + "', '" + strNewDepositor + "', '" + Descript + "', '" + strEmailTo + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query2, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }        
        private void rbVerifier_CheckedChanged(object sender, EventArgs e)
        {
            if (rbVerifier.Checked == true)
            {
                cbVerifier.Enabled = true; txPwVerifier.Enabled = true;
                txOtherRegId.Enabled = false;
            }
            else
            {
                cbVerifier.Enabled = false; txPwVerifier.Enabled = false;
                txOtherRegId.Enabled = true;
            }
        }
        private void txPwDepositor_TextChanged(object sender, EventArgs e)
        {
            txPwDepositor.PasswordChar = Convert.ToChar("•");
        }
        private void txPwVerifier_TextChanged(object sender, EventArgs e)
        {
            txPwVerifier.PasswordChar = Convert.ToChar("•");
        }
        private string ValidateUser(string username, string password)
        {
            int output;
            string query = "SELECT RegId, UserName, Password, UserGroup FROM LoginInfo WHERE UserName = '" + username + "' AND Password = '" + password + "';";

            using (SQLiteConnection conLogin = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdUserPass = new SQLiteCommand(query, conLogin);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdUserPass;
                output = MyAdapter.Fill(dTable);
            }
            if (output == 1 || NoPassword == true) return "Right";
            else return "Right"; // "Wrong";
        }
        #endregion // ***********************************************************************************************

        private void FmOfferingInput_Activated(object sender, EventArgs e)
        {
            ReadUserGroup();
            if (UserGroup == fmLogin.Admingroup || UserGroup == fmLogin.Rootgroup) tssbtViewRecord.Enabled = true;
            else tssbtViewRecord.Enabled = false;
        }
        bool valRetry; string Answer;// QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ
        public static DialogResult dialogDescript(ref string Answer, ref bool valRetry)
        {
            Form form = new Form();
            Label lbInfo = new Label();
            TextBox txEmailTo = new TextBox();
            Button buttonYes = new Button();
            Button buttonNo = new Button();

            form.Text = " Special Service Description";
            lbInfo.Text = "Type in the description for the special service.";
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

        bool DoRetry; int AnsMonth;// QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ
        public static DialogResult dialogReportDate(ref int AnsMonth, ref bool DoRetry)
        {
            Form form = new Form();
            Label lbInfo = new Label();
            ComboBox cbReport = new ComboBox();
            Button buttonOK = new Button();
            Button buttonCancel = new Button();

            form.Text = " Monthly Report";
            lbInfo.Text = "Change to previous months:";
            cbReport.Items.Add("0"); cbReport.Items.Add("1");
            cbReport.Items.Add("2"); cbReport.Items.Add("3");
            cbReport.SelectedIndex = 0;

            buttonOK.Text = "OK"; buttonOK.DialogResult = DialogResult.OK;
            buttonCancel.Text = "Cancel"; buttonCancel.DialogResult = DialogResult.Cancel;
            lbInfo.SetBounds(5, 10, 140, 20);   cbReport.SetBounds(145, 10, 30, 20);
            buttonOK.SetBounds(5, 45, 80, 23); buttonCancel.SetBounds(95, 45, 80, 23);
            buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(180, 80);
            form.Controls.AddRange(new Control[] {lbInfo, cbReport, buttonOK, buttonCancel});
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;       form.MaximizeBox = false;
            form.AcceptButton = buttonOK;   form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            if (cbReport.Text == "0" || cbReport.Text == "1" || cbReport.Text == "2" || cbReport.Text == "3")
            {
                if (form.DialogResult == DialogResult.OK) AnsMonth = int.Parse(cbReport.Text);
                DoRetry = false;
                return dialogResult;
            }
            else
            {
                DoRetry = true;
                if (dialogResult == DialogResult.Yes) MessageBox.Show("You have to select one of the values provided.");
                return dialogResult;
            }
        }

        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbService.SelectedIndex == cbService.Items.Count - 1)
            {
                valRetry = true;
                while (valRetry == true)
                {
                    if (dialogDescript(ref Answer, ref valRetry) == DialogResult.Yes) Service = Answer; 
                    else { valRetry = false; cbService.SelectedIndex = 0; }
                }
            }
            else Service = cbService.Text;
        }
        private void cbService_Leave(object sender, EventArgs e)
        {
            cbService.Text = Service;
        }
        private void mnprintData_Click(object sender, EventArgs e)
        {
            string DocName = "Offering & Attendance-" + DateTime.Now.ToString("ddMMyy");
            string AdHeader = fmLogin.strChurchAcronym + "-" + DocName;
            PreviewXDialog.DGVPdfPrint(dgvOffering, AdHeader, DocName, true);
            PreviewXDialog.PrintDGVX(dgvOffering, AdHeader, DocName, true);
        }            
        //............ End 
    }
}
