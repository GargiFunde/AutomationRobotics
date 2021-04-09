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
using System.Windows.Forms;

public partial class DemoMasterPage2_Configurations : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();
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
                serviceInterface.insertLog("Exception: Session Expired from Configurations Page", "Exception: Session Expired from Configurations Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                LoadProcesses();
                int groupid = 0;
                int tenantid = 0;
                if (Session["TenantId"] != null)
                {
                    groupid = (int)Session["GroupId"];
                    tenantid = (int)Session["TenantId"];
                }
                getdata(groupid, tenantid);

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Configuration Management");
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
                        DivConfiguarationManagement.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GVConfigParameters.Items)
                        {
                            ImageButton deleteConfiguration = item.FindControl("btnDeleteConfiguration") as ImageButton;

                            deleteConfiguration.Enabled = false;
                            deleteConfiguration.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteConfiguration.ToolTip = "You dont have access to DELETE Configuration Mapping. ";

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
                serviceInterface.insertLog("Exception: Session Expired from Configurations Page", "Exception: Session Expired from Configurations Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from Configurations Page", "Exception: Session Expired from Configurations Page", 0, 0);
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        int iAccessProcessId = 0;
        string strParameterName = txtParameterName.Text.Trim();
        string strParameterValue = txtParameterValue.Text.Trim();
             
        if (DrpProcesses.SelectedValue != null)
        {
            iAccessProcessId = Convert.ToInt32(DrpProcesses.SelectedValue);
        }
       
        if ((strParameterName == "") || (strParameterValue == ""))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
        }
        else
        {
            string strcurrentuser = (string)Session["UserName"];
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.AddConfigParameters(strParameterName, strParameterValue, iAccessProcessId, groupid, tenantid, strcurrentuser);

            if (result > 0)
            {
                serviceInterface.insertLog("Add Configuration Parameter", "Add Configuration Parameters : Configuration Parameters with ParameterName : " + strParameterName + " ParameterValue : " + strParameterValue + " and AccessProcessId : " + iAccessProcessId + " has been added Successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID : " + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitCONFIGURATIONADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitConfigurationName") + " " + strParameterName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
            }
            else
            {
                serviceInterface.insertLog("Add Configuration Parameter", "Add Configuration Parameters : Addition of Configuration Parameters with ParameterName : " + strParameterName + " ParameterValue : " + strParameterValue + " and AccessProcessId : " + iAccessProcessId + " by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID : " + tenantid + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOADDCONFIGURATION") + "',text:'" + (String)GetGlobalResourceObject("content", "LitConfigurationName") + " " + strParameterName + "" + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + " ', showConfirmButton: false,  timer: 1500});", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('CONFIGURATION NOT ADDED SUCCESSFULLY', 'Configuration Name " + strParameterName + " has not been added!', 'error'); ", true);
            }
            getdata(groupid, tenantid);
            clear();
        }
    }
    public void getdata(int groupid, int tenantid)
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetConfigParameters(groupid, tenantid);

        if (result != null)
        {

            GVConfigParameters.DataSource = result;
            GVConfigParameters.DataBind();
        }

    }
    public void LoadProcesses()
    {
        try
        {
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            ServiceInterface serviceInterface = new ServiceInterface();
            DataTable result = serviceInterface.GetProcessDataWitoutFile(groupid, tenantid);

            if (result != null)
            {
                DrpProcesses.DataSource = result;
                DrpProcesses.DataValueField = "processid";
                DrpProcesses.DataTextField = "processname";
                DrpProcesses.DataBind();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //throw;
        }
        
    }
    protected void GrvBots_RowCommand(object sender, GridViewCommandEventArgs e)
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

            int iParameterId = Convert.ToInt32(e.CommandArgument.ToString());
            string strcurrentuser = (string)Session["UserName"];

            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteConfigParameters(iParameterId, groupid, tenantid, strcurrentuser);
            if (0 < result)
            {
                serviceInterface.insertLog("Delete Configuration Parameter Successful", "Delete Configuration Parameter : Configuration Parameter with Parameter Id : " + iParameterId + " has been deleted successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getdata(groupid, tenantid);
            }
            else
            {
                serviceInterface.insertLog("Delete Configuration Parameter Failed", "Delete Configuration Parameter  : Deletion of Configuration Parameter with Parameter Id : " + iParameterId + " by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid + "has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
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
                string parameterName = commandArgs[1];
                string parameterValue = commandArgs[2];

                lblParameterId.Text = id;
                lblParameterName.Text = parameterName;
                lblParameterValue.Text = parameterValue;

                lblParameterIdSecondPopUp.Text = id;
                lblParameterNameSecondPopUp.Text = parameterName;
                lblParameterValueSecondPopUp.Text = parameterValue;

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);

            }
            else
            {
                Session["GroupId"] = null;
                Session["TenantId"] = null;
                serviceInterface.insertLog("Exception: Session Expired from Configurations Page", "Exception: Session Expired from Configurations Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        catch (Exception exception)
        {
        }
        if (e.CommandName == "DeleteParameter")
        {
            int groupid = 0;
            int tenantid = 0;
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }

            int iParameterId = Convert.ToInt32(e.CommandArgument.ToString());
            string strcurrentuser = (string)Session["UserName"];

            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteConfigParameters(iParameterId, groupid, tenantid, strcurrentuser);
            if (result == 1)
            {
                serviceInterface.insertLog("Delete Configuration Parameter Successful", "Delete Configuration Parameter : Configuration Parameter with Parameter Id : " + iParameterId + " has been deleted successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                getdata(groupid, tenantid);
            }
            else
            {
                serviceInterface.insertLog("Delete Configuration Parameter Failed", "Delete Configuration Parameter  : Deletion of Configuration Parameter with Parameter Id : " + iParameterId + " by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid + "has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
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
            string id = lblParameterId.Text;
            int iParameterId= Convert.ToInt32(id);
            string parameterName=lblParameterName.Text;
            string strcurrentuser = (string)Session["UserName"];
            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteConfigParameters(iParameterId, groupid, tenantid, strcurrentuser);
            if (result > 0)
            {
                serviceInterface.insertLog("Delete Configuration Parameter Successful", "Delete Configuration Parameter : Configuration Parameter with Parameter Id : " + iParameterId + " and ParameterName : " + parameterName + " has been deleted successfully by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitConfigurationName") + " " + parameterName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + id + "" + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                getdata(groupid, tenantid);
            }
            else
            {
                serviceInterface.insertLog("Delete Configuration Parameter Failed", "Delete Configuration Parameter  : Deletion of Configuration Parameter with Parameter Id : " + iParameterId + " and ParameterName : " + parameterName + " by User : " + strcurrentuser + " with Group ID : " + groupid + " Tenant ID :" + tenantid + "has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETECONFIGURATION") + " ',text:'" + (String)GetGlobalResourceObject("content", "LitConfigurationName") + "  " + parameterName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + id + " " + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "', showConfirmButton: false,  timer: 1500});", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                getdata(groupid, tenantid);
            }
        }
        catch (Exception exception)
        {

        }

    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }

    protected void ImageButton3_Command(object sender, CommandEventArgs e)
    {
        LoadProcesses();
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }
        getdata(groupid, tenantid);

    }

    protected void clear()
    {
        txtParameterName.Text = "";
        txtParameterValue.Text = "";

        if (DrpProcesses.SelectedValue != "0")
        {
            DrpProcesses.SelectedValue = "0";
        }
    }

    protected void BtnCancelConfiguration(object sender, EventArgs e)
    {

        clear();
    }
}