using CommonLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.RabbitMQ
{
    [Designer(typeof(ActivityDesigner2))]
    public class QueueExecute : NativeActivity
    {

        [Category("Input Parameter")]
        [Description("Enter Queue Name")]
        [DisplayName("Queue Name")]
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        [Category("Input parameter")]
        [Description("Enter the username")]
        [DisplayName("User Name")]
        [RequiredArgument]
        public InArgument<string> MQUserName { get; set; }

        [Category("Input parameter")]
        [Description("Enter the password")]
        [DisplayName("Password")]
        [RequiredArgument]
        public InArgument<string> MQPassword { get; set; }

        [Category("Input parameter")]
        [Description("Address where RabbitMQ is Hosted")]
        [DisplayName("Address")]
        [RequiredArgument]
        public InArgument<string> MQAddress { get; set; }

        [Category("Input parameter")]
        [Description("Port Address")]
        [DisplayName("Port")]
        [RequiredArgument]
        public InArgument<int> MQPort { get; set; }



        [Category("Input parameter")]
        [Description("Process path")]
        [DisplayName("Process")]
        [RequiredArgument]
        public InArgument<string> ProcessPath { get; set; }




        protected override void Execute(NativeActivityContext context)
        {

            //RequestInput rqInput = null;
            try
            {
                string queueName = QueueName.Get(context);
                string userName = MQUserName.Get(context);
                string password = MQPassword.Get(context);
                string address = MQAddress.Get(context);
                int port = MQPort.Get(context);
                string workflowPath = ProcessPath.Get(context);

                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = userName;
                factory.Password = password;
                factory.HostName = address;
                factory.Port = port;

                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {

                        bool noAck = false;
                        BasicGetResult result = channel.BasicGet(queueName, noAck);
                        if (result == null)
                        {

                            Logger.Log.Logger.LogData("The Execution is aborted : Queue is empty", Logger.LogLevel.Error);
                            context.Abort();

                        }
                        else
                        {
                            IBasicProperties props = result.BasicProperties;
                            var message = Encoding.UTF8.GetString(result.Body);

                          


                            RequestInput item = JsonConvert.DeserializeObject<RequestInput>(message);
                            System.Activities.Activity act = ActivityXamlServices.Load(workflowPath);
                            var output = WorkflowInvoker.Invoke(act, item.Input);
                            

                        }

                    }




                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("The Execution Error" + ex.Message, Logger.LogLevel.Error);

            }




        }




    }
}
