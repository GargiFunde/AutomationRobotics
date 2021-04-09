#region Header Declarations
using System;
using System.Collections.Generic;

using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class DemoMasterPage2_MasterPage1 : System.Web.UI.MasterPage
{
    public ServiceInterface serviceInterface = new ServiceInterface();

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Logged Out Forcefully from MasterPage", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                // Response.Redirect("~/DemoMasterPage2/Login.aspx", false);
            }
            else
            {
                string CurrentPage = Path.GetFileName(Request.Url.AbsolutePath);
                lblCurrentPage.Text = CurrentPage.Substring(0, CurrentPage.IndexOf(".")).ToString();

                string userName = String.Empty;
                int tenantId = 0;
                int groupId = 0;

                userName = Session["UserName"].ToString();
                tenantId = Convert.ToInt32(Session["TenantId"]);
                groupId = Convert.ToInt32(Session["GroupId"]);

                DataTable result = new DataTable();
                result.Clear();

                result = serviceInterface.GetLog(userName, tenantId, groupId);

                GetAuditTrail();
                GetLog();
            }
        }
    }
    #endregion

    #region LogOut
    protected void LogOut_OnClick(object sender, EventArgs e)
    {
        serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Logged Out", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        Session.Abandon();

        Session.Remove("DomainName");   
        Session.Remove("UserName");
        Session.Remove("Role");
        Session.Remove("GroupId");
        Session.Remove("TenantId");

        Session["DomainName"] = null;
        Session["UserName"] = null;
        Session["Role"] = null;
        Session["GroupId"] = null;
        Session["TenantId"] = null;

        Session.Clear();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: 'LOGOUT SUCCESSFULLY!', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('LOGOUT SUCCESSFULLY ', 'You are redirected to Login Page', 'success').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);

    }
    #endregion

    #region ChangePassword
    protected void ChangePassword(object sender, EventArgs e)
    {
        try
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Logged Out while changing PassWord", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: 'SESSION EXPIRED!',text:'You are redirected to Login Page', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
                //Response.Redirect("~/DemoMasterPage2/Login.aspx", false);
            }
            else
            {
                int groupid = (int)Session["GroupId"];
                int tenantid = (int)Session["TenantId"];

                string userid = Session["UserName"].ToString();
                string DomainName = Session["DomainName"].ToString();

                string OldPassword = txtOldPassword.Text.Trim();
                string NewPassword = txtNewPassword.Text.Trim();

                string encodePwd = string.Empty;
                byte[] encode = new byte[OldPassword.Length];
                encode = System.Text.Encoding.UTF8.GetBytes(OldPassword);
                encodePwd = Convert.ToBase64String(encode);
                OldPassword = encodePwd;

                string encodePwdNew = string.Empty;
                byte[] encodeNew = new byte[NewPassword.Length];
                encodeNew = System.Text.Encoding.UTF8.GetBytes(NewPassword);
                encodePwdNew = Convert.ToBase64String(encodeNew);
                NewPassword = encodePwdNew;

                ServiceInterface serviceInterface = new ServiceInterface();
                int result = serviceInterface.UpdateUserPassword(userid, OldPassword, NewPassword, groupid, tenantid);

                if (result == 1)
                {
                    serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "PassWord Changed Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    serviceInterface.insertLog("Password for "+Session["UserName"]+"has been changed Successfully.", "Password for " + Session["UserName"] + "has been changed Successfully at "+DateTime.Now,Convert.ToInt32(Session["GroupId"]),Convert.ToInt32(Session["TenantId"]));
                    string someScript = "";
                    someScript = "<script language='javascript'>  swal({  title:'Your password has been changed successfully!', text: 'Please Login Again', icon: 'success', type: 'success' }).then(function() {  window.location = 'LogIn.aspx'});</script>";

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", someScript);
                }
                else
                {
                    serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Failed PassWord Change", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    serviceInterface.insertLog("Password change Failed for " + Session["UserName"] , "Password for " + Session["UserName"] + " has not been changed Successfully ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    string someScriptError = "";
                    someScriptError = "<script language='javascript'>  swal({  title:'Error!', text: 'Old Password Is Incorrect', icon: 'warning' });</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", someScriptError);
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Failed PassWord Change", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            serviceInterface.insertLog("Exception in Password Change", "Exception : " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    #endregion

    #region Get Audit Trail
    /*Master Page for Taskbarup */
    private void GetAuditTrail()
    {
        try
        {
            string strcurrentuser = string.Empty;
            int groupid = 0;
            int tenantid = 0;
            DataTable result = null;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            strcurrentuser = (string)Session["UserName"];

            ServiceInterface serviceInterface = new ServiceInterface();
            result = serviceInterface.GetAuditTrail(groupid, tenantid, strcurrentuser);

            if (null != result.Rows[3])
            {
                Audit1.Text = Convert.ToString(result.Rows[0]["details"]);
                AuditUser1.Text = Convert.ToString(result.Rows[0]["userid"]);
                AuditDate1.Text = Convert.ToString(result.Rows[0]["date"]);

                Audit2.Text = Convert.ToString(result.Rows[1]["details"]);
                AuditUser2.Text = Convert.ToString(result.Rows[1]["userid"]);
                AuditDate2.Text = Convert.ToString(result.Rows[1]["date"]);

                Audit3.Text = Convert.ToString(result.Rows[2]["details"]);
                AuditUser3.Text = Convert.ToString(result.Rows[2]["userid"]);
                AuditDate3.Text = Convert.ToString(result.Rows[2]["date"]);

                Audit4.Text = Convert.ToString(result.Rows[3]["details"]);
                AuditUser4.Text = Convert.ToString(result.Rows[3]["userid"]);
                AuditDate4.Text = Convert.ToString(result.Rows[3]["date"]);
            }
            else if (null != result.Rows[2])
            {
                Audit1.Text = Convert.ToString(result.Rows[0]["details"]);
                AuditUser1.Text = Convert.ToString(result.Rows[0]["userid"]);
                AuditDate1.Text = Convert.ToString(result.Rows[0]["date"]);

                Audit2.Text = Convert.ToString(result.Rows[1]["details"]);
                AuditUser2.Text = Convert.ToString(result.Rows[1]["userid"]);
                AuditDate2.Text = Convert.ToString(result.Rows[1]["date"]);

                Audit3.Text = Convert.ToString(result.Rows[2]["details"]);
                AuditUser3.Text = Convert.ToString(result.Rows[2]["userid"]);
                AuditDate3.Text = Convert.ToString(result.Rows[2]["date"]);

                Audit4.Text = "No Data Found";
                AuditUser4.Text = "No Data Found";
                AuditDate4.Text = "No Data Found";
            }
            else if (null != result.Rows[1])
            {
                Audit1.Text = Convert.ToString(result.Rows[0]["details"]);
                AuditUser1.Text = Convert.ToString(result.Rows[0]["userid"]);
                AuditDate1.Text = Convert.ToString(result.Rows[0]["date"]);

                Audit2.Text = Convert.ToString(result.Rows[1]["details"]);
                AuditUser2.Text = Convert.ToString(result.Rows[1]["userid"]);
                AuditDate2.Text = Convert.ToString(result.Rows[1]["date"]);

                Audit3.Text = "No Data Found";
                AuditUser3.Text = "No Data Found";
                AuditDate3.Text = "No Data Found";

                Audit4.Text = "No Data Found";
                AuditUser4.Text = "No Data Found";
                AuditDate4.Text = "No Data Found";
            }
            else if (null != result.Rows[0])
            {
                Audit1.Text = Convert.ToString(result.Rows[0]["details"]);
                AuditUser1.Text = Convert.ToString(result.Rows[0]["userid"]);
                AuditDate1.Text = Convert.ToString(result.Rows[0]["date"]);

                Audit2.Text = "No Data Found";
                AuditUser2.Text = "No Data Found";
                AuditDate2.Text = "No Data Found";

                Audit3.Text = "No Data Found";
                AuditUser3.Text = "No Data Found";
                AuditDate3.Text = "No Data Found";

                Audit4.Text = "No Data Found";
                AuditUser4.Text = "No Data Found";
                AuditDate4.Text = "No Data Found";
            }
            else
            {
                Audit1.Text = "No Data Found";
                AuditUser1.Text = "No Data Found";
                AuditDate1.Text = "No Data Found";

                Audit2.Text = "No Data Found";
                AuditUser2.Text = "No Data Found";
                AuditDate2.Text = "No Data Found";

                Audit3.Text = "No Data Found";
                AuditUser3.Text = "No Data Found";
                AuditDate3.Text = "No Data Found";

                Audit4.Text = "No Data Found";
                AuditUser4.Text = "No Data Found";
                AuditDate4.Text = "No Data Found";
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Failed to get Audit Trail data from Database in MasterPage", "Exception : " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    #endregion

    #region GetLog
    private void GetLog()
    {
        try
        {
            string strcurrentuser = string.Empty;
            DataTable result = null;

            int groupid = 0;
            int tenantid = 0;

            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            strcurrentuser = (string)Session["UserName"];

            result = serviceInterface.GetLog(strcurrentuser, groupid, tenantid);
            if (1 == result.Rows.Count)
            {
                LogDate1.Text = Convert.ToString(result.Rows[0]["timestampvalue"]);
                LogMsg1.Text = Convert.ToString(result.Rows[0]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[0]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[0]["botname"]);
            }
            else if (2 == result.Rows.Count)
            {
                LogDate1.Text = Convert.ToString(result.Rows[0]["timestampvalue"]);
                LogMsg1.Text = Convert.ToString(result.Rows[0]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[0]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[0]["botname"]);
                LogDate2.Text = Convert.ToString(result.Rows[1]["timestampvalue"]);
                LogMsg2.Text = Convert.ToString(result.Rows[1]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[1]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[1]["botname"]);
            }
            else if (0 != result.Rows.Count)
            {
                LogDate1.Text = Convert.ToString(result.Rows[0]["timestampvalue"]);
                LogMsg1.Text = Convert.ToString(result.Rows[0]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[0]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[0]["botname"]);
                LogDate2.Text = Convert.ToString(result.Rows[1]["timestampvalue"]);
                LogMsg2.Text = Convert.ToString(result.Rows[1]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[1]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[1]["botname"]);
                LogDate3.Text = Convert.ToString(result.Rows[2]["timestampvalue"]);
                LogMsg3.Text = Convert.ToString(result.Rows[2]["messagevalue"]) + " on Machine " + Convert.ToString(result.Rows[2]["machine"]) + " where Bot is " + Convert.ToString(result.Rows[2]["botname"]);
            }
            else if (0 == result.Rows.Count)
            {
                LogDate1.Text = "No Data Found";
                LogMsg1.Text = "No Data Found";
                LogDate2.Text = "No Data Found";
                LogMsg2.Text = "No Data Found";
                LogDate3.Text = "No Data Found";
                LogMsg3.Text = "No Data Found";
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Failed to get Log data from Database in MasterPage", "Exception : " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }
    #endregion
}
