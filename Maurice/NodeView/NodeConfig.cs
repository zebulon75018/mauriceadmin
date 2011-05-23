using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{
    public class NodeConfig : NodesBase
    {

        public NodeConfig()
        {
            FilenameXml = CommunConfig.getInstance().guiconfigfile;
            Text = "Configuration";
            doc = new XmlDocument();
            try
            {
                doc.Load(FilenameXml);
                root = doc.ChildNodes[0];
                elm = root;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
        }

    public string ImageInfo
        {
            set
            {
                UpdateAttribut(ref elm, "imageinfo", value);
            }
            get { if (IsAttributExist(ref elm, "imageinfo")) return elm.Attributes["imageinfo"].Value; else return ""; }
        }

    public string ImagePhoto
    {
        set
        {
            UpdateAttribut(ref elm, "imagephoto", value);
        }
        get { if (IsAttributExist(ref elm, "imagephoto")) return elm.Attributes["imagephoto"].Value; else return ""; }
    }

    public string ImageLangue
    {
        set
        {
            UpdateAttribut(ref elm, "imagelangue", value);
        }
        get { if (IsAttributExist(ref elm, "imagelangue")) return elm.Attributes["imagelangue"].Value; else return ""; }
    }

    public Color colorStart
    {
        set
        {
            string colorValue = System.Drawing.ColorTranslator.ToHtml(value);
            UpdateAttribut(ref elm, "colorstart", colorValue);
        }
        get { if (IsAttributExist(ref elm, "colorstart")) 
        {
            return System.Drawing.ColorTranslator.FromHtml(elm.Attributes["colorstart"].Value); 
        }
        else return Color.Black;         
        }
    }

    public Color colorEnd
    {
        set
        {
            string colorValue = System.Drawing.ColorTranslator.ToHtml(value);
            UpdateAttribut(ref elm, "colorend", colorValue);
        }
        get
        {
            if (IsAttributExist(ref elm, "colorend"))
            {
                return System.Drawing.ColorTranslator.FromHtml(elm.Attributes["colorend"].Value);
            }
            else return Color.Black;
        }
    }

    public int Angle
    {
        set
        {
            UpdateAttribut(ref elm, "angle", value.ToString());
        }
        get {
            try
            {
                if (IsAttributExist(ref elm, "angle")) return Int32.Parse(elm.Attributes["angle"].Value); else return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }

    public string TypeGradient
    {
        set
        {
            UpdateAttribut(ref elm, "typegradient", value);
        }
        get { if (IsAttributExist(ref elm, "typegradient")) return elm.Attributes["typegradient"].Value; else return ""; }
    }

    public string RotateImage
    {
        set
        {
            UpdateAttribut(ref elm, "rotateimage", value);
        }
        get { if (IsAttributExist(ref elm, "rotateimage")) return elm.Attributes["rotateimage"].Value; else return ""; }
    }

        
        

   }
}
