using System;
using System.Activities;
using System.ComponentModel;
using Logger;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/Log.png")]
    [Designer(typeof(ActivityDesignerForLog))]
    public class Log : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Log Level")]
        [Description("Input")]
        [RequiredArgument]
        public LogLevel CsrId { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Message")]
        [Description("Input")]
        [RequiredArgument]
        public InArgument<Object> Message { get; set; }
        public Log() : base()
        {
            Message = new Object();
            this.DisplayName = "Message";
        }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string msg = null;
                if (this.Message.Get(context) != null)
                {
                    msg = this.Message.Get(context).ToString();

                }

                switch (CsrId)
                {
                    case LogLevel.Debug:
                        Logger.Log.Logger.LogData(msg, Logger.LogLevel.Debug);

                        break;
                    case LogLevel.Info:
                        Logger.Log.Logger.LogData(msg, Logger.LogLevel.Info);

                        break;
                    case LogLevel.Warning:
                        Logger.Log.Logger.LogData(msg, Logger.LogLevel.Warning);

                        break;
                    case LogLevel.Error:
                        Logger.Log.Logger.LogData(msg, Logger.LogLevel.Error);

                        break;
                    case LogLevel.Fatal:
                        Logger.Log.Logger.LogData(msg, Logger.LogLevel.Fatal);

                        break;


                    default:
                        //MessageBox.Show("Default");
                        //MessageBox.Show(Message.ToString());
                        break;
                }
            }

            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Log", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
