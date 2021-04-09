using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/read 16 px.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class ExcelRowReadSingle : BaseNativeActivity
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
        public OutArgument<dynamic> Value { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public ExcelRowReadSingle()
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
            Workbook xlWorkbook = null;
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
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Read_Single_Row", LogLevel.Error);
                    }
                    else
                    {
                        xlWorkbook = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                        xlWorksheet = xlWorkbook.Sheets[workSheetName];
                        int totalColumns = xlWorksheet.UsedRange.Columns.Count;
                        int totalRows = xlWorksheet.UsedRange.Rows.Count;

                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(totalColumns);

                        string rowLetter = startingCell.Substring(1);
                        string endingCell = colLetter + rowLetter;
                        xlRange = xlWorksheet.Range[startingCell, endingCell];

                        result = xlRange.Value;
                        Value.Set(context, result);

                        if (NeedToClose == true)
                        {
                            ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                        }

                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                    }


                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Read_Single_Row", LogLevel.Error);
                }

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
            catch (Exception ex)
            {
                Value.Set(context, null);
                Log.Logger.LogData(ex.Message + " in activity Excel_Read_Single_Row", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
