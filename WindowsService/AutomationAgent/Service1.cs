#region Headers
// <copyright file=Service1 company=E2E BOTS>
// Copyright (c) 2019 E2E BOTS Pvt Ltd All Rights Reserved
// </copyright>
// <author>Piyush Bhiwapurkar</author>
// <date> 02-01-2020 16:03:06</date>
// <summary></summary>

using CommonLibrary;
using RabbitMQListenerRobot;
using TCP.Server;
using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using System.Net;
//using System.Net.Sockets;
using System.ServiceProcess;
//using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP.Shared.Enums;
using System.Runtime.Serialization.Json;
using Logger;
using System.Globalization;
#endregion

#region Local Security Policies

//1. Start -> Control Panel -> Administrative Tools -> Local Security Policy
//2. Navigate to Security\Local Policies\Security Options
// a.Network Access: Let everyone permissions apply to anonymous users - Set to Enabled
//c.DCOM: Machine Access Restrictions - Add Anonymous, Everyone, Interactive, Network, System with full rights options set.
//d.Network Access: Let everyone permissions apply to anonymous users - Set to Enabled
//e. Network Access: Sharing security model for local accounts - Set to Classic

//The "Sharing Security model" is the real offending item I believe, and setting the above should fix the problem. If not then I went as far as setting the following in DCOMCNFG.

//DCOM Configuration

//1. Click Start -> Run
//2. Enter DCOMCNFG and press OK.This will open the DCOMCNFG window.
//3. Browse down the tree to Console Root -> Component Services -> Computers -> My Computer
//4. right-click on "My Computer" and select properties
//5. Select the "Default Properties" tab
// a. Enable Distributed COM on this computer - Option is checked
//b.Default Authentication Level - Set to Connect
//c. Default Impersonation Level - Set to Identify
//6. Select the "COM Security" tab
//7. Click on Access Permissions ' Edit Default
//a.Add "Anonymous", "Everyone", "Interactive", "Network", "System" with Local and Remote access permissions set.
//8. Click on Launch and Activation Permissions ' Edit Default
//a.Add "Anonymous", "Everyone", "Interactive", "Network", "System" with Local and Remote access permissions set.
//9. Click on OK
//10. Close the DCOMCNFG window
#endregion

namespace AutomationAgent
{
    public partial class Service1 : ServiceBase
    {
        private TCP.Server.Server server;
        private System.Timers.Timer updateListTimer;
        private System.Timers.Timer starterTimer;
        List<string> lstdisconnected = null;
        private object lockobj;
        bool starting = false;
        //int icountToremoveInitialAlerts = 0;
        private Logger.ServiceReference1.BOTServiceClient servRef = null; //Reference to WCF Service Hosted Over IIS Server. Declared inside Constrructor
        string strBotId = string.Empty;
        string strBotName = string.Empty;
        string strBotStatus = string.Empty;

        public Service1()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure(); // Added By Piyush
            starting = true;
            server = new Server(8888);
            lstdisconnected = server.LstDisconnected;
            updateListTimer = new System.Timers.Timer();
            updateListTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["StatusInterval"]);
            updateListTimer.Elapsed += updateListTimer_Tick;

            starterTimer = new System.Timers.Timer();
            starterTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["StartupPeriod"]);
            starterTimer.Elapsed += starterTimer_Tick;

            lockobj = new object();

            servRef = new Logger.ServiceReference1.BOTServiceClient();   //Reference to WCF Service Hosted Over IIS Server.
        }

        void starterTimer_Tick(object sender, EventArgs e)
        {
            starterTimer.Stop();
            starting = false;
        }

        void updateListTimer_Tick(object sender, EventArgs e)
        {
            UpdateClientsList();
        }
        
       
        private void UpdateClientsList()
        {
            try
            {
                foreach (string receivername in lstdisconnected)
                {
                    if(server.Receivers.Any(p => p.Email == receivername))
                    {
                        Receiver receiver = server.Receivers.First(p => p.Email == receivername);
                       
                        server.Receivers.Remove(receiver);
                        //need to check - do we need to disconnect receiver explicitly?
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Robot Name: " + receiver.ToString() + "\n");
                        PublishNotifications(receivername);
                    }
                }
                lstdisconnected.Clear();
                foreach (var receiver in server.Receivers)
                {
                    System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>UpdateClientsList=>receiver.Email: " + receiver.Email.ToString() + "\n");
                    lstdisconnected.Add(receiver.Email);
                }
              
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Exception=>Service1.cs=>UpdateClientsList " + ex.Message.ToString() + "\n");
                //Collection is modified
            }
        }
        public void PublishNotifications(string RobotName)               //Robot Name == Current Machine Name
        {
            string strMachine = Environment.MachineName;
            HeartBeat heartbeat = new HeartBeat();
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>Publish Notifications strMachine: " + strMachine + "\n");
            heartbeat.MachineName = strMachine;
            heartbeat.RobotName = RobotName;
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>Publish Notifications RobotName: " + RobotName + "\n");
            heartbeat.roboExecutionStatus = RoboExecutionStatus.RobotStopped; 
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HeartBeat));
            ser.WriteObject(stream1, heartbeat);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);

            #region Comment
            //Change Routing Key Configuration to IP for System.
            //string RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"];
            //Call service Method for HUB.
            //messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());
            //Logger.ServiceReference1.BOTServiceClient serviceRef = new Logger.ServiceReference1.BOTServiceClient();
            //serviceRef.sendMessageToClient();
            #endregion
        }
        protected override void OnStart(string[] args)
        {
            #region Comment
            //  string applicationName = "\"E:\\Work\\BOTDesignerMaster\\Build\\BackOfficeBot.exe\"";
            //// String applicationName = "cmd.exe";    
            //ApplicationLoader.PROCESS_INFORMATION procInfo;
            //ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);
            #endregion
            try
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>Subscribing to Notifications " + "\n");
                SubscribeToNotifications();
                var test = Task.Run(() =>
                {
                    System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>Starting Server " + "\n");
                    server.Start();
                });

                updateListTimer.Start();
                starterTimer.Start();
            }
            catch(Exception ex)
            {
                Logger.Log.Logger.LogData("Exception While Starting from Service: "+ex.Message,LogLevel.Error);
            }
        }
              
        protected override void OnStop()
        {
            server.Stop();
        }

        #region Subscribe To Notifications
        public Dictionary<string, string> ConnectionAttributes { get; set; }

        RabbitMQListenerRobot.RabbitMQListener messageAdapter = null;
        private void SubscribeToNotifications(/*RequestInput requestInput*/)
        {
            try
            {
                string strMachine = Environment.MachineName;

                //string GroupName = requestInput.AutomationGroupName;
                //string TenantName = requestInput.TenantName;
                //string ProcessName = requestInput.AutomationProcessName;
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Subscribe to Notification before getting datatable" + "\t" + Environment.MachineName);
                DataTable dt = GetRQDetails();
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Subscribe to Notification after getting datatable" + "\t" + dt.Rows.Count.ToString());


                if ((dt != null) && (dt.Rows.Count > 0))
                {

                    RabbitMQConnection RMQConnection = new RabbitMQConnection();
                    RMQConnection.HostName = (string)dt.Rows[0][0];     //ConfigurationManager.AppSettings["ServerName"]; -- Rabbitmq server name
                    RMQConnection.PortNumber = (int)dt.Rows[0][1];      //Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    RMQConnection.UserName = (string)dt.Rows[0][2];     //ConfigurationManager.AppSettings["CurrentUser"];
                    RMQConnection.Password = (string)dt.Rows[0][3];     //ConfigurationManager.AppSettings["Password"]; 
                    System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Subscribe to Notification RAbbitmq Credentials" +"\t" +RMQConnection.HostName +"\t" +RMQConnection.PortNumber + "\t" + RMQConnection.UserName);

                    messageAdapter = new RabbitMQListenerRobot.RabbitMQListener(RMQConnection);
                    ConnectionAttributes = messageAdapter.Connect();
                    messageAdapter.SubscribeToNotifications();
                    messageAdapter.NotificationReceivedEvent -= messageAdapter_NotificationsReceivedEvent;
                    messageAdapter.NotificationReceivedEvent += messageAdapter_NotificationsReceivedEvent;
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.Log.Logger.LogData("NullReferenceException While Subscibing to Notifications from Service: " + ex.Message, LogLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception While Subscibing to Notifications from Service: " + ex.Message, LogLevel.Error);
            }
        }
        #endregion

        void RemoveInitialAlerts()
        {
            
        }
        void messageAdapter_NotificationsReceivedEvent(object sender, NotificationEventArgs e)
        {
            string action = string.Empty;
            string botname = string.Empty;
            string botid = string.Empty;
            string pwd = string.Empty;
            string groupid = string.Empty;
            string queueName = string.Empty;
            string chronExpression = string.Empty;
            Logger.ServiceReference1.BOTServiceClient bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();

            string tenantid = string.Empty;
            try
            {
                CultureInfo cultures = new CultureInfo("en-US");
                //to remove initial unnecessory start stop alerts from RabbitMQ
                if (starting == true)
                {
                    return;
                }

                lock (lockobj)
                {
                    string[] list = null;
                    string msg = e.Message;

                    if (!string.IsNullOrEmpty(msg) && msg.Length > 0)
                    {
                        list = msg.Split(new string[] { "!#~=~!#" }, StringSplitOptions.None);

                        action = list[0];
                        botname = list[1];
                        botid = list[2];
                        pwd = list[3];
                        tenantid = list[4];

                        //Added By Piyush for Logger.
                        groupid = list[5];
                        queueName = list[6];

                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent action: " + action + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent botname: " + botname + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent botid: " + botid + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent pwd: " + pwd + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent tenantid: " + tenantid + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent groupid: " + groupid + "\n");
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent queueName: " + queueName + "\n");

                        Log.GroupId = Convert.ToInt32(groupid, cultures);
                        Log.TenantId = Convert.ToInt32(tenantid, cultures);
                        Log.TenantName = tenantid;
                        Log.BotName = botname;

                        if ((action.ToLower(new CultureInfo("en-US", false)) == "schedule") && (list.Length > 7))
                        {
                            chronExpression = list[7];
                            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>messageAdapter_NotificationsReceivedEvent chronExpression: " + chronExpression);
                        }
                                                
                        Logger.Log.Logger.LogData("Starting Process", LogLevel.Info);

                        Receiver receiver = server.GetBotListener(botname);
                        AgentOperations agentOperations = new AgentOperations();
                        if ((receiver == null) || (receiver.Status == StatusEnum.Disconnected) || (receiver.Status == StatusEnum.NotFound))
                        {

                            if ((!string.IsNullOrEmpty(action)) && (action != null))
                            {
                                if ((action.ToLower(new CultureInfo("en-US", false)) == "stop") || (action.ToLower(new CultureInfo("en-US", false)) == "stopforcefully"))
                                {
                                    //server.SendMessageToClient(botname, msg);
                                    agentOperations.StopForcefully(list);

                                    /*Added By Piyush to kill  Process*/
                                    Logger.Log.Logger.LogData("Process Stopping Now", LogLevel.Info);
                                    try
                                    {
                                        string processName = "BackOfficeBot";
                                        Process[] foundProcess = Process.GetProcessesByName(processName);

                                        foreach (Process p in foundProcess)
                                        {
                                            p.Kill();
                                        }
                                        Logger.Log.Logger.LogData("Process Stopped/Killed Now", LogLevel.Info);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    //May Need to implement logic of forcefull stop - to handle situation like closed from Taskmanager or started from outside
                                }
                                else //((action.ToLower() == "start")||(action.ToLower() == "schedule"))
                                {
                                    //First start robot and then send message to listen to any perticular queue. or scheduling will not work properly
                                    if (string.IsNullOrEmpty(chronExpression))
                                    {
                                        chronExpression = "Manually Started and Running";
                                    }
                                    int result5 = bOTServiceClient.CreateScheduleStatus(queueName, botname, chronExpression, "Running", Log.GroupId, Log.TenantId, DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"), "");

                                    StartBotProcess(list, msg, botname, agentOperations);
                                    Logger.Log.Logger.LogData("Process Started Now", LogLevel.Info);
                                }
                            }
                        }
                        else
                        {
                            // if it is already started and running, it might ne schedule or default queue..so need to stop 
                            if ((action.ToLower() == "start"))
                            {
                                //first send stop message to process to stop it
                                //then start bot exe
                                //then send schedule message
                                Logger.Log.Logger.LogData("Already Started Process Started Now", LogLevel.Info);
                                string msg1 = msg.Replace("start", "stop");
                                StopStartBotProcess(list, msg, botname, agentOperations, msg1);
                            }
                            else if ((action.ToLower() == "schedule"))
                            {
                                //first send stop message to process to stop it
                                //then start bot exe
                                //then send schedule message
                                Logger.Log.Logger.LogData("Already Scheduled Process Started Now", LogLevel.Info);
                                string msg1 = msg.Replace("schedule", "stop");
                                StopStartBotProcess(list, msg, botname, agentOperations, msg1);
                            }
                            
                            else if (action.ToLower() == "stop")
                            {
                                Logger.Log.Logger.LogData("Already Started Process Stopping Now", LogLevel.Info);
                                server.SendMessageToClient(botname, msg);
                                /*Added By Piyush to kill  Process*/
                                Logger.Log.Logger.LogData("Process Stopping Now", LogLevel.Info);
                                try
                                {
                                    string processName = "BackOfficeBot";
                                    Process[] foundProcess = Process.GetProcessesByName(processName);

                                    foreach (Process p in foundProcess)
                                    {
                                        p.Kill();
                                    }
                                    Logger.Log.Logger.LogData("Process Stopped/Killed Now", LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Exception while Stoping a Process: " + ex.Message, LogLevel.Error);
                                }
                            }
                            else if (action.ToLower() == "stopforcefully")
                            {
                                agentOperations.StopForcefully(list);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                int ServiceError = bOTServiceClient.CreateScheduleStatus(queueName, botname, chronExpression, "Error", Log.GroupId, Log.TenantId, "", DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                Logger.Log.Logger.LogData("Exception in Message Adapter Receiving Events: " + ex.Message, LogLevel.Error);
                servRef.insertLog("Service Exception", "Exception :" + ex.Message, Convert.ToInt32(groupid), Convert.ToInt32(tenantid));
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Exception=>Service1.cs=>messageAdapter_NotificationsReceivedEvent " + ex.Message.ToString() + "\n");
            }
            finally {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "\n Service1.cs=>finally: "+ "\n");
            }
        }

        private void StopStartBotProcess(string[] list, string msg, string botname, AgentOperations agentOperations, string msg1)
        {
            try
            {
                server.SendMessageToClient(botname, msg1);
                int iCount = 0;
                bool forcestop = true;
                while (iCount <= 30)
                {
                    iCount = iCount + 1;
                    Receiver receiver1 = server.GetBotListener(botname);
                    if ((receiver1 == null) || (receiver1.Status == StatusEnum.Disconnected) || (receiver1.Status == StatusEnum.NotFound))
                    {
                        forcestop = false;
                        StartBotProcess(list, msg, botname, agentOperations);
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                if (forcestop == true)
                {
                    StartBotProcess(list, msg, botname, agentOperations);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception While StopStartBotProcess to Notifications from Service: " + ex.Message, LogLevel.Error);
            }
        }

        private void StartBotProcess(string[] list, string msg, string botname, AgentOperations agentOperations)
        {
            try
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>StartBotProcess"+ "\n");
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>StartBotProcess list: " + list.ToString() + "\n");
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>StartBotProcess msg: " + msg.ToString() + "\n");
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>StartBotProcess botname: " + botname.ToString() + "\n");

                agentOperations.StartRobot(list);
                Receiver receiver1 = server.GetBotListener(botname);
                int iCount = 0;
                while (iCount <= 120)
                {
                    iCount = iCount + 1;

                    if ((receiver1 == null) || (receiver1.Status == StatusEnum.Disconnected) || (receiver1.Status == StatusEnum.NotFound))
                    {
                        Thread.Sleep(500);
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>Receiver Stopped: " + "\n");
                        receiver1 = server.GetBotListener(botname);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Service1.cs=>RSending Message to CLient: " + "\n");
                        server.SendMessageToClient(botname, msg);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "\n Exception at Service Start Bot::" + ex.Message);
                Logger.Log.Logger.LogData("Exception While StartBotProcess to Notifications from Service: " + ex.Message, LogLevel.Error);
            }
        }

        private DataTable GetRQDetails()
        {
            DataTable rqDetails = null;
            try
            {
                string strMachine = string.Empty;
                strMachine = Environment.MachineName;          //Capturing Current Machine Name.

                rqDetails = servRef.GetRQDetails(strMachine);  //Machine  Name must be mapped with Database and should be added from Control Tower First to access Details from RQDetails Database Table.
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt",strMachine+"\n");
                if (null == rqDetails)
                {
                    System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "GetRQDetails, The Datatable is null" + "\n");
                    Logger.Log.Logger.LogData("RQ Details: No Relevant Data available in Database.", LogLevel.Error);
                }
                return rqDetails;
            }
            catch (NullReferenceException ex)
            {
                System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "RQDetails" + ex.Message + "\n");
                Logger.Log.Logger.LogData("Null Reference Exception While Getting RQ Details from Service: " + ex.Message, LogLevel.Error);
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception While Getting RQ Details from Service: " + ex.Message, LogLevel.Error);
                return null;
            }
            finally
            {
                //return rqDetails;
            }
        }
    }
}
