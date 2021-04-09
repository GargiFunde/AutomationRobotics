//using System.Linq;
//using System.Activities;
//using System.ComponentModel;
//using System.IO;
//using Logger;
//using System;
//using ExcelObject = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab
//using System.Drawing;
//using System.Data;

//namespace Bot.Activity.Excel
//{
//    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/rangewrite.png")]
//    [Designer(typeof(Excel_Range_Write_ActivityDesigner))]
//    public class Excel_Range_Write : BaseNativeActivity
//    {
//        [Category("Input Paramaters")]
//        [DisplayName("File Path")]
//        [Description("Set File Path")]
//        [RequiredArgument]
//        public InArgument<string> FilePath { get; set; }

//        [Category("Input Paramaters")]
//        [DisplayName("Worksheet Name")]
//        [Description("Set Worksheet Name")]
//        [RequiredArgument]
//        public InArgument<string> WorksheetName { get; set; }

//        [Category("Input Paramaters")]
//        [DisplayName("Starting Cell")]
//        [Description("eg.\"A1\"")]
//        public InArgument<string> StartingCell { get; set; }

//        [Category("Input Paramaters")]
//        [DisplayName("Value")]
//        [Description("Set Write Value")]
//        [RequiredArgument]
//        public InArgument<dynamic> Value { get; set; }

//        [Category("Output Paramaters")]
//        [DisplayName("Status")]
//        [Description("Get Write Range Status")]
//        public OutArgument<bool> Result { get; set; }

//        [Category("Common Parameters")]
//        [DisplayName("Need To Close")]
//        public bool NeedToClose { get; set; }
//        public Excel_Range_Write()
//        {
//            NeedToClose = true;
//        }
//        protected override void Execute(NativeActivityContext context)
//        {
//            string workbookFullName = FilePath.Get(context);
//            string workSheetName = WorksheetName.Get(context);
//            ExcelObject.Range xlRange = null;
//            dynamic workBookObject = null;
//            DataTable inputDt = null;

//            try
//            {
//                string workbookName = string.Empty;
//                ExcelObject._Worksheet xlWorksheet = null;

//                if (File.Exists(workbookFullName))
//                {
//                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;
//                    workbookName = Path.GetFileName(workbookFullName);
//                    string worksheetName = WorksheetName.Get(context);


//                    workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
//                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
//                    if (false == sheetExist)
//                    {
//                        Result.Set(context, false);
//                        workBookObject.Close();
//                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
//                        {
//                            ExcelHelper.Shared.Dispose();
//                        }
//                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Range_Write", LogLevel.Error);
//                    }
//                    else
//                    {
//                        inputDt = Value.Get(context);

//                        xlWorksheet = workBookObject.Sheets[workSheetName];
//                        string startingCell = StartingCell.Get(context);

//                        int totalRows = inputDt.Rows.Count;
//                        int totalColumns = inputDt.Columns.Count;

//                        //String endAlpa = startingCell.Substring(0);

//                        String endAlpa = new String(startingCell.Where(c => Char.IsLetter(c)).ToArray()).ToUpper();

//                        int rangeEndColNum = ExcelHelper.Shared.NumberFromExcelColumn(endAlpa) - 1 + totalColumns;
//                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(rangeEndColNum);

//                        String endNum = new String(startingCell.Where(c => Char.IsDigit(c)).ToArray());

//                        int rangeEndNum = int.Parse(endNum) - 1 + totalRows;
//                        string endingCell = colLetter + rangeEndNum;

//                        if (null == inputDt)
//                        {
//                            Result.Set(context, false);
//                        }
//                        else
//                        {
//                            object[,] TwoDimensionalArray = ExcelHelper.Shared.ConvertDataTableToArray(inputDt);
//                            xlRange = xlWorksheet.Range[startingCell, endingCell];
//                            xlRange.Value = TwoDimensionalArray;
//                            Result.Set(context, true);
//                            workBookObject.Save();
//                        }

//                    }
//                }
//                else
//                {
//                    Result.Set(context, false);
//                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Range_Write", LogLevel.Error);

//                    if (!ContinueOnError)
//                    {
//                        context.Abort();
//                    }

//                }

//                if (NeedToClose == true)
//                {
//                    workBookObject.Close();
//                }

//                if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
//                {
//                    ExcelHelper.Shared.Dispose();
//                }

//            }
//            catch (Exception ex)
//            {
//                Result.Set(context, false);
//                Log.Logger.LogData(ex.Message + " in activity Excel_Range_Write", LogLevel.Error);
//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                }
//            }

//            finally
//            {
//                ReleaseObject(xlRange);
//                //if (NeedToClose == true)
//                //{
//                //    ReleaseObject(workBookObject); 
//                //}
//            }
//        }
//        private void ReleaseObject(object obj)
//        {
//            try
//            {
//                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
//                obj = null;
//            }
//            catch
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

using System.Linq;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Logger;
using System;
using ExcelObject = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab
using System.Drawing;
using System.Data;
using DataTable = System.Data.DataTable;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelRangeWrite.png")]
    [Designer(typeof(Excel_Range_Write_ActivityDesigner))]
    public class Excel_Range_Write : BaseNativeActivity
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
        [DisplayName("Starting Cell")]
        [Description("eg.\"A1\"")]
        public InArgument<string> StartingCell { get; set; }
        [Category("Input Paramaters")]
        [DisplayName("Value")]
        [Description("Set Write Value")]
        [RequiredArgument]
        public InArgument<DataTable> Value { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Status")]
        [Description("Get Write Range Status")]
        public OutArgument<bool> Result { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_Range_Write()
        {
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            string workbookFullName = FilePath.Get(context);
            string workSheetName = WorksheetName.Get(context);
            ExcelObject.Range xlRange = null;
            dynamic workBookObject = null;
            DataTable inputDt = null;

            try
            {
                string workbookName = string.Empty;
                ExcelObject._Worksheet xlWorksheet = null;

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                    workbookName = Path.GetFileName(workbookFullName);
                    string worksheetName = WorksheetName.Get(context);

                    workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        Result.Set(context, false);
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_Range_Write", LogLevel.Error);
                    }
                    else
                    {
                        inputDt = Value.Get(context);

                        xlWorksheet = workBookObject.Sheets[workSheetName];
                        string startingCell = StartingCell.Get(context);

                        if (startingCell == null)
                        {
                            startingCell = "A1";
                        }

                        int totalRows = inputDt.Rows.Count;
                        int totalColumns = inputDt.Columns.Count;

                        //String endAlpa = startingCell.Substring(0);

                        String endAlpa = new String(startingCell.Where(c => Char.IsLetter(c)).ToArray()).ToUpper();

                        int rangeEndColNum = ExcelHelper.Shared.NumberFromExcelColumn(endAlpa) - 1 + totalColumns;
                        string colLetter = ExcelHelper.Shared.ColumnIndexToColumnLetter(rangeEndColNum);

                        String endNum = new String(startingCell.Where(c => Char.IsDigit(c)).ToArray());

                        int rangeEndNum = int.Parse(endNum) - 1 + totalRows;
                        string endingCell = colLetter + rangeEndNum;

                        if (null == inputDt)
                        {
                            Result.Set(context, false);
                        }
                        else
                        {
                            object[,] TwoDimensionalArray = ExcelHelper.Shared.ConvertDataTableToArray(inputDt);
                            xlRange = xlWorksheet.Range[startingCell, endingCell];
                            xlRange.Value = TwoDimensionalArray;
                            Result.Set(context, true);

                            var range = xlWorksheet.get_Range("A1", "A1");
                            range.Select();

                            workBookObject.Save();
                        }
                        if (true == NeedToClose)
                        {
                            workBookObject.Close();
                        }
                        if (false == NeedToClose)
                        {
                            workBookObject.Close();
                            ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                        }
                        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                    }
                }
                else
                {
                    Result.Set(context, false);
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Range_Write", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity Excel_Range_Write", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

            finally
            {
                ReleaseObject(xlRange);
                //if (NeedToClose == true)
                //{
                //    ReleaseObject(workBookObject); 
                //}
            }
        }
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

