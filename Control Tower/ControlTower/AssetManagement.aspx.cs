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
using CommonLibrary;

public partial class ControlTower_AssetManagement1 : System.Web.UI.Page
{
    public ServiceInterface serviceInterface = new ServiceInterface();
    EncryptionHelper encryptionHelper = new EncryptionHelper();
    protected int groupId = 0;
    protected int tenantId = 0;
    protected string userName = string.Empty;
    public DataTable result = new DataTable();

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
                getAssets(groupid, tenantid);
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

    protected override void InitializeCulture()
    {
        UICulture = Request.UserLanguages[0];
        base.InitializeCulture();
    }

    protected void btnSave_Asset(object sender, EventArgs e)
    {
        try
        {

            if (txtAssetName.Text != "" && txtAssetValue.Text != "")
            {


                string strAssetName = txtAssetName.Text;
                string strAssetValue = txtAssetValue.Text;
                string encrKey = serviceInterface.getKey();
                string strEncryptedAssetValue = encryptionHelper.Encrypt(strAssetValue, encrKey);

                string strcreatedBy = (string)Session["UserName"];
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];

                int result = serviceInterface.AddAssets(strAssetName, strEncryptedAssetValue, tenantid, groupid, strcreatedBy);
                if (0 < result)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitASSETDETAILSADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAsset") + "" + strAssetName + " " + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                    getAssets(groupid, tenantid);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOADDASSETDETAILS") + " ," + (String)GetGlobalResourceObject("content", "LitASSETNAMEMAYALREADYPRESENT") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAsset") + " " + strAssetName + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + " ', showConfirmButton: false,  timer: 1500});", true);
                    getAssets(groupid, tenantid);
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
        txtAssetName.Text = "";
        txtAssetValue.Text = "";
    }

    protected void btnClear_Asset(object sender, EventArgs e)
    {
        ClearAll();
    }

    public void getAssets(int groupid, int tenantid)
    {
        result.Clear();

        string encrKey = serviceInterface.getKey();
        result = serviceInterface.getAssets(groupid, tenantid);
        foreach (DataRow row in result.Rows)
        {
            row["value"] = encryptionHelper.Decrypt(row["value"].ToString(), encrKey);
        }

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
                string AssetName = commandArgs[3];

                lblId.Text = id;
                lblAssetId.Text = AssetName;

                lblIdSecondPopUp.Text = id;
                lblAssetSecondPopUp.Text = AssetName;

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
            serviceInterface.insertLog("Exception while deleting Logs", exception.Message, groupId, tenantId);
        }

    }


    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
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
            int result = serviceInterface.DeleteAsset(id, groupid, tenantid, AssetName, user);
            if (result > 0)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitASSETDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAsset") + " " + AssetName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + "  " + groupid + "" + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete Asset Successful", "Delete Asset : Asset with Tenant ID : " + tenantid + "  and Group Id : " + GroupId + " with Asset Name : " + AssetName + "has deleted Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getAssets(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETEASSET") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAsset") + "   " + AssetName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + groupid + "" + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + "', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete User Failed", "Delete User : Deletion of Asset with Asset Name : " + AssetName + " by User : " + user + " with Group ID : " + groupid + " and Tenant ID : " + tenantid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getAssets(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
        }
        catch (Exception exception)
        {

        }

    }
}