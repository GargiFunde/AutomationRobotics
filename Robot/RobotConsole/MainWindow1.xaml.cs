// <copyright file=MainWindow1.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RobotAutomationFramework;
using RobotLibrary.Interfaces;
using RabbitMQListenerRobot;
using System.Threading;
using RobotLibrary;
using System.ServiceModel;

namespace RobotConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window//, IMessagingComponent
    {
      // public IEnumerable<IMessagingComponent> _messagingComponents;
        BaseRobotFramework roboFramework = null;
        private SynchronizationContext _syncContext;
        public MainWindow()
        {
            InitializeComponent();
            HostAsAService();
            if (Application.Current !=null)
            {
                _syncContext = SynchronizationContext.Current;
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        RobotInitialization();
                    }));
            }
            else
            {
                RobotInitialization();
            }
        }
        public Dictionary<string,string> ConnectionAttributes { get; set; }
        public string ServerName { get; set; }
        public string PortNumber { get; set; }
        IMessageAdapter messageAdapter = null;
        private void RobotInitialization()
        {
            messageAdapter = new RabbitMQListenerRobot.RabbitMQListener();
            ConnectionAttributes = messageAdapter.Connect();
            if(ConnectionAttributes.ContainsKey("ServerName"))
            {
                ServerName = ConnectionAttributes["ServerName"];
            }
            if (ConnectionAttributes.ContainsKey("PortNumber"))
            {
                PortNumber = ConnectionAttributes["PortNumber"];
            }
            Dictionary<string,string> dicChannel = new Dictionary<string,string>();
            dicChannel.Add("QueueName","robot.q.automation");
            dicChannel.Add("RoutingKey","Self.Group1");
            messageAdapter.SubscribeToChannels(dicChannel);
            messageAdapter.MessageReceivedEvent -= MessageAdapter_MessageReceivedEvent;
            messageAdapter.MessageReceivedEvent += MessageAdapter_MessageReceivedEvent;

        }

        RequestProperties requestMessageProperties;
        private object delivarytag;
        public Dictionary<string, object> MessageHeaders { get; set; }

        public RobotDetails RobotConfigurationDetails { get; set; }
        private void MessageAdapter_MessageReceivedEvent(object sender, RequestEventArgs e)
        {
            byte[] byteSrcMsgId;
            string source_Message_Id = string.Empty;
            MessageHeaders = e.MessageHeader;
            requestMessageProperties = e.RequestMessageProperties;
            delivarytag = e.DeliveryTag;
            if(!string.IsNullOrEmpty (requestMessageProperties.Expiration ))
            {
                int interval = 0;
                Int32.TryParse(requestMessageProperties.Expiration, out interval);
                //Set searchtimeout for robot search method... here
            }
            if(e.MessageHeader.ContainsKey("source_message_id"))
            {
                byteSrcMsgId = (byte[])(object)e.MessageHeader["source_message_id"];
                source_Message_Id = Encoding.UTF8.GetString(byteSrcMsgId);
                //Need to write code...

                RequestInput rqInput = (RequestInput)XMLHelper.Deserialize(e.Message, typeof(RequestInput));
               // PerformSearchOnrobot(rqInput);
                Thread.Sleep(10000);
                messageAdapter.SendAcknowledge(delivarytag); //will not get other message in queue unless acknowledged ...
                delivarytag = null;
            }
            else //No source message id
            {
              requestMessageProperties.Expiration = RobotConfigurationDetails.MessageExpTimeout.ToString();
              messageAdapter.PublishToChannel(PublishQueueTypes.AutomationFailure.ToString(), "source message id is null", requestMessageProperties, MessageHeaders);
            }

        }

        public void HostAsAService()
        {
            ServiceHost svcHost = null;
            try
            {
                svcHost = new ServiceHost(typeof(RobotAsService));
                svcHost.Open(); Console.WriteLine("\n\nService is Running  at following address");
                Console.WriteLine("\nhttp://localhost:8733/Design_Time_Addresses/RobotConsole/RobotAsService/");
                //Console.WriteLine("\nnet.tcp://localhost:9002/MyMathService");
            }
            catch (Exception eX)
            {
                svcHost = null;
                Console.WriteLine("Service can not be started \n\nError Message [" + eX.Message + "]");
            }
            
            //if (svcHost != null)
            //{
            //    Console.WriteLine("\nPress any key to close the Service");
            //   // Console.ReadKey();
            //    svcHost.Close();
            //    svcHost = null;
            //}       

        }

        private void PerformSearchOnrobot(RequestInput rqInput)
        {
          
            roboFramework = new BaseRobotFramework();

            if (Application.Current != null)
            {
                _syncContext = SynchronizationContext.Current;
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    roboFramework.LaunchAllApplications(rqInput);
                    roboFramework.Automate(rqInput);
                }));
            }
            else
            {
                roboFramework.LaunchAllApplications(rqInput);
                roboFramework.Automate(rqInput);
            }            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            roboFramework.ShutDownCurrentApplications();

        }
    }
}
