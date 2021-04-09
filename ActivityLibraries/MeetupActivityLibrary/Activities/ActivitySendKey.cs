using System;
using System.Activities;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(ActivitySendKeyDesigner1))]
    [ToolboxBitmap("Resources/SendKey.png")]
    public class SendKey : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> SendKeyValue { get; set; }
     
      
        protected override void Execute(NativeActivityContext context)
        {
            string sendKeyValue = string.Empty;
            try
            {
                sendKeyValue = SendKeyValue.Get(context);
                if (string.IsNullOrEmpty(sendKeyValue))
                {
                    Logger.Log.Logger.LogData("SendKey value is null or empty", Logger.LogLevel.Error);
                }
                else
                {
                    //ThreadInvoker.Instance.RunByUiThread(() =>
                    //{
                        SendKeys.SendWait(sendKeyValue);
                    //});
                    Logger.Log.Logger.LogData("SendKey value sent", Logger.LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity SendKey", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
