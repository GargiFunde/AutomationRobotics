// <copyright file=BaseRobotFramework company=E2E Robotics>
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
//using RobotLibrary;
//using RobotLibrary.Interfaces;
////using Entities;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
////using ProcessViewModel.Common;
////using CommonLibrary.Interfaces;
////using CommonLibrary;
//using System.Diagnostics;
//using System.Threading;
//using System.Configuration;
//using System.IO;
//using System.Runtime.Serialization.Json;
////using CommonLibrary.Entities;
//using System.Collections.ObjectModel;
//using Newtonsoft;
//using Newtonsoft.Json;
//using CommonLibrary;

//namespace RobotAutomationFramework
//{
   
//    public class BaseRobotFramework //: IRobotFramework, IRobotExecutorFramework
//    {       
//        #region "Load catelog dlls"
       
//        //[ImportMany(typeof(IAutomationComponent), AllowRecomposition = true)]
//        //IEnumerable<IAutomationComponent> _automationComponents;
//        //DirectoryCatalog dirCatalog;
//        public static bool IsHeadedAutomation { get; set; }

//        public string CurrentGroupName { get; set; }
//        //private void Compose()
//        //{
//        //    string AutomationComponents = System.Configuration.ConfigurationManager.AppSettings["AutomationComponents"];
//        //    dirCatalog = new DirectoryCatalog(AutomationComponents);
//        //    AssemblyCatalog assemblyCat = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
//        //    AggregateCatalog catalog = new AggregateCatalog(assemblyCat, dirCatalog);
//        //    CompositionContainer container = new CompositionContainer(catalog);
//        //    container.ComposeParts(this);
//        //}

//        # endregion "Load catelog dlls"

//        HeartBeat heartbeat = null;
//        RoboExecutionStatus robotExecutionStatus;
//        ConfiguratorLoader configuratorLoader = null;
//        RoboticAutomation robotConfigurations = null;
//        System.Timers.Timer tmr_Heartbeats = null;
//      //  public List<GroupDetails> GroupList { get; set; }
//      //  public ObservableCollection<CViewFields> RespKeyFieldsOutput { get; set; }

//        public IntPtr AgentHandle { get; set; }

//        string RoutingKeyNotifiation = string.Empty;
//        string strRobotName = string.Empty;
        

//      //  ProcessMapper processMapper = null;

     
//        public BaseRobotFramework()
//        {
//            try
//            {
//                ThreadInvoker.Instance.InitDispatcher();
//                //Compose();
//                configuratorLoader = new ConfiguratorLoader(); //will fetch all group and application related data
//                //configuratorLoader.IsHeadedAutomation = IsHeadedAutomation;
//                //configuratorLoader.RobotReadyState += c_RobotReadyState;
//                //configuratorLoader.AutomationSearchCompletedState += c_AutomationSearchCompletedState;
//                //configuratorLoader.TriggerSuccessResult += c_TriggerSuccessResult;
//                //GroupList = configuratorLoader.GroupList; //to set grouplist in sign in
//                //processMapper = configuratorLoader.processMapper;
//                if (!IsHeadedAutomation)
//                {
//                    long HeartBeatinterval = Convert.ToInt64(ConfigurationManager.AppSettings["HeartBeatinterval"]);
//                    tmr_Heartbeats = new System.Timers.Timer(HeartBeatinterval);
//                    tmr_Heartbeats.Elapsed += tmr_Heartbeats_Elapsed;
//                    robotExecutionStatus = RoboExecutionStatus.RobotLaunching;
//                    // Log.Logger.LogData("Transaction request log", LogLevel.Transaction);
//                    strRobotName = ConfigurationManager.AppSettings["RobotName"];
//                    //Log.Logger.LogData("Robot Launch: Started", LogLevel.Transaction);
//                    heartbeat = new HeartBeat();
//                    heartbeat.RoboColor = "";
//                    heartbeat.UserName = Environment.UserName;
//                    heartbeat.MachineName = Environment.MachineName;
//                    heartbeat.RobotName = strRobotName;
//                    heartbeat.roboExecutionStatus = RoboExecutionStatus.RobotLaunching;

//                    robotConfigurations = new RoboticAutomation(configuratorLoader, robotExecutionStatus, heartbeat);

//                    MemoryStream stream1 = new MemoryStream();
//                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HeartBeat));
//                    ser.WriteObject(stream1, heartbeat);
//                    stream1.Position = 0;
//                    StreamReader sr = new StreamReader(stream1);
//                    RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"];
//                   // strRobotName = ConfigurationManager.AppSettings["RobotName"];
                   
//                    IMessageAdapter messageAdapter = new RabbitMQListenerRobot.RabbitMQListener(null);
//                    messageAdapter.Connect();
//                    messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());
//                    tmr_Heartbeats.Enabled = true;
//                    robotConfigurations.RobotInitialization();
//                }

//            }catch(Exception ex)
//            {
//                RaiseErrorToTrayEventArgs raiseErrorToTrayEventArgs = new RaiseErrorToTrayEventArgs();
//                raiseErrorToTrayEventArgs.ErrorNumber = Convert.ToString(ex.GetHashCode());
//                raiseErrorToTrayEventArgs.ErrorDescription = ex.Message;
//                ErrorMapper.GetErrorMappingText(ex.GetHashCode());
//                RobotLibrary.SelectHelper.OnRaiseErrorToTray(raiseErrorToTrayEventArgs);
//                // Log.Logger.LogData(ex.Message, LogLevel.Error);
//            }
//        }
//        //public void HeadedAutomate(RequestInput rqInput,int iResetFireAfterSearch)
//        //{
           
//        //    Dictionary<string, string> dicSearchValues = new Dictionary<string, string>();
//        //    dicSearchValues.Add(rqInput.InputSearchParameters[0], rqInput.InputSearchParameters[1]);
//        //    Dictionary<string, string> dicResetValues = new Dictionary<string, string>();
//        //    configuratorLoader.ExecuteAutomation(rqInput.AutomationGroupName, rqInput.AutomationProcessName, rqInput.RequestTimeoutSLAInSeconds, dicSearchValues, dicResetValues, iResetFireAfterSearch);
//        //   // RespKeyFieldsOutput = CViewResults.CViewResultsAll;
//        //}
      
//        void c_RobotReadyState(object sender, EventArgs e)
//        {
//            if (!IsHeadedAutomation)
//            {
//                robotExecutionStatus = RoboExecutionStatus.RobotReadyState;
//               // Log.Logger.LogData("Robot: Launch Ready", LogLevel.Transaction);
//                if (robotConfigurations.Initialized == false)
//                {
//                    robotConfigurations.RobotInitialization();
//                }
//            }
//        }
//        void c_AutomationSearchCompletedState(object sender, EventArgs e)
//        {
//            if (!IsHeadedAutomation)
//            {
//                robotConfigurations.SendAutomationAcknowledgement();
//                robotExecutionStatus = RoboExecutionStatus.RobotCompetedAutomation;
//               // Log.Logger.LogData("Robot: Search Completed", LogLevel.Transaction);
//            }
             
//        }
//        void c_TriggerSuccessResult(object sender, EventArgs e)
//        {
//            if (!IsHeadedAutomation)
//            {
//                robotConfigurations.TriggerSuccess();
//            }
//           // configuratorLoader.IsHeadedAutomation = IsHeadedAutomation; //setting it because somehow it is becoming false in case of Headed automation
           
//        }
//        //to set group related application list in sign in
//        //public SignInMgeAppDetails GetSignInApplications(int iCurrentGroupId, string strGroupName, string strCurrentUser = "")
//        //{
           
//        //    return configuratorLoader.GetSignInApplications(iCurrentGroupId, strGroupName, strCurrentUser);
//        //}
       

//        //public int LaunchCurrentGroupApplications()
//        //{
//        //    robotExecutionStatus = RoboExecutionStatus.RobotLaunching;
//        //    configuratorLoader._automationComponents = _automationComponents;
//        //    Log.Logger.LogData("Robot: Profile Launch Started", LogLevel.Transaction );
//        //    configuratorLoader.LoadCurrentGroupApplications();
//        //    Log.Logger.LogData("Robot: Profile Launch Completed", LogLevel.Transaction);
//        //    return 1;//true
//        //}
//        public int PreLaunchAllApplications()
//        {
//            throw new NotImplementedException();
//        }
//        public int PostLaunchAllApplications()
//        {
//            throw new NotImplementedException();
//        }
//        public int ResetCurrentGroupApplications(int timeoutInSeconds, Dictionary<string, string> dicResetValues)
//        {
//           // configuratorLoader.StartReset(timeoutInSeconds, dicResetValues);
//            return 1;//true
//        }
//        public int PreAutomate()
//        {
//            throw new NotImplementedException();
//        }
            
//        public void ShutDownCurrentApplications()
//        {
//            //configuratorLoader.UnloadApplications();
//        }
//        public int PostAutomate()
//        {
//            throw new NotImplementedException();
//        }
//        public bool ShutdownRobot(int timeoutInSeconds)
//        {
//            if (!IsHeadedAutomation)
//            {
//                Stopwatch s = new Stopwatch();
//                s.Start();
//                while ((s.Elapsed < TimeSpan.FromSeconds(timeoutInSeconds)))
//                {
//                    if (robotExecutionStatus != RoboExecutionStatus.RobotProcessingAutomation)
//                    {
//                        if (robotConfigurations != null)
//                            robotConfigurations.RobotUnSubscription();
//                        //wait for current automation execution
//                        break;
//                    }
//                    Thread.Sleep(500);
//                }
//                robotExecutionStatus = RoboExecutionStatus.RobotStopping;
//                //force shutdown
//            }
//            ShutDownCurrentApplications();
//           // Log.Logger.LogData("Robot: Shut down", LogLevel.Transaction);
            
//            System.Environment.Exit(100);
//            return true;
//        }
//        //InterProcessCommunicator interprocessCommunicator = null;
//        //string timestamp = string.Empty();
//        //int count = 0;
//        void tmr_Heartbeats_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            try
//            {
//               robotConfigurations.PublishNotifications(heartbeat);
//            }catch(Exception ex)
//            {
//                //Log.Logger.LogData("Robot: Error in sending heart beats", LogLevel.Error);
//            }
//        }
//    }
//}
