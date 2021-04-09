using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Logger;
namespace Bot.Activity.ActivityLibrary
{
    [Serializable]
    public class BusinessException : Exception
    {

        public BusinessException()
        {
           
        }

        public BusinessException(string exceptionDetails)
            : base(String.Format("Business Exception: {0}", exceptionDetails))
        {
           // Logger.Log.Logger.LogData(exceptionDetails, LogLevel.Fatal);
        }

    }
}
