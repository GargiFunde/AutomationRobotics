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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoMasterPage2_AssignQueueToRobot : System.Web.UI.Page
{
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
                serviceInterface.insertLog("Exception: Session Expired from AssignQueueToRobot Page", "Exception: Session Expired from AssignQueueToRobot Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                LoadBots();
                LoadQueues();
                LoadMapping();

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Assign Queue To Bot");
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
                        DivAddQueue.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GVQueBotMapping.Items)
                        {
                            ImageButton deleteQueueBotMapping = item.FindControl("btnDeleteQueueBotMapping") as ImageButton;

                            deleteQueueBotMapping.Enabled = false;
                            deleteQueueBotMapping.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteQueueBotMapping.ToolTip = "You dont have access to DELETE this Queue Robot Mapping. ";
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
                serviceInterface.insertLog("Exception: Session Expired from AssignQueueToRobot Page", "Exception: Session Expired from AssignQueueToRobot Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AssignQueueToRobot Page", "Exception: Session Expired from AssignQueueToRobot Page", 0, 0);
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
    private void LoadMapping()
    {
        int groupid = 0;
        int tenantid = 0;

        if (Session["TenantId"] != null)
        {
             groupid = (int)Session["GroupId"];
             tenantid = (int)Session["TenantId"];
        }
        string strcurrentuser = (string)Session["UserName"];
        // strcurrentuser = "Ajit";//need to comment/remove
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetQueueToBotMapping(groupid, tenantid);
        if (result != null)
        {
            GVQueBotMapping.DataSource = result;
            GVQueBotMapping.DataBind();
            // LoadBots();
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
        DataTable result = serviceInterface.GetBots(groupid, tenantid);

        if (result != null)
        {
            DrpBots.DataSource = result;
            DrpBots.DataValueField = "botname";
            DrpBots.DataTextField = "botname";
            DrpBots.DataBind();
        }
    }
    public void LoadQueues()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetQueues(groupid, tenantid);

        if (result != null)
        {
            DrpQueues.DataSource = result;
            DrpQueues.DataValueField = "queuename";
            DrpQueues.DataTextField = "queuename";
            DrpQueues.DataBind();

            ddlQueueName.DataSource = result;
            ddlQueueName.DataValueField = "queuename";
            ddlQueueName.DataTextField = "queuename";
            ddlQueueName.DataBind();

        }
    }
    protected void DrpGetuser()
    {
        //SqlCommand cmd = new SqlCommand("select * from MTDUsers", con);
        //cmd.CommandType = CommandType.Text;
        //SqlDataAdapter da = new SqlDataAdapter();
        //da.SelectCommand = cmd;
        //DataSet ds = new DataSet();
        //da.Fill(ds);
        //if (ds.Tables.Contains("Table") == true)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DrpUser.DataSource = ds.Tables[0];
        //        DrpUser.DataTextField = ds.Tables[0].Columns[1].ColumnName;
        //        DrpUser.DataValueField = ds.Tables[0].Columns[0].ColumnName;
        //        DrpUser.DataBind();
        //    }
        //}
    }




    public void bindgrid()
    {

        //SqlCommand cmd = new SqlCommand("select * from MTDMenu where RoleID=" + DrpRoles.SelectedValue, con);
        //cmd.CommandType = CommandType.Text;
        //SqlDataAdapter da = new SqlDataAdapter();
        //da.SelectCommand = cmd;
        //DataTable dt = new DataTable();
        //da.Fill(dt);

        //if (dt.Rows.Count > 0)
        //{
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //}
        //else
        //{
        //    DataRow dr = dt.NewRow();
        //    dt.Rows.Add(dr);
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //}


    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField HFDMenuID = (HiddenField)e.Row.FindControl("HDmenuID");
            GridView GridView2 = (GridView)e.Row.FindControl("GridView2");

            //SqlCommand cmd = new SqlCommand("Usp_MTDgetSubMenuBymenu", con);
            //cmd.Parameters.AddWithValue("@MenuID", HFDMenuID.Value);
            //cmd.Parameters.AddWithValue("@role", DrpRoles.SelectedValue);
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //if (ds.Tables.Contains("Table") == true)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        GridView2.DataSource = ds.Tables[0];
            //        GridView2.DataBind();
            //    }
            //}

        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {

        String Success = (String)GetGlobalResourceObject("content", "LitSuccess") ;
        if ((DrpBots.SelectedValue == "Select") || (DrpQueues.SelectedValue  == "Select"))
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
        }

        if (DrpBots.SelectedValue != "Select" && DrpQueues.SelectedValue != "Select")
        { 
          
            string strBotId = string.Empty;
            string strqueuename = string.Empty;
            if (DrpQueues.SelectedValue != null)
            {
                strqueuename = DrpQueues.SelectedValue;
            }
            if (DrpBots.SelectedValue != null)
            {
               strBotId = DrpBots.SelectedValue;
            }
            if ((strBotId == "") || (strqueuename == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitENTERREQUIREDFIELDS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "', showConfirmButton: false,  timer: 1500});", true);
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
                int result = serviceInterface.AssignQueueToBot(strBotId, strqueuename, strcurrentuser, groupid, tenantid);
                if (result > 0)
                {
                    serviceInterface.insertLog("Assign Queue To Bot Successful", "Assign Queue To Bot : Queue with QueueName : " + strqueuename + " has been successfully assigned to Bot : " + strBotId + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "hide_MsgSuccess();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEASSIGNEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitQueueName") + "" + strqueuename + "" + (String)GetGlobalResourceObject("content", "LitassignedtoBot") + "  " + strBotId + "" + (String)GetGlobalResourceObject("content", "Litissuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                    msgBox.InnerHtml = Success;
                    clear();
                }
                else
                {
                    serviceInterface.insertLog("Assign Queue To Bot Failed", "Assign Queue To Bot : Failed to assign Queue with QueueName : " + strqueuename + " to Bot : " + strBotId + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                }
            }

        }
        LoadMapping();
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
            int result = serviceInterface.DeleteQueueToBotMapping(strid, groupid, tenantid, strcurrentuser);
            if (result == 1)
            {
                LoadMapping();
            }
        }
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
                    string defaultqueuename = commandArgs[2];
                    string strcurrentuser = (string)Session["UserName"];

                    lblId.Text = id;
                    lblRobotName.Text = botName;
                    lblQueueName.Text = defaultqueuename;

                    lblIdSecondPopUp.Text = id;
                    lblRobotNameSecondPopUp.Text = botName;
                    lblQueueNameSecondPopUp.Text = defaultqueuename;

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);

                }
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('EXCEPTION WHILE DELETING ROBOT', 'Please contact Admin for Deletion Exception', 'error').then((value) => {  window.location.href = 'AddRobot.aspx';}); ", true);
        }
    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        string id = string.Empty;
        string robotName = string.Empty;
        string queueName = string.Empty;
        string strcurrentuser = string.Empty;
        int groupid = 0;
        int tenantid = 0;
        int result = 0;
        try
        {
            if (Session["TenantId"] != null || Session["groupid"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                result = serviceInterface.DeleteQueueToBotMapping(id, groupid, tenantid, strcurrentuser);
                if (result > 0)
                {
                    serviceInterface.insertLog("Deletion of QueueMapping To Bot Successful", "Delete QueueMapping To Bot : Mapping of Queue with QueueName : " + queueName + " and Bot with BotName : " + robotName + " has been deleted successsfully by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEMAPPINGWITHROBOTDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotName") + " " + robotName + "" + (String)GetGlobalResourceObject("content", "LitwithQueueName") + "  " + queueName + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    LoadMapping();
                }
                else
                {
                    serviceInterface.insertLog("Deletion of QueueMapping To Bot Successful", "Delete QueueMapping To Bot : Mapping of Queue with QueueName : " + queueName + " and Bot with BotName : " + robotName + " has been deleted successsfully by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEMAPPINGWITHROBOTDELETEDFALED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotName") + "" + robotName + " " + (String)GetGlobalResourceObject("content", "LitwithMachineName") + " " + queueName + "" + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + " ', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    LoadMapping();
                }
            }


        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Deletion of QueueMapping To Bot Failed", "Delete QueueMapping To Bot : Deletion of Mapping of Queue with QueueName : " + queueName + " and Bot with BotName : " + robotName + " by User : " + strcurrentuser + " with TenantID : " + tenantid + " and Group ID : " + groupid + "has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }


    protected void GVQueBotMapping_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        LoadBots();
        LoadQueues();
        LoadMapping();

    }

    protected void clear()
    {
        if (DrpBots.SelectedValue != "Select")
        {
            DrpBots.SelectedValue = "Select";
        }
        if (DrpQueues.SelectedValue != "Select")
        {
            DrpQueues.SelectedValue = "Select";
        }
    }
   
     protected void BtnCancelUser(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }

    protected void btnXclosePopup(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void UpdateQueue_click(object sender, EventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        string botname = txtBotName.Text;
        string QueueName = ddlQueueName.SelectedValue;
        int result = serviceInterface.updateQueueToBotMapping(groupid, tenantid, botname, QueueName);

        if (result != 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUENOTUPDATESUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + botname + "" + (String)GetGlobalResourceObject("content", "LitwithQueueName") + "  " + QueueName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenupdatedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEUPDATESUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + botname + " " + (String)GetGlobalResourceObject("content", "LitwithQueueName") + " " + QueueName + " " + (String)GetGlobalResourceObject("content", "Lithasbeennotupdatedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            LoadMapping();
            txtBotName.Text = "";
            lblQuenameHidden.Value = "";
            ddlQueueName.SelectedIndex = 0;
        }

    }


    protected void btnClear_click(object sender, EventArgs e)
    {
        ddlQueueName.SelectedIndex = 0;
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openUpdate()", true);
    }
    protected void checkForQueueName_Click(object sender, EventArgs e)
    {
        string botName = txtBotName.Text;
        string queueNameprev = lblQuenameHidden.Value;
        string queueNamenext = ddlQueueName.SelectedValue;
        string botQueue = botName + "," + queueNameprev;

        if (queueNamenext == queueNameprev)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: ' " + (String)GetGlobalResourceObject("content", "LitQUEUEALREADYASSIGNE") + "',text:'" + (String)GetGlobalResourceObject("content", "LitPleaseselectanotherqueuename") + "', showConfirmButton: false,  timer: 1500});", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openUpdate()", true);
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openUpdate()", true);
        }

    }
}