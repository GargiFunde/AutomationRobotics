// <copyright file=MQHandler company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using RabbitMQ.Client;
using RabbitMQListenerRobot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAgent
{
    public class QueueInterface
    {


        //public void TriggerStart(string botname, int tenantid, string userid)
        //{
          
        //    DataTable result = GetBotStartDetails(botname);
        //    if ((result != null) && (result.Rows.Count > 0))
        //    {
        //        AutomationRequest(result);
        //    }
        //}
        //public DataTable GetBotStartDetails(string botname)
        //{
        //    DataTable result = null;
        //    try
        //    {
        //        ServiceReference1.BOTServiceClient  bOTServiceClient = new ServiceReference1.BOTServiceClient();
        //        result = bOTServiceClient.GetBotStartDetails(botname);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return result;
        //}
        //public bool AutomationRequest(DataTable result)
        //{
        //    string RobotName = string.Empty;
        //    string UserId = string.Empty;
        //    string Password = string.Empty;
        //    int TenanatId = 0;
        //    int EnvironmentId = 0;
        //    string ExchangeTopicName = string.Empty;
        //    string RoutingKey = string.Empty;
        //    string ServerName = string.Empty;
        //    string QueueName = string.Empty;
        //    if (result.Rows[0][0] != null)
        //        RobotName = (string)result.Rows[0][0];
        //    if (result.Rows[0][1] != null)
        //        UserId = (string)result.Rows[0][1];
        //    if (result.Rows[0][2] != null)
        //        Password = (string)result.Rows[0][2];
        //    if (result.Rows[0][3] != null)
        //        TenanatId = (int)result.Rows[0][3];
        //    if (result.Rows[0][4] != null)
        //        EnvironmentId = (int)result.Rows[0][4];
        //    if (result.Rows[0][5] != null)
        //        ExchangeTopicName = (string)result.Rows[0][5];
        //    if (result.Rows[0][6] != null)
        //        RoutingKey = (string)result.Rows[0][6];
        //    if (result.Rows[0][7] != null)
        //        ServerName = (string)result.Rows[0][7];
        //    if (result.Rows[0][8] != null)
        //        QueueName = (string)result.Rows[0][8];
        //    string message = RobotName + "~=~" + UserId + "~=~" + Password + "~=~" + TenanatId + "~=~" + EnvironmentId + "~=~" + ExchangeTopicName + "~=~" + RoutingKey + "~=~" + QueueName;

        //    RabbitMQQueue publisherSubscriberQueue = new RabbitMQQueue();
        //    publisherSubscriberQueue.BindExchangeToQueue = true;
        //    ExchangeDetails ed = new ExchangeDetails();
        //    ed.AutoDelete = false;
        //    ed.Durable = true;
        //    ed.IsInternal = false;
        //    ed.Name = ExchangeTopicName;// "robot.x.automation";
        //    ed.Type =  "topic";
        //    ed.Arguments = new List<ExchangeArgs>();
        //    ed.Arguments.Add(new ExchangeArgs());
        //    publisherSubscriberQueue.Exchange = ed;
        //    publisherSubscriberQueue.QueueDetails = new QDetails();
        //    publisherSubscriberQueue.QueueDetails.AutoDelete = false;
        //    publisherSubscriberQueue.QueueDetails.Durable = true;
        //    publisherSubscriberQueue.QueueDetails.Name = Environment.MachineName;
        //    publisherSubscriberQueue.QueueDetails.ReqAcknowlegement = false;
        //    publisherSubscriberQueue.QueueCriteriaFlag = QueueCriteriaFlag;
        //    publisherSubscriberQueue.RoutingKey = routingKey;// "automation.success";
        //    publisherSubscriberQueue.QueueDetails.Arguments = new List<QArguments>();
        //    return publisherSubscriberQueue;
        //}
        //public RabbitMQQueue RabbitMQ_QueuePublishSubscribe(RabbitMQQueue publisherSubscriberQueue, string exchangeName, string exchangeType, string queueName, string routingKey, string QueueCriteriaFlag)
        //{           
        //    publisherSubscriberQueue.BindExchangeToQueue = true;
        //    ExchangeDetails ed = new ExchangeDetails();
        //    ed.AutoDelete = false;
        //    ed.Durable = true;
        //    ed.IsInternal = false;
        //    ed.Name = exchangeName;// "robot.x.automation";
        //    ed.Type = exchangeType;// "topic";
        //    ed.Arguments = new List<ExchangeArgs>();
        //    ed.Arguments.Add(new ExchangeArgs());
        //    publisherSubscriberQueue.Exchange = ed;
        //    publisherSubscriberQueue.QueueDetails = new QDetails();
        //    publisherSubscriberQueue.QueueDetails.AutoDelete = false;
        //    publisherSubscriberQueue.QueueDetails.Durable = true;
        //    publisherSubscriberQueue.QueueDetails.Name = queueName;// "robot.q.success";
        //    publisherSubscriberQueue.QueueDetails.ReqAcknowlegement = false;
        //    publisherSubscriberQueue.QueueCriteriaFlag = QueueCriteriaFlag;
        //    publisherSubscriberQueue.RoutingKey = routingKey;// "automation.success";
        //    publisherSubscriberQueue.QueueDetails.Arguments = new List<QArguments>();
        //    return publisherSubscriberQueue;

        //}

        //public bool AutomationRequest(RequestInput _requestInput, string routingKey)
        //{
        //    string strExchangeTopic = ConfigurationManager.AppSettings["ExchangeTopicName"];

        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {

        //        var props = channel.CreateBasicProperties();
        //        props.DeliveryMode = 1; //non persistent
        //        props.Headers = new Dictionary<string, object>();
        //       // props.Headers["source_message_id"] = "MsgReqNo_" + DateTime.Now.Ticks.ToString()
        //        var message = XMLHelper.Serialize(_requestInput);
        //        var body = Encoding.UTF8.GetBytes(message);
        //        channel.BasicPublish(exchange: strExchangeTopic,
        //                             routingKey: routingKey,
        //                             basicProperties: props,
        //                             body: body);
        //    }
        //    return true; //acknowledgement
        //}
    }
}
