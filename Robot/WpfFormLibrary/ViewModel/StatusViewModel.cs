// <copyright file=StatusViewModel company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CommonLibrary;
using System.Activities.Presentation;
using System.Activities;
using System.IO;
using System.Activities.XamlIntegration;
using System.Configuration;
using TCP.Client;

namespace WpfFormLibrary.ViewModel
{
    public class StatusViewModel : ADR_Library.ViewModel.ViewModelBase
    {

        TCP.Client.Client client = null;
        RMQueue rmqueue = null;
        public StatusViewModel(TCP.Client.Client clientinner)
        {
            ThreadInvoker.Instance.InitDispatcher();
            StatusFlags = new System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, string>>();
            StatusFlags = new System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string,string>>();
            client = new Client();
            client.TextMessageReceived += client_TextMessageReceived;
            clientinner = client;
            client.Connect("localhost", 8888);
           
            rmqueue = new RMQueue();
            rmqueue.client = client;
            rmqueue.ExecuteAction("processstart");
           
            //StartRobotWorkflow();
        }
     
        void client_TextMessageReceived(Client sender, string message)
        {
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "StatusViewModel.cs=>client_TextMessageReceived message: " + message + "\n");
            rmqueue.ExecuteAction(message);
            //Status("Message received: " + message);
            //System.Windows.MessageBox.Show(message);
        }
      
        private System.Windows.Media.ImageSource _icon;

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private bool _isRunning = false;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string,string>> _statusFlags;

        public System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, string>> StatusFlags
        {
            get
            {
                return _statusFlags;
            }
            set
            {
                _statusFlags = value;
                OnPropertyChanged("StatusFlags");
            }
        }

        public void SetStatusFlags(List<KeyValuePair<string, string>> flags)
        {
           
        }
        Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
       // private CustomTrackingParticipant _executionLog;
        private WorkflowApplication _wfApp;

        //private void StartRobotWorkflow()
        //{         
        //        try
        //        {

                    //string filename = ConfigurationManager.AppSettings["TriggerWorkflow"];
                    //ThreadInvoker.Instance.RunByUiThread(() =>
                    //{
                        

                    //    if (WorkflowDictionary.ContainsKey(filename))
                    //    {
                    //        CustomWfDesigner.Instance = WorkflowDictionary[filename];
                    //    }
                    //    else
                    //    {
                    //        CustomWfDesigner.NewInstance(filename);
                    //        WorkflowDictionary.Add(filename, CustomWfDesigner.Instance);
                    //    }
                    //    CustomWfDesigner.Instance.Flush();
                    //    MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                    //    DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;

                    //    _wfApp = new WorkflowApplication(activityExecute);
                                             
                    //    _wfApp.Completed = WfExecutionCompleted;
                    //    //execute 
                    //    _wfApp.Run();
                    //});
        //        }
        //        catch (Exception ex)
        //        {
        //            //
        //        }
           
        //}
        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
        {
            try
            {
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }
    }
}
