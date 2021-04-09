using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Text;
using CommonLibrary;
using System.Drawing;

namespace AssetManagement
{
    [ToolboxBitmap("Resources/Lock.png")]
    [Designer(typeof(ActivityDesignerForAddAsset))]
    public class AddAsset : BaseNativeActivity
    {
        [Category("Input Parameters")]
        [DisplayName("Asset Name")]
        [Description("Set Asset Name")]
        [RequiredArgument]
        public InArgument<string> AssetName { get; set; }

       


        [Category("Input Parameters")]
        [DisplayName("Asset Value")]
        [Description("Get Secure Value")]
        [RequiredArgument]
        public InArgument<string> AssetValue { get; set; }

      

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
        [Description("Get Result")]        
        public OutArgument<bool> Result { get; set; }
        public AddAsset()
        {
            //Log.Logger.LogData("Constructor", LogLevel.Info);
         
        }
        protected override void Execute(NativeActivityContext context)
        {
            EncryptionHelper encryptionHelper = new EncryptionHelper();

            string assetName = AssetName.Get(context);
            string assetValue = AssetValue.Get(context);
           

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
            
            if (result !=null && result.Rows.Count > 0)
            {
                try
                {
                    int groupid = int.Parse(result.Rows[0]["groupid"].ToString());
                    int TenantId = int.Parse(result.Rows[0]["tenantid"].ToString());

                    string resultGetKey = ser.getKey();



                    if (resultGetKey != null)
                    {
                        string EncryptedAssetValue = encryptionHelper.Encrypt(assetValue, resultGetKey);
                        int resultGetAddAssetStatus = ser.AddAssets(assetName, EncryptedAssetValue, TenantId, groupid, strUserName);
                        if (resultGetAddAssetStatus == 1)
                        {

                            Result.Set(context, true);
                            Log.Logger.LogData("Asset added to Asset Management successfully by activity Add Asset", LogLevel.Info);

                        }
                        else if (resultGetAddAssetStatus == -1)
                        {
                            Result.Set(context, false);
                            Log.Logger.LogData("Asset already exists in Asset Management in activity Add Asset", LogLevel.Info);

                        }
                        else
                        {
                            Result.Set(context, false);
                            Log.Logger.LogData("Asset does not validate(exist) in activity Add Asset", LogLevel.Info);

                        }

                    }
                    else
                    {
                        Log.Logger.LogData("Enter Key does not validate(exist) in activity Add Asset", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message + " in activity Add Asset", LogLevel.Error);
                }
                
            }
            else
            {

                Log.Logger.LogData("User does not validate(exist) in activity Get Credentials", LogLevel.Info);

            }
        }
    }
}
