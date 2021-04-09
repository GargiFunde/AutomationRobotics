// <copyright file=File_Exists company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_Exists company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 14:56:05</date>
// <summary></summary>


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System;
using Logger;
using System.Drawing;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FileExists.png")]
    [Designer(typeof(File_Exists_ActivityDesigner))]
    public class File_Exists : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }


        [Category("Output Paramaters")]
        [DisplayName("Exists")]
        [Description("Get File Exists")]
        [RequiredArgument]
        public OutArgument<bool> Exists { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                bool result = File.Exists(filepath);
                Exists.Set(context, result);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Exists", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
