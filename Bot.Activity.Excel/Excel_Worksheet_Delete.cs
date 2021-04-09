
using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorksheetDelete.png")]
    [Designer(typeof(Excel_Worksheet_Delete_ActivityDesigner))]
    public class Excel_Worksheet_Delete : BaseNativeActivity
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
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = FilePath.Get(context);
                string worksheetName = WorksheetName.Get(context);
                string workbookName = string.Empty;

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);

                    workbookName = Path.GetFileName(workbookFullName);
                    ExcelHelper.Shared.GetApp(false).DisplayAlerts = false;
                    Workbook workBookObject = ExcelHelper.Shared.GetApp(false).Workbooks.Open(workbookFullName);

                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp(false).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Worksheet_Delete", LogLevel.Error);
                    }
                    else
                    {
                        int worksheetCount = workBookObject.Worksheets.Count;
                        if (1 == worksheetCount)
                        {
                            dynamic worksheetObject = null;
                            string newWorksheetName = "Sheet1";
                            if (worksheetName.Equals("Sheet1"))
                            {
                                newWorksheetName = "Sheet2";
                            }
                            worksheetObject = workBookObject.Worksheets.Add();
                            worksheetObject.Name = newWorksheetName;
                        }
                        Worksheet xlWorksheet = workBookObject.Sheets[worksheetName];
                        xlWorksheet.Delete();

                        workBookObject.Save();
                        workBookObject.Close();
                        ExcelHelper.Shared.Dispose();
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Worksheet_Delete", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Worksheet_Delete", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}
