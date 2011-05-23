using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public class CommunConfig
    {
        static CommunConfig _instance = null;
        XmlDocument doc = null;
        public CommunConfig(string filename)
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message);
            }
        }

        public string fileCat
        {
            get
            {
                return doc.LastChild.Attributes["categoryfile"].Value;
            }
        }

       public string fileInfo
        {
            get
            {
                return doc.LastChild.Attributes["infofile"].Value;
            }
        }

       public string userDirectory
        {
            get
            {
                return doc.LastChild.Attributes["userdirectory"].Value;
            }
        }

       public string photographefile
       {
           get
           {
               return doc.LastChild.Attributes["photographefile"].Value;
           }
       }

       public string guiconfigfile
       {
           get
           {
               return doc.LastChild.Attributes["guiconfigfile"].Value;
           }
       }

       public string imageMagicPath
       {
           get
           {
               return doc.LastChild.Attributes["imageMagicPath"].Value;
           }
       }

       public string productFile
       {
           get
           {
               return doc.LastChild.Attributes["productfile"].Value;
           }
       }



        static public CommunConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new CommunConfig(GlobalConfig.getInstance().communConfig);
            }

            return _instance;
        }
    }
}
