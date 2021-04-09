using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(Regex_Replace_ActivityDesigner))]
    [ToolboxBitmap("Resources/RegexReplace.png")]
    public class Regex_Replace : BaseNativeActivity
    {
        public Regex_Replace() : base()
        {
            this.RegexOption = RegexOptions.IgnoreCase | RegexOptions.Compiled;
        }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Pattern")]
        [Description("Enter Pattern")]
        public InArgument<string> Pattern { get; set; }

        [RequiredArgument]
        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<string> Result { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Input")]
        [Description("Enter Input")]
        public InArgument<string> Input { get; set; }

        [Category("Input")]
        [DisplayName("Regex Option")]
        [Description("Enter Regex Option")]
        public RegexOptions RegexOption { get; set; }

        [Category("Input")]
        [DisplayName("Replacement")]
        [Description("Enter Replacement")]
        public InArgument<string> Replacement { get; set; }

        [Category("Input")]
        [DisplayName("Match Evaluator")]
        [Description("Enter Match Evaluator")]
        public MatchEvaluator MatchEvaluator { get; set; }

        string res = string.Empty;
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (this.MatchEvaluator == null && (this.Replacement == null || this.Replacement.Expression == null))
            {
                metadata.AddValidationError("'Replacement' or 'MatchEvaluator' arguments in Replace not set.");
            }

            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                // we are using static method because by default the regular expression engine caches the 15 most recently used static regular expressions            

                if (this.Replacement.Get(context) != null)
                {
                    res = Regex.Replace(this.Input.Get(context), this.Pattern.Get(context), this.Replacement.Get(context), this.RegexOption);
                }
                else
                {
                    res = Regex.Replace(this.Input.Get(context), this.Pattern.Get(context), this.MatchEvaluator, this.RegexOption);
                }


                Result.Set(context, res);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Regex_Replace", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
