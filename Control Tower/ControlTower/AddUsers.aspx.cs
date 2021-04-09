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


using CommonLibrary;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoMasterPage2_AddUsers : System.Web.UI.Page
{
    public ServiceInterface serviceInterface = new ServiceInterface();
    public DataTable result = new DataTable();
    protected int groupId = 0;
    protected int tenantId = 0;
    protected string userName = string.Empty;
    DataTable db = null;
    EncryptionHelper encryptionHelper = new EncryptionHelper();
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

                result.Clear();
                result = serviceInterface.GetGroups(groupid,tenantid);

                if (result != null)
                {
                    DrpRoleType.DataSource = result;
                    DrpRoleType.DataValueField = "groupname";
                    DrpRoleType.DataTextField = "groupname";
                    DrpRoleType.DataBind();
                }

                txtDomain.Text = Session["DomainName"].ToString();
                getdata(groupid, tenantid);
                getGrounNames(tenantid);
                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupid, tenantid, "Add User");
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
                        DIVAddUsers.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //btnUserAccess.Enabled = false;
                        foreach (RepeaterItem item in GVRoles.Items)
                        {
                            ImageButton userAccess = item.FindControl("btnUserAccess") as ImageButton;
                            userAccess.Attributes.CssStyle.Add("opacity", "0.5");
                            userAccess.ToolTip = "You have only Read Access.";
                        }
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GVRoles.Items)
                        {
                            ImageButton deleteProcess = item.FindControl("btnDeleteUser") as ImageButton;

                            deleteProcess.Enabled = false;
                            deleteProcess.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteProcess.ToolTip = "You dont have access to DELETE this Process";
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
    public void getGrounNames(int tenantid)
    {
        ServiceInterface serviceInterface = new ServiceInterface();


        DataTable result = serviceInterface.getGrounNames(tenantid);

        if (result != null)
        {
            DrpGroupName.DataSource = result;
            DrpGroupName.DataValueField = "groupname";
            DrpGroupName.DataTextField = "groupname";
            DrpGroupName.DataBind();
        }

    }


    protected void getRoletypesForGroup(object sender, EventArgs e)
    {

        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        string groupname = DrpGroupName.SelectedValue;
        getRoleType(groupname, groupid, tenantid);
    }

    public void getRoleType(string groupname, int groupid, int tenantid)
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.getRoleType(groupname, groupid, tenantid);

        if (result != null)
        {
            DrpRoleType.Items.Clear();
            DrpRoleType.Items.Insert(0, "--Select--");
            DrpRoleType.DataSource = result;
            DrpRoleType.DataValueField = "rolename";
            DrpRoleType.DataTextField = "rolename";
            DrpRoleType.DataBind();
        }

    }
    #region BindPopupData on 26th Nov 2019
    //protected void BindPopupData(int groupid, int tenantid)
    //{
    //    ServiceInterface serviceInterface = new ServiceInterface();
    //    DataTable result = serviceInterface.GetUserBaseAccess(groupid, tenantid);
    //    if (result != null && result.Rows.Count > 0 && result.Rows[0][1] != DBNull.Value)
    //    {
    //        chkBotDashboardR.Checked = Convert.ToBoolean(result.Rows[0][1]);
    //        chkBotDashboardC.Checked = Convert.ToBoolean(result.Rows[0][2]);
    //        chkBotDashboardE.Checked = Convert.ToBoolean(result.Rows[0][3]);
    //        chkBotDashboardD.Checked = Convert.ToBoolean(result.Rows[0][1]);

    //        chkQueueR.Checked = Convert.ToBoolean(result.Rows[1][1]);
    //        chkQueueC.Checked = Convert.ToBoolean(result.Rows[1][2]);
    //        chkQueueE.Checked = Convert.ToBoolean(result.Rows[1][3]);
    //        chkQueueD.Checked = Convert.ToBoolean(result.Rows[1][4]);


    //        chkAddScheduleR.Checked = Convert.ToBoolean(result.Rows[2][1]);
    //        ChkAddScheduleC.Checked = Convert.ToBoolean(result.Rows[2][2]);
    //        chkAddScheduleE.Checked = Convert.ToBoolean(result.Rows[2][3]);
    //        chkAddScheduleD.Checked = Convert.ToBoolean(result.Rows[2][4]);

    //        chkAddUserR.Checked = Convert.ToBoolean(result.Rows[3][1]);
    //        chkAddUserC.Checked = Convert.ToBoolean(result.Rows[3][2]);
    //        chkAddUserE.Checked = Convert.ToBoolean(result.Rows[3][3]);
    //        chkAddUserD.Checked = Convert.ToBoolean(result.Rows[3][4]);

    //        chkAddRobotR.Checked = Convert.ToBoolean(result.Rows[4][1]);
    //        chkAddRobotC.Checked = Convert.ToBoolean(result.Rows[4][2]);
    //        chkAddRobotE.Checked = Convert.ToBoolean(result.Rows[4][3]);
    //        chkAddRobotD.Checked = Convert.ToBoolean(result.Rows[4][4]);

    //        chkAddQueueR.Checked = Convert.ToBoolean(result.Rows[5][1]);
    //        chkAddQueueC.Checked = Convert.ToBoolean(result.Rows[5][2]);
    //        chkAddQueueE.Checked = Convert.ToBoolean(result.Rows[5][3]);
    //        chkAddQueueD.Checked = Convert.ToBoolean(result.Rows[5][4]);

    //        chkAddGroupR.Checked = Convert.ToBoolean(result.Rows[6][1]);
    //        chkAddGroupC.Checked = Convert.ToBoolean(result.Rows[6][2]);
    //        chkAddGroupE.Checked = Convert.ToBoolean(result.Rows[6][3]);
    //        chkAddGroupD.Checked = Convert.ToBoolean(result.Rows[6][4]);


    //        chkAssignQueueBotR.Checked = Convert.ToBoolean(result.Rows[7][1]);
    //        chkAssignQueueBotC.Checked = Convert.ToBoolean(result.Rows[7][2]);
    //        chkAssignQueueBotE.Checked = Convert.ToBoolean(result.Rows[7][3]);
    //        chkAssignQueueBotD.Checked = Convert.ToBoolean(result.Rows[7][4]);

    //        chkAssignBotUserR.Checked = Convert.ToBoolean(result.Rows[8][1]);
    //        chkAssignBotUserC.Checked = Convert.ToBoolean(result.Rows[8][2]);
    //        chkAssignBotUserE.Checked = Convert.ToBoolean(result.Rows[8][3]);
    //        chkAssignBotUserD.Checked = Convert.ToBoolean(result.Rows[8][4]);


    //        chkBotLogR.Checked = Convert.ToBoolean(result.Rows[9][1]);
    //        chkBotLogC.Checked = Convert.ToBoolean(result.Rows[9][2]);
    //        chkBotLogE.Checked = Convert.ToBoolean(result.Rows[9][3]);
    //        chkBotLogD.Checked = Convert.ToBoolean(result.Rows[9][4]);

    //        chkAuditTrailR.Checked = Convert.ToBoolean(result.Rows[10][1]);
    //        chkAuditTrailC.Checked = Convert.ToBoolean(result.Rows[10][2]);
    //        chkAuditTrailE.Checked = Convert.ToBoolean(result.Rows[10][3]);
    //        chkAuditTrailD.Checked = Convert.ToBoolean(result.Rows[10][4]);


    //        chkScheduleDetailsR.Checked = Convert.ToBoolean(result.Rows[11][1]);
    //        ChkScheduleDetailsC.Checked = Convert.ToBoolean(result.Rows[11][2]);
    //        ChkScheduleDetailsE.Checked = Convert.ToBoolean(result.Rows[11][3]);
    //        ChkScheduleDetailsD.Checked = Convert.ToBoolean(result.Rows[11][4]);


    //        chkConfigurationR.Checked = Convert.ToBoolean(result.Rows[12][1]);
    //        chkConfigurationC.Checked = Convert.ToBoolean(result.Rows[12][2]);
    //        chkConfigurationE.Checked = Convert.ToBoolean(result.Rows[12][3]);
    //        chkConfigurationD.Checked = Convert.ToBoolean(result.Rows[12][4]);

    //        chkAddUpdateProcessR.Checked = Convert.ToBoolean(result.Rows[13][1]);
    //        chkAddUpdateProcessC.Checked = Convert.ToBoolean(result.Rows[13][2]);
    //        chkAddUpdateProcessE.Checked = Convert.ToBoolean(result.Rows[13][3]);
    //        chkAddUpdateProcessD.Checked = Convert.ToBoolean(result.Rows[13][4]);

    //        chkUploadProcessR.Checked = Convert.ToBoolean(result.Rows[14][1]);
    //        chkUploadProcessC.Checked = Convert.ToBoolean(result.Rows[14][2]);
    //        chkUploadProcessE.Checked = Convert.ToBoolean(result.Rows[14][3]);
    //        chkUploadProcessD.Checked = Convert.ToBoolean(result.Rows[14][4]);


    //        chkPromoteDemoteR.Checked = Convert.ToBoolean(result.Rows[15][1]);
    //        chkPromoteDemoteC.Checked = Convert.ToBoolean(result.Rows[15][2]);
    //        chkPromoteDemoteE.Checked = Convert.ToBoolean(result.Rows[15][3]);
    //        chkPromoteDemoteD.Checked = Convert.ToBoolean(result.Rows[15][4]);

    //        if ((string)Session["UserRole"] == "Admin")
    //        {
    //            chkBotDashboardR.Enabled = false;
    //            chkBotDashboardC.Enabled = false;
    //            chkBotDashboardE.Enabled = false;
    //            chkBotDashboardD.Enabled = false;

    //            chkQueueR.Enabled = false;
    //            chkQueueC.Enabled = false;
    //            chkQueueE.Enabled = false;
    //            chkQueueD.Enabled = false;


    //            chkAddScheduleR.Enabled = false;
    //            ChkAddScheduleC.Enabled = false;
    //            chkAddScheduleE.Enabled = false;
    //            chkAddScheduleD.Enabled = false;

    //            chkAddUserR.Enabled = false;
    //            chkAddUserC.Enabled = false;
    //            chkAddUserE.Enabled = false;
    //            chkAddUserD.Enabled = false;

    //            chkAddRobotR.Enabled = false;
    //            chkAddRobotC.Enabled = false;
    //            chkAddRobotE.Enabled = false;
    //            chkAddRobotD.Enabled = false;

    //            chkAddQueueR.Enabled = false;
    //            chkAddQueueC.Enabled = false;
    //            chkAddQueueE.Enabled = false;
    //            chkAddQueueD.Enabled = false;

    //            chkAddGroupR.Enabled = false;
    //            chkAddGroupC.Enabled = false;
    //            chkAddGroupE.Enabled = false;
    //            chkAddGroupD.Enabled = false;


    //            chkAssignQueueBotR.Enabled = false;
    //            chkAssignQueueBotC.Enabled = false;
    //            chkAssignQueueBotE.Enabled = false;
    //            chkAssignQueueBotD.Enabled = false;

    //            chkAssignBotUserR.Enabled = false;
    //            chkAssignBotUserC.Enabled = false;
    //            chkAssignBotUserE.Enabled = false;
    //            chkAssignBotUserD.Enabled = false;


    //            chkBotLogR.Enabled = false;
    //            chkBotLogC.Enabled = false;
    //            chkBotLogE.Enabled = false;
    //            chkBotLogD.Enabled = false;

    //            chkAuditTrailR.Enabled = false;
    //            chkAuditTrailC.Enabled = false;
    //            chkAuditTrailE.Enabled = false;
    //            chkAuditTrailD.Enabled = false;


    //            chkScheduleDetailsR.Enabled = false;
    //            ChkScheduleDetailsC.Enabled = false;
    //            ChkScheduleDetailsE.Enabled = false;
    //            ChkScheduleDetailsD.Enabled = false;


    //            chkConfigurationR.Enabled = false;
    //            chkConfigurationC.Enabled = false;
    //            chkConfigurationE.Enabled = false;
    //            chkConfigurationD.Enabled = false;

    //            chkAddUpdateProcessR.Enabled = false;
    //            chkAddUpdateProcessC.Enabled = false;
    //            chkAddUpdateProcessE.Enabled = false;
    //            chkAddUpdateProcessD.Enabled = false;

    //            chkUploadProcessR.Enabled = false;
    //            chkUploadProcessC.Enabled = false;
    //            chkUploadProcessE.Enabled = false;
    //            chkUploadProcessD.Enabled = false;


    //            chkPromoteDemoteR.Enabled = false;
    //            chkPromoteDemoteC.Enabled = false;
    //            chkPromoteDemoteE.Enabled = false;
    //            chkPromoteDemoteD.Enabled = false;
    //        }

    //    }


    //}
    #endregion

    protected void BindPopupData(int groupid, int tenantid)
    {
        //ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetUserBaseAccess(groupid, tenantid);
        if (result != null && result.Rows.Count > 0 && result.Rows[0][1] != DBNull.Value)
        {
            chkBotDashboardR.Checked = Convert.ToBoolean(result.Rows[0][1]);
            chkBotDashboardC.Checked = Convert.ToBoolean(result.Rows[0][2]);
            chkBotDashboardE.Checked = Convert.ToBoolean(result.Rows[0][3]);
            chkBotDashboardD.Checked = Convert.ToBoolean(result.Rows[0][1]);

            chkQueueR.Checked = Convert.ToBoolean(result.Rows[1][1]);
            chkQueueC.Checked = Convert.ToBoolean(result.Rows[1][2]);
            chkQueueE.Checked = Convert.ToBoolean(result.Rows[1][3]);
            chkQueueD.Checked = Convert.ToBoolean(result.Rows[1][4]);


            chkAddScheduleR.Checked = Convert.ToBoolean(result.Rows[2][1]);
            ChkAddScheduleC.Checked = Convert.ToBoolean(result.Rows[2][2]);
            chkAddScheduleE.Checked = Convert.ToBoolean(result.Rows[2][3]);
            chkAddScheduleD.Checked = Convert.ToBoolean(result.Rows[2][4]);

            chkAddUserR.Checked = Convert.ToBoolean(result.Rows[3][1]);
            chkAddUserC.Checked = Convert.ToBoolean(result.Rows[3][2]);
            chkAddUserE.Checked = Convert.ToBoolean(result.Rows[3][3]);
            chkAddUserD.Checked = Convert.ToBoolean(result.Rows[3][4]);

            chkAddRobotR.Checked = Convert.ToBoolean(result.Rows[4][1]);
            chkAddRobotC.Checked = Convert.ToBoolean(result.Rows[4][2]);
            chkAddRobotE.Checked = Convert.ToBoolean(result.Rows[4][3]);
            chkAddRobotD.Checked = Convert.ToBoolean(result.Rows[4][4]);

            chkQueueManagementR.Checked = Convert.ToBoolean(result.Rows[5][1]);
            chkQueueManagementC.Checked = Convert.ToBoolean(result.Rows[5][2]);
            chkQueueManagementE.Checked = Convert.ToBoolean(result.Rows[5][3]);
            chkQueueManagementD.Checked = Convert.ToBoolean(result.Rows[5][4]);

            //.Checked = Convert.ToBoolean(result.Rows[6][1]);
            //chkAddGroupC.Checked = Convert.ToBoolean(result.Rows[6][2]);
            //chkAddGroupE.Checked = Convert.ToBoolean(result.Rows[6][3]);
            //chkAddGroupD.Checked = Convert.ToBoolean(result.Rows[6][4]);


            chkAssignQueueBotR.Checked = Convert.ToBoolean(result.Rows[6][1]);
            chkAssignQueueBotC.Checked = Convert.ToBoolean(result.Rows[6][2]);
            chkAssignQueueBotE.Checked = Convert.ToBoolean(result.Rows[6][3]);
            chkAssignQueueBotD.Checked = Convert.ToBoolean(result.Rows[6][4]);

            chkAssignBotUserR.Checked = Convert.ToBoolean(result.Rows[7][1]);
            chkAssignBotUserC.Checked = Convert.ToBoolean(result.Rows[7][2]);
            chkAssignBotUserE.Checked = Convert.ToBoolean(result.Rows[7][3]);
            chkAssignBotUserD.Checked = Convert.ToBoolean(result.Rows[7][4]);


            chkBotLogR.Checked = Convert.ToBoolean(result.Rows[8][1]);
            chkBotLogC.Checked = Convert.ToBoolean(result.Rows[8][2]);
            chkBotLogE.Checked = Convert.ToBoolean(result.Rows[8][3]);
            chkBotLogD.Checked = Convert.ToBoolean(result.Rows[8][4]);

            chkAuditTrailR.Checked = Convert.ToBoolean(result.Rows[9][1]);
            chkAuditTrailC.Checked = Convert.ToBoolean(result.Rows[9][2]);
            chkAuditTrailE.Checked = Convert.ToBoolean(result.Rows[9][3]);
            chkAuditTrailD.Checked = Convert.ToBoolean(result.Rows[9][4]);


            chkScheduleDetailsR.Checked = Convert.ToBoolean(result.Rows[10][1]);
            ChkScheduleDetailsC.Checked = Convert.ToBoolean(result.Rows[10][2]);
            ChkScheduleDetailsE.Checked = Convert.ToBoolean(result.Rows[10][3]);
            ChkScheduleDetailsD.Checked = Convert.ToBoolean(result.Rows[10][4]);


            chkConfigurationR.Checked = Convert.ToBoolean(result.Rows[11][1]);
            chkConfigurationC.Checked = Convert.ToBoolean(result.Rows[11][2]);
            chkConfigurationE.Checked = Convert.ToBoolean(result.Rows[11][3]);
            chkConfigurationD.Checked = Convert.ToBoolean(result.Rows[11][4]);

            chkProcessManagementR.Checked = Convert.ToBoolean(result.Rows[12][1]);
            chkProcessManagementC.Checked = Convert.ToBoolean(result.Rows[12][2]);
            chkProcessManagementE.Checked = Convert.ToBoolean(result.Rows[12][3]);
            chkProcessManagementD.Checked = Convert.ToBoolean(result.Rows[12][4]);

            chkDetailLogR.Checked = Convert.ToBoolean(result.Rows[13][1]);
            chkDetailLogC.Checked = Convert.ToBoolean(result.Rows[13][2]);
            chkDetailLogE.Checked = Convert.ToBoolean(result.Rows[13][3]);
            chkDetailLogD.Checked = Convert.ToBoolean(result.Rows[13][4]);


            chkPromoteDemoteR.Checked = Convert.ToBoolean(result.Rows[14][1]);
            chkPromoteDemoteC.Checked = Convert.ToBoolean(result.Rows[14][2]);
            chkPromoteDemoteE.Checked = Convert.ToBoolean(result.Rows[14][3]);
            chkPromoteDemoteD.Checked = Convert.ToBoolean(result.Rows[14][4]);

            //chkBotDashboardR.Enabled = false;
            string role = (string)Session["Role"];

            if (!((role == "Admin") || (role == "SuperAdmin")  ))
            {
                chkBotDashboardR.Enabled = false;
                chkBotDashboardC.Enabled = false;
                chkBotDashboardE.Enabled = false;
                chkBotDashboardD.Enabled = false;

                chkQueueR.Enabled = false;
                chkQueueC.Enabled = false;
                chkQueueE.Enabled = false;
                chkQueueD.Enabled = false;


                chkAddScheduleR.Enabled = false;
                ChkAddScheduleC.Enabled = false;
                chkAddScheduleE.Enabled = false;
                chkAddScheduleD.Enabled = false;

                chkAddUserR.Enabled = false;
                chkAddUserC.Enabled = false;
                chkAddUserE.Enabled = false;
                chkAddUserD.Enabled = false;

                chkAddRobotR.Enabled = false;
                chkAddRobotC.Enabled = false;
                chkAddRobotE.Enabled = false;
                chkAddRobotD.Enabled = false;

                chkQueueManagementR.Enabled = false;
                chkQueueManagementC.Enabled = false;
                chkQueueManagementE.Enabled = false;
                chkQueueManagementD.Enabled = false;

                //.Enabled = false;
                //chkAddGroupC.Enabled = false;
                //chkAddGroupE.Enabled = false;
                //chkAddGroupD.Enabled = false;


                chkAssignQueueBotR.Enabled = false;
                chkAssignQueueBotC.Enabled = false;
                chkAssignQueueBotE.Enabled = false;
                chkAssignQueueBotD.Enabled = false;

                chkAssignBotUserR.Enabled = false;
                chkAssignBotUserC.Enabled = false;
                chkAssignBotUserE.Enabled = false;
                chkAssignBotUserD.Enabled = false;


                chkBotLogR.Enabled = false;
                chkBotLogC.Enabled = false;
                chkBotLogE.Enabled = false;
                chkBotLogD.Enabled = false;

                chkAuditTrailR.Enabled = false;
                chkAuditTrailC.Enabled = false;
                chkAuditTrailE.Enabled = false;
                chkAuditTrailD.Enabled = false;


                chkScheduleDetailsR.Enabled = false;
                ChkScheduleDetailsC.Enabled = false;
                ChkScheduleDetailsE.Enabled = false;
                ChkScheduleDetailsD.Enabled = false;


                chkConfigurationR.Enabled = false;
                chkConfigurationC.Enabled = false;
                chkConfigurationE.Enabled = false;
                chkConfigurationD.Enabled = false;

                chkProcessManagementR.Enabled = false;
                chkProcessManagementC.Enabled = false;
                chkProcessManagementE.Enabled = false;
                chkProcessManagementD.Enabled = false;

                chkDetailLogR.Enabled = false;
                chkDetailLogC.Enabled = false;
                chkDetailLogE.Enabled = false;
                chkDetailLogD.Enabled = false;


                chkPromoteDemoteR.Enabled = false;
                chkPromoteDemoteC.Enabled = false;
                chkPromoteDemoteE.Enabled = false;
                chkPromoteDemoteD.Enabled = false;

                chkReportsR.Enabled = false;
                chkReportsC.Enabled = false;
                chkReportsE.Enabled = false;
                chkReportsD.Enabled = false;
            }

        }


    }

    protected void btnSaveAccess_Command(object sender, CommandEventArgs e)
    {
        int groupid = Convert.ToInt32(_HGroupid.Value);
        int tenantid = Convert.ToInt32(_HTenantID.Value);
        string username = _HUserName.Value;
        int result = 0;
        // string role = _Role.Value;

        #region CheckBoxes

        bool bBotDashboardR = chkBotDashboardR.Checked;
        bool bBotDashboardC = chkBotDashboardC.Checked;
        bool bBotDashboardE = chkBotDashboardE.Checked;
        bool bBotDashboardD = chkBotDashboardD.Checked;





        bool bqueueR = chkQueueR.Checked;
        bool bqueueC = chkQueueC.Checked;
        bool bqueueE = chkQueueE.Checked;
        bool bqueueD = chkQueueD.Checked;


        bool bAddScheduleR = chkAddScheduleR.Checked;
        bool bAddScheduleC = ChkAddScheduleC.Checked;
        bool bAddScheduleE = chkAddScheduleE.Checked;
        bool bAddScheduleD = chkAddScheduleD.Checked;

        bool bAddUserR = chkAddUserR.Checked;
        bool bAddUserC = chkAddUserC.Checked;
        bool bAddUserE = chkAddUserE.Checked;
        bool bAddUserD = chkAddUserD.Checked;

        bool bAddRobotR = chkAddRobotR.Checked;
        bool bAddRobotC = chkAddRobotC.Checked;
        bool bAddRobotE = chkAddRobotE.Checked;
        bool bAddRobotD = chkAddRobotD.Checked;

        bool bQueueManagementR = chkQueueManagementR.Checked; //AddQueue bAddQueueR
        bool bQueueManagementC = chkQueueManagementC.Checked;
        bool bQueueManagementE = chkQueueManagementE.Checked;
        bool bQueueManagementD = chkQueueManagementD.Checked;

        //bool bAddGroupR = chkAddGroupR.Checked; //remove this
        //bool bAddGroupC = chkAddGroupC.Checked;
        //bool bAddGroupE = chkAddGroupE.Checked;
        //bool bAddGroupD = chkAddGroupD.Checked;


        bool bAssignQueueBotR = chkAssignQueueBotR.Checked;
        bool bAssignQueueBotC = chkAssignQueueBotC.Checked;
        bool bAssignQueueBotE = chkAssignQueueBotE.Checked;
        bool bAssignQueueBotD = chkAssignQueueBotD.Checked;

        bool bAssignBotUserR = chkAssignBotUserR.Checked;
        bool bAssignBotUserC = chkAssignBotUserC.Checked;
        bool bAssignBotUserE = chkAssignBotUserE.Checked;
        bool bAssignBotUserD = chkAssignBotUserD.Checked;


        bool bBotLogR = chkBotLogR.Checked;
        bool bBotLogC = chkBotLogC.Checked;
        bool bBotLogE = chkBotLogE.Checked;
        bool bBotLogD = chkBotLogD.Checked;

        bool bAuditTrailR = chkAuditTrailR.Checked;
        bool bAuditTrailC = chkAuditTrailC.Checked;
        bool bAuditTrailE = chkAuditTrailE.Checked;
        bool bAuditTrailD = chkAuditTrailD.Checked;


        bool bScheduleDetailsR = chkScheduleDetailsR.Checked;
        bool bScheduleDetailsC = ChkScheduleDetailsC.Checked;
        bool bScheduleDetailsE = ChkScheduleDetailsE.Checked;
        bool bScheduleDetailsD = ChkScheduleDetailsD.Checked;


        bool bConfigurationR = chkConfigurationR.Checked;
        bool bConfigurationC = chkConfigurationC.Checked;
        bool bConfigurationE = chkConfigurationE.Checked;
        bool bConfigurationD = chkConfigurationD.Checked;


        bool bProcessManagementR = chkProcessManagementR.Checked; //process management
        bool bProcessManagementC = chkProcessManagementC.Checked;
        bool bProcessManagementE = chkProcessManagementE.Checked;
        bool bProcessManagementD = chkProcessManagementD.Checked;

        bool bDetailLogR = chkDetailLogR.Checked; // Details Log
        bool bDetailLogC = chkDetailLogC.Checked;
        bool bDetailLogE = chkDetailLogE.Checked;
        bool bDetailLogD = chkDetailLogD.Checked;


        bool bPromoteDemoteR = chkPromoteDemoteR.Checked;
        bool bPromoteDemoteC = chkPromoteDemoteC.Checked;
        bool bPromoteDemoteE = chkPromoteDemoteE.Checked;
        bool bPromoteDemoteD = chkPromoteDemoteD.Checked;

        #endregion

        ServiceInterface serviceInterface = new ServiceInterface();
        result = serviceInterface.SaveRoleBaseAccessUser(groupid, username, tenantid, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bQueueManagementR, bQueueManagementC, bQueueManagementE, bQueueManagementD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bProcessManagementR, bProcessManagementC, bProcessManagementE, bProcessManagementD, bDetailLogR, bDetailLogC, bDetailLogE, bDetailLogD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);


        if (result > 0)
        {
            serviceInterface.insertLog("Role Based Access Updated Succesfully", "Role Based Access : Role Based Access Updated successfully for User " + username + " and Group ID : " + groupid + " and Tenant ID :" + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitACCESSUPDATEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAccesshasbeenUpdatedforUser") + " " + username + " " + (String)GetGlobalResourceObject("content", "LitRSuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
        }
        else
        {
            serviceInterface.insertLog("Role Based Access Update Failed", "Role Based Access : Role Based Access Update failed for User " + username + " with Group ID : " + groupid + " and Tenant ID :" + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOUPDATEACCESS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitAccessupdationFailedforUser") + "" + username + " ', showConfirmButton: false,  timer: 1500});", true);
        }
    }


    protected void ImageButton1_Command(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        int groupid = Convert.ToInt32(commandArgs[0]);
        int tenantid = Convert.ToInt32(commandArgs[1]);
        string username = (commandArgs[2].ToString());

        _HGroupid.Value = groupid.ToString();
        _HTenantID.Value = tenantid.ToString();
        _HUserName.Value = username.ToString();
        BindPopupData(groupid, tenantid);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }

    protected void btnSave(object sender, EventArgs e)
    {
        if (DrpRoleType.SelectedValue != "--Select--")
        {
            string strDomainName = txtDomain.Text.Trim();
            string strUserName = txtUser.Text.Trim();
            string strPwd = txtPwd.Text.Trim();
            strPwd = encryptpass(strPwd);
            string strUserRole = string.Empty;

            if (DrpRoleType.SelectedValue != null)
            {
                strUserRole = DrpRoleType.SelectedValue;
            }

            if ((strUserName == "") || (strPwd == "") || DrpRoleType.SelectedValue == "--Select--" || DrpGroupName.SelectedValue == "--Select--")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            }
            else
            {
                string strcurrentuser = (string)Session["UserName"];
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];
                string roletype = DrpRoleType.SelectedValue;
                string groupname = DrpGroupName.SelectedValue;



                int result = serviceInterface.Adduser(strDomainName, strUserName, strPwd, groupid, groupname, tenantid, strUserRole, strcurrentuser);
                if (result > 0)
                {
                    //" + (String)GetGlobalResourceObject("content", "LitUserAName") + " " + strUserName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitUSERADDEDSUCCESSFULLY") + "',text:' " + strUserName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                    serviceInterface.insertLog("Add User Successful", "Add User : User with Username : " + strUserName + " Domain " + strDomainName + "  and User Role : " + strUserRole + " has been added successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    getdata(groupid, tenantid);
                }
                else
                {
                    
                  
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOADDUSER") + "',text:' " + strUserName + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + " ', showConfirmButton: false,  timer: 1500});", true);
                    serviceInterface.insertLog("Add User Failed", "Add User : Addition of User with Username : " + strUserName + " Domain " + strDomainName + " and User Role : " + strUserRole + " by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                  
                    getdata(groupid, tenantid);
                }
            }
            clear();
        }

        else {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
        }
    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {
      
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }

    /*Added By Piyush For testing Password Encryption*/
    public string encryptpass(string password)
    {
        string encodePwd = string.Empty;
        // byte[] encode = new byte[password.Length];
        //encode = Encoding.UTF8.GetBytes(password);
        //encodePwd = Convert.ToBase64String(encode);

        // CommonLibrary.EncryptionHelper 

        string encrKey = serviceInterface.getKey();
        encodePwd= encryptionHelper.Encrypt(password,encrKey);

        return encodePwd;
    }
    public void getdata(int groupid, int tenantid)
    {
        result.Clear();
        result = serviceInterface.GetUsers(groupid, tenantid);

        if (result != null)
        {
            //GVRoles.DataSource = null;
            //GVRoles.DataBind();

            GVRoles.DataSource = result;
            GVRoles.DataBind();
        }
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
            int result = serviceInterface.DeleteUser(strid, groupid, tenantid, strcurrentuser);
            if (0 < result)
            {
                serviceInterface.insertLog("Delete User Successful", "Delete User : User with User ID : " + strid + "  has been delted successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getdata(groupid, tenantid);
            }
            else
            {
                serviceInterface.insertLog("Delete User Failed", "Delete User : Deletion of User with User ID : " + strid + " by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
    }

    protected void refreshUsers(object sender, CommandEventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        getdata(groupid, tenantid);
    }
    protected void clear()
    {
        txtUser.Text = null;
        //txtDomain.Text = Session["DomainName"].ToString();
        /*To clear User password text box*/
        txtPwd.Text = null;

        /*To clear User domain text box*/
        //txtDomain.Text = null;

        /*if "Admin" is not selected in Drop down list then move to below loop*/
        if (DrpRoleType.SelectedValue != "--Select--")
        {
            /* "Admin" is a bydafault value assign to drop down list*/
            DrpRoleType.SelectedValue = "--Select--";
            //DrpRoleType.SelectedIndex = -1;
        }
    }


    protected void BtnCancelUser(object sender, EventArgs e)
    {
        /*To clear User name text box*/
        clear();
    }

    //Delete Process
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
                string userId = commandArgs[1];

                lblId.Text = id;
                lblUserId.Text = userId;

                lblIdSecondPopUp.Text = id;
                lblUserIdSecondPopUp.Text = userId;

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
            
        }

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
            string id = lblId.Text;
            string UserId = lblUserId.Text;
            string strcurrentuser = (string)Session["UserName"];
            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteUser(id, groupid, tenantid, strcurrentuser);
            if (result > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitUSERDELETEDSUCCESSFULLY") + "',text:' " + (String)GetGlobalResourceObject("content", "LitUser") + " " + strcurrentuser + " " + (String)GetGlobalResourceObject("content", "LitwithID") + " " + UserId + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete User Successful", "Delete User : User with User ID : " + id + "  has been delted successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getdata(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETEUSER") + "',text:'" + (String)GetGlobalResourceObject("content", "LitUser") + "  " + strcurrentuser + " " + (String)GetGlobalResourceObject("content", "LitwithID") + " " + UserId + " " + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + " ', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Delete User Failed", "Delete User : Deletion of User with User ID : " + id + " by User : " + strcurrentuser + " with Group ID : " + groupid + " and Tenant ID : " + tenantid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getdata(groupid, tenantid);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
        }
        catch (Exception exception)
        {

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

    protected void imgChangeGroup_Command(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        string username = commandArgs[0].ToString();
        string roletype = commandArgs[1].ToString();
        string domainname = commandArgs[2].ToString();
        txtCGUserID.Text = username;
        txtCGCurrentRole.Text = roletype;
        txtDomainname.Text = domainname;
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];

        //result.Clear();
        result = serviceInterface.GetGroups(groupid, tenantid);

        //deleteing the current roles
        //var query = result.AsEnumerable().Where(x => x.Field<string>("groupname").ToLower() == roletype.ToLower());
        //foreach (var row in query)
        //{
        //    row.Delete();
        //}


        if (result != null)
        {
            for (int i = result.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = result.Rows[i];
                if (dr["groupname"].ToString() == roletype)
                    dr.Delete();
            }
            result.AcceptChanges();

            ddlAssignRole.Items.Clear();
            ddlAssignRole.DataSource = result;
            ddlAssignRole.DataValueField = "groupname";
            ddlAssignRole.DataTextField = "groupname";
            ddlAssignRole.DataBind();
        }
        txtCGUserID.ReadOnly = true;
        txtCGCurrentRole.ReadOnly = true;
        txtDomainname.ReadOnly = true;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalGroupChange();", true);


    }

    protected void btnSaveGroupChanges_Command(object sender, CommandEventArgs e)
    {
        int tenantID = (int)Session["TenantId"];
        string currentRoleCG = txtCGCurrentRole.Text;
        string username = txtCGUserID.Text;
        string AssignedRole = ddlAssignRole.SelectedValue.ToString();
        int result = 0;
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];



        //stored procedure is complete for the This method.
        //  check for any dependencies where the changing group matters -- since you are changin in user table aslo so find relation from there
        // test the method 
        try
        {
            result = serviceInterface.ChangeGroup(currentRoleCG, username, AssignedRole, tenantID);
            if (result > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitROLEUPDATEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitUser") + "  " + username + " " + (String)GetGlobalResourceObject("content", "LitwithRole") + " " + AssignedRole + " " + (String)GetGlobalResourceObject("content", "Lithasbeenupdatesuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);              
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                getdata(groupid, tenantid);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitROLENOTUPDATEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitUser") + "  " + username + " " + (String)GetGlobalResourceObject("content", "LitwithRole") + " " + AssignedRole + " " + (String)GetGlobalResourceObject("content", "Lithasnotupdatesuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.StackTrace);
        }


        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModalGroupChange();", true);
    }



    protected void btnClear_click(object sender, EventArgs e)
    {
        ddlAssignRole.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalGroupChange();", true);
    }
}