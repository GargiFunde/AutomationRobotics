// <copyright file=Folder_FileList company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_FileList company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FolderFileList.png")]
    [Designer(typeof(Folder_FileList_ActivityDesigner))]
    public class Folder_FileList : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Path Of The Folder")]
        public InArgument<string> FolderPath { get; set; }
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Filter On File Extension")]
        [Description("Set Filter eg. *.txt ")]
        public InArgument<string> FilterOnFileExtension { get; set; }
        [Category("Input Parameters")]
        [DefaultValue("System.Bool")]
        [DisplayName("All Sub Directories")]
        [Description("Include All Sub Directories ")]
        public bool AllSubDirectories { get; set; }
        [Category("Output Parameters")]
        
        [DisplayName("File List")]
        [Description("File List")]
        public OutArgument<string[]> ResultFileStringArray { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string[] files = null;
                string folderpath = FolderPath.Get(context); //@"C:\Work"
                string filter = FilterOnFileExtension.Get(context);
                if ((string.IsNullOrEmpty(folderpath)) || (folderpath.Trim().Length == 0))
                {
                    Log.Logger.LogData("Please enter folder path in activity Folder_FileList", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                else if (!Directory.Exists(folderpath))
                {
                    Log.Logger.LogData("Folder does not exist in path - "+folderpath+" in activity Folder_FileList", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
                else if (Directory.Exists(folderpath))
                {

                    if ((string.IsNullOrEmpty(filter)) || (filter.Trim().Length == 0))
                    {
                        filter = "*";
                    }
                    if (AllSubDirectories)
                    {
                        files = Directory.GetFiles(folderpath, filter, SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = Directory.GetFiles(folderpath, filter, SearchOption.TopDirectoryOnly);
                    }
                    ResultFileStringArray.Set(context, files);




                }





                
            }
            catch(Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_FileList", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }

        }
    }
}
