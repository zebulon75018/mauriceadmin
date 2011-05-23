using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Manina.Windows.Forms.NodeView
{
    class NodeInfo : NodeMultiLangue
    {              
        public NodeInfo(XmlNode node)
        {
            elm = node;
            Text = node.Attributes["name"].Value;            
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "category")
                {
                    this.Nodes.Add(new NodeCategory(n));
                }
            }
        }       

    }
}
