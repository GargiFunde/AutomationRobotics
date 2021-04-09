using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using Excel1 = Microsoft.Office.Interop.Excel;
using System.Drawing;
namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbookCreate.png")]
    [Designer(typeof(Excel_Workbook_Create_ActivityDesigner))]
    
    public class Excel_Workbook_Create : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Workbook")]
        [Description("Get Workbook")]
        public OutArgument<Excel1.Workbook> Workbook { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = FilePath.Get(context);

                if (!workbookFullName.Contains("."))
                {
                    workbookFullName = workbookFullName + ".xlsx";
                }

                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                var wb = ExcelHelper.Shared.GetApp().Workbooks.Add();
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                ExcelHelper.Shared.GetApp().ActiveWorkbook.SaveAs(workbookFullName);
                Workbook.Set(context,wb);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Workbook_Count", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
