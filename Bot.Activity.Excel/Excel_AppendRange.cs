using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace Bot.Activity.Excel
{
    [Designer(typeof(Excel_AppendRangeActivityDesigner))]
    [ToolboxBitmap("Resources/ExcelAppendRange.png")]
    class Excel_AppendRange : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Parameters")]
        [DisplayName("DataTable")]
        [Description("Enter input datatable")]
        public InArgument<System.Data.DataTable> datatablein { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("SheetName")]
        [Description("Enter Sheet Name")]
        [RequiredArgument]
        public InArgument<string> Sheet { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Range")]
        [Description("Enter Range")]
        [RequiredArgument]
        public InArgument<string> Range { get; set; }


        protected override void Execute(NativeActivityContext context)
        {

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel._Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range xlRange = null;
            object misValue = Missing.Value;

            System.Data.DataTable dt = new System.Data.DataTable();
            dt = datatablein.Get(context);
            string FileParth = FilePath.Get(context);
            string SheetName = Sheet.Get(context);
            string range = Range.Get(context);
            char[] ch = { ':' };
            string[] rang = range.Split(ch[0]);
            string R1 = rang[0];
            string R2 = rang[1];



            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;

            xlWorkBook = xlApp.Workbooks.Open(FileParth, misValue, false, misValue, misValue, misValue, true, misValue, misValue, misValue, misValue, misValue, false, misValue, misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[SheetName];
            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;
            xlWorkSheet.Activate();

            //  string colo= xlWorkSheet.Cells[R1,R2].Style.Fill.BackgroundColor;

            xlRange = xlWorkSheet.get_Range(R1, R2);
            // xlRange = xlWorkSheet.UsedRange.Offset["A"+xlWorkSheet.UsedRange.Rows.Count,Type.Missing];
            int i = 0;
            int j = 0;
            //Header
            Logger.Log.Logger.LogData("column count" + dt.Columns.Count, LogLevel.Info);
            for (i = 0; i < dt.Columns.Count; i++)
            {
                xlRange.Cells[1, i + 1] = dt.Columns[i].ColumnName;

            }
            //Datas

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    xlRange.Cells[i + 2, j + 1] = dt.Rows[i][j];
                }
            }
            if (FileParth != null || FileParth != "")
            {
                try
                {
                    xlApp.ActiveWorkbook.SaveAs(FileParth);
                    xlApp.Quit();

                    xlWorkSheet = null;
                    xlWorkBook = null;
                    xlApp = null;
                }
                catch (Exception ex)
                {
                    Logger.Log.Logger.LogData("Exception  " + ex.Message, LogLevel.Error);
                }
            }
            else
            {
                xlApp.Visible = true;
            }
        }
    }
}
