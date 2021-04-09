// <copyright file=Folder_Exists company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_Exists company=E2E BOTS>
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
using System.Drawing;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/Folderexists.png")]
    [Designer(typeof(Folder_Exists_ActivityDesigner))]
    public class Folder_Exists : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Path Of The Folder")]
        public InArgument<string> FolderPath { get; set; }
        [RequiredArgument]
        [Category("Output Parameters")]
        [DefaultValue("System.Bool")]
        [DisplayName("Folder Exists")]
        [Description("Folder Exists")]
        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string folderPath = FolderPath.Get(context);
                bool result = Directory.Exists(folderPath);
                Result.Set(context, result);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Exists", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
