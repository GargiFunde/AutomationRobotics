using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.MicrosoftOCR
{
    public abstract class BaseNativeActivity : NativeActivity
    {
        public bool ContinueOnError { get; set; }

        public BaseNativeActivity()
        {
            ContinueOnError = false;
        }

    }
}
