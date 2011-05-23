using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{

    public class NodePhotographer : NodesBase
    {
        protected XmlNode elm;
        protected int indexPhotographe;
        public NodePhotographer(XmlNode node)
        {
            Text = node.Attributes["name"].Value;
            try
            {
                indexPhotographe = Int32.Parse(node.Attributes["index"].Value);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
            elm = node;
        }

        public int Index
        {
            get { if (IsAttributExist(ref elm, "index")) return Int32.Parse(elm.Attributes["index"].Value); else return -1; }
        }

        public XmlNode Elm
        {
            get { return elm; }
        }

    }
}
