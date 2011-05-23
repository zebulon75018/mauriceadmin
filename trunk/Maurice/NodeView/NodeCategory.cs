using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml;

namespace Manina.Windows.Forms.NodeView
{
    public class NodeCategory : NodeMultiLangue
    {         
        public NodeCategory(XmlNode node)
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

        public NodeCategory(NodeCategory node)
        {
            elm = node.elm;
            Text = elm.Attributes["name"].Value;         
        }

        public bool IsLeaf()
        {
            if (elm.ChildNodes.Count == 0)
            {
                return true;
            }
            return false;
        }
      
        public void AddCategory(string name,string path)
        {            
            XmlNode  nN = elm.OwnerDocument.CreateElement("category");
            AddAttribut(ref nN, "name", name);
            AddAttribut(ref nN, "directory", path);                                   
            Nodes.Add(new NodeCategory(nN));
            elm.AppendChild(nN);          

        }        
    }
}
