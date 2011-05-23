using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{
   public class NodesInfo : NodesBase
    {
        public NodesInfo()
        {
            //CommunConfig.getInstance().fileInfo;
            FilenameXml = CommunConfig.getInstance().fileInfo; ;
            Text = "Informations";
            doc = new XmlDocument();
            try
            {
             doc.Load(FilenameXml);          
            }
            catch(Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
            XmlNode root = doc.ChildNodes[0];
             if (root == null)
            {
                doc.AppendChild(doc.CreateElement("informations"));
            }
            else
            {             
             foreach(XmlNode n in root.ChildNodes)
             {
                if (n.Name == "category")
                {
                    this.Nodes.Add(new NodeInfo(n));
                }
             }
            }
        }

        public void AddInfo(string name)
        {
            XmlNode root = doc.ChildNodes[0];
            if (root == null) return;
            XmlNode nN = doc.CreateElement("category");
            AddAttribut(ref nN, "name", name);                
            Nodes.Add(new NodeInfo(nN));
            root.AppendChild(nN);
            //root.AppendChild(new NodeCategory(nN));    
           
        }   
    }
}
