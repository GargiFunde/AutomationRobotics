///-----------------------------------------------------------------
///   Namespace:      WpfFormLibrary.ViewModel
///   Class:                RMQueue
///   Description:    <Description>
///   Author:         B.Piyush                    Date: <DateTime>
///   Notes:          <Notes>
///   Revision History:
///   Team:   E2E Team
///   Name:           Date:        Description:
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using RobotLibrary;
using RabbitMQListenerRobot;
using System.Activities.Presentation;
using System.IO;
using System.Activities.XamlIntegration;
using System.Runtime.Serialization.Json;
using System.Configuration;
using Logger;
using TCP.Client;
using System.Data;
using BotDesignCommon.Helpers;
using System.IO.Compression;
using System.Windows;
using SelectHelper = CommonLibrary.SelectHelper;
using Newtonsoft.Json;

namespace WpfFormLibrary.ViewModel
{
    public class RMQueue
    {
        [Browsable(false)]
        public RequestInput QueueRequestInputs { get; set; }
        public string AttachWorkflowPath { get; set; }
        IMessageAdapter messageAdapter = null;
        public Dictionary<string, string> ConnectionAttributes { get; set; }
        public string ServerName { get; set; }
        public string UserId { get; set; }
        public int PortNumber { get; set; }
        public string Password { get; set; }
        HeartBeat _heartbeat = null;
        RoboExecutionStatus RobotExecutionStatus;
        private object delivarytag;
        System.Timers.Timer tmr_Heartbeats = null;
        string RoutingKeyNotifiation = string.Empty;
        string strRobotName = string.Empty;
        bool stopflag = false;
        string strCurrentBuildPath = string.Empty;
        string RobotName = string.Empty;
        //int TenanatId = 0;4
       // int EnvironmentId = 0;
        int tenantId = 0;
        int groupId = 0;
        string ExchangeTopicName = string.Empty;
        string RoutingKey = string.Empty;
        string QueueName = string.Empty;
        string MachineName = string.Empty;
        int processGroupId = 0;
        int processTenantId = 0;
        string chronExpression = string.Empty;
        Logger.ServiceReference1.BOTServiceClient WCFServiceReference = new Logger.ServiceReference1.BOTServiceClient();

        public Client client { get; set; }

        public RMQueue()
        {
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Calling constructor of RMQ - \n");
            log4net.Config.XmlConfigurator.Configure();
            strCurrentBuildPath = ConfigurationManager.AppSettings["CurrentBuildPath"];
        }
        public void ExecuteAction(string inputs)
        {
            //File.AppendAllText(@"c:\Piyush\file.txt", "list: " + inputs + "\n");
            try
            {
                string[] list = null;
                list = inputs.Split(new string[] { "!#~=~!#" }, StringSplitOptions.None);
                string action = list[0];
                if (list.Length > 1)
                {
                    string botname = list[1];
                    Log.BotName = botname;
                }

                if (list.Length > 2)
                {
                    string botid = list[2];
                }

                if (list.Length > 4)
                {
                    string tenantid = list[4];
                    Log.TenantName = tenantid; //Need tenanat name frm DB
                    processGroupId = int.Parse(list[5]);
                    processTenantId = int.Parse(list[4]);
                    strRobotName = list[1];

                    Log.GroupId = processGroupId;
                    Log.TenantId = processTenantId;
                    Logger.Log.Logger.LogData("Execute Action Process Starting ",LogLevel.Info);
                    Logger.Log.Logger.LogData("RMQueue.cs Execute Action processGroupId"+ processGroupId, LogLevel.Info);
                    Logger.Log.Logger.LogData("RMQueue.cs Execute Action processTenantId"+ processTenantId, LogLevel.Info);
                    Logger.Log.Logger.LogData("RMQueue.cs Execute Action list[1] : "+list[1], LogLevel.Info);
                    //File.AppendAllText(@"c:\Piyush\file.txt", "processGroupId: " + processGroupId + "\n");
                    //File.AppendAllText(@"c:\Piyush\file.txt", "processTenantId: " + processTenantId + "\n");
                    //File.AppendAllText(@"c:\Piyush\file.txt", "list[1]: " + list[1] + "\n");
                }
                //MessageBox.Show("Complete String 3: " + inputs + "Action : " + action);

                if ((action != string.Empty) && (action != null))
                {

                    if (action.ToLower() == "processstart")
                    {
                        //File.AppendAllText(@"c:\Piyush\file.txt", "strRobotName: " + strRobotName + " \n " + "processGroupId: " + processGroupId + " \n " + "processTenantId: " + processTenantId + " \n ");
                        Logger.Log.Logger.LogData("strRobotName: " + strRobotName + " \n " + "processGroupId: " + processGroupId + " \n " + "processTenantId: " + processTenantId + " \n ", LogLevel.Info);
                        TriggerStart(strRobotName, processGroupId, processTenantId);
                    }
                    else if(action.ToLower() == "schedule")
                    {
                        //if (list.Length > 5)
                            if (list.Length > 6)
                        {
                            //File.AppendAllText(@"c:\Piyush\file.txt", "RMQUEUE SCHEDULE => strRobotName: " + strRobotName + " \n " + "processGroupId: " + processGroupId + " \n " + "processTenantId: " + processTenantId + " \n ");
                            Log.GroupId = processGroupId;
                            Log.TenantId = processTenantId;
                            Logger.Log.Logger.LogData("Scheduled Process Started", LogLevel.Info);
                            QueueName = list[6];
                            chronExpression = list[7];
                        }
                        Start();
                    }
                    else if (action.ToLower() == "start")
                    {
                       // System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "in Start \n");

                        try
                        {
                            Log.GroupId = processGroupId;
                            Log.TenantId = processTenantId;


                            Logger.Log.Logger.LogData("Process Started After Clicking Play Button", LogLevel.Info);
                            Start();
                        }
                        catch (Exception ex)
                        {
                       //   System.IO.File.AppendAllText(@"C:Users\Vinay Dighade\Desktop\BackofficeBot.txt", "start Error  \n" + ex.Message);

                        }
                    }
                    else if (action.ToLower() == "stop")
                    {
                        //File.AppendAllText(@"c:\Piyush\file.txt", "RMQUEUE STOP => strRobotName: " + strRobotName + " \n " + "processGroupId: " + processGroupId + " \n " + "processTenantId: " + processTenantId + " \n ");
                        if (RobotExecutionStatus == RoboExecutionStatus.RobotProcessingAutomation)
                        {
                            Log.GroupId = processGroupId;
                            Log.TenantId = processTenantId;
                            Logger.Log.Logger.LogData("Process Stopped After Clicking Stopped Button", LogLevel.Info);
                            stopflag = true;
                        }
                        else
                        {
                            if (client != null)
                            {
                                client.Disconnect();
                            }
                            if (messageAdapter != null)
                            {
                                try
                                {
                                    messageAdapter.UnSubscribeFromChannels();
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error in RMQueue Details during disconnecting adapter:" + ex.Message, LogLevel.Error);
                                }
                            }
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "RMQueue.cs=>ExecuteAction Exception: " + ex.Message + "\n");
                MessageBox.Show("In Catch Baba : "+ex.Message);
                Logger.Log.Logger.LogData("Error in RMQueue Details ExecuteAction:" + ex.Message, LogLevel.Error);
            }
        }
        public void TriggerStart(string botid,int groupid, int tenantid)
        {
            try
            {
                Logger.Log.Logger.LogData("Getting BotStart Details. GroupId : " + groupid + " .TenantId : " + tenantId + " .BotId: " + botid, LogLevel.Info);
                //File.AppendAllText(@"c:\Piyush\file.txt", "Getting BotStart Details. GroupId : "+groupid+" .TenantId : "+ tenantId+ " .BotId: "+botid + "\n");
                DataTable result = GetBotStartDetails();                 //Piyush => Sending groupId and TenantId
                //DataTable result = GetBotStartDetails(botid,groupid,tenantid);

                //File.AppendAllText(@"c:\Piyush\file.txt", "botid: " + botid + " \n " + "groupid: " + groupid + " \n " + "tenantid: " + tenantid + " \n ");

                if ((result != null) && (result.Rows.Count > 0))
                {
                    Logger.Log.Logger.LogData("Trigger Start DataTable Result Not Null", LogLevel.Info);
                    //File.AppendAllText(@"c:\Piyush\file.txt", "Trigger Start DataTable Result Not Null"+ " \n ");
                    AutomationRequest(result);
                    TCPClientLoginEntryToServer();                             //Sending the Request to Start the Process from Queue
                }
                else
                {
                    Logger.Log.Logger.LogData("No Bot Start Details Found", LogLevel.Info);
                    //File.AppendAllText(@"c:\Piyush\file.txt", "No Bot Start Details Found" + "\n");
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception inGetting Bot Details : "+ex.Message, LogLevel.Info);
                //File.AppendAllText(@"c:\Piyush\file.txt", "Exception inGetting Bot Details" + "\n");
                Logger.Log.Logger.LogData("Error in RMQueue Details TriggerStart:" + ex.Message, LogLevel.Error);
            }
        }
        private bool AutomationRequest(DataTable result)
        {
            try
            {
                if (result.Rows[0][0] != null)
                    RobotName = (string)result.Rows[0][0];
                if (result.Rows[0][1] != null)
                    UserId = (string)result.Rows[0][1];
                if (result.Rows[0][2] != null)
                    Password = (string)result.Rows[0][2];
                if (result.Rows[0][3] != null)
                    tenantId = (int)result.Rows[0][3];
                if (result.Rows[0][4] != null)
                    groupId = (int)result.Rows[0][4];
                if (result.Rows[0][5] != null)
                    ExchangeTopicName = (string)result.Rows[0][5];
                if (result.Rows[0][6] != null)
                    RoutingKey = (string)result.Rows[0][6];
                if (result.Rows[0][7] != null)
                    ServerName = (string)result.Rows[0][7];
                if (result.Rows[0][8] != null)
                    PortNumber = (int)result.Rows[0][8];
                if ((result.Rows[0][9] != null))
                    QueueName = (string)result.Rows[0][9];
                if (result.Rows[0][10] != null)
                    MachineName = (string)result.Rows[0][10];

                strRobotName = RobotName;

                processGroupId = tenantId;
                processTenantId = groupId;
              //System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Automation Request \n" + strRobotName + processGroupId + processTenantId);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in RMQueue Details AutomationRequest: " + ex.Message, LogLevel.Info);
                //File.AppendAllText(@"c:\Piyush\file.txt", "Error in RMQueue Details AutomationRequest:" + ex.Message + "\n");
                Logger.Log.Logger.LogData("Error in RMQueue Details AutomationRequest:" + ex.Message, LogLevel.Error);
            }
            return false;
        }
       
        void Start(string ScheduleQueueName = "" )
        {
            try
            {
                Logger.Log.Logger.LogData("Starting The Process from Start RMQUEUE.cs " , LogLevel.Info);
                //File.AppendAllText(@"c:\Piyush\file.txt", "Starting The Process" + "\n");
                long HeartBeatinterval = Convert.ToInt64(ConfigurationManager.AppSettings["HeartBeatinterval"]);


                RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"]; //it should come from DB
                tmr_Heartbeats = new System.Timers.Timer(HeartBeatinterval);
                tmr_Heartbeats.Elapsed += tmr_Heartbeats_Elapsed;
                _heartbeat = new HeartBeat();
                _heartbeat.RoboColor = "";
                _heartbeat.UserName = Environment.UserName; //Current machines user name 
                _heartbeat.MachineName = Environment.MachineName;
                _heartbeat.RobotName = strRobotName;
                _heartbeat.ProcessName = "";
                _heartbeat.TenantId = tenantId;

                RobotExecutionStatus = RoboExecutionStatus.RobotLaunching;

                String Message = "Robot and Process Started";
                int result = 0;

                if (bOTServiceClient == null)
                {
                    //bOTServiceClient = new ServiceReference1.BotServiceClient();
                    bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();
                    bOTServiceClient.PiyushLogs("START inside chronExpression : " + chronExpression);
                }

                Logger.Log.Logger.LogData("Process and Robot Starting",LogLevel.Info);

                Message = "Robot and Process Started";
                result = bOTServiceClient.InsertIntoLogger(_heartbeat.MachineName, _heartbeat.UserName, _heartbeat.RobotName, _heartbeat.ProcessName, DateTime.UtcNow, "Logger.Log", Message,
                   processGroupId, processTenantId);

                RabbitMQConnection RMQConnection = new RabbitMQConnection();
                RMQConnection.HostName = ServerName;
                RMQConnection.PortNumber = Convert.ToInt32(PortNumber);
                RMQConnection.UserName = UserId;  //RabbitMq  user name 
                RMQConnection.Password = Password; //RabbitMq  PWd

                RabbitMQQueue subscriberQueue = new RabbitMQQueue();
                // RabbitMQQueue rabbitMQSubQueue = RabbitMQ_QueuePublishSubscribe(subscriberQueue, SubscriberExchangeName, SubscriberExchangeType, SubscriberQueueName, SubscriberRoutingKey, SubscriberCriteriaFlag);
                RabbitMQQueue rabbitMQSubQueue = RabbitMQ_QueuePublishSubscribe(subscriberQueue, ExchangeTopicName, "topic", QueueName, RoutingKey, "AutomationQueue");
                RMQConnection.SubscriberQueueDetails.Add(rabbitMQSubQueue);

                RabbitMQQueue publisherQueue = new RabbitMQQueue();
                //RabbitMQQueue rabbitMQQueue = RabbitMQ_QueuePublishSubscribe(publisherQueue, PublisherSuccessExchangeName, PublisherSuccessExchangeType, PublisherSuccessQueueName, PublisherSuccessRoutingKey, PublisherSuccessCriteriaFlag);
                RabbitMQQueue rabbitMQQueue = RabbitMQ_QueuePublishSubscribe(publisherQueue, ExchangeTopicName, "topic", "robot.q.success", "automation.success", "AutomationQueue");
                RMQConnection.PublisherQueueDetails.Add(rabbitMQQueue);

                RabbitMQQueue publisherFailQueue = new RabbitMQQueue();
                //RabbitMQQueue rabbitMQFailQueue = RabbitMQ_QueuePublishSubscribe(publisherFailQueue, PublisherFailExchangeName, PublisherFailExchangeType, PublisherFailQueueName, PublisherFailRoutingKey, PublisherFailCriteriaFlag);

                /*Commented By Piyush*/
                RabbitMQQueue rabbitMQFailQueue = RabbitMQ_QueuePublishSubscribe(publisherFailQueue, ExchangeTopicName, "topic", "robot.q.failure", "automation.failure", "AutomationQueue");
                RMQConnection.PublisherQueueDetails.Add(rabbitMQFailQueue);

                Logger.Log.Logger.LogData("Connection to RabbitMQ Successfull", LogLevel.Info);
                Logger.Log.Logger.LogData("Connection to RabbitMQ RMQConnection.HostName " + RMQConnection.HostName, LogLevel.Info);
                Logger.Log.Logger.LogData("Connection to RabbitMQ RMQConnection.PortNumber "+ RMQConnection.PortNumber, LogLevel.Info);
                Logger.Log.Logger.LogData("Connection to RabbitMQ RMQConnection.UserName"+ RMQConnection.UserName, LogLevel.Info);
                Logger.Log.Logger.LogData("Connection to RabbitMQ RMQConnection.Password"+ RMQConnection.Password, LogLevel.Info);

                messageAdapter = new RabbitMQListenerRobot.RabbitMQListener(RMQConnection);
                ConnectionAttributes = messageAdapter.Connect();
                RobotExecutionStatus = RoboExecutionStatus.RobotLaunching;

                Logger.Log.Logger.LogData("RobotExecutionStatus Status: "+ RobotExecutionStatus.ToString(), LogLevel.Info);

                PublishNotifications(_heartbeat);
                Dictionary<string, string> dicChannel = new Dictionary<string, string>();
                //get below details from DB based on botname and default queue
                dicChannel.Add("QueueName", QueueName);
                dicChannel.Add("RoutingKey", RoutingKey);

                messageAdapter.MessageReceivedEvent -= MessageAdapter_MessageReceivedEvent;
                messageAdapter.MessageReceivedEvent += MessageAdapter_MessageReceivedEvent;

                messageAdapter.SubscribeToChannels(dicChannel);
                Logger.Log.Logger.LogData("After messageAdapter: " + _heartbeat.ProcessName, LogLevel.Info);
                tmr_Heartbeats.Enabled = true;
                RobotExecutionStatus = RoboExecutionStatus.RobotReadyState;

                Logger.Log.Logger.LogData("Publishing Hearbeat _heartbeat.ProcessName: " + _heartbeat.ProcessName, LogLevel.Info);
                Logger.Log.Logger.LogData("Publishing Hearbeat _heartbeat.MachineName: " + _heartbeat.MachineName, LogLevel.Info);
                PublishNotifications(_heartbeat);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in RMQueue Details Start:" +  ex.Message, LogLevel.Error);
            }
        }
        public RabbitMQQueue RabbitMQ_QueuePublishSubscribe(RabbitMQQueue publisherSubscriberQueue, string exchangeName, string exchangeType, string queueName, string routingKey, string QueueCriteriaFlag)
        {
            try
            { 
                // rq.PublisherQueueDetails.Add(publisherSubscriberQueue);
                publisherSubscriberQueue.BindExchangeToQueue = true;
                // publisherSubscriberQueue.Exchange = new ExchangeDetails();
                ExchangeDetails ed = new ExchangeDetails();
                ed.AutoDelete = false;
                ed.Durable = true;
                ed.IsInternal = false;
                ed.Name = exchangeName;
                ed.Type = exchangeType;
                ed.Arguments = new List<ExchangeArgs>();
                ed.Arguments.Add(new ExchangeArgs());
                publisherSubscriberQueue.Exchange = ed;
                publisherSubscriberQueue.QueueDetails = new QDetails();
                publisherSubscriberQueue.QueueDetails.AutoDelete = false;
                publisherSubscriberQueue.QueueDetails.Durable = true;
                publisherSubscriberQueue.QueueDetails.Name = queueName;// "robot.q.success";
                publisherSubscriberQueue.QueueDetails.ReqAcknowlegement = false;
                publisherSubscriberQueue.QueueCriteriaFlag = QueueCriteriaFlag;
                publisherSubscriberQueue.RoutingKey = routingKey;// "automation.success";
                publisherSubscriberQueue.QueueDetails.Arguments = new List<QArguments>();
                return publisherSubscriberQueue;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in RMQueue Details RabbitMQ_QueuePublishSubscribe:" + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public Dictionary<string, object> MessageHeaders { get; set; }
        public RobotDetails RobotConfigurationDetails { get; set; }
        RequestProperties requestMessageProperties;

        private void MessageAdapter_MessageReceivedEvent(object sender, RequestEventArgs e)
        {
            Logger.ServiceReference1.BOTServiceClient bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();

            Logger.Log.Logger.LogData("RMQueue->MessageAdapter_MessageReceivedEvent", LogLevel.Info);

            try
            { 
                if (stopflag == true)
                {
                    if (client != null)
                    {
                        try
                        {
                            client.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log.Logger.LogData("Error in RMQueue during disconnecting tcp client Details:" + ex.Message, LogLevel.Error);
                        }
                    }
                    try
                    {
                        messageAdapter.UnSubscribeFromChannels();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Logger.LogData("Error in RMQueue Details during disconnecting adapter:" + ex.Message, LogLevel.Error);
                    }
                    Environment.Exit(0);
                    return;
                }
                RobotExecutionStatus = RoboExecutionStatus.RobotProcessingAutomation; //so that no message will be missed
                //Log.Logger.LogData("Search: Started", LogLevel.Transaction);
            
                byte[] byteSrcMsgId;
                string source_Message_Id = string.Empty;
                MessageHeaders = e.MessageHeader;
                requestMessageProperties = e.RequestMessageProperties;
                delivarytag = e.DeliveryTag;
                if (!string.IsNullOrEmpty(requestMessageProperties.Expiration))
                {
                    int interval = 0;
                    Int32.TryParse(requestMessageProperties.Expiration, out interval);
                    //Set searchtimeout for robot search method... here
                }
                if (e.MessageHeader.ContainsKey("source_message_id"))
                {
                    byteSrcMsgId = (byte[])(object)e.MessageHeader["source_message_id"];
                    source_Message_Id = Encoding.UTF8.GetString(byteSrcMsgId);
                    //rqInput = (RequestInput)RobotLibrary.XMLHelper.Deserialize(e.Message, typeof(RequestInput));
                   
                    rqInput = JsonConvert.DeserializeObject<RequestInput>(e.Message);
                    bOTServiceClient.PiyushLogs("MessageAdapter_MessageReceivedEvent rqInput : "+rqInput);

                    _heartbeat.ProcessName =  rqInput.AutomationProcessName;
                    bOTServiceClient.PiyushLogs("MessageAdapter_MessageReceivedEvent _heartbeat.ProcessName : "+_heartbeat.ProcessName);

                    Log.ProcessName = rqInput.AutomationProcessName;
                    Log.RequestId = rqInput.RequestNumber;
                    Logger.Log.Logger.LogData("RMQueue->MessageAdapter_MessageReceivedEvent: " + rqInput.AutomationProcessName, LogLevel.Info);
                    Automate(rqInput);   //From this function we are getting RabbitMQ Load generator message.
                }
                else //No source message id
                {
                    requestMessageProperties.Expiration = RobotConfigurationDetails.MessageExpTimeout.ToString();
                    messageAdapter.PublishToChannel(PublishQueueTypes.AutomationFailure.ToString(), "source message id is null", requestMessageProperties, MessageHeaders);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in RMQueue Details:" + ex.Message, LogLevel.Error);
            }
        }       
        Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
        //private CustomTrackingParticipant _executionLog;
        private WorkflowApplication _wfApp;
        RequestInput rqInput = null;


        /// <summary>
        /// Vinay This is method where loadgenerartor 
        /// </summary>
        /// <param name="rqInput"></param>
        private void Automate(RequestInput rqInput)
        {
           //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate rqInput : " + rqInput); // Piyush
            Logger.Log.Logger.LogData("RMQueue->Automate", LogLevel.Info);
            RobotExecutionStatus = RoboExecutionStatus.RobotProcessingAutomation;
            //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate RobotExecutionStatus : " + RobotExecutionStatus); // Piyush
            PublishNotifications(_heartbeat);
            try
            {
                //string strFolderName = GetFolderName(rqInput.AutomationProcessName);

                Logger.Log.Logger.LogData("Process Name : " + rqInput.AutomationProcessName, LogLevel.Info);
                Logger.Log.Logger.LogData("Tenant Name : " + rqInput.TenantName, LogLevel.Info);
                Logger.Log.Logger.LogData("Group Name : " + rqInput.AutomationGroupName, LogLevel.Info);

                string strFolderName = GetFolderName(rqInput.AutomationProcessName,rqInput.AutomationGroupName,rqInput.TenantName);

             
                string strMainFilePath = GetPackageMainFilePath(rqInput,strFolderName);
                //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate strFolderName : " + strFolderName); // Piyush
                //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate strMainFilePath : " + strMainFilePath); // Piyush
                //bOTServiceClient.PiyushLogs(" CHRONEXPRESSION IN RMQUEUE Automate CHRONEXPRESSION : " + chronExpression); // Piyush

                // strMainFilePath = @"C:\Work\Work\E2EBots\Demo\Main.xaml";

                if (!string.IsNullOrEmpty(strMainFilePath))
                {
                    ThreadInvoker.Instance.RunByUiThread(() =>
                    {
                        #region Comment
                        //    Logger.Log.Logger.LogData("RMQueue->Automate-> FileName: " + strMainFilePath, LogLevel.Info);

                        //    if (!strMainFilePath.ToLower().Contains(".xaml"))
                        //    {
                        //        strMainFilePath = strMainFilePath + ".xaml";
                        //        Logger.Log.Logger.LogData("RMQueue->Automate-> FileName: " + strMainFilePath, LogLevel.Info);
                        //    }
                        //    if (WorkflowDictionary.ContainsKey(strMainFilePath))
                        //    {
                        //        CustomWfDesigner.Instance = WorkflowDictionary[strMainFilePath];
                        //    }
                        //    else
                        //    {
                        //        CustomWfDesigner.NewInstance(strMainFilePath);
                        //        WorkflowDictionary.Add(strMainFilePath, CustomWfDesigner.Instance);
                        //    }
                        //    CustomWfDesigner.Instance.Flush();
                        //    MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                        //    DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
                        //    _wfApp = new WorkflowApplication(activityExecute);
                        //    DynamicActivityProperty dynamicActivityProperty = new DynamicActivityProperty { Name = "QueueRequestInput", Type = typeof(InArgument<List<string>>), Value = rqInput.InputSearchParameters };

                        //                                                                                                               // AssignRequestInputParameters(rqInput, activityExecute);
                        //                                                                                                               //_wfApp.Extensions.Add(_executionLog);
                        //    _wfApp.Completed = WfExecutionCompleted;
                        ////execute 
                        //_wfApp.Run();
                        #endregion

                        if (WorkflowDictionary.ContainsKey(strMainFilePath))
                        {
                            CustomWfDesigner.Instance = WorkflowDictionary[strMainFilePath];
                        }
                        else
                        {
                            BotDesignCommon.Helpers.CustomWfDesigner.NewInstance(strMainFilePath);
                            WorkflowDictionary.Add(strMainFilePath, CustomWfDesigner.Instance);
                        }
                        SelectHelper._currentworkflowfile = strMainFilePath;
                        SelectHelper.ProjectLocation = System.IO.Path.GetDirectoryName(strMainFilePath);

                        CustomWfDesigner.Instance.Flush();
                        MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                        DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;

                        //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate activityExecute : " + activityExecute); // Piyush
                        //File.AppendAllText(@"c:\Piyush\file.txt", "Executing Activity" + "\n");

                        _wfApp = new WorkflowApplication(activityExecute);
                        if (SelectHelper.CurrentRuntimeApplicationHelper == null)
                        {
                            SelectHelper.CurrentRuntimeApplicationHelper = new RuntimeApplicationHelper();
                        }

                        //SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects = new Dictionary<string, object>(); //to clean stored runtime objects from previous run
                        DynamicActivityProperty dynamicActivityProperty = new DynamicActivityProperty { Name = "QueueRequestInput", Type = typeof(InArgument<List<string>>), Value = rqInput.InputSearchParameters };

                        //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate dynamicActivityProperty : " + dynamicActivityProperty); // Piyush

                        _wfApp.Completed = WfExecutionCompleted;


                        //bOTServiceClient.PiyushLogs(" IN RMQUEUE Automate _wfApp.Completed : " + _wfApp.Completed); // Piyush

                        if (Logger.Log.Logger.DatatableLog != null)
                        {
                            Logger.Log.Logger.DatatableLog.Clear();
                        }

                        //bOTServiceClient.PiyushLogs("IN RMQUEUE Automate Running Again **********");

                        _wfApp.Run();
                    });
                }
              
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"c:\Piyush\file.txt", "Error in RMQueue Details Automate:" + ex.Message + "\n");
                Logger.Log.Logger.LogData("Error in RMQueue Details Automate:" + ex.Message, LogLevel.Error);
            }
        }
        private string GetFolderName(string strProcessname,string groupname,string tenantname)
        {
            string strFolderName = strProcessname;
            string strVersion = string.Empty;

            try
            {
                DataTable result = null;
                try
                {
                    if (bOTServiceClient == null)
                    {
                        //bOTServiceClient = new ServiceReference1.BotServiceClient();
                         bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();
                    }
                    result = bOTServiceClient.GetProcessDetails(strProcessname, groupname, tenantname);

                    //if((result != null)&&(result.Rows.Count>0))
                    if ((result != null) && (result.Rows.Count != 0))
                    {
                        strVersion = result.Rows[0][4].ToString(); // to get process version from table.
                        strFolderName = strFolderName + "." + strVersion;
                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Error during getting version of an automation from DB:" + strProcessname, LogLevel.Error);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Logger.LogData("Error in RMQueue Details GetFolderName:" + ex.Message, LogLevel.Error);
                    return null;
                }

                return strFolderName ;
            }
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: Getting Folder Name", "Error in Getting Folder Name : " + ex.Message, groupId, tenantId);
                Logger.Log.Logger.LogData("Error in getting FolderName:" + ex.Message, LogLevel.Error);
                return string.Empty;
            }
        }

        private string GetPackageMainFilePath(RequestInput rqInput, string Foldername)
        {
            string strMainFilePath = string.Empty;
            try
            {
                string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                string strPublishFolderPath = filePath + Path.DirectorySeparatorChar + "E2EBot" + Path.DirectorySeparatorChar + "Publish";
                filePath = strPublishFolderPath + Path.DirectorySeparatorChar + Foldername;
                string strZIPFile = filePath + ".zip";
                if (Directory.Exists(filePath))
                {
                    strMainFilePath = filePath + Path.DirectorySeparatorChar + "Main.xaml";
                    if (File.Exists(strMainFilePath))
                    {
                        return strMainFilePath;
                    }
                    else
                    {
                         Logger.Log.Logger.LogData("Error in getting package: Main.xaml do not exist. Path: " + strMainFilePath, LogLevel.Error);
                         return string.Empty;
                    }
                }
                else if (File.Exists(strZIPFile))
                {
                    strMainFilePath = ZipToMainFilePath( strZIPFile,  filePath);
                }
                else
                {
                   if( DownloadPackage(rqInput, Foldername, strZIPFile))
                    {
                        strMainFilePath = ZipToMainFilePath(strZIPFile, filePath);
                        return strMainFilePath;
                    }
                   else
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: Getting Package", "Error in Getting Package : " + ex.Message, groupId, tenantId);
                Logger.Log.Logger.LogData("Error in getting package:" + ex.Message, LogLevel.Error);
                return string.Empty;
            }

        }

        private string ZipToMainFilePath(string strZIPFile, string filePath)
        {
            string strMainFilePath = string.Empty;

            ZipFile.ExtractToDirectory(strZIPFile, filePath);
            if (Directory.Exists(filePath))
            {
                strMainFilePath = filePath + Path.DirectorySeparatorChar + "Main.xaml";
                if (File.Exists(strMainFilePath))
                {
                    return strMainFilePath;
                }
                else
                {
                    Logger.Log.Logger.LogData("Error in getting package: Main.xaml do not exist. Path: " + strMainFilePath, LogLevel.Error);
                    return string.Empty;
                }
            }
            return string.Empty;
        }
        private bool DownloadPackage(RequestInput rqInput, string Foldername, string strZIPFile)
        {
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();
                }
                //DataTable dt = bOTServiceClient.DownloadAutomationZipBinary(rqInput.AutomationProcessName,rqInput.AutomationProcessVersion,rqInput.AutomationGroupName,rqInput.TenantName);
                DataTable dt = bOTServiceClient.DownloadAutomationZipBinary(rqInput.AutomationProcessName, rqInput.AutomationProcessVersion, rqInput.AutomationGroupName, rqInput.TenantName); //Piyush
                byte[] bytes =(byte[] ) dt.Rows[0][4];

                if (ByteToZipFile(bytes, strZIPFile))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //Logger.ServiceReference1.BOTServiceClient WCFServiceReference = new Logger.ServiceReference1.BOTServiceClient();
                WCFServiceReference.insertLog("Error: Downloading Package","Error in Downloading Package : "+ex.Message,groupId,tenantId);
                Logger.Log.Logger.LogData("Error in downloading package:" + ex.Message, LogLevel.Error);
                return false;
            }
        }

        int BufferSize = 65536;
        private bool ByteToZipFile(byte[] bytes, string strZipFileFullName)
        {
            try
            {
                File.WriteAllBytes(strZipFileFullName, bytes);
                //using (var mstrim = new MemoryStream(bytes))
                //{
                //    using (var inStream = new GZipStream(mstrim, CompressionMode.Decompress))
                //    {
                //        using (var outStream = File.Create(strZipFileFullName))
                //        {
                //            var buffer = new byte[BufferSize];
                //            int readBytes;
                //            while ((readBytes = inStream.Read(buffer, 0, BufferSize)) != 0)
                //            {
                //                outStream.Write(buffer, 0, readBytes);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in converting ByteToZipFile:" + ex.Message, LogLevel.Error);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Retrieve Workflow Execution Logs and Workflow Execution Outputs
        /// </summary>
        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
        {
            try
            {
                Logger.ServiceReference1.BOTServiceClient bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();

                int deleteQueueEntry = 0;
                deleteQueueEntry = bOTServiceClient.deleteprocessQMapping(_heartbeat.ProcessName, groupId, tenantId);
                if (0 > deleteQueueEntry)
                {
                    Logger.Log.Logger.LogData("Entry from Queue Table Not Deleted.", LogLevel.Error);
                }

                RobotExecutionStatus = RoboExecutionStatus.RobotCompetedAutomation;
                if (ev.CompletionState == ActivityInstanceState.Closed)
                {
                    SendAutomationAcknowledgement();
                    Logger.Log.Logger.LogData("Process Completed", LogLevel.Info);

                    //Environment.Exit(0);   // Exiting one process.

                    if (stopflag == true)
                    {
                        if (client != null)
                        {
                            try
                            {
                                client.Disconnect();
                            }
                            catch (Exception ex)
                            {
                                WCFServiceReference.insertLog("Error: RMQueue during disconnecting tcp client Details", "Error in RMQueue during disconnecting tcp client Details : " + ex.Message, groupId, tenantId);
                                Logger.Log.Logger.LogData(
                                    "Error in RMQueue during disconnecting tcp client Details:" + ex.Message,
                                    LogLevel.Error);
                            }
                        }

                        try
                        {
                            messageAdapter.UnSubscribeFromChannels();
                        }
                        catch (Exception ex)
                        {
                            WCFServiceReference.insertLog("Error: RMQueue Details during disconnecting adapter", "Error in RMQueue Details during disconnecting adapter : " + ex.Message, groupId, tenantId);
                            Logger.Log.Logger.LogData("Error in RMQueue Details during disconnecting adapter:" + ex.Message, LogLevel.Error);
                        }
                        //Environment.Exit(0);
                        return;
                    }

                    if (string.IsNullOrEmpty(chronExpression))
                    {
                        chronExpression = "Manually Started and Completed Successfully.";
                    }

                    Logger.Log.Logger.LogData("Process Completed Successfully",LogLevel.Info);
                    //bOTServiceClient.PiyushLogs("COMPLETE : PROCESS COMPLETED : "+chronExpression);
                    //int result2 = bOTServiceClient.CreateScheduleStatus(QueueName, RobotName, chronExpression, "Completed",processGroupId,processTenantId, "", DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                    bOTServiceClient.CreateScheduleStatus(QueueName, RobotName, chronExpression, "Completed", processGroupId, processTenantId, "", DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));

                    RobotExecutionStatus = RoboExecutionStatus.RobotReadyState;

                    //TimeSpan interval = new TimeSpan(0, 0, 2);
                    //System.Threading.Thread.Sleep(interval);

                    //Logger.Log.Logger.LogData("Process Started", LogLevel.Info);


                    //bOTServiceClient.CreateScheduleStatus(QueueName, RobotName, "Manually Started", "Started", processGroupId, processTenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), "");
                    //Okay
                   Environment.Exit(0); 
                }
            }
            catch (Exception ex)
            {
                //bOTServiceClient.PiyushLogs("COMPLETE : PROCESS FAILED FOR CHRON : " + chronExpression);
                int Error = bOTServiceClient.CreateScheduleStatus(QueueName, RobotName, chronExpression, "ERROR in Rmqueue",processGroupId,processTenantId, "", DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                int RmqueueError = bOTServiceClient.PiyushLogs("Error In RMQUEUE.cs : "+ex.Message);
                WCFServiceReference.insertLog("Error: RMQueue Details while Execution", "Error in RMQueue Details  while Execution: " + ex.Message, groupId, tenantId);
                Logger.Log.Logger.LogData("Error in RMQueue Details:" + ex.Message, LogLevel.Error);
            }
        }
        public void PublishNotifications(HeartBeat heartbeat)
        {
            try
            {
                //File.AppendAllText(@"c:\Piyush\file.txt", "Publish Notifications Started" + "\n");
                heartbeat.roboExecutionStatus = RobotExecutionStatus;
                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HeartBeat));
                ser.WriteObject(stream1, heartbeat);
                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);
                //messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());

                if (Connection == null || Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
                {
                    connectAsync();
                }
                else {
                    //string json = JsonConvert.SerializeObject(sr);
                    string notificationMessage = sr.ReadToEnd();
                    //sendMessageToClient(sr.ReadToEnd());
                    sendMessageToClient(notificationMessage,tenantId);
                }

                //SignalR Method
                //Logger.ServiceReference1.BOTServiceClient serviceRef = new Logger.ServiceReference1.BOTServiceClient();
                //serviceRef.sendMessageToClient();

                messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());
                TCP.Shared.Messages.TextMessageRequest tm = new TCP.Shared.Messages.TextMessageRequest();
                tm.Message = "heartbeat";
                client.SendMessage(tm);
            }
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: RMQueue Details while Publishing Notifications", "Error in RMQueue Details while while Publishing Notifications: " + ex.Message, groupId, tenantId);
                Logger.Log.Logger.LogData("Error in RMQueue Details:" + ex.Message, LogLevel.Error);
            }

        }

        #region SignalR Code writeen on 05-Dec-2019
        //Method connectAsync Related to SignalR
        private Microsoft.AspNet.SignalR.Client.IHubProxy HubProxy { get; set; }
        // const string ServerURI = "http://localhost:8081/E2EBots/signalr";
        //const string ServerURI = "https://localhost:44390";
        // const string ServerURI = "http://localhost:3455/signalr";  //Notifications will work for BotDashBoard Page only on 8081 Port.
        const string ServerURI = "http://localhost:8090/website_deploy/signalr";
        //string ServerUrlPath = ConfigurationManager.AppSettings["SignalRConnection"];
       
        private Microsoft.AspNet.SignalR.Client.HubConnection Connection { get; set; }

        //Call from Constructor
        private async void connectAsync()
        {
            Connection = new Microsoft.AspNet.SignalR.Client.HubConnection(ServerURI);
            // Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread

            Connection.TraceLevel = Microsoft.AspNet.SignalR.Client.TraceLevels.All;
            Connection.TraceWriter = Console.Out;

            Console.WriteLine("\n SIGNAL R RMQ TESTING : ");
            try
            {
                await Connection.Start();
                Console.WriteLine("\n COMPLETED AWAIT SIGNAL R RMQ TESTING : ");
            }
            //catch (HttpRequestException)
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: RMQueue Details SignalR", "Error in RMQueue Details SignalR: " + ex.Message, groupId, tenantId);
            }
        }

        public void sendMessageToClient(string sr, int tenantId)  //Receive from Windows Service
        {
            try
            {
                //client.Value.BroadcastToClient(eventData);
                if (Connection != null && Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    // HubProxy.Invoke("Send", eventData.ClientName, eventData.EventMessage);

                    HubProxy.Invoke("Send",sr,tenantId).Wait();
                    //HubProxy.Invoke("Send", sr, tenantId);
                }
                else
                {
                    Console.WriteLine("Connection Failed");
                }
            }
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: RMQueue Details Sending Message to Client", "Error in RMQueue Details Sending Message to Client: " + ex.Message, groupId, tenantId);
                Console.WriteLine("Connection Exception : " + ex.Message);
            }
        }
        #endregion

        public void SendAutomationAcknowledgement()
        {
            try
            { 
                // string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(heartbeat);
                messageAdapter.SendAcknowledge(delivarytag); //will not get other message in queue unless acknowledged ...
                delivarytag = null;
                RobotExecutionStatus = RoboExecutionStatus.RobotCompetedAutomation;
                _heartbeat.TotalRequestServed = _heartbeat.TotalRequestServed + 1;
                PublishNotifications(_heartbeat);
            }
            catch (Exception ex)
            {
                WCFServiceReference.insertLog("Error: RMQueue Details SendAutomationAcknowledgement", "Error in RMQueue Details SendAutomationAcknowledgement: " + ex.Message, groupId, tenantId);
                Logger.Log.Logger.LogData("Error in RMQueue Details SendAutomationAcknowledgement:" + ex.Message, LogLevel.Error);
            }
        }
        public void TriggerSuccess()
        {
            try
            { 
                ResponseOutput responseOutput = new ResponseOutput();
                //   responseOutput.cviewResultsAll = configLoader.processMapper.CViewProcessResults.CViewResultsAll.ToList<CViewFields>();
                responseOutput.requestInput = rqInput;
                    //  var message = CommonLibrary.XMLHelper.Serialize(responseOutput);
                    //  messageAdapter.PublishToChannel(PublishQueueTypes.AutomationSuccess.ToString(), message, null, null);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in RMQueue Details TriggerSuccess:" + ex.Message, LogLevel.Error);
            }
        }
            
        void tmr_Heartbeats_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                PublishNotifications(_heartbeat);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Robot: Error in sending heart beats", LogLevel.Error);
            }
        }

        Logger.ServiceReference1.BOTServiceClient bOTServiceClient = null;

        public DataTable GetBotStartDetails()
        //public DataTable GetBotStartDetails(string UserName,int groupId, int tenantId)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();
                }
                //File.AppendAllText(@"c:\Piyush\file.txt", "Getting BOTSTart Details for UserName:"+ Environment.UserName + " And Machine Name: "+ Environment.MachineName + "\n");
                result = bOTServiceClient.GetBotStartDetailsFromDesktop(Environment.UserName, Environment.MachineName);  //Piyush
                //result = bOTServiceClient.GetBotStartDetailsFromDesktop(UserName,groupId,tenantId, Environment.MachineName);

                if (null == result)
                { //File.AppendAllText(@"c:\Piyush\file.txt", "BotStart  Details Not Found" + "\n"); }
                }
            }
            catch (Exception ex)
            {
             //   System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "RMQueue GetBotStartDetails \n");
                Logger.Log.Logger.LogData("Error in RMQueue Details GetBotStartDetails:" + ex.Message, LogLevel.Error);
                return null;
            }
            return result;
        }
        bool TCPClientLoginEntryToServer()
        {
            try
            {

                if (!string.IsNullOrEmpty(RobotName))
                {
                    System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "RMQueue.cs => TCPClientLoginEntryToServer RobotName:"+ RobotName + "  \n");
                    client.Login(RobotName, (senderClient, args) =>
                    {

                    if (args.IsValid)
                    {
                        //File.AppendAllText(@"c:\Piyush\file.txt", "TCPClientLoginEntryToServer for Robot : "+ RobotName + "\n");
                        // "User Validated!";
                        if (args.IsValid)
                        {
                            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "RMQueue.cs => TCPClientLoginEntryToServer RobotName Validated" + RobotName + "  \n");
                            // "User Validated!";

                    }
                        }

                    if (args.HasError)
                    {
                        //File.AppendAllText(@"c:\Piyush\file.txt", "NOPE TCPClientLoginEntryToServer for Robot : " + RobotName + "\n");
                        //Error
                    }
                        if (args.HasError)
                        {
                            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Error => RMQueue.cs => TCPClientLoginEntryToServer RobotName NOT Validated" + RobotName + "  \n");
                            //Error
                        }

                    });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
              System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "TCPClientLoginEntryToServer Error "+ex.Message+"\n");
               
            }
            return false;
        }
              
    }
}
