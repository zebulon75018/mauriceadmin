using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Manina.Windows.Forms.ExportExcel;

namespace Manina.Windows.Forms.NodeView
{
    class NodePrice : NodesBase
    {
        protected XmlNode listProduct;
        protected XmlNode listPromotion;
        protected XmlNode listForfaits;
        public NodePrice()
        {
            FilenameXml = CommunConfig.getInstance().productFile;
            Text = "Price";
            doc = new XmlDocument();
            try
            {
                doc.Load(FilenameXml);
                root = doc.ChildNodes[0];
                
                foreach(XmlNode tmpNode in root)
                {
                  if (tmpNode.Name == "listproduct")
                  {
                     listProduct = tmpNode;
                  }
                  if (tmpNode.Name == "promotions")
                  {
                      listPromotion = tmpNode;
                  }

                  if (tmpNode.Name == "forfaits")
                  {
                      listForfaits = tmpNode;
                  }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
        }
        public int getprixPictureBook()
        {
            return XMLTools.GetAttributeIntValue(root, "prixPictureBook");
        }

        public int getforfait50CD()
        {
            return XMLTools.GetAttributeIntValue(root, "forfait50CD");
        }

        public int getforfait100CD()
        {
            return XMLTools.GetAttributeIntValue(root, "forfait100CD");
        }

        public int getPrixFortaitBookPrestige()
        {
            return XMLTools.GetAttributeIntValue(root, "prixPrestigeBook");
        }

        public int getprixPageBookMore()
        {
            return XMLTools.GetAttributeIntValue(root, "prixPrestigePageMore");
        }

        public Int32 PrixFichierNumerique()
        {
            Int32 prix = XMLTools.GetAttributeIntValue(doc.FirstChild, "prixFichierCD");

            if (prix == 0)
                return 80;
            else
                return prix;
        }

        public int NbProduct()
        {
            return listProduct.ChildNodes.Count;
        }

        public int NbPromotion()
        {
            return listPromotion.ChildNodes.Count;
        }

        public int NbForfaits()
        {
            return listForfaits.ChildNodes.Count;
        }

        public XmlNode getForfait(int n)
        {
            return listForfaits.ChildNodes[n];
        }

        public XmlNode getProduct(int n)
        {
            return listProduct.ChildNodes[n];
        }
        public XmlNode getPromotion(int n)
        {
            return listPromotion.ChildNodes[n];
        }
        /*
        public int getPriceProduct(int n)
        {            
            attribNode = _node.Attributes.GetNamedItem("price");
            if (attribNode != null)
            {
                try
                {
                    return int.Parse(node.Attributes["name"].Value);
                }

                this.textBoxPrice.Text = _node.Attributes["price"].Value;
            }            
            return elm.ChildNodes[n];
        }
        public string  getIDproduct(int n)
        {
            XmlNode node = getProduct(n);
            XmlNode attribNode = node.Attributes.GetNamedItem("name");
            if (attribNode != null)
            {
                return node.Attributes["name"].Value;                
            }
            return "";
        }
         * */

    }
}
