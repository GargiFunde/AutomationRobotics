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

public partial class ControlTower_ProcessQueueMapping : System.Web.UI.Page
{
    protected ServiceInterface serviceInterface = new ServiceInterface();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            }
            else
            {
                GetProcessQMapping();
                //#region RoleBaseAccess

                //try
                //{
                //    int groupId = 0;
                //    int tenantId = 0;
                //    string userName = string.Empty;
                //    DataTable db = null;
                //    userName = (string)Session["UserName"];
                //    groupId = Convert.ToInt32(Session["GroupId"]);
                //    tenantId = Convert.ToInt32(Session["TenantId"]);
                //    int roleid = Convert.ToInt32(Session["roleid"]);
                //    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Bot Logs");
                //    bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                //    bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                //    bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                //    bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                //    if (!ViewAccess)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: 'SESSION EXPIRED!',text:'You do not have access to this Page. You are making an Unauthorized Access.', showConfirmButton: false,  timer: 2500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                //        //Will be decided from MasterPage.
                //    }

                //    if (!CreateAccess)
                //    {

                //    }

                //    if (!EditAccess)
                //    {
                //        //No Edit Operation.
                //    }

                //    if (!DeleteAccess)
                //    {

                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                //#endregion  
            }
        }
        else if (IsPostBack)
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
            }
        }
    }

    //protected override void InitializeCulture()
    //{
    //    try
    //    {
    //        base.InitializeCulture();

    //        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session["Culture"].ToString());

    //        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session["Culture"].ToString());


    //    }
    //    catch (Exception ex)
    //    {

    //        throw;
    //    }

    //}
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
    private void GetProcessQMapping()
    {
        string strUserId = string.Empty;

        DataTable result = null;
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        string strcurrentuser = (string)Session["UserName"];
       

        result = serviceInterface.GetProcessQMapping(groupid, tenantid);

        GVprocessQMapping.DataSource = result;
        GVprocessQMapping.DataBind();

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        GetProcessQMapping();
    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        GetProcessQMapping();

    }
}