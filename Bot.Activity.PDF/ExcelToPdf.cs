///-----------------------------------------------------------------
///   Namespace:      Bot.Activity.PDF
///   Class:               ExcelToPdf
///   Description:    <para>Convert Excel To PDF </para>
///   Author:           E2E BOTS Team                   Date: 02 March,2020
///   Notes:            Convert Excel To PDF
///   Revision History: 2020.1.2.3
///   Name:           Date:        Description:
///-----------------------------------------------------------------


using System;
using Microsoft.Office.Interop.Excel;
using System.Activities;
using System.ComponentModel;
using Logger;
using System.Drawing;

namespace Bot.Activity.PDF
{
    [ToolboxBitmap("Resources/ExcelToPdf.png")]
    [Designer(typeof(ActivityDesignerForExcelToPdf))]
    public class ExcelToPdf : BaseNativeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ExcelSourcePath { get; set; }

        [Category("Input")]
        public InArgument<string> DestinationPath { get; set; }

        [Category("Input")]
        public InArgument<string> SheetName { get; set; }

        [Category("Output")]
        public OutArgument<bool> Output { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            string excelSourcePath = ExcelSourcePath.Get(context);
            string destinationPath = DestinationPath.Get(context);
            string sheetName = SheetName.Get(context);
            bool result = false;
            Microsoft.Office.Interop.Excel.Application excel;
            Workbook workbook;
            Worksheet worksheet;
            try
            {
                excel = new Application();
                //excel = new ApplicationClass(); // creates the instance of Excel Workbook
                workbook = excel.Workbooks.Open(excelSourcePath); //Open the particular excel file
                excel.Visible = false;
                if (destinationPath != null)
                {

                    if (result == false && sheetName != null)
                    {
                        worksheet = (Worksheet)excel.Worksheets[sheetName]; //Select a Sheet by specifying the worksheet name
                        worksheet.Activate(); //Activates the particular worksheet
                        worksheet.ExportAsFixedFormat(0, destinationPath); //converts excel to PDF format, type specifies the output format to which excel is converted, for PDF the value is 0
                        result = true;
                    }
                    if (result == false && sheetName == null)
                    {
                        workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, destinationPath);

                        //workbook.ExportAsFixedFormat(0, destinationPath);
                        result = true;
                    }
                }
                else
                {
                    destinationPath = excelSourcePath.Substring(0, excelSourcePath.LastIndexOf('.'));

                    if (result == false && sheetName != null)
                    {
                        worksheet = (Worksheet)excel.Worksheets[sheetName]; //Select a Sheet by specifying the worksheet name
                        worksheet.Activate(); //Activates the particular worksheet
                        worksheet.ExportAsFixedFormat(0, destinationPath); //converts excel to PDF format, type specifies the output format to which excel is converted, for PDF the value is 0
                        result = true;
                    }
                    if (result == false && sheetName == null)
                    {
                        workbook.ExportAsFixedFormat(0, destinationPath);
                        result = true;
                    }
                }
                workbook.Close(); //Closing the Workbook

                excel.Quit();
                Output.Set(context, result);



            }
            catch (Exception ex)
            {

                Log.Logger.LogData("Check a File Path and File name ", LogLevel.Error);

            }
        }
    }
}
