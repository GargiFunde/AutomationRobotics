using BotDesignCommon.Helpers;
using CommonLibrary;
using Newtonsoft.Json;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FrontOfficeBotExecutorService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        public string ProcessName { get; set; }
        Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
        private WorkflowApplication _wfApp;
        private bool stopFlag = false;
        public string ProcessInput { get; set; }   //added for FrontOfficeBot

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //T
            try
            {
                if (Connection == null || Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
                {
                    //  MessageBox.Show("Connect Async");
                    connectAsync();
                    //Thread.Sleep(120000);
                    //  MessageBox.Show("Connect Async Completed");
                }

                ProcessName = e.Args[0];
                //   ProcessName = @"C:\E2E\ProcessTest\Demo2\NewReqWeb.xaml";
                if (e.Args[1] != null)
                {
                    ProcessInput = e.Args[1];  //added for FrontOfficeBot
                                               //    ProcessInput = "{%in_RequisitionType%:%CPU%,%in_Services%:%IT&Services%,%in_ShortDescription%:%Need&CPU%}";  

                    ProcessInput = ProcessInput.Replace('&', ' ');
                    ProcessInput = ProcessInput.Replace('%', '"');

                   // MessageBox.Show(e.Args[1]);
                }

                if (File.Exists(ProcessName))
                {


                    if (WorkflowDictionary.ContainsKey(ProcessName))
                    {
                        CustomWfDesigner.Instance = WorkflowDictionary[ProcessName];
                    }
                    else
                    {
                        BotDesignCommon.Helpers.CustomWfDesigner.NewInstance(ProcessName);
                        WorkflowDictionary.Add(ProcessName, CustomWfDesigner.Instance);
                    }
                    SelectHelper._currentworkflowfile = ProcessName;
                    SelectHelper.ProjectLocation = System.IO.Path.GetDirectoryName(ProcessName);

                    CustomWfDesigner.Instance.Flush();
                    MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                    DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
                    //added for input args for FrontOfficeBot
                    //  MessageBox.Show(ProcessInput);
                    Dictionary<string, object> arguments = new Dictionary<string, object>();
                    arguments = JsonConvert.DeserializeObject<Dictionary<string, object>>(ProcessInput);
                    //  MessageBox.Show("Deserialization completed");
                    _wfApp = new WorkflowApplication(activityExecute, arguments);
                    //added for input args for FrontOfficeBot
                    if (SelectHelper.CurrentRuntimeApplicationHelper == null)
                    {
                        SelectHelper.CurrentRuntimeApplicationHelper = new RuntimeApplicationHelper();
                    }

                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects == null)
                    {
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects = new Dictionary<string, object>(); //to clean stored runtime objects from previous run
                    }
                    else
                    {
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Clear();
                    }
                    _wfApp.Completed = WfExecutionCompleted;
                    //if (Logger.Log.Logger.DatatableLog != null)
                    //{
                    //    Logger.Log.Logger.DatatableLog.Clear();
                    //}
                    _wfApp.Run();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
        {
            try
            {
                GC.Collect(0);
                GC.Collect(1);
                if (ev.CompletionState == ActivityInstanceState.Closed)
                {
                    //changes made for frontofficebot
                    //foreach (KeyValuePair<string, object> entry in ev.Outputs)

                    //    MessageBox.Show(entry.Value.ToString());
                    //    MessageBox.Show("Wf Completed");

                    //added for FrontOfficeBot
                    string JsonOutput = JsonConvert.SerializeObject(ev.Outputs);

                    if (Connection == null || Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
                    {
                        //  MessageBox.Show("Connect Async");
                        connectAsync();
                        //  MessageBox.Show("Connect Async Completed");
                    }

                    //string json = JsonConvert.SerializeObject(sr);
                    //     string notificationMessage = sr.ReadToEnd();
                    //sendMessageToClient(sr.ReadToEnd());
                    //  MessageBox.Show("Send Msg to SignalR"+JsonOutput);
                    sendMessageToClient(JsonOutput);
                    //  MessageBox.Show("Send Msg to SignalR completed");

                    //added for FrontOfficeBot
                    Environment.Exit(0);


                }
                if (stopFlag == true)
                {
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exc - " + ex);
                //  MessageBox.Show(ex.ToString());
            }
        }

        //added for FrontOfficeBot
        #region SignalR Code writeen on 05-Dec-2019
        //Method connectAsync Related to SignalR
        private Microsoft.AspNet.SignalR.Client.IHubProxy HubProxy { get; set; }
        // const string ServerURI = "http://localhost:8081/E2EBots/signalr";
        //const string ServerURI = "https://localhost:44390";
        // const string ServerURI = "http://localhost:3455/signalr";  //Notifications will work for BotDashBoard Page only on 8081 Port.
        // const string ServerURI = "https://localhost:44333/signalr";
        string ServerURI = ConfigurationManager.AppSettings["SignalRConnection"];
        //string ServerUrlPath = ConfigurationManager.AppSettings["SignalRConnection"];


        private Microsoft.AspNet.SignalR.Client.HubConnection Connection { get; set; }

        //Call from Constructor
        private async void connectAsync()
        {
            //  MessageBox.Show("New conn");
            Connection = new Microsoft.AspNet.SignalR.Client.HubConnection(ServerURI);
            //  MessageBox.Show("New conn created");
            // Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //  MessageBox.Show("New hub proxy");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread

            Connection.TraceLevel = Microsoft.AspNet.SignalR.Client.TraceLevels.All;
            Connection.TraceWriter = Console.Out;

            Console.WriteLine("\n SIGNAL R RMQ TESTING : ");
            try
            {
                await Connection.Start();


                //MessageBox.Show("Connection made");

                // Console.WriteLine("\n COMPLETED AWAIT SIGNAL R RMQ TESTING : ");
            }
            //catch (HttpRequestException)
            catch (Exception ex)
            {
                MessageBox.Show("Exc - " + ex);
                //   WCFServiceReference.insertLog("Error: RMQueue Details SignalR", "Error in RMQueue Details SignalR: " + ex.Message, groupId, tenantId);
            }
        }

        public void sendMessageToClient(string sr)  //Receive from Windows Service
        {
            try
            {
                //client.Value.BroadcastToClient(eventData);
                //MessageBox.Show((Connection != null).ToString());
                //MessageBox.Show((Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected).ToString());
                //MessageBox.Show((Connection.State).ToString());

                if (Connection != null && Connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    // HubProxy.Invoke("Send", eventData.ClientName, eventData.EventMessage);
                    //MessageBox.Show("Connection successful");
                    HubProxy.Invoke("Send", sr).Wait();

                    // Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Conn state " + Connection.State);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exc - " + ex);
                //  WCFServiceReference.insertLog("Error: RMQueue Details Sending Message to Client", "Error in RMQueue Details Sending Message to Client: " + ex.Message, groupId, tenantId);
                //  Console.WriteLine("Connection Exception : " + ex.Message);
            }
        }
        #endregion
        //added for FrontOfficeBot
    }
}
