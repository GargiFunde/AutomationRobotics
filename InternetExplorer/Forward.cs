using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Drawing;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/IE_Forward.png")]
    [Designer(typeof(ActivityDesignerForForward))]
    public class IE_Forward : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [DisplayName("Wait Until Contain Text")]
        public InArgument<string> WaitUntilContainText { get; set; }
        public InArgument<int> TimeOutInSecond { get; set; }

        public OutArgument<bool> ForwardResult { get; set; }


        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            string strWaitUntilContainText = string.Empty;
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.Forward();
                        if (WaitUntilContainText.Get(context) != null)
                        {
                            strWaitUntilContainText = WaitUntilContainText.Get(context).ToString();
                            IEWATIN.WaitUntilContainsText(strWaitUntilContainText, timeInSec);
                        }
                        else
                        {
                            IEWATIN.WaitForComplete(timeInSec);
                        }

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        ForwardResult.Set(context, true);
                    }
                }

            }
            catch (Exception ex)
            {
                ForwardResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity Forward", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }


    }
}
