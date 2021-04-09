using System.Linq;
using System.Activities;
using System.ComponentModel;
using Logger;
using System;
using System.Drawing;
namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbbokCloseAll.png")]
    [Designer(typeof(Excel_Workbook_CloseAll_ActivityDesigner))]
    public class Excel_Workbook_CloseAll : BaseNativeActivity
    {
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                var count = ExcelHelper.Shared.GetApp().Workbooks.Count;
             
                ExcelHelper.Shared.GetApp().Workbooks.Close();
                //for (var i = 0; i < count; i++)
                //{
                //     ExcelHelper.Shared.GetApp().Workbooks.Item(i).Close();

                //}

                if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                {
                    ExcelHelper.Shared.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Workbook_CloseAll", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
