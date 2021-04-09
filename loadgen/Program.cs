//using LoggingUtility;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Windows.Forms;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Mail;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Threading;
//using System.Threading.Tasks;

namespace DataConnectorToGenerateRequest
{
    class Program
    {
        static ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        static void Main(string[] args)
        {
            try
            {
                string RequestMode = System.Configuration.ConfigurationManager.AppSettings["RequestMode"];

                PublishSubscribe publishSubscribe = new PublishSubscribe();
                ServiceReference1.BOTServiceClient ser = new ServiceReference1.BOTServiceClient();
                publishSubscribe.factory = factory;
                ILoadGenerator loadGen1 = new LoadGenerator(publishSubscribe);
                //loadGen1.publishSubscribe = publishSubscribe;


                string InputTenantName = Microsoft.VisualBasic.Interaction.InputBox("Tenant Name:", "Tenant", "");
                string InputProcessName = Microsoft.VisualBasic.Interaction.InputBox("Process Name:", "Process", "");
                string InputGroupName = Microsoft.VisualBasic.Interaction.InputBox("Group Name:", "Group", "");

               


                if (RequestMode == "load")
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        //Thread.Sleep(100);
                        int result = ser.AddProcessQueueMapping(InputProcessName, InputGroupName, InputTenantName);
                        loadGen1.ReadClassData(InputTenantName, InputProcessName, InputGroupName);
                    }
                }
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while uploading Data: "+ex.Message);
                //Logger.LogMessage(LogLevel.Error, "Main:Exited", "Failed in Main" + ex.Message, System.Configuration.ConfigurationManager.AppSettings["Log_file_Name"]);

            }
        }

        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}
    }  

  
}
