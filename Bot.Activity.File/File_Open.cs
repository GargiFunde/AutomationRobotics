using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Logger;
using System.Diagnostics;
using System.Drawing;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FileOpen.png")]
    [Designer(typeof(File_Open_ActivityDesigner))]
    public class File_Open : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("File")]
        [Description("Get File Object")]
        public OutArgument<Process>FileObject { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                Process process = Process.Start(filepath);
                FileObject.Set(context, process);

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Open", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
        }
    }
}
