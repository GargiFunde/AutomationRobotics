// <copyright file=Notifications company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQListenerRobot
{
    //class Notifications
    //{
    //    string RobotName = null;
    //    string RoutingKey =null;
    //    ConnectionFactory factory = null;
    //    public Notifications()
    //    {
    //        string RobotName = ConfigurationManager.AppSettings["RobotName"];
    //        string RoutingKey = ConfigurationManager.AppSettings["RoutingKey"];
    //        factory = new ConnectionFactory() { HostName = "localhost" };
    //    }
    //    public void SendNotifications(string message )
    //    {
    //        factory.UserName = "guest";
    //        factory.Password = "guest";
    //        using (var connection = factory.CreateConnection())
    //        using (var channel = connection.CreateModel())
    //        {
    //            var routingKey = RoutingKey;             
    //            var body = Encoding.UTF8.GetBytes(message);
    //            channel.BasicPublish(exchange: "amq.topic",
    //                                    routingKey: routingKey,
    //                                    basicProperties: null,
    //                                    body: body);
    //            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
               
    //        }
    //    }
    //}
}
