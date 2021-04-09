// <copyright file=CellValue_Paste company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>

using System.Linq;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using Logger;
using System;
using System.IO;
//using Excel1 = Microsoft.Office.Interop.Excel;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Paste 16 px.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class Excel_CellValue_Paste : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> WorkbookFullName { get; set; }
        [RequiredArgument]
        public InArgument<string>WorksheetName { get; set; }
        [RequiredArgument]
        public InArgument<string>Cell { get; set; }
        [RequiredArgument]
        public XlPasteType PasteType { get; set; }
        public bool NeedToSave { get; set; }
        public bool NeedToClose { get; set; }
        public Excel_CellValue_Paste()
        {
            NeedToSave = true;
            NeedToClose = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                object misValue = System.Reflection.Missing.Value;
                string workbookFullName = WorkbookFullName.Get(context);
                string workbookName = string.Empty;
                //bool needToSave = NeedToSave.Get(context);
                //bool needToClose = NeedToClose.Get(context);
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;

                if (File.Exists(workbookFullName))
                {
                    workbookName = Path.GetFileName(workbookFullName);
                }
                else
                {
                    Log.Logger.LogData("Workbook file do not exist, Error in activity CellValue_Paste", LogLevel.Error);
                    context.Abort();
                }
                string worksheetName = WorksheetName.Get(context);
                string cell = Cell.Get(context);
               ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).PasteSpecial(PasteType);


              


                //if (PasteValuesOnly)
                //{
                //    ExcelHelper.Shared.GetRange(WorkbookName, WorksheetName, Cell).PasteSpecial(XlPasteType.xlPasteValuesAndNumberFormats);
                //}
                //else
                //{
                //    ExcelHelper.Shared.GetRange(WorkbookName, WorksheetName, Cell).PasteSpecial();
                //}
                if (NeedToSave == true)
                {
                    ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Save();
                }
                if (NeedToClose == true)
                {
                    ExcelHelper.Shared.GetWorkbookByName(workbookName, true).Close();
                }

                if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                {
                    ExcelHelper.Shared.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Execute", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        /// <summary>
        /// https://msdn.microsoft.com/en-us/vba/excel-vba/articles/xlpastetype-enumeration-excel
        /// </summary>
        public enum XlPasteType
        {
            xlPasteAll = -4104, //Everything will be pasted.
            xlPasteAllExceptBorders = 7, //Everything except borders will be pasted.
            xlPasteAllMergingConditionalFormats = 14, //Everything will be pasted and conditional formats will be merged.
            xlPasteAllUsingSourceTheme = 13, //Everything will be pasted using the source theme.
            xlPasteColumnWidths = 8, //Copied column width is pasted.
            xlPasteComments = -4144, //Comments are pasted.
            xlPasteFormats = -4122, //Copied source format is pasted.
            xlPasteFormulas = -4123, //Formulas are pasted.
            xlPasteFormulasAndNumberFormats = 11, //Formulas and Number formats are pasted.
            xlPasteValidation = 6, //Validations are pasted.
            xlPasteValues = -4163, //Values are pasted.
            xlPasteValuesAndNumberFormats = 12, //Values and Number formats are pasted.
        }
    }
}
