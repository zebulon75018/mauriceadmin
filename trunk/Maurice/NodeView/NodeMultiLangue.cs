using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Manina.Windows.Forms.NodeView
{
    public class NodeMultiLangue : NodesBase
    {           
        public NodeMultiLangue()
        { 
        }

        public string EnglishText
        {
            set
            {
                UpdateAttribut(ref elm, "englishtext", value);
            }
            get { if (IsAttributExist(ref elm, "englishtext")) return elm.Attributes["englishtext"].Value; else return ""; }
        }

        public string FenchText
        {
            set
            {
                UpdateAttribut(ref elm, "frenchtext", value);
            }
            get { if (IsAttributExist(ref elm, "frenchtext")) return elm.Attributes["frenchtext"].Value; else return ""; }
        }

        public string DeutchText
        {
            set
            {
                UpdateAttribut(ref elm, "deutchtext", value);
            }
            get { if (IsAttributExist(ref elm, "deutchtext")) return elm.Attributes["deutchtext"].Value; else return ""; }
        }

        public string ItalianText
        {
            set
            {
                UpdateAttribut(ref elm, "italiantext", value);
            }
            get { if (IsAttributExist(ref elm, "italiantext")) return elm.Attributes["italiantext"].Value; else return ""; }
        }

        public string SpanishText
        {
            set
            {
                UpdateAttribut(ref elm, "spanishtext", value);
            }
            get { if (IsAttributExist(ref elm, "spanishtext")) return elm.Attributes["spanishtext"].Value; else return ""; }
        }


        public string Bitmap
        {
            set
            {
                UpdateAttribut(ref elm, "bitmap", value);
            }
            get { if (IsAttributExist(ref elm, "bitmap")) return elm.Attributes["bitmap"].Value; else return ""; }
        }

        public string Directory
        {
            set
            {
                UpdateAttribut(ref elm, "directory", value);
            }
            get { if (IsAttributExist(ref elm, "directory")) return elm.Attributes["directory"].Value; else return ""; }
        }

        public String getDirectory()
        {
            try
            {
                return elm.Attributes["directory"].Value;
            }
            catch (Exception e)
            {
                return "";
            }
        }

    }
}
