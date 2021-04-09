using Bot.Activity.Excel.Properties;
using Logger;
using System;
using System.Activities;
using System.Activities.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExcelObject = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelReadFile.png")]
    [Designer(typeof(Excel_ReadFile_ActivityDesigner))]
    public class Excel_ReadFile : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }


        [Category("Input Paramaters")]
        [DisplayName("Worksheet Name")]
        [Description("Set Worksheet Name")]
        [RequiredArgument]
        public InArgument<string> WorksheetName { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Header")]
        public bool IsHeader { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }


        [Category("Output Paramaters")]
        [DisplayName("Result Data Table")]
        [Description("Get Result Data Table")]
        [RequiredArgument]
        public OutArgument<DataTable> ResultDataTable { get; set; }



        public DataTable dt;
       // public Dictionary<string, Argument> ChildArguments { get; set; } //this is required for designer, reffered internally
        public Excel_ReadFile()
        {
           
            IsHeader = true;
           
            NeedToClose = true;
          //  this.ChildArguments = new Dictionary<string, Argument>();
        }

        //Need to download and install https://www.microsoft.com/en-us/download/confirmation.aspx?id=23734
        protected override void Execute(NativeActivityContext context)
        {
            string workSheetName = WorksheetName.Get(context);
            string strFilePath = FilePath.Get(context);
            //string strFilePath = FilePath;
             string workbookName = FilePath.Get(context);
           // string workbookName = FilePath;
            //  bool bIsHeader = IsHeader.Get(context);

            ReadExcelData(workSheetName, strFilePath, workbookName, IsHeader, context);
        }
       
        public void ReadExcelData(string workSheetName, string strFilePath, string workbookName, bool IsHeader, NativeActivityContext context = null)
        {
           // ExcelObject.Application xlApp = null;
            ExcelObject.Workbook xlWorkbook = null;
            ExcelObject._Worksheet xlWorksheet = null;
            
            // bool needToClose = NeedToClose.Get(context);

            ExcelObject.Range xlRange = null;
            string colname = string.Empty;
            try
            {             

                strFilePath = strFilePath.Replace("\"", "");
                if (File.Exists(strFilePath))
                {
                    ExcelHelper.Shared.Close_OpenedFile(strFilePath);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                    workbookName = Path.GetFileName(strFilePath);
                    xlWorkbook = ExcelHelper.Shared.GetApp().Workbooks.Open(strFilePath);

                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, workSheetName, false) != null;
                    if (false == sheetExist)
                    {
                        xlWorkbook.Close();
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("\"" + workSheetName + "\" worksheet does not exist in activity Excel_ReadFile", LogLevel.Error);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(workSheetName))
                        {
                            xlWorksheet = xlWorkbook.Sheets[0];
                        }
                        else
                        {
                            xlWorksheet = xlWorkbook.Sheets[workSheetName];
                        }

                        xlRange = xlWorksheet.UsedRange;
                        int rowCount = xlRange.Rows.Count;
                        int colCount = xlRange.Columns.Count;

                        dt = new DataTable();
                        int noofrow = 1;

                        if (IsHeader == true)
                        {
                            for (int c = 1; c <= colCount; c++)
                            {
                                colname = xlWorksheet.Cells[1, c].Text;
                                dt.Columns.Add(colname);
                                noofrow = 2;
                            }
                        }

                        if (IsHeader == false)
                        {
                            for (int c = 1; c <= colCount; c++)
                            {
                                colname = xlWorksheet.Cells[1, c].Text;
                                dt.Columns.Add(colname);
                                noofrow = 1;
                            }
                        }
                        //iterate over the rows and columns and print to the console as it appears in the file
                        //excel is not zero based!!

                        bool blankRow = false;
                        for (int i = noofrow; i <= rowCount; i++)
                        {
                            DataRow dr = dt.NewRow();
                            blankRow = true;
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                {
                                    blankRow = false;
                                    dr[j - 1] = xlWorksheet.Cells[i, j].Text;
                                }
                            }
                            if (blankRow == true)
                            {
                                break;
                            }
                            else
                            {
                                dt.Rows.Add(dr);
                            }
                        }

                        if (context != null)
                        {
                            ResultDataTable.Set(context, dt);
                        }


                        if (NeedToClose == true)
                        {
                            xlWorkbook.Close();
                        }
                        if (false == NeedToClose)
                        {
                            xlWorkbook.Close();
                        }
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }

                    }
 
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:"+ strFilePath + " Error in activity Excel_ReadFile", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

                //xlApp = new ExcelObject.Application();
                //xlWorkbook = xlApp.Workbooks.Open(strFilePath);
               
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147352565)
                {
                    Log.Logger.LogData("Con"+ colname + " in activity Excel_ReadFile", LogLevel.Error);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity Excel_ReadFile", LogLevel.Error);
                    if (context != null)
                    {
                        context.Abort();
                    }
                }
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {
                ////rule of thumb for releasing com objects:
                ////  never use two dots, all COM objects must be referenced and released individually
                ////  ex: [somthing].[something].[something] is bad

                ////release com objects to fully kill excel process from running in the background
                //if (xlRange != null)
                //    Marshal.ReleaseComObject(xlRange);
                //if (xlWorksheet != null)
                //    Marshal.ReleaseComObject(xlWorksheet);
                //if (xlWorkbook != null)
                //{
                //    //close and release
                //    xlWorkbook.Close();
                //    Marshal.ReleaseComObject(xlWorkbook);
                //}

                //if (xlApp != null)
                //{
                //    //quit and release
                //    xlApp.Quit();
                //    Marshal.ReleaseComObject(xlApp);
                //}
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
    }
}
