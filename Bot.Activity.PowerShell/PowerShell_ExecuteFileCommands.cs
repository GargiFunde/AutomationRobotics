using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.PowerShell
{
    /// <summary>
    /// RefMaterial: Enable Remote PowerShell Execution in C# - CodeProject.html
    /// Provided that user is having admin rights on each machine
    /// To enable remote PowerShell execution, need to do the following steps:
    ///Install PowerShell 4.0 on both Remote Host and Local Client. 4.0 is not mandatory. 2.0+ is OK. Actually, both Windows Server 2008 R2 and Windows 7 have PowerShell 2.0 installed by default.
    ///Add my Windows account to the Administrators group on both Remote Host and Local Client. If both machines are in the same domain, you can use your domain account; if both are in WORKGROUP, you can create one account on each machine with the same name and password, both in the Administrators group.
    ///Run the following command on Remote Host: winrm quickconfig. This command automatically analyzes and configures the WinRM service which is the core service for remote PowerShell execution.
    ///The winrm quickconfig command is just for quick setup on experimental environment. For production environment, you should carefully configure the permission and firewall according to [2] and [3].
    ///Note that you may encounter several issues when you execute winrm quickconfig or other comlets to enable PowerShell remoting. You can refer to [4] to find the solutions for the 3 most common issues.
    /// </summary>
    /// 

    //scriptParameters = "test ~=~ 123 ~;~ test1 ~=~ 234"
    //in ps file variable name is $test and its value is 123
    //string scriptParameters,

    [ToolboxBitmap("Resources/PowerShell.png")]
    [Designer(typeof(ActivityDesignerForPowerShell_ExecuteFileCommands))]
    public class PowerShell_ExecuteFileCommands : BaseNativeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string>FullFileName { get; set; }

        [Category("Input")]
        public InArgument<string> PSSessionFileParams { get; set; }//global parameters in file

        [Category("Input")]
        public InArgument<string> ComputerName { get; set; } //For localhost do not pass/set anything

        [Category("Input")]
        public InArgument<string> PSCommandLineArgs { get; set; }//method arguments

        [Category("Output")]
        public OutArgument<System.Collections.ObjectModel.Collection<PSObject>> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string pssessionFileParams = string.Empty;
                string pscommandLineArgs = string.Empty;
                string computername = string.Empty;
                string pscommandFileName = FullFileName.Get(context);
                if (PSSessionFileParams != null)
                {
                    pssessionFileParams = PSSessionFileParams.Get(context);
                }
                if (PSCommandLineArgs != null)
                {
                    pscommandLineArgs = PSCommandLineArgs.Get(context);
                }
                if (ComputerName != null)
                {
                     computername = ComputerName.Get(context);
                }
                PowerShellEngine psEngine = new PowerShellEngine();
                Collection<System.Management.Automation.ErrorRecord> lErrors = new Collection<System.Management.Automation.ErrorRecord>();
                System.Collections.ObjectModel.Collection<PSObject> results = psEngine.ExecuteScriptFile(pscommandFileName, pssessionFileParams, null, computername);
                Result.Set(context, results);
               // lErrors = psEngine.Streams.Error.ReadAll();
                
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity PowerShell_ExecuteFileCommands", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
