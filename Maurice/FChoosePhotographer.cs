using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using  Manina.Windows.Forms.NodeView;

namespace Manina.Windows.Forms
{
    public partial class FChoosePhotographer : Form
    {
        public FChoosePhotographer(NodesPhotographer c)
        {
            InitializeComponent();
            foreach(String s in c.getPhotographer())
            {
                listBox1.Items.Add(s);
            }
        }

        public string Photographe
        {
            get
            {
                if (listBox1.SelectedIndex == -1)
                {
                    return "";
                }
                return listBox1.SelectedItem.ToString();
            }
        }



        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Error !! please choopse a photyographe in list ");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
