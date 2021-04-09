using System.Linq;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Logger;
using System;
using ExcelObject = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab
using System.Drawing;
using DataTable = System.Data.DataTable;
using System.Data;
using System.Collections.Generic;
using System.Dynamic;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelRangeRead.png")]
    [Designer(typeof(Excel_Range_Read_ActivityDesigner))]
    public class Excel_Range_Read : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Worksheet Name")]
        [Description("Set Worksheet Name")]
        [RequiredArgument]
        public InArgument<string> WorksheetName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Range")]
        [Description("e.g \"A1:F4\"")]
        [RequiredArgument]
        public InArgument<string> Range { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Value")]
        [Description("Get Read Range")]
        [RequiredArgument]
        public OutArgument<DataTable> Value { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Header")]
        public bool IsHeader { get; set; }
        public Excel_Range_Read()
        {
            NeedToOpen = true;
            NeedToClose = true;
            IsHeader = false;
        }
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                string workbookFullName = FilePath.Get(context);
                string workSheetName = WorksheetName.Get(context);
                ExcelObject.Range xlRange = null;
                DataTable resultDataTable = null;
                bool excelFileVisible = false;
                dynamic workBookObject = null;
                ExcelObject.Range xlRangeHeader = null;
                string workbookName = string.Empty;
                ExcelObject._Worksheet xlWorksheet = null;

                if (true == NeedToOpen)
                {
                    excelFileVisible = true;
                }

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    string worksheetName = WorksheetName.Get(context);
                    string range = Range.Get(context);
                    string[] cells = null;
                    workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);

                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        if (NeedToClose == true)
                        {
                            workBookObject.Close();
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Range_Read", LogLevel.Error);
                    }
                    else
                    {
                        if (range.Contains(":"))
                        {
                            cells = range.Split(':');
                            if (cells.Count() != 2)
                            {
                                Log.Logger.LogData("Please provide valid range in activity Excel_Range_Read", LogLevel.Error);
                                if (!ContinueOnError)
                                {
                                    context.Abort();
                                }
                            }
                            else
                            {
                                dynamic result = null;
                                dynamic resultHeader = null;
                                xlWorksheet = workBookObject.Sheets[workSheetName];

                                xlRange = xlWorksheet.Range[cells[0], cells[1]];
                                result = xlRange.Value;

                                if (!cells[0].Contains("1") && true == IsHeader)
                                {
                                    String startAlpa = new String(cells[0].Where(c => Char.IsLetter(c)).ToArray()).ToUpper();
                                    String endAlpa = new String(cells[1].Where(c => Char.IsLetter(c)).ToArray()).ToUpper();
                                    startAlpa = startAlpa + "1";

                                    endAlpa = endAlpa + "1";
                                    xlRangeHeader = xlWorksheet.Range[startAlpa, endAlpa];
                                    resultHeader = xlRangeHeader.Value;
                                    resultDataTable = Convert2DArraytoDatatableHeader(resultHeader, result);
                                }
                                else
                                {
                                    resultDataTable = Convert2DArraytoDatatable(result);
                                }
                                Value.Set(context, resultDataTable);


                                if (true == NeedToClose)
                                {
                                    workBookObject.Close();
                                }
                                if (false == NeedToClose && false == NeedToOpen)
                                {
                                    workBookObject.Close();
                                }
                                if (false == NeedToClose && true == NeedToOpen)
                                {
                                    workBookObject.Close();
                                    ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                                }
                                if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                                {
                                    ExcelHelper.Shared.Dispose();
                                }
                            }
                        }
                        else
                        {
                            Log.Logger.LogData("Please provide valid range in activity Excel_Range_Read", LogLevel.Error);
                            if (!ContinueOnError)
                            {
                                context.Abort();
                            }

                        }

                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Range_Read", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Range_Read", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            //finally
            //{
            //    ReleaseObject(xlRange);
            //}

        }

        public DataTable Convert2DArraytoDatatableHeader(Object[,] resultHeader, Object[,] numbers)
        {
            int colCountHeader = resultHeader.GetLength(1);
            int rowCountHeader = resultHeader.GetLength(0);

            DataTable dtHeader = new DataTable();

            for (int i = 1; i <= rowCountHeader; i++)
            {
                for (int j = 1; j <= colCountHeader; j++)
                {
                    if (1 == i)
                    {
                        dtHeader.Columns.Add(resultHeader[i, j].ToString());
                    }
                }
            }
            int colCount = numbers.GetLength(1);
            int rowCount = numbers.GetLength(0);
            for (int i = 1; i <= rowCount; i++)
            {
                DataRow dr = dtHeader.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    dr[j - 1] = numbers[i, j];
                }
                dtHeader.Rows.Add(dr);
            }
            return dtHeader;

        }

        public DataTable Convert2DArraytoDatatable(Object[,] numbers)
        {
            DataTable dt = new DataTable();
            int colCount = numbers.GetLength(1);
            int rowCount = numbers.GetLength(0);
            dt = new DataTable();

            if (true == IsHeader)
            {
                DataRow dr = null;

                for (int i = 1; i <= rowCount; i++)
                {
                    if (1 != i)
                    {
                        dr = dt.NewRow();
                    }

                    for (int j = 1; j <= colCount; j++)
                    {
                        if (1 == i)
                        {
                            dt.Columns.Add(numbers[i, j].ToString());
                        }
                        else
                        {
                            dr[j - 1] = numbers[i, j];
                        }
                    }
                    if (1 != i)
                    {
                        dt.Rows.Add(dr);
                    }

                }
            }
            else
            {
                for (int c = 1; c <= colCount; c++)
                {
                    dt.Columns.Add("Column" + (c));
                }

                for (int i = 1; i <= rowCount; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 1; j <= colCount; j++)
                    {
                        dr[j - 1] = numbers[i, j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        //private void ReleaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch
        //    {
        //        obj = null;
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}
    }
}
