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
    [ToolboxBitmap("Resources/IE_Refresh.png")]
    [Designer(typeof(IE_Refresh_ActivityDesigner))]
    public class IE_Refresh : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        public InArgument<int> TimeOutInSecond { get; set; }

        public OutArgument<bool> RefreshResult { get; set; }


        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.Refresh();
                        IEWATIN.WaitForComplete(5000);

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        RefreshResult.Set(context, true);
                    }
                }

            }
            catch (Exception ex)
            {
                //Log.Logger.LogData(ex.Message, LogLevel.Error);
                RefreshResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity Refresh", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }


    }
}
