using System.Linq;
using System.Activities;
using System.ComponentModel;
using Logger;
using System;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbookCount.png")]
    [Designer(typeof(Excel_Workbook_Count_ActivityDesigner))]
    public class Excel_Workbook_Count : BaseNativeActivity
    {
        [Category("Output Paramaters")]
        [DisplayName("Count")]
        [Description("Get Count")]
        [RequiredArgument]
        public OutArgument<int> Count { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                var count = ExcelHelper.Shared.GetApp().Workbooks.Count;
                Count.Set(context, count);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Workbook_Count", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
