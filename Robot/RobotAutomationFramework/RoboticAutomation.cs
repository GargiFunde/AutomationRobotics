// <copyright file=RoboticAutomation company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RobotLibrary.Interfaces;
//using RabbitMQListenerRobot;
////using CommonLibrary;
////using CommonLibrary.Entities;
//using RobotLibrary;
//using System.Threading;
//using System.Configuration;
//using System.IO;
//using System.Runtime.Serialization.Json;
//using System.Diagnostics;
//using CommonLibrary.Helpers;
//using System.Activities;
//using System.Activities.XamlIntegration;
//using System.Activities.Presentation;
//using CommonLibrary;
//using System.Activities.Presentation.Services;
//using System.Activities.Presentation.Model;
////using Entities;
//namespace RobotAutomationFramework
//{
//    class RoboticAutomation
//    {
//        ConfiguratorLoader configLoader = null;
//        string RoutingKeyNotifiation = string.Empty;
//        string strRobotName = string.Empty;
//        RoboExecutionStatus RobotExecutionStatus;
//        IMessageAdapter messageAdapter = null;
//        public Dictionary<string, string> ConnectionAttributes { get; set; }
//        public string ServerName { get; set; }
//        public string CurrentUser { get; set; }
//        public string PortNumber { get; set; }
//        public string Password { get; set; }
//        public Dictionary<string, object> MessageHeaders { get; set; }
//        public RobotDetails RobotConfigurationDetails { get; set; }
//        RequestProperties requestMessageProperties;
//        private object delivarytag;
//        HeartBeat _heartbeat = null;
       
//        public bool Initialized { get; set; }
//        public RoboticAutomation(ConfiguratorLoader configuratorLoader, RoboExecutionStatus robotExecutionStatus, HeartBeat heartBeat)
//        {
//            configLoader = configuratorLoader;
//            RobotExecutionStatus = robotExecutionStatus;
//            _heartbeat = heartBeat;
//            Initialized = false;
//            RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"];
//            strRobotName = ConfigurationManager.AppSettings["RobotName"];

//            // Automate(null);
//            RabbitMQConnection RMQConnection = new RabbitMQConnection();
//            messageAdapter = new RabbitMQListenerRobot.RabbitMQListener(RMQConnection);
//            ConnectionAttributes = messageAdapter.Connect();
//            if (ConnectionAttributes.ContainsKey("ServerName"))
//            {
//                ServerName = ConnectionAttributes["ServerName"];
//            }
//            if (ConnectionAttributes.ContainsKey("PortNumber"))
//            {
//                PortNumber = ConnectionAttributes["PortNumber"];
//            }
//            messageAdapter.SubscribeToNotifications();
//            messageAdapter.NotificationReceivedEvent += messageAdapter_NotificationsReceivedEvent;
//        }
//        public void RobotInitialization()
//        {
          
//            //RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"];
//            //strRobotName = ConfigurationManager.AppSettings["RobotName"];

//            //messageAdapter = new RabbitMQListenerRobot.RabbitMQListener();
//            //ConnectionAttributes = messageAdapter.Connect();
//            //if (ConnectionAttributes.ContainsKey("ServerName"))
//            //{
//            //    ServerName = ConnectionAttributes["ServerName"];
//            //}
//            //if (ConnectionAttributes.ContainsKey("PortNumber"))
//            //{
//            //    PortNumber = ConnectionAttributes["PortNumber"];
//            //}

//            Dictionary<string, string> dicChannel = new Dictionary<string, string>();
//            dicChannel.Add("QueueName", "robot.q.automation");
//            dicChannel.Add("RoutingKey", "Self.GroupDemo");
//            messageAdapter.MessageReceivedEvent -= MessageAdapter_MessageReceivedEvent;
//            messageAdapter.MessageReceivedEvent += MessageAdapter_MessageReceivedEvent;
//            messageAdapter.SubscribeToChannels(dicChannel);
//            RobotExecutionStatus = RoboExecutionStatus.RobotReadyState;
//            Initialized = true; 
//        }
       
//        public void RobotUnSubscription()
//        {
//            messageAdapter.MessageReceivedEvent -= MessageAdapter_MessageReceivedEvent;
//            bool result = messageAdapter.UnSubscribeFromChannels();
//        }
//        Stopwatch stopWatch = new Stopwatch();
//        RequestInput rqInput = null;
//        private void MessageAdapter_MessageReceivedEvent(object sender, RequestEventArgs e)
//        {
//            RobotExecutionStatus = RoboExecutionStatus.RobotProcessingAutomation; //so that no message will be missed
//            //Log.Logger.LogData("Search: Started", LogLevel.Transaction);
//            byte[] byteSrcMsgId;
//            string source_Message_Id = string.Empty;
//            MessageHeaders = e.MessageHeader;
//            requestMessageProperties = e.RequestMessageProperties;
//            delivarytag = e.DeliveryTag;
//            if (!string.IsNullOrEmpty(requestMessageProperties.Expiration))
//            {
//                int interval = 0;
//                Int32.TryParse(requestMessageProperties.Expiration, out interval);
//                //Set searchtimeout for robot search method... here
//            }
//            if (e.MessageHeader.ContainsKey("source_message_id"))
//            {
//                byteSrcMsgId = (byte[])(object)e.MessageHeader["source_message_id"];
//                source_Message_Id = Encoding.UTF8.GetString(byteSrcMsgId);
//                rqInput = (RequestInput)RobotLibrary.XMLHelper.Deserialize(e.Message, typeof(RequestInput));

//                _heartbeat.ProfileName = rqInput.AutomationGroupName;
//                _heartbeat.ProcessName = rqInput.AutomationProcessName;
//                stopWatch.Reset();
//                stopWatch.Start();
//                Automate(rqInput);
//                stopWatch.Stop();
//                _heartbeat.LastRequestTimeInSeconds= stopWatch.Elapsed.ToString();
//            }
//            else //No source message id
//            {
//                requestMessageProperties.Expiration = RobotConfigurationDetails.MessageExpTimeout.ToString();
//                messageAdapter.PublishToChannel(PublishQueueTypes.AutomationFailure.ToString(), "source message id is null", requestMessageProperties, MessageHeaders);
//            }
//        }

//        void messageAdapter_NotificationsReceivedEvent(object sender, NotificationEventArgs e)
//        {
//            string msg = e.Message;
//            string robotName = string.Empty;
//            //string profileName = string.Empty;
//            //string processName = string.Empty;
//            // throw new NotImplementedException();
//            if (!string.IsNullOrEmpty(msg) && msg.Length > 0)
//            {
//                string[] list = msg.Split(new string[] { "~=~" }, StringSplitOptions.None);
//                robotName = list[0];
//                //profileName = list[1];
//                //processName = list[2];
//            }
//            //string strCurrentUser = Environment.UserName;
//            //foreach (var item in robotDetailsList.RobotsCollection)
//            //{
//            //    if (robotName == item.RobotName)
//            //    {
//            //        agentOperations.StartRobot(item);
//            //        if (item.MainWindowHandle != IntPtr.Zero)
//            //        {
//            //            agentOperations.SignInRobot(item, e.Message);
//            //        }
//            //    }
//            //    //if(item.RobotFullName )
//            //}
//        }
//        Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
//        private CustomTrackingParticipant _executionLog;
//        private WorkflowApplication _wfApp;
//        private void Automate(RequestInput rqInput)
//        {
//            //int iResetFireAfterSearch = 0;
//            //RobotExecutionStatus = RoboExecutionStatus.RobotProcessingAutomation;
//            //PublishNotifications(_heartbeat);
//            //Dictionary<string, string> dicSearchValues = new Dictionary<string, string>();
//            //dicSearchValues.Add(rqInput.InputSearchParameters[0], rqInput.InputSearchParameters[1]);
//            //Dictionary<string, string> dicResetValues = new Dictionary<string, string>();
//            ////Need to execute workflow here
           
//            try
//            {
//                ThreadInvoker.Instance.RunByUiThread(() =>
//               {
//                   string filename = @"C:\Work\BOTDesignerMaster\SharePrice.xaml";

//                   if (WorkflowDictionary.ContainsKey(filename))
//                   {
//                       CustomWfDesigner.Instance = WorkflowDictionary[filename];
//                   }
//                   else
//                   {
//                       CustomWfDesigner.NewInstance(filename);
//                       WorkflowDictionary.Add(filename, CustomWfDesigner.Instance);
//                   }
                   
                  
//                   CustomWfDesigner.Instance.Flush();
//                   MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
//                   DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
                   
//                    //configure workflow appllication
//                    //consoleExecutionLog.Text = String.Empty;
//                    ////consoleOutput.Text = String.Empty;
//                    //_executionLog = new CustomTrackingParticipant();
//                    _wfApp = new WorkflowApplication(activityExecute);

//                  //  DynamicActivityProperty dynamicActivityProperty = new DynamicActivityProperty { Name = "QueueRequestInput", Type = typeof(InArgument<List<string>>), Value = rqInput.InputSearchParameters };
                   
//                   // AssignRequestInputParameters(rqInput, activityExecute);
//                   //_wfApp.Extensions.Add(_executionLog);
//                   _wfApp.Completed = WfExecutionCompleted;
//                //execute 
//                _wfApp.Run();
//               });
//            }catch(Exception ex)
//            {
//                //
//            }
//        }

//        //private void AssignRequestInputParameters(RequestInput rqInput, Activity dynamicroot)
//        //{
//        //    IEnumerator<Activity> activities =
//        //   WorkflowInspectionServices.GetActivities(dynamicroot).GetEnumerator();
            
//        //    //var modelService = CustomWfDesigner.Instance.Context.Services.GetService<ModelService>();
//        //    //IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(Activity));

//        //    while (activities.MoveNext())
//        //    {

//        //        if (activities.Current.GetType().FullName == "Core.ActivityLibrary.Activities.QueueInputs")
//        //        {
//        //            Core.ActivityLibrary.Activities.QueueInputs queueInputs = (Core.ActivityLibrary.Activities.QueueInputs)activities.Current;
//        //            queueInputs.QueueRequestInputs = rqInput;
//        //            break;
//        //        }
//        //    }
//        //}

//        public string ExecutionLog
//        {
//            get
//            {
//                if (_executionLog != null)
//                    return _executionLog.TrackData;
//                else
//                    return string.Empty;
//            }
//            set { _executionLog.TrackData = value; }
//        }
//        /// <summary>
//        /// Retrieve Workflow Execution Logs and Workflow Execution Outputs
//        /// </summary>
//        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
//        {
//            try
//            {
//                //retrieve & display execution log
//                // _timer.Stop();
//                // UpdateTrackingData();

//                //retrieve & display execution output
//                //foreach (var item in ev.Outputs)
//                //{
//                //   // consoleOutput.Dispatcher.Invoke(
//                //        System.Windows.Threading.DispatcherPriority.Normal,
//                //        new Action(
//                //            delegate ()
//                //            {
//                //                consoleOutput.Text += String.Format("[{0}] {1}" + Environment.NewLine, item.Key, item.Value);
//                //            }
//                //    ));
//                //}
//                //MessageBox.Show("Execution completed!");
//                SendAutomationAcknowledgement();
//            }
//            catch (Exception ex)
//            {
//               // MessageBox.Show(ex.ToString());
//            }
//        }
//        public void PublishNotifications(HeartBeat heartbeat)
//        {
//            heartbeat.roboExecutionStatus = RobotExecutionStatus;
//            MemoryStream stream1 = new MemoryStream();
//            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HeartBeat));
//            ser.WriteObject(stream1, heartbeat);
//            stream1.Position = 0;
//            StreamReader sr = new StreamReader(stream1);
//            messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());
//        }
//        public void SendAutomationAcknowledgement()
//        {
//           // string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(heartbeat);
//            messageAdapter.SendAcknowledge(delivarytag); //will not get other message in queue unless acknowledged ...
//            delivarytag = null;
//            RobotExecutionStatus = RoboExecutionStatus.RobotCompetedAutomation;
//            _heartbeat.TotalRequestServed = _heartbeat.TotalRequestServed + 1;
//            PublishNotifications(_heartbeat);
//        }
//        public void TriggerSuccess()
//        {
//            ResponseOutput responseOutput = new ResponseOutput();
//         //   responseOutput.cviewResultsAll = configLoader.processMapper.CViewProcessResults.CViewResultsAll.ToList<CViewFields>();
//            responseOutput.requestInput = rqInput;
//          //  var message = CommonLibrary.XMLHelper.Serialize(responseOutput);
//          //  messageAdapter.PublishToChannel(PublishQueueTypes.AutomationSuccess.ToString(), message, null, null);
//        }
//    }

//}
