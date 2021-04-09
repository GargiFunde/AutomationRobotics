using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using Logger;
using White.Core;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/WindowsGetDesktopWindowsTitles.png")]
    [Designer(typeof(WindowsGetDesktopWindowsTitles_ActivityDesigner))]
    public class WindowsGetDesktopWindowsTitles : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Output")]
        [DisplayName("Result File String Array")]
        [Description("Enter Result File String Array")]
        public OutArgument<string[]> ResultFileStringArray { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                List<White.Core.UIItems.WindowItems.Window> windows = Desktop.Instance.Windows();
                                            
                string[] files = new string[windows.Count];
                int j = 0;
                for (int i = 0; i < windows.Count; i++)
                {
                    if ("" != windows[i].Title)
                    {
                        files[j++] = windows[i].Title;                        
                    }
                }
                //j--;
                string[] newFile = new string[j];
                for (int i = 0,k=0; i < windows.Count; i++)
                {
                    if ("" != windows[i].Title)
                    {
                        newFile[k++] = windows[i].Title;
                    }
                }

                ResultFileStringArray.Set(context, newFile);
            }
            catch(Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Folder_FileList", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
