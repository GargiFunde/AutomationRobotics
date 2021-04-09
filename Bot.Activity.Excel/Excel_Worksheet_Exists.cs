using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorksheetExists.png")]
    [Designer(typeof(Excel_Worksheet_Exists_ActivityDesigner))]
    public class Excel_Worksheet_Exists : BaseNativeActivity
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

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Exists")]
        [Description("Get Exists")]
        [RequiredArgument]
        public OutArgument<bool> Exists { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string workbookFullName = FilePath.Get(context);
                string worksheetName = WorksheetName.Get(context);

                if (!workbookFullName.Contains("."))
                {
                    workbookFullName = workbookFullName + ".xlsx";
                }

                if (!File.Exists(workbookFullName))
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Worksheet_Exists", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                else
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);

                    string workbookName = Path.GetFileName(workbookFullName);
                    dynamic workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    dynamic worksheets = workBookObject.Worksheets;

                    bool value = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    Exists.Set(context, value);
                    if (true == NeedToClose)
                    {
                        workBookObject.Close();
                    }
                    if (false == NeedToClose)
                    {
                        workBookObject.Close();
                        ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    }
                    if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                    {
                        ExcelHelper.Shared.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Worksheet_Exists", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
