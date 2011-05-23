using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using  Manina.Windows.Forms.NodeView;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

namespace Manina.Windows.Forms.printandCommandControler
{
    class PrintAndCommandControler
    {     
        static public void CommandLogger(NodeUser user,NodePrice price)
        {
            string filename = "command_"+DateTime.Now.Month.ToString()+ "_"+ DateTime.Now.Year.ToString()+".log";
            StreamWriter sw = new StreamWriter(filename,true);

            int nbphoto = user.NbPhoto();            

            for (int n = 0; n < nbphoto; n++)
            {
                string photographe = getPhotographe(user.FilenamePhoto(n));
                sw.WriteLine(DateTime.Now.ToShortDateString() + ";"+ user.chamberNumber + ";" + user.FilenamePhoto(n) + ";" + photographe + ";" + user.FormatPhoto(n) + ";" + user.orderCD + ";");
            }
            sw.Close();

            try
            {
                for (int n = 0; n < nbphoto; n++)
                {
                    List<String> data = new List<string>();
                    string photographe = getPhotographe(user.FilenamePhoto(n));
                    data.Add(DateTime.Now.ToShortDateString());
                    data.Add(user.FilenamePhoto(n));
                    data.Add(photographe);
                    data.Add(user.FormatPhoto(n));
                 //   data.Add(user.orderCD.ToString());
                 //   data.Add(user.chamberNumber);
                    Insert(1, data);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message);
            }
        }

        static public void PrintLogger(string img, string format)
        {
            string filename = "print_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + ".log";
            StreamWriter sw = new StreamWriter(filename, true);

            string photographe= getPhotographe(img);
            sw.WriteLine(DateTime.Now.ToShortDateString() + ";" + img +";" +photographe + ";" + format + ";");
            sw.Close();

            /*
            try
            {
                List<String> data = new List<string>();
                data.Add(DateTime.Now.ToShortDateString());
                data.Add(img );
                data.Add(photographe);
                data.Add(format);
                Insert(0, data);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message);
            }
             */
        }

        static private void Insert(int feedNumber,List<String> data)
        {            
            try
            {
                SpreadsheetsService service = new SpreadsheetsService("exampleCo-exampleApp-1");
                service.setUserCredentials("mauriceprojet", "xxxxxxxx");
                SpreadsheetQuery query = new SpreadsheetQuery();
                SpreadsheetFeed feed = service.Query(query);
                SpreadsheetEntry entry = (SpreadsheetEntry)feed.Entries[feedNumber];                
                AtomLink link = entry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);

                WorksheetQuery query2 = new WorksheetQuery(link.HRef.ToString());
                WorksheetFeed feed2 = service.Query(query2);
                    
                WorksheetEntry worksheet = (WorksheetEntry )feed2.Entries[0];
                InsertRow(service, worksheet, data);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error " + e.Message);
            }                        
        }
        private static void InsertRow(SpreadsheetsService service, WorksheetEntry entry,List<String > data)
        {
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = service.Query(query);
            
            ListEntry newRow = new ListEntry();

            List<String> localName = new List<string>();
            localName.Add("date");
            localName.Add("filename");
            localName.Add("photographe");
            localName.Add("format");
           // localName.Add("orderCD");
           // localName.Add("chamber number");

            int n = 0;
            
            foreach (string value in data)
            {
                ListEntry.Custom curElement = new ListEntry.Custom();
                curElement.LocalName = localName[n];
                curElement.Value = value;                
                newRow.Elements.Add(curElement);
                n++;
            }

            ListEntry insertedRow = feed.Insert(newRow) as ListEntry;            
            //return insertedRow;
        }

        static public string getPhotographe(string img)
        {
            char [] sep = new char[1];
            sep[0] = '_';
            FileInfo fi = new FileInfo(img);
            if (fi.Name == null) return "NONAME";
            if (fi.Name == "") return "NONAME";
            string [] str = fi.Name.Split(sep);
            if (str.Length > 1)
            {
                return str[0];
            }
            return "NONAME";
        }
    }
}
