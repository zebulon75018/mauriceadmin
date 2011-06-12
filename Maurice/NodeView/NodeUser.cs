using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Manina.Windows.Forms.ExportExcel;

namespace Manina.Windows.Forms.NodeView
{
    public class NodeUser : NodesBase
    {
        private XmlNode photoList;

        public string chamberNumber="";       
        public NodeUser(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            FilenameXml = filename;

            Text = fi.Name.Replace(".xml", "");
            chamberNumber = Text;

            doc = new XmlDocument();
            try
            {
              doc.Load(filename);
                root = doc.FirstChild;

            } catch(Exception e)
            {
                MessageBox.Show("Error " + e.Message);                 
            }
        }

        public void refresh()
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(FilenameXml);
                root = doc.FirstChild;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message);
            }
        }

        public string getDirectory()
        {            
            return  DirUtil.JoinDirAndFile(NodesUser.path,chamberNumber);
        }

        public string getBackupDirectory()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(DirUtil.JoinDirAndFile(NodesUser.path, "Sauvegarde"));
            if (dirInfo.Exists == false)
            {
                Directory.CreateDirectory(DirUtil.JoinDirAndFile(NodesUser.path, "Sauvegarde"));
            }
            return DirUtil.JoinDirAndFile(dirInfo.FullName, chamberNumber + "__" + (String)LeavingDate);
        }


        public string Password
        {
            get { return root.Attributes["pass"].Value; }
            set {

                refresh();
                UpdateAttribut(ref root, "pass", value);
                internalSave();
            }
        }

        public DateTime LeavingDateTime        
        {
            get
            {
                string date = LeavingDate;
                char[] sep = new char[1];
                sep[0]='-';
                string [] strdate = date.Split(sep);
                DateTime result     = new DateTime(Int32.Parse(strdate[2]), Int32.Parse(strdate[1]), Int32.Parse(strdate[0]));
                return result;
            }
        }

        public string LeavingDate
        {
            get { return root.Attributes["date"].Value; }
        }

        public bool HasPayed
        {
            get {
                return getBooleanAttribut(root, "haspayed");
            }
            set
            {
                refresh();
                setBooleanAttribut(ref root, "haspayed", value);
                internalSave();
            }
        }        

        public bool FactureEdit
        {
            get
            {
                return getBooleanAttribut(root, "factureEdit");
            }
            set
            {
                refresh();
                setBooleanAttribut(ref root, "factureEdit", value);
                internalSave();
            }
        }

        public XmlNode GetPhoto(int n)
        {            
            XmlNode node = null;

            int i = 0;
            foreach (XmlNode elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    if (i == n)
                    {
                        node = elm;
                        break;
                    }
                    i++;
                }
            }

            return node;
        }

        public bool isPhotoOnCD(int n)
        {            
         XmlNode img = GetPhoto(n);
         String strImageOnCD = XMLTools.GetAttributeStringValue(img, "imageoncd");

         if (strImageOnCD.ToLower() == "true")
         {
             return true;
         }
         else
         {
             return false;
         }
        }

        public bool isPhotoOnBook(int n)
        {
            XmlNode img = GetPhoto(n);
            String strImageOnBook = XMLTools.GetAttributeStringValue(img, "imageonbook");

            if (strImageOnBook.ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int NbPhoto()
        {
            int result = 0;

            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img") result++;
            }
            return result;            
        }

        public string FormatPhoto(int n)
        {
            int i = 0;
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    try
                    {
                        if (i == n) return elm.Attributes["format"].Value.ToString();
                    }
                    catch (Exception)
                    {
                        return String.Empty;
                    }

                    
                    i++;
                }
            }
            return "";
        }

        public string FilenamePhoto(int n)
        {
            int i = 0;
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    if (i == n)
                    {
                        FileInfo fi  = new FileInfo(elm.Attributes["path"].Value.ToString());
                        return fi.Name;
                    }
                    i++;
                }
            }
            return "";
        }

        public string FullFilenamePhoto(int n)
        {
            int i = 0;
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    if (i == n)
                    {
                        FileInfo fi = new FileInfo(elm.Attributes["path"].Value.ToString());
                        return fi.FullName;
                    }
                    i++;
                }
            }
            return "";
        }

        public string FilenameWithoutPathPhoto(int n)
        {
            int i = 0;
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    if (i == n)
                    {
                        FileInfo fi = new FileInfo(elm.Attributes["path"].Value.ToString());
                        return fi.Name;
                    }
                    i++;
                }
            }
            return "";
        }

        public XmlNode getXmlImage(string filename)
        {
            foreach (XmlNode elm in root.ChildNodes)
            {
                if (elm.Name == "img" && elm.Attributes["path"].Value.ToString() == filename)
                {
                    return elm;
                }
            }
            return null;
        }

        public string getFormatImage(string filename)
        {            
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {
                    
                       if (elm.Attributes["path"].Value.ToString() == filename)
                       {
                           try
                           {
                               return elm.Attributes["format"].Value.ToString();
                           }
                           catch (Exception e)
                           {
                               return "";
                           }

                       }                    
                }
            }
            return "";
        }

        public void DeleteImage(string filename)
        {
            refresh();
            XmlElement findImage = null;
            foreach (XmlElement elm in root.ChildNodes)
            {
                if (elm.Name == "img")
                {

                    if (elm.Attributes["path"].Value.ToString() == filename)
                    {
                        findImage = elm;                        
                    }
                }
            }
            root.RemoveChild(findImage);
            internalSave();
        }


        public bool  orderCD
        {
            get {
                return getBooleanAttribut(root, "ordercd");
                }
            set
            {
               // setBooleanAttribut(ref root, "ordercd", value);
            }
        }

        public bool withoutprinting
        {
            get
            {
                return getBooleanAttribut(root, "withoutprinting");
            }
        }

        public bool orderBook
        {
            get
            {
                return getBooleanAttribut(root, "orderbook");
            }
            set
            {
                // setBooleanAttribut(ref root, "ordercd", value);
            }
        }

        public int getNbPageOnBook
        {
            get
            {
                return getIntAttribut(root, "nbpageonbook");
            }
            set
            {
                // setBooleanAttribut(ref root, "ordercd", value);
            }
        }

        public String name
        {
            get
            {
                try
                {
                    return root.Attributes["name"].Value;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            set
            {
              //  UpdateAttribut(ref root, "name", value);
            }
        }
        public String firstname
        {
            get
            {
                try
                {
                    return root.Attributes["firstname"].Value;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            set
            {
                //UpdateAttribut(ref root, "firstname", value);
            }
        }

        public String directoryBackup()
        {
            return Text + "__" + (String)LeavingDate;
        }

        public void internalSave()
        {
            if (File.Exists(FilenameXml) == false)
            {
                MessageBox.Show("Warning " + FilenameXml + " doesn't exists ");
                return;
            }               
            doc.Save(FilenameXml);
        }

        public void Save()
        {
            /// On ne sauvegarde pas par soucis de conflit avec le client
        }
    }
}
