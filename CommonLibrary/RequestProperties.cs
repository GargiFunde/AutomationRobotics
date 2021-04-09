// <copyright file=RequestProperties company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
   public class RequestProperties
    {
        public long TimeStamp { get; set; }
        public string SourceApplicationId { get; set; }
        public string MessageId { get; set; }
        public string ContentType { get; set; }
        public string ContentEncoding { get; set; }
        public string ReplyTo { get; set; }
        public string CorrelationId { get; set; }
        public string Expiration { get; set; }
    }
  

   public class RequestEventArgs:EventArgs 
   {
       public string Queue { get; set; }
       public object DeliveryTag { get; set; }
       public string Message { get; set; }
       public RequestProperties RequestMessageProperties { get; set; }
       public Dictionary<string, object> MessageHeader { get; set; }
       public RequestEventArgs(string _queue, string _message, RequestProperties _requestMessageProperties, object _deliveryTag, Dictionary<string, object> _messageHeader)
       {
           Queue = _queue;
           DeliveryTag = _deliveryTag;
           Message = _message;
           RequestMessageProperties = _requestMessageProperties;
           MessageHeader = _messageHeader;
       }
   }
   public class NotificationEventArgs : EventArgs
   {
       public string Message { get; set; }
       public NotificationEventArgs(string _message)
       {
           Message = _message;
       }
   }
}
