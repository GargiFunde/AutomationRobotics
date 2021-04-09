using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
//using System.IO;
//using System.Management;
using System.Diagnostics;
using System.Drawing;
using Logger;

namespace KillProcess
{
    [ToolboxBitmap("Resources/killProcess.png")]
    [Designer(typeof(ActivityDesignerForKillProcess))]
    public class KillProcess : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Process Name")]
        public InArgument<String> ProcessName { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string processName = ProcessName.Get(context);
                Process[] foundProcess = Process.GetProcessesByName(processName);

                foreach (Process p in foundProcess)
                {
                    p.Kill();
                    Logger.Log.Logger.LogData("Process: "+p+ "Killed Successfully: " , LogLevel.Info);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception while Killing Process: "+e.Message,LogLevel.Error);
            }
        }
    }
}
