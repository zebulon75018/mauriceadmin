using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Manina.Windows.Forms.ExportExcel;

namespace Manina.Windows.Forms
{
    class ConfigManager
    {
        static private ConfigManager singleton = null;

        static int numberOfFacture=0;

        private XmlDocument doc = null;

        public ConfigManager()
        {
            doc = new XmlDocument();
            doc.Load("configuration.txt");
        }

        public Int32 PrixFichierNumerique()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixFichierCD");

            if (prix == 0)
                return 420;
            else
                return prix;
        }

        public double PriceCD()
        {
            return double.Parse(doc.FirstChild.Attributes["CD"].Value);
        }
        public double PriceCDDollar()
        {
            return double.Parse(doc.FirstChild.Attributes["CD"].Value)* Double.Parse(doc.FirstChild.Attributes["euroDollar"].Value);
        }
        public double Price(string format)
        {            
            format = "F"+format;
            return double.Parse(doc.FirstChild.Attributes[format].Value);
        } 

        public double PriceDollar(string format)
        {
            format = "F" + format;
            return Double.Parse(doc.FirstChild.Attributes[format].Value) * Double.Parse(doc.FirstChild.Attributes["euroDollar"].Value);        
        }

        public string getExcelFile()
        {
            return doc.FirstChild.Attributes["excel"].Value;
        }


        public string getExcelTemplateFile()
        {            
            return doc.FirstChild.Attributes["exceltemplate"].Value;
        }

        public static ConfigManager getSingleton()
        {
            if (singleton == null) singleton = new ConfigManager();
            return singleton;
        }
        
    
    }
}
