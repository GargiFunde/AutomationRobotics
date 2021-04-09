using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(Regex_IsMatch_ActivityDesigner))]
    [ToolboxBitmap("Resources/RegexIsMatch.png")]
    public class Regex_IsMatch : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Input")]
        [Description("Input")]
        public InArgument<string> Input { get; set; }

        [Category("Input")]
        [DisplayName("Regex Option")]
        [Description("Enter Regex Option")]
        public RegexOptions RegexOption { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Pattern")]
        [Description("Enter RegEx Pattern")]
        public InArgument<string> Pattern { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("Enter Result")]
        public OutArgument<Boolean> Result { get; set; }

        public Regex_IsMatch() : base()
        {
            this.RegexOption = RegexOptions.IgnoreCase | RegexOptions.Compiled;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                // we are using static method because by default the regular expression engine caches the 15 most recently used static regular expressions            
                bool result = Regex.IsMatch(this.Input.Get(context), this.Pattern.Get(context), this.RegexOption);
                Result.Set(context, result);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Regex_IsMatch", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
