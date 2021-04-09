using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using CommonLibrary;

namespace Bot.Activity.Web
{
    [ToolboxBitmap("Resources/WebGoTo.png")]
    [Designer(typeof(Web_GoTo_ActivityDesigner))]

    public class Web_GoTo: BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach*")]
        [Description("Enter Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [RequiredArgument]
        [Category("Input ")]
        [DisplayName("Url*")]
        [Description("Enter Url")]
        public InArgument<string> Url { get; set; }

        //[Category("Input ")]
        //[DisplayName("Time Out In Second")]
        //[Description("Enter Time Out In Second")]
        //public InArgument<int> TimeOutInSecond { get; set; }

        //[Category("Output ")]
        //[DisplayName("Go To Url Result")]
        //[Description("Enter Go To Url Result")]
        //public OutArgument<string> GoToUrlResult { get; set; }

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

                if (AppId != string.Empty)
                {

                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                        commonWebBrowser_Current.driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeInSec);
                        commonWebBrowser_Current.driver.Navigate().GoToUrl(searchUrl);

                        //GoToUrlResult.Set(context, commonWebBrowser_Current.driver.Url);
                    }
                }

            }
            catch (Exception ex)
            {
                //GoToUrlResult.Set(context, "Error");

                if (ex.HResult == -2146233088)
                {
                    Logger.Log.Logger.LogData("Page load timeout in activity Web_GoTo", Logger.LogLevel.Info);
                }
                else
                {
                    Logger.Log.Logger.LogData(ex.Message + " in activity Web_GoTo", Logger.LogLevel.Error);
                }

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
