using System;
using System.Activities;
using System.ComponentModel;
using Logger;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelCellValueClear.png")]
    [Designer(typeof(Excel_CellValue_Clear_ActivityDesigner))]
    public class Excel_CellValue_Clear : BaseNativeActivity
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
        [DisplayName("Cell")]
        [Description("Set Cell Value")]
        [RequiredArgument]
        public InArgument<string> Cell { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        public Excel_CellValue_Clear()
        {
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = FilePath.Get(context);
                string worksheetName = WorksheetName.Get(context);
                string workbookName = string.Empty;
                bool excelFileVisible = false;
                
                if (true == NeedToOpen)
                {
                    excelFileVisible = true;
                }

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    workbookName = Path.GetFileName(workbookFullName);
                    ////
                    //FileStream stream = null;

                    //try
                    //{
                    //    stream = File.Open(workbookFullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    //}
                    //catch (IOException ex)
                    //{
                    //    ExcelHelper.Shared.Close_OpenedWorkbook(workbookName);
                    //}
                    //finally
                    //{
                    //    if (stream != null)
                    //        stream.Close();
                    //}


                    ////








                    ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;

                 
                    dynamic workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                    bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (false == sheetExist)
                    {
                        workBookObject.Close();
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_CellValue_Clear", LogLevel.Error);
                    }
                    else
                    {
                        string cell = Cell.Get(context);

                        ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).ClearContents();
                        ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).Clear();
                        workBookObject.Save();

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
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_CellValue_Clear", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_CellValue_Clear", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
