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
    public partial class TextBox_Ad : TextBox 
    {
        private string strText;
        private bool blText;
        public TextBox_Ad()
        {
            InitializeComponent();
            blText = true;
        }
        protected override void OnClick(EventArgs e)
        {
            if (initText != "No")
            {
                if (blText == true) { strText = this.Text; blText = false; }
                base.OnClick(e);
                if (this.Text == strText) { this.Text = ""; this.ForeColor = SystemColors.WindowText; }
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            if (initText != "No")
            {
                base.OnLeave(e);
                if (this.Text == "") { this.Text = strText; this.ForeColor = SystemColors.ScrollBar; }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (valueType != "String") 
            {
                base.OnKeyPress(e);
                if (Char.IsDigit(e.KeyChar)) return;
                if (Char.IsControl(e.KeyChar)) return;
                if (valueType != "Phone(+)")
                {
                    if ((e.KeyChar == '.') && (this.Text.Contains('.') == false) && valueType != "Int") return;
                    if ((e.KeyChar == '-') && (this.Text.Contains('-') == false) )
                    {
                        this.Text = '-' + this.Text;
                        this.SelectionLength = 0;
                        this.Select(1, 0);
                    }
                    e.Handled = true;
                }
                else // Phone(+)
                {
                    if ((e.KeyChar == '+') && (this.Text.Contains('+') == false) )
                    {
                        this.Text = '+' + this.Text;
                        this.SelectionLength = 0;
                        this.Select(1, 0);
                    }
                    e.Handled = true;
                }
            
            }
        }

        private string initText = "No";
        [TypeConverter(typeof(InitText))]
        public string Init_Text
        {
            get { return initText; }
            set { initText = value; }
        }

        private string valueType = "String";
        [TypeConverter(typeof(ValueType))]
        public string Valu_Type
        {
            get { return valueType; }
            set { valueType = value; }
        }

        private void TextBox_Ad_Layout(object sender, LayoutEventArgs e)
        {
            if (initText != "No") this.ForeColor = SystemColors.ScrollBar;
            else this.ForeColor = SystemColors.WindowText;
        }
    }
}


//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
public class ValueType : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new string[] { "Int", "Float", "Douuble", "Phone(+)", "String" });
    }
}

public class InitText : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new string[] { "Yes", "No" });
    }
}
