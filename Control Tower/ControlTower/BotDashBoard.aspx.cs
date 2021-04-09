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


#region Header Declarations
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
#endregion

public partial class DemoMasterPage2_BotDashBoard : System.Web.UI.Page
{
    public ServiceInterface serviceInterface = new ServiceInterface();
    public Startup startup = new Startup();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
                {
                    serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Session Expired from BotDashBoard Page", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
                else
                {
                    int groupid = 0;
                    int tenantid = 0;
                    int roleid = 0;
                    string username = String.Empty;
                    DataTable db = null;
                    DataTable rqDetails = null;

                    cardProcess.Attributes.Add("onclick", "window.location='ProcessManagement.aspx'");
                    cardSuccess.Attributes.Add("onclick", "previousSchedule();");
                    cardFailed.Attributes.Add("onclick", " failedSchedule();");
                    cardSchedule.Attributes.Add("onclick", "scheduledSchedule();");

                    #region Single Login Visit Code
                    string strcurrentuser = (string)Session["UserName"];
                    if (Application["visits"] == null)
                    {
                        Application["visits"] = 1;
                        String Success =  (String)GetGlobalResourceObject("content", "LitWelcomeStart") + strcurrentuser + (String)GetGlobalResourceObject("content", "LitWelcomeEnd");
                        alertSuccessLogin.InnerHtml = Success;
                    }
                    else
                    {
                        int visits = (int)Application["visits"];
                        visits++;
                        Application["visits"] = visits;
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "hide_SuccessMsg();", true);
                    }
                    #endregion

                    username = (string)Session["UserName"];
                    groupid = Convert.ToInt32(Session["GroupId"]);
                    tenantid = Convert.ToInt32(Session["TenantId"]);
                    roleid = Convert.ToInt32(Session["roleid"]);

                    db = serviceInterface.GetPageAccess(roleid, groupid, tenantid, "BotDashboard");
                    bool playA = Convert.ToBoolean(db.Rows[0]["createA"]);

                    rqDetails = serviceInterface.GetRQDetailsForBotDashboard(groupid, tenantid);

                    HiddenField1.Value = Convert.ToString(rqDetails.Rows[0]["userid"]);
                    HiddenField2.Value = Convert.ToString(rqDetails.Rows[0]["pwd"]);
                    string stompport = Convert.ToString(rqDetails.Rows[0]["stompport"]);
                    HiddenField3.Value = ":" + stompport + "/stomp";

                    int[] robots = new int[5];
                    robots = serviceInterface.GetDashboardData(groupid, tenantid);

                    if (0 == robots[0] || 0 == robots[1] || 0 == robots[2] || 0 == robots[3])
                    {
                        if (0 != robots[0])
                            lblTotalProcesses.Text = robots[0].ToString();
                        else
                            lblTotalProcesses.Text = "0";

                        if (0 != robots[1])
                        {
                            lblSuccessfulProcess.Text = robots[1].ToString();
                            successprogressbar.Style.Add("width", 0 + "%");
                            double totalProcessCount = Convert.ToDouble(robots.GetValue(0));
                            double successProcessCount = Convert.ToDouble(robots.GetValue(1));

                            double SuccessPercent = ((successProcessCount / totalProcessCount) * 100);
                            successprogressbar.Style.Add("width", SuccessPercent + "%");
                            lblSuccessPercent.Text = Convert.ToInt32(SuccessPercent) + "%";
                        }
                        else
                        {
                            lblSuccessfulProcess.Text = "0";
                            successprogressbar.Style.Add("width", 0 + "%");
                            lblSuccessPercent.Text = "0 %";
                        }

                        if (0 != robots[2])
                        {
                            lblScheduleCount.Text = robots[2].ToString();
                            failedProgressBar.Style.Add("width", 0 + "%");
                        }
                        else {
                            lblScheduleCount.Text = "0";
                            failedProgressBar.Style.Add("width", 0 + "%");
                        }
                        if (0 != robots[3])
                            lblFailedPercentage.Text = robots[3].ToString();
                        else
                            lblFailedPercentage.Text = "0";

                        LoadBots();
                    }
                    else
                    {
                        lblTotalProcesses.Text = robots[0].ToString();
                        lblSuccessfulProcess.Text = robots[1].ToString();
                        lblScheduleCount.Text = robots[2].ToString();
                        lblFailedPercentage.Text = robots[3].ToString();

                        double totalProcessCount = Convert.ToDouble(robots.GetValue(0));
                        double successProcessCount = Convert.ToDouble(robots.GetValue(1));

                        double SuccessPercent = ((successProcessCount / totalProcessCount) * 100);
                        successprogressbar.Style.Add("width", SuccessPercent + "%");
                        lblSuccessPercent.Text = Convert.ToInt32(SuccessPercent) + "%";

                        successProcessCount = Convert.ToDouble(robots.GetValue(3));
                        double failedPercent = ((successProcessCount / totalProcessCount) * 100);
                        failedProgressBar.Style.Add("width", failedPercent + "%");
                        lblFailedPercentage.Text = Convert.ToInt32(failedPercent) + "%";

                        LoadBots();

                        cardProcess.Attributes.Add("onclick", "window.location='ScheduleDetails.aspx'");
                    }

                    /* Purpose : Role Base Access
                     *     How : Depending if user has the access or not 
                     *           it will disable the play button 
                     *           
                     *    Scenario 1 : Only considerd if user is able to play button or not
                     *                 so no need to  disable stop button 
                     * **/

                    cardProcess.Attributes.Add("onclick", "window.location='ScheduleDetails.aspx'");
                    LoadBots();

                    #region RoleBaseAccess

                    try
                    {
                        roleid = Convert.ToInt32(Session["roleid"]);
                        db = serviceInterface.GetPageAccess(roleid, groupid, tenantid, "BotDashboard");
                        bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                        bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                        bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                        bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                        if (!ViewAccess)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYoudonothaveaccesstothisPageYouaremakinganUnauthorizedAccess") + " ', showConfirmButton: false,  timer: 2500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                             //Will be decided from MasterPage.
                        }

                        if (!CreateAccess)
                        {
                            //No Create Access for BotDashBoard Page.
                        }

                        if (!EditAccess)
                        {
                            foreach (RepeaterItem item in rptBots.Items)
                            {
                                ImageButton play = item.FindControl("StartButton") as ImageButton;
                                ImageButton stop = item.FindControl("StopButton") as ImageButton;

                                stop.Enabled = false;
                                play.Attributes.CssStyle.Add("opacity","0.5");
                                play.Enabled = false;
                                play.ToolTip = "You dont have access to START or STOP the Process";
                            }
                        }

                        if (!DeleteAccess)
                        {
                            //No Delete Access for BotDashBoard Page.
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion

                    //cardProcess.Attributes.Add("onclick", "window.location='ScheduleDetails.aspx'");
                    //LoadBots();
                }
            }
            else if (IsPostBack)
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Session Expired from BotDashBoard Page", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
            }
            else
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Session Expired from BotDashBoard Page", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception: Session Expired from BotDashBoard Page","Exception: "+ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Session Expired from BotDashBoard Page", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            
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
    public void LoadBots()
    {
        int groupid = 0;
        int tenantid = 0;
        string userid = String.Empty;
        DataTable resultForDashBoardBots = null;

        try
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
            userid = (string)Session["UserName"];
           // userid = (string)Session["Role"];  //Changed to Role for DataBase.

            resultForDashBoardBots = serviceInterface.GetDashboardBots(groupid, tenantid, userid);

            if (null != resultForDashBoardBots)
            {
                rptBots.DataSource = resultForDashBoardBots;
                rptBots.DataBind();
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Error while Loading Bots in BotDashBoard","Exception :"+ex.Message, groupid, tenantid);
            serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Logged Out Forcefully from BotDashBoard", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            //Response.Redirect("Login.aspx");
        }
    }

    protected void LoadBotLogs(object sender, CommandEventArgs e)
    {
        ImageButton objImage = (ImageButton)sender;
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        string userid = (string)Session["UserName"];

        string[] commandArgs = objImage.CommandArgument.ToString().Split(new char[] { ',' });
        string strbotid = commandArgs[0];
        string strmachinename = commandArgs[1];

        try
        {
            DataTable resultForLogs = null;
            resultForLogs = serviceInterface.GetLogsForDashboardBots(strbotid, strmachinename,DateTime.Now.ToString(),DateTime.Now.ToString(), groupid, tenantid);

            if (null != resultForLogs)
            {
                Repeater2.DataSource = resultForLogs;
                Repeater2.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception: Load Bots on BotDashBoard Page", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    protected void StartProcess(object sender, CommandEventArgs e)
    {
        ImageButton objImage = (ImageButton)sender;

        string[] commandArgs = objImage.CommandArgument.ToString().Split(new char[] { ',' });
        string strbotid = commandArgs[0];
        string strmachinename = commandArgs[1];

        try
        {
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            string userid = (string)Session["UserName"];
            serviceInterface.insertLog("Clicked Play Button ", "Clicked Play Button to Start Process Manually", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            QueueInterface qi = new QueueInterface();
            qi.TriggerStartStop("Start", strbotid, strmachinename, groupid, tenantid, userid);
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception: BotDashBoard Page - Start Process", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            //Response.Redirect("~/pages/Login.aspx");
        }
    }

    protected void StopProcess(object sender, CommandEventArgs e)
    {
        try
        {
            ImageButton objImage = (ImageButton)sender;

            string[] commandArgs = objImage.CommandArgument.ToString().Split(new char[] { ',' });
            string strbotid = commandArgs[0];
            string strmachinename = commandArgs[1];

            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            string userid = (string)Session["UserName"];
            serviceInterface.insertLog("Clicked Stop Button ", "Clicked Stop Button to Stopping Process Forcefully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            QueueInterface qi = new QueueInterface();
            qi.TriggerStartStop("Stop", strbotid, strmachinename, groupid, tenantid, userid);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            //serviceInterface.insertLog("Exception: Session Expired from BotDashBoard Page - Stop Process", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }


    protected void refreshBotDashboard(object sender, CommandEventArgs e)
    {
        LoadBots();
    }

}