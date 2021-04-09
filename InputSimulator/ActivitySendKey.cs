using System;
using System.Activities;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SendKey.png")]
    [Designer(typeof(ActivitySendKeyDesigner1))]
    public class SendKey : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Send Key Value")]
        [Description("Enter Send Key Value")]
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
                    //Logger.Log.Logger.LogData("SendKey value sent", Logger.LogLevel.Info);
                }
            }
            catch (Exception ex)
            { 
                Logger.Log.Logger.LogData("Error in SendKey:" + sendKeyValue, Logger.LogLevel.Error);
                context.Abort();
            }
        }
    }
}
