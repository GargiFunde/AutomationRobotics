using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab
using System.Drawing;
using DataTable = System.Data.DataTable;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWriteFile.png")]
    [Designer(typeof(Excel_WriteFile_ActivityDesigner))]
    public class Excel_WriteFile : BaseNativeActivity
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

        [Category("Input Paramaters")]
        [DisplayName("Table To Write")]
        [Description("Set Data Table To Write")]
        [RequiredArgument]
        public InArgument<DataTable> TableToWrite { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Header")]
        public bool IsHeader { get; set; }
        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }
        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        public Excel_WriteFile()
        {
            IsHeader = true;
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            string strFilePath = FilePath.Get(context);
            string workbookName = FilePath.Get(context);
            Worksheet xlWorksheet = null;
            Range xlRange = null;
            bool excelFileVisible = false;

            try
            {
                if (true == NeedToOpen)
                {
                    excelFileVisible = true;
                }

                if (File.Exists(strFilePath))
                {
                    ExcelHelper.Shared.Close_OpenedFile(strFilePath);

                    workbookName = Path.GetFileName(strFilePath);
                    string workSheetName = WorksheetName.Get(context);
                    Workbook xlWorkbook = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(strFilePath);
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, workSheetName, false) != null;
                    dynamic worksheets = xlWorkbook.Worksheets;
                    dynamic worksheetObject = null;
                    DataTable dt = TableToWrite.Get(context);

                    if (dt == null)
                    {
                        xlWorkbook.Close();
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Table To Write parameter(Datatable) is null in activity Excel_WriteFile", LogLevel.Error);
                    }
                    else
                    {
                        if (false == sheetExist)
                        {
                            worksheetObject = worksheets.Add();
                            worksheetObject.Name = workSheetName;
                        }
                        object[,] TwoDimensionalArray = null;
                        string colLetter = String.Empty;
                        string endingCell = String.Empty;
                        int rangeEnd = 0;
                        xlWorksheet = xlWorkbook.Sheets[workSheetName];

                        int rowCount = dt.Rows.Count;
                        int colCount = dt.Columns.Count;

                        if (IsHeader == true)
                        {
                            rangeEnd = 1 + rowCount;
                            TwoDimensionalArray = ConvertDataTableToArray(dt);
                        }
                        else
                        {
                            rangeEnd = rowCount;
                            TwoDimensionalArray = ExcelHelper.Shared.ConvertDataTableToArray(dt);
                        }

                        colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(colCount);
                        endingCell = colLetter + rangeEnd;

                        xlRange = xlWorksheet.Range["A1", endingCell];
                        xlRange.Value = TwoDimensionalArray;

                        var range = xlWorksheet.get_Range("A1", "A1");
                        range.Select();

                        xlWorkbook.Save();

                        if (true == NeedToClose)
                        {
                            xlWorkbook.Close();
                        }
                        if (false == NeedToClose && false == NeedToOpen)
                        {
                            xlWorkbook.Close();
                        }
                        if (false == NeedToClose && true == NeedToOpen)
                        {
                            xlWorkbook.Close();
                            ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(strFilePath);
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }

                    }

                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + strFilePath + "\" in activity Excel_WriteFile", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_WriteFile", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {
                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                //if(xlRange != null)
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
        public object[,] ConvertDataTableToArray(DataTable dt)
        {
            var ret = Array.CreateInstance(typeof(object), (dt.Rows.Count + 1), dt.Columns.Count) as object[,];
            for (var i = 0; i < (dt.Rows.Count + 1); i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if (0 == i)
                    {
                        ret[i, j] = dt.Columns[j].ColumnName;
                    }
                    else
                    {
                        ret[i, j] = dt.Rows[i - 1][j];
                    }

                }
            }
            return ret;
        }
    }
}
