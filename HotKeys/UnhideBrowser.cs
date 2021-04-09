using Bot.Activity.Web;
using CommonLibrary;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotKeys
{
    public class UnHideBrowser : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            string AppId = ApplicationIDToAttach.Get(context);
            try
            {

                if (AppId != null)
                {
                    CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                    commonWebBrowser_Current.driver.Manage().Window.Position = new Point(20, 20);
                    
                }

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception in " + this.DisplayName + ": " + ex.Message, Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }

            }
        }
    }
}
