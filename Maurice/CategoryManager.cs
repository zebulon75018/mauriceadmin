using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Manina.Windows.Forms
{    
    class CategoryManager
    {
        static CategoryManager singleton = null;
        public List<Category> cat;

        public CategoryManager()
        {
            cat = new List<Category>();
            LoadXml();
        }

        public static CategoryManager Get()
        {
            if (CategoryManager.singleton == null) singleton = new CategoryManager();
            return CategoryManager.singleton;
        }

        public void Add(ref Category c)
        {
        }

        public void Delete(Category c)
        {
        }

        private void LoadXml()
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load("Category.xml");

            foreach (XmlElement e in doc.ChildNodes)
            {
                cat.Add(new Category(e)); 
            }
        }

        public void SaveXml()
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<categories></categories>");
            foreach (Category c in cat)
            {
                XmlElement e = doc.CreateElement("category");
                c.SetToXml(ref e);
            }
            doc.Save("category.xml");           
        }
    }

}
