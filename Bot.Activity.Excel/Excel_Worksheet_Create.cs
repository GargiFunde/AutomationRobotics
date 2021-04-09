using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/ExcelWorksheetCreate.png")]
    [Designer(typeof(Excel_Worksheet_Create_ActivityDesigner))]
    public class Excel_Worksheet_Create : BaseNativeActivity
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
        // public InArgument<string> BeforeWorksheet { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("After Worksheet")]
        [Description("Set Worksheet Name")]
        //  [RequiredArgument]
        public InArgument<string> AfterWorksheet { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Need To Close")]
        public bool NeedToClose { get; set; }
        //  public bool NeedToSave { get; set; }
        //  public OutArgument<Excel1.Worksheet> Worksheet { get; set; }

        public Excel_Worksheet_Create()
        {
            NeedToClose = true;
            //  NeedToSave = true;
        }
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                string workbookFullName = FilePath.Get(context);
                if (!workbookFullName.Contains("."))
                {
                    workbookFullName = workbookFullName + ".xlsx";
                }
                // string before = BeforeWorksheet.Get(context);
                string after = AfterWorksheet.Get(context);

                if (!File.Exists(workbookFullName))
                {
                    Logger.Log.Logger.LogData("Excel file does not exist :" + workbookFullName + " in activity Excel_Worksheet_Create", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                else
                {
                    ExcelHelper.Shared.Close_OpenedFile(workbookFullName);

                    string worksheetName = string.Empty;
                    string workbookName = Path.GetFileName(workbookFullName);
                    dynamic workBookObject = ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                    dynamic worksheets = workBookObject.Worksheets;
                    dynamic worksheetObject = null;
                    if (WorksheetName != null)
                    {
                        worksheetName = WorksheetName.Get(context);
                    }


                  //  if (string.IsNullOrEmpty(after))
                    //{
                        bool AfterWorkSheetfound = false;
                        // Loop through all worksheets in the workbook
                        foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in workBookObject.Worksheets)
                        {
                            // Check the name of the current sheet
                            if (sheet.Name == after)
                            {
                                AfterWorkSheetfound = true;
                                break; // Exit the loop now
                            }
                        }
                        if (false == AfterWorkSheetfound)
                        {
                            after = null;
                        }
                 //   }

                    bool value = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    if (value == false)
                    {

                        if (string.IsNullOrEmpty(after) || false == AfterWorkSheetfound)
                        {
                            worksheetObject = worksheets.Add();
                        }

                        else if (!string.IsNullOrEmpty(after) || false == AfterWorkSheetfound)
                        {
                            dynamic afterworksheet = worksheets.Item(after);

                            worksheetObject = worksheets.Add(Type.Missing, afterworksheet, 1, Type.Missing);
                        }

                        if (!String.IsNullOrEmpty(worksheetName))
                        {
                            worksheetObject.Name = worksheetName;
                        }
                        workBookObject.Save();
                      
                    }
                    else
                    {
                        Log.Logger.LogData("Worksheet is already exists in activity Excel_Worksheet_Create", LogLevel.Info);
                        if (!ContinueOnError) { context.Abort(); }
                    }


                    //bool value = ExcelHelper.Shared.GetWorksheetByName(workbookName, worksheetName, false) != null;
                    //if (value == false)
                    //{
                    //    //if ((string.IsNullOrEmpty(before))&&(string.IsNullOrEmpty(after)))
                    //    //{
                    //    //    worksheetObject = worksheets.Add();
                    //    //}


                    //    if (string.IsNullOrEmpty(after))
                    //    {
                    //        worksheetObject = worksheets.Add();
                    //    }
                    //    //else if((string.IsNullOrEmpty(before)) && (!string.IsNullOrEmpty(after)))
                    //    //{
                    //    //    dynamic afterworksheet = worksheets.Item(after);
                    //    //    //if(afterworksheet == null)
                    //    //    //{
                    //    //    //    Log.Logger.LogData("Error in getting after worksheet " + " in activity Worksheet_Create", LogLevel.Error);
                    //    //    //    context.Abort();
                    //    //    //}
                    //    //    worksheetObject = worksheets.Add(Type.Missing, afterworksheet, 1, Type.Missing);
                    //    //}
                    //    else if (!string.IsNullOrEmpty(after))
                    //    {
                    //        dynamic afterworksheet = worksheets.Item(after);
                    //        //if(afterworksheet == null)
                    //        //{
                    //        //    Log.Logger.LogData("Error in getting after worksheet " + " in activity Worksheet_Create", LogLevel.Error);
                    //        //    context.Abort();
                    //        //}
                    //        worksheetObject = worksheets.Add(Type.Missing, afterworksheet, 1, Type.Missing);
                    //    }
                    //    //else if ((!string.IsNullOrEmpty(before)) && (string.IsNullOrEmpty(after)))
                    //    //{
                    //    //    dynamic beforeworksheet = worksheets.Item(before);
                    //    //    //if (beforeworksheet == null)
                    //    //    //{
                    //    //    //    Log.Logger.LogData("Error in getting before worksheet " + " in activity Worksheet_Create", LogLevel.Error);
                    //    //    //    context.Abort();
                    //    //    //}
                    //    //    worksheetObject = worksheets.Add(beforeworksheet, Type.Missing, 1, Type.Missing);
                    //    //}
                    //    //else if ((!string.IsNullOrEmpty(before)) && (!string.IsNullOrEmpty(after)))
                    //    //{

                    //    //    dynamic beforeworksheet = worksheets.Item(before);
                    //    //    //if (beforeworksheet == null)
                    //    //    //{
                    //    //    //    Log.Logger.LogData("Error in getting before worksheet " + " in activity Worksheet_Create", LogLevel.Error);
                    //    //    //}
                    //    //    dynamic afterworksheet = worksheets.Item(after);
                    //    //    //if (afterworksheet == null)
                    //    //    //{
                    //    //    //    Log.Logger.LogData("Error in getting after worksheet " + " in activity Worksheet_Create", LogLevel.Error);
                    //    //    //    context.Abort();
                    //    //    //}
                    //    //    worksheetObject = worksheets.Add(beforeworksheet, afterworksheet, 1, Type.Missing);
                    //    //}
                    //    if (!String.IsNullOrEmpty(worksheetName))
                    //    {
                    //        worksheetObject.Name = worksheetName;
                    //    }

                    //    //if (NeedToSave == true)
                    //    //{
                    //    //    if ((workBookObject != null) && (worksheetObject != null))
                    //    //    {
                    //    workBookObject.Save();
                    //    //    }
                    //    //}
                    //}
                    //else
                    //{
                    //    Log.Logger.LogData("Worksheet is already exists in activity Excel_Worksheet_Create", LogLevel.Info);
                    //}
                    //if (NeedToClose == true)
                    //{
                    //    if ((workBookObject != null) && (worksheetObject != null))
                    //    {
                    //        workBookObject.Close();
                    //        if (ExcelHelper.Shared.GetApp().Workbooks.Count == 0)
                    //        {
                    //            ExcelHelper.Shared.Dispose();
                    //        }
                    //    }
                    //}

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
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Excel_Worksheet_Create", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }

    }
}
