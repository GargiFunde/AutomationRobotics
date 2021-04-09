using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Bot.Activity.Excel
{
    [Designer(typeof(Excel_Insert_Delete_Rows_ActivityDesigner))]
    [ToolboxBitmap("Resources/ExcelInsertDeleteRow.png")]
    public class Excel_Insert_Delete_Rows : BaseNativeActivity
    {

        [Category("Destination")]
        [DisplayName("No.Rows")]
        [Description("Set Number of Rows")]
        [RequiredArgument]
        public InArgument<Int32> NoRows { get; set; }

        [Category("Destination")]
        [DisplayName("Position")]
        [Description("Set Position")]
        [RequiredArgument]
        public InArgument<Int32> Position { get; set; }


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
        [DisplayName("Change Mode")]
        [RequiredArgument]
        public Mode mode { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = FilePath.Get(context);
                string workSheetName = WorksheetName.Get(context);
                int NoRowsCounts = NoRows.Get(context);
                int PostionsCounts = Position.Get(context);
                string workbookName = string.Empty;

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    Workbook workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, workSheetName, false) != null;

                    if (false == sheetExist)
                    {
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("\"" + workSheetName + "\" worksheet does not exist in activity Excel_Insert_Delete_Rows", LogLevel.Error);
                    }
                    else
                    {
                        Worksheet xlWorksheet = workBookObject.Sheets[workSheetName];
                        if (mode == Mode.Add)
                        {
                            for (int i = 0; i < NoRowsCounts; i++)
                            {
                                xlWorksheet.Rows[PostionsCounts].Insert(XlInsertShiftDirection.xlShiftDown);
                            }
                        }
                        else if (mode == Mode.Delete)
                        {
                            for (int i = 0; i < NoRowsCounts; i++)
                            {
                                xlWorksheet.Rows[PostionsCounts].Delete(XlDeleteShiftDirection.xlShiftUp);
                            }
                        }

                        workBookObject.Save();
                        workBookObject.Close();
                        ExcelHelper.Shared.Dispose();
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Insert_Delete_Rows", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Insert_Delete_Rows", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}

