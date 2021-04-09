using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Logger;
using System;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Linq;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelCutpaste.png")]
    [Designer(typeof(Excel_Cutpaste_ActivityDesigner))]
    public class Excel_CutPaste : BaseNativeActivity
    {
        [Category("Input Paramaters : Cut")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> CutCopyModeFilePath { get; set; }

        [Category("Input Paramaters : Cut")]
        [DisplayName("Worksheet Name")]
        [Description("Set Worksheet Name")]
        [RequiredArgument]
        public InArgument<string> CutCopyModeWorksheetName { get; set; }

        [Category("Input Paramaters : Cut")]
        [DisplayName("Cells")]
        [Description("eg.\"A1\" or \"A1:D4\"")]
        [RequiredArgument]
        public InArgument<string> CutCopyModeCells { get; set; }

        [Category("Input Paramaters : Paste")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters : Paste")]
        [DisplayName("Worksheet Name")]
        [Description("Set Worksheet Name")]
        [RequiredArgument]
        public InArgument<string> WorksheetName { get; set; }

        [Category("Input Paramaters : Paste")]
        [DisplayName("Cells")]
        [Description("eg.\"A1\" or \"A1:D4\"")]
        [RequiredArgument]
        public InArgument<string> Cells { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Result")]
        [Description("Get Result")]
        public OutArgument<bool> Result { get; set; }
        public Excel_CutPaste()
        {
            NeedToOpen = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                bool excelFileVisible = false;
                string cutCopyModeFilePath = CutCopyModeFilePath.Get(context);

                if (File.Exists(cutCopyModeFilePath))
                {
                    if (true == NeedToOpen)
                    {
                        excelFileVisible = true;
                    }
                    ExcelHelper.Shared.Close_OpenedFile(cutCopyModeFilePath);
                    ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;

                    string workbookNamecutCopyMode = string.Empty;
                    workbookNamecutCopyMode = Path.GetFileName(cutCopyModeFilePath);
                    string worksheetNamecutCopyMode = CutCopyModeWorksheetName.Get(context);
                    dynamic workBookObjectcutCopyMode = null;

                    workBookObjectcutCopyMode = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(cutCopyModeFilePath);
                    bool sheetExistcutCopyMode = ExcelHelper.Shared.GetWorksheetByName(workbookNamecutCopyMode, worksheetNamecutCopyMode, false) != null;
                    if (false == sheetExistcutCopyMode)
                    {
                        if (NeedToClose == true)
                        {
                            workBookObjectcutCopyMode.Close();
                        }
                        if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                        {
                            ExcelHelper.Shared.Dispose();
                        }
                        if (!ContinueOnError) { context.Abort(); }
                        Log.Logger.LogData("Worksheet \"" + worksheetNamecutCopyMode + "\" does not exist in activity Excel_CutPaste", LogLevel.Error);
                    }
                    else
                    {
                        string workbookFullName = FilePath.Get(context);
                        if (File.Exists(workbookFullName))
                        {
                            ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                            ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;

                            string workbookName = string.Empty;
                            workbookName = Path.GetFileName(workbookFullName);
                            string worksheetName = WorksheetName.Get(context);
                            dynamic workBookObject = null;

                            workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                            bool sheetExist = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                            if (false == sheetExist)
                            {
                                if (NeedToClose == true)
                                {
                                    workBookObject.Close();
                                    workBookObjectcutCopyMode.Close();
                                }
                                if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                                {
                                    ExcelHelper.Shared.Dispose();
                                }
                                if (!ContinueOnError) { context.Abort(); }
                                Log.Logger.LogData("Worksheet \"" + worksheetName + "\" does not exist in activity Excel_CutPaste", LogLevel.Error);
                            }
                            else
                            {
                                string cells = Cells.Get(context);
                                string cellsCutCopyMode = CutCopyModeCells.Get(context);
                                Range xlRange = null;
                                Worksheet xlWorksheet = null;
                                Worksheet xlWorksheetCutCopyMode = null;
                                bool result = false;

                                workBookObject = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                                xlWorksheet = workBookObject.Sheets[worksheetName];
                                xlRange = xlWorksheet.Range[cells];

                                xlWorksheetCutCopyMode = workBookObjectcutCopyMode.Sheets[worksheetNamecutCopyMode];
                                Range xlRangeCutCopyMode = xlWorksheetCutCopyMode.Range[cellsCutCopyMode];

                                Range cutCopyRange = xlRangeCutCopyMode;
                                Range insertRange = xlRange;

                                result = cutCopyRange.Cut(insertRange);

                                if (cutCopyModeFilePath.Equals(workbookFullName))
                                {
                                    var range = xlWorksheet.get_Range("A1", "A1");
                                    range.Select();
                                    workBookObject.Save();
                                }

                                if (!cutCopyModeFilePath.Equals(workbookFullName))
                                {
                                    workBookObjectcutCopyMode.Save();
                                    workBookObjectcutCopyMode.Close();
                                    workBookObject.Save();

                                    var range = xlWorksheet.get_Range("A1", "A1");
                                    range.Select();
                                    workBookObject.Save();
                                }

                                Result.Set(context, result);

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
                            Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_CutPaste", LogLevel.Error);
                            if (!ContinueOnError) { context.Abort(); }
                        }
                    }

                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + cutCopyModeFilePath + "\" in activity Excel_CutPaste", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity Excel_CutPaste", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
