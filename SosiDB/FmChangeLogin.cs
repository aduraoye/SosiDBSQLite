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

namespace SosiDB
{
    public partial class FmChangeLogin : Form
    {
        FmLogin fmLogin; 
        public string strConx, strConReg;
        public string strChurchName, strChurchAcronym, password;
        public string RegId, UserGroup;

        public FmChangeLogin()
        {
            InitializeComponent();
            fmLogin = new FmLogin();
            txPassword.PasswordChar = '•';   // Personal initialization 
            FileStream fs = new FileStream(fmLogin.EtoFile, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            strChurchName = sr.ReadLine();
            strChurchAcronym = sr.ReadLine();
            password = sr.ReadLine();
            fs.Close(); sr.Close();
            fmLogin.niBackground.Visible = false;
            strConReg = fmLogin.strConReg;
        }
        private void FmLogin_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Change User] -" + fmLogin.strChurchAcronym;
            txUsername.Text = ""; txPassword.Text = ""; 
            btLogin.Text = "Login";
            dgvEdit.SendToBack();
        }
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (ValidateLogin(txUsername.Text, txPassword.Text) != "Wrong") 
            { 
                label5.Text = "      Correct! Click ethier button to continue..."; 
                this.Close(); 
            }
            else label5.Text = "Error! Username or password is incorrect. Try again.";
        }
        private void writeFile()
        {
            FileStream fsop = new FileStream(fmLogin.UserGroupFile, FileMode.Create, FileAccess.Write);
            StreamWriter swop = new StreamWriter(fsop);
            swop.WriteLine(UserGroup); ;
            swop.WriteLine(RegId);
            swop.Flush(); swop.Close();
        }
        public string ValidateLogin(string username, string password)
        {
            int output;
            string query = "SELECT RegId, UserName, Password, UserGroup FROM LoginInfo WHERE UserName = '" + username + "' AND Password = '" + password + "';";

            using (SQLiteConnection conLogin = new SQLiteConnection(strConReg))
            {
                SQLiteCommand cmdUserPass = new SQLiteCommand(query, conLogin);
                SQLiteDataAdapter MyAdapter = new SQLiteDataAdapter();
                DataTable dTable = new DataTable();
                MyAdapter.SelectCommand = cmdUserPass;
                output = MyAdapter.Fill(dTable);
                dgvEdit.DataSource = dTable;
            }

            if (output == 1)
            {
                RegId = dgvEdit.Rows[0].Cells[0].FormattedValue.ToString();
                UserGroup = dgvEdit.Rows[0].Cells[3].FormattedValue.ToString();
                writeFile();
                FileStream fsop = new FileStream(fmLogin.EtoFile, FileMode.Append, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                swop.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Login by User RegId(" + RegId + ")");
                swop.Flush(); swop.Close();
                return RegId;
            }
            else return "Wrong";  
        }
        //// End of methods 
    }
}
