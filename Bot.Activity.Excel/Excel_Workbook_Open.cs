using System.Linq;
using System.Activities;
using System.ComponentModel;
using Logger;
using System;
using System.Drawing;
namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbookOpen.png")]
    [Designer(typeof(Excel_Workbookopen_ActivityDesigner))]
    public class Excel_Workbook_Open : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                string workbookName = FilePath.Get(context);
                ExcelHelper.Shared.GetApp().Workbooks.Open(workbookName);

                if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                {
                    ExcelHelper.Shared.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Workbook_Open", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
