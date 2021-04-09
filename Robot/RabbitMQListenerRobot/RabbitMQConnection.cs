using System.Collections.Generic;

namespace RabbitMQListenerRobot
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RabbitMQConnection
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HostName { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int PortNumber { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string UserName { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Password { get; set; }
        [System.Xml.Serialization.XmlElementAttribute()]
        public List<RabbitMQQueue> PublisherQueueDetails { get; set; }
        [System.Xml.Serialization.XmlElementAttribute()]
        public List<RabbitMQQueue> SubscriberQueueDetails { get; set; }

        public RabbitMQConnection()
        {
            ////****** Code to generate xml *************

            RabbitMQConnection rq = this;
            //rq.HostName = "a.b.c.d";
            //rq.PortNumber = "5672";
            //rq.UserName = "UserName";
            //rq.Password = "Password";
            rq.PublisherQueueDetails = new List<RabbitMQQueue>();
            rq.SubscriberQueueDetails = new List<RabbitMQQueue>();

            //RabbitMQQueue publisherQueue = new RabbitMQQueue();
            //rq.PublisherQueueDetails.Add(publisherQueue);
            //publisherQueue.BindExchangeToQueue = true;
            //publisherQueue.Exchange = new ExchangeDetails();
            //ExchangeDetails ed = new ExchangeDetails();
            //ed.AutoDelete = false;
            //ed.Durable = true;
            //ed.IsInternal = false;
            //ed.Name = "robot.x.automation";
            //ed.Type = "topic";
            //ed.Arguments = new List<ExchangeArgs>();
            //ed.Arguments.Add(new ExchangeArgs());

            //publisherQueue.QueueDetails = new QDetails();
            //publisherQueue.QueueDetails.AutoDelete = false;
            //publisherQueue.QueueDetails.Durable = true;
            //publisherQueue.QueueDetails.Name = "robot.q.success";
            //publisherQueue.QueueDetails.ReqAcknowlegement = false;
            //publisherQueue.QueueCriteriaFlag = "AutomationSuccess";
            //publisherQueue.RoutingKey = "automation.success";


            //RabbitMQQueue publisherFailQueue = new RabbitMQQueue();
            //rq.PublisherQueueDetails.Add(publisherFailQueue);

            //publisherFailQueue.BindExchangeToQueue = true;
            //publisherFailQueue.Exchange = new ExchangeDetails();
            //ExchangeDetails edf = new ExchangeDetails();
            //edf.AutoDelete = false;
            //edf.Durable = true;
            //edf.IsInternal = false;
            //edf.Name = "robot.x.automation";
            //edf.Type = "topic";
            //edf.Arguments = new List<ExchangeArgs>();
            //edf.Arguments.Add(new ExchangeArgs());

            //publisherFailQueue.QueueDetails = new QDetails();
            //publisherFailQueue.QueueDetails.AutoDelete = false;
            //publisherFailQueue.QueueDetails.Durable = true;
            //publisherFailQueue.QueueDetails.Name = "robot.q.failure";
            //publisherFailQueue.QueueDetails.ReqAcknowlegement = false;
            //publisherFailQueue.QueueCriteriaFlag = "AutomationFailure";
            //publisherFailQueue.RoutingKey = "automation.failure";


            //RabbitMQQueue publisherHealthQueue = new RabbitMQQueue();
            //rq.PublisherQueueDetails.Add(publisherHealthQueue);

            //publisherHealthQueue.BindExchangeToQueue = true;
            //publisherHealthQueue.Exchange = new ExchangeDetails();
            //ExchangeDetails edh = new ExchangeDetails();
            //edh.AutoDelete = false;
            //edh.Durable = true;
            //edh.IsInternal = false;
            //edh.Name = "robot.x.health";
            //edh.Type = "fanout";
            //edh.Arguments = new List<ExchangeArgs>();
            //edh.Arguments.Add(new ExchangeArgs());

            //publisherHealthQueue.QueueDetails = new QDetails();
            //publisherHealthQueue.QueueDetails.AutoDelete = false;
            //publisherHealthQueue.QueueDetails.Durable = true;
            //publisherHealthQueue.QueueDetails.Name = "robot.q.health";
            //publisherHealthQueue.QueueDetails.ReqAcknowlegement = false;
            ////publisherHealthQueue.QueueCriteriaFlag = "AutomationFailure";
            //publisherHealthQueue.RoutingKey = "automation.health";


            //RabbitMQQueue subscriberQueue = new RabbitMQQueue();
            //rq.SubscriberQueueDetails.Add(subscriberQueue);
            //subscriberQueue.BindExchangeToQueue = true;
            //subscriberQueue.Exchange = new ExchangeDetails();
            //ExchangeDetails eds = new ExchangeDetails();
            //eds.AutoDelete = false;
            //eds.Durable = true;
            //eds.IsInternal = false;
            //eds.Name = "robot.x.fallout";
            //eds.Type = "topic";
            //eds.Arguments = new List<ExchangeArgs>();
            //eds.Arguments.Add(new ExchangeArgs());

            //subscriberQueue.QueueDetails = new QDetails();
            //subscriberQueue.QueueDetails.AutoDelete = false;
            //subscriberQueue.QueueDetails.Durable = true;
            //subscriberQueue.QueueDetails.Name = "robot.q.fallout";
            //subscriberQueue.QueueDetails.ReqAcknowlegement = false;
            //subscriberQueue.QueueCriteriaFlag = "AutomationFallout";
            //subscriberQueue.RoutingKey = "automation.fallout";

            //XmlDocument xml = null;
            //XMLHelper.Serialize(rq, ref xml);
            //xml.Save(@"E:\Work\Assist\Live\AutomationComponents\QueueConfigurations.xml");


            ////**********End - Code to generate xml **************

        }

        //public void Initialize(string strQueueConfigurations = @"E:\Work\BOTDesignerMaster\Build\QueueConfigurations.xml")
        //{
            //if(File.Exists(strQueueConfigurations))
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.Load(strQueueConfigurations);
            //    RabbitMQConnection rabbitMQConnection = (RabbitMQConnection)XMLHelper.Deserialize(doc, typeof(RabbitMQConnection));
            //    this.HostName = rabbitMQConnection.HostName;
            //    this.PortNumber = rabbitMQConnection.PortNumber;
            //    this.UserName = rabbitMQConnection.UserName;
            //    this.Password = rabbitMQConnection.Password;
            //    this.PublisherQueueDetails = rabbitMQConnection.PublisherQueueDetails;
            //    this.SubscriberQueueDetails = rabbitMQConnection.SubscriberQueueDetails;
            //}
            //return;
        //}
    }
}
