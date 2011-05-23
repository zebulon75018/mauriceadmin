using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Manina.Windows.Forms
{
    public class Category
    {
        public string Name;
        public string frenchName;
        public string germanName;
        public string ItalianName;
        public Category()
        {

        }

        public Category(XmlElement e)
        {
            Name = e.Attributes["Name"].Value;
            frenchName = e.Attributes["frenchName"].Value;
            germanName = e.Attributes["germanName"].Value;
            ItalianName = e.Attributes["ItalianName"].Value;
        }      
        public void SetToXml(ref XmlElement e)
        {
            e.Attributes["frenchName"].Value = frenchName;
            e.Attributes["germanName"].Value = germanName;
            e.Attributes["ItalianName"].Value = ItalianName;
            
        }
    }
}
