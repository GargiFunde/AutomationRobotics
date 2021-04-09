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

public partial class DemoMasterPage2_AddGroup : System.Web.UI.Page
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'error',title: '" + (String)GetGlobalResourceObject("content", "LitSESSIONEXPIRED") + "',text:'" + (String)GetGlobalResourceObject("content", "LitYouareredirectedtoLoginPage") + "', showConfirmButton: false,  timer: 1500}).then((value) => {  window.location.href = 'LogIn.aspx';});", true);
                serviceInterface.insertLog("Exception: Session Expired from ProcessManagement Page", "Exception: Session Expired from ProcessManagement Page", 0, 0);

            }
            else
            {
                getdata();

                #region RoleBaseAccess

                try
                {
                    userName = (string)Session["UserName"];
                    groupId = Convert.ToInt32(Session["GroupId"]);
                    tenantId = Convert.ToInt32(Session["TenantId"]);
                    int roleid = Convert.ToInt32(Session["roleid"]);
                    db = serviceInterface.GetPageAccess(roleid, groupId, tenantId, "Queue Management");
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
                        DivAddQueue.Visible = false;
                    }

                    if (!EditAccess)
                    {
                        //No Edit Operation.
                    }

                    if (!DeleteAccess)
                    {
                        foreach (RepeaterItem item in Repeater1.Items)
                        {
                            ImageButton deleteQueue = item.FindControl("btnDeleteQueue") as ImageButton;
                            ImageButton PurgeQueue = item.FindControl("btnPurgeQueue") as ImageButton;

                            deleteQueue.Enabled = false;
                            deleteQueue.Attributes.CssStyle.Add("opacity", "0.5");
                            deleteQueue.ToolTip = "You dont have access to DELETE Queue. ";

                            PurgeQueue.Enabled = false;
                            PurgeQueue.Attributes.CssStyle.Add("opacity", "0.5");
                            PurgeQueue.ToolTip = "You dont have access to Purge Queue. ";
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






        //if (!IsPostBack)
        //{
        //    if (Session["GroupId"] == null || Session["TenantId"] == null || Session["Role"] == null)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
        //    }
        //    else
        //    {
        //        int groupid = (int)Session["GroupId"];
        //        int tenantid = (int)Session["TenantId"];

        //        getdata();

        //    }



        //}
        //else if (IsPostBack)
        //{
        //    if (Session["TenantId"] == null || Session["GroupId"] == null)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
        //    }
        //}
        //else
        //{
        //    if (Session["TenantId"] == null || Session["GroupId"] == null)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('SESSION EXPIRED', 'You are redirected to Login Page', 'error').then((value) => {  window.location.href = 'LogIn.aspx';}); ", true);
        //    }
        //}
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
    public void getdata()
    {
        DataTable result;
        ServiceInterface obj = new ServiceInterface();
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        result = obj.getQueueNames(tenantid, groupid);

        if (result != null)
        {
            Repeater1.DataSource = result;
            Repeater1.DataBind();

        }


    }

    #region API



    //async void HTTP_GET()
    //{
    //    var TARGETURL = "http://localhost:15672/api/queues";

    //    HttpClientHandler handler = new HttpClientHandler();

    //    Console.WriteLine("GET: + " + TARGETURL);


    //    HttpClient client = new HttpClient(handler);

    //    var byteArray = Encoding.ASCII.GetBytes("se:se");
    //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

    //    HttpResponseMessage response = await client.GetAsync(TARGETURL);
    //    HttpContent content = response.Content;

    //    Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);


    //    string result = await content.ReadAsStringAsync();

    //    JArray o = JArray.Parse(result);

    //    var test = from p in o
    //               select (string)p["name"];

    //    Arr = test.ToArray();



    //    getdata(Arr);

    //} 
    #endregion

    protected void openAddPopUp(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalAddpop();", true);
    }


    protected void btnQueueAction(object sender, CommandEventArgs e)
    {
        ServiceInterface obj = new ServiceInterface();
        int tenantid = (int)Session["TenantId"];
        int groupid = (int)Session["GroupId"];
        string commandArgs = e.CommandArgument.ToString();
        
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    lblQueueNameDelete.Text = commandArgs;
                    lblQueueNameDeleteSec.Text = commandArgs;
                    //obj.DeleteQueue((String)e.CommandArgument, tenantid, groupid);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
                    break;
                case "Purge":
                    QueueName.Text = commandArgs;
                    QueueNameper.Text = commandArgs;
                    obj.PurgeQueue((String)e.CommandArgument, tenantid);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalpurge();", true);
                    break;
            }
            getdata();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);

        }


    }

    protected void btnXdelete_clickHideBgPop(object sender, EventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }


    protected void ImageButton1_Command(object sender, CommandEventArgs e)
    {
        getdata();
    }






    protected void btnSaveQueue(object sender, EventArgs e)
    {
        string queueName = txtQueueName.Text.ToString();
        string exchangeName = txtExchangeName.Text.ToString();
        int groupid = (int)Session["GroupId"];
        int tenantid = (int)Session["TenantId"];
        int result = 0;
        ServiceInterface obj = new ServiceInterface();

        if (queueName == "" || exchangeName == "")
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + (String)GetGlobalResourceObject("content", "LitPleaseEnterrequiredfields") + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalAddpop();", true);
        }
        else
        {

            result = obj.AddQueue(queueName, exchangeName, tenantid, groupid);


            if (result != 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUENOTADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + " " + (String)GetGlobalResourceObject("content", "LitnotaddedSuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEADDEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + " " + (String)GetGlobalResourceObject("content", "Lithasbeenaddedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
                txtQueueName.Text = "";
                txtExchangeName.Text = "";
                getdata();
            }
        }
    }
    //protected void btnSaveQueue_Command(object sender, CommandEventArgs e)
    //{

    //    string queueName    = txtQueueName.Text.ToString();
    //    string exchangeName = txtExchangeName.Text.ToString();
    //    int groupid = (int)Session["GroupId"];
    //    int tenantid = (int)Session["TenantId"];
    //    int result = 0;
    //    ServiceInterface obj = new ServiceInterface();

    //    result = obj.AddQueue(queueName,exchangeName,tenantid,groupid); 


    //}
    protected void btnDelete_Click(object sender, CommandEventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDelete();", true);
    }

    protected void CommandBtnDelete_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalDeleteSecondPopUp();", true);

    }
    protected void ModalPopUpBtnClose_ClickSecondPopUp(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void ModalPopUpBtnDelete_Click(object sender, EventArgs e)
    {

        ServiceInterface obj = new ServiceInterface();
        int tenantid = (int)Session["TenantId"];
        int groupid = (int)Session["GroupId"];
        string queueName = lblQueueNameDeleteSec.Text;

        int result = obj.DeleteQueue(queueName, tenantid, groupid);

        if (result != 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + "" + (String)GetGlobalResourceObject("content", "Lithasbeendeletedsuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
            
            getdata();
        }
        else 
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'warning',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUENOTDELETEDSUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + "" + (String)GetGlobalResourceObject("content", "LithasFailedtoDelete") + "', showConfirmButton: false,  timer: 1500});", true);
            getdata();
        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: 'QUEUE DELETED SUCCESSFULLY',text:'Queue " + queueName + " deleted Successfully!', showConfirmButton: false,  timer: 1500});", true);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    protected void CommandBtnPurge_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalPurgeSecondPopUp();", true);

    }
    protected void ModalPopUpBtnPurge_Click(object sender, EventArgs e)
    {
        ServiceInterface obj = new ServiceInterface();
        int tenantid = (int)Session["TenantId"];
        int groupid = (int)Session["GroupId"];
        string queueName = lblQueueNameDeleteSec.Text;

       int result = obj.PurgeQueue(queueName, tenantid);
        if (result >= 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'fail',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUENOTPURGESUCCESSFULLY") + " ',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + "" + (String)GetGlobalResourceObject("content", "LitnotpurgeSuccessfully") + "', showConfirmButton: false,  timer: 1500});", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: '" + (String)GetGlobalResourceObject("content", "LitQUEUEPURGESUCCESSFULLY") + "',text:'" + (String)GetGlobalResourceObject("content", "LitsdQueueName") + " " + queueName + "" + (String)GetGlobalResourceObject("content", "LitPURGESuccessfully") + " ', showConfirmButton: false,  timer: 1500});", true);
            getdata();
        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", " Swal.fire({position: 'top-end',type: 'success',title: 'QUEUE DELETED SUCCESSFULLY',text:'Queue " + queueName + " deleted Successfully!', showConfirmButton: false,  timer: 1500});", true);
        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalHideDelete();", true);
    }
    
}