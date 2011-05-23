using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Manina.Windows.Forms.NodeView
{
    public class NodesBase : TreeNode
    {
        protected string FilenameXml = "";
        protected  XmlDocument doc = null;
        protected XmlNode root = null;
        protected  XmlNode elm; 
        public NodesBase()
        {
        }

        public XmlNode Root
        {
            get { return doc.FirstChild; }
        }

        public XmlNode Elm
        {
            get { return elm; }
        }

        public void AddAttribut(ref XmlNode node, string name, string Value)
        {
            XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
            attr.Value = Value;
            node.Attributes.Append(attr);
        }

        public void Save()
        {
            if (File.Exists(FilenameXml) == false)
            {
                MessageBox.Show("Warning " + FilenameXml + " doesn't exists ");
                return;
            }               
            doc.Save(FilenameXml);
        }

        public void SaveElem(XmlNode e)
        {
        }

        public void DeleteElem(NodesBase c)        
        {
            root = doc.FirstChild;
            foreach (XmlNode elm in root.ChildNodes)
            {
                if (c.Elm == elm)
                {
                    root.RemoveChild(elm);
                    //elm.RemoveChild(elm);                    
                }
                else
                {
                    if (DeleteChild(c.Elm, elm) == true) break;
                }
            }
        }

        private bool DeleteChild(XmlNode c,  XmlNode elm)
        {            
            foreach (XmlNode e in elm.ChildNodes)
            {
                if (e == c)
                {
                    elm.RemoveChild(c);
                    return true;
                }
                else
                {
                    if (DeleteChild(c, e) == true) return true;
                }
            }
            return false;
        }

        protected bool IsAttributExist(ref XmlNode n, string attr)        
        {
            if (n == null) return false;
            foreach (XmlAttribute a in n.Attributes)
            {
                if (a.Name == attr) return true;
            }
            return false;
        }

        protected void UpdateAttribut(ref XmlNode elm,string nameAttribut,string value)
        {
            if (IsAttributExist(ref elm, nameAttribut)) elm.Attributes[nameAttribut].Value = value;
            else
            {
                XmlAttribute a = elm.OwnerDocument.CreateAttribute(nameAttribut);
                a.Value = value;
                elm.Attributes.Append(a);
            }
        }

        protected bool getBooleanAttribut( XmlNode n, string attr)
        {
            if (IsAttributExist(ref n,attr) == false) return false;            
                if (n.Attributes[attr].Value.ToLower() == "true") return true;
                if (n.Attributes[attr].Value.ToLower() == "false") return false;
                return false;
        }

        protected void setBooleanAttribut(ref XmlNode n, string attr,bool value)
        {            
            if (value)
            {
                UpdateAttribut(ref n, attr, "true");                
            }
            else
            {
                UpdateAttribut(ref n, attr, "false");                
            }                
        }

    }
}
