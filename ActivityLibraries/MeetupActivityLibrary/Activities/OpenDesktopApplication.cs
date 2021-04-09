using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Diagnostics;
using CommonLibrary.Interfaces;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/OpenDesktopApplication.png")]
    [Designer(typeof(OpenDesktopApplication_ActivityDesigner))]
    public class OpenDesktopApplication : BaseNativeActivity, IOpenApplicationInterface 
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID")]
        [Description("Enter Application ID")]
        public InArgument<string> ApplicationID { get; set; }

        [Category("Input")]
        [DisplayName("EXE Path")]
        [Description("Enter Desktop Application Executable Path")]
        public InArgument<string> EXEPath { get; set; }
        [Category("Input")]
        [DisplayName("Arguments")]
        [Description("Enter Arguments if any")]
        public InArgument<string> Arguments { get; set; }
        [Category("Output")]
        [DisplayName("LaunchResult")]
        [Description("Enter Launch Result")]
        public OutArgument<bool> LaunchResult { get; set; }
        [Category("Input")]
        [DisplayName("Time Out In Second")]
        [Description("Enter Time Out In Second's")]
        public InArgument<int> TimeOutInSecond { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {
            string AppId = string.Empty;
            string exepath = string.Empty;
            string arguments = string.Empty;
           
            try
            {
                AppId = ApplicationID.Get(context);
                if ((Arguments != null) && (Arguments.Expression != null))
                {
                    arguments = Arguments.Get(context);
                }
                if (AppId != string.Empty) //scraping time
                {
                    if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        if ((EXEPath != null) && (EXEPath.Expression != null))
                        {
                            exepath = EXEPath.Get(context);
                           
                            if(string.IsNullOrEmpty(arguments))
                            {
                                Process.Start(exepath);
                            }
                            else
                            {
                                Process.Start(exepath,arguments);
                            }
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, new object());
                            LaunchResult.Set(context, true);

                        }
                    }
                }
            }catch (Exception ex)
            {
                LaunchResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity OpenDesktopApplication", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
            
            //throw new NotImplementedException();
        }
    }
}
