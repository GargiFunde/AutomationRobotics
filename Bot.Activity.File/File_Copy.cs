// <copyright file=File_Copy company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_Copy company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FileCopy.png")]
    [Designer(typeof(File_Copy_ActivityDesigner))]
    public class File_Copy : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Source File Path")]
        [Description("Set Source File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination File Path")]
        [Description("Set Destination File Path")]
        [RequiredArgument]
        public InArgument<string> ToFilePath { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Override")]
        public bool IsOverride { get; set; }

        public File_Copy()
        {
            IsOverride = false;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string fromfilepath = FilePath.Get(context);
                string tofilepath = ToFilePath.Get(context);

                if (File.Exists(tofilepath))
                {
                    if (true== IsOverride)
                    {
                        File.Delete(tofilepath);
                        File.Copy(fromfilepath, tofilepath);
                    }
                    else
                    {
                        Log.Logger.LogData("The file "+ tofilepath + " is already exists in activity File_Copy", LogLevel.Error);
                    }                   
                }
                else
                {
                    File.Copy(fromfilepath, tofilepath);
                }
              
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Copy", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
        }
    }
}
