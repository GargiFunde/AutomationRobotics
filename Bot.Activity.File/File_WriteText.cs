// <copyright file=File_WriteText company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_WriteText company=E2E BOTS>
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
using System.Runtime.InteropServices;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FileWriteText.png")]
    [Designer(typeof(File_WriteText_ActivityDesigner))]
    public class File_WriteText : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Text")]
        [Description("Set Text")]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is New File")]
        public bool NewFile { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Override Text")]
        public bool IsOverride { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Append Text")]
        public bool IsAppend { get; set; }
        public File_WriteText()
        {
            IsOverride = false;
            IsAppend = true;
            NewFile = false;
        }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                string text = Text.Get(context);

                if (true== NewFile)
                {
                    writeText(filepath, text);
                }
                else
                {
                    if (File.Exists(filepath))
                    {
                        writeText(filepath, text);
                    }
                    else
                    {
                        Log.Logger.LogData("The file " + filepath + " does not exist in activity File_WriteText", LogLevel.Error);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_WriteText", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
        public void writeText(string filepath,string text)
        {
            if (true == IsOverride)
            {
                File.WriteAllText(filepath, text);
            }
            if (true == IsAppend)
            {
                File.AppendAllText(filepath, text);
            }
        }
    }
}
