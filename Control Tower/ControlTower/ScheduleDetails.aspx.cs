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
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoMasterPage2_ScheduleDetails : System.Web.UI.Page
{
    protected static int RefreshTimer;
    DataTable result = new DataTable();
    ServiceInterface serviceInterface = new ServiceInterface();
    private bool Previous_Click = false;
    private bool Processing_Click = false;
    private bool Scheduled_Click = false;
    private bool Failed_Click = false;
    protected int groupId = 0;
    protected int tenantId = 0;
    protected string userName = string.Empty;
    DataTable db = null;
    public DemoMasterPage2_ScheduleDetails()              /*Constructor*/
    {
        /*Initialization can be done here.*/
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                clearDataTable();
                loadRunningProcess();

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Schedule Details");
                    bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                    bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                    bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                    bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                    if (!ViewAccess)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: 'SESSION EXPIRED!',text:'You do not have access to this Page. You are making an Unauthorized Access.', showConfirmButton: false,  timer: 2500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                        //Will be decided from MasterPage.
                    }

                    if (!CreateAccess)
                    {
                        // Nothing to Create on this Page.
                    }

                    if (!EditAccess)
                    {
                        // Nothing to Edit on this Page.
                    }

                    if (!DeleteAccess)
                    {
                        // Nothing to Delete on this Page.
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
    protected void clearDataTable()
    {
        GrvSchedules.DataSource = null;
        GrvSchedules.DataBind();
    }

    protected void loadCompletedProcess()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        string Previous = "Previous";

        lblPageName.Text = (String)GetGlobalResourceObject("content", "LitCompletedProcesses");
        clearDataTable();

        result = serviceInterface.GetScheduleStatus(groupid, tenantid, Previous);

        var tz = TimeZone.CurrentTimeZone;
        TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
        int totalMinute = (int)ts.TotalMinutes;
        int Hour = totalMinute / 60;
        int Minute = totalMinute % 60;

        DateTimeOffset now = DateTimeOffset.Now;
        DateTime dt = new DateTime();
        DateTime dtAdd = new DateTime();
        string StrChronExpression = null;
        String newStrChronExpression = null;
        String ChronExpression = string.Empty;
        string ChronExpressionHumanReadable = string.Empty;
        string[] StrChronExpressionArray = null;
        string[] HRMinAdd = null;
        string time = null;
        string dtAddstr = null;

        if (totalMinute != 0)
        {
            foreach (DataRow row in result.Rows)
            {
                ChronExpression = (row[2].ToString());
                if (ChronExpression.Contains("*"))
                {
                    ChronExpressionHumanReadable = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(ChronExpression);
                    row.SetField(2, ChronExpressionHumanReadable);
                }

                if (ChronExpression.Contains("At"))
                {
                    StrChronExpression = (row[2].ToString());

                    StrChronExpressionArray = StrChronExpression.Split(' ');
                    time = StrChronExpressionArray[1];

                    HRMinAdd = time.Split(':');

                    if (StrChronExpression.Contains("AM"))
                    {
                        dt = DateTime.Now.Date.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                    }
                    if (StrChronExpression.Contains("PM"))
                    {
                        DateTime dt1 = DateTime.Now.Date.Add(new TimeSpan(12, 0, 0));
                        dt = dt1.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                    }
                    dtAdd = dt.AddHours(Hour).AddMinutes(Minute);
                    dtAddstr = dtAdd.ToString("h:mm tt");

                    string StrChronExpressionWithoutTime = StrChronExpression.Remove(3, 8);
                    newStrChronExpression = StrChronExpressionWithoutTime.Insert(3, dtAddstr);
                    row.SetField(2, newStrChronExpression);
                }
            }
        }

        if (result != null)
        {
            GrvSchedules.DataSource = result;
            GrvSchedules.DataBind();
        }
    }

    protected void loadRunningProcess()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        string Processing = "Processing";
        lblPageName.Text = (String)GetGlobalResourceObject("content", "LitCurrentlyExecutingProcesses");
        clearDataTable();

        ServiceInterface serviceInterface = new ServiceInterface();
        result = serviceInterface.GetScheduleStatus(groupid, tenantid, Processing);

        if (null != result)
        {
            //Convert String cron expression(UTC) to local time format(Human readable)
            var tz = TimeZone.CurrentTimeZone;
            TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
            int totalMinute = (int)ts.TotalMinutes;
            int Hour = totalMinute / 60;
            int Minute = totalMinute % 60;

            DateTimeOffset now = DateTimeOffset.Now;
            DateTime dt = new DateTime();
            DateTime dtAdd = new DateTime();
            string StrChronExpression = null;
            String newStrChronExpression = null;
            String ChronExpression = string.Empty;
            string ChronExpressionHumanReadable = string.Empty;
            string[] StrChronExpressionArray = null;
            string[] HRMinAdd = null;
            string time = null;
            string dtAddstr = null;

            if (totalMinute != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    ChronExpression = (row[2].ToString());
                    if (ChronExpression.Contains("*"))
                    {
                        ChronExpressionHumanReadable = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(ChronExpression);
                        row.SetField(2, ChronExpressionHumanReadable);
                    }

                    if (ChronExpression.Contains("At"))
                    {
                        StrChronExpression = (row[2].ToString());

                        StrChronExpressionArray = StrChronExpression.Split(' ');
                        time = StrChronExpressionArray[1];

                        HRMinAdd = time.Split(':');

                        if (StrChronExpression.Contains("AM"))
                        {
                            dt = DateTime.Now.Date.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                        }
                        if (StrChronExpression.Contains("PM"))
                        {
                            DateTime dt1 = DateTime.Now.Date.Add(new TimeSpan(12, 0, 0));
                            dt = dt1.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                        }
                        dtAdd = dt.AddHours(Hour).AddMinutes(Minute);
                        dtAddstr = dtAdd.ToString("h:mm tt");

                        string StrChronExpressionWithoutTime = StrChronExpression.Remove(3, 8);
                        newStrChronExpression = StrChronExpressionWithoutTime.Insert(3, dtAddstr);
                        row.SetField(2, newStrChronExpression);
                    }
                }
            }

                GrvSchedules.DataSource = result;
                GrvSchedules.DataBind();
        }
    }

    protected void loadSchedules()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        string Scheduled = "Scheduled";
        lblPageName.Text = (String)GetGlobalResourceObject("content", "LitScheduledProcesses");
        clearDataTable();

        result = serviceInterface.GetScheduleStatus(groupid, tenantid, Scheduled);

        if (null != result)
        {
            var tz = TimeZone.CurrentTimeZone;
            TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
            int totalMinute = (int)ts.TotalMinutes;
            int Hour = totalMinute / 60;
            int Minute = totalMinute % 60;

            DateTimeOffset now = DateTimeOffset.Now;

            DateTime dt = new DateTime();
            DateTime dtAdd = new DateTime();
            string StrChronExpression = null;
            String newStrChronExpression = null;
            string[] StrChronExpressionArray = null;
            string[] HRMinAdd = null;
            string time = null;
            string dtAddstr = null;

            if (totalMinute != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    StrChronExpression = (row[2].ToString());
                    StrChronExpressionArray = StrChronExpression.Split(' ');
                    time = StrChronExpressionArray[1];

                    HRMinAdd = time.Split(':');

                    if (StrChronExpression.Contains("AM"))
                    {
                        dt = DateTime.Now.Date.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                    }
                    if (StrChronExpression.Contains("PM"))
                    {
                        DateTime dt1 = DateTime.Now.Date.Add(new TimeSpan(12, 0, 0));
                        dt = dt1.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                    }

                    dtAdd = dt.AddHours(Hour).AddMinutes(Minute);
                    dtAddstr = dtAdd.ToString("h:mm tt");

                    string StrChronExpressionWithoutTime = StrChronExpression.Remove(3, 8);
                    newStrChronExpression = StrChronExpressionWithoutTime.Insert(3, dtAddstr);
                    row.SetField(2, newStrChronExpression);
                }

            }
            GrvSchedules.DataSource = result;
            GrvSchedules.DataBind();
        }
    }


    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        loadCompletedProcess();
        Previous_Click = true;
        Processing_Click = false;
        Scheduled_Click = false;
        Failed_Click = false;
        btnPrevious.CssClass = "btn btn-warning";
        btnProcessing.CssClass = "btn btn-dark";
        btnScheduled.CssClass = "btn btn-dark";
        btnFailed.CssClass = "btn btn-dark";
    }

    protected void btnProcessing_Click(object sender, EventArgs e)
    {
        loadRunningProcess();
        Previous_Click = false;
        Processing_Click = true;
        Scheduled_Click = false;
        Failed_Click = false;
        btnPrevious.CssClass = "btn btn-dark";
        btnProcessing.CssClass = "btn btn-success";
        btnScheduled.CssClass = "btn btn-dark";
        btnFailed.CssClass = "btn btn-dark";
    }

    protected void btnScheduled_Click(object sender, EventArgs e)
    {
        loadSchedules();
        Previous_Click = false;
        Processing_Click = false;
        Scheduled_Click = true;
        Failed_Click = false;
        btnPrevious.CssClass = "btn btn-dark";
        btnProcessing.CssClass = "btn btn-dark";
        btnScheduled.CssClass = "btn btn-primary";
        btnFailed.CssClass = "btn btn-dark";
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "InfoPrevious();", true);
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "InfoScheduled();", true);
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "InfoProcessing();", true);
        if (lblPageName.Text.Equals("Currently Executing Processes"))
        {

            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            string Processing = "Processing";
            lblPageName.Text = (String)GetGlobalResourceObject("content", "LitCurrentlyExecutingProcesses");

            ServiceInterface serviceInterface = new ServiceInterface();
            DataTable result = serviceInterface.GetScheduleStatus(groupid, tenantid, Processing);

            if (result != null)
            {
                GrvSchedules.DataSource = result;
                GrvSchedules.DataBind();
            }
        }
    }

    protected void refreshCommand(object sender, CommandEventArgs e)
    {

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
        string startTime = commandArgs[2];
        string endTime = commandArgs[3];

        ServiceInterface serviceInterface = new ServiceInterface();
        try
        {
            DataTable result1 = serviceInterface.GetLogsForDashboardBots(strbotid, strmachinename,startTime,endTime, groupid, tenantid);

            Repeater2.DataSource = result1;
            Repeater2.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
        catch (Exception ex)
        {
            Console.Write("Piyush : " + ex.Message);
        }
    }

    //Added on 20-Nov-2019 for failed Processes 
    #region Failed Process
    protected void loadFailedProcess()
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
            string Failed = "Failed";
            lblPageName.Text = (String)GetGlobalResourceObject("content", "LitFailedProcesses");
            clearDataTable();

            result = serviceInterface.GetScheduleStatus(groupid, tenantid, Failed);

            var tz = TimeZone.CurrentTimeZone;
            TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
            int totalMinute = (int)ts.TotalMinutes;
            int Hour = totalMinute / 60;
            int Minute = totalMinute % 60;

            DateTimeOffset now = DateTimeOffset.Now;
            DateTime dt = new DateTime();
            DateTime dtAdd = new DateTime();
            string StrChronExpression = null;
            String newStrChronExpression = null;
            String ChronExpression = string.Empty;
            string ChronExpressionHumanReadable = string.Empty;
            string[] StrChronExpressionArray = null;
            string[] HRMinAdd = null;
            string time = null;
            string dtAddstr = null;

            if (totalMinute != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    ChronExpression = (row[2].ToString());
                    if (ChronExpression.Contains("*"))
                    {
                        ChronExpressionHumanReadable = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(ChronExpression);
                        row.SetField(2, ChronExpressionHumanReadable);
                    }
                    if (ChronExpression.Contains("At"))
                    {
                        StrChronExpression = (row[2].ToString());

                        StrChronExpressionArray = StrChronExpression.Split(' ');
                        time = StrChronExpressionArray[1];

                        HRMinAdd = time.Split(':');

                        if (StrChronExpression.Contains("AM"))
                        {
                            dt = DateTime.Now.Date.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                        }
                        if (StrChronExpression.Contains("PM"))
                        {
                            DateTime dt1 = DateTime.Now.Date.Add(new TimeSpan(12, 0, 0));
                            dt = dt1.Add(new TimeSpan(Int32.Parse(HRMinAdd[0]), Int32.Parse(HRMinAdd[1]), 0));
                        }
                        dtAdd = dt.AddHours(Hour).AddMinutes(Minute);
                        dtAddstr = dtAdd.ToString("h:mm tt");

                        string StrChronExpressionWithoutTime = StrChronExpression.Remove(3, 8);
                        newStrChronExpression = StrChronExpressionWithoutTime.Insert(3, dtAddstr);
                        row.SetField(2, newStrChronExpression);
                    }
                }
            }



            if (result != null)
            {
                GrvSchedules.DataSource = result;
                GrvSchedules.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        
    }

    protected void btnFailed_Click(object sender, EventArgs e)
    {
        try
        {
            Previous_Click = false;
            Processing_Click = false;
            Scheduled_Click = false;
            Failed_Click = true;

            btnPrevious.CssClass = "btn btn-dark";
            btnProcessing.CssClass = "btn btn-dark";
            btnScheduled.CssClass = "btn btn-dark";
            btnFailed.CssClass = "btn btn-danger";

            loadFailedProcess();
        }
        catch(Exception ex)
        {

        }
        
    }
    #endregion
}