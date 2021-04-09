//using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using Aspose.Cells;


//using wd = Microsoft.Office.Interop.Word;
//using Excel = Microsoft.Office.Interop.Excel;


namespace InternetExplorer
{
    public class PDFHelper
    {

        DataSet dataSetTable = null;
        public DataSet ReadTablesFromPdf(string filePath)
        {
            dataSetTable = new DataSet();
            try
            {
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='C:\\Users\\Admin\\Desktop\\BOTDesignerMaster\\BOTDesignerMaster\\PDFFiles\\Invoice6.xlsx';Extended Properties=Excel 8.0;");
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                MyCommand.TableMappings.Add("Table", "TestTable");
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet);
                MyConnection.Close();
            
            }
            catch (Exception ex)
            {
                //Utilities.Logging.Logging.LogErrorMsg("Read_PDFTables", "ReadTablesFromPdf", ex.Message, (ex.InnerException) != null ? ex.InnerException.ToString() : "inner exception is null" + ex.ToString());
                Console.WriteLine("Exception :"+ex);
            }
            return dataSetTable;
        }
               
        private string GetValue(SpreadsheetDocument doc, DocumentFormat.OpenXml.Spreadsheet.Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

      
        public static int? GetColumnIndexFromName(string columnName)
        {
            int? columnIndex = null;

            string[] colLetters = Regex.Split(columnName, "([A-Z]+)");
            colLetters = colLetters.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (colLetters[0].Count() <= 2)
            {
                int index = 0;
                foreach (string col in colLetters)
                {
                    List<char> col1 = colLetters.ElementAt(index).ToCharArray().ToList();
                    int? indexValue = Letters.IndexOf(col1.ElementAt(index));

                    if (indexValue != -1)
                    {
                        // The first letter of a two digit column needs some extra calculations
                        if (index == 0 && colLetters[0].Count() == 2)
                        {
                            columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
                        }
                        else
                        {
                            columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
                        }
                    }

                    index++;
                }
            }

            return columnIndex;
        }

        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        public enum CellReferencePartEnum
        {
            None,
            Column,
            Row,
            Both
        }

        private static List<char> Letters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ' };


        public static XElement WDRetrieveTOC(string filePath)
        {
            XElement TOC = null;

            using (var document = WordprocessingDocument.Open(filePath, false))
            {
                var docPart = document.MainDocumentPart;
                var doc = docPart.Document;

                OpenXmlElement block = doc.Descendants<DocPartGallery>().
                  Where(b => b.Val.HasValue &&
                    (b.Val.Value == "Table of Contents")).FirstOrDefault();

                if (block != null)
                {
                    // Back up to the enclosing SdtBlock and return that XML.
                    while ((block != null) && (!(block is SdtBlock)))
                    {
                        block = block.Parent;
                    }
                    TOC = new XElement("TOC", block.OuterXml);
                }
            }
            return TOC;
        }

        internal string ReadPdfAsText(string filePath)
        {
            // throw new NotImplementedException();
            PDDocument doc = null;

            doc = PDDocument.load(filePath);
            PDFTextStripper stripper = new PDFTextStripper();
            Console.WriteLine(stripper.getText(doc));
            return stripper.getText(doc);

            // return string.Empty;
        }

        public string BuildDataString(System.Data.DataTable data)
        {
            string dataString = "";
            for (int nrCol = 1; nrCol <= data.Columns.Count; nrCol++)
            {
                // Fill the column headings.
                dataString += data.Columns[nrCol - 1].ColumnName;
                if (nrCol < data.Columns.Count)
                {
                    // Append a field delimiter.
                    dataString += "\t";
                }
                else
                {
                    // We're on the last colunm, so append a 
                    // record delimiter
                    dataString += "\n";
                }
            } // end for column headings
            for (int nrRow = 1; nrRow <= data.Rows.Count; nrRow++)
            {
                System.Data.DataRow rw = data.Rows[nrRow - 1];
                for (int nrCol = 1; nrCol <= data.Columns.Count; nrCol++)
                {
                    dataString += rw[nrCol - 1].ToString();
                    if (nrCol < data.Columns.Count)
                    {
                        // Append a field delimiter.
                        dataString += "\t";
                    }
                    else
                    {
                        // We're on the last column, so append a
                        // record delimiter.
                        dataString += "\n";
                    }
                }
            }
            return dataString;
        }

        public static void xlxs()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"D:\Invoice6.xlsx");
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            string Value1 = "Coordinator_K";

            for (int i = 1; i < rowCount; i++)
            {
                for (int j = 1; j < colCount; j++)
                {
                // Commented by  vinay : Error Value2 is not Defined By Object
               //if (Value1.Equals(xlRange.Cells[i, j].Value2.ToString()))
                     
                        Console.WriteLine("true");


                }
            }
        }

        int rownumExcelSrNo = 0;
        int rowNumExcelTaxval = 0;
        //SP: Find Row Num Using text String....
        public void SearchText()
        {
         
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook("D:\\Sautinsoft\\Invoice6.xlsx");
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];

            Aspose.Cells.Cells cells = sheet.Cells;
            //Suppose you need to add a text into the cells.
            //You need to find out if this text is already there in some cells.
            //You may use Find and Search options provided by Aspose.Cells APIs for this purpose.
            string[] SrNos = new string[] { "SNo.", "Serial No.", "Sr. No.", "Serial Number." };

           // string text = "Sr. No.";
            FindOptions findOptions = new FindOptions();
            findOptions.CaseSensitive = false;
            findOptions.LookInType = LookInType.Values;
           // Aspose.Cells.Cell foundCell = cells.Find(text.Trim(), null, findOptions);
            Aspose.Cells.Cell foundCell;
            serialfind();

            void serialfind()
            {
                for (int i = 0; i < SrNos.Count(); i++)
                {
                    findOptions = new FindOptions();
                    findOptions.CaseSensitive = false;
                    findOptions.LookInType = LookInType.Values;
                    foundCell = cells.Find(SrNos[i].Trim(), null, findOptions);
                    if (foundCell != null)
                    {
                        rownumExcelSrNo = foundCell.Row;
                        string name = foundCell.Name;
                        break;
                    }
                }
            }

            string[] taxablevalue = new string[] { "Total Taxable Amount", "Total Taxable", "Total taxable amount", "Taxable Amount" };
         
            Aspose.Cells.Cell foundCell2;
            taxablevaluefind();

            void taxablevaluefind()
            {
                for (int i = 0; i < taxablevalue.Count(); i++)
                {
                    findOptions = new FindOptions();
                    findOptions.CaseSensitive = false;
                    findOptions.LookInType = LookInType.Values;
                    foundCell2 = cells.Find(taxablevalue[i].Trim(), null, findOptions);
                    if (foundCell2 != null)
                    {
                        rowNumExcelTaxval = foundCell2.Row;
                        string name = foundCell2.Name;
                        break;
                    }
                }
            }

            List<int> termsList = new List<int>();

            for (int i = rownumExcelSrNo + 3; i < rowNumExcelTaxval + 1; i++)
            {

                termsList.Add(i);
            }


            // int column = 15; //this is fixed 
            int row = termsList.ElementAt(0);     // this is fixed
            int rows = termsList.Last();
            Aspose.Cells.Cell rows66 = cells.GetRow(row).GetCellOrNull(1);
            //Get the last cell in the 4th column (D)
            Aspose.Cells.Cell endCell = cells.EndCellInColumn(18);
            //Last row to loop through for D column
            // int maxrow = endCell.Row;
            double SrNo;
            //double val1;
            double qty;
            double Rate;
            double Discount;
            double Taxable;
            double cgstRate;
            double cgstAmount;
            double scgstRate;
            double scgstAmount;
            double icsgtRate;
            double icgstAmount;
            double Total;

            string Desc;
            string HSN;

            string UOM;

            int j = 0;

            for (int i = termsList.ElementAt(j) - 1; i <= rows - 1; i++)
            {

                SrNo = cells[i, 0].DoubleValue;  //A.. SR No

                Desc = cells[i, 1].StringValue; //B... ---Descript

                HSN = cells[i, 2].StringValue; //C...Descript ion of Goods..

                qty = cells[i, 3].DoubleValue; //D... -----qty

                // HSN = cells[i, 4].StringValue;  //E......HSN Code(G ST) value

                UOM = cells[i, 5].StringValue;  //F...UOM

                Rate = cells[i, 6].DoubleValue; //G... Rate

                Discount = cells[i, 7].DoubleValue; //H...Discount

                Taxable = cells[i, 8].DoubleValue; //I...Taxable

                //UOM = cells[i, 9].StringValue; //J...UOM

                cgstRate = cells[i, 10].DoubleValue; //K..cgstRate

                cgstAmount = cells[i, 11].DoubleValue; //L.. cgstAmount...

                scgstRate = cells[i, 12].DoubleValue; //M..scgstRate

                scgstAmount = cells[i, 13].DoubleValue; //N..scgstAmount t...

                icsgtRate = cells[i, 14].DoubleValue; //O..icsgtRate

                icgstAmount = cells[i, 15].DoubleValue; //P..icgstAmount value,,,

                // val1 = cells[i, 16].DoubleValue; //Q..

                Total = cells[i, 17].DoubleValue; //R..Total 
                j++;
            }

        }

        public  DataTable ExtractPdfTables(string pathToExcel, string[] findtext1, string[] findtext2)
        {
            DataSet dsPDFTables = new DataSet();
            DataTable dataTable = new DataTable();
            DataTable dtInvoicedata = new DataTable();
            dtInvoicedata.Columns.AddRange(new DataColumn[15] { new DataColumn("SR.No", typeof(string)),
                            new DataColumn("Desc", typeof(string)),
                            new DataColumn("HSN",typeof(string)),  new DataColumn("qty", typeof(string)),  new DataColumn("UOM", typeof(string)),
                  new DataColumn("Rate", typeof(string)),  new DataColumn("Discount", typeof(string)), new DataColumn("Taxable", typeof(string)),
                new DataColumn("CGSTRate", typeof(string)),
                new DataColumn("CGstAmount", typeof(string)), new DataColumn("ScgstRate", typeof(string)), new DataColumn("ScgstAmount", typeof(string)),
             new DataColumn("IgstRate", typeof(string)),  new DataColumn("IgstAmount", typeof(string)),  new DataColumn("Total", typeof(string))});


            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(pathToExcel);
            Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];

            Aspose.Cells.Cells cells = sheet.Cells;
            //Suppose you need to add a text into the cells.
            //You need to find out if this text is already there in some cells.
            //You may use Find and Search options provided by Aspose.Cells APIs for this purpose.
            string[] SrNos = new string[] { "SNo.", "Serial No.", "Sr. No.", "Serial Number." };

            // string text = "Sr. No.";
            FindOptions findOptions = new FindOptions();
            findOptions.CaseSensitive = false;
            findOptions.LookInType = LookInType.Values;
            // Aspose.Cells.Cell foundCell = cells.Find(text.Trim(), null, findOptions);
            Aspose.Cells.Cell foundCell;
            serialfind();

            void serialfind()
            {
                for (int i = 0; i < findtext1.Count(); i++)
                {
                    findOptions = new FindOptions();
                    findOptions.CaseSensitive = false;
                    findOptions.LookInType = LookInType.Values;
                    foundCell = cells.Find(findtext1[i].Trim(), null, findOptions);
                    if (foundCell != null)
                    {
                        rownumExcelSrNo = foundCell.Row;
                        string name = foundCell.Name;
                        break;
                    }
                }
            }

            string[] taxablevalue = new string[] { "Total Taxable Amount", "Total Taxable", "Total taxable amount", "Taxable Amount" };
            // string taxablevalue = "Total Taxable Amount";
            // Aspose.Cells.Cell foundCell2 = cells.Find(taxablevalue.Trim(), null, findOptions);
            Aspose.Cells.Cell foundCell2;
            taxablevaluefind();

            void taxablevaluefind()
            {
                for (int i = 0; i < findtext2.Count(); i++)
                {
                    findOptions = new FindOptions();
                    findOptions.CaseSensitive = false;
                    findOptions.LookInType = LookInType.Values;
                    foundCell2 = cells.Find(findtext2[i].Trim(), null, findOptions);
                    if (foundCell2 != null)
                    {
                        rowNumExcelTaxval = foundCell2.Row;
                        string name = foundCell2.Name;
                        break;
                    }
                }
            }

            List<int> termsList = new List<int>();


            for (int i = rownumExcelSrNo + 3; i < rowNumExcelTaxval + 1; i++)
            {

                termsList.Add(i);
            }

            if (termsList.Count > 0)
            {
                // int column = 15; //this is fixed 
                int row = termsList.ElementAt(0);     // this is fixed
                int rows = termsList.Last();

                Aspose.Cells.Cell rows66 = cells.GetRow(row).GetCellOrNull(1);

                //Get the last cell in the 4th column (D)
                Aspose.Cells.Cell endCell = cells.EndCellInColumn(18);
                //Last row to loop through for D column
                string qty = "";
                string Rate = "";
                string Discount = "";
                string Taxable = "";
                string cgstRate = "";
                string cgstAmount = "";
                string scgstRate = "";
                string scgstAmount = "";
                string icsgtRate = "";
                string icgstAmount = "";
                string Total = "";
                string Desc = "";
                string HSN = ""; string cell4 = ""; string UOM = ""; string cell9 = ""; string Cell16 = ""; string srNo = "";
                int j = 0;

                for (int i = termsList.ElementAt(j) - 1; i <= rows - 1; i++)
                {

                //    for (int i = termsList.ElementAt(j); i <= rows; i++)
                //{

                    if (!cells[i, 0].Equals("") && cells[i, 0] != null && cells[i, 0].Type == CellValueType.IsNumeric)
                    {
                        srNo = Convert.ToString(cells[i, 0].DoubleValue);  //A.. SR No
                    }
                    else if (!cells[i, 0].Equals("") && cells[i, 0] != null && cells[i, 0].Type == CellValueType.IsString)
                    {
                        srNo = cells[i, 0].StringValue; //Q..

                    }

                    if (!cells[i, 1].Equals("") && cells[i, 1] != null && cells[i, 1].Type == CellValueType.IsString)
                    {
                        Desc = cells[i, 1].StringValue; //B... ---Descript
                    }
                    else if (!cells[i, 1].Equals("") && cells[i, 1] != null && cells[i, 1].Type == CellValueType.IsNumeric)
                    {
                        Desc = Convert.ToString(cells[i, 1].DoubleValue); //B... ---Descript
                    }

                    if (!cells[i, 2].Equals("") && cells[i, 2] != null && cells[i, 2].Type == CellValueType.IsString)
                    {
                        HSN = cells[i, 2].StringValue; //C...Descript ion of Goods..
                    }
                    else if (!cells[i, 2].Equals("") && cells[i, 2] != null && cells[i, 2].Type == CellValueType.IsNumeric)
                    {
                        HSN = Convert.ToString(cells[i, 2].DoubleValue); 
                    }

                    if (!cells[i, 3].Equals("") && cells[i, 3] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        qty = Convert.ToString(cells[i, 3].DoubleValue); //D... -----qty
                    }
                    else if (!cells[i, 3].Equals("") && cells[i, 3] != null && cells[i, 3].Type == CellValueType.IsNumeric)
                    {
                        qty = Convert.ToString(cells[i, 3].DoubleValue); 
                    }

                    if (!cells[i, 4].Equals("") && cells[i, 4] != null && cells[i, 4].Type == CellValueType.IsString)
                    {
                        cell4 = cells[i, 4].StringValue;  //E......HSN Code(G ST) value
                    }
                    else if (!cells[i, 4].Equals("") && cells[i, 4] != null && cells[i, 4].Type == CellValueType.IsNumeric)
                    {
                        cell4 = Convert.ToString(cells[i, 4].DoubleValue); 
                    }

                    if (!cells[i, 5].Equals("") && cells[i, 5] != null && cells[i, 5].Type == CellValueType.IsString)
                    {
                        UOM = cells[i, 5].StringValue;  //F...UOM
                    }
                    else if (!cells[i, 5].Equals("") && cells[i, 5] != null && cells[i, 5].Type == CellValueType.IsNumeric)
                    {
                        UOM = Convert.ToString(cells[i, 5].DoubleValue);
                    }

                    if (!cells[i, 6].Equals("") && cells[i, 6] != null && cells[i, 6].Type == CellValueType.IsString)
                    {
                        Rate = Convert.ToString(cells[i, 6].DoubleValue); //G... Rate
                    }
                    else if (!cells[i, 6].Equals("") && cells[i, 6] != null && cells[i, 6].Type == CellValueType.IsNumeric)
                    {
                        Rate = Convert.ToString(cells[i, 6].DoubleValue);
                    }

                    if (!cells[i, 7].Equals("") && cells[i, 7] != null && cells[i, 7].Type == CellValueType.IsString)
                    {
                        Discount = Convert.ToString(cells[i, 7].DoubleValue); //H...Discount
                    }
                    else if (!cells[i, 7].Equals("") && cells[i, 7] != null && cells[i, 7].Type == CellValueType.IsNumeric)
                    {
                        Discount = Convert.ToString(cells[i, 7].DoubleValue);
                    }

                    if (!cells[i, 8].Equals("") && cells[i, 8] != null && cells[i, 8].Type == CellValueType.IsString)
                    {
                        Taxable = Convert.ToString(cells[i, 8].DoubleValue); //I...Taxable
                    }
                    else if (!cells[i, 8].Equals("") && cells[i, 8] != null && cells[i, 8].Type == CellValueType.IsNumeric)
                    {
                        Taxable = Convert.ToString(cells[i, 8].DoubleValue);
                    }

                    if (!cells[i, 9].Equals("") && cells[i, 9] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        cell9 = Convert.ToString(cells[i, 9].StringValue); //J...UOM
                    }
                    else if (!cells[i, 9].Equals("") && cells[i, 9] != null && cells[i, 9].Type == CellValueType.IsNumeric)
                    {
                        cell9 = Convert.ToString(cells[i, 9].DoubleValue);
                    }

                    if (!cells[i, 10].Equals("") && cells[i, 10] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        cgstRate = Convert.ToString(cells[i, 10].DoubleValue); //K..cgstRate
                    }
                    else if (!cells[i, 10].Equals("") && cells[i, 10] != null && cells[i, 10].Type == CellValueType.IsNumeric)
                    {
                        cgstRate = Convert.ToString(cells[i, 10].DoubleValue);
                    }

                    if (!cells[i, 11].Equals("") && cells[i, 11] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        cgstAmount = Convert.ToString(cells[i, 11].DoubleValue); //L.. cgstAmount...
                    }
                    else if (!cells[i, 11].Equals("") && cells[i, 11] != null && cells[i, 11].Type == CellValueType.IsNumeric)
                    {
                        cgstAmount = Convert.ToString(cells[i, 11].DoubleValue);
                    }

                    if (!cells[i, 12].Equals("") && cells[i, 12] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        scgstRate = Convert.ToString(cells[i, 12].DoubleValue); //M..scgstRate
                    }
                    else if (!cells[i, 12].Equals("") && cells[i, 12] != null && cells[i, 12].Type == CellValueType.IsNumeric)
                    {
                        scgstRate = Convert.ToString(cells[i, 12].DoubleValue);
                    }

                    if (!cells[i, 13].Equals("") && cells[i, 13] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        scgstAmount = Convert.ToString(cells[i, 13].DoubleValue); //N..scgstAmount t...
                    }
                    else if (!cells[i, 13].Equals("") && cells[i, 13] != null && cells[i, 13].Type == CellValueType.IsNumeric)
                    {
                        scgstAmount = Convert.ToString(cells[i, 13].DoubleValue);
                    }

                    if (!cells[i, 14].Equals("") && cells[i, 14] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        icsgtRate = Convert.ToString(cells[i, 14].DoubleValue); //O..icsgtRate
                    }
                    else if (!cells[i, 14].Equals("") && cells[i, 14] != null && cells[i, 14].Type == CellValueType.IsNumeric)
                    {
                        icsgtRate = Convert.ToString(cells[i, 14].DoubleValue);
                    }

                    if (!cells[i, 15].Equals("") && cells[i, 15] != null && cells[i, 3].Type == CellValueType.IsString)
                    {
                        icgstAmount = Convert.ToString(cells[i, 15].DoubleValue); //P..icgstAmount value,,,
                    }
                    else if (!cells[i, 15].Equals("") && cells[i, 15] != null && cells[i, 15].Type == CellValueType.IsNumeric)
                    {
                        icgstAmount = Convert.ToString(cells[i, 15].DoubleValue);
                    }

                    if (!cells[i, 16].Equals("") && cells[i, 16] != null && cells[i, 16].Type == CellValueType.IsNumeric)
                    {
                        Cell16 = Convert.ToString(cells[i, 16].DoubleValue); //Q..
                    }
                    else if (!cells[i, 16].Equals("") && cells[i, 16] != null && cells[i, 16].Type == CellValueType.IsString)
                    {
                        Cell16 = cells[i, 16].StringValue; //Q..
                    }

                    if (!cells[i, 17].Equals("") && cells[i, 17] != null && cells[i, 17].Type == CellValueType.IsString)
                    {
                        Total = Convert.ToString(cells[i, 17].DoubleValue); //R..Total 
                    }
                    else if (!cells[i, 17].Equals("") && cells[i, 17] != null && cells[i, 17].Type == CellValueType.IsNumeric)
                    {
                        Total = Convert.ToString(cells[i, 17].DoubleValue);
                    }
                   
                    dtInvoicedata.Rows.Add(srNo, Desc, HSN, qty, UOM, Rate, Discount, Taxable, cgstRate, cgstAmount, scgstRate, scgstAmount, icsgtRate, icgstAmount, Total);


                    j++;

                }


                //SP: Remove Null Columns from datatable..

                var columns = dtInvoicedata.Columns.Cast<DataColumn>().ToArray();
                foreach (var col in columns)
                {
                ////    // check column values for null 
                  if ( dtInvoicedata.AsEnumerable().All(dr => dr.IsNull(col) ))
                  {
                //       // remove all null value columns 
                       dtInvoicedata.Columns.Remove(col);
                  }

                }


                dataTable =  SupressEmptyColumns(dtInvoicedata);

                dsPDFTables.Tables.Add(dtInvoicedata);
            
            }
            else
            {
                return null;
            }

            return dataTable;
        }



        private DataTable SupressEmptyColumns(DataTable dtSource)
        {
            //the DataTable is dynamic, loop thru each col and thru each row to
            // determine if that column is empty.
            System.Collections.ArrayList columnsToRemove = new System.Collections.ArrayList();
            foreach (DataColumn dc in dtSource.Columns)
            {
                bool colEmpty = true;
                foreach (DataRow dr in dtSource.Rows)
                {
                    if (Convert.ToString(dr[dc.ColumnName]) != string.Empty)
                    {
                        colEmpty = false;
                    }
                }
                if (colEmpty == true)
                {
                    columnsToRemove.Add(dc.ColumnName);
                }
            }
            //remove all columns that are empty
            foreach (string columnName in columnsToRemove)
            {
                dtSource.Columns.Remove(columnName);
            }
            return dtSource;
        }



        private static void RemoveUnusedColumnsAndRows(DataTable table)
        {
           
            foreach (var column in table.Columns.Cast<DataColumn>().ToArray())
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(column)))
                    table.Columns.Remove(column);
            }
            table.AcceptChanges();
        }
     
        private Microsoft.Office.Interop.Excel.Range GetSpecifiedRange(string matchStr, Microsoft.Office.Interop.Excel.Worksheet objWs)
        {
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Range currentFind = null;
            Microsoft.Office.Interop.Excel.Range firstFind = null;
            currentFind = objWs.get_Range("A1", "AM100").Find(matchStr, missing,
                           Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues,
                           Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                           Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows,
                           Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, missing, missing);
            return currentFind;
        }
    
    }
      
 }



