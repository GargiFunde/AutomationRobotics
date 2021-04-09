using CommonLibrary;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PDFExtractor;
using System.Diagnostics;
//using PdfSharp;
//using PdfSharp.Drawing;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
//using PdfSharp.Pdf.Advanced;
using System.Net;
using System.Net.Http;

namespace InternetExplorer
{
    [Designer(typeof(PDFFieldProperties1))]
    public class PDFFieldProperties : ActivityExtended
    {
        [RequiredArgument]
        public string FilePath { get; set; }
        public PDFFieldProperties() : base()
        {
            //SetPDFDataSetValue = new DataSet();
            ResultStringValue = new OutArgument<string>();
        }

        [DisplayName("Get Value")]
      //  [Description("Get Control Value")]
        public OutArgument<Object> GetControlValue { get; set; }

        
        [DisplayName("Set PDF DataSet Value")]
      //  [Description("Set Value to control")]
        public InArgument<DataTable> SetPDFDataSetValue { get; set; }

        [DisplayName("Set PDF Text Value")]
        //  [Description("Set Value to control")]
        public InArgument<string> SetPDFTextValue { get; set; }

        private InArgument<int> rownumber;
        [DisplayName("Row Number")]
        // [Description("Previous Sibling Id")]
        [CategoryAttribute("By Table [Priority:1]")]
        public InArgument<int> RowNumber { get; set; }


        [DisplayName("Column Number")]
        // [Description("Previous Sibling Id")]
        [CategoryAttribute("By Table [Priority:1]")]
        public InArgument<int> ColumnNumber { get; set; }


        [DisplayName("Previous Text")]
        // [Description("Previous Sibling Id")]
        [CategoryAttribute("By Text [Priority:2]")]
        public InArgument<string> PreviousText { get; set; }


        [DisplayName("Search Text")]
        //  [Description("Previous Sibling Name")]
        [CategoryAttribute("By Text [Priority:2]")]
        public InArgument<string> TextToSearch { get; set; }


        [DisplayName("Next Text")]
        //  [Description("Previous Sibling Name")]
        [CategoryAttribute("By Text [Priority:2]")]
        public InArgument<string> NextText { get; set; }
               
        [DisplayName("Text Length")]
        //  [Description("Previous Sibling Name")]
        [CategoryAttribute("By Text [Priority:2]")]
        public InArgument<int> TextLength { get; set; }
               
        [DisplayName("Image Path [Priority:11]")]
        //   [Description("XPath to find control")]

        public InArgument<string> ImagePath { get; set; }

        [DisplayName("Result String")]
        //  [Description("Get Control Value")]
        public OutArgument<string> ResultStringValue { get; set; }

       // public string strpdfText;
        string strPDFText = "";
        protected override void Execute(NativeActivityContext context)
        {
            DataTable dsTables = SetPDFDataSetValue.Get(context);
            //strpdfText = SetPDFTextValue.Get(context);
            PDFHelper pDFHelper = new PDFHelper();
           
            if (strPDFText == "" || strPDFText == null)
            {
                strPDFText = SetPDFTextValue.Get(context);
            }


            string[] values = strPDFText.Split(',');


            //string[] words;
            //words = strPDFText.Split('\n');


            //DataTable tbl = new DataTable();
            //tbl.Columns.Add("Col1");


            //string[] array = strPDFText.Split(',');

            //foreach (string s in array)
            //{
            //    DataRow row = tbl.NewRow();
            //    string[] numb = s.Split(',');
            //    row["Col1"] = numb[0];
            //    // row["Col1"] = numb[1];

            //    tbl.Rows.Add(row);
            //}   //Your code goes here



            //for (int j = 0; j < tbl.Rows.Count; j++)
            //{
            //    for (int i = 0; i < tbl.Columns.Count; i++)
            //    {
            //        Console.Write(tbl.Columns[i].ColumnName + " ");
            //        Console.WriteLine(tbl.Rows[j].ItemArray[i]);
            //    }
            //}

            //const string format = "xlsx-single";
            //const string apiKey = "r4i5cvh74tvn";
            //const string uploadURL = "https://pdftables.com/api?key=" + apiKey + "&format=" + format;
            //string excel = "Invoice.xlsx";

            //var task = PDFToExcel(FilePath, excel);
            //task.Wait();

            //Console.WriteLine("Response status {0} {1}", (int)task.Result, task.Result);

            //if ((int)task.Result != 200)
            //{
            //    return 1;
            //}

           // Console.WriteLine("Written {0} bytes", new System.IO.FileInfo(args[1]).Length);


            // async Task<HttpStatusCode> PDFToExcel(string pdfFilename, string xlsxFilename)
            //{
            //    using (var f = System.IO.File.OpenRead(pdfFilename))
            //    {
            //        var client = new HttpClient();
            //        var mpcontent = new MultipartFormDataContent();
            //        mpcontent.Add(new StreamContent(f));

            //        using (var response = await client.PostAsync(uploadURL, mpcontent))
            //        {
            //            if ((int)response.StatusCode == 200)
            //            {
            //                using (
            //                    Stream contentStream = await response.Content.ReadAsStreamAsync(),
            //                    stream = File.Create(xlsxFilename))
            //                {
            //                    await contentStream.CopyToAsync(stream);
            //                }
            //            }
            //            return response.StatusCode;
            //        }
            //    }
            //}

            //const string filename = "Invoice6.pdf";
            //File.Copy(Path.Combine("C:\\Users\\Admin\\Desktop\\BOTDesignerMaster\\BOTDesignerMaster\\PDFFiles\\", filename),
            //Path.Combine(Directory.GetCurrentDirectory(), filename), true);
            //PdfDocument document = PdfReader.Open(filename);
            //PdfDictionary dict = new PdfDictionary(document);
            //dict.Elements["/S"] = new PdfName("/GoTo");
            //PdfArray array1 = new PdfArray(document);
            //dict.Elements["/D"] = array1;
            //PdfReference iref = PdfInternals.GetReference(document.Pages[0]);
            //array1.Elements.Add(iref);
            //array1.Elements.Add(new PdfName("/FitV"));
            //array1.Elements.Add(new PdfInteger(-32768));
            //document.Internals.AddObject(dict);
            //document.Internals.Catalog.Elements["/OpenAction"] =
            //PdfInternals.GetReference(dict);
            //document.Save(filename);
            //Process.Start(filename);





            //try
            //{
            //    // Construct the datatable programetically 
            //   // DataTable datatable = MakeDataTable();

            //    string appPath = AppDomain.CurrentDomain.BaseDirectory;

            //    string excelFileName = appPath + @"\TestFile.xlsx";

            //    ExportDataSet(tbl, excelFileName);

            //    Console.Write("\n\nDatatable exported to Excel file successfully");
            //}
            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}


            //DataTable MakeDataTable()
            //{
            //    // Create a new DataTable. 
            //    System.Data.DataTable table = new DataTable("ParentTable");
            //    // Declare variables for DataColumn and DataRow objects. 
            //    DataColumn column;
            //    DataRow row;

            //    // Create new DataColumn, set DataType,  
            //    // ColumnName and add to DataTable.     
            //    column = new DataColumn();
            //    column.DataType = System.Type.GetType("System.Int32");
            //    column.ColumnName = "id";
            //    column.ReadOnly = true;
            //    column.Unique = true;
            //    // Add the Column to the DataColumnCollection. 
            //    table.Columns.Add(column);

            //    // Create second column. 
            //    column = new DataColumn();
            //    column.DataType = System.Type.GetType("System.String");
            //    column.ColumnName = "ParentItem";
            //    column.AutoIncrement = false;
            //    column.Caption = "ParentItem";
            //    column.ReadOnly = false;
            //    column.Unique = false;
            //    // Add the column to the table. 
            //    table.Columns.Add(column);

            //    // Make the ID column the primary key column. 
            //    DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            //    PrimaryKeyColumns[0] = table.Columns["id"];
            //    table.PrimaryKey = PrimaryKeyColumns;

            //    // Create three new DataRow objects and add  
            //    // them to the DataTable 
            //    for (int i = 0; i <= 2; i++)
            //    {
            //        row = table.NewRow();
            //        row["id"] = i;
            //        row["ParentItem"] = "ParentItem " + i;
            //        table.Rows.Add(row);
            //    }

            //    return table;
            //}

            //void ExportDataSet(DataTable table, string excelFileName)
            //{
            //    using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(excelFileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            //    {
            //        var workbookPart = workbook.AddWorkbookPart();

            //        workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //        workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

            //        var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            //        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            //        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

            //        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            //        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

            //        uint sheetId = 1;
            //        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
            //        {
            //            sheetId =
            //                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            //        }

            //        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
            //        sheets.Append(sheet);

            //        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();


            //        // Construct column names 
            //        List<String> columns = new List<string>();
            //        foreach (System.Data.DataColumn column in table.Columns)
            //        {
            //            columns.Add(column.ColumnName);

            //            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            //            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            //            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
            //            headerRow.AppendChild(cell);
            //        }

            //        // Add the row values to the excel sheet 
            //        sheetData.AppendChild(headerRow);

            //        foreach (System.Data.DataRow dsrow in table.Rows)
            //        {
            //            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
            //            foreach (String col in columns)
            //            {
            //                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            //                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            //                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
            //                newRow.AppendChild(cell);
            //            }

            //            sheetData.AppendChild(newRow);
            //        }
            //    }
            //}





            string searchVals = gstNofind(values, strPDFText);

            if(searchVals == "")
            {
                searchVals = "NULL";
            }
           
            context.SetValue(ResultStringValue, searchVals);
            GetControlValue.Set(context, searchVals);
            //string sInvoiceNos = gstNofind(values, strPdfText);
            // context.SetValue(ResultStringValue, sInvoiceNos);
            //string returnvalue = string.Empty;
            //string value =  string.Empty;
            //try
            //{


            //    Thread.Sleep(500);
            //    try
            //    {
            //        value = SetControlValue.Get(context).ToString();
            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //    PerformActionOncontrol(context, ref returnvalue, ref value);

            //    if ((returnvalue != string.Empty) && (returnvalue != null) && (returnvalue.Trim().Length > 0))
            //    {
            //        GetControlValue.Set(context, returnvalue);
            //    }

            //    //});
            //}
            //catch (Exception ex)
            //{

            //}
        }
      //  bool UseNextPriority = false;
        
        //public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach)
        //{

            
           

        //}
              
        string gstNofind(string[] sgstNos, string strPDFText)
        {
            string texttosearch = TextToSearch.Expression.ToString();
            char[] MyChar = { '\r', '.', ':', '\\', ' ' };
            string[] stringArray = new string[] { "" };
            stringArray = texttosearch.Split(',');

            string gstNo = string.Empty;
            for (int i = 0; i < stringArray.Count(); i++)
            {
                gstNo = SubstringExtensions.After(strPDFText, stringArray[i].Trim()).TrimEnd(MyChar);
                if (gstNo != null)
                {

                    return gstNo;
                }

            }
            return "";
        }
    }
}
