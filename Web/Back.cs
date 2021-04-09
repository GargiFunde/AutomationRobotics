// <copyright file=ResetWeb company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:57</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using Logger;

namespace Bot.Activity.Web
{
    [Designer(typeof(WebControlProperties1))]
    public class IE_Back : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        public InArgument<int> TimeOutInSecond { get; set; }

        public OutArgument<bool> BackResult { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {

            string AppId = ApplicationIDToAttach.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.Back();
                        IEWATIN.WaitForComplete(5000);

                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        BackResult.Set(context, true);
                    }
                }

            }
            catch (Exception ex)
            {
                BackResult.Set(context, false);
                Logger.Log.Logger.LogData(ex.Message + " in activity Back", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }


    }
}
