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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using Logger;
public partial class DemoMasterPage2_AssignRobotToUser : System.Web.UI.Page
{
    public int groupid = 0;
    public int tenantid = 0;
    public string strcurrentuser = String.Empty;
    ServiceInterface serviceInterface = new ServiceInterface();
    protected int groupId = 0;
    protected int tenantId = 0;
    protected string userName = string.Empty;
    DataTable db = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AssignRobotToUser Page", "Exception: Session Expired from AssignRobotToUser Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                groupid = Convert.ToInt32(Session["GroupId"]);
                tenantid = Convert.ToInt32(Session["TenantId"]);
                strcurrentuser = Session["UserName"].ToString();

                LoadUsers();
                LoadBots();
                LoadMapping(strcurrentuser, groupid, tenantid);

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Assign Bot To User");
                    bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                    bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                    bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                    bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                    if (!ViewAccess)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                        //Will be decided from MasterPage.
                    }

                    if (!CreateAccess)
                    {
                        DivRobotUserMapping.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GVUserToBotMapping.Items)
                        {
                            ImageButton deleteRobotUserMapping = item.FindControl("btnDeleteRobotUserMapping") as ImageButton;

                            deleteRobotUserMapping.Enabled = false;
                            deleteRobotUserMapping.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteRobotUserMapping.ToolTip = (String)GetGlobalResourceObject("content", "LitYoudonthaveaccesstoDELETEthisRobotUserMapping") ;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion
            }
        }
        else if (IsPostBack)
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AssignRobotToUser Page", "Exception: Session Expired from AssignRobotToUser Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AssignRobotToUser Page", "Exception: Session Expired from AssignRobotToUser Page", 0, 0);
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
    protected void loadMapping()
    {
        try {
            if (Session["TenantId"] != null || Session["GroupId"] != null)
            {
                groupid = Convert.ToInt32(Session["GroupId"]);
                tenantid = Convert.ToInt32(Session["TenantId"]);
                strcurrentuser = Session["UserName"].ToString();

                LoadMapping(strcurrentuser, groupid, tenantid);
            }
        }
        catch (Exception ex) {
            string logTenantId = Session["TenantId"].ToString();
            string logGroupId = Session["GroupId"].ToString();
            //Log.TenantName = logTenantId;
            ////Log.
            //Logger.Log.Logger.LogData("Error while LoadMapping : "+ex.Message,Logger.LogLevel.Error);
        }

        
    }

    public void LoadBots()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetBots(groupid,tenantid);

        if (result != null)
        {
            DrpBots.DataSource = result;
            DrpBots.DataValueField = "botname";
            DrpBots.DataTextField = "botname";
            DrpBots.DataBind();
        }
    }
    public void LoadUsers()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetUsers(groupid, tenantid);

        if (result != null)
        {
            DrpUsers.DataSource = result;
            DrpUsers.DataValueField = "userid";
            DrpUsers.DataTextField = "userid";
            DrpUsers.DataBind();
        }
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        if ((DrpBots.SelectedValue == "0") || (DrpUsers.SelectedValue == "0"))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitPLEASESELECTREQUIREDFIELDS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitMandatoryFieldsarenotselected") + "' , showConfirmButton: false,  timer: 1500});", true);
        }

        if (DrpBots.SelectedValue != "0" && DrpUsers.SelectedValue != "0")
        {
            string strBotId = string.Empty;
            string strUserId = string.Empty;

            if (DrpBots.SelectedValue != null)
            {
                strBotId = DrpBots.SelectedValue;
            }
            if (DrpUsers.SelectedValue != null)
            {
                strUserId = DrpUsers.SelectedValue;
            }
            if ((strBotId == "") || (strUserId == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitPLEASESELECTREQUIREDFIELDS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitMandatoryFieldsarenotselected") + "' , showConfirmButton: false,  timer: 1500});", true);
            }
            else
            {
                int groupid = 0;
                int tenantid = 0;
                if (Session["TenantId"] != null)
                {
                    groupid = (int)Session["GroupId"];
                    tenantid = (int)Session["TenantId"];
                }

                string strcurrentuser = (string)Session["UserName"];
                ServiceInterface serviceInterface = new ServiceInterface();
                int result = serviceInterface.AssignBotToUser(strBotId, strUserId, strcurrentuser, groupid, tenantid);
                if (result > 0)
                {
                    serviceInterface.insertLog("Mapping of Robot To User Successful", "Assign Robot To User : Robot with Name " + strBotId + " has been mapped successfully to User : " + strUserId + " by User : " + strcurrentuser + " with Tenant Id : " + tenantid + " and Group Id : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitROBOTASSIGNEDTOUSERSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + strBotId + "" + (String)GetGlobalResourceObject("content", "LitwithUserName") + "  " + strUserId + "" + (String)GetGlobalResourceObject("content", "Lithasbeenmappedsuccessfully!") + " ' , showConfirmButton: false,  timer: 1500});", true);
                    LoadMapping(strUserId, groupid, tenantid);
                }
                else
                {
                    serviceInterface.insertLog("Mapping of Robot To User Successful", "Assign Robot To User : Robot with Name " + strBotId + " has been mapped successfully to User : " + strUserId + " by User : " + strcurrentuser + " with Tenant Id : " + tenantid + " and Group Id : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOASSIGNROBOT") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + strBotId + " " + (String)GetGlobalResourceObject("content", "LitwithUserName") + "  " + strUserId + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenmappedsuccessfully") + "' , showConfirmButton: false,  timer: 1500});", true);
                }
            }
        }
        clear();
        
    }
    private void LoadMapping(string strUserId, int groupid, int tenantid)
    {

        if (strUserId != "")
        {
            ServiceInterface serviceInterface = new ServiceInterface();
            DataTable result = serviceInterface.GetUserToBotMapping(strUserId,groupid,tenantid);
            if (result != null)
            {
                GVUserToBotMapping.DataSource = result;
                GVUserToBotMapping.DataBind();
            }
        }
    }

    protected void DrpUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        string strcurrentuser = (string)Session["UserName"];
        LoadMapping(strcurrentuser, groupid, tenantid);
    }
    protected void CommandBtnDelete_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }

            string strid = e.CommandArgument.ToString();
            string strcurrentuser = (string)Session["UserName"];
            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteUserToBotMapping(strid, groupid,tenantid, strcurrentuser);
            if (result == 1)
            {
                LoadMapping(strcurrentuser,groupid,tenantid);
            }
        }
    }
    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }
    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                int groupid = 0;
                int tenantid = 0;
                if (Session["TenantId"] != null || Session["GroupId"] != null)
                {
                    groupid = (int)Session["GroupId"];
                    tenantid = (int)Session["TenantId"];

                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    string id = commandArgs[0];
                    string botName = commandArgs[1];
                    string userName = commandArgs[2];
                    string strcurrentuser = (string)Session["UserName"];

                    lblId.Text = id;
                    lblBotName.Text = botName;
                    lblUserName.Text = userName;
                    lblIdSecondPopUp.Text = id;
                    lblBotNameSecondPopUp.Text = botName;
                    lblUserNameSecondPopUp.Text = userName;


                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);

                }


            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + (String)GetGlobalResourceObject("content", "LitEXCEPTIONWHILEDELETINGROBOT") + "', '" + (String)GetGlobalResourceObject("content", "LitPleasecontactAdminforDeletionException") + "', 'error').then((value) => {  window.location.href = 'AddRobot.aspx';}); ", true);
        }
    }
    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        string id = string.Empty;
        string robotName = string.Empty;
        string userName = string.Empty;
        string strcurrentuser = string.Empty;
        int groupid = 0;
        int tenantid = 0;
        int result = 0;
        try
        {
             id = lblId.Text; //Id tof QueueMapping to be deleted
             robotName = lblBotName.Text;
             userName = lblUserName.Text;
             strcurrentuser = (string)Session["UserName"];
            
            if (Session["TenantId"] != null || Session["groupid"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                result = serviceInterface.DeleteUserToBotMapping(id, groupid, tenantid, strcurrentuser);
                if (result > 0)
                {
                    serviceInterface.insertLog("Delete User Mapping With Robot Successful", "Delete User Mapping With Robot : Mapping of User : " + userName + " with Bot : " + robotName + " has been deleted successfully by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID :" + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitUSERMAPPINGWITHROBOTDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + robotName + "" + (String)GetGlobalResourceObject("content", "LitwithUserName") + " " + userName + "" + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    LoadMapping(strcurrentuser, groupid, tenantid);
                }
                else
                {
                    serviceInterface.insertLog("Delete User Mapping With Robot Failed", "Delete User Mapping With Robot : Deletion of Mapping of User : " + userName + " with Bot : " + robotName + " by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID :" + groupid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDDELETONOFUSERMAPPINGWITHROBOT") + "',text: '" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + robotName + " " + (String)GetGlobalResourceObject("content", "LitwithUserName") + " " + User + " " + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                }
            }


        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception : Delete User Mapping With Robot Failed", "Delete User Mapping With Robot : Deletion of Mapping of User : " + userName + " with Bot : " + robotName + " by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID :" + groupid + " has failed.Exception: "+ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void DrpUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void GVUserToBotMapping_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        LoadUsers();
        LoadBots();
      

    }
    protected void clear()
    {
        if (DrpUsers.SelectedValue != "0")
        {
            DrpUsers.SelectedValue = "0";
        }

        if (DrpBots.SelectedValue != "0")
        {
            DrpBots.SelectedValue = "0";
        }
    }
    protected void BtnCancelUser(object sender, EventArgs e)
    {

        clear();

    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void btnXclosePopup(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void btnClear_click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    //protected void UpdateQueue_click(object sender, EventArgs e)
    //{
    //    int groupid = (int)Session["GroupId"];
    //    int tenantid = (int)Session["TenantId"];       
    //    strcurrentuser = Session["UserName"].ToString();

    //    string username = txtUserName.Text;
    //    string botname = ddlBotName.SelectedValue;
    //    int result = serviceInterface.updateBotNameTouserBotMapping(groupid, tenantid, botname, username);

    //    if (result != 1)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'BOT NAME NOT UPDATE SUCCESSFULLY!',text:'Robot Name " + botname + " with User Name : " + username + " has been updated successfully!', showConfirmButton: false,  timer: 1500});", true);
    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: 'BOT NAME UPDATE SUCCESSFULLY!',text:'Robot Name " + botname + " with User Name : " + username + " has been not updated successfully!', showConfirmButton: false,  timer: 1500});", true);
    //        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    //        LoadMapping(strcurrentuser, groupid, tenantid);
    //    }

    //}

}