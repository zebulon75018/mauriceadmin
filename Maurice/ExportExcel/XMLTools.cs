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

                returnValue = returnValue;
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

        static public bool IsAttributExist(ref XmlNode n, string attr)
        {
            if (n == null) return false;
            foreach (XmlAttribute a in n.Attributes)
            {
                if (a.Name == attr) return true;
            }
            return false;
        }

        static public void SetAttributeValue(XmlNode node, String name,String value)
        {

            if (IsAttributExist(ref node, name)) node.Attributes[name].Value = value;
            else
            {
                XmlAttribute a = node.OwnerDocument.CreateAttribute(name);
                a.Value = value;
                node.Attributes.Append(a);
            }            
        }

        static public void SetAttributeIntValue(XmlNode node, String name, int value)
        {
            SetAttributeValue(node, name, value.ToString());
        }

        static public void SetAttributeBoolValue(XmlNode node, String name, bool value)
        {
            if (value == true)
            {
                SetAttributeValue(node, name,"true");
            }
            else
            {
                SetAttributeValue(node, name, "false");
            }
        }

        static public bool GetAttributeBoolValue(XmlNode node, String name)
        {
            String val = GetAttributeStringValue(node, name);

            if (val.ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }          
        }
    }
}
