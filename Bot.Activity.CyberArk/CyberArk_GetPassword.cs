using CyberArk.AIM.NetPasswordSDK;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.CyberArk
{
    [ToolboxBitmap("Resources/CyberArkGetPassword.png")]
    [Designer(typeof(CyberArk_GetPassword_ActivityDesigner))]

    public class CyberArk_GetPassword : BaseNativeActivity
    {

        [Category("Input")]
        [DisplayName("Application Id")]
        [Description("Set Application Id")]
        public InArgument<string> ApplicationId { get; set; }
        [Category("Input")]
        [DisplayName("Safe")]
        [Description("Set Safe")]
        public InArgument<string> Safe { get; set; }
        [Category("Input")]
        [DisplayName("Object Name")]
        [Description("Set Object Name")]
        public InArgument<string> ObjectName { get; set; }

        [Category("OutPut")]
        [DisplayName("User Name")]
        [Description("Get User Name")]
        public OutArgument<string> UserName { get; set; }
        [Category("OutPut")]
        [DisplayName("Password")]
        [Description("Get secure Password")]
        public OutArgument<SecureString> Password { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string strAppId = ApplicationId.Get(context);
                string strSafe = Safe.Get(context);
                string strObjectName = ObjectName.Get(context);

                PSDKPasswordRequest passRequest = new PSDKPasswordRequest();
                PSDKPassword psdkpassword;

                passRequest.AppID = strAppId; // "RESTExamples";
                passRequest.Safe = strSafe;// "T-APP-CYBR-RESTAPI";
                passRequest.Object = strObjectName;//"Database-MicrosoftSQLServer-JG-sql01.joe-garcia.local-Svc_BambooHR";

                // Set required properties to be returned other than password
                passRequest.RequiredProperties.Add("PolicyId");
                passRequest.RequiredProperties.Add("UserName");
                passRequest.RequiredProperties.Add("Address");

                // Sending the request to get the password
                psdkpassword = PasswordSDK.GetPassword(passRequest);

                string username = psdkpassword.UserName;
                string textPwd = psdkpassword.Content;
                if (!string.IsNullOrEmpty(username))
                {
                    UserName.Set(context, username);
                }
                else
                {
                    Logger.Log.Logger.LogData("User name is null or empty in activity FetchSecurePassword", Logger.LogLevel.Info);
                }
                if (!string.IsNullOrEmpty(textPwd))
                {
                    SecureString strpwd = ToSecureString(textPwd);
                    if (strpwd != null)
                    {
                        Password.Set(context, strpwd);
                    }
                }
                else
                {
                    Logger.Log.Logger.LogData("Password is null or empty in activity FetchSecurePassword", Logger.LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity FetchSecurePassword", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        private SecureString ToSecureString(string plainString)
        {
            if (plainString == null)
                return null;

            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }
    }
}
