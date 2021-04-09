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
    public class UserInput_SingleValue : BaseNativeActivity
    {

        public OutArgument<string> UserInput { get; set; }
        public OutArgument<bool> Result { get; set; }

        bool dialogResult = false;
        UserInputEntity userInputEntity = null;
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
               
                    userInputEntity = new UserInputEntity();
                    Bookmark bk = context.CreateBookmark("UserInputs", new BookmarkCallback(OnResumeBookmark));
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    SingleInput single = new SingleInput(ref userInputEntity);
                    single.ShowDialog();
                    if (single.DialogResult == true)
                    {
                        dialogResult = true;
                        SelectHelper._wfApplication.ResumeBookmark("UserInputs", "");
                    }
                });
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity UserInput_SingleValue", LogLevel.Error);
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
                    UserInput.Set(context, userInputEntity.userid);
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