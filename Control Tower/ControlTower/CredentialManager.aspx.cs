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
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ControlTower_AssetManagement1 : System.Web.UI.Page
{
    public ServiceInterface serviceInterface = new ServiceInterface();

    protected int groupId = 0;
    protected int tenantId = 0;
    protected string userName = string.Empty;
    public DataTable result = new DataTable();
    private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
    private int BlockSize = 128;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AddUsers Page", "Exception: Session Expired from AddUsers Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];
                getCredentials(groupid, tenantid);
            }
        }
        else if (IsPostBack)
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AddUsers Page", "Exception: Session Expired from AddUsers Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AddUsers Page", "Exception: Session Expired from AddUsers Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
    }

    protected override void InitializeCulture()
    {
        UICulture = Request.UserLanguages[0];
        base.InitializeCulture();
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (Session["theme"] == null)
        {
            Theme = "default";  //for the first time Session["theme"] does not contain any value.
        }
        else
        {
            Theme = Session["theme"].ToString(); //with in the dropdown we set name of theme in session variable, this is set when user selects his choice of theme from dropdownlist.
        }

    }

    protected void btnSave_Asset(object sender, EventArgs e)
    {
        try
        {
            if (txtCredentialName.Text != "" && txtUser.Text != "" && txtPassWord.Text != "")
            {
                string strCredentialName = txtCredentialName.Text;
                string strUserName = txtUser.Text;
                //string strPwd = txtPassWord.Text;
                //string Encrypted_Pwd = encryptpass(strPwd);

                string key = serviceInterface.getKey();
                string password = txtPassWord.Text;
                int keysize = 256;
               
                string encyptedPassword = Encryption(key);
                //MessageBox.Show("encyptedPassword "+ encyptedPassword);
                string strcreatedBy = (string)Session["UserName"];
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];

                int result = serviceInterface.AddCredentials(strCredentialName, strUserName, encyptedPassword, tenantid, groupid, strcreatedBy);
                if (0 <  result)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitCREDENTIALDETAILSADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitCREDENTIAL") + " " + strUserName + " " + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                    getCredentials(groupid, tenantid);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOADDCREDENTIALDETAILS") + " ," + (String)GetGlobalResourceObject("content", "LitUSERNAMEMAYALREADYPRESENT") + "',text:'" + (String)GetGlobalResourceObject("content", "LitCREDENTIAL") + " " + strUserName + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + "', showConfirmButton: false,  timer: 1500});", true);
                    getCredentials(groupid, tenantid);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            }
        }
        catch (Exception)
        {

            throw;
        }
        
    }

    private void ClearAll()
    {
        txtCredentialName.Text = "";
        txtPassWord.Text = "";
        txtUser.Text = "";
    }

    protected void btnClear_Asset(object sender, EventArgs e)
    {
        ClearAll();
    }

    private string Encryption(string passkey)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(txtPassWord.Text);
        //Encrypt
        SymmetricAlgorithm crypt = Aes.Create();
        HashAlgorithm hash = MD5.Create();
        crypt.BlockSize = BlockSize;
        crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(passkey));
        crypt.IV = IV;

        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream =
               new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
            }

            string encryptedpass  = Convert.ToBase64String(memoryStream.ToArray());
           
            return encryptedpass;
        }
    }

    private string Decrypt(string passkey)
    {
        //Get the password value here to decrypt.
        //Decrypt
        byte[] bytes = Convert.FromBase64String("znrmtHSUTrhHugtDZ23WzPXtZB86SONlVvKD6bmeFY4=");
        SymmetricAlgorithm crypt = Aes.Create();
        HashAlgorithm hash = MD5.Create();
        crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(passkey));
        crypt.IV = IV;

        using (MemoryStream memoryStream = new MemoryStream(bytes))
        {
            using (CryptoStream cryptoStream =
               new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
            {
                //byte[] decryptedBytes = new byte[bytes.Length];
                //cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                //string decryptedpassword = ;//Encoding.Unicode.GetString(decryptedBytes);
                using (StreamReader reader = new StreamReader(cryptoStream))
                {
                    // Reutrn all the data from the streamreader   
                    return reader.ReadToEnd();
                }
                //return decryptedpassword;
            }
        }
    }

    public void getCredentials(int groupid, int tenantid)
    {
        result.Clear();
        result = serviceInterface.getCredentials(groupid, tenantid);

        if (result != null)
        {
            GVAssets.DataSource = result;
            GVAssets.DataBind();
        }
        ClearAll();
    }

    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {
        try
        {
            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null || Session["GroupId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string id = commandArgs[0];
                string TenantId = commandArgs[1];
                string GroupId = commandArgs[2];
                string CredentialName = commandArgs[3];

                lblId.Text = id;
                lblCredentialId.Text = CredentialName;

                lblIdSecondPopUp.Text = id;
                lblAssetSecondPopUp.Text = CredentialName;

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
            }
            else
            {
                Session["GroupId"] = null;
                Session["TenantId"] = null;
                serviceInterface.insertLog("Exception: Session Expired from AddUsers Page", "Exception: Session Expired from AddUsers Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        catch (Exception exception)
        {
            serviceInterface.insertLog("Exception while deleting Logs", exception.Message,groupId,tenantId);
        }

    }


    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_Click(object sender,EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }

    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        try
        {
            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            string strid = lblIdSecondPopUp.Text;
            int id = Int32.Parse(strid);
            string AssetName = lblAssetSecondPopUp.Text;

            int TenantId = (int)Session["TenantId"];
            int GroupId = (int)Session["GroupId"];
            string user = (string)Session["UserName"];

            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteCredential(id, groupid, tenantid, AssetName, user);
            if (result > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitCredentialDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitCREDENTIAL") + " " + AssetName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + "  " + groupid + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete Credential Successful", "Delete Credential : Credential with Tenant ID : " + tenantid + "  and Group Id : " + GroupId + " with Credential Name : " + AssetName + "has deleted Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getCredentials(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: ' " + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETECREDENTIAL") + "',text:'" + (String)GetGlobalResourceObject("content", "LitCREDENTIAL") + "   " + AssetName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + "  " + groupid + " " + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + " ', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete User Failed", "Delete User : Deletion of Credential with Credential Name : " + AssetName + " by User : " + user + " with Group ID : " + groupid + " and Tenant ID : " + tenantid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getCredentials(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
        }
        catch (Exception exception)
        {

        }

    }
}