using System.Activities;
using System.ComponentModel;
using System.IO;
using Logger;
using System;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelCellValueRead.png")]
    [Designer(typeof(Excel_CellValue_Read_ActivityDesigner))]
    public class Excel_CellValue_Read : BaseNativeActivity
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
        [DisplayName("Cell")]
        [Description("Set Cell Value")]
        [RequiredArgument]
        public InArgument<string> Cell { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Output Value")]
        [Description("Get Output Value")]
        [RequiredArgument]
        public OutArgument<dynamic> Value { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_CellValue_Read()
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
                bool excelFileVisible = false;

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
                    string cell = Cell.Get(context);

                    dynamic workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                  
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("\"" + worksheetName + "\" worksheet does not exist in activity Excel_CellValue_Read", LogLevel.Error);
                    }
                    else
                    {
                        var value = ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).Value?.ToString() ?? string.Empty;
                        Value.Set(context, value);

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
                    Log.Logger.LogData("Excel file does not exist:" + workbookFullName + " in activity Excel_CellValue_Read", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
           
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_CellValue_Read", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }
    }
}
