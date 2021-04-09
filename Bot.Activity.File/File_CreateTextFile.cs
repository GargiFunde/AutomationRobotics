// <copyright file=File_CreateTextFile company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_CreateTextFile company=E2E BOTS>
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
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Create Text File 16 px.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class  File_CreateTextFile : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        [RequiredArgument]
        public InArgument<string> ToFilePath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                File.CreateText(filepath).Close();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_CreateTextFile", LogLevel.Error);
                 if (!ContinueOnError){context.Abort();}
            }
}
    }
}
