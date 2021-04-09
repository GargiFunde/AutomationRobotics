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
    [ToolboxBitmap("Resources/CredentialManagerGetCredential.png")]
    [Designer(typeof(CredentialManager_GetCredential_ActivityDesigner))]
    public class CredentialManager_GetCredential : BaseNativeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ApplicationName { get; set; }
        [Category("Output")]
        public OutArgument<string> UserName { get; set; }
        [Category("Output")]
        public OutArgument<string> Password { get; set; }

        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                bool result = false;
                string applicationName = ApplicationName.Get(context);

                //  var cm = new Credential { Target = applicationName, Type=CredentialType.Generic, PersistanceType = PersistanceType.LocalComputer};
                var cm = new Credential { Target = applicationName };
                if (cm.Exists())
                {
                    if (!cm.Load())
                    {
                        Result.Set(context, false);
                        return;
                    }
                }
                else
                {
                    Result.Set(context, false);
                    return;
                }
                  
                //UserPass is just a class with two string properties for user and pass
                UserName.Set(context, cm.Username);
                Password.Set(context, cm.Password);
                Result.Set(context, true);

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
