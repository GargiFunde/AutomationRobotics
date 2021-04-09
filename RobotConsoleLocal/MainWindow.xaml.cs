// <copyright file=MainWindow.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:58</date>
// <summary></summary>

using CommonLibrary;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Logger;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BotDesignCommon.Helpers;

namespace RobotConsoleLocal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //, INotifyPropertyChanged
    {
        int iLog = 0;
        bool stopflag = false;
        //private DataView _datatableLog { get; set; }
        //public DataView DatatableLog
        //{
        //    get
        //    {
        //        if (_datatableLog != null)
        //            return _datatableLog;
        //        else
        //            return null;
        //    }
        //    set { _datatableLog = value; NotifyPropertyChanged("DatatableLog"); }
        //}
        private ObservableCollection<DataRow> logDataToGrid = new ObservableCollection<DataRow>();
        public ObservableCollection<DataRow> LogDataToGrid { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();
            ThreadInvoker.Instance.InitDispatcher();
       
            if (iLog > 0)
            {
                for (int i = 0; i < iLog; i++)
                {
                    Log.LogHandler -= Log_LogHandler;
                }
            }
            iLog = 0;
            Log.LogHandler += Log_LogHandler;
            iLog = iLog + 1;
            log4net.Config.XmlConfigurator.Configure();
            string ConsoleMode = ConfigurationManager.AppSettings["ConsoleMode"];
            if (ConsoleMode.ToLower().Trim() == "ui")
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                stopflag = true;
                this.Visibility = Visibility.Hidden;
                string strProcessName = ConfigurationManager.AppSettings["ExecuteProcessName"];
                StartRobot(strProcessName);
            }
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //  DatatableLog = Log.Logger.DatatableLog.DefaultView;
            dgInfoErrorWarnings.DataContext = Log.Logger.DatatableLog.DefaultView;
            dgInfoErrorWarnings.ItemsSource = Log.Logger.DatatableLog.DefaultView;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            stopflag = false;
            Mouse.OverrideCursor = Cursors.Wait;
            //string strProcessName = cmbProcessList.SelectedItem.ToString();
            string strProcessName = ConfigurationManager.AppSettings["ExecuteProcessName"];
            StartRobot( strProcessName);
            Mouse.OverrideCursor = Cursors.Arrow;
           
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
         
        }
        Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
        private WorkflowApplication _wfApp;
        private void StartRobot(string ProcessName)
        {
            try
            {
                
                if (File.Exists(ProcessName))
                {
                    //ThreadInvoker.Instance.RunByUiThread(() =>
                    //{

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
                    //});
                }
            }
            catch (Exception ex)
            {
                //
            }
        }
        /// <summary>
        /// Retrieve Workflow Execution Logs and Workflow Execution Outputs
        /// </summary>
        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
        {
            try
            {
                GC.Collect(0);
                GC.Collect(1);
                if (ev.CompletionState == ActivityInstanceState.Closed)
                {
                    
                }
                if (stopflag == true)
                {
                    Environment.Exit(0);
                }        
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }
        private void btnInformation_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = "(LogLevel = 'INFO')";
            //dgInfoErrorWarnings.ItemsSource = Logger.Log.DatatableLog.DefaultView;
        }
        private void btnWarning_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = "(LogLevel = 'WARN')";
            // dgInfoErrorWarnings.ItemsSource = Logger.Log.DatatableLog.DefaultView;
            //VisualDesignerHelper visualDesignerHelper = new VisualDesignerHelper();
            //visualDesignerHelper.InitializeVisualDesigner(_automationElementTree);
        }
        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = "(LogLevel = 'ERROR')";
            // dgInfoErrorWarnings.ItemsSource = Logger.Log.DatatableLog.DefaultView;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Logger.DatatableLog.Rows.Clear();
            //dgInfoErrorWarnings.ItemsSource = Logger.Log.DatatableLog.DefaultView;
        }
        private void Log_LogHandler(object sender, EventArgs e)
        {
           
            ThreadInvoker.Instance.RunByUiThread(() =>
            {
                if (stopflag == false)
                {                    
                    dgInfoErrorWarnings.DataContext = null;
                    dgInfoErrorWarnings.ItemsSource = null;
                    dgInfoErrorWarnings.DataContext = Log.Logger.DatatableLog.DefaultView;
                    dgInfoErrorWarnings.ItemsSource = Log.Logger.DatatableLog.DefaultView;

                }
            });

        }
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
