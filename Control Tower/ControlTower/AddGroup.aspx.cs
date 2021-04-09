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

#region Headers
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class DemoMasterPage2_AddGroup : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
                else
                {
                    int groupid = (int)Session["GroupId"];
                    int tenantid = (int)Session["TenantId"];
                    getdata(groupid, tenantid);
                }
            }
            else if (IsPostBack)
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
            }
            else
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Control_Tower_AddGroup issue : While  Page_Load ", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));           
        }

    }

    protected override void InitializeCulture()
    {
        UICulture = Request.UserLanguages[0];
        base.InitializeCulture();
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
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
        catch(Exception ex)
        {
            serviceInterface.insertLog("Control_Tower_AddGroup issue : While  Page_PreInit ", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));                  //#
        }
    }
    protected void ImageButton1_Command(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        int groupid = Convert.ToInt32(commandArgs[0]);
        int tenantid = Convert.ToInt32(commandArgs[1]);
        string role = commandArgs[2].ToString().ToLower();
        _HGroupid.Value = groupid.ToString();
        _HTenantID.Value = tenantid.ToString();
        _Role.Value = role.ToString();
        BindPopupData(groupid, tenantid, role);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }

    #region protected void BindPopupData(int groupid, int tenantid, string role) Commented by Piyush on 26-Nov-2019
    //protected void BindPopupData(int groupid, int tenantid, string role)
    //{


    //    ServiceInterface serviceInterface = new ServiceInterface();
    //    DataTable result = serviceInterface.GetRoleBaseAccess(groupid, tenantid);

    //    if (result != null && result.Rows.Count > 0 && result.Rows[0][1] != DBNull.Value)
    //    {

    //        //check for the pages
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
    //    }

    //    if (Session["Role"].ToString().ToLower() != "admin")
    //    {
    //        chkBotDashboardR.Enabled = false;
    //        chkBotDashboardC.Enabled = false;
    //        chkBotDashboardE.Enabled = false;
    //        chkBotDashboardD.Enabled = false;



    //        chkQueueR.Enabled = false;
    //        chkQueueC.Enabled = false;
    //        chkQueueE.Enabled = false;
    //        chkQueueD.Enabled = false;




    //        chkAddScheduleR.Enabled = false;
    //        ChkAddScheduleC.Enabled = false;
    //        chkAddScheduleE.Enabled = false;
    //        chkAddScheduleD.Enabled = false;



    //        chkAddUserR.Enabled = false;
    //        chkAddUserC.Enabled = false;
    //        chkAddUserE.Enabled = false;
    //        chkAddUserD.Enabled = false;



    //        chkAddRobotR.Enabled = false;
    //        chkAddRobotC.Enabled = false;
    //        chkAddRobotE.Enabled = false;
    //        chkAddRobotD.Enabled = false;



    //        chkAddQueueR.Enabled = false;
    //        chkAddQueueC.Enabled = false;
    //        chkAddQueueE.Enabled = false;
    //        chkAddQueueD.Enabled = false;



    //        chkAddGroupR.Enabled = false;
    //        chkAddGroupC.Enabled = false;
    //        chkAddGroupE.Enabled = false;
    //        chkAddGroupD.Enabled = false;




    //        chkAssignQueueBotR.Enabled = false;
    //        chkAssignQueueBotC.Enabled = false;
    //        chkAssignQueueBotE.Enabled = false;
    //        chkAssignQueueBotD.Enabled = false;



    //        chkAssignBotUserR.Enabled = false;
    //        chkAssignBotUserC.Enabled = false;
    //        chkAssignBotUserE.Enabled = false;
    //        chkAssignBotUserD.Enabled = false;




    //        chkBotLogR.Enabled = false;
    //        chkBotLogC.Enabled = false;
    //        chkBotLogE.Enabled = false;
    //        chkBotLogD.Enabled = false;



    //        chkAuditTrailR.Enabled = false;
    //        chkAuditTrailC.Enabled = false;
    //        chkAuditTrailE.Enabled = false;
    //        chkAuditTrailD.Enabled = false;




    //        chkScheduleDetailsR.Enabled = false;
    //        ChkScheduleDetailsC.Enabled = false;
    //        ChkScheduleDetailsE.Enabled = false;
    //        ChkScheduleDetailsD.Enabled = false;




    //        chkConfigurationR.Enabled = false;
    //        chkConfigurationC.Enabled = false;
    //        chkConfigurationE.Enabled = false;
    //        chkConfigurationD.Enabled = false;



    //        chkAddUpdateProcessR.Enabled = false;
    //        chkAddUpdateProcessC.Enabled = false;
    //        chkAddUpdateProcessE.Enabled = false;
    //        chkAddUpdateProcessD.Enabled = false;



    //        chkUploadProcessR.Enabled = false;
    //        chkUploadProcessC.Enabled = false;
    //        chkUploadProcessE.Enabled = false;
    //        chkUploadProcessD.Enabled = false;




    //        chkPromoteDemoteR.Enabled = false;
    //        chkPromoteDemoteC.Enabled = false;
    //        chkPromoteDemoteE.Enabled = false;
    //        chkPromoteDemoteD.Enabled = false;
    //    }

    //    #region MyRegion
    //    //chkAddGroupR.Checked = true;
    //    //chkAddQueueR.Checked = true;

    //    //chkBotDashboardR.Checked = true;

    //    //chkQueueR.Checked = true;
    //    //chkAddScheduleR.Checked = true;
    //    //chkAddUserR.Checked = true;
    //    //chkAddRobotR.Checked = true;
    //    //chkAssignQueueBotR.Checked = true;
    //    //chkAssignBotUserR.Checked = true;
    //    //chkBotLogR.Checked = true;
    //    //chkAuditTrailR.Checked = true;
    //    //chkScheduleDetailsR.Checked = true;
    //    //chkConfigurationR.Checked = true;
    //    //chkAddUpdateProcessR.Checked = true;
    //    //chkUploadProcessR.Checked = true;
    //    //chkPromoteDemoteR.Checked = true;
    //    #endregion


    //}

    #endregion

    protected void BindPopupData(int groupid, int tenantid, string role)
    {
        DataTable result = serviceInterface.GetRoleBaseAccess(groupid, tenantid);
        if (result != null && result.Rows.Count > 0 && result.Rows[0][1] != DBNull.Value)
        {
            //check for the pages
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
        }
                
        if (Session["Role"].ToString().ToLower() != "admin")
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
        }
    }

    protected void BtnSaveGroup(object sender, EventArgs e)
    {
        string StrGroupName = txtGroup.Text.Trim();

        if ((StrGroupName == ""))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
        }
        else
        {
            string strcurrentuser = (string)Session["UserName"];
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            int result = serviceInterface.AddGroup(StrGroupName, groupid, tenantid, strcurrentuser);
            if (result > 0)
            {
                serviceInterface.insertLog("Group Addedd Successfully", "Group Name " + StrGroupName + " has been Added successfully!", groupid, tenantid);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitGROUPADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitGroupName") + " " + StrGroupName + " " + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
            }
            else
            {
                serviceInterface.insertLog("Failed Adding Group", "Group Name " + StrGroupName + " has not been Added!", groupid, tenantid);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDGROUPADDITION") + "',text:'" + (String)GetGlobalResourceObject("content", "LitGroupName") + " " + StrGroupName + " " + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + "', showConfirmButton: false,  timer: 1500});", true);
            }
            getdata(groupid, tenantid);
        }
    }

    public void getdata(int groupid, int tenantid)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetGroups(groupid, tenantid);

        if (result != null)
        {
            GVRoles.DataSource = result;
            GVRoles.DataBind();
        }
    }


    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        getdata(groupid, tenantid);

    }

    /*Method name : BtnCancelGroup
     Purpose     : Call when click on Cancel button to clear all entries filled in Text boxes
     Called from : UI  */
    protected void BtnCancelGroup(object sender, EventArgs e)
    {
        /*To clear User name text box*/
        txtGroup.Text = null;
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
                string groupName = commandArgs[1];
                string strcurrentuser = (string)Session["UserName"];

                lblId.Text = id;
                lblGroupName.Text = groupName;
                lblGroupIdSecondPopUp.Text = id;
                lblGroupNameSecondPopUp.Text = groupName;
                lblIdThirdPopUp.Text = id;
                lblGroupNameThirdPopUp.Text = groupName;

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);

            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('EXCEPTION WHILE DELETING GROUP', 'Please contact Admin for Deletion Exception', 'error').then((value) => {  window.location.href = 'AddGroup.aspx';}); ", true);
        }
    }

    protected void CommandBtnDisable_Click(object sender, EventArgs e)
    {

        int groupidToDisable = 0;
        int isActiveStatus = 0;
        groupidToDisable = Int32.Parse(lblGroupIdSecondPopUp.Text);
        string strGroupName = lblGroupName.Text;

        if (btnEnableDisable.Text == "Disable")
        {
           
            isActiveStatus = -1;
        }
        if (btnEnableDisable.Text == "Enable")
        {
            isActiveStatus = 0;
            
        }


        ServiceInterface serviceInterface = new ServiceInterface();
        int result =
        serviceInterface.UpdateIsactiveStatusGroupRelatedTables(groupidToDisable, isActiveStatus);
 
        if (result > 0 && btnEnableDisable.Text == "Enable")
        {
                btnEnableDisable.Text = "Disable";
                btnEnableDisable.Attributes["class"] = "btn btn-warning btn-block";           
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
           
        }

        else if (result > 0 && btnEnableDisable.Text == "Disable")
        {
            btnEnableDisable.Text = "Enable";
            btnEnableDisable.Attributes["class"] = "btn btn-success btn-block";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
           
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitGROUPENABLEDISABLEFAILED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitGroupName") + " " + strGroupName + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenenabledisablesuccessfully") + "  ', showConfirmButton: false,  timer: 1500});", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
        }

    }
    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteThirdPopUp();", true);
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        #region Declare Variables
        DataTable result = null;
        int resultStatus = 0;
        int groupid = Int32.Parse(lblId.Text);

        int cnt_Bot = 0;
        int cnt_User = 0;
        int cnt_Process = 0;
        int cnt_RQDetails = 0;
        int cnt_ProcessUpload = 0;
        int cnt_ScheduleDeatils = 0;
        int cnt_UserBotMapping = 0;
        int cnt_botdefaultqueue = 0;
        int cnt_configurationparameters = 0;
        int cnt_messagingdetails = 0;
        #endregion 

        ServiceInterface serviceInterface = new ServiceInterface();
        result = serviceInterface.GetCountToDeleteGroup(groupid);

        if (result != null)
        {
            if ((result.Rows[0][0] != null) && (result.Rows[0][0] != System.DBNull.Value))
                cnt_User = (int)result.Rows[0][0];

            if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
                cnt_Bot = (int)result.Rows[0][1];

            if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
                cnt_Process = (int)result.Rows[0][2];

            if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                cnt_ProcessUpload = (int)result.Rows[0][3];

            if ((result.Rows[0][4] != null) && (result.Rows[0][4] != System.DBNull.Value))
                cnt_RQDetails = (int)result.Rows[0][4];

            if ((result.Rows[0][5] != null) && (result.Rows[0][5] != System.DBNull.Value))
                cnt_ScheduleDeatils = (int)result.Rows[0][5];

            if ((result.Rows[0][6] != null) && (result.Rows[0][6] != System.DBNull.Value))
                cnt_UserBotMapping = (int)result.Rows[0][6];

            if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
                cnt_botdefaultqueue = (int)result.Rows[0][7];

            if ((result.Rows[0][9] != null) && (result.Rows[0][9] != System.DBNull.Value))
                cnt_messagingdetails = (int)result.Rows[0][9];

            if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
                cnt_configurationparameters = (int)result.Rows[0][10];

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

            resultStatus = serviceInterface.CheckIsactiveStatusGroup(groupid);
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

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + (String)GetGlobalResourceObject("content", "LitGROUPENABLEDISABLEFAILED") + "', '" + (String)GetGlobalResourceObject("content", "LitAGGroupId") + " " + groupid + "  " + (String)GetGlobalResourceObject("content", "Lithasnotbeenenabledisablesuccessfully") + "', 'error').then((value) => {  window.location.href = 'AddGroup.aspx';}); ", true);
        }
    }

    protected void ModalPopUpBtnClose_ClickPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
        //Response.Redirect(Request.RawUrl, false);
    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_ClickThirdPopUp(object sender, EventArgs e)
    {
        try
        {
            int groupid = 0;
            int tenantid = 0;
            int groupidtodelete = 0;

            if (Session["tenantid"] != null || Session["groupid"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                string strcurrentuser = (string)Session["UserName"];
                groupidtodelete = Int32.Parse(lblId.Text);
                string strGroupName = lblGroupName.Text;

                ServiceInterface serviceInterface = new ServiceInterface();
                int result = serviceInterface.DeleteGroupWithAllRelatedData(groupidtodelete, tenantid, strcurrentuser);
                if (result > 0)
                {
                    serviceInterface.insertLog("GROUP NAME DELETED SUCCESSFULLY!", "Group Name " + strGroupName + " deleted successfully!", groupid, tenantid);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    getdata(groupid,tenantid);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitGROUPNAMEDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitGroupName") + " " + strGroupName + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                }
                else
                {
                    serviceInterface.insertLog("FAILED TO DELETE GROUP NAME!", "Group Name " + strGroupName + " failed deleted successfully!", groupid, tenantid);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitGROUPNAMENOTDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitGroupName") + " " + strGroupName + "" + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                }
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('EXCEPTION WHILE DELETING GROUP', 'Deletion not Successful. Please contact admin for further details. ' , 'error'); ", true);
        }
    }


    protected void btnSaveAccess_Command(object sender, CommandEventArgs e)
    {
        int groupid = Convert.ToInt32(_HGroupid.Value);
        int tenantid = Convert.ToInt32(_HTenantID.Value);
        string role = _Role.Value;
        int result = 0;

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

        bool bQueueManagementR = chkQueueManagementR.Checked;
        bool bQueueManagementC = chkQueueManagementC.Checked;
        bool bQueueManagementE = chkQueueManagementE.Checked;
        bool bQueueManagementD = chkQueueManagementD.Checked;

        //bool bAddGroupR = chkAddGroupR.Checked;
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


        bool bProcessManagementR = chkProcessManagementR.Checked;
        bool bProcessManagementC = chkProcessManagementC.Checked;
        bool bProcessManagementE = chkProcessManagementE.Checked;
        bool bProcessManagementD = chkProcessManagementD.Checked;

        bool bDetailLogR = chkDetailLogR.Checked;
        bool bDetailLogC = chkDetailLogC.Checked;
        bool bDetailLogE = chkDetailLogE.Checked;
        bool bDetailLogD = chkDetailLogD.Checked;


        bool bPromoteDemoteR = chkPromoteDemoteR.Checked;
        bool bPromoteDemoteC = chkPromoteDemoteC.Checked;
        bool bPromoteDemoteE = chkPromoteDemoteE.Checked;
        bool bPromoteDemoteD = chkPromoteDemoteD.Checked;

        #endregion
        ServiceInterface serviceInterface = new ServiceInterface();
        result = serviceInterface.SaveRoleBaseAccess(groupid, tenantid, role, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bQueueManagementR, bQueueManagementC, bQueueManagementE, bQueueManagementD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bProcessManagementR, bProcessManagementC, bProcessManagementE, bProcessManagementD, bDetailLogR, bDetailLogC, bDetailLogE, bDetailLogD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);

        if (result > 0)
        {
            serviceInterface.insertLog("Access has been updated successfully!", "Access has been updated successfully!", groupid, tenantid);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitAccesshasbeenupdatedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
        }
        else
        {
            serviceInterface.insertLog("Access has been updated successfully!", "Access has been updated successfully!", groupid, tenantid);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitAccesshasnotbeenUpdatedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
        }
    }
}