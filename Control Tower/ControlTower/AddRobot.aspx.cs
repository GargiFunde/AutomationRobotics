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
using System.Net;
using System.Net.NetworkInformation;
using CommonLibrary;

public partial class DemoMasterPage2_AddRobot : System.Web.UI.Page
{
    ServiceInterface serviceInterface = new ServiceInterface();
    EncryptionHelper encryptionHelper = new EncryptionHelper();
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
                serviceInterface.insertLog("Exception: Session Expired from AddRobot Page", "Exception: Session Expired from AddRobot Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
            else
            {
                LoadBots();
                resetTextboxData();

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Add Robot");
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
                        DivAddRobot.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in GrvBots.Items)
                        {
                            ImageButton deleteRobot = item.FindControl("btnDeleteRobot") as ImageButton;

                            deleteRobot.Enabled = false;
                            deleteRobot.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteRobot.ToolTip = "You dont have access to DELETE this Robot";
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
                serviceInterface.insertLog("Exception: Session Expired from AddRobot Page", "Exception: Session Expired from AddRobot Page", 0, 0);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
            }
        }
        else
        {
            if (Session["TenantId"] == null || Session["GroupId"] == null)
            {
                serviceInterface.insertLog("Exception: Session Expired from AddRobot Page", "Exception: Session Expired from AddRobot Page", 0, 0);
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

    public void LoadBots()
    {
        int groupid = 0;
        int tenantid = 0;
        if (Session["TenantId"] != null)
        {
            groupid = (int)Session["GroupId"];
            tenantid = (int)Session["TenantId"];
        }

        DataTable result = serviceInterface.GetBots(groupid, tenantid);
        result.Columns.Add(new DataColumn("styleClass", typeof(String)));
        result.Columns.Add(new DataColumn("connectionStatusMessage", typeof(String)));

        if (result != null)
        {


            #region Adding by Piyush for Status Checking
            foreach (DataRow row in result.Rows)
            {
                //string a = "IWPUNLPT0004"; //Here you will have to pass the machine name 
                try
                {
                    string a = row.Field<string>("MachineName");
                    string IPAddress = Dns.GetHostAddresses(a).GetValue(0).ToString();
                    Ping pinger = new Ping();
                    PingReply reply = pinger.Send(IPAddress);
                    bool pingable = reply.Status == IPStatus.Success;


                    if (pingable)
                    {
                        row["styleClass"] = "background-color:#00C853;width: 17px; height: 25px";
                        row["connectionStatusMessage"] = "Robot in Network";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "turnRed()", true);
                    }
                }
                catch (System.Net.Sockets.SocketException)
                {
                    row["connectionStatusMessage"] = "Robot NOT in Network";
                    row["styleClass"] = "background-color:red;width: 17px; height: 25px";
                }

                GrvBots.DataSource = result;
                GrvBots.DataBind();
            }
            #endregion
        }
    }

    protected void DrpGetuser()
    {

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string strBotName = txtBotName.Text.Trim();
        string strBotId = txtBotId.Text.Trim();
        string strPwd = txtPwd.Text.Trim();
        string strBotKey = Guid.NewGuid().ToString();
        string strMachineName = txtMachineName.Text.Trim();
        ServiceInterface serviceInterface = new ServiceInterface();
        string passkey = serviceInterface.getKey();
      //  string strBotIdEncrypted = encryptionHelper.Encrypt(strBotId, passkey);  //Machine Name will be Encrypted and stored to DB using a key already available in DB. 
        string strPwdEncrypted = encryptionHelper.Encrypt(strPwd, passkey);     // PWD doesnot have any dependency only the user name shoule be correct.

        int groupid = 0;
        int tenantid = 0;
        int result = 0;
        string strcurrentuser = string.Empty;

        try
        {
            if ((strBotName == "") || (strBotId == "") || (strPwd == ""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterbotdetails") + "');", true);
            }
            else
            {
                if (Session["TenantId"] != null)
                {
                    groupid = (int)Session["GroupId"];
                    tenantid = (int)Session["TenantId"];
                }
                strcurrentuser = (string)Session["UserName"];
                //   ServiceInterface serviceInterface = new ServiceInterface();
                result = serviceInterface.AddBot(strBotName, strBotId, strPwdEncrypted, strBotKey, strMachineName, groupid, tenantid, strcurrentuser);
                if (result > 0)
                {
                    serviceInterface.insertLog("Add Robot Successful", "Add Robot : Robot with Name : " + strBotName + " with ID : " + strBotId + " and MachineName : " + strMachineName + " has been added sucessfully by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitROBOTADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + " " + strBotName + "" + (String)GetGlobalResourceObject("content", "LitwithMachineName") + "  " + strMachineName + "" + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                    LoadBots();
                    resetTextboxData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitROBOTFAILEDTOADDED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + "" + strBotName + " " + (String)GetGlobalResourceObject("content", "LitwithMachineName") + "  " + strMachineName + " " + (String)GetGlobalResourceObject("content", "Lithasnotbeenadded") + "', showConfirmButton: false,  timer: 1500});", true);
                    serviceInterface.insertLog("Add Robot Successful", "Add Robot : Robot with Name : " + strBotName + " with ID : " + strBotId + " and MachineName : " + strMachineName + " has been added sucessfully by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                   
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Add Robot Failed", "Add Robot : Addition of Robot with Name : " + strBotName + " with ID : " + strBotId + " and MachineName : " + strMachineName + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid + " has failed. ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }

    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {
        DataTable result = null;
        int groupid = 0;
        int tenantid = 0;
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        string id = string.Empty;
        string botName = string.Empty;
        string machineName = string.Empty;
        string strcurrentuser = string.Empty;
        try
        {
            if (e.CommandName == "Delete")
            {
                if (Session["TenantId"] != null || Session["GroupId"] != null)
                {
                    groupid = (int)Session["GroupId"];
                    tenantid = (int)Session["TenantId"];
                    int cnt_ScheduleDeatils = 0;

                    commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    id = commandArgs[0];
                    botName = commandArgs[1];
                    machineName = commandArgs[2];
                    strcurrentuser = (string)Session["UserName"];

                    lblId.Text = id;
                    lblRobotName.Text = botName;
                    lblMachineName.Text = machineName;

                    lblRobotNameSecondPopUp.Text = botName;
                    lblIdSecondPopUp.Text = id;
                    lblMachineNameSecondPopUp.Text = machineName;

                    ServiceInterface serviceInterface = new ServiceInterface();
                    result = serviceInterface.GetCountScheduleRelatedBot(tenantid, botName);

                    if ((result.Rows[0][0] != null) && (result.Rows[0][0] != System.DBNull.Value))
                        cnt_ScheduleDeatils = (int)result.Rows[0][0];

                    lblScheduleDeatils.Text = cnt_ScheduleDeatils.ToString();

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Add Robot Failed", "Add Robot : Addition of Robot with Name : " + botName + " with ID : " + id + " and MachineName : " + machineName + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid + " has failed. Exception :" + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }
    }

    protected void GrvBots_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }


    protected void refreshRobots(object sender, CommandEventArgs e)
    {
        LoadBots();

    }

    protected void resetTextboxData()
    {
        /*To clear bot name text box*/
        txtBotName.Text = null;

        /*To clear machine name text box*/
        txtMachineName.Text = null;

        /*To clear bot userid text box*/
        txtBotId.Text = null;

        /*To clear bot password text box*/
        txtPwd.Text = null;
    }

    protected void BtnCancelRobot(object sender, EventArgs e)
    {
        resetTextboxData();
    }

    protected void ModalPopUpBtnDelete_ClickSecondPopUp(object sender, EventArgs e)
    {
        string botname = lblRobotName.Text;
        // string robotName = lblRobotName.Text;
        string machineName = lblMachineName.Text;
        string strcurrentuser = (string)Session["UserName"];
        int groupid = 0;
        int tenantid = 0;
        int result = 0;

        try
        {
            if (Session["TenantId"] != null || Session["groupid"] != null)
            {
                groupid = (int)Session["GroupId"];
                tenantid = (int)Session["TenantId"];

                result = serviceInterface.DeleteBot(botname, groupid, tenantid, strcurrentuser);
                if (result > 0)
                {

                    serviceInterface.insertLog("Delete Robot Successful", "Delete Robot : Robot with Name : " + botname + "  and MachineName : " + machineName + " has been deleted successfully by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid + " . ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitROBOTDELETEDSUCCESSFULLY") + "',text:' " + (String)GetGlobalResourceObject("content", "LitRobotrName") + "" + botname + "" + (String)GetGlobalResourceObject("content", "LitwithMachineName") + " " + machineName + " " + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    LoadBots();
                }
                else
                {
                    serviceInterface.insertLog("Delete Robot Failed", "Delete Robot : Robot with Name : " + botname + "  and MachineName : " + machineName + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid + " has failed . ", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitROBOTDELETEDFALED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitRobotrName") + "" + botname + "" + (String)GetGlobalResourceObject("content", "LitwithMachineName") + " " + machineName + " " + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + " ', showConfirmButton: false,  timer: 1500});", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                    LoadBots();
                }
            }
        }
        catch (Exception ex)
        {
            serviceInterface.insertLog("Exception : Delete Robot Failed", "Delete Robot : Robot with Name : " + botname + "  and MachineName : " + machineName + " by User : " + strcurrentuser + " with Tenant ID : " + tenantid + " and Group ID : " + groupid + " has failed .Exception: " + ex.Message, Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }

    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }

    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);
    }
    protected string GetItemStyle(int itemIndex)
    {
        if (itemIndex == 0)
        //return "first btn btn-circle btn-primary";
        //else if (itemIndex == this.ItemCount - 1)
        //    return "last";
        {

            return "background-color: #64DD17;width: 17px; height: 25px";
        }
        else
            return "background-color: #D50000;width: 17px; height: 25px";
    }


}