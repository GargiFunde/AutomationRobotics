// <copyright file=File_Delete company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_Delete company=E2E BOTS>
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
using Logger;
using System;
using System.Drawing;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FileDelete.png")]
    [Designer(typeof(FileDelete_ActivityDesigner))]
    public class File_Delete : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                else
                {
                    Log.Logger.LogData("The file " + filepath + " does not exist in activity File_Delete", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Delete", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
