// <copyright file=RabbitMQListener company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date> </date>
// <summary></summary>

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RobotLibrary;
using System.Configuration;
using CommonLibrary;

namespace RabbitMQListenerRobot
{
    public class RabbitMQListener : IMessageAdapter, INotifyPropertyChanged
    {
        RabbitMQConnection rabbitMQConnection = null;

        private readonly object thisLock = new Object();
        public RabbitMQConnection RMQConnection 
        { 
            get{return rabbitMQConnection;}
            set { rabbitMQConnection = value; }
        }
        public IModel Channel { get; set; }
        public IConnection  Connection { get; set; }

        bool isConnected = false;
        public bool IsConnected
        {
            get {
                if (Connection == null)
                    isConnected = false;
                else
                    isConnected = Connection.IsOpen;
                return isConnected;
            }
            set {
                isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }
      
        public bool ISSQConnectionExists { get; set; }
        IConnectionFactory rqConnenctionFactory = null;
        public IConnectionFactory RQConnenctionFactory
        {
            get { return rqConnenctionFactory; }
            set { rqConnenctionFactory = value; }
        }
        public RabbitMQListener(RabbitMQConnection _rabbitMQConnection)
        {
            RMQConnection = _rabbitMQConnection;
            //RMQConnection = new RabbitMQConnection();
            //RMQConnection.Initialize();
        }
        public RabbitMQListener(IConnectionFactory _connectionFactory, RabbitMQConnection _rabbitMQConnection)
        {
            RQConnenctionFactory = _connectionFactory;
            if (_rabbitMQConnection != null)
            {
                this.RMQConnection = _rabbitMQConnection;
            }
            //else
            //{
            //    RMQConnection.Initialize();
            //}
        }

        void Initialize(RabbitMQConnection rabbitMQConnection)
        {
            try
            {
                if (RQConnenctionFactory == null)
                {
                    rqConnenctionFactory = new ConnectionFactory()
                    {
                        HostName = rabbitMQConnection.HostName,
                        Port = rabbitMQConnection.PortNumber,
                        UserName = rabbitMQConnection.UserName,
                        Password = rabbitMQConnection.Password,
                        AutomaticRecoveryEnabled = true,
                        TopologyRecoveryEnabled = true

                    };
                }
                if (Connection == null || !Connection.IsOpen)
                    Connection = RQConnenctionFactory.CreateConnection();

                Connection.ConnectionShutdown += Connection_ConnectionShutdown;
                if (Channel == null || !Channel.IsOpen)
                    Channel = Connection.CreateModel();
                var properties = Channel.CreateBasicProperties();
                properties.DeliveryMode = 2;//persist
                IsConnected = Connection.IsOpen;
            }
            catch (Exception ex)
            {

              System.IO.File.AppendAllText(@"C:\DebugService.txt", "Initialize Method Catch (Initialize RMQ): " + ex.Message);

            }
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            IsConnected = false;
            ISSQConnectionExists = false;
        }

        public virtual Dictionary<string,string>Connect()
        {
            if(!IsConnected)
            {
                Initialize(rabbitMQConnection);
                rabbitMQConnection.SubscriberQueueDetails.ForEach(p => { p.ChannelConsumer = string.Empty; });
            }
            Dictionary<string,string> connectionSettings = new Dictionary<string,string>();
            connectionSettings.Add("ServerName",rabbitMQConnection.HostName);
            connectionSettings.Add("PortNumber",rabbitMQConnection.PortNumber.ToString());
            connectionSettings.Add("ConnectionStatus",IsConnected==true?"Connected":"Disconnected");
            return connectionSettings;
        }

        public bool PublishToChannel(string queueType, string message, RequestProperties messageProperties, Dictionary<string, object> messageHeader)
        {
            OnPropertyChanged("IsConnected");
            if(!IsConnected )
            {
                //log error - connection must be established before trying to send any msg
                return false;
            }
            if(rabbitMQConnection ==null || rabbitMQConnection.PublisherQueueDetails ==null)
            {
                return false; //connection details are null
            }
            RabbitMQQueue publishingQueue = null;
            if(rabbitMQConnection.PublisherQueueDetails.Any(p=>p.QueueCriteriaFlag.ToLower()==queueType.ToLower()))
            {
                publishingQueue = rabbitMQConnection.PublisherQueueDetails.First(p=>p.QueueCriteriaFlag.ToLower()==queueType.ToLower());
            }
            else
            {
                return false; //No publishing queue of type
            }
            lock(thisLock)
            {
                if(!string.IsNullOrEmpty(publishingQueue.Exchange.Name))
                {
                    CreateExchangeForPublishing(publishingQueue);
                }
                if(messageProperties != null && messageProperties.CorrelationId  != null && !string.IsNullOrEmpty(messageProperties.CorrelationId))
                {
                    publishingQueue.QueueDetails.Name = messageProperties.ReplyTo;
                    publishingQueue.RoutingKey = messageProperties.ReplyTo;
                }
                if (string.IsNullOrEmpty(message))
                {
                    return false;//message should not be empty;
                }

                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = GetDeliveryPropertiesFromMessage(messageProperties);

                properties.Headers = messageHeader;

                Channel.BasicPublish(exchange: publishingQueue.Exchange.Name,
                    routingKey: publishingQueue.RoutingKey,
                    basicProperties: properties,
                    body: body);
            }
            return true;
        }

        public bool PublishNotifications(string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            
            Channel.BasicPublish(exchange: "amq.topic",
                                        routingKey: routingKey,
                                        basicProperties: null,
                                        body: body);
            return true;
        }

        public bool SubscribeToNotifications()
        {
            var consumer = new EventingBasicConsumer(Channel);
            // string strRobotName = ConfigurationManager.AppSettings["RobotName"];
            string strMachineName = Environment.MachineName;
            try
            {
                System.IO.File.AppendAllText(@"C:\DebugService.txt", "\n Decelaring the Queue");
                Channel.QueueDeclare(queue: strMachineName,//"yourmessage",//RobotName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            }catch(Exception ex)
            {
                //queue already exists;
              System.IO.File.AppendAllText(@"C:\DebugService.txt", "Subscribe to Notification RMQ Listener" + ex.Message);

            }

            Channel.QueueBind(queue: strMachineName, //"yourmessage",//RobotName,
             exchange: "amq.topic",
             routingKey: strMachineName); // "yourmessage");//RobotName);
            //Channel.BasicConsume(queue: "yourmessage", noAck: true, consumer: consumer);
            Channel.BasicConsume(queue: strMachineName, noAck: true, consumer: consumer);
            consumer.Received += consumer_Received;
            return true;
        }
        void consumer_Received(object sender, BasicDeliverEventArgs e)
        {           
            // RequestProperties properties = GetRequestPropertiesFromMessage(ea);
            var body1 = e.Body;
            var message1 = Encoding.UTF8.GetString(body1);
            var deliveryTag = e.DeliveryTag;
            //Console.WriteLine("Test");
            Thread.Sleep(500);
             OnNotificationsReceivedEvent(new NotificationEventArgs( message1));
            Thread.Sleep(500);

        }
        public virtual bool SubscribeToChannels(Dictionary<string,string> channelDetails)
        {
            if (!IsConnected)
            {
                //log error - connection must be established before trying to send any msg
                return false;
            }
            if (rabbitMQConnection == null || rabbitMQConnection.SubscriberQueueDetails == null)
            {
                return false; //connection details are null
            }
            rabbitMQConnection.SubscriberQueueDetails.ForEach(p =>
            {
                    lock(thisLock)
                    {
                    if(!string.IsNullOrEmpty(p.ChannelConsumer))
                    {
                        Channel.BasicCancel(p.ChannelConsumer);
                        p.ChannelConsumer= string.Empty;
                    }
                        //Create Exchange and queue if not created
                        if(channelDetails != null && channelDetails.ContainsKey("QueueName")&& (p.QueueDetails.Name.Contains("{0}")))
                            p.QueueDetails.Name = string.Format(p.QueueDetails.Name,channelDetails["QueueName"]); 
      
                        if(channelDetails != null && channelDetails.ContainsKey("RoutingKey")&& (p.RoutingKey.Contains("{0}")))
                            p.RoutingKey = string.Format(p.RoutingKey,channelDetails["RoutingKey"]); 

                        CreaQueueForSubscription(p);

                        var consumer = new EventingBasicConsumer(Channel);
                        consumer.Received += (model, ea)=>
                            {
                                RequestProperties properties = GetRequestPropertiesFromMessage(ea);
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                var deliveryTag = ea.DeliveryTag;

                                Thread.Sleep(500);
                                OnMessageReceivedEvent(new RequestEventArgs(p.QueueDetails.Name,message,properties,deliveryTag, (Dictionary<string,object>)ea.BasicProperties.Headers));
                                Thread.Sleep(500);
                            };

                        Channel.BasicConsume(queue:p.QueueDetails.Name,
                            noAck: p.QueueDetails.ReqAcknowlegement ,
                            consumer: consumer);
                         
                        ISSQConnectionExists = true ;

                        Thread.Sleep(1000);
                        p.ChannelConsumer =consumer.ConsumerTag;
                    }
            });
            return true;
        }

        private RequestProperties GetRequestPropertiesFromMessage(BasicDeliverEventArgs ea)
        {
            if(ea==null || ea.BasicProperties ==null)
            {
                return null;
            }
            RequestProperties properties = new RequestProperties();
            properties.SourceApplicationId = ea.BasicProperties.AppId;
            properties.MessageId = ea.BasicProperties.MessageId;
            properties.TimeStamp = ea.BasicProperties.Timestamp.UnixTime;
            properties.ContentType = ea.BasicProperties.ContentType;
            properties.ContentEncoding = ea.BasicProperties.ContentEncoding;
            properties.Expiration = ea.BasicProperties.Expiration;
            properties.CorrelationId = ea.BasicProperties.CorrelationId;
            properties.ReplyTo = ea.BasicProperties.ReplyTo;
            return properties;
        }

        public event EventHandler<RequestEventArgs> MessageReceivedEvent;
        protected virtual void OnMessageReceivedEvent(RequestEventArgs e)
        {
            if(MessageReceivedEvent != null)
            {
                this.MessageReceivedEvent(this, e);
            }
        }
        public event EventHandler<NotificationEventArgs> NotificationReceivedEvent;
        protected virtual void OnNotificationsReceivedEvent(NotificationEventArgs e)
        {
            if (NotificationReceivedEvent != null)
            {
                this.NotificationReceivedEvent(this, e);
            }
        }
        private void CreaQueueForSubscription(RabbitMQQueue p)
        {
            Dictionary<string, object> argDictionary = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(p.QueueDetails.DLX))
            {
                argDictionary.Add("x-dead-letter-exchange", p.QueueDetails.DLX);
            }
            if (!string.IsNullOrEmpty(p.QueueDetails.DLXRoutingKey))
            {
                argDictionary.Add("x-dead-letter-routing-key", p.QueueDetails.DLXRoutingKey);
            }
            foreach (var arg in p.QueueDetails.Arguments)
            {
                if(!string.IsNullOrEmpty(arg.ArgKey))
                {
                    int result = -1;
                    if (Int32.TryParse(arg.ArgValue, out result))
                        argDictionary.Add(arg.ArgKey, result);
                    else
                        argDictionary.Add(arg.ArgKey, arg.ArgValue);
                }
            }
            Channel.QueueDeclare(queue: p.QueueDetails.Name,
                durable: p.QueueDetails.Durable,
                exclusive: false,
                autoDelete: p.QueueDetails.AutoDelete,
                arguments: argDictionary);
            Channel.BasicQos(0, 1, false);

            bool bindExchangeToQueue = p.BindExchangeToQueue;
            if(bindExchangeToQueue && ! string.IsNullOrEmpty(p.QueueDetails.Name))
            {
                Channel.QueueBind(queue: p.QueueDetails.Name,
                    exchange: p.Exchange.Name,
                    routingKey: p.RoutingKey);
            }

        }
        
        //void consumer_Received(object sender, BasicDeliverEventArgs e)
        //{
        //    throw new NotImplementedException();
        //} 
        IBasicProperties GetDeliveryPropertiesFromMessage(RequestProperties messageProperties)
        {
            IBasicProperties properties = new RabbitMQ.Client.Framing.BasicProperties();
            if(messageProperties != null)
            {
                if (!string.IsNullOrEmpty(messageProperties.Expiration))
                {
                    properties.Expiration = messageProperties.Expiration;
                }
                if (!string.IsNullOrEmpty(messageProperties.MessageId))
                {
                    properties.MessageId = messageProperties.MessageId ;
                }
                if (!string.IsNullOrEmpty(messageProperties.SourceApplicationId))
                {
                    properties.AppId = messageProperties.SourceApplicationId;
                }
                if (!string.IsNullOrEmpty(messageProperties.ContentType))
                {
                    properties.ContentType = messageProperties.ContentType;
                }
                if (!string.IsNullOrEmpty(messageProperties.ContentEncoding))
                {
                    properties.ContentEncoding = messageProperties.ContentEncoding;
                }
                if (!string.IsNullOrEmpty(messageProperties.CorrelationId))
                {
                    properties.CorrelationId = messageProperties.CorrelationId;
                }
                if (!string.IsNullOrEmpty(messageProperties.ReplyTo))
                {
                    properties.ReplyTo = messageProperties.ReplyTo;
                }
                
            }
            else
            {
                //Error: request properties are null
            }
            return properties;
        }

        private void CreateExchangeForPublishing(RabbitMQQueue publishingQueue)
        {
            if(publishingQueue.Exchange ==null)
            {
                //error: exchange is null
                return;
            }
            if(string.IsNullOrEmpty(publishingQueue.Exchange.Name))
            {
                //exchange name is null
                return;
            }
            if (string.IsNullOrEmpty(publishingQueue.Exchange.Type))
            {
                //exchange type is null
                return;
            }
            Channel.ExchangeDeclare(exchange: publishingQueue.Exchange.Name,
                type: publishingQueue.Exchange.Type,
                durable: publishingQueue.Exchange.Durable);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual bool SendAcknowledge(object ackTag)
        {
            if(!IsConnected)
            {
                //Connection is closed
                return false;
            }
            if(ackTag != null)
            {
                Channel.BasicAck(deliveryTag: (ulong)ackTag, multiple: false);
            }
            return true;
        }
        public bool UnSubscribeFromChannels()
        {
            if (!IsConnected)
            {
                //Connection is closed
                return false;
            }
            if((rabbitMQConnection==null)|| (rabbitMQConnection.SubscriberQueueDetails == null))
            {
                return false;
            }
            rabbitMQConnection.SubscriberQueueDetails.ForEach(queueDetail=>
                {
                lock(thisLock)
                {
                    Channel.BasicCancel(queueDetail.ChannelConsumer);
                }
                queueDetail.ChannelConsumer = string.Empty;
                });
            ISSQConnectionExists = false;
            return true;
        }
    }
}
