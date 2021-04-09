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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

public partial class DemoMasterPage2_ProcessManagement : System.Web.UI.Page
{
    private object litMsg;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: Session Expired from ProcessManagement Page", 0, 0);

            }
            else
            {
                loadProcesses();
                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Process Management");
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
                        IDaddUploadProcess.Visible = false;
                        foreach (RepeaterItem item in GVUserToBotMapping.Items)
                        {
                            ImageButton uploadProcess = item.FindControl("Upload") as ImageButton;

                            uploadProcess.Enabled = false;
                            uploadProcess.Attributes.CssStyle.Add("opacity", "0.5");
                            uploadProcess.ToolTip = "You dont have access to UPLOAD any Process";
                        }
                    }

                    if (!EditAccess)
                    {
                        foreach (RepeaterItem item in GVUserToBotMapping.Items)
                        {
                            ImageButton updateVersion = item.FindControl("btnVersion") as ImageButton;

                            updateVersion.Enabled = false;
                            updateVersion.Attributes.CssStyle.Add("opacity", "0.5");
                            updateVersion.ToolTip = "You dont have access to Update any Process";
                        }
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GVUserToBotMapping.Items)
                        {
                            ImageButton deleteProcess = item.FindControl("btnShow") as ImageButton;

                            deleteProcess.Enabled = false;
                            deleteProcess.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteProcess.ToolTip = "You dont have access to DELETE this Process";
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
                serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: Session Expired from ProcessManagement Page", 0, 0);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: Session Expired from ProcessManagement Page", 0, 0);
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
    public void loadProcesses()
    {

        try
        {
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];

            getProcesses(groupid, tenantid);
        }
        catch (Exception e)
        {
            //write code to logout
            Response.Redirect("Login.aspx");
            serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: " + e.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }


    public void getProcesses(int groupid, int tenantid)
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetProcessDataWithFile(groupid, tenantid);

        if (result != null)
        {
            GVUserToBotMapping.DataSource = result;
            GVUserToBotMapping.DataBind();
        }
        else
        {
            Debug.Write("Error in Database");
        }

    }

    protected void SaveZIP_Click(object sender, EventArgs e)
    {


        string filename = null;
        string ProcessName = ""; /*Process Name*/
        int groupid = 0;
        int tenantid = 0;
        string userid = string.Empty;


        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
            userid = (string)Session["UserName"];
        }

        string ProcessVersion = ""; /*Process Version*/
        bool LatestVersion = false; /*Latest Version set to false by default.*/
                                    //string DefaultVersion = "";
        string createdBy = "";

        if (true == CheckBox1.Checked)
        {
            LatestVersion = true;
        }
        else
        {
            LatestVersion = false;
        }

        createdBy = userid;

        ServiceInterface serviceInterface = new ServiceInterface();


        filename = Path.GetFileName(FileUploadZip.PostedFile.FileName);
        string contentType = FileUploadZip.PostedFile.ContentType;
        string[] ZipFileName = filename.Split(new char[] { '.' }, 2);

        try
        {
            ProcessName = ZipFileName[0].ToString();
            ProcessVersion = ZipFileName[1];
            ProcessVersion = ProcessVersion.Substring(0, ProcessVersion.Length - 4).ToString();
            byte[] ZipDataFile;

            using (Stream fs = FileUploadZip.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    ZipDataFile = br.ReadBytes((Int32)fs.Length);
                }
            }

            int result = 0; //Assigning a default value to result.
            result = serviceInterface.AddProcessWithZip(ProcessName, groupid, tenantid, ProcessVersion, LatestVersion, createdBy, ZipDataFile);

            if (1 == result)
            {
                
                //
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + ProcessName + " " + (String)GetGlobalResourceObject("content", "LitwithVersion") + " " + ProcessVersion + " " + (String)GetGlobalResourceObject("content", "Lithasbeenuploadedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Process Upload Successful", "Upload Process: Process " + ProcessName + " with Process Version" + ProcessVersion + " has been uploaded Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                loadProcesses();
                //Response.Write("<script>location.href='ProcessManagement.aspx'</script>");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Process Upload Failed", "Upload Process: Upload of Process " + ProcessName + " with Process Version" + ProcessVersion + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + (String)GetGlobalResourceObject("content", "LitNOPROCESSUPDATED") + "', '" + (String)GetGlobalResourceObject("content", "LitTherewasanExceptionwhileUploadingaProcessPleasecontactAdmin") + "', 'error').then((value) => { ProcessEmpty(); }); ", true);
            serviceInterface.insertLog("Process Upload Failed", "Upload Process: Upload of Process " + ProcessName + " with Process Version" + ProcessVersion + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }

    protected void ReloadProcesses(object sender, CommandEventArgs e)
    {
        loadProcesses();
    }

    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {

        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        string strid = commandArgs[0];
        string strProcessName = commandArgs[1];

        lblProcessId.Text = strid;
        lblProcessName.Text = strProcessName;
        lblProcessIdSecondPopUp.Text = strid;
        lblProcessNameSecondPopUp.Text = strProcessName;

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
    }

    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        int groupid = 0;
        int tenantid = 0;
        string strid = string.Empty;
        string StrProcessName = string.Empty;
        string strcurrentuser = string.Empty;
        try
        {
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            strid = lblProcessId.Text;
            StrProcessName = lblProcessName.Text;
            strcurrentuser = (string)Session["UserName"];
            ServiceInterface serviceInterface = new ServiceInterface();
            int result = serviceInterface.DeleteProcess(strid, groupid, tenantid, strcurrentuser);
            if (result == 1)
            {
                loadProcesses();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitPROCESSDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + StrProcessName + " " + (String)GetGlobalResourceObject("content", "LitwithVersion") + "  " + strid + " " + (String)GetGlobalResourceObject("content", "Lithasbeenuploadedsuccessfully!") + "  ', showConfirmButton: false,  timer: 1500});", true);
                serviceInterface.insertLog("Process Deletion Successful", "Delete Process: Process " + StrProcessName + " with Process Version" + strid + " has been deleted Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                alertDeletedSuccess.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTODELETEPROCESS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + StrProcessName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + strid + " " + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + "', showConfirmButton: false,  timer: 1500});", true);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                serviceInterface.insertLog("Process Deletion Failed", "Delete Process: Deletion of Process " + StrProcessName + " with Process Version" + strid + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
        catch (Exception exception)
        {
            serviceInterface.insertLog("Process Deletion Failed", "Delete Process: Deletion of Process " + StrProcessName + " with Process Version" + strid + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }

    }

    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void btnDelete_ClickUpdateVersion(object sender, CommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        string strid = commandArgs[0];
        string strProcessName = commandArgs[1];
        string strProcessVersion = commandArgs[2];

        lblProcessIdUpdateVersion.Text = strid;
        lblProcessNameUpdateVersion.Text = strProcessName;
        lblProcessVersion.Text = strProcessVersion;

        lblProcessIdUpdateVersionSecondPopUp.Text = strid;
        lblProcessNameUpdateVersionSecondPopUp.Text = strProcessName;
        lblProcessVersionSecondPopUp.Text = strProcessVersion;

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteUpdateVersion();", true);
    }

    protected void ModalPopUpBtnClose_ClickUpdateVersion(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);

    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void ModalPopUpBtnDelete_ClickSecondPopUpDeleteUpdateVersion(object sender, EventArgs e)
    {
        int groupid = 0;
        int tenantid = 0;
        string strProcessId = string.Empty;
        string strProcessName = string.Empty;
        string strProcessVersion = string.Empty;
        try
        {
           
            if (Session["TenantId"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];
            }
            strProcessId = lblProcessIdUpdateVersion.Text;
            strProcessName = lblProcessNameUpdateVersion.Text;
            strProcessVersion = lblProcessVersion.Text;

            string createdby = (string)Session["UserName"];
            ServiceInterface serviceInterface = new ServiceInterface();


            int result = serviceInterface.DeleteProcessVersion(strProcessId, strProcessName, strProcessVersion, groupid, tenantid, createdby);
            if (0 < result)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitSwalDELETEPROCESS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + strProcessName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + strProcessId + " " + (String)GetGlobalResourceObject("content", "LitwithVersion") + " " + strProcessVersion + "" + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                serviceInterface.insertLog("Process Version Deletion Successful", "Delete Process Version: Process Version " + strProcessVersion + " of Process " + strProcessName + " has been deleted Successfully", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                loadProcesses();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSwalDELETEPROCESS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + "  " + strProcessName + "" + (String)GetGlobalResourceObject("content", "LitwithID") + " " + strProcessId + "" + (String)GetGlobalResourceObject("content", "LitwithVersion") + "" + strProcessVersion + " " + (String)GetGlobalResourceObject("content", "Litcouldnotbedeleted") + " ', showConfirmButton: false,  timer: 1500});", true);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                serviceInterface.insertLog("Process Version Deletion Failed", "Delete Process Version: Deletion of Process Version " + strProcessVersion + " of Process " + strProcessName + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                loadProcesses();
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Process Version Deletion Failed", "Delete Process Version: Deletion of Process Version " + strProcessVersion + " of Process " + strProcessName + " has failed", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }

    protected void ModalPopUpBtnDelete_ClickUpdateVersion(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUpUpdateVersion();", true);
    }

    string value = string.Empty;
    protected void ddlArea_DataBound(object sender, EventArgs e)
    {
        DropDownList1.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        value = DropDownList1.SelectedValue;
    }

    protected void btnUpdateZip_Click(object sender, EventArgs e)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];

        string ProcessId = Pid.Text;
        LoadProcessVersion(tenantid, groupid, ProcessId);

        string updatedVersion = string.Empty;
        bool isLatest = false;
        updatedVersion = value;
        isLatest = CheckBox1.Checked;
        ServiceInterface serviceInterface = new ServiceInterface();
        //int result = serviceInterface.UpdateDefaultVersion(groupid, tenantid, ProcessId, updatedVersion, isLatest);

        if ("" == updatedVersion)
        {
            lblErrorMsg.Text = "Please select version";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        else {
            int result = serviceInterface.UpdateDefaultVersion(groupid, tenantid, ProcessId, updatedVersion, isLatest);
            if (0 < result)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitPROCESSUPLOADEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + Pname.Text + "  " + (String)GetGlobalResourceObject("content", "LitwithID") + " " + ProcessId + " " + (String)GetGlobalResourceObject("content", "Lithasbeenupdatedwithitsversion ") + " " + updatedVersion + " !', showConfirmButton: false,  timer: 1500});", true);

                loadProcesses();
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                serviceInterface.insertLog("Process Version Upload Successful", "Upload Process Version: Process " + Pname.Text + " with Process Version" + updatedVersion + " has been uploaded successfully.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitFAILEDTOUPLOADPROCESS") + "',text:'" + (String)GetGlobalResourceObject("content", "LitProcess") + " " + Pname.Text + "  " + (String)GetGlobalResourceObject("content", "LitwithID") + " " + ProcessId + " " + (String)GetGlobalResourceObject("content", "Lithasnotbeenupdatedwithitsversion ") + " " + updatedVersion + " !', showConfirmButton: false,  timer: 1500});", true);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                serviceInterface.insertLog("Process Version Upload Failed", "Upload Process Version: Process " + Pname.Text + " with Process Version" + updatedVersion + " has failed.", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            }
        }
        
    }

    public void LoadProcessVersion(int groupid, int tenantid, String ProcessId)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.LoadProcessVersion(groupid, tenantid, ProcessId);

        if (result != null)
        {
            DropDownList1.DataSource = result;
            DropDownList1.DataTextField = "processversion";
            DropDownList1.DataValueField = "processversion";
            DropDownList1.DataBind();
        }
        else
        {

        }
    }

    public void LoadProcessesforVersion(object sender, CommandEventArgs e)
    {
        try
        {
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            ImageButton objImage = (ImageButton)sender;

            string[] commandArgs = objImage.CommandArgument.ToString().Split(new char[] { ',' });
            string ProcessId = commandArgs[0];
            string ProcessName = commandArgs[1];
            Pid.Text = ProcessId;
            Pname.Text = ProcessName;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            getProcesses(groupid, tenantid, ProcessId);

            LoadProcessVersion(groupid, tenantid, ProcessId);
            LoadProcessWithSameName(groupid, tenantid, ProcessId);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: " + exception.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            Response.Redirect("LogInPage.aspx");

            throw;
        }
    }

    public void LoadProcessWithSameName(int groupid, int tenantid, String ProcessId)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.LoadProcessVersion(groupid, tenantid, ProcessId);
    }

    public void getProcesses(int groupid, int tenantid, string ProcessId)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.LoadProcessVersion(groupid, tenantid, ProcessId);

        if (result != null)
        {
            GVForVersion.DataSource = result;
            GVForVersion.DataBind();
        }
        else
        {

        }
    }

    private void LoadMapping(string strUserId)
    {
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];

        if (strUserId != "")
        {
            ServiceInterface serviceInterface = new ServiceInterface();
            DataTable result = serviceInterface.GetUserToBotMapping(strUserId, groupid, tenantid);
        }
    }

    protected string GetProcessIDForVersion(object sender, CommandEventArgs e)
    {

        return null;
    }
    protected void ModalPopUpBtnClose_ClickPopUp(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
        //Response.Redirect(Request.RawUrl, false);
    }
    protected void btnEdit_Click(object sender, CommandEventArgs e)
    {
        string store = Convert.ToString(e.CommandArgument);
        try
        {
            int groupid = (int)Session["GroupId"];
            int tenantid = (int)Session["TenantId"];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: " + exception.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
            Response.Redirect("LogInPage.aspx");
            throw;
        }
    }

}