using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Created a seperate project so that it can be refered in bot robot and listener
namespace Validator
{

    public class RequestValidator
    {

        public static int ValidateRequest(RequestInput _requestInput)
        {
            if (string.IsNullOrEmpty(_requestInput.AutomationGroupName ))
            {
                //"Group Name should not be empty"
                return -1;
            }
            if (string.IsNullOrEmpty(_requestInput.AutomationProcessName))
            {
                //"Process Name should not be empty"
                return -1;
            }
            if ((_requestInput.InputSearchParameters == null) || (_requestInput.InputSearchParameters.Count < 1))
            {
                //"Search parameters should not be empty"
                return -1;
            }
            if (string.IsNullOrEmpty(_requestInput.TenantName))
            {
                //"Tenant Name should not be empty"
                return -1;
            }
            return 0; //for success
        }
    }
}
