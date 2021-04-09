
using System.Linq;
using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelMacroRun.png")]
    [Designer(typeof(Excel_Macro_Run_ActivityDesigner))]
    public class Excel_Macro_Run : BaseNativeActivity
    {
        [Category("Input Parameters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Parameters")]
        [DisplayName("Macro Name")]
        [Description("Set Macro Name")]
        [RequiredArgument]
        public InArgument<string> Macro { get; set; }

        [Category("Input Parameters")]
        [DisplayName("Macro Parameters")]
        [Description("Set Macro Parameters")]
        public InArgument<IEnumerable<object>> MacroParameters { get; set; }


        //public InArgument<object[]> Macro { get; set; } //need object args   object[] oRunArgs

        [Category("Output Parameters")]
        [DisplayName("Macro result")]
        [Description("Get Macro Output Value")]
        public OutArgument<object> Result { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_Macro_Run()
        {
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            string workbookFullName = FilePath.Get(context);
            string macro = Macro.Expression.ToString();
            IEnumerable<object> param = MacroParameters.Get(context);


            try
            {
                object result = null;
                string workbookName = string.Empty;

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    dynamic workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    int count;
                    Object[] arr = new Object[10];
                    if (param != null)
                    {
                        count = param.Count();

                        arr = param.ToArray();
                    }

                    else
                    {
                        count = 0;
                    }


                    switch (count)
                    {
                        case 0:
                            result = ExcelHelper.Shared.GetApp().Run(macro);
                            break;
                        case 1:

                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0]);
                            break;
                        case 2:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1]);
                            break;
                        case 3:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2]);
                            break;
                        case 4:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3]);
                            break;
                        case 5:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4]);
                            break;
                        case 6:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                            break;
                        case 7:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
                            break;
                        case 8:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
                            break;
                        case 9:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8]);
                            break;
                        case 10:
                            result = ExcelHelper.Shared.GetApp().Run(macro, arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[9]);
                            break;
                        default:
                            {
                                Log.Logger.LogData("Number of Parameters exceeds limit of 10 " + " in activity Excel_Macro_Run", LogLevel.Error);
                            }
                            if (!ContinueOnError) { context.Abort(); }
                            break;


                    }

                    workBookObject.Save();
                    Result.Set(context, result);

                    if (NeedToClose == true)
                    {
                        ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                        ExcelHelper.Shared.Dispose();
                    }
                    if (false == NeedToClose)
                    {
                        workBookObject.Close();
                        ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:" + workbookFullName + " in activity Excel_Macro_Run", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146827284)
                {
                    Log.Logger.LogData("The macro " + macro + " may not be available in workbook " + workbookFullName + "  or all macros may be disabled in activity Excel_Macro_Run", LogLevel.Error);
                    ExcelHelper.Shared.Dispose();
                    if (!ContinueOnError) { context.Abort(); }
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity Excel_Macro_Run", LogLevel.Error);
                }
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}

