using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class PhotoPromotionEntry : UserControl
    {
        private XmlNode _node;
        public PhotoPromotionEntry()
        {
            InitializeComponent();
        }

        public void setXmlNode(XmlNode node)
        {
            _node = node;
            if (_node == null) return;
            try
            {
                XmlNode attribNode = _node.Attributes.GetNamedItem("nbphoto");
                if (attribNode != null)
                {
                    numericUpDownNbPhoto.Value = int.Parse(_node.Attributes["nbphoto"].Value);
                }
                attribNode = _node.Attributes.GetNamedItem("promotion");
                if (attribNode != null)
                {
                    numericUpDownPourcentage.Value = int.Parse(_node.Attributes["promotion"].Value);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in promotion " + e.Message);
            }
        }
         

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownNbPhoto_ValueChanged(object sender, EventArgs e)
        {
            XmlNode attribNode = _node.Attributes.GetNamedItem("nbphoto");
                if (attribNode != null)
                {
                    attribNode.Value = numericUpDownNbPhoto.Value.ToString();
                }
                else
                {
                    MessageBox.Show("Error Promotion Node not content nbphoto attribut ");
                }            
        }

        private void numericUpDownPourcentage_ValueChanged(object sender, EventArgs e)
        {
            XmlNode attribNode = _node.Attributes.GetNamedItem("promotion");
            if (attribNode != null)
            {
                attribNode.Value = numericUpDownPourcentage.Value.ToString();
            }
            else
            {
                MessageBox.Show("Error Promotion Node not content promotion attribut ");
            }            
        }
    }
}
