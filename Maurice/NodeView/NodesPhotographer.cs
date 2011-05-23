using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{
    public class NodesPhotographer : NodesBase
    {
        public NodesPhotographer()
        {
            FilenameXml = CommunConfig.getInstance().photographefile;
            Text = "Photographe";
            doc = new XmlDocument();
            try
            {
                doc.Load(FilenameXml);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Loading Photographe File "+e.Message);
            }
            root = doc.ChildNodes[0];
            if (root == null)
            {
                doc.AppendChild(doc.CreateElement("photographers"));
            }
            else
            {
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "photographer")
                    {
                        this.Nodes.Add(new NodePhotographer(n));
                    }
                }
            }
        }

        public int getNextIndex()
        {
            int max =0;
            XmlNode root = doc.ChildNodes[0];
            if (root == null) return 0;
            foreach (XmlElement e in root.ChildNodes)
            {
                int value = Int32.Parse(e.Attributes["index"].Value);
                if (max < value) max = value;
            }
            return max +1;
        }

        public List<String> getPhotographer()
        {
            List<String> result = new List<string>();
            foreach (TreeNode n in this.Nodes)
            {
                result.Add(n.Text);
            }

            return result;
        }

        public void AddPhotographe(string n)
        {
            XmlNode root = doc.ChildNodes[0];
            if (root == null) return;
            XmlNode nN = doc.CreateElement("photographer");
            AddAttribut(ref nN, "name", n);
            AddAttribut(ref nN, "index", getNextIndex().ToString());            

            Nodes.Add(new NodeInfo(nN));
            root.AppendChild(nN);
        }

        public void DeleteElem(NodePhotographer c)
        {
            root = doc.ChildNodes[0];
            foreach (XmlNode elm in root.ChildNodes)
            {
                if (c.Elm == elm)
                {
                    root.RemoveChild(elm);                    
                }
            }
        }

    }
}
