// <copyright file=IMessageAdapter company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLibrary
{
     public interface  IMessageAdapter
     {
         bool IsConnected { get; set; }

         bool ISSQConnectionExists { get; set; }

         event EventHandler<RequestEventArgs> MessageReceivedEvent;
         event EventHandler<NotificationEventArgs> NotificationReceivedEvent;
         Dictionary<string, string> Connect();

         bool PublishToChannel(string queueType, string message, RequestProperties messageProperties, Dictionary<string, object> messageHeader);

         bool PublishNotifications(string routingKey, string message);
         bool SubscribeToChannels(Dictionary<string, string> channelDetails);

         bool SubscribeToNotifications();
         bool UnSubscribeFromChannels();

         bool SendAcknowledge(object ackTag);
     }
    public enum PublishQueueTypes
    {
        AutomationFailure,
        AutomationSuccess,
        Transaction,
        HealthAgent,
        HealthRobot,
        Authenticate,
        ReplyTo
    }
}
