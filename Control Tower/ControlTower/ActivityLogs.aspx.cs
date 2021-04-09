///-----------------------------------------------------------------
///   Namespace:      -
///   Class:                Control_Tower_ActivityLogs
///   Description:       To capture all Active Logs.
///   Author:         B.Piyush                    Date: 2020-04-30 13:04:25.667
///   Notes:          <Notes>
///   Revision History:
///   Team:   E2E Team
///   Name:           Date:        Description:
///-----------------------------------------------------------------

#region headers
using System;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion 

public partial class Control_Tower_ActivityLogs : System.Web.UI.Page
{
    #region Variable Declarations
    public ServiceInterface serviceInterface = new ServiceInterface();
    DataTable result = null;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
                else
                {
                    loadActivityLogs();
                }
            }
            else if (IsPostBack)
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
            }
            else
            {
                if (Session["TenantId"] == null || Session["GroupId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                }
            }
        }
        catch(Exception ex)
        {
            serviceInterface.insertLog("Exception: Load Bots on BotDashBoard Page", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    #endregion

    #region Page_PreInit
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
        catch (Exception ex)
        {
            serviceInterface.insertLog("Control_Tower_ActivityLogs issue : While  Page_PreInit ", "Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    #endregion

    #region ReloadActivityLogs
    protected void ReloadActivityLogs(object sender, CommandEventArgs e)
    {
        loadActivityLogs();  
    }
    #endregion

    #region loadActivityLogs
    public void loadActivityLogs()
    {
        try
        {
            int groupid = (int)Session["GroupId"];      //Get GroupId from Session
            int tenantid = (int)Session["TenantId"];   //Get TenantId from Session

            getActivityLog(groupid, tenantid);
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
        }
    }
    #endregion

    #region getActivityLog
    public void getActivityLog(int groupid, int tenantid)
    {
        try
        {
            result = serviceInterface.getActivityLog(groupid, tenantid);

            if (result != null)
            {
                GVActivityLogs.DataSource = result;
                GVActivityLogs.DataBind();
            }
            else
            {
                serviceInterface.insertLog("Control_Tower_ActivityLogs issue : Log Table Empty ","Log Table Empty", groupid, tenantid);
            }
        }
        catch(Exception ex)
        {
            serviceInterface.insertLog("Control_Tower_ActivityLogs issue : While  getActivityLog ", "Exception: " + ex.Message, groupid, tenantid);
        }
    }
    #endregion

}