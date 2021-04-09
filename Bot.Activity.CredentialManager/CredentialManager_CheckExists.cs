// <copyright file=CellValue_Clear company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using Logger;
using CredentialManagement;
using System.Drawing;

namespace Bot.Activity.CredentialManager
{
    [ToolboxBitmap("Resources/CredentialManagerCheckExists.png")]
    [Designer(typeof(CredentialManager_CheckExists_ActivityDesigner))]
    public class CredentialManager_CheckExists : BaseNativeActivity
    {

        [RequiredArgument]
        public InArgument<string> ApplicationName { get; set; }

        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                bool result = false;
                string applicationName = ApplicationName.Get(context);
                //  var cm = new Credential { Target = applicationName, Type=CredentialType.Generic, PersistanceType = PersistanceType.LocalComputer};
                var cm = new Credential { Target = applicationName };  
                Result.Set(context, cm.Exists());
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity CellValue_Clear", LogLevel.Error);
                if (!ContinueOnError){context.Abort();}
            }
        }
    }
}   