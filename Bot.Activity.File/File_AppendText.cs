// <copyright file=File_AppendText company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:24</date>
// <summary></summary>

// <copyright file=File_AppendText company=E2E BOTS>
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
using Bot.Activity.Files;
using Logger;
using System.Drawing;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/Append text 16 px.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class File_AppendText : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        [RequiredArgument]
        public InArgument<string> Text { get; set; }
              
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
            string filepath = FilePath.Get(context);
            string text = Text.Get(context);

            System.IO.File.AppendAllText(filepath, text);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_AppendText", LogLevel.Error);
                 if (!ContinueOnError){
                    context.Abort();
                }
            }
        }
    }
}
