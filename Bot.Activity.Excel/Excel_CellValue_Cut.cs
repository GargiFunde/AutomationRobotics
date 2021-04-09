using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Cut _ 16 px.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class Excel_CellValue_Cut : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> WorkbookFullName { get; set; }
        [RequiredArgument]
        public InArgument<string> WorksheetName { get; set; }
        [RequiredArgument]
        public InArgument<string> Cell { get; set; }
        public bool NeedToOpen { get; set; }

      

        public Excel_CellValue_Cut()
        {
            NeedToOpen = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = WorkbookFullName.Get(context);
                string workbookName = string.Empty;
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                // bool needToOpen = NeedToOpen.Get(context);
                if (File.Exists(workbookFullName))
                {
                    workbookName = Path.GetFileName(workbookFullName);
                }
                else
                {
                    Log.Logger.LogData("Workbook file do not exist, Error in activity CellValue_Cut", LogLevel.Error);
                    context.Abort();
                }
                if (NeedToOpen == true)
                {
                    ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                }
                string worksheetName = WorksheetName.Get(context);
                string cell = Cell.Get(context);
                 ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).Cut();
           

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity CellValue_Cut", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
