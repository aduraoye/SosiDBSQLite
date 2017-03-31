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
    public partial class FmUsers : Form
    {
        FmLogin fmLogin;
        string strId0, strId, strPassword, UserGroup;
        public FmUsers()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            fmLogin.niBackground.Visible = false;
        }

        private void FmUsers_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Login User] -" + fmLogin.strChurchAcronym;
        }
        public void CreateLoginUser()
        {
            try
            {
                FileStream fs = new FileStream(fmLogin.TempFile, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                strId0 = sr.ReadLine();
                lbTitle.Text = sr.ReadLine();
                txFName.Text = sr.ReadLine();
                txLName.Text = sr.ReadLine();
                txEmail.Text = sr.ReadLine();
                fs.Close(); sr.Close();
                this.Text = fmLogin.AppName + "[Create User]";
                btCreate.Text = "Create";
                SelectLogin();
                this.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }        
        public void ViewUser()
        {
            this.Text = fmLogin.AppName + "[View User]";
            btCreate.Text = "Edit";       
            txUsername.Enabled = false; txPassword.Enabled = false; txPWConfirmation.Enabled = false;

            ReadUserGroup();
            if (UserGroup == fmLogin.Rootgroup) cbUserGroup.Enabled = true; else cbUserGroup.Enabled = false;
            SelectLogin();
            this.ShowDialog();
        }
        private void ReadUserGroup()
        {
            FileStream fs = new FileStream(fmLogin.UserGroupFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            UserGroup = sr.ReadLine();
           // RegId = sr.ReadLine();
            fs.Close(); sr.Close();
        }
        private void btCreate_Click(object sender, EventArgs e)
        {
            if (btCreate.Text == "Create") PreCreateUser();
            else if (btCreate.Text == "Edit") SelectUser();
            else UpdateUser(); //if(btCreate.Text = "Update")
        }         
        private bool UserExist()
        {   // Select 
            string strCount = "SELECT RegId FROM LoginInfo WHERE RegId = '" + strId0 + "';";
            int intCount;
            using (var conPix = new SQLiteConnection(fmLogin.strConReg))
            {
                conPix.Open();
                using (var cmd = new SQLiteCommand(strCount, conPix))
                {
                    intCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            if (intCount != 0) return true;
            else return false;
        }
        private void SelectLogin()
        {   // Select 
            string queryx = "SELECT Id, Username, Title, First_Name, Last_Name, Email, Usergroup, RegId, Date FROM LoginInfo;";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdRegister = new SQLiteCommand(queryx, conTblx);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdRegister;
                MyAdapter.Fill(dTable);
                dgvUserLogin.DataSource = dTable;
            }
            dgvUserLogin.Columns[0].Visible = false;
            dgvUserLogin.Columns[1].Width = 100;
            dgvUserLogin.Columns[2].Width = 50;
            dgvUserLogin.Columns[3].Width = 100;
            dgvUserLogin.Columns[4].Width = 100;
            dgvUserLogin.Columns[5].Width = 100;
        }
        private void PreCreateUser()
        {
            if (cbUserGroup.Text == "Usergroup") {MessageBox.Show("You have not selected a group for the user!", "Error!",MessageBoxButtons.OK, MessageBoxIcon.Error); return;}
            if (txPassword.Text != txPWConfirmation.Text) { MessageBox.Show("The Passwords do not match!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            else
            {
                if (UserExist() == true) { MessageBox.Show("An account is already created for this user!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                else
                {
                    CreateUser();
                    SelectLogin();

                    using (FileStream fsopx = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write))
                    {
                        StreamWriter swopx = new StreamWriter(fsopx);
                        swopx.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - RegId(" + strId0 + ") UserLogin record created.");
                        swopx.Flush(); swopx.Close();
                    }


                    using (FileStream fsop = new FileStream(fmLogin.TempFile, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter swop = new StreamWriter(fsop);
                        swop.WriteLine(lbTitle.Text);//Title
                        swop.WriteLine(txFName.Text);//First name
                        swop.WriteLine(txLName.Text);// Last Name
                        swop.WriteLine(txUsername.Text);//UserName
                        swop.WriteLine(txEmail.Text);//Email
                        swop.Flush(); swop.Close();
                    }
                    FmEmailPad fmEmailPad = new FmEmailPad();
                    fmEmailPad.EmailLoginUser();

                    MessageBox.Show("User created successfully");
                    this.Close();
                }
            }
        }
        private void CreateUser()
        {  // Create or insert User
            string Today = DateTime.Today.ToString("yyyy-MM-dd");
            string strDollarCent = "RegId, Date, Title, First_Name, Last_Name, Username, Password, Email, UserGroup";
            string strtxDollarCent = strId0 + "', '" + Today + "', '" + lbTitle.Text + "', '" + txFName.Text + "', '" + txLName.Text
                                     + "', '" + txUsername.Text + "', '" + txPassword.Text + "', '" + txEmail.Text + "', '" + cbUserGroup.Text;
            string query = "INSERT INTO LoginInfo(" + strDollarCent + ") VALUES ('" + strtxDollarCent + "');";
            using (SQLiteConnection conTbly = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOffering = new SQLiteCommand(query, conTbly);
                conTbly.Open();
                cmdOffering.ExecuteNonQuery();
            }
        }
        private void SelectUser()
        {
            if (dgvUserLogin.CurrentRow == null) { MessageBox.Show("Create or select a person's record you want to get the RegID!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            else
            {
                int SelIndex = dgvUserLogin.CurrentRow.Index;
                strId = dgvUserLogin.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                DataTable dTable = new DataTable();
                string queryx = "SELECT Id, Username, Title, First_Name, Last_Name, Email, Usergroup, PassWord FROM LoginInfo WHERE Id = '" + strId + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                    MyAdapter.SelectCommand = cmdOfferingx;
                    MyAdapter.Fill(dTable);
                }

                txUsername.Text = dTable.Rows[0][1].ToString();
                lbTitle.Text = dTable.Rows[0][2].ToString();
                txFName.Text = dTable.Rows[0][3].ToString();
                txLName.Text = dTable.Rows[0][4].ToString();
                txEmail.Text = dTable.Rows[0][5].ToString();
                cbUserGroup.Text = dTable.Rows[0][6].ToString();
                strPassword = dTable.Rows[0][7].ToString();

                btCreate.Text = "Update";
                txUsername.Enabled = true; txPassword.Enabled = true; txPWConfirmation.Enabled = true;
            }
        }
        
        private void UpdateUser() 
        {
            string queryx = "UPDATE LoginInfo SET  Usergroup = '" + cbUserGroup.Text + "',  Username = '" + txUsername.Text + "',  PassWord = '" + txPassword.Text + "',  Email = '" + txEmail.Text + "' WHERE Id = '" + strId + "'";
            using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
            {
                SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                conTblx.Open();
                cmdOfferingx.ExecuteNonQuery();
            }
            btCreate.Text = "Edit";
            txUsername.Enabled = false; txPassword.Enabled = false; txPWConfirmation.Enabled = false;
            cbUserGroup.Text = "Usergroup"; txUsername.Text = "Username";
            lbTitle.Text = "Title"; txFName.Text = "First Name";
            txLName.Text = "Surname"; txEmail.Text = "E-mail Address"; 
            SelectLogin();

            FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - UserLogin record with RegId(" + strId + ") Username and/or Password changed.");
            swop.Flush(); swop.Close();
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvUserLogin.CurrentRow == null) return;
            else
            {
                int SelIndex = dgvUserLogin.CurrentRow.Index;
                string strId = dgvUserLogin.Rows[SelIndex].Cells[0].FormattedValue.ToString();
                string queryx = "DELETE FROM LoginInfo WHERE Id = '" + strId + "';";
                using (SQLiteConnection conTblx = new SQLiteConnection(fmLogin.strConReg))
                {
                    SQLiteCommand cmdOfferingx = new SQLiteCommand(queryx, conTblx);
                    conTblx.Open();
                    cmdOfferingx.ExecuteNonQuery();
                }
                dgvUserLogin.Rows.RemoveAt(SelIndex);
                dgvUserLogin.Refresh();

                FileStream fsop = new FileStream(fmLogin.EditFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - UserLogin record with RegId(" + strId + ") deleted.");
                swop.Flush(); swop.Close();
            }
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        // end
    }
}
