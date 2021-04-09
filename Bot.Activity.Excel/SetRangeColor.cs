using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Excel
{
    [Designer(typeof(SetRangeColor_ActivityDesigner))]
    [ToolboxBitmap("Resources/SetRangeColor.png")]
    public class SetRangeColor : BaseNativeActivity
    {

        [Category("Input Paramaters")]
        [DisplayName("Range")]
        [Description("Eg A1:H3")]
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


        [Category("Input Paramaters")]
        [DisplayName("Color")]
        [Description("Enter Color Variable")]
        [RequiredArgument]
        public InArgument<System.Drawing.Color> Colo { get; set; }

        Microsoft.Office.Interop.Excel.Application xlApp;
        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        Microsoft.Office.Interop.Excel._Worksheet xlWorkSheet;
        Microsoft.Office.Interop.Excel.Range xlRange = null;
        string FileParth = null;
        string SheetName = null;

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                object misValue = Missing.Value;
                string[] rang = null;
                string R1 = null;
                string R2 = null;
                FileParth = @FilePath.Get(context);
                SheetName = Sheet.Get(context);
                string range = Range.Get(context);

                ExcelHelper.Shared.Close_OpenedFile(FileParth);

                char[] ch = { ':' };

                if (range.Contains(":"))
                {
                    rang = range.Split(ch[0]);
                    R1 = rang[0];
                    R2 = rang[1];
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

                if (R1 != null && R2 != null)
                {
                    xlRange = xlWorkSheet.get_Range(R1, R2);

                }

                if (R1 != null && R2 == null)
                {
                    xlRange = xlWorkSheet.get_Range(R1);
                }

                xlRange.Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(Colo.Get(context));
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
