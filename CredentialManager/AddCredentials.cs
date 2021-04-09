using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using CommonLibrary;
using System.Drawing;

namespace CredentialManager
{
    [ToolboxBitmap("Resources/Lock.png")]
    [Designer(typeof(ActivityDesignerForAddCredential))]
   public class AddCredentials : BaseNativeActivity
    {
        

        [Category("Input Parameters")]
        [DisplayName("Credential Name")]
        [Description("Set Credential Name")]
        [RequiredArgument]
        public InArgument<string> AssetName { get; set; }

        [Category("Input Parameters")]
        [DisplayName("User Name")]
        [Description("Get User Name")]
        [RequiredArgument]
        public InArgument<string> UserName { get; set; }


        [Category("Input Parameters")]
        [DisplayName("Password")]
        [Description("Get Secure Password")]
        [RequiredArgument]
        public InArgument<string> PassWord { get; set; }



        [Category("User Login Parameters")]
        [DisplayName("Tenant Name")]
        [Description("Set Tenant Name")]
        [RequiredArgument]
        public InArgument<string> TenantName { get; set; }

        [Category("User Login Parameters")]
        [DisplayName("Group Name")]
        [Description("Set Group Name")]
        [RequiredArgument]
        public InArgument<string> GroupName { get; set; }

        [Category("User Login Parameters")]
        [DisplayName("Password")]
        [Description("Set Login Password")]
        [RequiredArgument]
        public InArgument<string> LoginPassword { get; set; }


        [Category("User Login Parameters")]
        [DisplayName("Domain Name/User Name")]
        [Description("Set Domain Name/User Name")]
        [RequiredArgument]
        public InArgument<string> DomainNameUserName { get; set; }

        [Category("Output Parameters")]
        [DisplayName("Result")]
        [Description("Result")]
        public OutArgument<bool> Result { get; set; }
        public AddCredentials()
        {

        }
        protected override void Execute(NativeActivityContext context)
        {

           
            //default declaration of the variables
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string VarUsername = null;
            string VarPassword = null;
            string VarCredential = null;
            string VarResult = null;
            string VarTenant = null;
            string VarDomainLogin = null;
            string VarPwdLogin = null;
            string VarGroup = null;


            //setting the context to the variables
            VarCredential = AssetName.Get(context);
            VarUsername = UserName.Get(context);
            //VarPassword = activityDesignerForAddCredential.pwdtxt.Password;
            VarPassword = PassWord.Get(context);
            VarDomainLogin = DomainNameUserName.Get(context);
            VarPwdLogin = LoginPassword.Get(context);
            VarTenant = TenantName.Get(context);
            VarGroup = GroupName.Get(context);


            //Logic
            string[] struserDomain = null;
            if (VarDomainLogin.Contains("/"))
            {
                struserDomain = VarDomainLogin.Split('/');
            }
            else if (VarDomainLogin.Contains("\\"))
            {
                struserDomain = VarDomainLogin.Split('\\');
            }
            string strDomainName = struserDomain[0];
            string strUserName = struserDomain[1];


            string encodePwd = string.Empty;
            byte[] encode = new byte[VarPwdLogin.Length];
            encode = Encoding.UTF8.GetBytes(VarPwdLogin);
            encodePwd = Convert.ToBase64String(encode);
            string VarEncryptedPwdLogin = encodePwd;
            DataTable result = null;

            //consoleExecutionLog.AppendText("Calling Service");
            Logger.ServiceReference1.BOTServiceClient ser = new Logger.ServiceReference1.BOTServiceClient();
            result = ser.LoginUser(strDomainName, strUserName, VarEncryptedPwdLogin, VarTenant, VarGroup);
            //consoleExecutionLog.AppendText("Got Data");
           

            if (result != null && result.Rows.Count > 0)
            {
                //int keySize = 256;
                try
                {
                    int groupid = int.Parse(result.Rows[0]["groupid"].ToString());
                    int TenantId = int.Parse(result.Rows[0]["tenantid"].ToString());
                    string resultGetKey = ser.getKey();

                    //int resultKeyExist = ser.KeyExist(key);

                    if (resultGetKey != null)
                    {
                        string EncryptedCredPassword = encryptionHelper.Encrypt(VarPassword, resultGetKey);
                        int resultGetCredentials = ser.AddCredentials(VarCredential, VarUsername, EncryptedCredPassword, TenantId, groupid, strUserName);
                        if (resultGetCredentials == 1)
                        {

                            Result.Set(context, true);
                            Log.Logger.LogData("Credential has been added successfully to Credential Management", LogLevel.Info);

                        }
                        else if (resultGetCredentials == -1)
                        {
                            Result.Set(context, false);
                            Log.Logger.LogData("Credential already exists in Credential Management", LogLevel.Info);

                        }
                        else
                        {
                            Result.Set(context, false);
                            Log.Logger.LogData("Credential could not be added into Credential Management", LogLevel.Info);

                        }

                    }
                    else
                    {
                        Result.Set(context, false);
                        Log.Logger.LogData("Enter Key does not validate(exist) in activity Add Credentials", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    Result.Set(context, false);
                    Log.Logger.LogData(ex.Message + " in activity Add Credentials", LogLevel.Error);
                }


               
            }
            else
            {
                Log.Logger.LogData("User does not validate(exist) in activity Add Credentials", LogLevel.Info);
            }







        }
    }
}
