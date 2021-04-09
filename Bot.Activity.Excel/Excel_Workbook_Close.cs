using System.Linq;
using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using System.IO;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbookClose.png")]
    [Designer(typeof(Excel_Workbook_Close_Designer))]
    public class Excel_Workbook_Close : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            string workbookName = FilePath.Get(context);
            try
            {
                if (!workbookName.Contains("."))
                {
                    workbookName = workbookName + ".xlsx";
                }
                if (File.Exists(workbookName))
                {
                    workbookName = Path.GetFileName(workbookName);
                    ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                }

                if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                {
                    ExcelHelper.Shared.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    Log.Logger.LogData("Excel file \"" + workbookName + "\" is not opened to close in activity Workbook_Close", LogLevel.Info);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity Workbook_Close", LogLevel.Error);
                    if (!ContinueOnError)
                    {
                        context.Abort();
                    }
                }
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}
