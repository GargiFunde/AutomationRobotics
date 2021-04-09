// <copyright file=Folder_GetInformation company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_GetInformation company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FolderGetInformation.png")]
    [Designer(typeof(Folder_GetInformation_ActivityDesigner))]
    public class Folder_GetInformation : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Set Path For The Folder")]
        public InArgument<string> FolderPath { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Name")]
        [Description("Folder Name")]
        public OutArgument<string> FolderName { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Count")]
        [Description("Folder Count")]
        public OutArgument<int> FolderCount { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("File Count")]
        [Description("File Count")]
        public OutArgument<int> FileCount { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.DateTime")]
        [DisplayName("Creation Time")]
        [Description("Creation Time")]
        public OutArgument<DateTime> CreationTime { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.DateTime")]
        [DisplayName("Last Access Time")]
        [Description("Last Access Time")]
        public OutArgument<DateTime> LastAccessTime { get; set; }
        [Category("Output Parameters")]
        [DefaultValue("System.DateTime")]
        [DisplayName("Last Write Time")]
        [Description("Last Write Time")]
        public OutArgument<DateTime> LastWriteTime { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string folderpath = FolderPath.Get(context);
                if (Directory.Exists(folderpath))
                {
                    var folderInfo = new FileInfo(folderpath);
                    var dirInfo = new DirectoryInfo(folderpath);
                    FolderName.Set(context, folderInfo.Name);
                    // FolderCount.Set(context, folderInfo.Extension);
                    // FileCount.Set(context, folderInfo.Length);
                    FolderCount.Set(context, dirInfo.GetDirectories().Length);
                    FileCount.Set(context, dirInfo.GetFiles().Length);
                    CreationTime.Set(context, folderInfo.CreationTime);
                    LastAccessTime.Set(context, folderInfo.LastAccessTime);
                    LastWriteTime.Set(context, folderInfo.LastWriteTime);
                }
                else if(!Directory.Exists(folderpath))
                {
                   
                    Log.Logger.LogData("Folder does not exist in path - " + folderpath + " in activity Folder_GetInformation", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                
               
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_GetInformation", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
