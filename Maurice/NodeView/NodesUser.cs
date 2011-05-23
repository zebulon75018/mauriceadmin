using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Manina.Windows.Forms;
using System.Windows.Forms;

namespace Manina.Windows.Forms.NodeView
{
    public class NodesUser : NodesBase
    {               
        static public string path=CommunConfig.getInstance().userDirectory;
        public NodesUser()
        {
            Text = "Customer";
            try
            {
                foreach (string file in Directory.GetFileSystemEntries(NodesUser.path, "*.xml"))
                {
                    this.Nodes.Add(new NodeUser(file));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message); 
            }
        }

        public NodeUser Select(string u)
        {
            foreach (NodeUser n in this.Nodes)
            {
                if (n.chamberNumber == u)
                {
                    return n;
                }
            }
            return null;
        }

        public List<String> TomorowCustomerLeave()
        {
            List<String> result = new List<string>();
            DateTime tomorow = new DateTime(DateTime.Now.AddDays(1).Year,DateTime.Now.AddDays(1).Month,DateTime.Now.AddDays(1).Day);
            foreach (NodeUser u in this.Nodes)
            {
                
                if (u.LeavingDateTime == tomorow)
                {
                    result.Add(u.chamberNumber + ":"+ u.firstname + "/" + u.name);
                }
            }
            return result;
        }

        public List<String> TomorowCustomerToday()
        {
            List<String> result = new List<string>();
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            foreach (NodeUser u in this.Nodes)
            {
                if (u.LeavingDateTime == today)
                {
                    result.Add(u.chamberNumber + ":" + u.firstname + "/" + u.name);
                }
            }
            return result;
        }

        public List<String> TomorowCustomerLeft()
        {
            List<String> result = new List<string>();
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            foreach (NodeUser u in this.Nodes)
            {
                if (u.LeavingDateTime < today)
                {
                    result.Add(u.chamberNumber + ":" + u.firstname + "/" + u.name);
                }
            }
            return result;
        }
    }
}
