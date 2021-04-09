/*
 .*********************BotService.svc : Web Service to be hosted.********************* 
 */

using CommonLibrary;
using RabbitMQ.Client;
using RobotLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ServiceModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Channels;
//using System.ServiceModel;                   // For Token Based Authentication
//using Microsoft.AspNet.SignalR.Client;

namespace AutomationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    //Implementing service as PerSession instance
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BOTService : IBOTService
    {
        //To hold user token returned from WCF service after validating user
        //string UserToken = string.Empty; //Added by Piyush for checking Token Based Authentication.

        //Constructor
        public BOTService()
        {
            //  log4net.Config.XmlConfigurator.Configure();
            //   ThreadInvoker.Instance.InitDispatcher();
            QuartzHelper.InitializeQuartz();
            quartzHelper = new QuartzHelper();
            if (Connection == null || Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
            {
                connectAsync();
            }
        }



        #region "Listener"
        public bool AutomationRequest(RequestInput _requestInput)
        {
            string strExchangeTopic = ConfigurationManager.AppSettings["ExchangeTopicName"];
            string strRoutingLevel = ConfigurationManager.AppSettings["RoutingSupport"];

            string routingKey = string.Empty;
            #region RequestValidator

            //int ErrorResult = Validator.RequestValidator.ValidateRequest(_requestInput);
            //if (ErrorResult != 0)
            //{
            //    //give error and return;
            //    return false;
            //}

            #endregion RequestValidator

            DataTable dt = GetRQDetailsByName(_requestInput.AutomationGroupName, _requestInput.TenantName);

            if (dt.Rows.Count > 0)
            {
                string servername = dt.Rows[0][0].ToString();

                var factory = new ConnectionFactory() { };
                factory.UserName = dt.Rows[0][2].ToString();
                factory.Password = dt.Rows[0][3].ToString();
                factory.HostName = dt.Rows[0][0].ToString();
                factory.Port = (int)dt.Rows[0][1];

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    _requestInput.UniqueRequestNumber = "MsgReqNo_" + DateTime.Now.Ticks.ToString();

                    if (strRoutingLevel == "ProfileLevel")
                    {
                        routingKey = _requestInput.TenantName + "." + _requestInput.AutomationGroupName;
                    }
                    else
                    {
                        routingKey = _requestInput.TenantName + "." + _requestInput.AutomationGroupName + "." + _requestInput.AutomationProcessName;
                    }

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = 2; //persistent
                    props.Headers = new Dictionary<string, object>();
                    props.Headers["source_message_id"] = "MsgReqNo_" + DateTime.Now.Ticks.ToString();

                    #region CalculateQueueDepth

                    #endregion CalculateQueueDepth

                    var message = XMLHelper.Serialize(_requestInput);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: strExchangeTopic,
                                         routingKey: routingKey,
                                         basicProperties: props,
                                         body: body);
                }
                return true; //acknowledgement
            }
            return false;
        }
        #endregion

        string strDeploymentFolderPath = string.Empty;
        private QuartzHelper quartzHelper;

        private SqlConnection GetConnection()
        {
            string connstring = ConfigurationManager.ConnectionStrings["ConnectionStringSql"].ConnectionString;
            strDeploymentFolderPath = ConfigurationManager.AppSettings["DeploymentFolderPath"];
            // Making connection with Npgsql provider
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            return conn;
        }


        #region SignalR Code writeen on 05-Dec-2019
        //Method connectAsync Related to SignalR
        private Microsoft.AspNet.SignalR.Client.IHubProxy HubProxy { get; set; }
        //const string ServerURI = "http://localhost:3455/signalr";
        const string ServerURI = "http://localhost:3455";
        //const string ServerURI = "https://e2ebots.com:443";
        private Microsoft.AspNet.SignalR.Client.HubConnection Connection { get; set; }

        //Call from Constructor
        private async void connectAsync()
        {
            Connection = new Microsoft.AspNet.SignalR.Client.HubConnection(ServerURI);
            // Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread

            try
            {
                await Connection.Start();
            }
            //catch (HttpRequestException)
            catch (Exception ex)
            {
                return;
            }
        }

        public void sendMessageToClient()  //Receive from Windows Service
        {
            try
            {
                //client.Value.BroadcastToClient(eventData);
                if (Connection != null && Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    // HubProxy.Invoke("Send", eventData.ClientName, eventData.EventMessage);
                    HubProxy.Invoke("Send");
                }
                else
                {
                    Console.WriteLine("Connection Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Exception : " + ex.Message);
            }
        }
        //#endregion

        public int DeleteProcessSchedule(string strId, string strprocessename, string strBotName, string ChronExp, int groupid, int tenantid, string createdby)
        {
            try
            {
                string strChronExp = String.Empty;
                strChronExp = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(ChronExp);

                if (quartzHelper == null)
                {
                    quartzHelper = new QuartzHelper();
                }
                string strMachineName = Environment.MachineName;
                quartzHelper.DeleteJob(strMachineName, strprocessename, strBotName, strChronExp, tenantid);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {

                    try
                    {
                        SqlCommand command = new SqlCommand("um_DeleteProcessSchedule", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "scheduleid";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.Int32;
                        parameter.Value = strId;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@processeName";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strprocessename;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botName";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strBotName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "chronExpression";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = ChronExp;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);

                        parameter = command.CreateParameter();

                        command = AddAuditParamCreatedBy(command, createdby);

                        i = command.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return i;
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Delete Schedule Exception : " + ex.Message);
                return 0; //fail
            }

        }
        public DataTable GetProcessSchedules(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getprocessschedules", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }



        #region ChangeGroup
        public int ChangeGroup(string currentRoleCG, string userName, string newRole, int tenantid)
        {

            int result = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_changegroup", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@currentuserrole";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = currentRoleCG;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@newrole";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = newRole;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@username";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userName;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@TenantID";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);



                    result = command.ExecuteNonQuery();


                }
                catch (Exception ex)
                {


                    Console.WriteLine("Error :-", ex.StackTrace);
                }
            }



            return result;


        }
        #endregion

      

        /*Role Based Access*/
        public DataTable GetRoleBaseAccess(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getrolebaseaccess", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                }
                catch (Exception e)
                {

                }
            }
            return dt;
        }

        /*User Based Access*/
        public DataTable GetUserBaseAccess(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getuserrolebaseaccess", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                }
                catch (Exception ex)
                {

                }
            }
            return dt;
        }

        public int CheckScheduleStatus(String botname, int tenantid)
        {
            int statusResult = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_CheckScheduleStatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);

                    SqlParameter prm1 = new SqlParameter("@status", SqlDbType.Int);
                    prm1.Direction = ParameterDirection.Output;
                    command.Parameters.Add(prm1);

                    command.ExecuteNonQuery();

                    statusResult = (int)(command.Parameters["@status"].Value);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return -1;
                }

                return statusResult;
            }
        }

        public DataTable GetCountScheduleRelatedBot(int tenantid, string botName)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getCountScheduleRelatedBot", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botName;
                    command.Parameters.Add(parameter);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        #region SaveRoleBaseAccess Old
        public int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bAddQueueR, bool bAddQueueC, bool bAddQueueE, bool bAddQueueD, bool bAddGroupR, bool bAddGroupC, bool bAddGroupE, bool bAddGroupD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bAddUpdateProcessR, bool bAddUpdateProcessC, bool bAddUpdateProcessE, bool bAddUpdateProcessD, bool bUploadProcessR, bool bUploadProcessC, bool bUploadProcessE, bool bUploadProcessD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
        {
            int result = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_saverolebaseaccessdetails", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserD;
                    command.Parameters.Add(parameter);


                    #region Add Robot

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotD;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotE;
                    command.Parameters.Add(parameter);
                    #endregion


                    #region Add Queue


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddQueueR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddQueueR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddQueueC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddQueueC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddQueueE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddQueueE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddQueueD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddQueueD;
                    command.Parameters.Add(parameter);
                    #endregion


                    #region Add Group

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddGroupR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddGroupR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddGroupC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddGroupC;
                    command.Parameters.Add(parameter);




                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddGroupE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddGroupE;
                    command.Parameters.Add(parameter);





                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddGroupD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddGroupD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region QueueToBot

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region BotLOg
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region Audit Trail

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailR;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region ScheduleDetails
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsE;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsD;
                    command.Parameters.Add(parameter);

                    #endregion
                    #region Add  Update process

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUpdateProcessR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUpdateProcessR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUpdateProcessC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUpdateProcessC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUpdateProcessE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUpdateProcessE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUpdateProcessD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUpdateProcessD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region Configuration
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region UploadProcess

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bUploadProcessR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bUploadProcessR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bUploadProcessC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bUploadProcessC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bUploadProcessE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bUploadProcessE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bUploadProcessD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bUploadProcessD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region PromoteDemote
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region BotUSER
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserD;
                    command.Parameters.Add(parameter);
                    #endregion

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "role";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = role;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);


                    result = command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }
                catch (Exception ex)
                {


                    Console.WriteLine(ex.StackTrace);

                }



            }

            return result;
        }
        #endregion

        public int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
        {
            int result = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_saverolebaseaccessdetails", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotDashboardD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotDashboardD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bqueueD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bqueueD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddScheduleD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddScheduleD;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddUserD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddUserD;
                    command.Parameters.Add(parameter);


                    #region Add Robot

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotD;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAddRobotE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAddRobotE;
                    command.Parameters.Add(parameter);
                    #endregion


                    #region Queue management
                    //Changes in Sql

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueManagementR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bQueueManagementR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueManagementC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bQueueManagementC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueManagementE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bQueueManagementE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bQueueManagementD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bQueueManagementD;
                    command.Parameters.Add(parameter);
                    #endregion




                    #region QueueToBot

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignQueueBotD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignQueueBotD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region BotLOg
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bBotLogD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bBotLogD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region Audit Trail

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailR;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAuditTrailD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAuditTrailD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region ScheduleDetails
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsC;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsE;
                    command.Parameters.Add(parameter);



                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bScheduleDetailsD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bScheduleDetailsD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region Add  Update process/Process Management

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bProcessManagementR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bProcessManagementR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bProcessManagementC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bProcessManagementC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bProcessManagementE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bProcessManagementE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bProcessManagementD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bProcessManagementD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region Configuration
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bConfigurationD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bConfigurationD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region Detail Log


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bDetailLogR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bDetailLogR;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bDetailLogC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bDetailLogC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bDetailLogE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bDetailLogE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bDetailLogD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bDetailLogD;
                    command.Parameters.Add(parameter);
                    #endregion

                    #region PromoteDemote
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteE;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bPromoteDemoteD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bPromoteDemoteD;
                    command.Parameters.Add(parameter);

                    #endregion

                    #region BotUSER
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserR";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserR;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserC";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserC;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserE";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserE;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "bAssignBotUserD";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = bAssignBotUserD;
                    command.Parameters.Add(parameter);
                    #endregion

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "role";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = role;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);


                    result = command.ExecuteNonQuery();
                    command.Parameters.Clear();

                }
                catch (Exception ex)
                {
                }

            }

            return result;
        }
        /*Description : This method is gets the data/Access for Perticular user      **/
        public DataTable GetPageAccess(int groupid, int tenantid, string username, string pagename)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("um_getpageaccess", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();   // this is extra parameter here 
                    parameter.ParameterName = "username";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = username;
                    command.Parameters.Add(parameter);


                    parameter = command.CreateParameter();
                    parameter.ParameterName = "pagename";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = pagename;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];


                }

            }
            catch (Exception e)
            {
                //remove in production
                Debug.WriteLine(e.StackTrace);
            }

            return dt;
        }

        /*Purpose : Below Methods are  Are for user base Access **/
        public DataTable GetPageAccessUser(int groupid, int tenantid, string username)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("um_getpageaccessuser", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "username";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = username;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];


                }

            }
            catch (Exception e)
            {
                //remove in production
                Debug.WriteLine(e.StackTrace);
            }

            return dt;
        }

        //Innovative New method for User Base Access

        #region SaveRoleBaseAccessUser on 26th Nov 2019
        //public int SaveRoleBaseAccessUser(int Igroupid, string usernameI, int Itenantid, bool bbBotDashboardR, bool bbBotDashboardC, bool bbBotDashboardE, bool bbBotDashboardD, bool bbqueueR, bool bbqueueC, bool bbqueueE, bool bbqueueD, bool bbAddScheduleR, bool bbAddScheduleC, bool bbAddScheduleE, bool bbAddScheduleD, bool bbAddUserR, bool bbAddUserC, bool bbAddUserE, bool bbAddUserD, bool bbAddRobotR, bool bbAddRobotC, bool bbAddRobotE, bool bbAddRobotD, bool bbAddQueueR, bool bbAddQueueC, bool bbAddQueueE, bool bbAddQueueD, bool bbAddGroupR, bool bbAddGroupC, bool bbAddGroupE, bool bbAddGroupD, bool bbAssignQueueBotR, bool bbAssignQueueBotC, bool bbAssignQueueBotE, bool bbAssignQueueBotD, bool bbAssignBotUserR, bool bbAssignBotUserC, bool bbAssignBotUserE, bool bbAssignBotUserD, bool bbBotLogR, bool bbBotLogC, bool bbBotLogE, bool bbBotLogD, bool bbAuditTrailR, bool bbAuditTrailC, bool bbAuditTrailE, bool bbAuditTrailD, bool bbScheduleDetailsR, bool bbScheduleDetailsC, bool bbScheduleDetailsE, bool bbScheduleDetailsD, bool bbConfigurationR, bool bbConfigurationC, bool bbConfigurationE, bool bbConfigurationD, bool bbAddUpdateProcessR, bool bbAddUpdateProcessC, bool bbAddUpdateProcessE, bool bbAddUpdateProcessD, bool bbUploadProcessR, bool bbUploadProcessC, bool bbUploadProcessE, bool bbUploadProcessD, bool bbPromoteDemoteR, bool bbPromoteDemoteC, bool bbPromoteDemoteE, bool bbPromoteDemoteD)
        //{
        //    int result = 0;
        //    using (SqlConnection conn = GetConnection())
        //    {
        //        try
        //        {
        //            SqlCommand command = new SqlCommand("um_saverolebaseaccessuserpage", conn);
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Clear();
        //            #region CheckBoxes

        //            #region BotDashboard

        //            var Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotDashboardR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotDashboardR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotDashboardC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotDashboardC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotDashboardE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotDashboardE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotDashboardD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotDashboardD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region Queue Details





        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbQueueR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbqueueR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbqueueC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbqueueC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbqueueE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbqueueE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbqueueD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbqueueD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region Add Schedules
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddScheduleR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddScheduleR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddScheduleC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddScheduleC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddScheduleE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddScheduleE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddScheduleD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddScheduleD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion


        //            #region Add User

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUserR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUserR;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUserC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUserC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUserE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUserE;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUserD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUserD;
        //            command.Parameters.Add(Uparameter);
        //            #endregion

        //            #region Add Robot

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddRobotR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddRobotR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddRobotC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddRobotC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddRobotD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddRobotD;
        //            command.Parameters.Add(Uparameter);



        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddRobotE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddRobotE;
        //            command.Parameters.Add(Uparameter);
        //            #endregion


        //            #region Add Queue


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddQueueR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddQueueR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddQueueC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddQueueC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddQueueE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddQueueE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddQueueD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddQueueD;
        //            command.Parameters.Add(Uparameter);
        //            #endregion


        //            #region Add Group

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddGroupR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddGroupR;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddGroupC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddGroupC;
        //            command.Parameters.Add(Uparameter);




        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddGroupE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddGroupE;
        //            command.Parameters.Add(Uparameter);





        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddGroupD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddGroupD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region QueueToBot

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignQueueBotR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignQueueBotR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignQueueBotC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignQueueBotC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignQueueBotE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignQueueBotE;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignQueueBotD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignQueueBotD;
        //            command.Parameters.Add(Uparameter);
        //            #endregion

        //            #region BotLOg
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotLogR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotLogR;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotLogC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotLogC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotLogE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotLogE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbBotLogD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbBotLogD;
        //            command.Parameters.Add(Uparameter);
        //            #endregion

        //            #region Audit Trail

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAuditTrailR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAuditTrailR;
        //            command.Parameters.Add(Uparameter);



        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAuditTrailC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAuditTrailC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAuditTrailE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAuditTrailE;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAuditTrailD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAuditTrailD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region ScheduleDetails
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbScheduleDetailsR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbScheduleDetailsR;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbScheduleDetailsC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbScheduleDetailsC;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbScheduleDetailsE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbScheduleDetailsE;
        //            command.Parameters.Add(Uparameter);



        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbScheduleDetailsD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbScheduleDetailsD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion
        //            #region Add  Update process

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUpdateProcessR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUpdateProcessR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUpdateProcessC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUpdateProcessC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUpdateProcessE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUpdateProcessE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAddUpdateProcessD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAddUpdateProcessD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region Configuration
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbConfigurationR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbConfigurationR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbConfigurationC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbConfigurationC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbConfigurationE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbConfigurationE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbConfigurationD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbConfigurationD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region UploadProcess

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbUploadProcessR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbUploadProcessR;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbUploadProcessC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbUploadProcessC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbUploadProcessE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbUploadProcessE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbUploadProcessD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbUploadProcessD;
        //            command.Parameters.Add(Uparameter);
        //            #endregion

        //            #region PromoteDemote
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbPromoteDemoteR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbPromoteDemoteR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbPromoteDemoteC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbPromoteDemoteC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbPromoteDemoteE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbPromoteDemoteE;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbPromoteDemoteD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbPromoteDemoteD;
        //            command.Parameters.Add(Uparameter);

        //            #endregion

        //            #region BotUSER
        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignBotUserR";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignBotUserR;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignBotUserC";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignBotUserC;
        //            command.Parameters.Add(Uparameter);

        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignBotUserE";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignBotUserE;
        //            command.Parameters.Add(Uparameter);


        //            Uparameter = command.CreateParameter();
        //            Uparameter.ParameterName = "bbAssignBotUserD";
        //            Uparameter.DbType = System.Data.DbType.Boolean;
        //            Uparameter.Value = bbAssignBotUserD;
        //            command.Parameters.Add(Uparameter);
        //            AddGroupAndTenantParametesRoleBase(ref command, Igroupid, Itenantid, usernameI);
        //            result = command.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            return 0;
        //        }
        //        return result;
        //    }
        //}
        #endregion

        public int SaveRoleBaseAccessUser(int Igroupid, string usernameI, int Itenantid, bool bbBotDashboardR, bool bbBotDashboardC, bool bbBotDashboardE, bool bbBotDashboardD, bool bbqueueR, bool bbqueueC, bool bbqueueE, bool bbqueueD, bool bbAddScheduleR, bool bbAddScheduleC, bool bbAddScheduleE, bool bbAddScheduleD, bool bbAddUserR, bool bbAddUserC, bool bbAddUserE, bool bbAddUserD, bool bbAddRobotR, bool bbAddRobotC, bool bbAddRobotE, bool bbAddRobotD, bool bbQueueManagementR, bool bbQueueManagementC, bool bbQueueManagementE, bool bbQueueManagementD, bool bbAssignQueueBotR, bool bbAssignQueueBotC, bool bbAssignQueueBotE, bool bbAssignQueueBotD, bool bbAssignBotUserR, bool bbAssignBotUserC, bool bbAssignBotUserE, bool bbAssignBotUserD, bool bbBotLogR, bool bbBotLogC, bool bbBotLogE, bool bbBotLogD, bool bbAuditTrailR, bool bbAuditTrailC, bool bbAuditTrailE, bool bbAuditTrailD, bool bbScheduleDetailsR, bool bbScheduleDetailsC, bool bbScheduleDetailsE, bool bbScheduleDetailsD, bool bbConfigurationR, bool bbConfigurationC, bool bbConfigurationE, bool bbConfigurationD, bool bbProcessManagementR, bool bbProcessManagementC, bool bbProcessManagementE, bool bbProcessManagementD, bool bbDetailLogR, bool bbDetailLogC, bool bbDetailLogE, bool bbDetailLogD, bool bbPromoteDemoteR, bool bbPromoteDemoteC, bool bbPromoteDemoteE, bool bbPromoteDemoteD)
        {
            int result = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_saverolebaseaccessuserpage", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Clear();
                    #region CheckBoxes

                    #region BotDashboard

                    var Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotDashboardR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotDashboardR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotDashboardC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotDashboardC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotDashboardE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotDashboardE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotDashboardD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotDashboardD;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region Queue Details





                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbQueueR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbqueueR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbqueueC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbqueueC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbqueueE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbqueueE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbqueueD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbqueueD;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region Add Schedules
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddScheduleR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddScheduleR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddScheduleC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddScheduleC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddScheduleE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddScheduleE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddScheduleD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddScheduleD;
                    command.Parameters.Add(Uparameter);

                    #endregion


                    #region Add User

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddUserR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddUserR;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddUserC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddUserC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddUserE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddUserE;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddUserD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddUserD;
                    command.Parameters.Add(Uparameter);
                    #endregion

                    #region Add Robot

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddRobotR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddRobotR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddRobotC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddRobotC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddRobotD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddRobotD;
                    command.Parameters.Add(Uparameter);



                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAddRobotE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAddRobotE;
                    command.Parameters.Add(Uparameter);
                    #endregion


                    #region Add Queue/ QueueManagement


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbQueueManagementR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbQueueManagementR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbQueueManagementC";      //Add Queue
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbQueueManagementC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbQueueManagementE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbQueueManagementE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbQueueManagementD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbQueueManagementD;
                    command.Parameters.Add(Uparameter);
                    #endregion


                    #region Add Group

                    //Uparameter = command.CreateParameter();
                    //Uparameter.ParameterName = "bbAddGroupR";
                    //Uparameter.DbType = System.Data.DbType.Boolean;
                    //Uparameter.Value = bbAddGroupR;
                    //command.Parameters.Add(Uparameter);


                    //Uparameter = command.CreateParameter();
                    //Uparameter.ParameterName = "bbAddGroupC";
                    //Uparameter.DbType = System.Data.DbType.Boolean;
                    //Uparameter.Value = bbAddGroupC;
                    //command.Parameters.Add(Uparameter);




                    //Uparameter = command.CreateParameter();
                    //Uparameter.ParameterName = "bbAddGroupE";
                    //Uparameter.DbType = System.Data.DbType.Boolean;
                    //Uparameter.Value = bbAddGroupE;
                    //command.Parameters.Add(Uparameter);





                    //Uparameter = command.CreateParameter();
                    //Uparameter.ParameterName = "bbAddGroupD";
                    //Uparameter.DbType = System.Data.DbType.Boolean;
                    //Uparameter.Value = bbAddGroupD;
                    //command.Parameters.Add(Uparameter);

                    #endregion

                    #region QueueToBot

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignQueueBotR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignQueueBotR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignQueueBotC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignQueueBotC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignQueueBotE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignQueueBotE;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignQueueBotD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignQueueBotD;
                    command.Parameters.Add(Uparameter);
                    #endregion

                    #region BotLOg
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotLogR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotLogR;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotLogC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotLogC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotLogE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotLogE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbBotLogD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbBotLogD;
                    command.Parameters.Add(Uparameter);
                    #endregion

                    #region Audit Trail

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAuditTrailR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAuditTrailR;
                    command.Parameters.Add(Uparameter);



                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAuditTrailC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAuditTrailC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAuditTrailE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAuditTrailE;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAuditTrailD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAuditTrailD;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region ScheduleDetails
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbScheduleDetailsR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbScheduleDetailsR;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbScheduleDetailsC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbScheduleDetailsC;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbScheduleDetailsE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbScheduleDetailsE;
                    command.Parameters.Add(Uparameter);



                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbScheduleDetailsD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbScheduleDetailsD;
                    command.Parameters.Add(Uparameter);

                    #endregion
                    #region Add  Update process /Process Management

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbProcessManagementR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbProcessManagementR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbProcessManagementC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbProcessManagementC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbProcessManagementE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbProcessManagementE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbProcessManagementD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbProcessManagementR;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region Configuration
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbConfigurationR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbConfigurationR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbConfigurationC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbConfigurationC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbConfigurationE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbConfigurationE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbConfigurationD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbConfigurationD;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region UploadProcess/Details Log

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbDetailLogR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbDetailLogR;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbDetailLogC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbDetailLogC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbDetailLogE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbDetailLogE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbDetailLogD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbDetailLogE;
                    command.Parameters.Add(Uparameter);
                    #endregion

                    #region PromoteDemote
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbPromoteDemoteR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbPromoteDemoteR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbPromoteDemoteC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbPromoteDemoteC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbPromoteDemoteE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbPromoteDemoteE;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbPromoteDemoteD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbPromoteDemoteD;
                    command.Parameters.Add(Uparameter);

                    #endregion

                    #region BotUSER
                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignBotUserR";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignBotUserR;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignBotUserC";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignBotUserC;
                    command.Parameters.Add(Uparameter);

                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignBotUserE";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignBotUserE;
                    command.Parameters.Add(Uparameter);


                    Uparameter = command.CreateParameter();
                    Uparameter.ParameterName = "bbAssignBotUserD";
                    Uparameter.DbType = System.Data.DbType.Boolean;
                    Uparameter.Value = bbAssignBotUserD;
                    command.Parameters.Add(Uparameter);
                    AddGroupAndTenantParametesRoleBase(ref command, Igroupid, Itenantid, usernameI);
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return result;
            }
        }


        private void AddGroupAndTenantParametesRoleBase(ref SqlCommand command, int iGroupId, int iTenantId, string IUsername)
        {
            var Uparameter = command.CreateParameter();

            Uparameter.ParameterName = "Igroupid";
            Uparameter.DbType = System.Data.DbType.Int32;
            Uparameter.Value = iGroupId;
            command.Parameters.Add(Uparameter);

            Uparameter = command.CreateParameter();
            Uparameter.ParameterName = "Itenantid";
            Uparameter.DbType = System.Data.DbType.Int32;
            Uparameter.Value = iTenantId;
            command.Parameters.Add(Uparameter);

            Uparameter = command.CreateParameter();
            Uparameter.ParameterName = "usernameA";
            Uparameter.DbType = System.Data.DbType.String;
            Uparameter.Value = IUsername;
            command.Parameters.Add(Uparameter);

        }

        /*End of Role Based Access*/


        /*Delete Group Methods*/
        public int DeleteGroupWithAllRelatedData(int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteGroupWithAllRelatedData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby);
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }


        public DataTable GetCountToDeleteGroup(int groupid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getCountToDeleteGroup", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@groupid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);



                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }


        public int UpdateIsactiveStatusGroupRelatedTables(int groupid, int isactive)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_updateIsactiveStatusGroupRelatedTables", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "groupid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "isactive";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = isactive;
                    command.Parameters.Add(parameter);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }


        public int CheckIsactiveStatusGroup(int groupid)
        {
            int statusResult = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_CheckIsactiveStatusGroup", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "groupid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    SqlParameter prm1 = new SqlParameter("@status", SqlDbType.Int);
                    prm1.Direction = ParameterDirection.Output;
                    command.Parameters.Add(prm1);

                    command.ExecuteNonQuery();

                    statusResult = (int)(command.Parameters["@status"].Value);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return -1;
                }

                return statusResult;
            }
        }

        /*End Delete Group Method*/


        //Update User Password from Masterpage
        public int UpdateUserPassword(string userid, string OldPassword, string NewPassword, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_updateUserPassword", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "OldPassword";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = OldPassword;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "NewPassword";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = NewPassword;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);


                    i = command.ExecuteNonQuery();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                return i;
            }
        }

        /*Method added to get dashboard card details*/
        public int[] GetDetailsDashboard(int groupid, int tenantid)
        {
            int[] results = new int[5];
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getDashboardData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    AddGroupAndTenantParametes(ref command, groupid, tenantid);
                    SqlParameter param1 = new SqlParameter("@robots", SqlDbType.Int);
                    param1.Direction = ParameterDirection.Output;
                    command.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter("@successcount", SqlDbType.Int);
                    param2.Direction = ParameterDirection.Output;
                    command.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter("@schedulecount", SqlDbType.Int);
                    param3.Direction = ParameterDirection.Output;
                    command.Parameters.Add(param3);

                    SqlParameter param4 = new SqlParameter("@failedprocesscount", SqlDbType.Int);
                    param4.Direction = ParameterDirection.Output;
                    command.Parameters.Add(param4);

                    command.ExecuteNonQuery();

                    results[0] = Convert.ToInt32(command.Parameters["@robots"].Value);
                    results[1] = Convert.ToInt32(command.Parameters["@successcount"].Value);
                    results[2] = Convert.ToInt32(command.Parameters["@schedulecount"].Value);
                    results[3] = Convert.ToInt32(command.Parameters["@failedprocesscount"].Value);
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return results;
        }


        private void AddGroupAndTenantParametes(ref SqlCommand command, int iGroupId, int iTenantId)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = "groupid";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = iGroupId;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();

            parameter.ParameterName = "tenantid";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = iTenantId;
            command.Parameters.Add(parameter);

        }


        //public DataTable GetRQDetails(int iGroupId,int iTenantId)
        public DataTable GetRQDetails(string machinename)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = null;


                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_getrqdetailsbyid", conn);
                        command.CommandType = CommandType.StoredProcedure;

                        var parameter = command.CreateParameter();


                        parameter.ParameterName = "machinename";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = machinename;
                        command.Parameters.Add(parameter);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                        if (ds.Tables.Count > 0)
                            dt = ds.Tables[0];




                    }
                    catch (Exception exception)
                    {
                        Debug.Write("GetRQDetails Exception Inside Try Try: " + exception.Message);


                    }
                }


                return dt; //success
            }
            catch (Exception ex)
            {
                Debug.Write("GetRQDetails Exception : " + ex.Message);
                return null; //fail
            }


        }
        public DataTable GetRQDetailsByName(string strGroupName, string strTenantName)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = null;


                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_getrqdetails", conn);
                        command.CommandType = CommandType.StoredProcedure;


                        var parameter = command.CreateParameter();


                        parameter.ParameterName = "groupname";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strGroupName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "tenantname";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strTenantName;
                        command.Parameters.Add(parameter);



                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                        if (ds.Tables.Count > 0)
                            dt = ds.Tables[0];



                    }
                    catch (Exception exception)
                    {
                        Debug.Write("GetRQDetailsByName Exception Inside Try Try: " + exception.Message);


                    }
                }


                return dt; //success
            }
            catch (Exception ex)
            {
                Debug.Write("GetRQDetailsByName Exception : " + ex.Message);
                return null; //fail
            }


        }



        public DataTable GetCustomRoleBasedAccess(int iGroupId, int iTenantId)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = null;


                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_getcustomrolebasedaccess", conn);
                        command.CommandType = CommandType.StoredProcedure;

                        AddGroupAndTenantParametes(ref command, iGroupId, iTenantId);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                        if (ds.Tables.Count > 0)
                            dt = ds.Tables[0];



                    }
                    catch (Exception exception)
                    {
                        Debug.Write("GetCustomRoleBasedAccess Exception Inside Try Try: " + exception.Message);


                    }
                }


                return dt; //success
            }
            catch (Exception ex)
            {
                Debug.Write("GetCustomRoleBasedAccess Exception : " + ex.Message);
                return null; //fail
            }


        }

        public int UpdateCustomRoleBasedAccess(bool QDetailsDevVal, bool QDetailsProdVal, bool QDetailsTestVal, bool AddSchedDevVal, bool AddSchedProdVal, bool AddSchedTestVal, bool AddUserDevVal, bool AddUserProdVal, bool AddUserTestVal,
            bool AddRobotDevVal, bool AddRobotProdVal, bool AddRobotTestVal, bool AddQDevVal, bool AddQProdVal, bool AddQTestVal, bool AddGroupDevVal, bool AddGroupProdVal, bool AddGroupTestVal,
            bool AddQueToBotDevVal, bool AddQueToBotProdVal, bool AddQueToBotTestVal, bool AddBotToUserDevVal, bool AddBotToUserProdVal, bool AddBotToUserTestVal,
            bool BotLogDevVal, bool BotLogProdVal, bool BotLogTestVal, bool AuditTrailDevVal, bool AuditTrailProdVal, bool AuditTrailTestVal, int groupid, int tenantid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = null;
                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_updatecustomrolebasedaccess", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();

                        parameter.ParameterName = "qdetailsdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = QDetailsDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "qdetailsprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = QDetailsProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "qdetailstest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = QDetailsTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addscheddev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddSchedDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addschedprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddSchedProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addschedtest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddSchedTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "adduserdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddUserDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "adduserprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddUserProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addusertest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddUserTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addrobotdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddRobotDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addrobotprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddRobotProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addrobottest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddRobotTestVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addqdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addqprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addqtest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQTestVal;
                        command.Parameters.Add(parameter);



                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addgroupdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddGroupDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addgroupprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddGroupProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addgrouptest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddGroupTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addquetobotdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQueToBotDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addquetobotprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQueToBotProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addquetobottest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddQueToBotTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addbottouserdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddBotToUserDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addbottouserprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddBotToUserProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "addbottousertest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AddBotToUserTestVal;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botlogdev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = BotLogDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botlogprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = BotLogProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botlogtest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = BotLogTestVal;
                        command.Parameters.Add(parameter);



                        parameter = command.CreateParameter();
                        parameter.ParameterName = "audittraildev";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AuditTrailDevVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "audittrailprod";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AuditTrailProdVal;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "audittrailtest";
                        parameter.DbType = System.Data.DbType.Boolean;
                        parameter.Value = AuditTrailTestVal;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);



                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();

                    }
                    catch (Exception exception)
                    {
                        Debug.Write("UpdateCustomRoleBasedAccess Exception Inside Try Try: " + exception.Message);
                    }
                }

                return 1; //success
            }
            catch (Exception ex)
            {
                Debug.Write("UpdateCustomRoleBasedAccess Exception : " + ex.Message);
                return 0; //fail
            }

        }


        public int AddSchedule(string strQueueName, string strBotName, string strChronExp, string StopAfter, int groupid, int tenantid, string createdby)
        {
            try
            {
                if (quartzHelper == null)
                {
                    quartzHelper = new QuartzHelper();
                }

                string strMachineName = Environment.MachineName;
                //quartzHelper.CreateJob("local");
                quartzHelper.CreateJob(strMachineName, strQueueName, strBotName, strChronExp, groupid, tenantid);
                // Logger.Log.Logger.LogData("ScheduleAdded", LogLevel.Info);

                /*Insertion To Database method Added on 06/05/2019 */

                string strChronExpDescription = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(strChronExp);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_addschedule", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "queuename";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strQueueName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botname";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strBotName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "chronexpression";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strChronExp;
                        //parameter.Value = strChronExpDescription;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);

                        /*Added on 05-06-2019 for storing bot formats of chronexpression.*/
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "strChronExp";
                        parameter.DbType = System.Data.DbType.String;
                        //parameter.Value = strChronExp;
                        parameter.Value = strChronExpDescription;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        if (null == StopAfter)
                        {
                            StopAfter = "";
                        }
                        parameter.ParameterName = "StopAfter";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = StopAfter;
                        command.Parameters.Add(parameter);
                        command = AddAuditParameters(command, createdby);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                    }
                    catch (Exception exception)
                    {
                        Debug.Write("Add Schedule Exception Inside Try Try: " + exception.Message);
                    }
                }
                return 1; //success
            }
            catch (Exception ex)
            {
                Debug.Write("Add Schedule Exception : " + ex.Message);
                return 0; //fail
            }
        }

        #region Add Schedule For Process
        public int AddScheduleForProcess(string strProcessName, string strBotName, string strChronExp, string StopAfter, int groupid, int tenantid, string createdby)
        {
            try
            {
                if (quartzHelper == null)
                {
                    quartzHelper = new QuartzHelper();
                }

                string strMachineName = Environment.MachineName;
                //quartzHelper.CreateJob("local");
                quartzHelper.CreateJobForProcess(strMachineName, strProcessName, strBotName, strChronExp, groupid, tenantid);

                /*Insertion To Database method Added on 06/05/2019 */

                string strChronExpDescription = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(strChronExp);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_addScheduleForProcess", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "processname";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strProcessName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botname";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strBotName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "chronexpression";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strChronExp;
                        //parameter.Value = strChronExpDescription;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);

                        /*Added on 05-06-2019 for storing bot formats of chronexpression.*/
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "strChronExp";
                        parameter.DbType = System.Data.DbType.String;
                        //parameter.Value = strChronExp;
                        parameter.Value = strChronExpDescription;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        if (null == StopAfter)
                        {
                            StopAfter = "";
                        }
                        parameter.ParameterName = "StopAfter";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = StopAfter;
                        command.Parameters.Add(parameter);
                        command = AddAuditParameters(command, createdby);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                    }
                    catch (Exception exception)
                    {
                        Debug.Write("Add Schedule For Process Exception Inside Try Try: " + exception.Message);
                    }
                }
                return 1; //success
            }
            catch (Exception ex)
            {
                Debug.Write("Add Schedule For Process Exception : " + ex.Message);
                return 0; //fail
            }
        }
        #endregion

        #region Activity Log
        public int insertActivityLog(string domainName, string userName, string groupName, string action, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int resultExecuteQuery = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_ActivityLogs", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "domainName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = domainName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "userName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "action";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = action;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    resultExecuteQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }
                return resultExecuteQuery;
            }
        }
        #endregion


        public int InsertIntoLogger(string MachineName, string UserName, string RobotName, string ProcessName,
            DateTime dateUtc, string Logger, string Message, int groupid, int tenantid)
        {
            try
            {
                if (quartzHelper == null)
                {
                    quartzHelper = new QuartzHelper();
                }

                string strMachineName = Environment.MachineName;

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_addLogs", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "MachineName";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strMachineName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "UserName";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = UserName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "RobotName";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = RobotName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "ProcessName";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = ProcessName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        string date = dateUtc.ToString("yyyyMMdd");


                        parameter.ParameterName = "dateUtc";
                        parameter.DbType = System.Data.DbType.String;
                        //parameter.Value = dateUtc;
                        parameter.Value = date;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "Logger";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = Logger;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "Message";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = Message;
                        command.Parameters.Add(parameter);


                        string LogLevel = string.Empty;
                        LogLevel = "INFO";
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "LogLevel";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = LogLevel;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);
                        //command = AddAuditParameters(command, createdby);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();

                    }
                    catch (Exception exception)
                    {
                        Debug.Write("Add Log Exception Inside Try Try: " + exception.Message);
                    }
                }

                return 1; //success
            }
            catch (Exception ex)
            {
                Debug.Write("Add Schedule Exception : " + ex.Message);
                return 0; //fail
            }
        }

        //public int DeleteSchedule(string strId, string strQueueName, string strBotName, string strChronExp, int groupid, int tenantid, string createdby)
        //{
        //    try
        //    {
        //        strChronExp = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(strChronExp);

        //        if (quartzHelper == null)
        //        {
        //            quartzHelper = new QuartzHelper();
        //        }
        //        string strMachineName = Environment.MachineName;
        //        quartzHelper.DeleteJob(strMachineName, strQueueName, strBotName, strChronExp, tenantid);

        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        int i = 0;

        //        using (SqlConnection conn = GetConnection())
        //        {

        //            try
        //            {
        //                SqlCommand command = new SqlCommand("um_deleteschedules", conn);
        //                command.CommandType = CommandType.StoredProcedure;
        //                var parameter = command.CreateParameter();
        //                parameter.ParameterName = "scheduleid";
        //                //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
        //                parameter.DbType = System.Data.DbType.Int32;
        //                parameter.Value = strId;
        //                command.Parameters.Add(parameter);
        //                //parameter = command.CreateParameter();
        //                //parameter.ParameterName = "tenantid";
        //                //parameter.DbType = System.Data.DbType.Int32;
        //                //parameter.Value = tenantid;
        //                //command.Parameters.Add(parameter);

        //                AddGroupAndTenantParametes(ref command, groupid, tenantid);

        //                command = AddAuditParamCreatedBy(command, createdby);
        //                i = command.ExecuteNonQuery();
        //                conn.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (conn.State == ConnectionState.Open)
        //                {
        //                    conn.Close();
        //                }
        //            }
        //            return i;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Write("Delete Schedule Exception : " + ex.Message);
        //        return 0; //fail
        //    }

        //}

        public int DeleteSchedule(string strId, string strQueueName, string strBotName, string ChronExp, int groupid, int tenantid, string createdby)
        {
            try
            {
                string strChronExp = String.Empty;
                strChronExp = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(ChronExp);

                if (quartzHelper == null)
                {
                    quartzHelper = new QuartzHelper();
                }
                string strMachineName = Environment.MachineName;
                quartzHelper.DeleteJob(strMachineName, strQueueName, strBotName, strChronExp, tenantid);

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {

                    try
                    {
                        SqlCommand command = new SqlCommand("um_deleteschedules", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "scheduleid";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.Int32;
                        parameter.Value = strId;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "queueName";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strQueueName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "botName";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = strBotName;
                        command.Parameters.Add(parameter);

                        parameter = command.CreateParameter();
                        parameter.ParameterName = "chronExpression";
                        //parameter.DbType = System.Data.DbType.String;            //Converted String to int in net statement.
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = ChronExp;
                        command.Parameters.Add(parameter);

                        AddGroupAndTenantParametes(ref command, groupid, tenantid);

                        parameter = command.CreateParameter();

                        command = AddAuditParamCreatedBy(command, createdby);

                        i = command.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                    return i;
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Delete Schedule Exception : " + ex.Message);
                return 0; //fail
            }

        }


        public int DeleteTenantWithAllRelatedData(int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteTenantWithAllRelatedData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby);
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        public DataTable GetCountToDeleteTenant(int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getCountToDeleteTenant", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);



                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        /*Service For Charts*/

        public DataTable getMonthlyChartData(string status)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {

                    SqlCommand command = new SqlCommand("getMonthlyChartData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "status";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = status;
                    command.Parameters.Add(parameter);


                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }



        public DataTable GetData_createschedulestatus()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("GetData_Automation_createschedulestatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public DataTable GetddlData_createschedulestatus()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("GetddlData_createschedulestatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public DataTable getChartData(string status)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {

                    SqlCommand command = new SqlCommand("getChartData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "status";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = status;
                    command.Parameters.Add(parameter);


                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }


        public DataTable getDoughnutChartData()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("getDoughnutChartData_createschedulestatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }



        /*End of Service for Charts*/

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public int AddUser(string domainname, string userid, string pwd, int groupid, int tenantid, string strUserRole, string createdby)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createuser", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "domainname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = domainname;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "pwd";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = pwd;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "roletype";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strUserRole;
                    command.Parameters.Add(parameter);

                    command = AddAuditParameters(command, createdby);

                    i = command.ExecuteNonQuery();
                    //SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return i;
            }
        }

        /*Add Tenant Code : Called from AddTenant.aspx.cs*/
        public int AddTenant(string TenantName, string owner, int iGroupId, int iTenantId, string createdby)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createtenant", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "tenantname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = TenantName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "owner";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = owner;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, iGroupId, iTenantId);

                    command = AddAuditParameters(command, createdby);

                    //SqlDataAdapter da = new SqlDataAdapter(command);
                    i = command.ExecuteNonQuery();
                    //da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return i;
            }
        }

        /*Adding Latest Version Function to be used.*/
        public int UpdateDefaultVersion(int iGroupId, int iTenantId, string ProcessId, string updatedVersion, bool isLatest)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_UpdateDefaultVersion", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    AddGroupAndTenantParametes(ref command, iGroupId, iTenantId);

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "ProcessId";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "updatedVersion";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = updatedVersion;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "isLatest";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = isLatest;
                    command.Parameters.Add(parameter);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exx)
                {
                    Debug.Write("UpdateDefaultVersion Exception : " + exx.Message);
                }

                return i;
            }
        }

        /*Adding Function for Uploading a Process in the Database on Date 27/04/2019*/

        public int AddProcess(string ProcessName, int EnvironmentName, int groupid, int tenantid, string ProcessVersion, bool latestVersion, string createdBy)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createprocess", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "processname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "environmentid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = EnvironmentName;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "defaultversion";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessVersion;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "uselatest";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = latestVersion;
                    command.Parameters.Add(parameter);

                    /*parameter = command.CreateParameter();
                    parameter.ParameterName = "createdby";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = createdBy;
                    command.Parameters.Add(parameter);*/

                    command = AddAuditParameters(command, createdBy);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exx)
                {
                    Debug.Write("Piyush******Error in  DAtabase : " + exx.Message + "*******Piyush");
                }

                return i;
            }
        }

        //Addded For Testing by Piyush
        public int CreateScheduleStatus(string QueueName, string BotName, string ChronExpression, string Status, int GroupId, int TenantId, string StartTime, string EndTime)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createschedulestatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "QueueName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = QueueName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "BotName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = BotName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "ChronExpression";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ChronExpression;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "Status";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = Status;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, GroupId, TenantId);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "StartTime";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = StartTime;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "EndTime";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = EndTime;
                    command.Parameters.Add(parameter);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }
                return i;
            }
        }

        #region QueueRelated

        public int DeleteQueue(string queueName, int tenantid, int groupid)
        {
            int result = 0;
            DataTable dt;
            try
            {
                result = Delete_Queue(queueName, tenantid, groupid);

                if (0 < result)
                {
                    dt = GetRMQCreadential(tenantid);
                    ConnectionFactory factory = new ConnectionFactory();
                    factory.UserName = dt.Rows[0][0].ToString();
                    factory.Password = dt.Rows[0][1].ToString();
                    IConnection conn = factory.CreateConnection();
                    IModel channel = conn.CreateModel();
                    channel.QueueDelete(queueName, false, false);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

            }

            return result;
        }
        /* To delete from datatabase*/
        private int Delete_Queue(string queueName, int tenantid, int groupid)
        {
            int result = 0;
            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_deleteQueue", conn);
                command.CommandType = CommandType.StoredProcedure;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "tenantid";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = tenantid;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "groupid";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = groupid;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "queueName";
                parameter.DbType = System.Data.DbType.String;
                parameter.Value = queueName;
                command.Parameters.Add(parameter);


                result = command.ExecuteNonQuery();

            }

            return result;
        }
        public int PurgeQueue(string queueName, int tenantid)
        {
            int result = 0;
            DataTable dt;
            try
            {
                dt = GetRMQCreadential(tenantid);
                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = dt.Rows[0][0].ToString();
                factory.Password = dt.Rows[0][1].ToString();
                IConnection conn = factory.CreateConnection();
                IModel channel = conn.CreateModel();
                channel.QueuePurge(queueName);
                result = 1;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.StackTrace);
            }
            return result;
        }
        public int AddQueue(string queueName, string exchangeName, int tenantid, int groupid)
        {
            int result = 0;
            DataTable dt, dy;



            try
            {
                dt = GetRMQCreadential(tenantid);
                dy = GetGroupName(tenantid, groupid);
                string key = dy.Rows[0][0].ToString() + "." + dy.Rows[0][1].ToString();
                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = dt.Rows[0][0].ToString();
                factory.Password = dt.Rows[0][1].ToString();
                IConnection conn = factory.CreateConnection();
                IModel channel = conn.CreateModel();
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, exchangeName, key, null);
                result = 1;

                InsertDB(queueName, exchangeName, key, tenantid, groupid);

            }
            catch (Exception)
            {

            }
            return result;
        }

        private DataTable GetGroupName(int tenantid, int groupid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getGroupName", conn);
                command.CommandType = CommandType.StoredProcedure;


                var parameter = command.CreateParameter();

                parameter.ParameterName = "groupid";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = groupid;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();

                parameter.ParameterName = "tenantid";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = tenantid;
                command.Parameters.Add(parameter);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            return dt;
        }


        private DataTable GetRMQCreadential(int tenantid)
        {

            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getRMQCredential", conn);
                command.CommandType = CommandType.StoredProcedure;
                var parameter = command.CreateParameter();

                parameter.ParameterName = "tenantid";
                parameter.DbType = System.Data.DbType.Int32;
                parameter.Value = tenantid;
                command.Parameters.Add(parameter);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];


            }
            return dt;
        }


        private void InsertDB(string queueName, string exchangeName, string key, int tenantid, int groupid)
        {
            int i = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_queueExchangeDetails", conn);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    var parameter2 = command.CreateParameter();
                    parameter2.ParameterName = "queuename";
                    parameter2.DbType = System.Data.DbType.String;
                    parameter2.Value = queueName;
                    command.Parameters.Add(parameter2);

                    parameter2 = command.CreateParameter();
                    parameter2.ParameterName = "exchangename";
                    parameter2.DbType = System.Data.DbType.String;
                    parameter2.Value = exchangeName;
                    command.Parameters.Add(parameter2);

                    parameter2 = command.CreateParameter();
                    parameter2.ParameterName = "routingkey";
                    parameter2.DbType = System.Data.DbType.String;
                    parameter2.Value = key;
                    command.Parameters.Add(parameter2);

                    parameter2 = command.CreateParameter();
                    parameter2.ParameterName = "groupid";
                    parameter2.DbType = System.Data.DbType.Int32;
                    parameter2.Value = groupid;
                    command.Parameters.Add(parameter2);

                    parameter2 = command.CreateParameter();

                    parameter2.ParameterName = "tenantid";
                    parameter2.DbType = System.Data.DbType.Int32;
                    parameter2.Value = tenantid;
                    command.Parameters.Add(parameter2);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }
            }
        }

        public DataTable getQueueNames(int tenantid, int groupid)
        {

            DataSet ds = new DataSet();
            DataTable dt = null;
            using (SqlConnection dbconnection = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getQueueNames", dbconnection);
                command.CommandType = CommandType.StoredProcedure;

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                dbconnection.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            return dt;
        }
        #endregion

        public int PiyushLogs(string Message)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_piyushlogs", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "Message";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = Message;
                    command.Parameters.Add(parameter);


                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return i;
            }
        }

        #region Get Activity Logs
        public DataTable getActivityLog(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable resultForActivityLog = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getActivityLogs", conn);
                command.CommandType = CommandType.StoredProcedure;

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();

                if (ds.Tables.Count > 0)
                {
                    resultForActivityLog = ds.Tables[0];
                    return resultForActivityLog;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Get Complete Logs
        public DataTable getCompleteLogs(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable resultForCompleteLogs = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getCompleteLogs", conn);
                command.CommandType = CommandType.StoredProcedure;

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();

                if (ds.Tables.Count > 0)
                {
                    resultForCompleteLogs = ds.Tables[0];
                    return resultForCompleteLogs;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Inserting Log to CompleteLog Table
        public int insertLog(string Message, string detailLog, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int resultExecuteQuery = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_completeLogs", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "Message";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = Message;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "detailLog";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = detailLog;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    resultExecuteQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }
                return resultExecuteQuery;
            }
        }
        #endregion

        public int AddGroup(string groupName, int groupid, int tenantid, string createdby)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createGroup", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "groupName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupName;
                    command.Parameters.Add(parameter);


                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParameters(command, createdby);

                    i = command.ExecuteNonQuery();
                    conn.Close();

                    //SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(ds);
                    //conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return i;
            }
        }

        public int GetGroupId(string groupName, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;
            int result = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getGroupId", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@groupName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantId";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);


                    var parameterreturn = command.CreateParameter();
                    parameterreturn.ParameterName = "result";
                    parameterreturn.DbType = System.Data.DbType.Int32;
                    parameterreturn.Direction = ParameterDirection.Output;
                    command.Parameters.Add(parameterreturn);



                    result = command.ExecuteNonQuery();
                    conn.Close();
                    result = Convert.ToInt32(parameterreturn.Value);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return result;
            }
        }

        public int GetTenantId(int groupid, int tenantid, string TenantName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;
            int result = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getTenantId", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = TenantName;
                    command.Parameters.Add(parameter);


                    var parameterreturn = command.CreateParameter();
                    parameterreturn.ParameterName = "result";
                    parameterreturn.DbType = System.Data.DbType.Int32;
                    parameterreturn.Direction = ParameterDirection.Output;
                    command.Parameters.Add(parameterreturn);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    result = command.ExecuteNonQuery();
                    conn.Close();
                    result = Convert.ToInt32(parameterreturn.Value);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exception)
                {
                    Debug.Write("Add User Exception : " + exception.Message);
                }

                return result;
            }
        }





        public DataTable GetGroups(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getgroups", conn);
                command.CommandType = CommandType.StoredProcedure;

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }



        public int DeleteGroup(int id, int groupId, int tenantId, string groupName, string createdBy)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_deletegroup", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "id";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = id;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupId, tenantId);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupName;
                    command.Parameters.Add(parameter);

                    command = AddAuditParamCreatedBy(command, createdBy);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }




        /*Adding Function for Uploading a Process with zip attachment in the Database on Date 27/04/2019*/

        public int AddProcessWithZip(string ProcessName, int groupid, int tenantid, string ProcessVersion, bool latestVersion, string createdBy, byte[] ZipDataFile)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createprocess", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "processname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessName;
                    command.Parameters.Add(parameter);


                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "defaultversion";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessVersion;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "uselatest";
                    parameter.DbType = System.Data.DbType.Boolean;
                    parameter.Value = latestVersion;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "processfiles";
                    parameter.DbType = System.Data.DbType.Binary;
                    parameter.Value = ZipDataFile;
                    command.Parameters.Add(parameter);

                    /*parameter = command.CreateParameter();
                    parameter.ParameterName = "createdby";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = createdBy;
                    command.Parameters.Add(parameter);*/

                    //AddGroupAndTenantParametes(ref command, groupid, tenantid);
                    command = AddAuditParameters(command, createdBy);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception exx)
                {
                    Debug.Write("Piyush ******* ERROR ******" + exx.Message + "******END");
                }

                return i;
            }
        }

        public int AddQueueDetails(string queueName, int iGroupId, int iTenantId, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;
            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_queueDetails", conn);
                command.CommandType = CommandType.StoredProcedure;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "QueueName";
                parameter.DbType = System.Data.DbType.String;
                parameter.Value = queueName;
                command.Parameters.Add(parameter);

                AddGroupAndTenantParametes(ref command, iGroupId, iTenantId);

                command = AddAuditParameters(command, createdby);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
            }
            return 1;
        }

        //public int AddBot(string strBotName, string strBotId, string pwd, string botkey, string strMachineName, int groupid, int tenantid, string createdby)
        //{

        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    int i = 0;

        //    using (SqlConnection conn = GetConnection())
        //    {
        //        try
        //        {
        //            SqlCommand command = new SqlCommand("um_createbot", conn);
        //            command.CommandType = CommandType.StoredProcedure;
        //            var parameter = command.CreateParameter();
        //            parameter.ParameterName = "@botname";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = strBotName;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "@botid";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = strBotId;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "@pwd";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = pwd;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "@botkey";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = botkey;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "@machinename";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = strMachineName;
        //            command.Parameters.Add(parameter);

        //            AddGroupAndTenantParametes(ref command, groupid, tenantid);

        //            command = AddAuditParameters(command, createdby);

        //            i = command.ExecuteNonQuery();

        //            //SqlDataAdapter da = new SqlDataAdapter(command);
        //            //da.Fill(ds);
        //            conn.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //            {
        //                conn.Close();
        //            }
        //        }

        //        return i;
        //    }
        //}

        public int AddBot(string strBotName, string strBotId, string pwd, string botkey, string strMachineName, int groupid, int tenantid, string createdby)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int statusResult = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_createbot", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strBotName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strBotId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@pwd";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = pwd;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botkey";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botkey;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@machinename";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strMachineName;
                    command.Parameters.Add(parameter);

                    SqlParameter prm1 = new SqlParameter("@status", SqlDbType.Int);
                    prm1.Direction = ParameterDirection.Output;
                    command.Parameters.Add(prm1);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParameters(command, createdby);
                    command.ExecuteNonQuery();

                    statusResult = (int)(command.Parameters["@status"].Value);
                    //SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                return statusResult;
            }
        }

        public int AddConfigParameters(string strParameterName, string strParameterValue, int iAccessLevelProcessId, int groupid, int tenantid, string createdby)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_addconfigparameters", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@parametername";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strParameterName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@parametervalue";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strParameterValue;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@accesslevelprocessid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = iAccessLevelProcessId;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParameters(command, createdby);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                return i;
            }
        }

        public int DeleteBot(string botname, int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deletebot", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);
                    command = AddAuditParamCreatedBy(command, createdby);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        public int DeleteUser(string strId, int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteuser", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "id";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strId;
                    command.Parameters.Add(parameter);
                    AddGroupAndTenantParametes(ref command, groupid, tenantid);
                    command = AddAuditParamCreatedBy(command, createdby);
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        /*Delete Tenant */
        public int DeleteTenant(int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteTenant", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    //parameter.ParameterName = "TenantId";
                    //parameter.DbType = System.Data.DbType.String;
                    //parameter.Value = TenantId;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);


                    command = AddAuditParamCreatedBy(command, createdby);
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        /*Deleting Process added onn 29/04/2019*/
        public int DeleteProcess(string strId, int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteProcess", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "id";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strId;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        /*Deleting Process Version added onn 19/June/2019. Used in ProcessUpdate.aspx.cs*/
        public int DeleteProcessVersion(string strProcessId, string strProcessName, string strProcessVersion, int groupid, int tenantid, string createdby)
        //public int DeleteProcessVersion(string strProcessVersion, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteProcessVersion", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "processId";              //Passing ProcessId to DataBase Procedure.
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strProcessId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "processName";            //Passing ProcessName to DataBase Procedure.
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strProcessName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "processVersion";          //Passing ProcessVersion to DataBase Procedure.
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strProcessVersion;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby); //Passing Created By to DataBase Procedure.
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }



        public int DeleteConfigParameters(int iParameterId, int groupid, int tenantid, string currentuser)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteconfigparameters", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "parameterid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = iParameterId;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);
                    command = AddAuditParamCreatedBy(command, currentuser);
                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        public int DeleteQueueToBotMapping(string strId, int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_deletequeuetobotmapping", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "id";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strId;
                    command.Parameters.Add(parameter);
                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        public int DeleteUserToBotMapping(string strId, int groupid, int tenantid, string createdby)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_deleteusertobotmapping", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "id";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strId;
                    command.Parameters.Add(parameter);

                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParamCreatedBy(command, createdby);

                    i = command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return i;
            }
        }

        private SqlCommand AddAuditParameters(SqlCommand command, string createdby)
        {
            command = AddAuditParamCreatedBy(command, createdby);

            var parameter = command.CreateParameter();
            parameter.ParameterName = "updatedby";
            parameter.DbType = System.Data.DbType.String;
            //parameter.Value = System.Security.Principal.WindowsIdentity.GetCurrent().ToString();
            parameter.Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "updateddate";
            parameter.DbType = System.Data.DbType.DateTime;
            parameter.Value = DateTime.Now;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "isactive";
            parameter.DbType = System.Data.DbType.Int16;
            parameter.Value = 0;
            command.Parameters.Add(parameter);

            return command;
        }

        private SqlCommand AddAuditParamCreatedBy(SqlCommand command, string createdby)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "createdby";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = createdby;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "createddate";
            parameter.DbType = System.Data.DbType.DateTime;
            parameter.Value = DateTime.Now;
            command.Parameters.Add(parameter);
            return command;
        }

        public DataTable GetProcessDataWitoutZipFile(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getProcessDataWithoutFile", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        /*Added Process LoadProcessVersion on 01/05/2019*/
        public DataTable LoadProcessVersion(int groupid, int tenantid, string ProcessId)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_LoadProcessVersion", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    var parameter1 = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    parameter1.ParameterName = "@ProcessId";
                    parameter1.DbType = System.Data.DbType.String;
                    parameter1.Value = ProcessId;
                    command.Parameters.Add(parameter1);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Debug.Write("Exception BOTSERVICE : " + ex.Message);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }




        public DataTable GetBots(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getbots", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetSchedules(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getschedules", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetConfigParameters(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getconfigparameters", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetQueues(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getqueues", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //var parameter = command.CreateParameter();

                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetBotStartDetails(string botname, string MachineName, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getbotstartdetails", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@MachineName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = MachineName;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        //public DataTable GetBotStartDetailsFromDesktop(string userid, string machinename, int groupid, int tenantid)
        public DataTable GetBotStartDetailsFromDesktop(string userid, string machinename)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getbotstartdetailsfromdesktop", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userid;
                    command.Parameters.Add(parameter);
                    parameter = command.CreateParameter();

                    parameter.ParameterName = "@machinename";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = machinename;
                    command.Parameters.Add(parameter);

                    //AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetStompDetails(string botname, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getstompdetails", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetRQDetails(string botname, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getRQdetails", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetDashboardBots(int groupid, int tenantid, string userid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getdashboardbots", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //var parameter = command.CreateParameter();
                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userid;

                    command.Parameters.Add(parameter);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public int AssignQueueToBot(string strBotId, string queuename, string createdby, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_assignqueuetobot", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strBotId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "queuename";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = queuename;
                    command.Parameters.Add(parameter);

                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParameters(command, createdby);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return 1;
            }
        }

        public int AssignBotToUser(string strBotId, string strUserId, string createdby, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_assignbottouser", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strBotId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strUserId;
                    command.Parameters.Add(parameter);

                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    command = AddAuditParameters(command, createdby);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return 1;
            }
        }

        public DataTable GetQueueToBotMapping(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getqueuebotsmapping", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //var parameter = command.CreateParameter();
                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "@tenantid";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }



        public DataTable GetScheduleStatus(int groupid, int tenantid, string status)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getschedulestatus", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@status";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = status;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        /*To transfer rqdetails on DashBoard*/

        public DataTable GetRQDetailsForBotDashboard(int groupid, int tenantid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = null;



                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_getrqdetailsforbotdashboard", conn);
                        command.CommandType = CommandType.StoredProcedure;



                        var parameter = command.CreateParameter();



                        parameter.ParameterName = "groupid";
                        parameter.DbType = System.Data.DbType.Int32;
                        parameter.Value = groupid;
                        command.Parameters.Add(parameter);


                        parameter = command.CreateParameter();
                        parameter.ParameterName = "tenantid";
                        parameter.DbType = System.Data.DbType.Int32;
                        parameter.Value = tenantid;
                        command.Parameters.Add(parameter);




                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        conn.Close();
                        if (ds.Tables.Count > 0)
                            dt = ds.Tables[0];




                    }
                    catch (Exception exception)
                    {
                        Debug.Write("GetRQDetailsForBotDashboard Exception Inside Try Try: " + exception.Message);



                    }
                }



                return dt; //success
            }
            catch (Exception ex)
            {
                Debug.Write("GetRQDetailsForBotDashboard Exception : " + ex.Message);
                return null; //fail
            }



        }


        public DataTable GetLogsForDashboardBots(string strbotid, string strmachinename, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getLogsForDashboardBots", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@strbotid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strbotid;
                    command.Parameters.Add(parameter);
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@strmachinename";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = strmachinename;
                    command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }
        public DataTable GetUserToBotMapping(string userid, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getusertobotmapping", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@userid";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userid;
                    command.Parameters.Add(parameter);

                    //parameter = command.CreateParameter();
                    //parameter = command.CreateParameter();
                    //parameter.ParameterName = "@tenantId";
                    //parameter.DbType = System.Data.DbType.Int32;
                    //parameter.Value = tenantid;
                    //command.Parameters.Add(parameter);

                    AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        // public int LoginUser(string domainname, string userid, string pwd, int tenantid)
        public DataTable LoginUser(string domainname, string userName, string pwd, string tenantName, string groupName)
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int result = 0; //false

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_loginuser", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "domainname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = domainname;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "userName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = userName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "pwd";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = pwd;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = tenantName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupName;
                    command.Parameters.Add(parameter);

                    var parameterreturn = command.CreateParameter();
                    parameterreturn.ParameterName = "result";
                    parameterreturn.DbType = System.Data.DbType.Int32;
                    parameterreturn.Direction = ParameterDirection.Output;
                    command.Parameters.Add(parameterreturn);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();

                    //UserToken = OperationContext.Current.SessionId; 

                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;


                    //command.ExecuteNonQuery();
                    //conn.Close();
                    //result = Convert.ToInt32(parameterreturn.Value);
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    //return 0; //false
                }
                //if(obj != null)
                //{
                //    i = Convert.ToInt32(obj);
                //}

                return dt;

            }
        }

        public DataTable GetUsers(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getusers", conn);
                command.CommandType = CommandType.StoredProcedure;

                //var parameter = command.CreateParameter();
                //parameter = command.CreateParameter();
                //parameter.ParameterName = "@tenantid";
                //parameter.DbType = System.Data.DbType.Int32;
                //parameter.Value = tenantid;
                //command.Parameters.Add(parameter);

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public DataTable GetAllTenants(int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getAllTenants", conn);
                command.CommandType = CommandType.StoredProcedure;

                //var parameter = command.CreateParameter();
                //parameter = command.CreateParameter();
                //parameter.ParameterName = "@tenantid";
                //parameter.DbType = System.Data.DbType.Int32;
                //parameter.Value = tenantid;
                //command.Parameters.Add(parameter);

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void LogDataToDB(DataTable LogData)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("InsertLogTable", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@mylogtable", LogData));
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public DataTable GetLog(string userid, int groupid, int tenantid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getlog", conn);
                command.CommandType = CommandType.StoredProcedure;

                var parameter = command.CreateParameter();
                parameter = command.CreateParameter();
                parameter.ParameterName = "@userid";
                parameter.DbType = System.Data.DbType.String;
                parameter.Value = userid;
                command.Parameters.Add(parameter);

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        /*check status and update isactive state*/
        public int UpdateIsactiveStatusTenantRelatedTables(int tenantid, int isactive)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_updateIsactiveStatusTenantRelatedTables", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "isactive";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = isactive;
                    command.Parameters.Add(parameter);

                    i = command.ExecuteNonQuery();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                return i;
            }
        }




        public int CheckIsactiveStatusTenant(int tenantid)
        {

            int statusResult = 0;
            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_CheckIsactiveStatusTenant", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantid";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantid;
                    command.Parameters.Add(parameter);

                    SqlParameter prm1 = new SqlParameter("@status", SqlDbType.Int);
                    prm1.Direction = ParameterDirection.Output;
                    command.Parameters.Add(prm1);

                    command.ExecuteNonQuery();

                    statusResult = (int)(command.Parameters["@status"].Value);

                    conn.Close();


                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    return -1;
                }

                return statusResult;
            }
        }
        /*End of Check status*/

        public DataTable GetAuditTrail(int groupid, int tenantid, string userid)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand("um_getaudittrail", conn);
                command.CommandType = CommandType.StoredProcedure;

                //var parameter = command.CreateParameter();
                //parameter = command.CreateParameter();
                //parameter.ParameterName = "@tenantid";
                //parameter.DbType = System.Data.DbType.Int32;
                //parameter.Value = tenantid;
                //command.Parameters.Add(parameter);

                AddGroupAndTenantParametes(ref command, groupid, tenantid);

                //parameter = command.CreateParameter();
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@userid";
                parameter.DbType = System.Data.DbType.String;
                parameter.Value = userid;
                command.Parameters.Add(parameter);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }
        // #region "Designer"
        //public bool PublishProcess(PublishProcessEntity publishProcessEntity)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        int i = 0;

        //        using (SqlConnection conn = GetConnection())
        //        {
        //            SqlCommand command = new SqlCommand("ds_publishAutomation", conn);
        //            command.CommandType = CommandType.StoredProcedure;
        //            var parameter = command.CreateParameter();
        //            parameter.ParameterName = "processname";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = publishProcessEntity.processname;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "processfiles";
        //            parameter.DbType = System.Data.DbType.Binary;
        //            parameter.Value = publishProcessEntity.processfiles;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "version";
        //            parameter.DbType = System.Data.DbType.String;
        //            parameter.Value = publishProcessEntity.version;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "environmentid";
        //            parameter.DbType = System.Data.DbType.Int32;
        //            parameter.Value = 0;
        //            command.Parameters.Add(parameter);

        //            parameter = command.CreateParameter();
        //            parameter.ParameterName = "tenantid";
        //            parameter.DbType = System.Data.DbType.Int32;
        //            parameter.Value = 0;
        //            command.Parameters.Add(parameter);

        //            command = AddAuditParameters(command, publishProcessEntity.createdby);

        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            da.Fill(ds);
        //            conn.Close();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;

        //    }

        //}

        //public DataTable GetProcessDetails(string ProcessName, int groupid, int tenantid)
        public DataTable GetProcessDetails(string ProcessName, string groupname, string tenantname)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("ds_getProcessDetails", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "processname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = groupname;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = tenantname;
                    command.Parameters.Add(parameter);

                    //AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public DataTable DownloadAutomationZipBinary(string ProcessName, string ProcessVersion, string AutomationGroupName,
            string TenantName/*,int groupid,int tenantid*/)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int i = 0;

                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand command = new SqlCommand("ds_downloadprocess", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "processname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = ProcessName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "processversion";
                    parameter.DbType = System.Data.DbType.String;
                    if ((string.IsNullOrEmpty(ProcessVersion)) || (ProcessVersion.Trim().Length == 0))
                    {
                        ProcessVersion = "Latest"; //hardcoded in DB in stored procedure
                    }
                    parameter.Value = ProcessVersion;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "groupname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = AutomationGroupName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "tenantname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = TenantName;
                    command.Parameters.Add(parameter);

                    //  AddGroupAndTenantParametes(ref command, groupid, tenantid);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        #endregion "Designer"

    }
    #endregion
}
#endregion