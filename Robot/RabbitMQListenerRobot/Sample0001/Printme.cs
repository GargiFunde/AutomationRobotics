using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;

namespace Sample0001
{
    public class Printme : BaseNativeActivity
    {
        public InArgument<string> input { get; set; }
        public OutArgument<string> output { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
           Logger.Log.Logger.LogData("invoke Test",LogLevel.Info);
        }
    }
}
