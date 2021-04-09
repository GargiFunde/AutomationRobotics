using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using ExcelDna.Integration;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [Designer(typeof(Excel_Insert_Delete_Column_ActivityDesigner))]
    [ToolboxBitmap("Resources/ExcelInserDeleteColumn.png")]
    public class Excel_Insert_Delete_Column : BaseNativeActivity
    {
        [Category("Destination")]
        [DisplayName("No.Columns")]
        [Description("Set Number of Columns")]
        [RequiredArgument]
        public InArgument<Int32> NoColumns { get; set; }

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
                int NoColumnsCounts = NoColumns.Get(context);
                int PostionsCounts = Position.Get(context);
                string workbookName = string.Empty;

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    string worksheetName = WorksheetName.Get(context);

                    Workbook workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);               
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;

                    if (false == sheetExist)
                    {
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("\"" + worksheetName + "\" worksheet does not exist in activity Excel_Insert_Delete_Column", LogLevel.Error);
                    }
                    else
                    {
                        Worksheet xlWorksheet = workBookObject.Sheets[workSheetName];

                        if (mode == Mode.Add)
                        {
                            for (int i = 0; i < NoColumnsCounts; i++)
                            {
                                xlWorksheet.Columns[PostionsCounts].Insert(XlInsertShiftDirection.xlShiftToRight);
                                //Delete(XlDeleteShiftDirection.xlShiftToLeft);
                            }
                        }
                        else if (mode == Mode.Delete)
                        {
                            for (int i = 0; i < NoColumnsCounts; i++)
                            {
                                xlWorksheet.Columns[PostionsCounts].Delete(XlDeleteShiftDirection.xlShiftToLeft);
                            }
                        }
                        // workBookObject.SaveAs(workbookFullName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        workBookObject.Save();
                        workBookObject.Close();
                        ExcelHelper.Shared.Dispose();
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Insert_Delete_Column", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Insert_Delete_Column", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
    public enum Mode
    {
        Add = 0,
        Delete = 1
    }
}
