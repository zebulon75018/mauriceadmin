using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms; 
using Manina.Windows.Forms.ExportExcel;
using  Manina.Windows.Forms.NodeView;

namespace Manina.Windows.Forms
{
    [DefaultPropertyAttribute("Name")]
    public class FormatGoodiesPicture
    {
        private XmlNode n;
        private NodeUser user;
        // Name property with category attribute and 
        // description attribute added 
        [CategoryAttribute("Filename "), DescriptionAttribute("Filename Picture")]
        public string Filename
        { get { return XMLTools.GetAttributeStringValue(n, "path"); } }
        [CategoryAttribute("Filename "), DescriptionAttribute("Filename Picture")]
        public string Source
        { get { return XMLTools.GetAttributeStringValue(n, "originalpath"); } }

        [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 15x23")]       
        public int F_1523
        {
            get { return XMLTools.GetAttributeIntValue(n, "F_1523"); }
            set { XMLTools.SetAttributeIntValue(n,"F_1523",value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 20x30")]       
        public int F_2030
        { get { return XMLTools.GetAttributeIntValue(n, "F_2030"); }
            set { XMLTools.SetAttributeIntValue(n, "F_2030", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 30x45")]       
        public int F_3045
        { get { return XMLTools.GetAttributeIntValue(n, "F_3045"); }
            set { XMLTools.SetAttributeIntValue(n, "F_3045", value);
            user.internalSave();
            }
        }

          [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 40x60")]
        public int F_4060
        { get { return XMLTools.GetAttributeIntValue(n, "F_4060"); }
            set { XMLTools.SetAttributeIntValue(n, "F_4060", value);
            user.internalSave();
            }
          }

          [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 50x75")]
        public int F_5075
          { get { return XMLTools.GetAttributeIntValue(n, "F_5075"); }
              set { XMLTools.SetAttributeIntValue(n, "F_5075", value);
              user.internalSave();              
              }
          }

          [CategoryAttribute("Format "), DescriptionAttribute("Format de photo 60x95")]
        public int F_6095
          { get { return XMLTools.GetAttributeIntValue(n, "F_6095"); }
              set { XMLTools.SetAttributeIntValue(n, "F_6095", value);
              user.internalSave();
              }
          }

        [CategoryAttribute("Objet"),DescriptionAttribute("Goodies")]
        public int TeeShirt
          { get { return XMLTools.GetAttributeIntValue(n, "TeeShirt"); }
              set { XMLTools.SetAttributeIntValue(n, "TeeShirt", value);
              user.internalSave();
              }
        }
        [CategoryAttribute("Objet")]
        public int Bags
        { get { return XMLTools.GetAttributeIntValue(n, "Bags"); }
            set { XMLTools.SetAttributeIntValue(n, "Bags", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Objet")]
        public int Mug
        { get { return XMLTools.GetAttributeIntValue(n, "Mug"); }
            set { XMLTools.SetAttributeIntValue(n, "Mug", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Objet")]
        public int Pelluche
        { get { return XMLTools.GetAttributeIntValue(n, "Pelluche"); }
            set { XMLTools.SetAttributeIntValue(n, "Pelluche", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Objet")]
        public int PorteCles
        { get { return XMLTools.GetAttributeIntValue(n, "PorteCles"); }
            set { XMLTools.SetAttributeIntValue(n, "PorteCles", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("Objet")]
        public int Tapisdesouri
        { get { return XMLTools.GetAttributeIntValue(n, "Tapisdesouri"); }
            set { XMLTools.SetAttributeIntValue(n, "Tapisdesouri", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("OnCd")]
        public bool onCD
        { get { return XMLTools.GetAttributeBoolValue(n, "imageoncd"); }
            set { XMLTools.SetAttributeBoolValue(n, "imageoncd", value);
            user.internalSave();
            }
        }

        [CategoryAttribute("OnBook")]
        public bool onBook
        { get { return XMLTools.GetAttributeBoolValue(n, "imageonbook"); }
            set { XMLTools.SetAttributeBoolValue(n, "imageonbook", value);
            user.internalSave();
            }
        }

        public FormatGoodiesPicture(XmlNode node,NodeUser u)
        {
            n = node;
            user = u;
        }
    } 

}
