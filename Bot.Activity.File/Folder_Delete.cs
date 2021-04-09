// <copyright file=Folder_Delete company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_Delete company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FolderDelete.png")]
    [Designer(typeof(Folder_Delete_ActivityDesigner))]
    public class Folder_Delete : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Path Of The Folder")]
        public InArgument<string> FolderPath { get; set; }
       
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string folderPath = FolderPath.Get(context);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
                else 
                {
                    Log.Logger.LogData("Folder does not exist in path - " + folderPath + " in activity Folder_Delete", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }

                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Delete", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
