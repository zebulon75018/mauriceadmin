using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Manina.Windows.Forms
{
    public partial class ProductControl : UserControl
    {
        private XmlNode _node;
        public ProductControl(XmlNode node)
        {
            InitializeComponent();
            _node = node;

            InitGui();
        }

        public void InitGui()
        {
            XmlNode attribNode = _node.Attributes.GetNamedItem("name");
            if (attribNode != null)
            {
                this.labelText.Text = _node.Attributes["name"].Value;
            }
             attribNode = _node.Attributes.GetNamedItem("price");
            if (attribNode != null)
            {
                this.textBoxPrice.Text = _node.Attributes["price"].Value;
            }            
        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            _node.Attributes["price"].Value = textBoxPrice.Text;
        }

    }
}
