// <copyright file=Service1 company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using CommonLibrary;
using RabbitMQListenerRobot;
using TCP.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCP.Shared.Enums;
using System.Runtime.Serialization.Json;


//Local Security Policies

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
        int icountToremoveInitialAlerts = 0;
        public Service1()
        {
            InitializeComponent();
            starting = true;
            server = new Server(8888);
            lstdisconnected = server.LstDisconnected;
            updateListTimer = new System.Timers.Timer();
            updateListTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["StatusInterval"]);
            updateListTimer.Elapsed +=  updateListTimer_Tick;

            starterTimer = new System.Timers.Timer();
            starterTimer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["StartupPeriod"]);
            starterTimer.Elapsed += starterTimer_Tick;

            lockobj = new object();
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
        string strBotId = string.Empty;
        string strBotName = string.Empty;
        string strBotStatus = string.Empty;
       
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
                        PublishNotifications(receivername);
                    }
                }
                lstdisconnected.Clear();
                foreach (var receiver in server.Receivers)
                {
                    lstdisconnected.Add(receiver.Email);
                }
              
            }
            catch (Exception ex)
            {
                //Collection is modified
            }
        }
        public void PublishNotifications(string RobotName)
        {
            string strMachine = Environment.MachineName;
            HeartBeat heartbeat = new HeartBeat();
            heartbeat.MachineName = strMachine;
            heartbeat.RobotName = RobotName;
            heartbeat.roboExecutionStatus = RoboExecutionStatus.RobotStopped; 
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HeartBeat));
            ser.WriteObject(stream1, heartbeat);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string RoutingKeyNotifiation = ConfigurationManager.AppSettings["RoutingKeyNotifiation"];
            messageAdapter.PublishNotifications(RoutingKeyNotifiation, sr.ReadToEnd());
        }
        protected override void OnStart(string[] args)
        {
            //  string applicationName = "\"E:\\Work\\BOTDesignerMaster\\Build\\BackOfficeBot.exe\"";
            //// String applicationName = "cmd.exe";
            //ApplicationLoader.PROCESS_INFORMATION procInfo;
            //ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);
            try
            {
                SubscribeToNotifications();
                var test = Task.Run(() =>
                {
                  server.Start();
                });

                updateListTimer.Start();
                starterTimer.Start();
            }
            catch(Exception ex)
            {
               
            }
        }
              
        protected override void OnStop()
        {
            server.Stop();
        }
        public Dictionary<string, string> ConnectionAttributes { get; set; }

        RabbitMQListenerRobot.RabbitMQListener messageAdapter = null;
        private void SubscribeToNotifications()
        {
            try
            { 
                string strMachine = Environment.MachineName;
          
                DataTable dt = GetRQDetails();
              
                if ((dt != null)&&(dt.Rows.Count >0))
                {
                  
                    RabbitMQConnection RMQConnection = new RabbitMQConnection();
                    RMQConnection.HostName =(string) dt.Rows[0][0]; // ConfigurationManager.AppSettings["ServerName"]; -- Rabbitmq server name
                    RMQConnection.PortNumber =(int) dt.Rows[0][1];//Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    RMQConnection.UserName = (string)dt.Rows[0][2]; //ConfigurationManager.AppSettings["CurrentUser"];
                    RMQConnection.Password = (string)dt.Rows[0][3]; //ConfigurationManager.AppSettings["Password"]; ;
                                      
                    messageAdapter = new RabbitMQListenerRobot.RabbitMQListener(RMQConnection);
                    ConnectionAttributes = messageAdapter.Connect();
                    messageAdapter.SubscribeToNotifications();
                    messageAdapter.NotificationReceivedEvent -= messageAdapter_NotificationsReceivedEvent;
                    messageAdapter.NotificationReceivedEvent += messageAdapter_NotificationsReceivedEvent;
                }
            }
            catch (Exception ex)
            {
             
            }
        }
        void RemoveInitialAlerts()
        {
            
        }
        void messageAdapter_NotificationsReceivedEvent(object sender, NotificationEventArgs e)
        {
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

                    string action = list[0];
                    string botname = list[1];
                    string botid = list[2];
                    string pwd = list[3];
                    string tenantid = list[4];

                    Receiver receiver = server.GetBotListener(botname);
                    AgentOperations agentOperations = new AgentOperations();
                    if ((receiver == null) || (receiver.Status == StatusEnum.Disconnected) || (receiver.Status == StatusEnum.NotFound))
                    {
                       
                        if ((action != string.Empty) && (action != null))
                        {
                             if ((action.ToLower() == "stop")||(action.ToLower() == "stopforcefully"))
                            {
                                //server.SendMessageToClient(botname, msg);
                                agentOperations.StopForcefully(list);
                                //May Need to implement logic of forcefull stop - to handle situation like closed from Taskmanager or started from outside
                            }
                            else //((action.ToLower() == "start")||(action.ToLower() == "schedule"))
                            {
                                //First start robot and then send message to listen to any perticular queue. or scheduling will not work properly
                                StartBotProcess(list, msg, botname, agentOperations);
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
                            string msg1 = msg.Replace("start", "stop");
                            StopStartBotProcess(list, msg, botname, agentOperations, msg1);
                        }
                        else if ((action.ToLower() == "schedule"))
                        {
                            //first send stop message to process to stop it
                            //then start bot exe
                            //then send schedule message
                            string msg1 = msg.Replace("schedule", "stop");
                            StopStartBotProcess(list, msg, botname, agentOperations, msg1);
                        }
                        else if (action.ToLower() == "stop")
                        {
                            server.SendMessageToClient(botname, msg);
                        }
                        else if (action.ToLower() == "stopforcefully")
                        {
                            agentOperations.StopForcefully(list);
                        }
                    }
                   
                }
               
            }
        }

        private void StopStartBotProcess(string[] list, string msg, string botname, AgentOperations agentOperations, string msg1)
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
            if(forcestop ==true)
            {
                StartBotProcess(list, msg, botname, agentOperations);
            }
        }

        private void StartBotProcess(string[] list, string msg, string botname, AgentOperations agentOperations)
        {
            agentOperations.StartRobot(list);
            Receiver receiver1 = server.GetBotListener(botname);
            int iCount = 0;
            while (iCount <= 120)
            {
                iCount = iCount + 1;

                if ((receiver1 == null) || (receiver1.Status == StatusEnum.Disconnected) || (receiver1.Status == StatusEnum.NotFound))
                {
                    Thread.Sleep(500);
                    receiver1 = server.GetBotListener(botname);
                }
                else
                {
                    server.SendMessageToClient(botname, msg);
                    break;
                }
            }
        }

        private DataTable GetRQDetails()
        {
            DataTable result = null;
            try
            {
              
                string strMachine = Environment.MachineName;
                ServiceReference1.BOTServiceClient bOTServiceClient = new ServiceReference1.BOTServiceClient();
                result = bOTServiceClient.GetRQDetails(strMachine);
               
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
    }
}
