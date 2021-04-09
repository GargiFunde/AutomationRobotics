using System.Activities;
using System.ComponentModel;

namespace Bot.Activity.Image
{
    public abstract class BaseNativeActivity : NativeActivity
    {
        [Category("Common Parameters")]
        [DisplayName("Continue On Error")]
        public bool ContinueOnError { get; set; }

        public BaseNativeActivity()
        {
            ContinueOnError = false;
        }

    }
}
