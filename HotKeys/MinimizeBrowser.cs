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
    [Designer(typeof(MinimizeBrowser_ActivityDesigner))]
    [ToolboxBitmap("Resources/MinimizeBrowser.png")]

    public class MinimizeBrowser : BaseNativeActivity
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

                if (AppId!=null)
                {
                    CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                    commonWebBrowser_Current.driver.Manage().Window.Minimize();
                     //commonWebBrowser_Current.driver.Manage().Window.Position = new Point(-2000, 0); 
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
