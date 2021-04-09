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
    [ToolboxBitmap("Resources/CredentialManagerSaveCredentials.png")]
    [Designer(typeof(CredentialManager_SaveCredentials_ActivityDesigner))]
    public class CredentialManager_SaveCredentials : BaseNativeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> UserName { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Password { get; set; }
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ApplicationName { get; set; }

        string persistType = "LocalComputer";
        [RequiredArgument]
        [TypeConverter(typeof(Collection2PropertyConverter1))]
        public string PersistType
        {  
            get
            {
                return persistType;
            }
            set
            {
                persistType = value;
            }
        }

        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string userName = UserName.Get(context);
                string password = Password.Get(context);
                string applicationName = ApplicationName.Get(context);
                bool result = false;
                PersistanceType pt = new PersistanceType();
                if(PersistType == "LocalComputer")
                {
                    pt = PersistanceType.LocalComputer;
                }
                else if(PersistType == "Enterprise")
                {
                    pt = PersistanceType.Enterprise;
                }
                else if (PersistType == "Session")
                {
                    pt = PersistanceType.Session;
                }

                result = new Credential
                {
                    Target = applicationName,
                    Username = userName,
                    Password = password,
                    PersistanceType = pt,
                    Type = CredentialType.Generic
            }.Save();

                Result.Set(context, result);
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity CellValue_Clear", LogLevel.Error);
                if (!ContinueOnError){context.Abort();}
            }
        }
    }
    public class Collection2PropertyConverter1 : StringConverter
    {

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, but allow free-form entry
            return true;
        }
       
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> persistType = new List<string>();
            persistType.Add("LocalComputer");
            persistType.Add("Enterprise");
            persistType.Add("Session");
          
            return new StandardValuesCollection(persistType);
        }

    }
}
