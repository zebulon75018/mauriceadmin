using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Collections;

using Manina.Windows.Forms.NodeView;
using ShellLib;

namespace Manina.Windows.Forms.ExportExcel
{
    class ExcelExporter
    {
        int idFacture = 0;
        public ExcelExporter()
        {
        }

        public int nbIDFacture()
        {
            try
            {
                StreamReader sr = new StreamReader("idfacture.txt");
                String content = sr.ReadToEnd();
                int result = Int32.Parse(content);
                int newID = result + 1;
                sr.Close();
                StreamWriter sw = new StreamWriter("idfacture.txt");
                sw.WriteLine(newID.ToString());
                sw.Close();

                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Reading idfacture.txt , please edit it and put into a number \n " + e.Message);
                return -1;
            }
        }

        public void ExcelCustomer(NodeUser user,NodePrice price)
        {
            ConfigManager cm = ConfigManager.getSingleton();
            using (ExcelPackage p = new ExcelPackage(new FileInfo(cm.getExcelTemplateFile()), true))
            {
                //Set up the headers
                ExcelWorksheet ws = p.Workbook.Worksheets[1];               

                int nbphoto = user.NbPhoto();
                idFacture = nbIDFacture();

                ws.Cells["C6"].Value = user.chamberNumber.ToString();
                ws.Cells["C4"].Value = user.name;
                ws.Cells["C5"].Value = user.firstname;
                ws.Cells["B2"].Value = idFacture;
                ws.Cells["C7"].Value = user.LeavingDate;
                int step = 11;
                double finalPrise = 0;
                double finalPriseDollar = 0;

                IDictionary formatPhoto = new Dictionary<string, int>();

                for (int n = 0; n < nbphoto; n++)
                {

                    if (formatPhoto.Contains(user.FormatPhoto(n)))
                    {
                        formatPhoto[user.FormatPhoto(n)] = ((int)formatPhoto[user.FormatPhoto(n)]) + 1;
                    }
                    else
                    {
                        formatPhoto.Add(user.FormatPhoto(n), 1);
                    }
                }
                int nc=0;
                foreach (string format in formatPhoto.Keys)
                 {                                    
                    ws.Cells["A" + (nc + step).ToString()].Value = format;
                    ws.Cells["C" + (nc + step).ToString()].Value = formatPhoto[format];
                    ws.Cells["D" + (nc + step).ToString()].Value = ((int)formatPhoto[format]) * cm.Price(format);
                    finalPrise += cm.Price(format);                                         
                    //ws.Cells["E" + (n + step).ToString()].Value = cm.PriceDollar(user.FormatPhoto(n));
                    finalPriseDollar += cm.PriceDollar(format);
                    nc++;
                 }
                
                    //ws.Cells["B" + (n + step).ToString()].Value = user.FilenamePhoto(n);
                                
                if (user.orderCD)
                {
                    
                    ws.Cells["A" + (nbphoto + step).ToString()].Value = "CD";
                    ws.Cells["D" + (nbphoto + step).ToString()].Value = cm.PriceCD();
                    finalPrise += cm.PriceCD();
                    //ws.Cells["E" + (nbphoto + step).ToString()].Value = cm.PriceCDDollar();
                    finalPriseDollar += cm.PriceCDDollar();
                    nbphoto += 1;
                }

                ws.Cells["B" + (nbphoto + step).ToString()].Value = "Total";
                ws.Cells["D" + (nbphoto + step).ToString()].Value = finalPrise;
                //ws.Cells["E" + (nbphoto + step).ToString()].Value = finalPriseDollar;
                /*
                ws.Cells["A20"].Value = "Date";
                ws.Cells["B20"].Value = "EOD Rate";
                ws.Cells["B20:D20"].Merge = true;
                ws.Cells["E20"].Value = "Change";
                ws.Cells["E20:G20"].Merge = true;
                ws.Cells["B20:E20"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                using (ExcelRange row = ws.Cells["A20:G20"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    row.Style.Font.Color.SetColor(Color.White);
                    row.Style.Font.Bold = true;
                }
                ws.Cells["B21"].Value = "USD/JPY";
                ws.Cells["C21"].Value = "USD/EUR";
                ws.Cells["D21"].Value = "USD/GBP";
                ws.Cells["E21"].Value = "USD/JPY";
                ws.Cells["F21"].Value = "USD/EUR";
                ws.Cells["G21"].Value = "USD/GBP";
                using (ExcelRange row = ws.Cells["A21:G21"])
                {
                    row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    row.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    row.Style.Font.Color.SetColor(Color.Black);
                    row.Style.Font.Bold = true;
                }
                 * */
                Byte[] bin = p.GetAsByteArray();

                string file = cm.getExcelFile() + idFacture.ToString() + ".xlsx";//.getExcelFile();
                try
                {
                    File.WriteAllBytes(file, bin);
                    ShellExecute se = new ShellExecute();
                    //se.Verb = ShellExecute.PrintFile;
                    se.Path = file;
                    se.Execute();                    
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error Exporting Excel " + e.Message);
                }
            }
        }

        public Dictionary<string, int> getTotalProduct(NodeUser user, NodePrice price)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            for (int n = 0; n < price.NbProduct(); n++)
            {
               // XmlNode
                //nodePrice.getProduct(n)
            }
            return result;
        }        

        public void ExcelAddCommand()
        {
            using (ExcelPackage p = new ExcelPackage(new FileInfo(@"c:\dev\command.xlsx")))
            {
                ExcelWorksheet ws = p.Workbook.Worksheets[1];       
                //ws.Cells[
            }
        }

        public void ExcelAddPrint()
        {
        }
    }
}
