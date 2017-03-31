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

namespace SosiDB
{
    public partial class FmSchedulerMonitor : Form
    {
        FmLogin fmLogin; 
        public FmSchedulerMonitor()
        {
            InitializeComponent();            
            fmLogin = new FmLogin();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = fmLogin.AppName + "[Scheduler Monitor] -" + fmLogin.strChurchAcronym;
            DirectoryInfo apple = new DirectoryInfo(fmLogin.PathSchedule);
            int i = 0, j = 0;
            foreach (var file in apple.GetFiles("*.sch")) { i++; }

            string[] Filename = new string[i];
            foreach (var file in apple.GetFiles("*.sch"))
            {  //do the thing
                Filename[j] = Path.GetFileNameWithoutExtension(file.ToString());
                j++;
            }
            Array.Sort(Filename);
            ltSchedule.Items.Clear(); ltSchedule.Items.AddRange(Filename);
            rtDeleted.Lines = File.ReadAllLines(fmLogin.PathResource + "Deletedfilenames.dbax");
        }
        // ******** End **********
    }
}
