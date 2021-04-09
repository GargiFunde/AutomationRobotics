using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Drawing;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/IE_GoTo.png")]
    [Designer(typeof(IE_GoToActivityDesigner))]
    public class IE_GoTo : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input ")]
        [DisplayName("Application ID To Attach*")]
        [Description("Enter Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [RequiredArgument]
        [Category("Input ")]
        [DisplayName("Url*")]
        [Description("Enter Url")]
        public InArgument<string> Url { get; set; }

        [Category("Input ")]
        [DisplayName("Time Out In Second")]
        [Description("Enter Time Out In Second")]
        public InArgument<int> TimeOutInSecond { get; set; }

        [Category("Output ")]
        [DisplayName("Go To Url Result")]
        [Description("Enter Go To Url Result")]
        public OutArgument<string> GoToUrlResult { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            string searchUrl = Url.Get(context);
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
                        IEWATIN.GoTo(searchUrl);
                        IEWATIN.WaitForComplete(timeInSec);

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        GoToUrlResult.Set(context, IEWATIN.Url);
                    }
                }

            }
            catch (Exception ex)
            {
                GoToUrlResult.Set(context, "Error");
                Logger.Log.Logger.LogData(ex.Message + " in activity GoTo", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }


    }
}
