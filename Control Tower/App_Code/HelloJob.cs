using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using RabbitMQ.Client;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace AutomationService
{
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Debug.Write("Piyush Time from Execute : "+DateTime.Now);
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string serverName = dataMap.GetString("ServerName");
                string botName = dataMap.GetString("BotName");
                string QueueName = dataMap.GetString("QueueName");
                string dateTime = dataMap.GetString("DateTime");

                DataTable result = GetBotStartDetails(botName);
                if ((result != null) && (result.Rows.Count > 0))
                {
                    AutomationRequest("schedule", botName, QueueName, result);
                }

                //for (int i = 0; i < 5; i++)
                //{
                //    //Logger.Log.Logger.LogData($"Running step {i} on {serverName} server at {dateTime}",LogLevel.Info);
                //    await Console.Out.WriteLineAsync($"Running step {i} on {serverName} server at {dateTime}");

                //    Thread.Sleep(2000);
                //}
                //Logger.Log.Logger.LogData("Hello Job completing", LogLevel.Info);
            }
            catch(Exception ex)
            {
                Debug.Write("\n Exception in Execute Try : "+ex.Message);
            }
        }

        public bool AutomationRequest(string action, string botname,string QueueName, DataTable result)
        {
            string UserId = string.Empty;
            string BotId = string.Empty;
            string Password = string.Empty;
            string RobotPwd = string.Empty;
            int TenanatId = 0;
            string ServerName = string.Empty;
            string MachineName = string.Empty;
            int PortNumber = 0;

            if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
                UserId = (string)result.Rows[0][1];
            if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
                Password = (string)result.Rows[0][2];
            if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                TenanatId = (int)result.Rows[0][3];
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
            string message = action + "!#~=~!#" + botname + "!#~=~!#" + BotId + "!#~=~!#" + RobotPwd + "!#~=~!#" + TenanatId + "!#~=~!#" + QueueName;


            var factory = new ConnectionFactory() { HostName = ServerName };
            factory.UserName = UserId;
            factory.Password = Password;
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

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

        private SqlConnection GetConnection()
        {
            string connstring = ConfigurationManager.ConnectionStrings["ConnectionStringSql"].ConnectionString;
            // Making connection with Npgsql provider
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            return conn;
        }
        public DataTable GetBotStartDetails(string botname)
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
                    Debug.Write("Error In HelloJob SQL : "+ex.Message);

                }
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                return dt;
            }
        }
    }
}