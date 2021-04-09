using CommonLibrary.Interfaces;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using org.apache.pdfbox.util;
using System.Net;
using HtmlAgilityPack;

namespace InternetExplorer
{
    [Designer(typeof(OpenPDF1))]
    public class OpenPDF : NativeActivity, IOpenApplicationInterface
    {
       // [RequiredArgument]
       // public InArgument<string> ApplicationID { get; set; }
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        [RequiredArgument]
        public InArgument<string> MovePath { get; set; }

        [RequiredArgument]
        public string findtablebyText1 { get; set; }

        [RequiredArgument]
        public string findtablebyText2 { get; set; }

        [RequiredArgument]

        [DisplayName("DataSet Output Value")]
        //  [Description("Get Control Value")]
        public OutArgument<DataTable> DataSetValue { get; set; }

        [DisplayName("String Value")]
        //  [Description("Get Control Value")]
        public OutArgument<string> StringValue { get; set; }
        // public string UniqueApplicationID { get; set; }

        public OutArgument<bool> LaunchResult { get; set; }
       
        public OpenPDF()
        {
            
        }

        internal string ReadPdfAsText(string filePath)
        {
            // throw new NotImplementedException();
            org.apache.pdfbox.pdmodel.PDDocument doc = null;

            doc = org.apache.pdfbox.pdmodel.PDDocument.load(filePath);
            PDFTextStripper stripper = new PDFTextStripper();
            Console.WriteLine(stripper.getText(doc));
            return stripper.getText(doc);

            // return string.Empty;
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        /// 

        async private void GetTransactions()
        {
           string url = "https://www.tutorialspoint.com/html/html_tables.htm";
            string html;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse x = await req.GetResponseAsync();
                HttpWebResponse res = (HttpWebResponse)x;
                if (res != null)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Stream stream = res.GetResponseStream();
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            html = reader.ReadToEnd();
                        }
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(html);

                        var tsTable = htmlDocument.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes["div"].
                                 ChildNodes["div"].ChildNodes["div"].ChildNodes["table"].InnerHtml;

                        int n = 2;
                        var tsRow = tsTable.Split(Environment.NewLine.ToCharArray()).Skip(n).ToArray();

                        for (var index = 1; index < tsRow.Count(); index++)
                        {

                        }
                    }
                }
            }
            catch
            {
                  
                     // ("A tear occured in the space-time continuum. Please try again when all planets in the solar system are aligned.");
            }
        }


        protected override void Execute(NativeActivityContext context)
        {
            PDFHelper pDFHelper = new PDFHelper();


            //PdfDocument doc = new PdfDocument();

            //doc.LoadFromFile(FilePath);

            //doc.SaveToFile("PDFtoDoc.doc", FileFormat.DOC);

            //System.Diagnostics.Process.Start("PDFtoDoc.doc");


            // Load PDF document
            //  Aspose.Pdf.Document doc = new Aspose.Pdf.Document(@"C:\input.pdf");
            // Instantiate ExcelSave Option object
            //Aspose.Pdf.ExcelSaveOptions excelsave = new ExcelSaveOptions();
            // Save the output in XLS format
            // doc.Save("c:/resultant.xls", excelsave);
           // GetTransactions();

            string pathToPdf = FilePath.Get(context);
          //  string PathToExcel = MovePath.Get(context);
           // string pathexcel = "";

           // pathexcel = After(pathToPdf, "PDFFiles");

           // string subPath1 = @"C:\\Processed"; // your code goes here

            // If directory does not exist, create it.
            FileInfo fileInfo = new FileInfo(pathToPdf);
            string directoryFullPath = fileInfo.DirectoryName;

            //if (!Directory.Exists(subPath1))
            //{

            //    Directory.CreateDirectory(subPath1);

            //}

           // string moveexcel = PathToExcel + pathexcel;
            


            string After(string value, string a)
            {
                int posA = value.LastIndexOf(a);
                if (posA == -1)
                {
                    return "";
                }
                int adjustedPosA = posA + a.Length;
                if (adjustedPosA >= value.Length)
                {
                    return "";
                }
                char[] MyChar = { '\\', ':', '\\', ' ' };
                string result = "";
                // string NewString = MyString.TrimStart(MyChar);
                int index = value.IndexOf(" ");
                string after = value.Substring(adjustedPosA);

                String searchVal = after.TrimStart(MyChar);
                
                // Console.WriteLine("New Striiiiiiiiiiiiiiiiiiiiiiiiiiingg....." + value.Substring(adjustedPosA).TrimEnd(MyChar));
                return searchVal;
            }



            string pathToExcel = Path.ChangeExtension(pathToPdf, ".xlsx");


            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();


            f.ExcelOptions.ConvertNonTabularDataToSpreadsheet = false;


            f.ExcelOptions.PreservePageLayout = true;

            f.OpenPdf(pathToPdf);

            if (f.PageCount > 0)
            {
               int result = f.ToExcel(pathToExcel);

            //    //Open a produced Excel workbook
               if (result == 0)
               {
                   //System.Diagnostics.Process.Start(pathToExcel);
               }
            }
            f.ClosePdf();

            
            string[] FindtablebyText1 = new string[] { "" };

            FindtablebyText1 = findtablebyText1.Split(',');

            string[] FindtablebyText2 = new string[] { "" };
            FindtablebyText2 = findtablebyText2.Split(',');

           // string path = "C:\\Users\\Admin\\Desktop\\BOTDesignerMaster\\BOTDesignerMaster\\PDFFiles\\Invoice-0000001.xlsx";

            DataTable dspdfTables =   pDFHelper.ExtractPdfTables(pathToExcel, FindtablebyText1, FindtablebyText2);

            context.SetValue(DataSetValue, dspdfTables);
            string strPDFText = pDFHelper.ReadPdfAsText(FilePath.Get(context));

            context.SetValue(StringValue, strPDFText);

            //SP:23072018.. Move PDF File After Reading.....
           // string destinationFile = MovePath.Get(context);

           // string subPath = @"C:\\Completed"; // your code goes here

            // If directory does not exist, create it.

            //if (!Directory.Exists(subPath))
            //{

            //    Directory.CreateDirectory(subPath);

            //}

            //string pdfMove = "C:\\Completed\\" + pathexcel;
            //System.IO.File.Move(pathToPdf, pdfMove);


            // CBHAReportGenMgr mcbhareport = new CBHAReportGenMgr();

            // char[] MyChar = { '\r', '.', ':', '\\', ' ' };



            // 


            // pDFHelper.SearchText();


            // string[] sgstNos = new string[] { "GSTIN No", "GST No", "GST ", "GST Number." };
            // string[] sInvoiceNos = new string[] { "Invoice Serial Number", "Invoice Number", "Invoice Serial No ", "Invoice No" };

            // string[] sstcode = new string[] { "State Code", "State Code"};
            // string[] sinvDate = new string[] { "Invoice Date", "Invoice date" };

            // string[] splaceofsup = new string[] { "Place OF Supply", "Place OF Supply" };
            // string[] staxableAmount = new string[] { "Total Taxable Amount", "Total Taxable" };
            // string[] sTotaltax = new string[] { "Total Tax", "Total tax" };
            // string[] sloadingChg = new string[] { "Loading and Packing Charges", "Loading and Packing Chg" };
            // string[] sCess = new string[] { "Cess Total", "cess total" };
            // string[] sInsuchg = new string[] { "Insurance Charges", "Insurance Chg" };
            // string[] sOtherchg = new string[] { "Other Charges", "Other Chg" };
            // string[] sAmountoftax = new string[] { "Amount of Tax Subject to Reverse Charge", "Amount of Tax Subject to Reverse Chg" };
            // string[] sInvTotal = new string[] { "Invoice Total", "Invoice Total." };
            // string[] stermandcond = new string[] { "YOUR TERM & CONDITION OF SALE", "YOUR TERM & CONDITION" };




            // ReadInvoiceFields readInvoiceFields = new ReadInvoiceFields();
            // string gstNo, InvserNo, stcode, invdate, placeofsup, totalTaxamount, totaltax, loadingchg, cess, insuchg, otherchg, amountoftax, invTotal, companyname;



            // gstNo =  gstNofind();
            // readInvoiceFields.GSTINNo = gstNo;

            // string gstNofind()
            // {
            //     for (int i = 0; i < sgstNos.Count(); i++)
            //     {
            //         gstNo = SubstringExtensions.After(strPDFText, sgstNos[i].Trim()).TrimEnd(MyChar);
            //         if (gstNo != null)
            //         {

            //             return gstNo;
            //         }

            //     }
            //     return "";
            // }


            // InvserNo =  invNofind();
            // readInvoiceFields.InvoiceNo = InvserNo;
            // string invNofind()
            // {
            //     for (int i = 0; i < sInvoiceNos.Count(); i++)
            //     {
            //         InvserNo = SubstringExtensions.After(strPDFText, sInvoiceNos[i].Trim()).TrimEnd(MyChar);
            //         if (InvserNo != null)
            //         {

            //             return InvserNo;
            //         }
            //     }
            //     return "";
            // }


            // stcode =  stCodefind();
            // readInvoiceFields.StateCode = stcode;
            // string stCodefind()
            // {
            //     for (int i = 0; i < sstcode.Count(); i++)
            //     {
            //         stcode = SubstringExtensions.After(strPDFText, sstcode[i].Trim()).TrimEnd(MyChar);
            //         if (stcode != null)
            //         {

            //             return stcode;
            //         }
            //     }
            //     return "";
            // }


            // invdate = InvDatefind();
            // readInvoiceFields.Invoicedate = invdate;
            // string InvDatefind()
            // {
            //     for (int i = 0; i < sinvDate.Count(); i++)
            //     {
            //         invdate = SubstringExtensions.After(strPDFText, sinvDate[i].Trim()).TrimEnd(MyChar);
            //         if (invdate != null)
            //         {

            //             return invdate;
            //         }
            //     }
            //     return "";
            // }



            // placeofsup = plcaeofsupfind();
            // readInvoiceFields.Placeofsup = placeofsup;
            // string plcaeofsupfind()
            // {
            //     for (int i = 0; i < splaceofsup.Count(); i++)
            //     {
            //         placeofsup = SubstringExtensions.After(strPDFText, splaceofsup[i].Trim()).TrimEnd(MyChar);
            //         if (placeofsup != null)
            //         {

            //             return placeofsup;
            //         }
            //     }
            //     return "";
            // }


            // totalTaxamount = taxaleAmountfind();
            // readInvoiceFields.TaxAmount = totalTaxamount;
            // string taxaleAmountfind()
            // {
            //     for (int i = 0; i < staxableAmount.Count(); i++)
            //     {
            //         totalTaxamount = SubstringExtensions.After(strPDFText, staxableAmount[i].Trim()).TrimEnd(MyChar);
            //         if (totalTaxamount != null)
            //         {

            //             return totalTaxamount;
            //         }
            //     }
            //     return "";
            // }



            // totaltax = totaltaxfind();
            // readInvoiceFields.TotalTax = totaltax;
            // string totaltaxfind()
            // {
            //     for (int i = 0; i < sTotaltax.Count(); i++)
            //     {
            //         totaltax = SubstringExtensions.After(strPDFText, sTotaltax[i].Trim()).TrimEnd(MyChar);
            //         if (totaltax != null)
            //         {

            //             return totaltax;
            //         }
            //     }
            //     return "";
            // }


            // loadingchg =  loadingfind();
            // readInvoiceFields.LoadingCharges = loadingchg;
            // string  loadingfind()
            // {
            //     for (int i = 0; i < sloadingChg.Count(); i++)
            //     {
            //         loadingchg = SubstringExtensions.After(strPDFText, sloadingChg[i].Trim()).TrimEnd(MyChar);
            //         if (loadingchg != null)
            //         {

            //             return loadingchg;
            //         }
            //     }
            //     return "";
            // }



            //cess =  cessfind();
            // readInvoiceFields.CessTotal = cess;
            // string cessfind()
            // {
            //     for (int i = 0; i < sCess.Count(); i++)
            //     {
            //         cess = SubstringExtensions.After(strPDFText, sCess[i].Trim()).TrimEnd(MyChar);
            //         if (cess != null)
            //         {

            //             return cess;
            //         }
            //     }
            //     return "";
            // }



            // insuchg =  insuchgfind();
            // readInvoiceFields.Insucharges = insuchg;
            // string insuchgfind()
            // {
            //     for (int i = 0; i < sInsuchg.Count(); i++)
            //     {
            //         insuchg = SubstringExtensions.After(strPDFText, sInsuchg[i].Trim()).TrimEnd(MyChar);
            //         if (insuchg != null)
            //         {

            //             return insuchg;
            //         }
            //     }
            //     return "";
            // }


            // otherchg =  otherchgfind();
            // readInvoiceFields.Othercharges = otherchg;
            // string otherchgfind()
            // {
            //     for (int i = 0; i < sOtherchg.Count(); i++)
            //     {
            //         otherchg = SubstringExtensions.After(strPDFText, sOtherchg[i].Trim()).TrimEnd(MyChar);
            //         if (otherchg != null)
            //         {

            //             return otherchg;
            //         }
            //     }
            //     return "";
            // }


            // amountoftax = Amountoftaxfind();
            // readInvoiceFields.AmountTax = amountoftax;
            // string Amountoftaxfind()
            // {
            //     for (int i = 0; i < sAmountoftax.Count(); i++)
            //     {
            //         amountoftax = SubstringExtensions.After(strPDFText, sAmountoftax[i].Trim()).TrimEnd(MyChar);
            //         if (amountoftax != null)
            //         {

            //             return amountoftax;
            //         }
            //     }
            //     return "";
            // }




            // invTotal = sInvTotalfind();
            // readInvoiceFields.Invoicetotal = invTotal;
            // string sInvTotalfind()
            // {
            //     for (int i = 0; i < sInvTotal.Count(); i++)
            //     {
            //         invTotal = SubstringExtensions.After(strPDFText, sInvTotal[i].Trim()).TrimEnd(MyChar);
            //         if (invTotal != null)
            //         {

            //             return invTotal;
            //         }
            //     }
            //     return "";
            // }





            // companyname = scompanynamefind();
            // readInvoiceFields.CompanyName = companyname;

            // string scompanynamefind()
            // {
            //     for (int i = 0; i < stermandcond.Count(); i++)
            //     { 
            //         companyname = SubstringExtensions.After(strPDFText, stermandcond[i].Trim());
            //         if (companyname != null)
            //         {

            //             return companyname;
            //         }
            //     }
            //     return "";
            // }

            // List<ReadInvoiceFields> listinvoiceFields = new List<ReadInvoiceFields>();
            // listinvoiceFields.Add(readInvoiceFields);




        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OpenWebApplication() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
