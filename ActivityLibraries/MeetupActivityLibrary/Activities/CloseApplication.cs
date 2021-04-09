using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using CommonLibrary.Interfaces;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/CloseApplication.png")]
    [Designer(typeof(CloseApplication_ActivityDesigner))]
    public class CloseApplication : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("ApplicationID*")]
        [Description("Set Application ID")]
        public InArgument<string> ApplicationID { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string strAppIdToAttach = ApplicationID.Get(context);
                if (!string.IsNullOrEmpty(strAppIdToAttach)) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strAppIdToAttach))
                    {
                        IApplicationInterface application = (IApplicationInterface)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strAppIdToAttach];
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(strAppIdToAttach);
                        application.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity CloseApplication", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
