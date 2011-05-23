using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class FCategoryDialog : Form
    {
        public Category category = null;
        public FCategoryDialog()
        {
            InitializeComponent();
        }

        public FCategoryDialog(ref Category c)
        {
            InitializeComponent();
            category = c;
            setCategoryToGUI();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void FCategoryDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void setCategoryToGUI()
        {
            textBox1.Text = category.frenchName;
            textBox2.Text = category.germanName;
            textBox2.Text = category.ItalianName;
        }

        public void getCategoryFromGUI()
        {
        }
    }
}
