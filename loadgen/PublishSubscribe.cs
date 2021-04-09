// <copyright file=PublishSubscribe company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:03:08</date>
// <summary></summary>

using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using CommonLibrary;
using System.Net.Mail;
using RobotLibrary;
using CommonLibrary.Entities;
using Newtonsoft.Json;

namespace DataConnectorToGenerateRequest
{
    public class PublishSubscribe
    {
        public ConnectionFactory factory;
        public IConnection connection = null;
        public IModel channel = null;
        EventingBasicConsumer consumer = null;
        EventingBasicConsumer consumerFail = null;
        string requestid = string.Empty;
        public void AttachListener()
        {
            string SuccessQueueName = System.Configuration.ConfigurationManager.AppSettings["SuccessQueueName"];
            string FailureQueueName = System.Configuration.ConfigurationManager.AppSettings["FailureQueueName"];


            connection = factory.CreateConnection();
            channel = connection.CreateModel();
          
            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: SuccessQueueName,noAck: true,consumer: consumer);
            consumer.Received += consumersuccess_Received;

                

                //consumerFail = new EventingBasicConsumer(channel);
                //consumerFail.Received += consumerfail_Received;
                //channel.BasicConsume(queue: FailureQueueName,
                //                 noAck: true,
                //                 consumer: consumer);
            
          
        }

        void consumersuccess_Received(object sender, BasicDeliverEventArgs e)
        {
             requestid = string.Empty;

            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            var routingKey = e.RoutingKey;
            Console.WriteLine(" [x] Received '{0}':'{1}'",
                              routingKey,
                              message);
            ResponseOutput responseOutput = new ResponseOutput();
            //responseOutput =(ResponseOutput) XMLHelper.Deserialize(message, typeof(ResponseOutput));

            responseOutput = JsonConvert.DeserializeObject<ResponseOutput>(message);

            requestid = responseOutput.requestInput.RequestNumber;
            ProcessSuccessFailQueue(responseOutput, message,"Success");
        }
        void consumerfail_Received(object sender, BasicDeliverEventArgs e)
        {
            requestid = string.Empty;
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            ResponseOutput responseOutput = new ResponseOutput();
            //responseOutput = (ResponseOutput)XMLHelper.Deserialize(message, typeof(ResponseOutput));

            responseOutput = JsonConvert.DeserializeObject<ResponseOutput>(message);
            requestid = responseOutput.requestInput.RequestNumber;
            ProcessSuccessFailQueue(responseOutput, message,"Fail");
        }
        public void ProcessSuccessFailQueue(ResponseOutput responseOutput, string message, string result)
        {
            string RoutingKey = string.Empty;
                string messageBody = string.Empty;
            if (result == "Success")
            {
                RoutingKey = System.Configuration.ConfigurationManager.AppSettings["RoutingKeyUnprocessedSuccess"];
                List<CViewFields> Results = responseOutput.cviewResultsAll;
               
                foreach (CViewFields item in Results)
                {
                    messageBody = messageBody + item.CViewName + ":" + item.CViewValue + "\r\n";
                }
            }
            else
            {
                RoutingKey = System.Configuration.ConfigurationManager.AppSettings["RoutingKeyUnprocessedFail"];
                messageBody = "Automation Request failed \r\n" + message;
            }
             if (!string.IsNullOrEmpty(requestid))
            {
                if (LoadGenerator.reqData.ContainsKey(requestid))
                {
                    SendGmailMail(messageBody);
                    LoadGenerator.reqData.Remove(requestid);
                }
                else
                {
                    UnprocessedSuccessFailRequest(messageBody, RoutingKey);    
                }
             }
             else
             {
                UnprocessedSuccessFailRequest(messageBody, RoutingKey);            
             }
        }
        public bool UnprocessedSuccessFailRequest(string message, string RoutingKeyUnprocessed)
        {
            string strExchangeTopic = System.Configuration.ConfigurationManager.AppSettings["ExchangeTopicName"];

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = 2; //persistent


                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: strExchangeTopic,
                                         routingKey: RoutingKeyUnprocessed,
                                         basicProperties: props,
                                         body: body);
                }
            }
            return true; //acknowledgement
        }

        private void SendGmailMail(string message)
        {
           
                    Requestor reqtor = LoadGenerator.reqData[requestid];
                    MailMessage msg = new MailMessage(reqtor.To, reqtor.From); //reverse
                    msg.Subject = reqtor.Subject;

                   // string BodyText = string.Empty;
                    //int FileRecords = 0;
                    //string filePath = Path.Combine(Environment.CurrentDirectory, Log_file_Name + "_" + DateTime.Now.ToString("dd-MM-yyy") + ".log");
                    //if (File.Exists(filePath))
                    //{
                    //    System.Net.Mail.Attachment atchItem = new System.Net.Mail.Attachment(filePath);
                    //    FileRecords = File.ReadAllLines(filePath).Length;
                    //    msg.Attachments.Add(atchItem);
                    //}

                    msg.Body =  "this is an auto generated message by robot!!! \r\n";
                    msg.Body = msg.Body + message;

                    SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    smtp.UseDefaultCredentials = false;
                    System.Net.NetworkCredential Credentials = new System.Net.NetworkCredential("e2erobotic", "dxlufhxgjtfnsymd");
                    smtp.Credentials = Credentials;
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                    
             
        }
    }
}
