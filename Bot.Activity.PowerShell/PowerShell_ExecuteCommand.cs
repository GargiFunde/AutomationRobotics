using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Management.Automation;

namespace Bot.Activity.PowerShell
{
    [ToolboxBitmap("Resources/PowerShell.png")]
    [Designer(typeof(ActivityDesignerForPowerShell_ExecuteCommand))]
    public class PowerShell_ExecuteCommand : BaseNativeActivity
    {

        [RequiredArgument]
        public InArgument<string> UserName { get; set; }
        [RequiredArgument]
        public InArgument<string> Password { get; set; }
        [RequiredArgument]
        public InArgument<string> ComputerName { get; set; } //For localhost do not pass/set anything
        [RequiredArgument]
        public InArgument<string> PSCommand { get; set; }

        public OutArgument<System.Collections.ObjectModel.Collection<PSObject>> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string pscommand = PSCommand.Get(context);
                string username = UserName.Get(context);
                string password = Password.Get(context);
                string computername = ComputerName.Get(context);
                System.Security.SecureString s = new System.Security.SecureString();
                foreach (char c in password)
                {
                    s.AppendChar(c);
                }
                PowerShellEngine psEngine = new PowerShellEngine();
                PSCredential psc = new PSCredential(username, s);

                System.Collections.ObjectModel.Collection<PSObject> results = psEngine.ExecuteScript(pscommand, psc, null, computername);
                Result.Set(context, results);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity PowerShell_ExecuteCommand", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
