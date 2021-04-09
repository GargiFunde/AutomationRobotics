using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using DataTable = System.Data.DataTable;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [Designer(typeof(Excel_RowWrite_ActivityDesigner))]
    [ToolboxBitmap("Resources/ExcelRowWrite.png")]
    public class Excel_RowWrite : BaseNativeActivity
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
        [DisplayName("Value")]
        [Description("Set Write Value")]
        [RequiredArgument]
        public InArgument<DataTable> Value { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Starting Cell")]
        [Description("eg.\"A1\"")]
        public InArgument<string> StartingCell { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Status")]
        [Description("Get Write Range Status")]
        public OutArgument<bool> Status { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_RowWrite()
        {
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            string workbookFullName = FilePath.Get(context);
            string workSheetName = WorksheetName.Get(context);
            Range xlRange = null;
            bool excelFileVisible = false;
            object[,] TwoDimensionalArray = null;
            DataTable inputDt = null;
            try
            {
                string workbookName = string.Empty;
                //  Workbook xlWorkbook = null;
                Worksheet xlWorksheet = null;

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
                        Status.Set(context, false);

                        if (NeedToClose == true)
                        {
                            workBookObject.Close();
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Row_Write", LogLevel.Error);
                    }
                    else
                    {
                        xlWorksheet = workBookObject.Sheets[workSheetName];
                        inputDt = Value.Get(context);
                        int totalRows = inputDt.Rows.Count;
                        int totalColumns = inputDt.Columns.Count;
                        string startingCell = StartingCell.Get(context);

                        if (null == startingCell)
                        {
                            startingCell = "A1";
                        }

                        String endingNum = startingCell.Substring(1);
                        int rangeEnd = totalRows - 1 + int.Parse(endingNum);
                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(totalColumns);
                        string endingCell = colLetter + rangeEnd;

                        TwoDimensionalArray = ExcelHelper.Shared.ConvertDataTableToArray(inputDt);

                        if (null == inputDt)
                        {
                            Status.Set(context, false);
                        }
                        else
                        {
                            xlRange = xlWorksheet.Range[startingCell, endingCell];
                            xlRange.Value = TwoDimensionalArray;
                            workBookObject.Save();
                            Status.Set(context, true);
                        }
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
                    Status.Set(context, false);
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Row_Write", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }              
            }
            catch (Exception ex)
            {
                Status.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity Excel_Row_Write", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
