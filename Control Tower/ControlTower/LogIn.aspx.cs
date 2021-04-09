///-----------------------------------------------------------------
///   Namespace:      -
///   Class:                DemoMasterPage2_AddGroup
///   Description:       To capture all Active Logs.
///   Author:         B.Piyush                    Date: 2020-04-30 13:04:46.267
///   Notes:          <Notes>
///   Revision History:
///   Team:   E2E Team
///   Name:           Date:        Description:
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Novell.Directory.Ldap;
using CredentialManagement;
using System.IO;
using CommonLibrary;

public partial class DemoMasterPage2_LogIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Session["DomainName"] = null;
        Session["UserName"] = null;
        Session["Role"] = null;
        Session["GroupId"] = null;
        Session["TenantId"] = null;
        Application["visits"] = null;
    }

    #region Hashing
    public string ComputeHash(string input, HashAlgorithm algorithm)
    {
        Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

        return BitConverter.ToString(hashedBytes);
    }
    #endregion

    #region Encoding Password
    static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    #endregion


    protected override void InitializeCulture()
    {
        UICulture = Request.UserLanguages[0];
        base.InitializeCulture();
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
       // File.AppendAllText(@"c:\E2EDebug\file.txt", "Login Button Clicked" + "\n");
        // string strDomainName = txtDomainName.Text.Trim();
        string strDomainName = string.Empty;
        string strUserName = string.Empty;
        string strPwd = string.Empty;
        string strTenantName = string.Empty;
        string strGroupName = string.Empty;
        DataTable result = null;
        int TenantId = 0;  /*Need to be changed.*/
        string Role = string.Empty;

        //Remember me Functionality
        string StrPwdOriginal = string.Empty;
        string StrUserNameFull = string.Empty;
        string Selected = string.Empty;
        Selected = Request.Form["customCheck"];
        strUserName = txtUserName.Text.Trim();
        StrUserNameFull = strUserName;
        //Remember me End

        strUserName = txtUserName.Text.Trim();
        string[] struserDomain = null;
        if (strUserName.Contains("/"))
        {
            struserDomain = strUserName.Split('/');
        }
        else if (strUserName.Contains("\\"))
        {
            struserDomain = strUserName.Split('\\');
        }

        strDomainName = struserDomain[0];
        strUserName = struserDomain[1];
        strPwd = txtPassWord.Text;
        StrPwdOriginal = strPwd;
        strTenantName = txtTenantName.Text;
        strGroupName = txtGroupName.Text;
        Session["GroupName"] = strGroupName;
        Console.WriteLine(ComputeSha256Hash("ajit"));

        //strPwd = Encoding.UTF8.GetString(Convert.FromBase64String(strPwd));\


        /*Added By Piyush For Testing Purpose to decrypt Password fromm Database*/
        /*Code for Decypting Password*/
        //string decryptPass = String.Empty;
        //UTF8Encoding encode = new UTF8Encoding();
        //Decoder decode = encode.GetDecoder();
        //byte[] todecode = Convert.FromBase64String(strPwd);
        //int charcountvariable = decode.GetCharCount(todecode, 0, todecode.Length);
        //char[] decode_array = new char[charcountvariable];
        //decode.GetChars(todecode, 0, todecode.Length, decode_array, 0);
        //decryptPass = new String(decode_array);

        //bool LDAPResult = LDAPVerification(strUserName,strPwd);

        //if (true == LDAPResult)
        {
            string encodePwd = string.Empty;

            #region oldCodeEncryption

            //byte[] encode = new byte[strPwd.Length];
            //encode = Encoding.UTF8.GetBytes(strPwd);
            //encodePwd = Convert.ToBase64String(encode);  
            #endregion


            ServiceInterface serviceInterface = new ServiceInterface();
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string encrKey = serviceInterface.getKey();
            encodePwd = encryptionHelper.Encrypt(strPwd,encrKey);
            strPwd = encodePwd;

            if ((strUserName == "") || (strPwd == "") || (strDomainName == "") || (strTenantName == "") || (strGroupName == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Enter details');",true);
            }
            else
            {
                File.AppendAllText(@"c:\E2EDebug\file.txt", "Creating Service interface Object" + "\n");
                //bool result = serviceInterface.LoginUser(strDomainName, strUserName, strPwd, iTenantId);
                result = serviceInterface.LoginUser(strDomainName, strUserName, strPwd, strTenantName, strGroupName);
               
                int groupId = 0;
                int tenantId = 0;

                //tenantId = serviceInterface.GetTenantId(strTenantName, 0, 0);//need to change gettenantid service parameter.
                //groupId = serviceInterface.GetGroupId(strGroupName, tenantId);

                try
                {
                    if (result != null)
                    {
                        int userIsActive = 0;
                        int tenantIsActive = 0;
                       
                        if ((result.Rows[0][12] != null) && (result.Rows[0][12] != System.DBNull.Value))
                            userIsActive = int.Parse(result.Rows[0][12].ToString());
                        if ((result.Rows[0][13] != null) && (result.Rows[0][13] != System.DBNull.Value))
                            tenantIsActive = int.Parse(result.Rows[0][13].ToString());
                        if (userIsActive == -1 && tenantIsActive == -1)
                        {
                            serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName,"Tenant and User are not Active",groupId,tenantId);
                            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Alert', '\"" + strTenantName + "\" tenant and \"" + strUserName + "\" user are not active', 'warning').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                        }
                        if (userIsActive == -1 && tenantIsActive == 0)
                        {
                            serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName, "Tenant is Active but User is not Active", groupId, tenantId);
                            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Alert', '\"" + strUserName + "\" user is not active', 'warning').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                        }
                        if (userIsActive == 0 && tenantIsActive == -1)
                        {
                            serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName, "Tenant is not Active but User is Active", groupId, tenantId);
                            this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Alert', '\"" + strTenantName + "\" tenant is not active', 'warning').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                        }

                        if (userIsActive == 0 && tenantIsActive == 0)
                        {
                            TenantId = int.Parse(result.Rows[0]["tenantid"].ToString());
                            Role = result.Rows[0]["roletype"].ToString();
                            int groupid = int.Parse(result.Rows[0]["groupid"].ToString());

                            int roleid = serviceInterface.getRoleId(TenantId,Role, groupid);


                            Session["DomainName"] = strDomainName;
                            Session["UserName"] = strUserName;
                            Session["Role"] = Role;
                            Session["GroupId"] = groupid;
                            Session["TenantId"] = TenantId;

                            Session["roleid"] = roleid;

                            //Remember Me Funtinality
                            if (Selected == "IsChecked")
                            {
                                SavePassword(StrUserNameFull, StrPwdOriginal);
                            }
                            else
                            {
                                DestroyCredentials();
                            }


                            serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName, "Successful Login", groupId, tenantId);
                            serviceInterface.insertLog("LogIn Successfull ","LogIn SuccessFull at "+DateTime.Now, groupId, tenantId);

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: 'Welcome " + strUserName + "!!',text:'You are redirected to DashBoard Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'BotDashBoard.aspx';});", true);
                            //this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Welcome " + strUserName + "!!', '', 'success').then((value) => {  window.location.href = 'BotDashboard.aspx';}); ", true);
                        }
                    }
                    else
                    {
                        serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName, "Login Failed", groupId, tenantId);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'Please Enter Valid Credentials " + strUserName + "!!', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                        //this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Please Enter Valid Credentials " + strUserName + "!!', '', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                    }
                }
                catch (Exception ex)
                {
                    serviceInterface.insertActivityLog(strDomainName, strUserName, strGroupName, "Login Failed", groupId, tenantId);
                    serviceInterface.insertLog("Logged Out","Forcefully Logged Out from LogIn Page becuase of : "+ex.Message,groupId,tenantId);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'Please Enter Valid Credentials " + strUserName + "!!', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                    // this.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "swal('Please Enter Valid Credentials !!', '', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                }
            }
        }
    }

    public Boolean LDAPVerification(string UserName, string PassWord)
    {
        string ldapHost = "localhost";
        int ldapPort = 10389;
        string loginDN = "uid=admin,ou=system";
        string password = "secret";
        string searchBase = "ou=users,o=Company";
        string searchFilter = "objectClass=inetOrgPerson";

        try
        {

            LdapConnection conn = new LdapConnection();
            Console.WriteLine("Connecting to " + ldapHost);
            conn.Connect(ldapHost, ldapPort);
            conn.Bind(loginDN, password);
            string[] requiredAttributes = { "cn", "sn", "uid" };
            //string[] requiredAttributes = { "sn" };
            LdapSearchResults lsc = conn.Search(searchBase,
                                LdapConnection.SCOPE_SUB,
                                searchFilter,
                                requiredAttributes,
                                false);
            while (lsc.hasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = lsc.next();

                }
                catch (LdapException e)
                {
                    Console.WriteLine("Error : " + e.LdapErrorMessage);
                    continue;
                }
                Console.WriteLine("\n" + nextEntry.DN);
                LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                while (ienum.MoveNext())
                {
                    LdapAttribute attribute = (LdapAttribute)ienum.Current;
                    string attributeName = attribute.Name;
                    string attributeVal = attribute.StringValue;
                    Console.WriteLine("\t" + attributeName + "\tvalue  = \t" + attributeVal);
                    if (string.Compare(attributeVal, UserName) == 0)
                    {

                        return true;
                        //Console.WriteLine("Piyush");
                    }
                }
            }
            conn.Disconnect();
            return false;


        }
        catch (LdapException e)
        {
            Console.WriteLine("Error :" + e.LdapErrorMessage);
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error :" + e.Message);
            return false;
        }
      //  Console.WriteLine("Press any key ");
      //  Console.ReadKey(true);
        //return false;
    }

    //save password to the windows vault store using Credential Manager  
    public void SavePassword(string UserName, string PassWord)
    {
        try
        {
            using (var cred = new Credential())
            {
                cred.Username = UserName;
                cred.Password = PassWord;



                // cred.Target = "UserPassword12";
                cred.Target = UserName;
                cred.Type = CredentialType.Generic;
                cred.PersistanceType = PersistanceType.LocalComputer;
                cred.Save();
            }
        }
        catch (Exception ex)
        {
        }
    }
    //retrieve password from the windows vault store using Credential Manager   
    public string[] GetPassword(string UserName)
    {
        try
        {
            using (var cred = new Credential())
            {
                cred.Target = UserName;
                // cred.Target = "UserPassword12";
                cred.Load();
                string[] Credentials = { cred.Username, cred.Password };



                //    Credentials[0] = cred.Username;
                //  Credentials[1] = cred.Password;




                return Credentials;
            }
        }
        catch (Exception ex)
        {
        }
        return null;
    }




    // Destroy Credentials if user does not want his credentials to be remembered
    public bool DestroyCredentials()
    {
        try
        {
            using (var cred = new Credential())
            {
                cred.Target = txtUserName.Text;
                cred.Load();
                // cred.Dispose();
                cred.Delete();
                return true;
            }
        }
        catch (Exception ex)
        {
        }
        return false;
    }

    protected void txtPassWord_TextChanged(object sender, EventArgs e)
    {
        string[] Credentials = null;
        Credentials = GetPassword(txtUserName.Text);
        if (Credentials != null && txtUserName.Text == Credentials[0])
        {
            //   txtUserName.Text = Credentials[0];
            txtPassWord.Text = Credentials[1];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Javascript", "document.getElementById('customCheck').checked = true;", true);
        }
        else
        {
            txtPassWord.Text = "";
        }
    }

}