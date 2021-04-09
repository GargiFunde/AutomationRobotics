using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Files
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
