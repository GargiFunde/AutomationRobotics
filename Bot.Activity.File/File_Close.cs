// <copyright file=File_Create company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_Create company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 14:56:05</date>
// <summary></summary>


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
    [ToolboxBitmap("Resources/FileClose.png")]
    [Designer(typeof(File_Close_ActivityDesigner))]
    public class File_Close : BaseNativeActivity
    {
        //
        [Category("Input Paramaters")]
        [DisplayName("File")]
        [Description("Set File Object")]
        [RequiredArgument]
        public InArgument<Process>FileObject { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                Process process = FileObject.Get(context);
               // process.Close(); //not working
                process.Kill(); //Will not pop up save file dialog
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Close", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
        }
    }
}
