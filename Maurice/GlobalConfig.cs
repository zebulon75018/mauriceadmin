using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public class GlobalConfig
    {
        static GlobalConfig _instance = null;
        XmlDocument doc = null;
        public GlobalConfig()
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(@"config.xml");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading config.xml " + e.Message);
            }
        }

        public string communConfig
        {
            get
            {
                try
                {
                    XmlAttribute attr = doc.LastChild.Attributes["communConfig"];

                    if (attr != null)
                    {
                        return attr.Value;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
        }

        static public GlobalConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new GlobalConfig();
            }

            return _instance;
        }
    }
}
