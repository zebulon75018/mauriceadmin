using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Manina.Windows.Forms.NodeView;

namespace Manina.Windows.Forms
{
    public partial class FFindCustomerByDate : Form
    {
        public List<String> users;
        public enum typeSearchCustomer {
            TOMOROW,
            TODAY,
            PAST
        } 
        public FFindCustomerByDate(NodesUser u,typeSearchCustomer search)
        {
            InitializeComponent();
            users = new List<string>();
            switch (search)
            {
                case typeSearchCustomer.TOMOROW:
                    label1.Text = "Customer(s) Leaving Tomorow";
                    users = u.TomorowCustomerLeave();
                    break;
                case typeSearchCustomer.TODAY:
                    label1.Text = "Customer(s) Leaving Today";
                    users = u.TomorowCustomerToday();
                    break;
                case typeSearchCustomer.PAST:
                    label1.Text = "Customer(s) Left";
                    users = u.TomorowCustomerLeft();
                    break;
            }

            foreach(string s in users)
            {
                listBox1.Items.Add(s);
            }             
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public string Selected {
            get
            {
                if (listBox1.SelectedIndex == -1) return "";
                else
                {
                    char[] sep = new char[1];
                    sep[0] = ':';
                    string[] numberChamber = listBox1.SelectedItem.ToString().Split(sep);
                    if (numberChamber.Length > 1)
                    {
                        return numberChamber[0];
                    }
                    return "";
                }
            }
        }
    }
}
