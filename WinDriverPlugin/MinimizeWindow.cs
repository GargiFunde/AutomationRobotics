using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Logger;
using System.Drawing;

namespace Bot.Activity.WinDriverPlugin
{
    [ToolboxBitmap("Resources/MaximizeWindow.png")]
    [Designer(typeof(MinimizeWindow_ActivityDesigner))]
    public class MinimizeWindow : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("Window ")]
        [Description("")]
        public InArgument<string> win { get; set; }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                if ((win.Get(context)) != null)
                {
                    Process[] proc = Process.GetProcessesByName(win.Get(context));

                    foreach (Process p in proc)
                    {
                        ShowWindow(p.MainWindowHandle, SW_SHOWMINIMIZED);
                    }
                }
                else
                {
                    IntPtr hwnd = GetForegroundWindow();
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    Process p = Process.GetProcessById((int)pid);
                    ShowWindow(p.MainWindowHandle, SW_SHOWMINIMIZED);
                }

            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception in MinimizeWindow  " + e.Message, LogLevel.Error);
            }


        }
    }
}
