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


using CronExpressionDescriptor;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class DemoMasterPage2_QueueDetails : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();
    private bool Queue_Click = true;
    private bool Process_Click = false;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from QueueDetails Page", 0, 0);
            }
            else
            {
                LoadSchedules();
                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Queue Details");
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
                        // Nothing to create on this Page.
                    }

                    if (!EditAccess)
                    {
                        // Nothing to Edit on this Page.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GrvSchedules.Items)
                        {
                            
                            ImageButton deleteSchedule = item.FindControl("btnDelete") as ImageButton;

                            deleteSchedule.Enabled = false;
                            deleteSchedule.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteSchedule.ToolTip = "You dont have access to DELETE this Schedule";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from QueueDetails Page", 0, 0);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from QueueDetails Page", 0, 0);
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

    //Load Schedules
    public void LoadSchedules()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetSchedules(groupid, tenantid);

        /*Converting UTC Time Format into Local Time Format*/


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
                StrChronExpression = (row[3].ToString());
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
                row.SetField(3, newStrChronExpression);
            }

        }
        /**/

        if (result != null)
        {
            GrvSchedules.DataSource = result;

            GrvSchedules.DataBind();
        }
    }

    //Delete Schedules
    protected void CommandBtnDelete_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string strScheduleid = commandArgs[0];
            string stbotname = commandArgs[1];
            string strqueuename = commandArgs[2];
            string strchronexpression = commandArgs[3];

            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }


            //string strid = e.CommandArgument.ToString(); 
            string strid = e.CommandArgument.ToString().Substring(0, e.CommandArgument.ToString().IndexOf(",")); // Taking only the First String Element


            string strcurrentuser = (string)Session["UserName"];

            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteSchedule(strid, strqueuename, stbotname, strchronexpression, groupid, tenantid, strcurrentuser);
            if (0 < result)
            {
                LoadSchedules();
                serviceInterface.insertLog("Delete Schedule Successful", "Delete Schedule: Schedule with ID: " + strid + " and Chron : " + strchronexpression + " has been deleted successfully. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
    }
    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        LoadSchedules();
    }

    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

        string strid = commandArgs[0];
        string strQueueName = commandArgs[1];
        string strBotName = commandArgs[2];
        string strChronExpression = commandArgs[3];

        lblScheduleIdDelete.Text = strid;
        lblQueueNameDelete.Text = strQueueName;
        lblBotNameDelete.Text = strBotName;
        lblChronExpressionDelete.Text = strChronExpression;


        lblScheduleIdDeleteSecondPopUp.Text = strid;
        lblQueueNameDeleteSecondPopUp.Text = strQueueName;
        lblBotNameDeleteSecondPopUp.Text = strBotName;
        lblChronExpressionDeleteSecondPopUp.Text = strChronExpression;

        ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('ID Exists ')</script>");
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        //Response.Redirect(Request.RawUrl, false);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        string strid = lblScheduleIdDelete.Text;
        string stbotname = lblBotNameDelete.Text;
        string strqueuename = lblQueueNameDelete.Text;
        string strchronexpression = lblChronExpressionDelete.Text;
        string strcurrentuser = (string)Session["UserName"];

        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();

        int result = serviceInterface.DeleteSchedule(strid, strqueuename, stbotname, strchronexpression, groupid, tenantid, strcurrentuser);
        if (result > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitPROCESSDELETEDSUCCESSFULLY") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforBot") + " " + stbotname + "" + (String)GetGlobalResourceObject("content", "LitwithChron") + "  " + strchronexpression + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
            serviceInterface.insertLog("Delete Schedule Successful", "Delete Schedule: Schedule with QueueName : " + strqueuename + " with Bot : " + stbotname + " and Chron : " + strchronexpression + " has been deleted successfully. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            LoadSchedules();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETEPROCESS") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforBot") + " " + stbotname + " " + (String)GetGlobalResourceObject("content", "LitwithChron") + "" + strchronexpression + " " + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "' , showConfirmButton: false,  timer: 1500});", true);
            serviceInterface.insertLog("Delete Schedule Failed", "Delete Schedule: Deletion of Schedule with QueueName : " + strqueuename + " with Bot : " + stbotname + "  and Chron : " + strchronexpression + " has failed. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            LoadSchedules();
        }
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }


    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void btnQueue_Click(object sender, EventArgs e)
    {
        Queue_Click = true;
        Process_Click = false;
        btnProcess.CssClass = "btn btn-dark";
        btnQueue.CssClass = "btn btn-success";
        processRepeater.Visible = false;
        GrvSchedules.Visible = true;
        LoadSchedules();
    }

    public void LoadProcessSchedules()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();


        DataTable result = serviceInterface.GetProcessSchedules(groupid, tenantid);


        /*Converting UTC Time Format into Local Time Format*/


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
                StrChronExpression = (row[3].ToString());
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
                row.SetField(3, newStrChronExpression);
            }

        }
        /**/

        if (result != null)
        {
            processRepeater.DataSource = result;

            processRepeater.DataBind();
        }
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {

        Queue_Click = false;
        Process_Click = true;
        btnQueue.CssClass = "btn btn-dark";
        btnProcess.CssClass = "btn btn-success";
        GrvSchedules.Visible = false;
        processRepeater.Visible = true;
        LoadProcessSchedules();
    }


    protected void btnProcessDelete_Click(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

        string strid = commandArgs[0];
        string strProcessName = commandArgs[1];
        string strBotName = commandArgs[2];
        string strChronExpression = commandArgs[3];

        lblScheduleIdDelete1.Text = strid;
        lblprocessname.Text = strProcessName;
        lblbotname.Text = strBotName;
        lblchron.Text = strChronExpression;


        lblScheduleId.Text = strid;
        lblprocessNameSec.Text = strProcessName;
        lblBotNameSec.Text = strBotName;
        lblChronSec.Text = strChronExpression;

        ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('ID Exists ')</script>");
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalProcessDelete();", true);
    }
    protected void ModalPopUpBtnProcessDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        string strid = lblScheduleIdDelete1.Text;
        string stbotname = lblbotname.Text;
        string strprocessename = lblprocessname.Text;
        string strchronexpression = lblchron.Text;
        string strcurrentuser = (string)Session["UserName"];

        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();

        int result = serviceInterface.DeleteProcessSchedule(strid, strprocessename, stbotname, strchronexpression, groupid, tenantid, strcurrentuser);
        if (result > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitPROCESSDELETEDSUCCESSFULLY") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforBot") + " " + stbotname + " " + (String)GetGlobalResourceObject("content", "LitwithChron") + " " + strchronexpression + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
            serviceInterface.insertLog("Delete Schedule Successful", "Delete Schedule: Schedule with QueueName : " + strprocessename + " with Bot : " + stbotname + " and Chron : " + strchronexpression + " has been deleted successfully. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            LoadProcessSchedules();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETEPROCESS") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforBot") + " " + stbotname + " " + (String)GetGlobalResourceObject("content", "LitwithChron") + "" + strchronexpression + " " + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "' , showConfirmButton: false,  timer: 1500});", true);
            serviceInterface.insertLog("Delete Schedule Failed", "Delete Schedule: Deletion of Schedule with QueueName : " + strprocessename + " with Bot : " + stbotname + "  and Chron : " + strchronexpression + " has failed. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            LoadProcessSchedules();
        }
    }
    protected void ModalPopUpBtnProcessDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalProcessDeleteSecondPopUp();", true);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }


}