// <copyright file=Folder_Move company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=Folder_Move company=E2E BOTS>
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
    [ToolboxBitmap("Resources/foldermove.png")]
    [Designer(typeof(Folder_Move_ActivityDesigner))]
    public class Folder_Move : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Source Folder Path")]
        [Description("Set Source Folder Path")]
        public InArgument<string> FromFolderPath { get; set; }
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Destination Folder Path")]
        [Description("Set Destination Folder Path")]
        public InArgument<string> ToFolderPath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string fromfolderpath = FromFolderPath.Get(context);
                if (Directory.Exists(fromfolderpath))
                {
                    string tofolderpath = ToFolderPath.Get(context);
                    string destinationfolder = Path.GetFileName(fromfolderpath);
                    tofolderpath = tofolderpath + "\\" + destinationfolder;
                    Directory.Move(fromfolderpath, tofolderpath);

                }
                else if(!Directory.Exists(fromfolderpath)) 
                {
                    
                    Log.Logger.LogData("Folder does not exist in path - " + fromfolderpath + " in activity Folder_Move", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }




            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Move", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
        }
    }
}
