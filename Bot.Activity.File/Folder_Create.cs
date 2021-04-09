// <copyright file=Folder_Create company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_Create company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FolderCreate.png")]
    [Designer(typeof(Folder_Create_ActivityDesigner))]
    public class Folder_Create : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Path Of The Folder")]
        public InArgument<string> FolderPath { get; set; }


        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Name")]
        [Description("Name Of The Folder")]
        public InArgument<string> FolderName { get; set; }



        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string folderName = FolderName.Get(context);
                string folderPath = FolderPath.Get(context);
                string rawPath = folderPath;
                if (Directory.Exists(folderPath))
                {
                    folderPath = folderPath + "\\" + folderName;

                    if (Directory.Exists(folderPath))
                    {
                        Log.Logger.LogData("Folder already exists in path - " + folderPath + " in activity Folder_Create", LogLevel.Error);
                        if (!ContinueOnError) { context.Abort(); }
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                }
                else
                {
                    Log.Logger.LogData("Path does not exist - " + rawPath + " in activity Folder_Create", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                
                
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Create", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
