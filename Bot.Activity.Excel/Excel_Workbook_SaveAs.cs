using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorkbookSaveAs.png")]
    [Designer(typeof(Excel_Workbook_Save_ActivityDesigner))]
    public class Excel_Workbook_SaveAs : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Old File Path")]
        [Description("Set Old File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("New File Path")]
        [Description("Set new file path without name and extensions")]
        [RequiredArgument]
        public InArgument<string> NewFilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("New File Name")]
        [Description("Set new file name without extensions")]
        [RequiredArgument]
        public InArgument<string> NewFileName { get; set; }

        //[Category("Input Paramaters")]
        //[DisplayName("File Format")]
        //[DefaultValue(".xlsx")]
        //public XlFileFormat FileFormatType { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("File Format")]
        [DefaultValue("xlsx_WorkbookDefault")]
        public FileExtension FileFormatType { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Open")]
        public bool NeedToOpen { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Override")]
        public bool IsOverride { get; set; }
        public Excel_Workbook_SaveAs()
        {
            NeedToOpen = true;
            NeedToClose = true;
            IsOverride = false;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                String workbookFullName = FilePath.Get(context);
                String newFileName = NewFileName.Get(context);
                string newfilePath = NewFilePath.Get(context);

                string fileFormat = ToDescriptionString(FileFormatType);
                String[] fileFormatTypeStr = FileFormatType.ToString().TrimStart().Split('_');
                String extension = String.Concat(".", fileFormatTypeStr[0]);
                string newFileNameWithExtension = String.Concat(newFileName, extension);

                string workbookName = string.Empty;
                dynamic xlWorkbook = null;
                bool excelFileVisible = false;

                if (true == NeedToOpen)
                {
                    excelFileVisible = true;
                }

                // Combine 2 path parts.
                string workbooknameas = Path.Combine(newfilePath, newFileName);
                string workbooknameasWithExtension = Path.Combine(newfilePath, newFileNameWithExtension);

                if (File.Exists(workbookFullName))
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);
                    workbookName = Path.GetFileName(workbookFullName);

                    ExcelHelper.Shared.GetApp(excelFileVisible).DisplayAlerts = false;
                    xlWorkbook = ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);

                    if (File.Exists(workbooknameasWithExtension))
                    {
                        if (true == IsOverride)
                        {
                            // xlWorkbook.SaveAs(workbooknameas, FileFormatType, Missing.Value, Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
                            XlFileFormat fm = (XlFileFormat)Enum.Parse(typeof(XlFileFormat), fileFormat);
                            xlWorkbook.SaveAs(workbooknameas, fm, Missing.Value, Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);

                        }
                        else
                        {
                            Log.Logger.LogData("The file \"" + newFileName + "\" is already exists in activity Excel_Workbook_SaveAs", LogLevel.Error);
                            if (!ContinueOnError) { context.Abort(); }
                        }
                    }
                    else
                    {
                        // xlWorkbook.SaveAs(workbooknameas, XlFileFormat.xlWorkbookDefault, Missing.Value, Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
                        //xlWorkbook.SaveAs(workbooknameas, FileFormatType, Missing.Value,Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange,XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
                        XlFileFormat fm = (XlFileFormat)Enum.Parse(typeof(XlFileFormat), fileFormat);
                        xlWorkbook.SaveAs(workbooknameas, fm, Missing.Value, Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);

                    }
                    if (true == NeedToClose)
                    {
                        xlWorkbook.Close();
                    }
                    if (false == NeedToClose && false == NeedToOpen)
                    {
                        xlWorkbook.Close();
                    }
                    if (false == NeedToClose && true == NeedToOpen)
                    {
                        xlWorkbook.Close();
                        ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Open(workbookFullName);
                    }
                    if (ExcelHelper.Shared.GetApp(excelFileVisible).Workbooks.Count == 0)
                    {
                        ExcelHelper.Shared.Dispose();
                    }
                }
                else
                {
                    Log.Logger.LogData("Excel file does not exist:\"" + workbookFullName + "\" in activity Excel_Workbook_SaveAs", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Workbook_SaveAs", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }

        public enum FileExtension
        {
            [Description("xlWorkbookDefault")]
            xlsx_WorkbookDefault,

            [Description("xlCSV")]
            csv_CommaSeparatedValue,

            [Description("xlCSVMac")]
            csv_MacintoshCSV,

            [Description("xlCSVMSDOS")]
            csv_MSDOSCSV,

            [Description("xlCSVWindows")]
            csv_WindowsCSV,

            [Description("xlCurrentPlatformText")]
            txt_CurrentPlatformText,

            [Description("xlDIF")]
            dif_DataInterchangeFormat,

            [Description("xlExcel12")]
            xlsb_ExcelBinaryWorkbook,

            [Description("xlExcel5")]
            xls_Excelversion5Or1994,

            [Description("xlExcel8")]
            xls_Excel97to2003Workbook,

            [Description("xlHtml")]
            html_HTMLFormat,

            [Description("xlOpenXMLTemplate")]
            xltx_OpenXMLTemplate,

            [Description("xlOpenXMLTemplateMacroEnabled")]
            xltm_OpenXMLTemplateMacroEnabled,

            [Description("xlOpenXMLWorkbook")]
            xlsx_OpenXMLWorkbook,

            [Description("xlOpenXMLWorkbookMacroEnabled")]
            xlsm_OpenXMLWorkbookMacroEnabled,

            [Description("xlSYLK")]
            slk_SymbolicLinkformat,

            [Description("xlTemplate8")]
            xlt_Template8,

            [Description("xlTextMac")]
            txt_MacintoshText,

            [Description("xlTextMSDOS")]
            txt_MSDOSText,

            [Description("xlTextPrinter")]
            prn_PrinterText,

            [Description("xlTextWindows")]
            txt_WindowsText,

            [Description("xlUnicodeText")]
            txt_UnicodeText,

            [Description("xlWebArchive")]
            mhtml_WebArchive,

            [Description("xlWorkbookNormal")]
            xls_WorkbookNormal,

            [Description("xlXMLSpreadsheet")]
            xml_XMLSpreadsheet
        }
        public static string ToDescriptionString(FileExtension val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
