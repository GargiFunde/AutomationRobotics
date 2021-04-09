using System;
using System.Data;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logger;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelRowRead.png")]
    [Designer(typeof(Excel_Row_Read_ActivityDesigner))]
    public class Excel_Row_Read : BaseNativeActivity
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
        [DisplayName("Starting Row")]
        [Description("eg.1")]
        public InArgument<int> StartingRow { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Ending Row")]
        [Description("eg.1")]
        public InArgument<int> EndingRow { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Value")]
        [Description("Get Read Row")]
        [RequiredArgument]
        public OutArgument<DataTable> Value { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_Row_Read()
        {
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            string workbookFullName = FilePath.Get(context);
            string workSheetName = WorksheetName.Get(context);
            dynamic result = null;
            bool excelFileVisible = false;
            string workbookName = string.Empty;
            Range xlRange = null;
            Worksheet xlWorksheet = null;
            Range xlRows = null;
            DataTable outputDT = null;
            try
            {
                if (true == NeedToOpen)
                {
                    excelFileVisible = true;
                }

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    string worksheetName = WorksheetName.Get(context);
                    dynamic workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);

                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        if (NeedToClose == true)
                        {
                            workBookObject.Close();
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Row_Read", LogLevel.Error);
                    }
                    else
                    {
                        xlWorksheet = workBookObject.Sheets[workSheetName];
                        int totalColumns = xlWorksheet.UsedRange.Columns.Count;
                        int totalRows = xlWorksheet.UsedRange.Rows.Count;

                        int startingRow = StartingRow.Get(context);
                        int endingRow = EndingRow.Get(context);
                        string range = null;

                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(totalColumns);
                        string startingCell = null;
                        string endingCell = null;

                        if (0 != startingRow && 0 != endingRow && startingRow == endingRow)
                        {
                            Log.Logger.LogData("Starting Row and Ending Row must be different in activity Excel_Row_Read", LogLevel.Error);
                        }
                        else
                        {
                            if (0 == startingRow && 0 == endingRow)
                            {
                                // range = 1 + ":" + totalRows;
                                startingCell = "A1";
                                endingCell = colLetter + totalRows;
                            }
                            else if (0 != startingRow && 0 == endingRow)
                            {
                                //range = startingRow + ":" + totalRows;
                                startingCell = "A" + startingRow;
                                endingCell = colLetter + totalRows;
                            }
                            else if (0 == startingRow && 0 != endingRow)
                            {
                                //range = 1 + ":" + endingRow;
                                startingCell = "A1";
                                endingCell = colLetter + endingRow;
                            }
                            else
                            {
                                // range = startingRow + ":" + endingRow;
                                startingCell = "A" + startingRow;
                                endingCell = colLetter + endingRow;
                            }
                            //xlRows = xlWorksheet.Rows[range];
                            xlRange = xlWorksheet.Range[startingCell, endingCell];
                            result = xlRange.Value;
                            outputDT = ExcelHelper.Shared.Convert2DArraytoDatatable(result);
                        }


                        Value.Set(context, outputDT);

                        if (true == NeedToClose)
                        {
                            workBookObject.Close();
                        }
                        if (false == NeedToClose && false == NeedToOpen)
                        {
                            workBookObject.Close();
                        }
                        if (false == NeedToClose && true == NeedToOpen)
                        {
                            workBookObject.Close();
                            ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }

                    }


                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Row_Read", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Value.Set(context, null);
                Log.Logger.LogData(ex.Message + " in activity Excel_Row_Read", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {
                ReleaseObject(xlRows);
            }

        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
