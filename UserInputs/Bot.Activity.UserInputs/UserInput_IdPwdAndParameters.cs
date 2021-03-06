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
using CommonLibrary;

namespace Bot.Activity.UserInputs
{

    [Designer(typeof(ActivityDesigner1))]
    public class UserInput_IdPwdAndParameters : BaseNativeActivity
    {
        [RequiredArgument]
        public OutArgument<string> UserId { get; set; }
        [RequiredArgument]
        public OutArgument<string> Password { get; set; }
        [RequiredArgument]
        public OutArgument<string> Parameter { get; set; }
        public OutArgument<bool> Result { get; set; }
        bool dialogResult = false;
        UserInputEntity userInputEntity = null;
        protected override void Execute(NativeActivityContext context)
        {
            try
            {

                userInputEntity = new UserInputEntity();
                Bookmark bk = context.CreateBookmark("UserIdPasswordParameter", new BookmarkCallback(OnResumeBookmark));
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    UserIdPasswordParameter useridpwd = new UserIdPasswordParameter(ref userInputEntity);
                    useridpwd.ShowDialog();
                    if (useridpwd.DialogResult == true)
                    {
                        dialogResult = true;
                        SelectHelper._wfApplication.ResumeBookmark("UserIdPasswordParameter", "");
                    }
                });
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity UserIdPasswordParameter", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        public void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            if (dialogResult)
            {
                if (!string.IsNullOrEmpty(userInputEntity.userid) && (userInputEntity.userid.Trim().Length > 0))
                {
                    UserId.Set(context, userInputEntity.userid);
                }
                if (!string.IsNullOrEmpty(userInputEntity.password) && (userInputEntity.password.Trim().Length > 0))
                {
                    Password.Set(context, userInputEntity.password);
                }
                if (!string.IsNullOrEmpty(userInputEntity.param1) && (userInputEntity.param1.Trim().Length > 0))
                {
                    Parameter.Set(context, userInputEntity.param1);
                }
                Result.Set(context, true);
            }
            else
            {
                Result.Set(context, false);
            }
            // When the Bookmark is resumed, assign its value to  
            // the Result argument.  
            // Result.Set(context, (UserInputEntity)obj);

            // Result.Set(context, false);
        }

    }
}