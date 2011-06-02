using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manina.Windows.Forms.ExportExcel
{
    public class OrderedItem
    {
        public Int32 Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double TotalPrice { get { return Quantity * UnitPrice; } }
        public String Name { get; set; }

        public OrderedItem()
        {

        }
    }
}
