using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SosiDB
{
    public partial class ComboBox_Ad : ComboBox
    {
        public ComboBox_Ad()
        {
            InitializeComponent();
        }
        private void ComboBox_Ad_Layout(object sender, LayoutEventArgs e)
        {
            if (initText != "No") this.ForeColor = SystemColors.ScrollBar;
            else this.ForeColor = SystemColors.WindowText;
        }
        protected override void OnDropDown(EventArgs e)
        {
            if (initText != "No")
            {
                this.ForeColor = SystemColors.WindowText;
            }
        }
        protected override void OnDropDownClosed(EventArgs e)
        {
            if (initText != "No")
            {
              if (this.SelectedIndex == -1) { this.ForeColor = SystemColors.ScrollBar;}
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {// comboBox1 is readonly
            //e.SuppressKeyPress = true;
            //e.Handled = true;
            e.KeyChar = (char)Keys.None; 
        }

        private string initText = "No";
        [TypeConverter(typeof(InitText))]
        public string Init_Text
        {
            get { return initText; }
            set { initText = value; }
        }
    }
}

