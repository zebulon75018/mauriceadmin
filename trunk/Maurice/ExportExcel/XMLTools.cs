using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Manina.Windows.Forms.ExportExcel
{
    static public class XMLTools
    {
        static public String GetAttributeStringValue(XmlNode node, String name)
        {
            String ret = String.Empty;

            try
            {
                ret = node.Attributes[name].Value;
            }
            catch (Exception)
            {
                ret = String.Empty;
            }

            return ret;
        }

        static public Double GetAttributePriceValue(XmlNode node, String name)
        {
            String val = GetAttributeStringValue(node, name);
            Double returnValue = 0.00;

            if (!String.IsNullOrEmpty(val))
            {
                Double.TryParse(val, out returnValue);

                returnValue = returnValue / 100.0;
            }

            return returnValue;
        }

        static public Int32 GetAttributeIntValue(XmlNode node, String name)
        {
            String val = GetAttributeStringValue(node, name);
            Int32 returnValue = 0;

            if (!String.IsNullOrEmpty(val))
            {
                Int32.TryParse(val, out returnValue);
            }

            return returnValue;
        }
    }
}
