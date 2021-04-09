using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Read
{
    public abstract class BaseNativeActivity : NativeActivity
    {
        public bool ContinueOnError { get; set; }

        public BaseNativeActivity()
        {
            ContinueOnError = true;
        }

    }
}
