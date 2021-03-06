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
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Control_Tower_CompleteLogs : System.Web.UI.Page
{

    protected ServiceInterface serviceInterface = new ServiceInterface();
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
                loadCompleteLogs();
                #region RoleBaseAccess

                try
                {
                    int groupId = 0;
                    int tenantId = 0;
                    string userName = string.Empty;
                    DataTable db = null;
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Complete Logs");
                    bool ViewAccess = Convert.ToBoolean(db.Rows[0]["ReadA"]);
                    bool CreateAccess = Convert.ToBoolean(db.Rows[0]["CreateA"]);
                    bool EditAccess = Convert.ToBoolean(db.Rows[0]["EditA"]);
                    bool DeleteAccess = Convert.ToBoolean(db.Rows[0]["DeleteA"]);

                    if (!ViewAccess)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYoudonothaveaccesstothisPageYouaremakinganUnauthorizedAccess") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                       
                        //Will be decided from MasterPage.
                    }

                    if (!CreateAccess)
                    {

                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {

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

    protected void ReloadCompleteLogs(object sender, CommandEventArgs e)
    {

        loadCompleteLogs();

    }

    public void loadCompleteLogs()
    {

        //try
        //{

            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];

            getCompleteLogs(groupid, tenantid);
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    Response.Redirect("Login.aspx");
        //    throw;
        //}
    }

    public void getCompleteLogs(int groupid, int tenantid)
    {
        DataTable result = serviceInterface.getCompleteLogs(groupid, tenantid);

        if (result != null)
        {
            GVCompleteLogs.DataSource = result;
            GVCompleteLogs.DataBind();
        }
        else
        {
            Debug.Write("Error in Database");
        }

    }

}