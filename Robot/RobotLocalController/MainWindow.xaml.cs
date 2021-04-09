// <copyright file=MainWindow.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Windows;
using RobotLibrary;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Xml;
using System.Configuration;
//using RobotLibrary.Interfaces;
using CommonLibrary;
using Newtonsoft.Json;

namespace RobotLocalController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // public IEnumerable<IMessagingComponent> _messagingComponents;
        XmlDocument dom = new XmlDocument();
        InterProcessCommunicator interProcess = null;
        RobotDetailsList robotDetailsList = null;
        string strRobotList = string.Empty;
        AgentOperations agentOperations = null;
        public MainWindow()
        {
            strRobotList = ConfigurationManager.AppSettings["RobotListFile"];
            robotDetailsList = new RobotDetailsList();
          
           InitializeComponent();
           agentOperations = new AgentOperations();
           robotDetailsList.RobotsCollection = new ObservableCollection<RobotDetails>();
           SubscribeToNotifications();
           try
           {
               dom.Load(strRobotList);
           }
           catch (Exception ex)
           {
               //Log.Logger.LogData("Please check RobotList.xml file. " + ex.Message, LogLevel.Error);
           }
           if (dom.InnerXml != string.Empty)
           {
               robotDetailsList = (RobotDetailsList)RobotLibrary.XMLHelper.Deserialize(dom.InnerXml, typeof(RobotDetailsList));
               foreach (RobotDetails item in robotDetailsList.RobotsCollection)
               {
                   item.Delete += new DeleteEventHandler(DeleteRobot);
               }
               lstRobots.ItemsSource = robotDetailsList.RobotsCollection;
           }
            
        }

        void interProcess_communicationMessageReceived(object sender, CommunicatorMessageEventArgs e)
        {
            RobotLibrary.InterProcessCommands interprocCmd = JsonConvert.DeserializeObject<InterProcessCommands>(e.Message);
            if (interprocCmd.InterProcessCommandType == InterProcessCmdType.Heartbeat)
            {

            }
        }
       
      //  public ObservableCollection<RobotDetails> RobotsCollection { get; set; }
        public bool Start()
        {
           
            return true;
        }

        public bool Stop()
        {
            //interProcess.SendMessage(this)
            return true;
        }

        public bool Automate()
        {
            throw new NotImplementedException();
        }

        public bool Upgrade()
        {
            throw new NotImplementedException();
        }

        public int SendHeartBeats()
        {
            throw new NotImplementedException();
        }

        private void mnuAddRobot_Click(object sender, RoutedEventArgs e)
        {
            RobotDetails rd = new RobotDetails();
            rd.Delete += new DeleteEventHandler(DeleteRobot);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                rd.RobotFullName = openFileDialog.FileName;
                rd.RobotPath =openFileDialog.FileName.Remove(openFileDialog.FileName.IndexOf(openFileDialog.SafeFileName)); 
            }
            rd.RobotId = DateTime.Now.Ticks;
            robotDetailsList.RobotsCollection.Add(rd);
            lstRobots.ItemsSource = robotDetailsList.RobotsCollection;
          
        }

        private void DeleteRobot(object sender, RoboEventArgs e)
        {
            RobotDetails rd = (RobotDetails)sender;
            robotDetailsList.RobotsCollection.Remove(rd);
            lstRobots.ItemsSource = robotDetailsList.RobotsCollection;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InterProcessCommunicator interProcess = new InterProcessCommunicator(this);
            interProcess.communicationMessageReceived -= interProcess_communicationMessageReceived;
            interProcess.communicationMessageReceived += interProcess_communicationMessageReceived;
        }
        public Dictionary<string, string> ConnectionAttributes { get; set; }
        public string ServerName { get; set; }
        public string PortNumber { get; set; }
      private void SubscribeToNotifications()
        {
           
            RabbitMQListenerRobot.RabbitMQListener messageAdapter = new RabbitMQListenerRobot.RabbitMQListener();
            ConnectionAttributes = messageAdapter.Connect();
            if (ConnectionAttributes.ContainsKey("ServerName"))
            {
                ServerName = ConnectionAttributes["ServerName"];
            }
            if (ConnectionAttributes.ContainsKey("PortNumber"))
            {
                PortNumber = ConnectionAttributes["PortNumber"];
            }
            messageAdapter.SubscribeToNotifications();
            messageAdapter.NotificationReceivedEvent += messageAdapter_NotificationsReceivedEvent;
        }

      void messageAdapter_NotificationsReceivedEvent(object sender, NotificationEventArgs e)
      {
          string msg = e.Message;
          string robotName = string.Empty;
          //string profileName = string.Empty;
          //string processName = string.Empty;
         // throw new NotImplementedException();
          if (!string.IsNullOrEmpty(msg) && msg.Length > 0)
          {
              string[] list = msg.Split(new string[] { "~=~" }, StringSplitOptions.None);
              robotName = list[0];
              //profileName = list[1];
              //processName = list[2];
          }
          //string strCurrentUser = Environment.UserName;
          foreach (var item in robotDetailsList.RobotsCollection)
          {
              if (robotName == item.RobotName)
              {
                  agentOperations.StartRobot(item);
                  if (item.MainWindowHandle != IntPtr.Zero)
                  {
                      agentOperations.SignInRobot(item, e.Message);
                  }
              }
              //if(item.RobotFullName )
          }
      }
      private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
          dom.InnerXml = RobotLibrary.XMLHelper.Serialize(robotDetailsList);
          dom.Save(strRobotList);
      }
    }
}
