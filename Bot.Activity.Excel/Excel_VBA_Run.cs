////using System.Activities;
////using System.ComponentModel;
////using System;
////using Logger;
////using System.Drawing;
////using System.IO;

////namespace Bot.Activity.Excel
////{
////    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/VBA Run 16 Px.png")]
////    [Designer(typeof(ActivityDesigner1))]
////    public class Excel_VBA_Run : BaseNativeActivity
////    {
////        [RequiredArgument]
////        public InArgument<string> FunctionName { get; set; }

////        //[RequiredArgument]
////        //public InArgument<dynamic[]> Parameters { get; set; } //need object args   object[] oRunArgs
////        public OutArgument<string> Result { get; set; }

////        protected override void Execute(NativeActivityContext context)
////        {
////            try
////            {
////                string macro = FunctionName.Expression.ToString();
////                // dynamic[] functionParameters = Parameters.Get(context);
////                var result = string.Empty;
////                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
////                //  result = ExcelHelper.Shared.GetApp().Run(macro, "1234"); //it works for one argument but not for object[] provided that VBA function is having matching argument type
////                result = ExcelHelper.Shared.GetApp().Run(macro);
////                Result.Set(context, result);
////            }
////            catch (Exception ex)
////            {
////                Log.Logger.LogData(ex.Message + " in activity Excel_VBA_Run", LogLevel.Error);
////                if (!ContinueOnError)
////                {
////                    context.Abort();
////                }
////            }
////        }

////    }
////}

//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;
//using Excel = Microsoft.Office.Interop.Excel;
//using Logger;
//using System.ComponentModel;
//using Bot.Activity.Excel;

//namespace EXCEL_VBA_RUN
//{
//    //[Designer(typeof(ActivityDesigner1))]

//    public class Excel_VBA_Run : BaseNativeActivity
//    {
//        [Category("Input Paramaters")]
//        [DisplayName("FilePath")]
//        [Description("")]
//        [RequiredArgument]
//        public InArgument<string> FilePath { get; set; }


//        [Category("Input Paramaters")]
//        [DisplayName("MacroName")]
//        [Description("")]
//        [RequiredArgument]
//        public InArgument<string> MacroName { get; set; }



//        [Category("Input Paramaters")]
//        [DisplayName("MacroParameters")]
//        [Description("arg1, arg2")]
//        [RequiredArgument]
//        public InArgument<IEnumerable<object>> MacroParameters { get; set; }

//        [Category("Options")]
//        [DisplayName("OutputFilePath")]
//        [Description("IF you want to dump the macro result, along with old result, into new excel file")]

//        public InArgument<string> OutFilePath { get; set; }


//        protected override void Execute(NativeActivityContext context)
//        {
//            object oMissing = System.Reflection.Missing.Value;


//            Microsoft.Office.Interop.Excel.ApplicationClass oExcel = new Microsoft.Office.Interop.Excel.ApplicationClass();
//            // oExcel.Visible = true;
//            Microsoft.Office.Interop.Excel.Workbooks oBooks = oExcel.Workbooks;
//            Microsoft.Office.Interop.Excel._Workbook oBook = null;




//            try
//            {


//                int count;
//                string outfilepath = OutFilePath.Get(context);
//                string filepath = @FilePath.Get(context);
//                string macroname = MacroName.Get(context);
//                Object[] arr = new Object[10];
//                IEnumerable<object> macroparameters = MacroParameters.Get(context);

//                if (macroparameters != null)
//                {
//                    count = macroparameters.Count();



//                    arr = macroparameters.ToArray();
//                }

//                else
//                {
//                    count = 0;
//                }


//                oBook = oBooks.Open(filepath, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);


//                switch (count)
//                {
//                    case 0:
//                        RunMacro(oExcel, new Object[] { macroname });

//                        break;


//                    case 1:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0] });

//                        break;

//                    case 2:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1] });

//                        break;
//                    case 3:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3] });

//                        break;
//                    case 4:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4] });

//                        break;
//                    case 5:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5] });

//                        break;
//                    case 6:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5], arr[6] });

//                        break;

//                    case 7:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5], arr[6], arr[7] });

//                        break;

//                    case 8:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8] });

//                        break;

//                    case 9:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[9] });

//                        break;

//                    case 10:
//                        RunMacro(oExcel, new Object[] { macroname, arr[0], arr[1], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[9], arr[10] });

//                        break;
//                    default:
//                        {
//                            Log.Logger.LogData("Number of Parameters exceeds limit of 10 " + " in  Excel_Macro_Run", LogLevel.Error);
//                        }
//                        if (!ContinueOnError)
//                        {
//                            context.Abort();
//                        }
//                        break;
//                }

//                if (outfilepath != null)
//                {


//                    oBook.Close(true, outfilepath, oMissing);
//                }
//                else
//                {

//                    oBook.Save();

//                    oBook.Close(false, oMissing, oMissing);
//                }


//            }
//            catch (TargetInvocationException e)
//            {
//                Log.Logger.LogData("Exception in Excelvba " + e.Message + " #" + e.InnerException, LogLevel.Error);
//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                }
//            }
//            catch (Exception e)
//            {
//                Log.Logger.LogData("Exception in Excelvba " + e.Message, LogLevel.Error);
//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                }
//            }

//            finally
//            {
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBook);
//                oBook = null;
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBooks);
//                oBooks = null;
//                oExcel.Quit();
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
//                oExcel = null;


//                GC.Collect();
//            }



//        }
//        private void RunMacro(object oApp, object[] oRunArgs)
//        {
//            oApp.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oApp, oRunArgs);
//        }

//        private void releaseObject(object obj)
//        {
//            try
//            {
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
//                obj = null;
//            }
//            catch (Exception ex)
//            {
//                obj = null;
//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }
//    }

//}

