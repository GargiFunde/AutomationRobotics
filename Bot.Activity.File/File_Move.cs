// <copyright file=File_Move company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_Move company=E2E BOTS>
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
    [ToolboxBitmap("Resources/FileMove.png")]
    [Designer(typeof(File_Move_ActivityDesigner))]
    public class File_Move : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Source File Path")]
        [Description("Set Source File Path")]
        [RequiredArgument]
        public InArgument<string> FromFilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination File Path")]
        [Description("Set Destination File Path")]
        [RequiredArgument]
        public InArgument<string> ToFilePath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string fromfilepath = FromFilePath.Get(context);
                string tofilepath = ToFilePath.Get(context);
                File.Move(fromfilepath, tofilepath);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Move", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
