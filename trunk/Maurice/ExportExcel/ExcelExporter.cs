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
using System.Xml;
using System.Text.RegularExpressions;

namespace Manina.Windows.Forms.ExportExcel
{
    class ExcelExporter
    {
        int idFacture = 0;
        private const Int32 STARTING_ROW = 11;

        public ExcelExporter()
        {
        }

        public int nbIDFacture()
        {
            try
            {
                if (!File.Exists("idfacture.txt"))
                    File.WriteAllText("idfacture.txt", "0");

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


        public void ExcelCustomer(NodeUser user, NodePrice price)
        {

            // On va générer une facture excel à partir des données de l'utilisateur (sa commande)
            // et des données référence des prix (NodePrice)

            ConfigManager cm = ConfigManager.getSingleton();

            if (File.Exists(cm.getExcelTemplateFile()))
            {
                using (ExcelPackage p = new ExcelPackage(new FileInfo(cm.getExcelTemplateFile()), true))
                {
                    //Set up the headers
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];

                    int nbphoto = user.NbPhoto();
                    idFacture = nbIDFacture();

                    // Identification de la facture
                    ws.Cells["B1"].Value = String.Format("FACTURE N° {0}", idFacture.ToString().PadLeft(6, '0'));
                    ws.Cells["B2"].Value = DateTime.Today.ToString("yyyy-MM-dd");

                    // Identification du client
                    ws.Cells["C6"].Value = user.chamberNumber.ToString();
                    ws.Cells["C4"].Value = user.name;
                    ws.Cells["C5"].Value = user.firstname;
                    ws.Cells["C7"].Value = user.LeavingDate;

                    #region Ici, on va générer tous les tableaux de données qui seront utilisés pour la suite de la construction du fichier excel

                    List<OrderedItem> listForfaitsDivers = new List<OrderedItem>();

                    for (int i = 0; i < price.NbForfaits(); i++)
                    {
                        XmlNode n = price.getForfait(i);

                        OrderedItem item = new OrderedItem();
                        item.Name = XMLTools.GetAttributeStringValue(n, "name");

                        listForfaitsDivers.Add(item);
                    }

                    Int32 TotalPhotoCD = 0;                    
                    Int32 TotalPhotoTirage = 0;

                    List<OrderedItem> listFormatPhotos = new List<OrderedItem>();
                    List<OrderedItem> listMerchandising = new List<OrderedItem>();
                    List<OrderedItem> listImageOnBook = new List<OrderedItem>();

                    for (int i = 0; i < price.NbProduct(); i++)
                    {
                        XmlNode n = price.getProduct(i);

                        String formatPhoto = XMLTools.GetAttributeStringValue(n, "formatphoto");

                        if (formatPhoto.ToLower() == "true")
                        {
                            OrderedItem item = new OrderedItem();
                            String currentFormatName = XMLTools.GetAttributeStringValue(n, "name");
                            item.Name = GetFormatDisplayName(currentFormatName);
                            item.UnitPrice = XMLTools.GetAttributePriceValue(n, "price");

                            // parcourir la commande utilisateur pour déterminer les quantités !!
                            // pour chaque photo commandée par l'utilisateur
                            for (int j = 0; j < user.NbPhoto(); j++)
                            {
                                XmlNode img = user.GetPhoto(j);

                                // on regarde si le format actuel a été commandé et si oui, en combien d'exemplaires
                                String strOrderedQuantity = XMLTools.GetAttributeStringValue(img, currentFormatName);

                                // Le format a été commandé, on va récupérer le chiffre et l'ajouter à la quantité de photos commandées
                                if (!String.IsNullOrEmpty(strOrderedQuantity))
                                {
                                    Int32 qty = 0;
                                    Int32.TryParse(strOrderedQuantity, out qty);

                                    item.Quantity += qty;
                                    TotalPhotoTirage += qty;
                                }
                            }

                            listFormatPhotos.Add(item);
                        }
                        else
                        {
                            OrderedItem item = new OrderedItem();

                            String currentFormatName = XMLTools.GetAttributeStringValue(n, "name");

                            item.Name = XMLTools.GetAttributeStringValue(n, "description");
                            if (item.Name == String.Empty)
                                item.Name = currentFormatName;
                            item.UnitPrice = XMLTools.GetAttributePriceValue(n, "price");

                            // parcourir la commande utilisateur pour déterminer les quantités !!
                            // pour chaque photo commandée par l'utilisateur
                            for (int j = 0; j < user.NbPhoto(); j++)
                            {
                                XmlNode img = user.GetPhoto(j);

                                // on regarde si le format actuel a été commandé et si oui, en combien d'exemplaires
                                String strOrderedQuantity = XMLTools.GetAttributeStringValue(img, currentFormatName);

                                // Le format a été commandé, on va récupérer le chiffre et l'ajouter à la quantité de photos commandées
                                if (!String.IsNullOrEmpty(strOrderedQuantity))
                                {
                                    Int32 qty = 0;
                                    Int32.TryParse(strOrderedQuantity, out qty);

                                    item.Quantity += qty;
                                }
                            }
                            listMerchandising.Add(item);
                        }
                    }

                    // avant de finir, on reparcours encore la commande utilisateur pour calculer le nombre d'images commandées sur CD (minimum 10 normalement, mais ça c'est censé etre checké en amont)
                    for (int j = 0; j < user.NbPhoto(); j++)
                    {
                        // On en profite également pour regarder si la photo a été commandée sur CD
                        // Dans ce cas, on incrément le compteur. cela nous permettra de calculer le prix de revient du CD                        
                        if (user.isPhotoOnCD(j))
                        {
                            TotalPhotoCD++;
                        }
                                                
                    }

                    // avant de finir, on reparcours encore la commande utilisateur pour calculer le nombre d'images commandées sur CD (minimum 10 normalement, mais ça c'est censé etre checké en amont)
                    for (int j = 0; j < user.NbPhoto(); j++)
                    {
                        XmlNode img = user.GetPhoto(j);
                        // On en profite également pour regarder si la photo a été commandée sur CD
                        // Dans ce cas, on incrément le compteur. cela nous permettra de calculer le prix de revient du CD
                       
                    }


                    // Maintenant on détermine les remises auxquelles l'utilisateur a le droit
                    String strPromoList = String.Empty;
                    Int32 pourcentageRemise = 0;
                    for (int i = 0; i < price.NbPromotion(); i++)
                    {
                        XmlNode n = price.getPromotion(i);

                        if (!String.IsNullOrEmpty(strPromoList))
                            strPromoList += " / ";

                        Int32 nbPhotos = XMLTools.GetAttributeIntValue(n, "nbphoto");
                        Int32 montantReduction = XMLTools.GetAttributeIntValue(n, "promotion");

                        strPromoList += String.Format("{0} P={1}%", nbPhotos.ToString(), montantReduction.ToString());
                        
                        
                        if (TotalPhotoTirage >= nbPhotos && pourcentageRemise < montantReduction)
                        {
                            pourcentageRemise = montantReduction;
                        }
                    }

                    #endregion

                    // On va itérer sur chaque format de photo disponible
                    int row = STARTING_ROW;

                    createCategoryRows(ref ws, row, listFormatPhotos);
                    row += listFormatPhotos.Count - 1;

                    createSubTotalRow(ref ws, ++row);
                    createRemiseRow(ref ws, ++row, 0.0, pourcentageRemise); // remise à calculer
                    createTotalRow(ref ws, ++row);
  
                    //createPromotionListRow(ref ws, ++row, strPromoList);

                    // Formules photos
                    List<OrderedItem> listFormulesPhotos = new List<OrderedItem>();

                    if (user.withoutprinting )
                    {
                        if (user.NbPhoto() == 12)
                        {
                            listFormulesPhotos.Add(new OrderedItem() { Name = "Forfait 50 Images sur CD", Quantity = 1, UnitPrice = price.getforfait50CD() });                            
                        }
                        else
                        {
                            listFormulesPhotos.Add(new OrderedItem() { Name = "Forfait 100 Images sur CD", Quantity = 1, UnitPrice = price.getforfait100CD() });
                            
                        }
                    }
                    else
                    {
                        listFormulesPhotos.Add(new OrderedItem() { Name = "Fichiers numériques sur CD", Quantity = TotalPhotoCD, UnitPrice = price.PrixFichierNumerique() });
                    }
                    
                    
                    createCategoryRows(ref ws, ++row, listFormulesPhotos);
                    row += listFormulesPhotos.Count - 1;
                    createInterCategoryRow(ref ws, ++row);

                    // merchandising
                    createCategoryRows(ref ws, ++row, listMerchandising);
                    row += listMerchandising.Count - 1;
                    createInterCategoryRow(ref ws, ++row);

                    // Nombre de page sur le livre.
                    // 20 pages a 300 euros soit 12000 Mur
                    // 500 mur la page supplémentaire , fonctionnant par 2 pages 

                    if (user.orderBook == true)
                    {
                        int nbPageOnBook = user.getNbPageOnBook;
                        int nbPageSupplementaire = nbPageOnBook -20;
                        row += createPhotoOnBook(ref ws, ++row, nbPageSupplementaire, price.getPrixFortaitBookPrestige(), price.getprixPageBookMore());

                        createInterCategoryRow(ref ws, ++row);
                        
                    }
                                        
                    // forfaits divers
                    createCategoryRows(ref ws, ++row, listForfaitsDivers, false);
                    row += listForfaitsDivers.Count - 1;		

                    // FINALLY
                    // LE GRAND TOTAL
                    createMainTotalQuantiteRow(ref ws, ++row, listFormatPhotos);
                    createMainTotalTtcRow(ref ws, ++row, listFormatPhotos);
                    createMainTVARow(ref ws, ++row);

                    Byte[] bin = p.GetAsByteArray();

                    string file = String.Format("{0}\\{1}-{2}.{3}.{4}-{5}.xlsx", user.getDirectory(), idFacture.ToString().PadLeft(5, '0'), user.name, user.firstname, user.chamberNumber, DateTime.Now.ToString("yyyyMMdd"));
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
            else
            {
                MessageBox.Show("Unable to open template file located at : \"" + cm.getExcelTemplateFile() + "\". Please check the file and try again.");
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

        #region Create Rows

        private void createInterCategoryRow(ref ExcelWorksheet ws, Int32 row)
        {
            ExcelRange cell = ws.Cells[String.Format("C{0}:E{0}", row)];
            cell.Merge = true;
            setCellDisabled(ref cell);
            setCellBoxed(ref cell);
        }

        private void createCategoryRows(ref ExcelWorksheet ws, Int32 row, List<OrderedItem> list, Boolean fillValues = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Boolean isFirstLine = (i == 0);
                Boolean isLastLine = (i == (list.Count - 1));

                OrderedItem item = list[i];

                ExcelRange cellLibelle = ws.Cells["A" + row];
                ExcelRange cellQuantity = ws.Cells["C" + row];
                ExcelRange cellPrice = ws.Cells["D" + row];
                ExcelRange cellTotal = ws.Cells["E" + row];

                cellLibelle.Value = item.Name;

                if (fillValues)
                    cellQuantity.Value = item.Quantity;

                if (fillValues)
                    cellPrice.Value = item.UnitPrice;
                setCellPriceFormat(ref cellPrice);

                if (fillValues)
                    cellTotal.Formula = String.Format("C{0}*D{0}", row);
                else
                    cellTotal.Value = 0.00;
                setCellPriceFormat(ref cellTotal);

                if (isFirstLine)
                {
                    setCellPriceTop(ref cellQuantity);
                    setCellPriceTop(ref cellPrice);
                    setCellPriceTop(ref cellTotal);
                }
                else if (isLastLine)
                {
                    setCellPriceBottom(ref cellQuantity);
                    setCellPriceBottom(ref cellPrice);
                    setCellPriceBottom(ref cellTotal);
                }
                else
                {
                    setCellPriceMiddle(ref cellQuantity);
                    setCellPriceMiddle(ref cellPrice);
                    setCellPriceMiddle(ref cellTotal);
                }

                cellQuantity.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cellQuantity.Style.Font.Bold = true;

                row++;
            }
        }

        private int createPhotoOnBook(ref ExcelWorksheet ws, Int32 row,Int32 quantityMore,Int32 priceForfait,Int32 pricePageBookMore)
        {
            // LE nombre de row ajouté.
            int returnvalue = 1;
            ExcelRange cellLibelle = ws.Cells["A" + row];
            ExcelRange cellQuantity = ws.Cells["C" + row];
            //ExcelRange cellPrice = ws.Cells["D" + row];
            ExcelRange cellTotal = ws.Cells["E" + row];

            cellLibelle.Value = "Livre Prestige";
            cellQuantity.Value = 1 ;
            //cellPrice.Value = priceUnity;
            cellTotal.Value =(Double ) priceForfait ;

            setCellBoxed(ref cellQuantity);            
            setCellPriceMiddle(ref cellTotal);
          
             row++;
             returnvalue++;

             ExcelRange cellLibelle2 = ws.Cells["A" + row];
             ExcelRange cellQuantity2 = ws.Cells["C" + row];
             ExcelRange cellPrice2 = ws.Cells["D" + row];
             ExcelRange cellTotal2 = ws.Cells["E" + row];

             cellLibelle2.Value = "Pages supplémentaires";
             cellQuantity2.Value = quantityMore ;
             setCellBoxed(ref cellQuantity2);
             cellPrice2.Value = ((Double)pricePageBookMore);
             cellTotal2.Value = (Double)( quantityMore * pricePageBookMore);

             setCellPriceMiddle(ref cellPrice2);

             row++;
             returnvalue++;                                   
            return returnvalue;
            
        }

        private void createSubTotalRow(ref ExcelWorksheet ws, Int32 row)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange cellSubTotalCount = ws.Cells["C" + row];
            ExcelRange cellSubTotalUnitPrice = ws.Cells["D" + row];
            ExcelRange cellSubTotalPrice = ws.Cells["E" + row];

            cellSubTotalLib.Value = "Sous Total";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            cellSubTotalCount.Formula = String.Format("SUM(C{0}:C{1})", STARTING_ROW, (row - 1));
            setCellBoxed(ref cellSubTotalCount);
            cellSubTotalCount.Style.Font.Bold = true;
            cellSubTotalCount.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            cellSubTotalUnitPrice.Value = String.Empty;
            setCellBoxed(ref cellSubTotalUnitPrice);
            setCellDisabled(ref cellSubTotalUnitPrice);

            cellSubTotalPrice.Formula = String.Format("SUM(E{0}:E{1})", STARTING_ROW, (row - 1));
            setCellBoxed(ref cellSubTotalPrice);
            setCellPriceFormat(ref cellSubTotalPrice);
            cellSubTotalPrice.Style.Font.Bold = false;
        }

        private void createRemiseRow(ref ExcelWorksheet ws, Int32 row, Double remise, Int32 percentRemise)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange cellSubTotalCount = ws.Cells["C" + row];
            ExcelRange cellSubTotalUnitPrice = ws.Cells["D" + row];
            ExcelRange cellSubTotalPrice = ws.Cells["E" + row];

            cellSubTotalLib.Value = "Remise";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            cellSubTotalCount.Value = String.Empty;
            setCellStandard(ref cellSubTotalCount);
            setCellDisabled(ref cellSubTotalCount);

            cellSubTotalUnitPrice.Value = String.Empty;
            setCellStandard(ref cellSubTotalUnitPrice);
            setCellDisabled(ref cellSubTotalUnitPrice);

            cellSubTotalPrice.Formula = String.Format("E{0}*C{1}", row - 1, row);
            setCellStandard(ref cellSubTotalPrice);
            setCellBoxed(ref cellSubTotalPrice);
            setCellPriceFormat(ref cellSubTotalPrice);
            cellSubTotalPrice.Style.Font.Bold = false;

            ExcelRange cellQtyPrice = ws.Cells[row, 3, row, 4];
            cellQtyPrice.Merge = true;
            setCellBoxed(ref cellQtyPrice);
            cellQtyPrice.Value = String.Format("{0}%", percentRemise);
            cellQtyPrice.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cellQtyPrice.Style.Font.Bold = true;
        }

        private void createTotalRow(ref ExcelWorksheet ws, Int32 row)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange cellSubTotalCount = ws.Cells["C" + row];
            ExcelRange cellSubTotalUnitPrice = ws.Cells["D" + row];
            ExcelRange cellSubTotalPrice = ws.Cells["E" + row];

            cellSubTotalLib.Value = "Total";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            cellSubTotalCount.Value = String.Empty;
            setCellStandard(ref cellSubTotalCount);
            setCellDisabled(ref cellSubTotalCount);

            cellSubTotalUnitPrice.Value = String.Empty;
            setCellStandard(ref cellSubTotalUnitPrice);
            setCellDisabled(ref cellSubTotalUnitPrice);

            cellSubTotalPrice.Formula = String.Format("E{0}-E{1}", row - 2, row - 1);
            setCellStandard(ref cellSubTotalPrice);
            setCellBoxed(ref cellSubTotalPrice);
            setCellPriceFormat(ref cellSubTotalPrice);
            cellSubTotalPrice.Style.Font.Bold = false;

            ExcelRange cellQtyPrice = ws.Cells[row, 3, row, 4];
            cellQtyPrice.Merge = true;
            setCellPriceBottom(ref cellQtyPrice);
        }

        private void createPromotionListRow(ref ExcelWorksheet ws, Int32 row, String content) {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange mainCell = ws.Cells["A" + row + ":D" + row];
            ExcelRange lastCell = ws.Cells["E" + row];

            setCellStandard(ref mainCell);
            mainCell.Value = content;
            mainCell.Merge = true;

            setCellDisabled(ref lastCell);
            setCellBoxed(ref lastCell);
            lastCell.Value = String.Empty;
        }

        #endregion

        #region Rows Total Final

        private void createMainTotalQuantiteRow(ref ExcelWorksheet ws, Int32 row, List<OrderedItem> list)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange cellSubTotalCount = ws.Cells["C" + row];
            ExcelRange lastCell = ws.Cells[String.Format("D{0}:E{0}", row)];

            cellSubTotalLib.Value = "Total quantité";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            cellSubTotalCount.Formula = String.Format("C{0}+SUM(C{1}:C{2})", STARTING_ROW + list.Count - 1, STARTING_ROW + list.Count + 3, (row - 1));
            setCellBoxed(ref cellSubTotalCount);
            cellSubTotalCount.Style.Font.Bold = true;
            cellSubTotalCount.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            lastCell.Merge = true;
            setCellDisabled(ref lastCell);
            setCellBoxed(ref lastCell);
        }

        private void createMainTotalTtcRow(ref ExcelWorksheet ws, Int32 row, List<OrderedItem> list)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange firstCell = ws.Cells[String.Format("C{0}:D{0}", row)];
            ExcelRange cellSubTotalPrice = ws.Cells["E" + row];

            cellSubTotalLib.Value = "Total TTC MUR";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            firstCell.Merge = true;
            setCellDisabled(ref firstCell);
            setCellBoxed(ref firstCell);

            cellSubTotalPrice.Formula = String.Format("SUM(E{0}:E{1})", STARTING_ROW + list.Count + 2, (row - 1));
            setCellStandard(ref cellSubTotalPrice);
            setCellBoxed(ref cellSubTotalPrice);
            setCellPriceFormat(ref cellSubTotalPrice);
            cellSubTotalPrice.Style.Font.Bold = false;
        }

        private void createMainTVARow(ref ExcelWorksheet ws, Int32 row)
        {
            // Puis on va créer les cellules de total
            // On sait où on en est grace à row
            ExcelRange cellSubTotalLib = ws.Cells["B" + row];
            ExcelRange firstCell = ws.Cells[String.Format("C{0}:D{0}", row)];
            ExcelRange lastCell = ws.Cells["E" + row];

            cellSubTotalLib.Value = "TVA 15%";
            setCellStandard(ref cellSubTotalLib);
            cellSubTotalLib.Style.Font.Bold = true;
            cellSubTotalLib.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            setCellDisabled(ref lastCell);
            setCellBoxed(ref lastCell);

            firstCell.Merge = true;
            setCellBoxed(ref firstCell);
            firstCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            setCellPriceFormat(ref firstCell);
            firstCell.Formula = String.Format("E{0}*0.85", row - 1); // 100% ttc - 15% tva = 0.85
        }

        #endregion

        #region Cell Styles
        private void setCellPriceTop(ref ExcelRange cell)
        {
            setCellPriceMiddle(ref cell);
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        }

        private void setCellPriceMiddle(ref ExcelRange cell)
        {
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.General;
        }

        private void setCellPriceBottom(ref ExcelRange cell)
        {
            setCellPriceMiddle(ref cell);
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        private void setCellDisabled(ref ExcelRange cell)
        {
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        }

        private void setCellBoxed(ref ExcelRange cell)
        {
            setCellPriceTop(ref cell);
            setCellPriceBottom(ref cell);
        }

        private void setCellStandard(ref ExcelRange cell)
        {
            cell.Style.Font.Bold = false;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.None;
            cell.Style.Border.Top.Style = ExcelBorderStyle.None;
            cell.Style.Border.Left.Style = ExcelBorderStyle.None;
            cell.Style.Border.Right.Style = ExcelBorderStyle.None;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.General;
            cell.Style.Fill.PatternType = ExcelFillStyle.None;
        }

        private void setCellPriceFormat(ref ExcelRange cell)
        {
            cell.Style.Numberformat.Format = "# ##0.00  ";
        }

        #endregion

        private String GetFormatDisplayName(String name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                Regex r = new Regex("[^0-9]");
                name = r.Replace(name, "");

                if (name.Length > 2)
                {
                    name = "Photos format " + name.Substring(0, name.Length - 2) + "x" + name.Substring(name.Length - 2);
                }
                else
                {
                    name = "Photos format " + name;
                }

                return name;
            }
            else
            {
                return name;
            }
        }
    }
}
