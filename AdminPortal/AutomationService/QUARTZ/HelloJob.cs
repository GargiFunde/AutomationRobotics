// <copyright file=ControlHighlighterWpfForm.xaml company=E2E BOTS>
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>Piyush Bhiwapurkar</author>
// <date>20-11-2019 16:02:53</date>
// <summary>Scheduled Jobs will give a Hit to Execute Function.</summary>

using Quartz;
using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
using System.Threading.Tasks;
//using System.Web;
using System.Data;
using RabbitMQ.Client;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using CommonLibrary;
using System.Web.Configuration;
using Newtonsoft.Json;

//using AutomationAgent.Service1;

namespace AutomationService
{
    public class HelloJob : IJob
    {
        private QuartzHelper quartzHelper;
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string processName = "BackOfficeBot";
                Process[] foundProcess = Process.GetProcessesByName(processName);
                foreach (Process p in foundProcess)
                {
                     p.Kill();   //to kill backofficebot for both successfully and failure scheduler 
                }
            }
            catch (Exception ex)
            {
            }



            JobKey key = context.JobDetail.Key;
            string jobName = key.Name;

            try
            {
                Debug.Write("Piyush Time from Execute : " + DateTime.Now);
                JobDataMap dataMap = context.JobDetail.JobDataMap;

                //Piyush on 20Nov2019
                #region Declare Variables
                string processName = string.Empty;
                string QueueName = string.Empty;
                string serverName = string.Empty;
                string botName = string.Empty;
                string dateTime = string.Empty;
                string tenantId = string.Empty;
                string chronexpression = string.Empty;
                string groupId = string.Empty;
                int TenantId = 0;
                int GroupId = 0;
                #endregion

                QueueName = dataMap.GetString("QueueName");   //For Scheduling Using Queues i.e. CASE 1
                processName = dataMap.GetString("ProcessName"); //For Scheduling Using Process Name i.e. CASE 2

                if (!string.IsNullOrEmpty(QueueName))
                {
                    serverName = dataMap.GetString("ServerName");
                    botName = dataMap.GetString("BotName");
                    QueueName = dataMap.GetString("QueueName");
                    dateTime = dataMap.GetString("DateTime");
                    tenantId = dataMap.GetString("tenantid");
                    chronexpression = dataMap.GetString("strChronExp");
                    groupId = dataMap.GetString("strGroupid");

                    TenantId = int.Parse(tenantId);
                    GroupId = int.Parse(groupId);
                    DateTime dt = DateTime.UtcNow;
                    DataTable result1 = GetScheduleDetails(QueueName, botName, GroupId, TenantId, dt, chronexpression);

                    /*Calling this Function Locally GetScheduleDetails() and getting rows while comparing current date and Stop Date
                     where StopDate should be greater.*/
                    if ((result1 != null) && (result1.Rows.Count > 0))
                    {
                        DataTable result = GetBotStartDetails(botName); /*Getting Current Process if available in Database or not.*/
                        if ((result != null) && (result.Rows.Count > 0))
                        {
                            AutomationRequest("schedule", botName, QueueName, result, chronexpression); /*Automation Starts.*/
                        }
                        Debug.Write("\n Piyush Execution BotStarting");
                    }
                    else
                    {
                        DataTable result2 = GetScheduleDetailsToDelete(QueueName, botName, GroupId, TenantId, dt);
                        /*Calling this Function Locally GetScheduleDetails() and getting rows while comparing current date and Stop Date
                          where StopDate should be lesser.*/

                        string strId = null;
                        string strQueueName = null;
                        string strBotName = null;
                        string strChronExp = null;
                        int tenantid = 0;
                        string createdby = null;

                        if ((result2 != null) && (result2.Rows.Count > 0))
                        {

                            foreach (DataRow row in result2.Rows)
                            {
                                string jobNameFromDT = (row[2].ToString()) + (row[1].ToString()) + (row[3].ToString());
                                if (jobNameFromDT == jobName)
                                {
                                    chronexpression = (row[3].ToString());
                                    strId = (row[0].ToString());
                                    strQueueName = (row[1].ToString());
                                    strBotName = (row[2].ToString());
                                    strChronExp = (row[3].ToString());
                                    tenantid = Convert.ToInt32((row[5].ToString()));
                                    createdby = (row[6].ToString());
                                    break;
                                }
                            }

                            int resultDeleteSchedule = DeleteSchedule(strId, strQueueName, strBotName, strChronExp, tenantid, createdby);

                            if (1 == resultDeleteSchedule)
                            {
                                Debug.Write("\n Succcessfully Deleted");
                            }
                            else
                            {
                                Debug.Write("\n NOt Deleted");
                            }
                            Debug.Write("\n *******Piyush Execution Stopped*******");

                        }
                    }
                }

                else if (!string.IsNullOrEmpty(processName))
                {
                    serverName = dataMap.GetString("ServerName");
                    botName = dataMap.GetString("BotName");
                    processName = dataMap.GetString("ProcessName");
                    dateTime = dataMap.GetString("DateTime");
                    tenantId = dataMap.GetString("tenantid");
                    chronexpression = dataMap.GetString("strChronExp");
                    groupId = dataMap.GetString("strGroupid");

                    TenantId = int.Parse(tenantId);
                    GroupId = int.Parse(groupId);
                    DateTime dt = DateTime.UtcNow;
                    DataTable result1 = GetScheduleDetailsForProcess(processName, botName, GroupId, TenantId, dt, chronexpression);

                    /*Calling this Function Locally GetScheduleDetails() and getting rows while comparing current date and Stop Date
                     where StopDate should be greater.*/
                    if ((result1 != null) && (result1.Rows.Count > 0))
                    {
                        DataTable result = GetBotStartDetails(botName); /*Getting Current Process if available in Database or not.*/
                        if ((result != null) && (result.Rows.Count > 0))
                        {
                            AutomationRequestForProcess("schedule", botName, processName, result, chronexpression); /*Automation Starts.*/
                        }
                        Debug.Write("\n Piyush Execution BotStarting");
                    }
                    else
                    {
                        DataTable result2 = GetScheduleDetailsToDelete(QueueName, botName, GroupId, TenantId, dt);
                        /*Calling this Function Locally GetScheduleDetails() and getting rows while comparing current date and Stop Date
                          where StopDate should be lesser.*/

                        string strId = null;
                        string strQueueName = null;
                        string strBotName = null;
                        string strChronExp = null;
                        int tenantid = 0;
                        string createdby = null;

                        if ((result2 != null) && (result2.Rows.Count > 0))
                        {

                            foreach (DataRow row in result2.Rows)
                            {
                                string jobNameFromDT = (row[2].ToString()) + (row[1].ToString()) + (row[3].ToString());
                                if (jobNameFromDT == jobName)
                                {
                                    chronexpression = (row[3].ToString());
                                    strId = (row[0].ToString());
                                    strQueueName = (row[1].ToString());
                                    strBotName = (row[2].ToString());
                                    strChronExp = (row[3].ToString());
                                    tenantid = Convert.ToInt32((row[5].ToString()));
                                    createdby = (row[6].ToString());
                                    break;
                                }
                            }

                            int resultDeleteSchedule = DeleteSchedule(strId, strQueueName, strBotName, strChronExp, tenantid, createdby);

                            if (1 == resultDeleteSchedule)
                            {
                                Debug.Write("\n Succcessfully Deleted");
                            }
                            else
                            {
                                Debug.Write("\n NOt Deleted");
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Debug.Write("Exception Caught in Execute : " + ex.Message);
            }
        }

     

        public bool AutomationRequest(string action, string botname, string QueueName, DataTable result, string chronExpression)
        {
            string UserId = string.Empty;
            string BotId = string.Empty;
            string Password = string.Empty;
            string RobotPwd = string.Empty;
            int TenantId = 0;
            int GroupId = 0;
            string ServerName = string.Empty;
            string MachineName = string.Empty;
            int PortNumber = 0;

            BOTService botServiceClient = new BOTService();
            try
            {
                if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
                    UserId = (string)result.Rows[0][1];
                if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
                    Password = (string)result.Rows[0][2];
                if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                    TenantId = (int)result.Rows[0][3];
                if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                    GroupId = (int)result.Rows[0][4];
                if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
                    ServerName = (string)result.Rows[0][7];
                if ((result.Rows[0][8] != null) && (result.Rows[0][8] != System.DBNull.Value))
                    PortNumber = (int)result.Rows[0][8];
                if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
                    MachineName = (string)result.Rows[0][10];
                if ((result.Rows[0][11] != null) && (result.Rows[0][11] != System.DBNull.Value))
                    BotId = (string)result.Rows[0][11];
                if ((result.Rows[0][12] != null) && (result.Rows[0][12] != System.DBNull.Value))
                    RobotPwd = (string)result.Rows[0][12];
                string message = action + "!#~=~!#" + botname + "!#~=~!#" + BotId + "!#~=~!#" + RobotPwd + "!#~=~!#" + TenantId + "!#~=~!#" + GroupId + "!#~=~!#" + QueueName + "!#~=~!#" + chronExpression;

                var factory = new ConnectionFactory() { HostName = ServerName };
                factory.UserName = UserId;
                factory.Password = Password;
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //BOTService botServiceClient = new BOTService();
                    int result1 = botServiceClient.PiyushLogs("Process Started By Piyush in Hello Job");
                    int ScheduleStatus = botServiceClient.CreateScheduleStatus(QueueName, botname, chronExpression, "STARTED", GroupId, TenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), "");

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = 1; //non persistent
                    props.Headers = new Dictionary<string, object>();

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: MachineName,
                                         basicProperties: props,
                                         body: body);
                }
                return true; //acknowledgement
            }
            catch (Exception ex)
            {
                int ScheduleStatus = botServiceClient.CreateScheduleStatus(QueueName, botname, chronExpression, "ERROR", GroupId, TenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToLocalTime().ToString());
                int ScheduleError = botServiceClient.PiyushLogs("Error in HelloJob : " + ex.Message);
                return false;
            }

        }


        #region Automation Schedule Request for Process
        public bool AutomationRequestForProcess(string action, string botname, string ProcessName, DataTable result, string chronExpression)
        {
            string UserId = string.Empty;
            string BotId = string.Empty;
            string Password = string.Empty;
            string RobotPwd = string.Empty;
            int TenantId = 0;
            int GroupId = 0;
            string ServerName = string.Empty;
            string MachineName = string.Empty;
            int PortNumber = 0;

            BOTService botServiceClient = new BOTService();
            try
            {
                if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
                    UserId = (string)result.Rows[0][1];
                if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
                    Password = (string)result.Rows[0][2];
                if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                    TenantId = (int)result.Rows[0][3];
                if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                    GroupId = (int)result.Rows[0][4];
                if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
                    ServerName = (string)result.Rows[0][7];
                if ((result.Rows[0][8] != null) && (result.Rows[0][8] != System.DBNull.Value))
                    PortNumber = (int)result.Rows[0][8];
                if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
                    MachineName = (string)result.Rows[0][10];
                if ((result.Rows[0][11] != null) && (result.Rows[0][11] != System.DBNull.Value))
                    BotId = (string)result.Rows[0][11];
                if ((result.Rows[0][12] != null) && (result.Rows[0][12] != System.DBNull.Value))
                    RobotPwd = (string)result.Rows[0][12];
                string message = action + "!#~=~!#" + botname + "!#~=~!#" + BotId + "!#~=~!#" + RobotPwd + "!#~=~!#" + TenantId + "!#~=~!#" + GroupId + "!#~=~!#" + "robot.q.Process" + "!#~=~!#" + chronExpression;

                var factory = new ConnectionFactory() { HostName = ServerName };
                factory.UserName = UserId;
                factory.Password = Password;
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //var props = channel.CreateBasicProperties();
                    //props.DeliveryMode = 1; //non persistent
                    //props.Headers = new Dictionary<string, object>();

                    //Logger.Log.Logger.LogData("ScheduleAdded", Logger.LogLevel.Debug);

                    /*load Generator Code*/
                    if (action == "Start" || "schedule" == action)
                    {
                        try
                        {
                            ConnectionFactory factory1 = new ConnectionFactory();

                            factory1.HostName = "localhost";
                            factory1.UserName = "guest";
                            factory1.Password = "guest";

                            using (var connection1 = factory.CreateConnection())
                            {
                                using (var channel1 = connection.CreateModel())
                                {
                                    //channel1.QueuePurge("robot.q.automation");
                                }
                            }
                            //BOTService AutomationRequest = new BOTService();

                            RequestInput _requestInput = new RequestInput();

                            _requestInput.TenantName = "InnoWise";                                  // need to remove Hardcoding.
                            _requestInput.AutomationGroupName = "Default";
                            _requestInput.AutomationProcessName = ProcessName;
                            List<string> lstSearchParam = new List<string>();
                            lstSearchParam.Add("SearchField1");
                            lstSearchParam.Add("Infosys Ltd.");
                            _requestInput.InputSearchParameters = lstSearchParam;
                            _requestInput.RequestNumber = "100";
                            _requestInput.RequestTimeoutSLAInSeconds = 100000;
                            AutomationRequest(_requestInput);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                        
                    
                    //BOTService botServiceClient = new BOTService();
                    int result1 = botServiceClient.PiyushLogs("Process Started By Piyush in Hello Job");
                    int ScheduleStatus = botServiceClient.CreateScheduleStatus("robot.q.Process", botname, chronExpression, "STARTED", GroupId, TenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), "");

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = 1; //non persistent
                    props.Headers = new Dictionary<string, object>();

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: MachineName,
                                         basicProperties: props,
                                         body: body);
                }
                return true; //acknowledgement
            }
            catch (Exception ex)
            {
                int ScheduleStatus = botServiceClient.CreateScheduleStatus("robot.q.Process", botname, chronExpression, "ERROR", GroupId, TenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToLocalTime().ToString());
                int ScheduleError = botServiceClient.PiyushLogs("Error in HelloJob : " + ex.Message);
                return false;
            }

        }
        #endregion

        public bool AutomationRequest(RequestInput _requestInput)
        {
            //string strExchangeTopic = ConfigurationManager.AppSettings["ExchangeTopicName"];
            //string strRoutingLevel = ConfigurationManager.AppSettings["RoutingSupport"];

            string strExchangeTopic = string.Empty;
            string strRoutingLevel = string.Empty;
            string routingKey = string.Empty;
            BOTService client = new BOTService();

            strExchangeTopic = "robot.x.automation";
            strRoutingLevel = "ProfileLevel";

            DataTable dt = client.GetRQDetailsByName(_requestInput.AutomationGroupName, _requestInput.TenantName);

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

                    //var message = RobotLibrary.XMLHelper.Serialize(_requestInput);
                    var message = JsonConvert.SerializeObject(_requestInput);
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

        private SqlConnection GetConnection()
        {
            string connstring = ConfigurationManager.ConnectionStrings["ConnectionStringSql"].ConnectionString;
            // Making connection with Npgsql provider
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            return conn;
        }

        /*Added on 09-July-2019*/
        public DataTable GetScheduleDetails(string QueueName, string botName, int groupid, int tenantId, DateTime date, String chronexpression)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            string date1 = date.ToString("dd MMMM yyyy hh:mm:ss");

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getscheduleDetailsStopStatus", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@queueName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = QueueName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@groupId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@dateNow";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = date1;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@chronexpression";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = chronexpression;
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
                    Debug.Write("Error In GetBotStartDetails SQL : " + ex.Message);

                }
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                return dt;
            }
        }

        //Added on 20-Nov-2019
        public DataTable GetScheduleDetailsForProcess(string processName, string botName, int groupid, int tenantId, DateTime date, String chronexpression)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            string date1 = date.ToString("dd MMMM yyyy hh:mm:ss");

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getscheduleDetailsStopStatusforProcess", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@processName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = processName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@groupId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@dateNow";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = date1;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@chronexpression";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = chronexpression;
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
                    Debug.Write("Error In GetBotStartDetails SQL : " + ex.Message);

                }
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                return dt;
            }
        }


        /*Added on 09-July-2019*/
        public DataTable GetScheduleDetailsToDelete(string QueueName, string botName, int groupid, int tenantId, DateTime date)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            string date1 = date.ToString("dd MMMM yyyy hh:mm:ss");

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getscheduleStop&Delete", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@queueName";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = QueueName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@groupId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = groupid;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@tenantId";
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = tenantId;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.ParameterName = "@dateNow";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = date1;
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
                    Debug.Write("Error In GetBotStartDetails SQL : " + ex.Message);

                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }

        public int DeleteSchedule(string strId, string strQueueName, string strBotName, string strChronExp, int tenantid, string createdby)
        {
            try
            {
                strChronExp = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(strChronExp);

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
                        parameter.ParameterName = "tenantid";
                        parameter.DbType = System.Data.DbType.Int32;
                        parameter.Value = tenantid;
                        command.Parameters.Add(parameter);
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
        public int DeleteScheduleTrigger(string trigger_name)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int i = 0;

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    SqlCommand command = new SqlCommand("um_deleteScheduleTrigger", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "trigger_name";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = trigger_name;
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

        /*    public DataTable GetAllScheduleDetails(string QueueName, string botName)
            {
                DataSet ds = new DataSet();
                DataTable dt = null;


                using (SqlConnection conn = GetConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("um_getScheduleDetails", conn);
                        command.CommandType = CommandType.StoredProcedure;

                        var parameter = command.CreateParameter();

                        parameter.ParameterName = "@queueName";
                        parameter.DbType = System.Data.DbType.String;
                        parameter.Value = QueueName;
                        command.Parameters.Add(parameter);

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
                    {
                        dt = ds.Tables[0];
                    }

                    return dt;
                }
            }
    */

        public DataTable GetBotStartDetails(string botname)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand("um_getbotstartdetailsforschedule", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = command.CreateParameter();

                    parameter.ParameterName = "@botname";
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = botname;
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
                    Debug.Write("Error In HelloJob SQL : " + ex.Message);

                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }
    }
}