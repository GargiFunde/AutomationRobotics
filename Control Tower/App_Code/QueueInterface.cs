using System;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Data;
using System.Text;
//using Quartz.Logging;
//using Logger.ServiceReference1;

public class QueueInterface
{
    public static int i = 1;
    public void TriggerStartStop(string action, string botname, string strmachinename,int groupid, int tenantid, string userid)
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable result = serviceInterface.GetBotStartDetails(botname, strmachinename,groupid,tenantid);
        if ((result != null) && (result.Rows.Count > 0))
        {
            AutomationRequest(action, botname, result);
        }
    }

    public bool AutomationRequest(string action, string botname, DataTable result)
    {
        int groupid = 0;
        int TenanatId = 0;

        try
        {
            string UserId = string.Empty;
            string BotId = string.Empty;
            string Password = string.Empty;
            string RobotPwd = string.Empty;

            string ServerName = string.Empty;
            string MachineName = string.Empty;
            int PortNumber = 0;
            string QueueName = string.Empty;

            if ((result.Rows[0][1] != null) && (result.Rows[0][1] != System.DBNull.Value))
                UserId = (string)result.Rows[0][1];
            if ((result.Rows[0][2] != null) && (result.Rows[0][2] != System.DBNull.Value))
                Password = (string)result.Rows[0][2];
            if ((result.Rows[0][3] != null) && (result.Rows[0][3] != System.DBNull.Value))
                TenanatId = (int)result.Rows[0][3];
            if ((result.Rows[0][4] != null) && (result.Rows[0][4] != System.DBNull.Value))
                groupid = (int)result.Rows[0][4];
            //if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
            //    ServerName = (string)result.Rows[0][7];
            if ((result.Rows[0][5] != null) && (result.Rows[0][5] != System.DBNull.Value))
                ServerName = (string)result.Rows[0][5];
            //ServerName = (string) "IWPUNLPT0090";
            //if ((result.Rows[0][8] != null) && (result.Rows[0][8] != System.DBNull.Value))
            //    PortNumber = (int)result.Rows[0][8];
            if ((result.Rows[0][6] != null) && (result.Rows[0][6] != System.DBNull.Value))
                PortNumber = (int)result.Rows[0][6];
            //if ((result.Rows[0][9] != null) && (result.Rows[0][9] != System.DBNull.Value))
            //    QueueName = (string)result.Rows[0][9];
            if ((result.Rows[0][7] != null) && (result.Rows[0][7] != System.DBNull.Value))
                QueueName = (string)result.Rows[0][7];
            if ((result.Rows[0][8] != null) && (result.Rows[0][8] != System.DBNull.Value))
                MachineName = (string)result.Rows[0][8];
            if ((result.Rows[0][9] != null) && (result.Rows[0][9] != System.DBNull.Value))
                BotId = (string)result.Rows[0][9];
            if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
                RobotPwd = (string)result.Rows[0][10];

            //if ((result.Rows[0][10] != null) && (result.Rows[0][10] != System.DBNull.Value))
            //    MachineName = (string)result.Rows[0][10];
            //if ((result.Rows[0][11] != null) && (result.Rows[0][11] != System.DBNull.Value))
            //    BotId = (string)result.Rows[0][11];
            //if ((result.Rows[0][12] != null) && (result.Rows[0][12] != System.DBNull.Value))
            //    RobotPwd = (string)result.Rows[0][12];

            string message = action + "!#~=~!#" + botname + "!#~=~!#" + BotId + "!#~=~!#" + RobotPwd + "!#~=~!#" +
                             TenanatId + "!#~=~!#" + groupid + "!#~=~!#" + QueueName; /*Added Tenant Id*/


            #region inserting to queue
            var factory = new ConnectionFactory() { HostName = ServerName };
            factory.UserName = UserId;
            factory.Password = Password;
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 1; //non persistent
                props.Headers = new Dictionary<string, object>();

                //Logger.Log.Logger.LogData("ScheduleAdded", Logger.LogLevel.Debug);

                /*load Generator Code*/
                //if (i == 1)
                //{
                //    if (action == "Start")
                //    {
                //        try
                //        {
                //            ConnectionFactory factory1 = new ConnectionFactory();

                //            factory1.HostName = "localhost";
                //            factory1.UserName = "guest";
                //            factory1.Password = "guest";

                //            using (var connection1 = factory.CreateConnection())
                //            {
                //                using (var channel1 = connection.CreateModel())
                //                {
                //                    channel1.QueuePurge("robot.q.automation");
                //                }
                //            }
                //            ServiceReference1.BOTServiceClient AutomationRequest = new ServiceReference1.BOTServiceClient();

                //            RequestInput _requestInput = new RequestInput();

                //            _requestInput.TenantName = "InnoWise";
                //            _requestInput.AutomationGroupName = "Default";
                //            _requestInput.AutomationProcessName = "Piyush";
                //            List<string> lstSearchParam = new List<string>();
                //            lstSearchParam.Add("SearchField1");
                //            lstSearchParam.Add("Infosys Ltd.");
                //            _requestInput.RequestNumber = "100";
                //            _requestInput.RequestTimeoutSLAInSeconds = 100000;
                //            ServiceReference1.BOTServiceClient client = new ServiceReference1.BOTServiceClient();
                //            client.AutomationRequest(_requestInput);
                //            i++;
                //        }
                //        catch (Exception e)
                //        {
                //        }

                //    }
                //}
                //else
                //{
                //    if (action == "Start")
                //    {
                //        try
                //        {
                //            ConnectionFactory factory1 = new ConnectionFactory();

                //            factory1.HostName = "localhost";
                //            factory1.UserName = "guest";
                //            factory1.Password = "guest";

                //            using (var connection1 = factory.CreateConnection())
                //            {
                //                using (var channel1 = connection.CreateModel())
                //                {
                //                    channel1.QueuePurge("robot.q.automation");
                //                }

                //            }

                //            ServiceReference1.BOTServiceClient AutomationRequest = new ServiceReference1.BOTServiceClient();

                //            RequestInput _requestInput = new RequestInput();

                //            _requestInput.TenantName = "InnoWise";
                //            _requestInput.AutomationGroupName = "Default";
                //            _requestInput.AutomationProcessName = "Piyush";
                //            List<string> lstSearchParam = new List<string>();
                //            lstSearchParam.Add("SearchField1");
                //            lstSearchParam.Add("Infosys Ltd.");
                //            //_requestInput.InputSearchParameters = lstSearchParam;
                //            _requestInput.RequestNumber = "100";
                //            _requestInput.RequestTimeoutSLAInSeconds = 100000;
                //            ServiceReference1.BOTServiceClient client = new ServiceReference1.BOTServiceClient();
                //            client.AutomationRequest(_requestInput);

                //        }
                //        catch (Exception e)
                //        {
                //            Console.WriteLine(e);
                //            throw;
                //        }

                //    }

                //}
                #endregion

                ServiceInterface botServiceClient = new ServiceInterface();

                if (action == "Start")
                {
                    //Logger.Log.Logger.LogData("Process Started Successfully", LogLevel.Info);
                    botServiceClient.CreateScheduleStatus(QueueName, botname, "Manually Started", action, groupid, TenanatId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), "");
                    botServiceClient.insertLog("Robot " + botname + "has been Started.", "Robot " + botname + " has been started Successfully at " + DateTime.Now + ".For Queue" + QueueName + " and Server : " + ServerName + ".", groupid, TenanatId);
                    
                }
                else if (action == "Stop")
                {
                    botServiceClient.CreateScheduleStatus(QueueName, botname, "Manually Stopped", action, groupid, TenanatId, "", DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                    botServiceClient.insertLog("Robot " + botname + " is trying to Stop.", "Robot " + botname + " is stopping at " + DateTime.Now + ".For Queue" + QueueName + " and Server : " + ServerName + ".", groupid, TenanatId);
                }
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
            Console.WriteLine("Error in Queue Interface", ex.Message);
            //Logger.Log.Logger.LogData("Queue Interface Error", Logger.LogLevel.Error,groupid,TenanatId);
            return false;
        }



    }
}
