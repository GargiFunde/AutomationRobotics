// <copyright file=Regex_Matches company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DefaultDesigner))]
    public class Regex_Matches : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> Input { get; set; }

        public RegexOptions RegexOption { get; set; }

        [RequiredArgument]
        public InArgument<string> Pattern { get; set; }

       
        public OutArgument<MatchCollection> Result { get; set; }
        public Regex_Matches() : base()
        {
            this.RegexOption = RegexOptions.IgnoreCase | RegexOptions.Compiled;
        }
        protected override void Execute(NativeActivityContext context)
        {
            // we are using static method because by default the regular expression engine caches the 15 most recently used static regular expressions            
            try
            {
                MatchCollection matchCollection = Regex.Matches(this.Input.Get(context), this.Pattern.Get(context), this.RegexOption);
                Result.Set(context, matchCollection);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Regex_Matches", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
