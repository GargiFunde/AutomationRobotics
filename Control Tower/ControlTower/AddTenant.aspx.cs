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
using System.Data.SqlClient;
//using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class DemoMasterPage2_AddTenant : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            }
            else
            {
                txtTenantName.Text = "";
                txtOwner.Text = "";
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];
                getAllTenants(groupid, tenantid);
            }
        }
        else if (IsPostBack)
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from AddTenant Page", "Exception: Session Expired from AddTenant Page", 0, 0);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from AddTenant Page", "Exception: Session Expired from AddTenant Page", 0, 0);
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
    protected void Save_Click(object sender, EventArgs e)
    {

        string tenantName = txtTenantName.Text;
        string owner = txtOwner.Text;

        string TenantName = string.Empty;
        TenantName = txtTenantName.Text;
        txtDomain.Text = TenantName;

        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];

        ServiceInterface serviceInterface = new ServiceInterface();
        tenantid = serviceInterface.GetTenantId(TenantName, groupid, tenantid);

        if (tenantid > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitTENANTNAMEALREADYEXIST") + "',text:'" + (String)GetGlobalResourceObject("content", "LitTenantName") + "" + txtTenantName.Text + " " + (String)GetGlobalResourceObject("content", "Litalreadyexist") + "', showConfirmButton: false,  timer: 1500});", true);

        }
        else
        {
            if ((tenantName == "") || (owner == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtTenantName.Text = null;
        txtOwner.Text = null;
    }
    private void AddTenant()
    {
        int iTenantId = 0;
        string tenantName = string.Empty;
        string owner = string.Empty;

        string strcurrentuser = string.Empty;
        int groupid = 0;
        int tenantid = 0;
        try
        {
            iTenantId = (int)Session["TenantId"];  //should come from session object
            tenantName = txtTenantName.Text.Trim();
            owner = txtOwner.Text.Trim();

            if ((tenantName == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            }
            else
            {
                strcurrentuser = (string)Session["UserName"];
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                ServiceInterface serviceInterface = new ServiceInterface();
                int result = serviceInterface.AddTenant(tenantName, owner, groupid, tenantid, strcurrentuser);
                if (0 < result)
                {
                    serviceInterface.insertLog("Add Tenant Successful", "Add Tenant : Tenant with TenantName: " + tenantName + " and Owner : " + owner + " has been added successfully by User " + strcurrentuser + " with GroupID : " + groupid + " and TenantId : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                }
                else
                {
                    serviceInterface.insertLog("Tenant Addition Failed", "Add Tenant : Tenant with TenantName: " + tenantName + " and Owner : " + owner + " has FAILED to add by User " + strcurrentuser + " with GroupID : " + groupid + " and TenantId : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                }
            }
        }
        catch (Exception e)
        {
            serviceInterface.insertLog("Add Tenant Failed", "Add Tenant : Addition of Tenant with TenantName: " + tenantName + " and Owner : " + owner + " by User " + strcurrentuser + " with GroupID : " + groupid + " and TenantId : " + tenantid + " has failded.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }

    public void getAllTenants(int tenantid, int groupid)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetAllTenants(groupid, tenantid);

        if (result != null)
        {
            GVRoles.DataSource = result;
            GVRoles.DataBind();
        }
    }

    protected void GVRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        getAllTenants(groupid, tenantid);
    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        getAllTenants(groupid, tenantid);
    }

    protected void CreateNewUser(object sender, EventArgs e)
    {
        AddTenant();

        char[] charsToTrim = {' ', '\'' };

        string TenantName = string.Empty;
        TenantName = txtTenantName.Text;

        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        int result = 0;
        ServiceInterface serviceInterface = new ServiceInterface();

        tenantid = serviceInterface.GetTenantId(TenantName, groupid, tenantid);
        string groupName = String.Empty;
        groupName = "Default";
        groupid = serviceInterface.GetGroupId(groupName,tenantid);

        string domainName = TenantName;
        string strDomainName = domainName;
        string strUserName = txtUser.Text;
        string strPwd = txtPwd.Text;
        string strUserRole = DrpRoleType.Text;

        string encodePwd = string.Empty;
        byte[] encode = new byte[strPwd.Length];
        encode = System.Text.Encoding.UTF8.GetBytes(strPwd);
        encodePwd = Convert.ToBase64String(encode);
        strPwd = encodePwd;

        if ((strUserName == "") || (strPwd == ""))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        else
        {
            string strcurrentuser = (string)Session["UserName"];
            result = serviceInterface.Adduser(strDomainName, strUserName, strPwd, groupid, groupName, tenantid, strUserRole, strcurrentuser);
            if (0 < result)
            {
                serviceInterface.insertLog("Add Tenant Master Admin Successful", "Add Tenant Master Admin : Tenant Master Admin with Name : " + strUserName + " and TenantName : " + strDomainName + " with Role : " + strUserRole + " has been added successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitTENANTADDEDSUCCESSFULLY") + "   ',text:'" + (String)GetGlobalResourceObject("content", "LitTenantName") + " " + txtTenantName.Text + " " + (String)GetGlobalResourceObject("content", "LitTenantMasterAdmin") + "  " + strUserName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                getAllTenants(tenantid, groupid);
                clearField();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                serviceInterface.insertLog("Add Tenant Master Admin Failed", "Addition of Tenant Master Admin : Add Tenant Master Admin with Name : " + strUserName + " and TenantName : " + strDomainName + " with Role : " + strUserRole + " by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid + " has failed. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitTENANTADDINGFAILED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitTenantName") + " " + txtTenantName.Text + "" + (String)GetGlobalResourceObject("content", "LitTenantMasterAdmin") + "  " + strUserName + "" + txtTenantName.Text + "" + (String)GetGlobalResourceObject("content", "LithasFailedtoAddtoDataBase") + " ', showConfirmButton: false,  timer: 1500});", true);
            }
        }
    }
    protected void cancelPopup(object sender, EventArgs e)
    {
        clearPopUpField();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void btnXclosePopup(object sender, EventArgs e)
    {
        clearPopUpField();
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void clearField()
    {
        txtTenantName.Text = null;
        txtOwner.Text = null;
        txtUser.Text = null;
        txtPwd.Text = null;
    }

    protected void clearPopUpField()
    {
        txtUser.Text = null;
        txtPwd.Text = null;
    }

    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

        string strTenantId = commandArgs[0];
        string strTenantName = commandArgs[1];

        lblTenanIdDelete.Text = strTenantId;
        lblTenanNameDelete.Text = strTenantName;

        lblTenanIdDeleteSecondPopUp.Text = strTenantId;
        lblTenanNameDeleteSecondPopUp.Text = strTenantName;

        lblTenanIdDeleteThird.Text = strTenantId;
        lblTenanNameDeleteThird.Text = strTenantName;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
    }

    protected void ModalPopUpBtnDelete_ClickThirdPopUp(object sender, EventArgs e)
    {
        int groupid = 0;
        int tenantid = 0;
        int tenantidtodelete = 0;

        string strcurrentuser = string.Empty;
        string strTenantName = string.Empty;
        int result = 0;

        try
        {
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            strcurrentuser = (string)Session["UserName"];
            tenantidtodelete = Int32.Parse(lblTenanIdDelete.Text);
            strTenantName = lblTenanNameDelete.Text;

            ServiceInterface serviceInterface = new ServiceInterface();
            result = serviceInterface.DeleteTenantWithAllRelatedData(groupid, tenantidtodelete, strcurrentuser);

            if (result > 0)
            {
                serviceInterface.insertLog("Delete Tenant Successful", "Delete Tenant : Tenant with TenantName: " + strTenantName + " has been deleted Successfully  ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitTENANTDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitTenantName") + " " + strTenantName + " " + (String)GetGlobalResourceObject("content", "LitwithID") + " " + tenantidtodelete + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                getAllTenants(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                serviceInterface.insertLog("Delete Tenant Failed", "Delete Tenant : Deletion of Tenant with TenantName: " + strTenantName + " has failed.  ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETETENANT") + "',text:'" + (String)GetGlobalResourceObject("content", "LitTenantName") + "  " + strTenantName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + "" + tenantidtodelete + " " + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + "', showConfirmButton: false,  timer: 1500});", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception: Add Tenant ", "Delete Tenant : Deletion of Tenant with TenantName: " + strTenantName + " has failed.Exception :  "+ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
       
    }
    protected void ModalPopUpBtnClose_ClickPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void CommandBtnDelete_Click(object sender, EventArgs e)
    {
        DataTable result = null;
        int resultStatus = 0;
        int tenantid = Int32.Parse(lblTenanIdDelete.Text);

        int cnt_Bot = 0;
        int cnt_Group = 0;
        int cnt_User = 0;
        int cnt_Process = 0;
        int cnt_RQDetails = 0;
        int cnt_ProcessUpload = 0;
        int cnt_ScheduleDeatils = 0;
        int cnt_UserBotMapping = 0;
        int cnt_botdefaultqueue = 0;
        int cnt_configurationparameters = 0;
        int cnt_messagingdetails = 0;

        ServiceInterface serviceInterface = new ServiceInterface();
        result = serviceInterface.GetCountToDeleteTenant(tenantid);

        if ((result.Rows[0][0] != null) && (result.Rows[0][0] != System.DBNull.Value))
            cnt_Group = (int)result.Rows[0][0];

        if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
            cnt_User = (int)result.Rows[0][1];

        if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
            cnt_Bot = (int)result.Rows[0][2];

        if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
            cnt_Process = (int)result.Rows[0][3];

        if ((result.Rows[0][4] != null) && (result.Rows[0][4] != System.DBNull.Value))
            cnt_ProcessUpload = (int)result.Rows[0][4];

        if ((result.Rows[0][5] != null) && (result.Rows[0][5] != System.DBNull.Value))
            cnt_RQDetails = (int)result.Rows[0][5];

        if ((result.Rows[0][6] != null) && (result.Rows[0][6] != System.DBNull.Value))
            cnt_ScheduleDeatils = (int)result.Rows[0][6];

        if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
            cnt_UserBotMapping = (int)result.Rows[0][7];

        if ((result.Rows[0][8] != null) && (result.Rows[0][8] != System.DBNull.Value))
            cnt_botdefaultqueue = (int)result.Rows[0][8];

        if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
            cnt_messagingdetails = (int)result.Rows[0][10];

        if ((result.Rows[0][10] != null) && (result.Rows[0][11] != System.DBNull.Value))
            cnt_configurationparameters = (int)result.Rows[0][11];

        lblGroups.Text = cnt_Group.ToString();
        lblUsers.Text = cnt_User.ToString();
        lblBot.Text = cnt_Bot.ToString();
        lblProcess.Text = cnt_Process.ToString();
        lblProcessUpload.Text = cnt_ProcessUpload.ToString();
        lblRQDetails.Text = cnt_RQDetails.ToString();
        lblScheduleDeatils.Text = cnt_ScheduleDeatils.ToString();
        lblUserBotMapping.Text = cnt_UserBotMapping.ToString();
        lblBotdefaultqueue.Text = cnt_botdefaultqueue.ToString();
        lblMessagingdetails.Text = cnt_messagingdetails.ToString();
        lblconfigurationparameters.Text = cnt_configurationparameters.ToString();


        resultStatus = serviceInterface.CheckIsactiveStatusTenant(tenantid);
        if (resultStatus == 1)
        {
            btnEnableDisable.Text = "Disable";
            btnEnableDisable.Attributes["class"] = "btn btn-warning btn-block";

        }
        if (resultStatus == 0)
        {
            btnEnableDisable.Text = "Enable";
            btnEnableDisable.Attributes["class"] = "btn btn-success btn-block";
        }

        if (result != null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
        }

    }

    protected void CommandBtnDeleteClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl, false);
    }


    protected void CommandBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteThirdPopUp();", true);

    }

    protected void changeColor()
    {
        int resultStatus = 0;
        int tenantid = Int32.Parse(lblTenanIdDelete.Text);
        ServiceInterface serviceInterface = new ServiceInterface();
        resultStatus = serviceInterface.CheckIsactiveStatusTenant(tenantid);
        if (resultStatus == 1)
        {
            btnEnableDisable.Text = "Disable";
            btnEnableDisable.Attributes["class"] = "btn btn-warning btn-block";

        }
        if (resultStatus == 0)
        {
            btnEnableDisable.Text = "Enable";
            btnEnableDisable.Attributes["class"] = "btn btn-success btn-block";
        }
    }

    protected void CommandBtnDisable_Click(object sender, EventArgs e)
    {
        int tenantidToDisable = 0;
        int isActiveStatus = 0;
        tenantidToDisable = Int32.Parse(lblTenanIdDelete.Text);

        if (btnEnableDisable.Text == "Disable")
        {
            isActiveStatus = -1;
            btnEnableDisable.Attributes["class"] = "btn btn-warning btn-block";


        }
        if (btnEnableDisable.Text == "Enable")
        {
            isActiveStatus = 0;
            btnEnableDisable.Attributes["class"] = "btn btn-success btn-block";
        }


        ServiceInterface serviceInterface = new ServiceInterface();
        int result = serviceInterface.UpdateIsactiveStatusTenantRelatedTables(tenantidToDisable, isActiveStatus);
        if (result >= 1)
        {
            changeColor();
            serviceInterface.insertLog("Update Tenant Active Status Successful", "IsActive Status of Tenant : Tenant with ID: " + tenantidToDisable + " has been updated successfully to " + isActiveStatus, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
            
        }
        else
        {
            serviceInterface.insertLog("Update Tenant Active Status ", "IsActive Status of Tenant : Update IsActive Status of Tenant with ID: " + tenantidToDisable + " to " + isActiveStatus + " has failed. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }
}