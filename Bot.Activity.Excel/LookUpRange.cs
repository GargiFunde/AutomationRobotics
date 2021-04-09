using Logger;
using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Bot.Activity.Excel
{
    [Designer(typeof(LookUpRange_ActivityDesigner))]
    [ToolboxBitmap("Resources/LookUpRange.png")]
    public class LookUpRange : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("File Path")]
        [Description("Enter Excel File Path")]
        [RequiredArgument]
        public InArgument<string> ExcelFilePath { get; set; }

        [Category("Input")]
        [DisplayName("Range")]
        [Description("A1:B5")]
        [RequiredArgument]
        public InArgument<string> Range { get; set; }

        [Category("Input")]
        [DisplayName("Worksheet Name")]
        [Description("Enter SheetName")]
        [RequiredArgument]
        public InArgument<string> SheetName { get; set; }

        [Category("Input")]
        [DisplayName("Value")]
        [Description("Enter Value")]
        public InArgument<string> Value { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("Enter Result Variable in string Format")]
        public OutArgument<string> Result { get; set; }

        object False = false;
        object True = true;
        string filepath = null;
        string from = null;
        string to = null;
        Range rng;
        Application excel = new Application();
        Workbook wb;
        _Worksheet ws;
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                filepath = ExcelFilePath.Get(context);
                //_Application excel = new ApplicationClass();
                ExcelHelper.Shared.Close_OpenedFile(filepath);

                excel.DisplayAlerts = false;

                wb = excel.Workbooks._Open(@filepath, False, False, Missing.Value, Missing.Value, False, False, Missing.Value, Missing.Value, False, Missing.Value, Missing.Value, True);

                String sheet = SheetName.Get(context);
                if (sheet != null)
                {
                    ws = (_Worksheet)wb.Worksheets[sheet];
                }
                else
                {
                    ws = (_Worksheet)wb.Worksheets[1];
                }

                string temp = Range.Get(context);
                char[] a = { ':', ';' };

                if (temp != null)
                {
                    string[] temp2 = temp.Split(a[0]);
                    if (temp2.Length == 1)
                    {
                        from = temp2[0];
                        Range last = ws.UsedRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                        rng = ws.get_Range(from, last);
                    }
                    if (temp2.Length == 2)
                    {
                        from = temp2[0];
                        to = temp2[1];
                        rng = ws.get_Range(from, to);
                    }


                }
                else
                {
                    Range last = ws.UsedRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                    from = "A1";
                    rng = ws.get_Range("A1", last);
                }

                Range findRng = rng.Find(Value.Get(context), Missing.Value, XlFindLookIn.xlValues, Missing.Value, Missing.Value, XlSearchDirection.xlNext, False, False, Missing.Value);
                string Address = RemoveSpecialCharacters(findRng.Address);
                Result.Set(context, Address);

                //Log.Logger.LogData("\n## The Address is  " + Address, LogLevel.Info);
                string RemoveSpecialCharacters(string str)
                {
                    return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
                }
                wb.Save();


            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Exception in LookUpRange:  " + ex.Message, LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            finally
            {


                wb.Close();
                excel.Quit();

                Marshal.ReleaseComObject(ws);
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(excel);
            }
        }
    }
}
