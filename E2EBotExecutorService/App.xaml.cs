using BotDesignCommon.Helpers;
using CommonLibrary;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace E2EBotExecutorService
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


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ProcessName = e.Args[0];

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
                _wfApp = new WorkflowApplication(activityExecute);
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
                if (Logger.Log.Logger.DatatableLog != null)
                {
                    Logger.Log.Logger.DatatableLog.Clear();
                }
                _wfApp.Run();
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
                    Environment.Exit(0);
                }
                if (stopFlag == true)
                {
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.ToString());
            }
        }
    }
}