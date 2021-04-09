using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using RabbitMQ.Client;

namespace WebApi.Controllers
{

    public class ClientModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string folderName { get; set; }

        public string fileName { get; set; }
        public string inputArgs { get; set; }

    }
    [RoutePrefix("api/executor")]
    public class ExecutorController : ApiController
    {
        public static Object Port;

        public static string Connect(String server, String message)
        {
            try
            {
                #region Old Code
                //Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SocketPort"]);
                //lock (Port)
                //{

                //    Int32 port = (Int32)Port;
                //    TcpClient client = new TcpClient(server, port);
                //    NetworkStream stream = client.GetStream();
                //    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                //    stream.Write(data, 0, data.Length);
                //    Console.WriteLine("Sent: {0}", message);
                //    data = new Byte[256];
                //    String response = String.Empty;
                //    int bytes = stream.Read(data, 0, data.Length);
                //    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //    Console.WriteLine("Received: {0}", response);


                //        Thread.Sleep(2000);
                //    stream.Close();
                //    client.Close();
                //    return response;
                //} 
                #endregion

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
                return "Error while connecting executor : " + e.Message;
            }

        }

        [Route("runbot")]
        [HttpPost]
        public IHttpActionResult PostRunBot([FromBody]ClientModel clientModel)
        {
            try
            {

                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "se",
                    Password = "se"

                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "frontoffice_task_queue",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);


                    var body = Encoding.UTF8.GetBytes(clientModel.username + Environment.NewLine + clientModel.password + Environment.NewLine + clientModel.folderName + Environment.NewLine + clientModel.fileName + Environment.NewLine + clientModel.inputArgs + "FrontOffice");
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                 routingKey: "frontoffice_task_queue",
                                 basicProperties: properties,
                                 body: body);

                }
                return Ok("Bot Is Successful");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message + "     " + e.StackTrace);
                return Ok("Bot failed!!");
            }
           


        }

        [Route("test")]
        [HttpGet]
        public IHttpActionResult GetTest()
        {
            return Ok("Server Up and Running");
        }
    }
}
