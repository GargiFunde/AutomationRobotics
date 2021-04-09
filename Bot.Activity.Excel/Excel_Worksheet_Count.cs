using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/Excel_Worksheet_Count.png")]
    [Designer(typeof(Excel_Worksheet_Count_ActivityDesigner))]
    public class Excel_Worksheet_Count : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Count")]
        [Description("Get Count")]
        [RequiredArgument]
        public OutArgument<int> Count { get; set; }

        public Excel_Worksheet_Count()
        {
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = FilePath.Get(context);
                int count = 0;
                //    ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                //   var count = ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Worksheets.Count;
      

                if (!workbookFullName.Contains("."))
                {
                    workbookFullName = workbookFullName + ".xlsx";
                }

                if (!File.Exists(workbookFullName))
                {
                    Logger.Log.Logger.LogData("File do not exists :" + workbookFullName + " in activity Excel_Worksheet_Count", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                else
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);

                    string worksheetName = string.Empty;
                    string workbookName = Path.GetFileName(workbookFullName);
                    dynamic workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    dynamic worksheets = workBookObject.Worksheets;
                    count = worksheets.count();

                    if (NeedToClose == true)
                    {
                        workBookObject.Close();
                    }
                    if (false == NeedToClose )
                    {
                        workBookObject.Close();
                        ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    }
                    if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                    {
                        ExcelHelper.Shared.Dispose();
                    }
                }
                Count.Set(context, count);
              
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Worksheet_Count", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}
