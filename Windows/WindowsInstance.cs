using CommonLibrary;
using CommonLibrary.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace Bot.Activity.Windows
{
    public class WindowsInstance : IApplicationInterface
    {
        [DllImport("kernel32.dll")]
        public static extern bool TerminateJobObject(IntPtr hJob, uint uExitCode);
        public IntPtr Job { get; set; }
        public int ProcessId { get; set; }
        public string WindowTitle { get; set; }
        public string ApplicationId { get; set; }
       
        public void Close()
        {
            //if (Job != null)
            //    TerminateJobObject(Job, 0);
            //Job = IntPtr.Zero;
            Process process = Process.GetProcessById(ProcessId);
            if (process != null)
            {
                process.Kill();
            }
        }

       
        public void OnApplicationClose(object sender, System.Windows.Automation.AutomationEventArgs e)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(1);
                });
                WindowsInstance wininstance = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
               
                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(ApplicationId);
               // wininstance.Close();
            }
        }

    }
}
