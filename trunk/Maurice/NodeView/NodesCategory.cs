using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{
    public class NodesCategory : NodesBase
    {
        public NodesCategory()
        {
            //CommunConfig.getInstance().fileCat;
            FilenameXml = CommunConfig.getInstance().fileCat;
            Text = "Category";
            doc = new XmlDocument();            
            try
            {
             doc.Load(FilenameXml);          
            }
            catch(Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
            root = doc.ChildNodes[0];
             if (root == null)
            {
                doc.AppendChild(doc.CreateElement("categories"));
            }
            else
            {             
               foreach(XmlNode n in root.ChildNodes)
               {
                if (n.Name == "category")
                {
                    this.Nodes.Add(new NodeCategory(n));
                }
               }
             }
        }

        public string Directory
        {           
            get { if (IsAttributExist(ref root, "directory")) return root.Attributes["directory"].Value; else return ""; }
        }

        public void AddCategory(string name,string path)
        {
            XmlNode root = doc.ChildNodes[0];
            XmlNode nN = doc.CreateElement("category");
            AddAttribut(ref nN, "name", name);
            AddAttribut(ref nN, "directory",path);                
            Nodes.Add(new NodeCategory(nN));
            root.AppendChild(nN);
            //root.AppendChild(new NodeCategory(nN));    
           
        }
    }
}
