
using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CredentialManager
{
    [ToolboxBitmap("Resources/Lock.png")]
    [Designer(typeof(ActivityDesignerForGetCredential))]
    public class GetCredentials : BaseNativeActivity
    {
        //[Category("Input Paramaters")]
        //[DisplayName("Key")]
        //[Description("Set Key")]
        //[RequiredArgument]
        //public InArgument<string> Key { get; set; }

        [Category("Input Parameters")]
        [DisplayName("Asset Name")]
        [Description("Set Asset Name")]
        [RequiredArgument]
        public InArgument<string> AssetName { get; set; }

        [Category("Output Parameters")]
        [DisplayName("User Name")]
        [Description("Get User Name")]
        public OutArgument<string> UserName { get; set; }


        [Category("Output Parameters")]
        [DisplayName("Password")]
        [Description("Get Secure Password")]        
        public OutArgument<string> Password { get; set; }

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


        public GetCredentials()
        {
            //Log.Logger.LogData("Constructor", LogLevel.Info);
            //bool Private = false;
        }

        private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        protected override void Execute(NativeActivityContext context)
        {
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

                        DataTable resultGetCredentials = ser.GetCredentialsActivity(assetName, TenantId, groupid);
                        if (resultGetCredentials != null && resultGetCredentials.Rows.Count > 0)
                        {
                            string username = resultGetCredentials.Rows[0]["username"].ToString();
                            string password = resultGetCredentials.Rows[0]["pwd"].ToString();
                            //Private = false;
                            //if (Private == false)  commenting Private 
                            //{ }
                            // password = Decrypt(password, key, keySize);

                            password = Decrypt(password, resultGetKey);

                            UserName.Set(context, username);
                            Password.Set(context, password);
                        }
                        else
                        {
                            Log.Logger.LogData("Asset does not validate(exist) in activity Get Credentials", LogLevel.Info);

                        }

                    }
                    else
                    {
                        Log.Logger.LogData("Enter Key does not validate(exist) in activity Get Credentials", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message + " in activity Get Credentials", LogLevel.Error);
                }

               
            }
            else
            {
                Log.Logger.LogData("User does not validate(exist) in activity Get Credentials", LogLevel.Info);
            }

        }

        private string Decrypt(String password,string passkey)
        {
            //Decrypt
            byte[] bytes = Convert.FromBase64String(password);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(passkey));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    string decryptedpassword = Encoding.Unicode.GetString(decryptedBytes);
                    return decryptedpassword;
                }
            }
        }

        //private static string GenerateKey(int iKeySize)
        //{
        //    RijndaelManaged aesEncryption = new RijndaelManaged();
        //    aesEncryption.KeySize = iKeySize;
        //    aesEncryption.BlockSize = 128;
        //    aesEncryption.Mode = CipherMode.CBC;
        //    aesEncryption.Padding = PaddingMode.PKCS7;
        //    aesEncryption.GenerateIV();
        //    string ivStr = Convert.ToBase64String(aesEncryption.IV);
        //    aesEncryption.GenerateKey();
        //    string keyStr = Convert.ToBase64String(aesEncryption.Key);
        //    string completeKey = ivStr + "," + keyStr;

        //    return Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(completeKey));
        //}

        //private static string Decrypt(string iEncryptedText, string iCompleteEncodedKey, int iKeySize)
        //{
        //    RijndaelManaged aesEncryption = new RijndaelManaged();
        //    aesEncryption.KeySize = iKeySize;
        //    aesEncryption.BlockSize = 128;
        //    aesEncryption.Mode = CipherMode.CBC;
        //    aesEncryption.Padding = PaddingMode.PKCS7;
        //    aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[0]);
        //    aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[1]);
        //    ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
        //    byte[] encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
        //    return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        //}
    }
}
