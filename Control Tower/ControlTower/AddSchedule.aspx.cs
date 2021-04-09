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
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class DemoMasterPage2_AddSchedule : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();
    protected static bool Queue_Click = false;
    protected static bool Process_Click = false;
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
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from AddSchedule Page", 0, 0);
            }
            else
            {
                LoadBots();
                LoadQueues();
                LoadProcess();

                string label = (String)GetGlobalResourceObject("content", "LitScheduleRobotUsingaQueue");
                lblPageName.Text = label;

                Queue_Click = true;
                Process_Click = false;

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]); 
                    db = serviceInterface.GetPageAccess(roleid, groupId, roleid, "Add Schedule");
                    bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                    bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                    bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                    bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                    if (!ViewAccess)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYoudonothaveaccesstothisPageYouaremakinganUnauthorizedAccess") + "', showConfirmButton: false,  timer: 2500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                        //Will be decided from MasterPage.
                    }

                    if (!CreateAccess)
                    {
                        btnCreateSchedule.Enabled = false;
                        btnCreateSchedule.Attributes.CssStyle.Add("opacity", "0.5");
                        btnCreateSchedule.CssClass = "btn btn-primary btn-lg";
                        btnCreateSchedule.ToolTip = "You don't have Create Schedule Privileges";
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: " + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from AddSchedule Page", 0, 0);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from QueueDetails Page", "Exception: Session Expired from AddSchedule Page", 0, 0);
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
    public void LoadBots()
    {
        int iTenantId = 0;
        int groupid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            iTenantId = (int)Session["TenantId"];
        }

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetBots(groupid, iTenantId);

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
        int iTenantId = 0;
        int groupid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            iTenantId = (int)Session["TenantId"];
        }
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetQueues(groupid, iTenantId);

        if (result != null)
        {
            DrpQueues.DataSource = result;
            DrpQueues.DataValueField = "queuename";
            DrpQueues.DataTextField = "queuename";
            DrpQueues.DataBind();
        }
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        CronExpressionlbl.Text = "";
        if (RadioButtonList1.SelectedIndex == 0)
        {
            Dailypnl.Visible = true;
            Weeklpnl.Visible = false;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = false;

        }
        else if (RadioButtonList1.SelectedIndex == 1)
        {

            Dailypnl.Visible = true;
            Weeklpnl.Visible = true;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = false;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Confirm();", true);
        }
        else if (RadioButtonList1.SelectedIndex == 2)
        {
            Dailypnl.Visible = true;
            Weeklpnl.Visible = true;
            Monthlypnl.Visible = true;
            Advancepnl.Visible = false;
        }
        else if (RadioButtonList1.SelectedIndex == 3)
        {
            Dailypnl.Visible = false;
            Weeklpnl.Visible = false;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = true;
        }
    }
    public void ScheduleAdd(string stopAfter)
    {
        CronExpressionlbl.Text = "";

        if (RadioButtonList1.SelectedIndex == 0 && HoursComboBox.SelectedValue != "Select" && MinutesComboBox.SelectedValue != "Select")
        {
            CronExpressionlbl.Text = CronExpression.EveryNDaysAt(7, Convert.ToInt32(HoursComboBox.SelectedValue), Convert.ToInt32(MinutesComboBox.SelectedValue));
            Dailypnl.Visible = true;
            Weeklpnl.Visible = false;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = false;
        }
        else if (RadioButtonList1.SelectedIndex == 1 && HoursComboBox.SelectedValue != "Select" && MinutesComboBox.SelectedValue != "Select" && (MondayCheckBox.Checked == true || TuesdayCheckBox.Checked == true || WednesdayCheckBox.Checked == true || ThursdayCheckBox.Checked == true || FridayCheckBox.Checked == true || SaturdayCheckBox.Checked == true || SundayCheckBox.Checked == true))
        {
            int total = 0;
            if (MondayCheckBox.Checked) total += 1;
            if (TuesdayCheckBox.Checked) total += 2;
            if (WednesdayCheckBox.Checked) total += 4;
            if (ThursdayCheckBox.Checked) total += 8;
            if (FridayCheckBox.Checked) total += 16;
            if (SaturdayCheckBox.Checked) total += 32;
            if (SundayCheckBox.Checked) total += 64;

            if (total == 0)
            {
                CronExpressionlbl.Text = "At least one day of the week is required.";
            }
            else
            {
                CronExpressionlbl.Text = CronExpression.EverySpecificWeekDayAt(Convert.ToInt32(HoursComboBox.SelectedValue), Convert.ToInt32(MinutesComboBox.SelectedValue), (DaysOfWeek)total);
            }

            Dailypnl.Visible = true;
            Weeklpnl.Visible = true;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = false;

        }
        else if (RadioButtonList1.SelectedIndex == 2 && MonthUpDown.SelectedValue != "Select" && HoursComboBox.SelectedValue != "Select" && MinutesComboBox.SelectedValue != "Select" && (MondayCheckBox.Checked == true || TuesdayCheckBox.Checked == true || WednesdayCheckBox.Checked == true || ThursdayCheckBox.Checked == true || FridayCheckBox.Checked == true || SaturdayCheckBox.Checked == true || SundayCheckBox.Checked == true))
        {
            int total = 0;
            if (MondayCheckBox.Checked) total += 1;
            if (TuesdayCheckBox.Checked) total += 2;
            if (WednesdayCheckBox.Checked) total += 4;
            if (ThursdayCheckBox.Checked) total += 8;
            if (FridayCheckBox.Checked) total += 16;
            if (SaturdayCheckBox.Checked) total += 32;
            if (SundayCheckBox.Checked) total += 64;
            if (total == 0)
            {
                CronExpressionlbl.Text = "At least one day of the week is required.";
            }
            else
            {
                CronExpressionlbl.Text =
                CronExpression.EverySpecificDayMonthAt(Convert.ToInt32(MonthUpDown.SelectedValue), Convert.ToInt32(HoursComboBox.SelectedValue),
                                                                                Convert.ToInt32(MinutesComboBox.SelectedValue), (DaysOfWeek)total);
            }

            Dailypnl.Visible = true;
            Weeklpnl.Visible = true;
            Monthlypnl.Visible = true;
            Advancepnl.Visible = false;
        }
        else if (RadioButtonList1.SelectedIndex == 3)
        {
            if (!string.IsNullOrEmpty(txtCroneExpression.Text))
            {
                CronExpressionlbl.Text = txtCroneExpression.Text;
            }
            Dailypnl.Visible = false;
            Weeklpnl.Visible = false;
            Monthlypnl.Visible = false;
            Advancepnl.Visible = true;
        }

        try
        {
            if (!string.IsNullOrEmpty(CronExpressionlbl.Text))
            {
                if (true == Queue_Click)
                {
                    string strqueuename = DrpQueues.SelectedItem.ToString();
                    string strBotName = DrpBots.SelectedItem.ToString();
                    int tenantId = 0; //need to comment/remove
                    int groupId = 0;
                    if (Session["TenantId"] != null)
                    {
                        tenantId = (int)Session["TenantId"];
                        groupId = (int)Session["GroupId"];
                    }
                    string strcurrentuser = (string)Session["UserName"];
                    //int groupid = 2;
                    strcurrentuser = "Ajit";//need to comment/remove
                    ServiceInterface serviceInterface = new ServiceInterface();
                    int result = serviceInterface.AddSchedule(strqueuename, strBotName, CronExpressionlbl.Text, stopAfter, groupId, tenantId, strcurrentuser);

                    /*Sweetalert for PopupMessage*/
                    if (0 < result)
                    {
                        // " + (String)GetGlobalResourceObject("content", "LitScheduleforQueue") + " " + strqueuename + "" + (String)GetGlobalResourceObject("content", "LitwithBotName") + "  " + strBotName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "
                        serviceInterface.insertLog("Add Schedule Successful", "Add Schedule : Schedule for Queue : " + strqueuename + " with Bot " + strBotName + " and Chron : " + CronExpressionlbl.Text + " has been added Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitSCHEDULEADDEDSUCCESSFULLY") + "',text:'" + strqueuename + "" + (String)GetGlobalResourceObject("content", "LitwithBotName") + "  " + strBotName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'QueueDetails.aspx';});", true);
                    }
                    else
                    {
                        serviceInterface.insertLog("Add Schedule Failed", "Add Schedule : Addition of Schedule for Queue : " + strqueuename + " with Bot " + strBotName + " and Chron : " + CronExpressionlbl.Text + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDADDINGSCHEDULE") + "',text:' " + strqueuename + " " + (String)GetGlobalResourceObject("content", "LitwithBotName") + "" + strBotName + "" + (String)GetGlobalResourceObject("content", "LithasfailedtoAddSchedule") + " ', showConfirmButton: false,  timer: 1500});", true);
                    }
                }

                else if (true == Process_Click)
                {
                    string strProcessName = DrpProcess.SelectedItem.ToString();
                    string strBotName = DrpBots.SelectedItem.ToString();
                    int tenantId = 0;
                    int groupId = 0;
                    string strcurrentuser = String.Empty;
                    int addSchedule = 0;

                    if ((Session["TenantId"] != null) && Session["GroupId"] != null)
                    {
                        tenantId = (int)Session["TenantId"];
                        groupId = (int)Session["GroupId"];
                        strcurrentuser = (string)Session["UserName"];
                    }



                    addSchedule = serviceInterface.AddScheduleForProcess(strProcessName, strBotName, CronExpressionlbl.Text, stopAfter, groupId, tenantId, strcurrentuser);

                    /*Sweetalert for PopupMessage*/
                    if (0 < addSchedule)
                    {
                        serviceInterface.insertLog("Add Schedule Successful", "Add Schedule : Schedule for Process : " + strProcessName + " with Bot " + strBotName + " and Chron : " + CronExpressionlbl.Text + " has been added Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitSCHEDULEADDEDSUCCESSFULLY") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforProcess") + " " + strProcessName + " " + (String)GetGlobalResourceObject("content", "LitwithBotName") + " " + strBotName + " " + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'QueueDetails.aspx';});", true);
                    }
                    else
                    {
                        serviceInterface.insertLog("Add Schedule Failed", "Add Schedule : Addition of Schedule for Process : " + strProcessName + " with Bot " + strBotName + " and Chron : " + CronExpressionlbl.Text + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDADDINGSCHEDULE") + "',text:'" + "" + (String)GetGlobalResourceObject("content", "LitScheduleforProcess") + " " + strProcessName + " " + (String)GetGlobalResourceObject("content", "LitwithBotName") + " " + strBotName + " " + (String)GetGlobalResourceObject("content", "LithasfailedtoAddSchedule") + "', showConfirmButton: false,  timer: 1500});", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception: Add Schedule", ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    protected void btnCreateScheduleNow(object sender, EventArgs e)
    {
        try
        {
            #region Process For A Queue
            if (true == Queue_Click)
            {
                if (DrpBots.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterBotname") + "');", true);
                }

                if (DrpQueues.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterQueuename") + "');", true);
                }

                if ((HoursComboBox.SelectedValue == "Select") || (MinutesComboBox.SelectedValue == "Select"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(' " + (String)GetGlobalResourceObject("content", "LitPleaseEnterAppointmenttime") + "');", true);
                }

                if (RadioButtonList1.SelectedIndex == 1)
                {
                    if (MondayCheckBox.Checked == false && TuesdayCheckBox.Checked == false && WednesdayCheckBox.Checked == false && ThursdayCheckBox.Checked == false && FridayCheckBox.Checked == false && SaturdayCheckBox.Checked == false && SundayCheckBox.Checked == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseSelectatleastoneday") + "');", true);
                    }
                }

                if (RadioButtonList1.SelectedIndex == 2)
                {
                    if (MondayCheckBox.Checked == false && TuesdayCheckBox.Checked == false && WednesdayCheckBox.Checked == false && ThursdayCheckBox.Checked == false && FridayCheckBox.Checked == false && SaturdayCheckBox.Checked == false && SundayCheckBox.Checked == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseSelectatleastoneday") + "');", true);
                    }
                    if (RadioButtonList1.SelectedIndex == 2 && MonthUpDown.SelectedValue == "Select")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterMonth") + "');", true);

                    }
                }

                if (CheckBox1.Checked == true)
                {
                    if ((DropDownList3.SelectedValue == "Select") || (DropDownList4.SelectedValue == "Select"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterStopaftertime") + "');", true);
                    }
                    if (txtUserName.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterStopaftertime") + "');", true);
                    }
                }

                if (DrpBots.SelectedValue != "0" && DrpQueues.SelectedValue != "0")
                {
                    string stopAfter = null;
                    if (true == CheckBox1.Checked && DropDownList3.SelectedValue != "Select" && DropDownList4.SelectedValue != "Select" && txtUserName.Text != "")
                    {
                        DateTime dt = new DateTime();
                        try
                        {
                            int Days = int.Parse(txtUserName.Text);
                            int Hours = int.Parse(DropDownList3.SelectedValue);
                            int Minutes = int.Parse(DropDownList4.SelectedValue);

                            int HoursStart = int.Parse(HoursComboBox.SelectedValue);
                            int MinutesStart = int.Parse(MinutesComboBox.SelectedValue);
                            dt = DateTime.Now.Date.Add(new TimeSpan(HoursStart, MinutesStart, 0));

                            DateTime dateUTC = dt.ToUniversalTime();

                            DateTime dt2 = dateUTC.AddDays(int.Parse(txtUserName.Text)).AddHours(int.Parse(DropDownList3.SelectedValue)).AddMinutes(int.Parse(DropDownList4.SelectedValue));
                            Console.WriteLine(string.Format("{0}", dt2));
                            stopAfter = dt2.ToString("dd MMMM yyyy HH:mm:ss");
                            Console.WriteLine("\n Stop After : " + stopAfter);

                        }
                        catch (System.FormatException ex)
                        {
                            stopAfter = null;
                        }
                        ScheduleAdd(stopAfter);
                    }
                    if (false == CheckBox1.Checked)
                    {
                        ScheduleAdd(stopAfter);
                    }
                }
            }
            #endregion

            #region  Start Robot with a Process.
            else if (true == Process_Click)
            {
                if ("0" == DrpBots.SelectedValue)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterBotname") + "');", true);
                }
                if ("0" == DrpProcess.SelectedValue)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterProcessname") + "');", true);
                }
                if ((HoursComboBox.SelectedValue == "Select") || (MinutesComboBox.SelectedValue == "Select"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterAppointmenttime") + "');", true);
                }

                if (RadioButtonList1.SelectedIndex == 1)
                {
                    if (MondayCheckBox.Checked == false && TuesdayCheckBox.Checked == false && WednesdayCheckBox.Checked == false && ThursdayCheckBox.Checked == false && FridayCheckBox.Checked == false && SaturdayCheckBox.Checked == false && SundayCheckBox.Checked == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseSelectatleastoneday") + "');", true);
                    }
                }

                if (RadioButtonList1.SelectedIndex == 2)
                {
                    if (MondayCheckBox.Checked == false && TuesdayCheckBox.Checked == false && WednesdayCheckBox.Checked == false && ThursdayCheckBox.Checked == false && FridayCheckBox.Checked == false && SaturdayCheckBox.Checked == false && SundayCheckBox.Checked == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseSelectatleastoneday") + "');", true);
                    }
                    if (RadioButtonList1.SelectedIndex == 2 && MonthUpDown.SelectedValue == "Select")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterMonth") + "');", true);

                    }
                }

                if (CheckBox1.Checked == true)
                {
                    if ((DropDownList3.SelectedValue == "Select") || (DropDownList4.SelectedValue == "Select"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterStopaftertime") + "');", true);
                    }
                    if (txtUserName.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterStopaftertime") + "');", true);
                    }
                }

                if (DrpBots.SelectedValue != "0" && DrpProcess.SelectedValue != "0")
                {
                    string stopAfter = null;
                    if (true == CheckBox1.Checked && DropDownList3.SelectedValue != "Select" && DropDownList4.SelectedValue != "Select" && txtUserName.Text != "")
                    {
                        DateTime dt = new DateTime();
                        try
                        {
                            int Days = int.Parse(txtUserName.Text);
                            int Hours = int.Parse(DropDownList3.SelectedValue);
                            int Minutes = int.Parse(DropDownList4.SelectedValue);

                            int HoursStart = int.Parse(HoursComboBox.SelectedValue);
                            int MinutesStart = int.Parse(MinutesComboBox.SelectedValue);
                            dt = DateTime.Now.Date.Add(new TimeSpan(HoursStart, MinutesStart, 0));

                            DateTime dateUTC = dt.ToUniversalTime();

                            DateTime dt2 = dateUTC.AddDays(int.Parse(txtUserName.Text)).AddHours(int.Parse(DropDownList3.SelectedValue)).AddMinutes(int.Parse(DropDownList4.SelectedValue));
                            stopAfter = dt2.ToString("dd MMMM yyyy HH:mm:ss");
                        }
                        catch (System.FormatException ex)
                        {
                            stopAfter = null;
                        }
                        ScheduleAdd(stopAfter);
                    }
                    if (false == CheckBox1.Checked)
                    {
                        ScheduleAdd(stopAfter);
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Error While adding Process Schedule ","Exception Message :"+ex.Message,groupId,tenantId);
        }
    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        LoadBots();
        LoadQueues();
    }

    protected void BtnCancelSchedule(object sender, EventArgs e)
    {
        //txtQueueListenerTime.Text = null;
        txtUserName.Text = null;
        if (DrpBots.SelectedValue != "0")
        {
            DrpBots.SelectedValue = "0";
        }
        if (DrpQueues.SelectedValue != "0")
        {
            DrpQueues.SelectedValue = "0";
        }
        if (HoursComboBox.SelectedValue != "Select")
        {
            HoursComboBox.SelectedValue = "Select";
        }
        if (MinutesComboBox.SelectedValue != "Select")
        {
            MinutesComboBox.SelectedValue = "Select";
        }
        if (DropDownList3.SelectedValue != "Select")
        {
            DropDownList3.SelectedValue = "Select";
        }
        if (DropDownList4.SelectedValue != "Select")
        {
            DropDownList4.SelectedValue = "Select";
        }
        if (RadioButtonList1.SelectedIndex == 2 && MonthUpDown.SelectedValue != "Select")
        {
            MonthUpDown.SelectedValue = "Select";
        }

        if (CheckBox1.Checked == true)
        {
            CheckBox1.Checked = false;
        }
        if (MondayCheckBox.Checked == true)
        {
            MondayCheckBox.Checked = false;
        }
        if (TuesdayCheckBox.Checked == true)
        {
            TuesdayCheckBox.Checked = false;
        }
        if (WednesdayCheckBox.Checked == true)
        {
            WednesdayCheckBox.Checked = false;
        }
        if (ThursdayCheckBox.Checked == true)
        {
            ThursdayCheckBox.Checked = false;
        }
        if (FridayCheckBox.Checked == true)
        {
            FridayCheckBox.Checked = false;
        }
        if (SaturdayCheckBox.Checked == true)
        {
            SaturdayCheckBox.Checked = false;
        }
        if (SundayCheckBox.Checked == true)
        {
            SundayCheckBox.Checked = false;
        }
    }

    #region Select between a Queue and a Process
    protected void btnQueue_Click(object sender, EventArgs e)
    {
        Queue_Click = true;
        Process_Click = false;
        string label = (String)GetGlobalResourceObject("content", "LitScheduleRobotUsingaQueue");
        lblPageName.Text = label;
        btnQueue.CssClass = "btn btn-success";
        btnProcess.CssClass = "btn btn-dark";
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        // loadRunningProcess();
        Queue_Click = false;
        Process_Click = true;
        string label = (String)GetGlobalResourceObject("content", "LitScheduleRobotUsingaProcess");
        lblPageName.Text = label;
        btnQueue.CssClass = "btn btn-dark";
        btnProcess.CssClass = "btn btn-success";
    }

    public void LoadProcess()
    {
        int iTenantId = 0;
        int groupid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            iTenantId = (int)Session["TenantId"];
        }

        DataTable result = serviceInterface.GetProcessDataWithFile(groupid, iTenantId);

        if (result != null)
        {
            DrpProcess.DataSource = result;
            DrpProcess.DataValueField = "processname";
            DrpProcess.DataTextField = "processname";
            DrpProcess.DataBind();
        }
    }
    #endregion
}