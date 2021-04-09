using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using Logger;
using System.Drawing;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/IE_Back.png")]
    [Designer(typeof(WebControlProperties1))]
    public class IE_Back : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input ")]
        [DisplayName("Application ID To Attach*")]
        [Description("Enter Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input ")]
        [DisplayName("Time Out In Second")]
        [Description("Enter Time Out In Second")]
        public InArgument<int> TimeOutInSecond { get; set; }

        public OutArgument<bool> BackResult { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);

            if (0 == timeInSec)
            {
                timeInSec = 30;
            }

            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.Back();
                        IEWATIN.WaitForComplete(timeInSec);

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        BackResult.Set(context, true);
                    }
                }

            }
            catch (Exception ex)
            {
                BackResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity Back", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }


    }
}
