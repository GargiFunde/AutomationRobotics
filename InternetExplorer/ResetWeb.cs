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
    [ToolboxBitmap("Resources/IE_Reset.png")]
    [Designer(typeof(IE_ResetUrlActivityDesigner))]
    public class IE_Reset : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string>ApplicationIDToAttach { get; set; }

        [RequiredArgument]
        public InArgument<string> ResetUrl { get; set; }

        public InArgument<int> TimeOutInSecond { get; set; }

        public OutArgument<bool> ResetResult { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            string searchUrl = ResetUrl.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.GoTo(searchUrl);
                        IEWATIN.WaitForComplete(5000);

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        ResetResult.Set(context, true);
                    }
                }
               
            }
            catch (Exception ex)
            {
               // Log.Logger.LogData(ex.Message, LogLevel.Error);
                ResetResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity RefreshWeb", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

       
    }
}
