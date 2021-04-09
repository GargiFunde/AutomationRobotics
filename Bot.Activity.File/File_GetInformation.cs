// <copyright file=File_GetInformation company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_GetInformation company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FileGetInformation.png")]
    [Designer(typeof(File_GetInformation_ActivityDesigner))]
    public class File_GetInformation : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("File Name")]
        [Description("Get File Name")]
        public OutArgument<string> FileName { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("File Extension")]
        [Description("Get FileExtension")]
        public OutArgument<string> FileExtension { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("File Size")]
        [Description("Get File Size")]
        public OutArgument<long> FileSize { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Creation Time")]
        [Description("Get Creation Time")]
        public OutArgument<DateTime> CreationTime { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("LastAccess Time")]
        [Description("Get LastAccess Time")]
        public OutArgument<DateTime> LastAccessTime { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("LastWrite Time")]
        [Description("Get LastWrite Time")]
        public OutArgument<DateTime> LastWriteTime { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                var fileInfo = new FileInfo(filepath);
                FileName.Set(context, fileInfo.Name);
                FileExtension.Set(context, fileInfo.Extension);
                FileSize.Set(context, fileInfo.Length);
                CreationTime.Set(context, fileInfo.CreationTime);
                LastAccessTime.Set(context, fileInfo.LastAccessTime);
                LastWriteTime.Set(context, fileInfo.LastWriteTime);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_GetInformation", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
        }
    }
}
