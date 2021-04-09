using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;



namespace Bot.Activity.Excel
{
    [Designer(typeof(ReadCellFormula_ActivityDesigner))]
    [ToolboxBitmap("Resources/ReadCellFormula.png")]
    public class ReadCellFormula : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Cell")]
        [Description("Eg A1")]
        [RequiredArgument]
        public InArgument<string> Range { get; set; }



        [Category("Input Paramaters")]
        [DisplayName("SheetName")]
        [Description("Enter Sheet Name")]
        [RequiredArgument]
        public InArgument<string> Sheet { get; set; }



        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }



        [Category("Output Paramaters")]
        [DisplayName("Result")]
        [Description("Enter Output variable")]



        public OutArgument<string> Result { get; set; }



        Microsoft.Office.Interop.Excel.Application xlApp;
        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        Microsoft.Office.Interop.Excel._Worksheet xlWorkSheet;
        Microsoft.Office.Interop.Excel.Range xlRange = null;
        string FileParth = null;
        string SheetName = null;
        protected override void Execute(NativeActivityContext context)
        {
            object misValue = Missing.Value;
            string[] rang = null;
            string R1 = null;
            string R2 = null;
            FileParth = @FilePath.Get(context);
            SheetName = Sheet.Get(context);
            string range = Range.Get(context);




            try
            {
                ExcelHelper.Shared.Close_OpenedFile(FileParth);

                //if (CommonLibrary.CustomException.CheckFileExists(FileParth) == true)       //Custom Exception
                //{






                char[] ch = { ':' };



                    if (range.Contains(":"))
                    {
                        //rang = range.Split(ch[0]);
                        //R1 = rang[0];
                        //R2 = rang[1];
                        throw new Exception("Exception in " + DisplayName + ": invalid cell number");
                    }
                    else
                    {
                        R1 = range;
                    }





                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlApp.Visible = false;



                    xlWorkBook = xlApp.Workbooks.Open(FileParth, misValue, false, misValue, misValue, misValue, true, misValue, misValue, misValue, misValue, misValue, false, misValue, misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[SheetName];



                    xlWorkSheet.Activate();



                    if (R1 != null)
                    {
                        xlRange = xlWorkSheet.get_Range(R1);



                    }



                    //if (R1 != null && R2 == null)
                    //{
                    //    xlRange = xlWorkSheet.get_Range(R1);
                    //}



                    string str = xlRange.Formula;



                    Result.Set(context, str);
                //}




            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception  " + e.Message, LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {
                xlWorkBook.Save();
                xlWorkBook.Close(true, FileParth, Missing.Value);
                xlApp.Quit();



                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);





            }
        }
    }
}