using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NetWork
{
    public partial class LicenceQuery : Form
    {
        public LicenceQuery(String mac)
        {
            InitializeComponent();
            label1.Text = "Please enter the licence key for ";
            textBox2.Text = mac;
        }

        public string LicenceText
        {
            get { return this.textBox1.Text; }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
