using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Manina.Windows.Forms
{
    public partial class FAddChildCategory : Form
    {
        public FAddChildCategory()
        {
            InitializeComponent();
        }

        public string NameCategory
        {
            get { return  textBoxName.Text.Trim(); } 
        }
        public string NameDirectory
        {
            get { return textBoxDirectory.Text; }
        }

        public bool IsCreateDirectoryFromParent
        {
            get { return checkBoxCreatePath.Checked; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text.Trim() == "" || ( textBoxDirectory.Text.Trim() == "" && checkBoxCreatePath.Checked==false))
            {                
                MessageBox.Show("Entry(s) are Empty, please Fill ");
                return;
            }
            if (textBoxName.Text.Contains('\\') || textBoxName.Text.Contains('/'))
            {
                MessageBox.Show("Entry(s) can't contains \\ or / ");
                return;
            }
            if (textBoxName.Text[0] == ' ')
            {
                MessageBox.Show("Entry(s) can't begin by white space ");
                return;
            }

            if (Directory.Exists(textBoxDirectory.Text) == false && checkBoxCreatePath.Checked == false)
            {
                MessageBox.Show("Directory " + textBoxDirectory.Text + " doesn't exist please check");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonChooseDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDirectory.Text = folderBrowserDialog1.SelectedPath;                     
            }
        }

        private void checkBoxCreatePath_CheckedChanged(object sender, EventArgs e)
        {
            textBoxDirectory.Visible = !checkBoxCreatePath.Checked;
            buttonChooseDirectory.Visible = !checkBoxCreatePath.Checked;
            label2.Visible = !checkBoxCreatePath.Checked;
        }
    }
}
