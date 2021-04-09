
using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbooktoPdf.png")]
    [Designer(typeof(Excel_Workbook_ToPDF_ActivityDesigner))]
    public class Excel_Workbook_ToPDF : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("PDF Path")]
        [Description("Set PDF Path")]
        [RequiredArgument]
        public InArgument<string> PDFPath{ get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_Workbook_ToPDF()
        {
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string workbookFullName = FilePath.Get(context);
                string workbookName = string.Empty;
                string strPDF = PDFPath.Get(context); ;
                if (!workbookFullName.Contains("."))
                {
                    workbookFullName = workbookFullName + ".xlsx";
                }
                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);

                    workbookName = Path.GetFileName(workbookFullName);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                    if (NeedToOpen == true)
                    {
                        ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    }

                    if (File.Exists(strPDF))
                    {
                        Log.Logger.LogData("PDF file is already exist:" + strPDF + " in activity Excel_Workbook_ToPDF", LogLevel.Error);
                        if (!ContinueOnError) { context.Abort(); }

                    }
                    else
                    {
                        if ((workbookFullName.Contains(".xlsx")) || (workbookFullName.Contains(".xls")))
                        {
                            ExcelHelper.Shared.GetApp().ActiveWorkbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, strPDF);

                        }
                        else
                        {
                            Log.Logger.LogData("File should be .xlsx or .xls", LogLevel.Error);
                        }

                      
                    }
                    if (NeedToClose == true)
                    {
                        ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                    }
                    if (false == NeedToClose )
                    {
                        ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                        ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    }
                    if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                    {
                        ExcelHelper.Shared.Dispose();
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:" + workbookFullName + " in activity Excel_Workbook_ToPDF", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Workbook_ToPDF", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}
