using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelRowRead.png")]
    [Designer(typeof(Excel_ReadSingleRow_ActivityDesigner))]
    public class Excel_ReadSingleRow:BaseNativeActivity
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
        [DisplayName("Starting Cell")]
        [Description("eg.\"A1\"")]
        [RequiredArgument]
        public InArgument<string> StartingCell { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Value")]
        [Description("Get Read Row")]
        [RequiredArgument]
        public OutArgument<IEnumerable<object>> Value { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_ReadSingleRow()
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
            Worksheet xlWorksheet = null;
            Range xlRange = null;

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
                    string startingCell = StartingCell.Get(context);

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
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Read_Single_Row", LogLevel.Error);
                    }
                    else
                    {
                        // xlWorkbook = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                        xlWorksheet = workBookObject.Sheets[workSheetName];
                        int totalColumns = xlWorksheet.UsedRange.Columns.Count;
                        int totalRows = xlWorksheet.UsedRange.Rows.Count;
                        string rowLetter = startingCell.Substring(1);

                        String newStr = startingCell.Substring(1);
                        Range lastCell = xlWorksheet.Cells[int.Parse(newStr), totalColumns];
                        if (lastCell.Value2 == null)
                        {
                            lastCell = lastCell.End[XlDirection.xlToLeft];
                        }
                        int lastColumn = lastCell.Column;
                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(lastColumn);

                        string endingCell = colLetter + newStr;

                        xlRange = xlWorksheet.Range[startingCell, endingCell];

                        result = xlRange.Value;

                        IEnumerable<object> rs = ((System.Collections.IEnumerable)result).Cast<object>();

                        Value.Set(context, rs);

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
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Read_Single_Row", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Value.Set(context, null);
                Log.Logger.LogData(ex.Message + " in activity Excel_Read_Single_Row", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {
                ReleaseObject(xlRange);
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
