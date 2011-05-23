using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Manina.Windows.Forms.NodeView;

namespace Manina.Windows.Forms.wizard
{
    public partial class FWizardImport : Form
    {
        public bool hasError = false;
        public FWizardImport(NodesPhotographer p,NodesCategory cat)
        {
            InitializeComponent();

            foreach (TreeNode s in p.Nodes)
            {
                listBox1.Items.Add(s.Text);
            }
            foreach (NodeCategory n in cat.Nodes)
            {
                AddCategory(n,"");    
            }
            this.wizardControl1.NextButtonEnabled = true;        
        }

        public String[] ListImage
        {
            get
            {                                
                ImageListView.ImageListViewSelectedItemCollection selected = imageListView1.SelectedItems;

                String [] file = new string[selected.Count];

                int n = 0;
                foreach (ImageListViewItem f in selected)
                {
                    file[n] = f.FileName;
                    n++;
                }                          
                return file;
            }
        }

        public String[] DstImage
        {
            get
            {
                String photog = listBox1.SelectedItem.ToString();
                FilenamePhotoProvider fpp = new FilenamePhotoProvider(photog);

                ImageListView.ImageListViewSelectedItemCollection selected = imageListView1.SelectedItems;
                
                List<string> Lfile = fpp.getPossibleFilename(DirectoryDst,selected.Count);

                string[] file = new String[Lfile.Count];

                int n=0;
                foreach (String s in Lfile)
                {
                    file[n] = s;
                    n++;
                }
                /*
                ImageListView.ImageListViewSelectedItemCollection selected = imageListView1.SelectedItems;

                String[] file = new string[selected.Count];

                int n = 0;
                foreach (ImageListViewItem f in selected)
                {
                    FileInfo fi = new FileInfo(f.FileName);

                    file[n] = DirectoryDst + "\\" + fi.Name; 
                    n++;
                }
                 * */
                return file;
            }
        }

        public string DirectoryDst
        {
            get
            {
                if (treeView1.SelectedNode ==null) return "";

                NodeCategory selected = (NodeCategory) treeView1.SelectedNode ;
                return selected.getDirectory();
            }
        }

        public NodeCategory getSelectedCategory()
        {
            if (treeView1.SelectedNode == null) return null;
            return (NodeCategory)treeView1.SelectedNode;
        }
        public void AddCategory(NodeCategory node,string parentName)
        {
            NodeCategory n = new NodeCategory(node);
            if (node.IsLeaf())
            {
                n.Text = parentName + " \\ " + n.Text;
                treeView1.Nodes.Add(n);
            }            
            foreach (NodeCategory nchild in node.Nodes)
            {
                AddCategory(nchild,parentName + " \\ "+ node.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                labelPath.Text = folderBrowserDialog1.SelectedPath;
                DirectoryInfo path = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
            imageListView1.Items.Clear();
            imageListView1.SuspendLayout();
            foreach (FileInfo p in path.GetFiles("*.*"))
            {
                if (p.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||                    
                    p.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    imageListView1.Items.Add(p.FullName);
            }
            imageListView1.ResumeLayout();        
            }
        }

        private void wizardControl1_NextButtonClick(object sender)
        {            
        }

        private void wizardControl1_FinishButtonClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void wizardControl1_CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void wizardControl1_CurrentStepIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.wizardControl1.NextButtonEnabled = true;   
        }

        private void wizardControl1_NextButtonClick(object sender,EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && treeView1.SelectedNode != null && imageListView1.SelectedItems.Count != 0)
            {
                hasError = false;
            }
            if (listBox1.SelectedIndex == -1)
            {
                checkBoxPhotographe.Text = "Error No photographe have been selected";
                checkBoxPhotographe.Checked = false;
                checkBoxPhotographe.ForeColor = Color.Red;
                hasError = true;
            }
            else
            {
                checkBoxPhotographe.Checked = true;
                checkBoxPhotographe.Text = "OK photographe " + listBox1.SelectedItem.ToString() + " have been selected";
                checkBoxPhotographe.ForeColor = Color.Black;
            }
            if (treeView1.SelectedNode == null)
            {
                checkBoxCategory.Text = "Error Category have been selected";
                checkBoxCategory.Checked = false;
                checkBoxCategory.ForeColor = Color.Red;
                hasError = true;
            }
            else
            {
                checkBoxCategory.Text = "OK Category " + treeView1.SelectedNode.Text + " have been selected";
                checkBoxCategory.Checked = true;
                checkBoxCategory.ForeColor = Color.Black;
            }
            if (imageListView1.SelectedItems.Count == 0)
            {
                checkBoxImages.Text = "Error No image have been selected";
                checkBoxImages.Checked = false;
                checkBoxImages.ForeColor = Color.Red;
                hasError = true;
            }
            else
            {
                checkBoxImages.Text = "OK " + labelPath.Text + " Images selected ";
                checkBoxImages.Checked = true;
                checkBoxImages.ForeColor = Color.Black;
            }

        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            imageListView1.SelectAll();
        }      
    }
}
