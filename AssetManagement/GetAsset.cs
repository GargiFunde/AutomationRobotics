using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using System.Data;
using System.Drawing;

namespace AssetManagement
{
    [ToolboxBitmap("Resources/Lock.png")]
    [Designer(typeof(ActivityDesignerForGetAsset))]
    public class GetAsset : BaseNativeActivity
    {
        [Category("Input Parameters")]
        [DisplayName("Asset Name")]
        [Description("Set Asset Name")]
        [RequiredArgument]
        public InArgument<string> AssetName { get; set; }
        
        [Category("Output Parameters")]
        [DisplayName("Asset Value")]
        [Description("Get Secure AssetValue")]
        public OutArgument<string> AssetValue { get; set; }

        [Category("Input Parameters")]
        public bool Private { get; set; }

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


        public GetAsset()
        {
            //Log.Logger.LogData("Getting Asset", LogLevel.Info);
            //bool Private = false;
        }
        protected override void Execute(NativeActivityContext context)
        {
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string assetName = AssetName.Get(context);
            //    string key = Key.Get(context);
            //  bool Private = Private1.Get(context);

            string tenantName = TenantName.Get(context);
            string groupName = GroupName.Get(context);
            string loginPassword = LoginPassword.Get(context);
            string domainNameUserName = DomainNameUserName.Get(context);

            string[] struserDomain = null;
            if (domainNameUserName.Contains("/"))
            {
                struserDomain = domainNameUserName.Split('/');
            }
            else if (domainNameUserName.Contains("\\"))
            {
                struserDomain = domainNameUserName.Split('\\');
            }
            string strDomainName = struserDomain[0];
            string strUserName = struserDomain[1];


            string encodePwd = string.Empty;
            byte[] encode = new byte[loginPassword.Length];
            encode = Encoding.UTF8.GetBytes(loginPassword);
            encodePwd = Convert.ToBase64String(encode);
            loginPassword = encodePwd;
            DataTable result = null;

            //consoleExecutionLog.AppendText("Calling Service");
            Logger.ServiceReference1.BOTServiceClient ser = new Logger.ServiceReference1.BOTServiceClient();
            result = ser.LoginUser(strDomainName, strUserName, loginPassword, tenantName, groupName);
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

                        DataTable resultGetCredentials = ser.GetAssetsActivity(assetName, groupid, TenantId);
                        if (resultGetCredentials != null && resultGetCredentials.Rows.Count > 0)
                        {

                            string assetval = resultGetCredentials.Rows[0]["value"].ToString();
                            //Private = false;
                            //if (Private == false)    Commenting Private 
                            //{ }
                            // password = Decrypt(password, key, keySize);

                            assetval = encryptionHelper.Decrypt(assetval, resultGetKey);


                            AssetValue.Set(context, assetval);

                        }
                        else
                        {
                            Log.Logger.LogData("Asset does not validate(exist) in activity Get Asset", LogLevel.Info);

                        }

                    }
                    else
                    {
                        Log.Logger.LogData("Enter Key does not validate(exist) in activity Get Asset", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message + " in activity Get Asset", LogLevel.Error);
                }


               
            }
            else
            {
                Log.Logger.LogData("User does not validate(exist) in activity Get Credentials", LogLevel.Info);
            }
        }
    }
}
