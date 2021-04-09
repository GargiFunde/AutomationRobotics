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
    [Designer(typeof(Excel_Append_Designer))]
    [ToolboxBitmap("Resources/ExcelAppend.png")]
    public class Excel_Append : BaseNativeActivity
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

        public Excel_Append()
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
                        Log.Logger.LogData("Table To Write parameter(Datatable) is null in activity Excel_Append", LogLevel.Error);
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


                        Range xlRangeExcel = xlWorksheet.UsedRange;
                        int rowCountExcel = xlRangeExcel.Rows.Count;
                        if (1 != rowCountExcel)
                        {
                            rowCountExcel = rowCountExcel + 1;
                        }

                        int rowCount = dt.Rows.Count;
                        int colCount = dt.Columns.Count;

                        if (IsHeader == true)
                        {
                            rangeEnd = rowCount + rowCountExcel;
                            TwoDimensionalArray = ConvertDataTableToArray(dt);
                        }
                        else
                        {
                            rangeEnd = rowCount + rowCountExcel - 1;
                            TwoDimensionalArray = ExcelHelper.Shared.ConvertDataTableToArray(dt);
                        }

                        colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(colCount);
                        endingCell = colLetter + rangeEnd;

                        string startingCell = "A" + rowCountExcel;
                        xlRange = xlWorksheet.Range[startingCell, endingCell];
                        xlRange.Value = TwoDimensionalArray;

                        var range = xlWorksheet.get_Range("A1", "A1");
                        range.Select();

                        xlWorkbook.Save();

                        if (NeedToClose == true)
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
                    Log.Logger.LogData("Excel file does not exist:\"" + strFilePath + "\" in activity Excel_Append", LogLevel.Error);
                    if (!ContinueOnError)
                    {
                        context.Abort();
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Append", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }               
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
