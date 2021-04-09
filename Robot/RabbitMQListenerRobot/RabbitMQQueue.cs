// <copyright file=RabbitMQQueue company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQListenerRobot
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RabbitMQQueue
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool BindExchangeToQueue { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChannelConsumer { get; set; }
        [System.Xml.Serialization.XmlElementAttribute()]
        public ExchangeDetails Exchange { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PayLoadMessage { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string QueueCriteriaFlag { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RoutingKey { get; set; }
        [System.Xml.Serialization.XmlElementAttribute()]
        public QDetails QueueDetails { get; set; }
        public RabbitMQQueue()
        { }
        public RabbitMQQueue(bool _bindExchangeToQueue, string _channelConsumer, ExchangeDetails _exchange, string _payLoadMessage, string _queueCriteriaFlag, string _routingKey, QDetails _queueDetails)
        { 
            BindExchangeToQueue = _bindExchangeToQueue;
            ChannelConsumer = _channelConsumer;
            Exchange = _exchange;
            PayLoadMessage = _payLoadMessage;
            QueueCriteriaFlag = _queueCriteriaFlag;
            RoutingKey = _routingKey;
            QueueDetails = _queueDetails;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ExchangeDetails
    {
         [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
         [System.Xml.Serialization.XmlAttributeAttribute()]
         public string Type { get; set; }
         [System.Xml.Serialization.XmlAttributeAttribute()]
         public bool AutoDelete { get; set; }
         [System.Xml.Serialization.XmlAttributeAttribute()]
         public bool Durable { get; set; }
         [System.Xml.Serialization.XmlAttributeAttribute()]
         public bool IsInternal { get; set; }
         [System.Xml.Serialization.XmlElementAttribute()]
        public List<ExchangeArgs> Arguments { get; set; }
        public ExchangeDetails()
        {

        }
        public ExchangeDetails(string _name, string _type, bool _autoDelete, bool _durable, bool _isInternal, List<ExchangeArgs> _arguments)
        {
            Name = _name;
            Type = _type;
            AutoDelete = _autoDelete;
            Durable = _durable;
            IsInternal = _isInternal;
            Arguments = _arguments;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ExchangeArgs
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ArgKey { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ArgValue { get; set; }

        public ExchangeArgs()
        {

        }
        public ExchangeArgs(string _key, string _value)
        {
            ArgKey = _key;
            ArgValue = _value;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class QDetails
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool AutoDelete { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Durable { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ReqAcknowlegement { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DLX { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DLXRoutingKey { get; set; }
         [System.Xml.Serialization.XmlElementAttribute()]
        public List<QArguments>Arguments { get; set; }
        public QDetails()
        {

        }
        public QDetails(string _name, bool _autoDelete, bool _durable, bool _reqAcknowlegement, string _dlx, string _dlxRoutingKey, List<QArguments> _arguments)
        {
            Name = _name;
            AutoDelete = _autoDelete;
            Durable = _durable;
            ReqAcknowlegement = _reqAcknowlegement;
            DLX = _dlx;
            DLXRoutingKey = _dlxRoutingKey;
            Arguments = _arguments;
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class QArguments
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ArgKey { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ArgValue { get; set; }
        public QArguments()
        {
        }
        public QArguments(string _key, string _value)
        {
            ArgKey = _key;
            ArgValue = _value;
        }
    }
}
