using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(Web_ExecuteJavaScript_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebExecuteJavaScript.png")]
    public class Web_ExecuteJavaScript : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        //[Category("Input")]
        //public InArgument<int> TimeOutInSecond { get; set; }

        [Category("Input")]
        [DisplayName("Java Script")]
        public InArgument<string> JavaScript { get; set; }

        [Category("Output")]
        [DisplayName("Output")]
        public OutArgument<object> JavaScriptOutput { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            object outputObj = null;
            string AppId = ApplicationIDToAttach.Get(context);
            string sJavaScript = JavaScript.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (!string.IsNullOrEmpty(sJavaScript))
                {
                    CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                    outputObj = commonWebBrowser_Current.driver.ExecuteScript(sJavaScript);

                    if (outputObj != null)
                    {
                        JavaScriptOutput.Set(context, outputObj);
                    }
                }
                else
                {
                    Logger.Log.Logger.LogData("Script is null or empty", Logger.LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in Web activity Web_ExecuteJavaScript", Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
