// <copyright file=CellValue_Copy company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>

using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Copy_24 px.png")]
    [Designer(typeof(ActivityDesigner1))]
   public class Excel_CellValue_Copy : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> WorkbookFullName { get; set; }
        [RequiredArgument]
        public InArgument<string>WorksheetName { get; set; }
        [RequiredArgument]
        public InArgument<string>Cell { get; set; }

        public bool NeedToOpen { get; set; }

        public Excel_CellValue_Copy()
        {
            NeedToOpen = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string workbookFullName = WorkbookFullName.Get(context);
                string workbookName = string.Empty;
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                // bool needToOpen = NeedToOpen.Get(context);
                if (File.Exists(workbookFullName))
                {
                    workbookName = Path.GetFileName(workbookFullName);
                }
                else
                {
                    Log.Logger.LogData("Workbook file do not exist, Error in activity CellValue_Copy", LogLevel.Error);
                    context.Abort();
                }
                if (NeedToOpen == true)
                {
                    ExcelHelper.Shared.GetApp().Workbooks.Open(workbookFullName);
                }
                string worksheetName = WorksheetName.Get(context);
                string cell = Cell.Get(context);
                ExcelHelper.Shared.GetRange(workbookName, worksheetName, cell).Copy();

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity CellValue_Copy", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}

    
