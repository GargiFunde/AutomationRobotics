// <copyright file=Workbook_RefreshAll company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>

using System.Linq;
using System.Activities;
using System.ComponentModel;
using Logger;
using System;
using System.Drawing;


namespace Bot.Activity.Excel
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Refresh 16 px.png")]
    [Designer(typeof(Excel_Workbook_RefreshAll_ActivityDesigner))]
    public class Excel_Workbook_RefreshAll : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("WorkBook Name")]
        [Description("Set WorkBook Name")]
        [RequiredArgument]
        public InArgument<string>WorkbookName { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ExcelHelper.Shared.GetApp().DisplayAlerts = false;
                string workbookName = WorkbookName.Get(context);
                ExcelHelper.Shared.GetWorkbookByName(workbookName, true).RefreshAll();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Workbook_RefreshAll", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
