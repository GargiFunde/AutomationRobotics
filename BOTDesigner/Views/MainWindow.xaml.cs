using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Activities;
using System.Activities.Presentation.Toolbox;
using System.Reflection;
using System.IO;
using System.Activities.XamlIntegration;
using System.ComponentModel;
using System.Timers;
using System.Activities.Presentation;
using CommonLibrary;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Logger;
using System.Activities.Statements;
using System.Activities.Debugger;
using System.Activities.Presentation.Debug;
using System.IO.Compression;
using BOTDesigner.Helpers;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Net;
using System.Windows.Controls.Ribbon;
using Microsoft.Win32;
using System.Activities.Core.Presentation;
using Bot.Activity.Web;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading.Tasks;

namespace BOTDesigner.Views
{
    public partial class MainWindow :  INotifyPropertyChanged
    {
        private bool IsLicenseValid = false;
        private ToolboxControl _wfToolbox;
        private Border currentBorder = null;
        LayoutDocument currentLayoutDocument = null;
        DashBoard dashboard = null;
        private string currentWorkflowFile = string.Empty;
        private System.Timers.Timer _timer;
        int iLog = 0;
        int iOpenFile = 0;
        private const String _defaultSequence = "Sequence.xaml";
        private const String _defaultWorkflow = "FlowChart.xaml";
        private const String _defaultSM = "StateMachine.xaml";
        CommonTreeView ctr = null;
        LayoutDocument ldashboard = null;
        string projectLocation = string.Empty;
        string projectName = string.Empty;
        private Helpers.CustomTrackingParticipant _executionLog;
        string strSource = string.Empty;
        public IDesignerDebugView DebuggerService { get; set; }
        WorkflowApplication _wfApp = null;
        private bool IsMaximized = true;
        public bool IsFirstLaunch { get; set; } = true;

        private void Ribbon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
                {
                    var ribbon = sender as Ribbon;
                    if (ribbon != null)
                    {
                        ribbon.IsMinimized = false;
                    }


                    // Handled
                    e.Handled = true;
                }





        string _currentWorkflowFile
        {
            get
            {
                return currentWorkflowFile;
            }
            set
            {
                currentWorkflowFile = value;
                SelectHelper._currentworkflowfile = currentWorkflowFile;
                if (string.IsNullOrEmpty(currentWorkflowFile))
                {
                    ButtonWorkflowSaveMe.IsEnabled = false;
                    ButtonWorkflowSaveAll.IsEnabled = false;
                }
                else
                {
                    ButtonWorkflowSaveMe.IsEnabled = true;
                    ButtonWorkflowSaveAll.IsEnabled = true;
                }
            }
        }
        public string ProjectLocation
        {
            get
            {
                //if (_wfDesigner == null)
                //    NewInstance(_defaultWorkflow);
                return projectLocation;
            }
            set
            {
                projectLocation = value;
                IntializeButtonsEnableDisable();
            }
        }

        //public Border WfOutlineBorder { get; private set; }

        private void IntializeButtonsEnableDisable()
        {
                if (string.IsNullOrEmpty(projectLocation))
                {
                    ButtonWorkflowRun.IsEnabled = false;
                    ButtonWorkflowStop.IsEnabled = false;
                    ButtonDebug.IsEnabled = false;

                ButtonWorkflowNew.IsEnabled = false;
                ButtonWorkflowNewSeq.IsEnabled = false;
                ButtonWorkflowNewFC.IsEnabled = false;
                ButtonWorkflowNewSM.IsEnabled = false;

                //ButtonWorkflowNew.IsEnabled = true;
                //ButtonWorkflowNewSeq.IsEnabled = true;
                //ButtonWorkflowNewFC.IsEnabled = true;
                //ButtonWorkflowNewSM.IsEnabled = true;

                ButtonWorkflowSave.IsEnabled = false;

                    ButtonWorkflowImport.IsEnabled = true;
                    ButtonWorkflowExport.IsEnabled = false;
                    ButtonWorkflowPublish.IsEnabled = false;

                }
                else
                {
                    ButtonWorkflowRun.IsEnabled = true;
                    ButtonWorkflowStop.IsEnabled = true;
                    ButtonDebug.IsEnabled = true;

                    ButtonWorkflowNew.IsEnabled = true;
                    ButtonWorkflowNewSeq.IsEnabled = true;
                    ButtonWorkflowNewFC.IsEnabled = true;
                    ButtonWorkflowNewSM.IsEnabled = true;

                ButtonWorkflowSave.IsEnabled = true;

                    ButtonWorkflowImport.IsEnabled = true;
                    ButtonWorkflowExport.IsEnabled = true;
                    ButtonWorkflowPublish.IsEnabled = true;
                }
        }
        public MainWindow()
        {
            InitializeComponent();


            log4net.Config.XmlConfigurator.Configure();

            try
            {
                //if (!CheckLicenseValidity())
                //{
                //    IsLicenseValid = true;
                //    LicenseActivator licenseActivator = new LicenseActivator();
                //    licenseActivator.ShowDialog();

                //    if (licenseActivator.DialogResult == false)
                //    {
                //        Environment.Exit(0);
                //    }
                //}
                IntializeButtonsEnableDisable();

                ThreadInvoker.Instance.InitDispatcher();
                this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
                _timer = new System.Timers.Timer(1000);
                _timer.Enabled = false;
                _timer.Elapsed += TrackingDataRefresh;

                //load all available workflow activities from loaded assemblies 
                InitializeActivitiesToolbox();
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

                if (iOpenFile > 0)
                {
                    for (int i = 0; i < iOpenFile; i++)
                    {
                        SelectHelper.OpenXamlFile -= OpenWorkflowFile;
                    }
                }
                iOpenFile = 0;
                SelectHelper.OpenXamlFile += OpenWorkflowFile;
                iOpenFile = iOpenFile + 1;
                ldashboard = new LayoutDocument();
                ldashboard.ContentId = 1000;
                ldashboard.CanClose = false;

                dashboard = new DashBoard();
                dashboard.NewProjectDiagram += OpenBlankProject;
                dashboard.OpenProject += OpenProject;
                dashboard.Margin = new Thickness(0, 0, 0, 0);
                ldashboard.Content = dashboard;
                ldashboard.IsActiveChanged += layoutDocument_IsActiveChanged;

                ClearAvalonDocTabs();
                ldashboard.Title = "Dashboard";
                ldashboard.ToolTip = "Dashboard";

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

            AddVersionNumber();
            //CheckForupdates();
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            lbl_Title.Content += $" v.{ versionInfo.FileVersion }";
        }

        //private async Task CheckForupdates()
        //{
        //    using (var manager = new UpdateManager(@"C:\Temp\Releases"))
        //    {
        //        await manager.UpdateApp();
        //    }
        //}

        private void Ldashboard_Closing(object sender, CancelEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void Log_LogHandler(object sender, EventArgs e)
        {

            ThreadInvoker.Instance.RunByUiThread(() =>
            {
                try
                {
                    dgInfoErrorWarnings.DataContext = Logger.Log.Logger.DatatableLog;
                    dgInfoErrorWarnings.ItemsSource = null;
                    dgInfoErrorWarnings.ItemsSource = Logger.Log.Logger.DatatableLog.DefaultView;
                }
                catch (Exception)
                {
                    //raised by other background thread
                }
            });

        }

        //private LayoutAnchorable GetAnchorWindow(int contentid)
        //{
        //    try
        //    {
        //        IEnumerable<ILayoutElement> LayoutDocuments = null;
        //        LayoutDocuments = this.dockManager.Layout.Descendents();
        //        foreach (ILayoutElement item in LayoutDocuments)
        //        {
        //            if (item is LayoutAnchorable)
        //            {
        //                LayoutAnchorable laProp = (LayoutAnchorable)item;
        //                if (laProp.ContentId == contentid)
        //                {
        //                    return laProp;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
        //    return null;
        //}

        private void CmdWorkflowXPathWeb(object sender, ExecutedRoutedEventArgs e)
        {
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
            try
            {
                XPathWebWindow xpwindow = new XPathWebWindow();
                xpwindow.Show();
            }
            catch (Exception )
            {

               // System.IO.File.AppendAllText(@"C:\Work\XPathLogs","MainWindow - "+ex+"\n");
            }
        //});
        }
        private void CmdWorkflowXPathWin(object sender, ExecutedRoutedEventArgs e)
        {
            //GetWinXPath window = new GetWinXPath();
            //window.Show();
            ThreadInvoker.Instance.RunByUiThread(() =>
            {
                Bot.Activity.WinDriverPlugin.MainWindow uirecorder = new Bot.Activity.WinDriverPlugin.MainWindow();
                uirecorder.Show();
            });
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgInfoErrorWarnings.DataContext = Logger.Log.Logger.DatatableLog;
                dgInfoErrorWarnings.ItemsSource = Logger.Log.Logger.DatatableLog.DefaultView;

                string strLibraryPath = ConfigurationManager.AppSettings["LibraryPath"];

                var serializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
                serializer.LayoutSerializationCallback += (s, args) =>
                {
                    args.Content = args.Content;
                };

                if (File.Exists(@".\AvalonDock.config"))
                    serializer.Deserialize(@".\AvalonDock.config");

                LoadLibrary(strLibraryPath);
           
                System.Windows.Application.Current.DispatcherUnhandledException -= Current_DispatcherUnhandledException;
                System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException; //required for hiding unhiding layoutanchor doc to suppress Avalondock error

                HideAllAnchorWindows(); //hide
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void TrackingDataRefresh(Object source, ElapsedEventArgs e)
        {
            NotifyPropertyChanged("ExecutionLog");
        }


        /// <summary>
        /// show execution log in ui
        /// </summary>
        private void UpdateTrackingData()
        {
            //retrieve & display execution log
            //consoleExecutionLog.Dispatcher.Invoke(
            //    System.Windows.Threading.DispatcherPriority.Normal,
            //    new Action(
            //        delegate ()
            //        {
            //            //consoleExecutionLog.Text = _executionLog.TrackData;
            consoleExecutionLog.Text = _executionLog.TrackData;
            NotifyPropertyChanged("ExecutionLog");
            SelectHelper.Border.Child.IsEnabled = true;
            //        }
            //));
        }

        //For invokeXAML Workflow
        private void CmdOpenInvokedWorkflow(object sender, ExecutedRoutedEventArgs e)
        {

            string filetobeopened3 = e.Parameter.ToString();

            OpenFile(filetobeopened3);
        }

        /// <summary>
        /// Retrieves all Workflow Activities from the loaded assemblies and inserts them into a ToolboxControl 
        /// </summary>
        private void InitializeActivitiesToolbox()
        {
            const Int32 BufferSize = 128;
            string currentassembly = string.Empty;
            try
            {
                WfToolboxBorder.Child = null;
                _wfToolbox = new ToolboxControl();

                using (var fileStream = File.OpenRead("LoadToolBox.txt"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if ((!string.IsNullOrEmpty(line)) && (line.Trim().Length != 0))
                        {
                            line = line.Trim();
                            string lineComment = line.Substring(0, 2);
                            if (lineComment.Contains("//"))
                            {
                                string categoryname = line.Replace("//", "");
                                if (_wfToolbox.Categories.Any(p => p.CategoryName == categoryname))
                                {
                                    _wfToolbox.Categories.Remove(_wfToolbox.Categories.First(p => p.CategoryName == categoryname));
                                }
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    AppDomain.CurrentDomain.Load(line);
                                }
                                catch (Exception ex)
                                {
                                    if (!currentassembly.ToLower().Contains("microsoft.generatedcode"))
                                        Log.Logger.LogData("Error while loading assembly:" + currentassembly + " Details:" + ex.Message, LogLevel.Error);
                                }
                            }
                        }
                    }
                }
                _wfToolbox.Categories.Add(
                     new System.Activities.Presentation.Toolbox.ToolboxCategory
                     {
                         CategoryName = "Conditions",
                         Tools = {
                                new ToolboxItemWrapper(typeof(Switch<>), "Switch"),
                                new ToolboxItemWrapper(typeof(AddToCollection<>), "AddToCollection"),
                                new ToolboxItemWrapper(typeof(ExistsInCollection<>), "ExistsInCollection"),
                                new ToolboxItemWrapper(typeof(ClearCollection<>), "ClearCollection"),
                                new ToolboxItemWrapper(typeof(RemoveFromCollection<>), "RemoveFromCollection"),
                                new ToolboxItemWrapper(typeof(System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory<>),"ForEachWithBodyFactory"),
                                new ToolboxItemWrapper(typeof(System.Activities.Core.Presentation.Factories.ParallelForEachWithBodyFactory<>),"ParallelForEachWithBodyFactory"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.AddToDictionary<,>),"AddToDictionary"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.ClearDictionary<,>),"ClearDictionary"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.RemoveFromDictionary<,>),"RemoveFromDictionary"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.GetFromDictionary<,>),"GetFromDictionary"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.KeyExistsInDictionary<,>),"KeyExistsInDictionary"),
                                new ToolboxItemWrapper(typeof(Microsoft.Activities.Extensions.Statements.ValueExistsInDictionary<,>),"ValueExistsInDictionary"),
                           }
                     }
              );

               IEnumerable<Assembly> appAssemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.GetName().Name);

                // check if assemblies contain activities
                int activitiesCount = 0;
                foreach (Assembly activityLibrary in appAssemblies)
                {
                    try
                    {
                        var wfToolboxCategory = new ToolboxCategory(activityLibrary.GetName().Name);
                        currentassembly = activityLibrary.GetName().Name;
                        var actvities = from
                                            activityType in activityLibrary.GetExportedTypes()
                                        where
                                            (activityType.IsSubclassOf(typeof(Activity))
                                            || activityType.IsSubclassOf(typeof(NativeActivity))
                                            || activityType.IsSubclassOf(typeof(DynamicActivity))
                                            || activityType.IsSubclassOf(typeof(ActivityWithResult))
                                            || activityType.IsSubclassOf(typeof(AsyncCodeActivity))
                                            || activityType.IsSubclassOf(typeof(CodeActivity))
                                            || activityType == typeof(System.Activities.Core.Presentation.Factories.ForEachWithBodyFactory<Type>)
                                            || activityType == typeof(System.Activities.Statements.FlowNode)
                                            || activityType == typeof(System.Activities.Statements.State)
                                            || activityType == typeof(System.Activities.Core.Presentation.FinalState)
                                            || activityType == typeof(System.Activities.Statements.FlowDecision)
                                            || activityType == typeof(System.Activities.Statements.FlowNode)
                                            || activityType == typeof(System.Activities.Statements.FlowStep)
                                            || activityType == typeof(System.Activities.Statements.FlowSwitch<Type>)
                                            || activityType == typeof(System.Activities.Statements.ForEach<Type>)
                                            || activityType == typeof(System.Activities.Statements.Switch<Type>)
                                            || activityType == typeof(System.Activities.Statements.TryCatch)
                                            || activityType == typeof(System.Activities.Statements.While))
                                            && activityType.IsVisible
                                            && activityType.IsPublic
                                            && !activityType.IsNested
                                            && !activityType.IsAbstract 
                                            && (activityType.GetConstructor(Type.EmptyTypes) != null)
                                            && !activityType.Name.Contains('`') //optional, for extra cleanup
                                        orderby
                                            activityType.Name
                                        select
                                            new ToolboxItemWrapper(activityType);

                        // actvities.ToList().ForEach(wfToolboxCategory.Add);                    
                        //wfToolboxCategory.CategoryName = wfToolboxCategory.CategoryName.Replace("Bot.Activity.", "");
                        //wfToolboxCategory.CategoryName = wfToolboxCategory.CategoryName.Replace("System.", "");
                        foreach (ToolboxItemWrapper item in actvities.ToList())
                        {
                            wfToolboxCategory.Add(item);
                        }
                        if (wfToolboxCategory.Tools.Count > 0)
                        {
                            _wfToolbox.Categories.Add(wfToolboxCategory);
                            activitiesCount += wfToolboxCategory.Tools.Count;
                        }
                    }
                    catch (Exception)
                    {
                        //if(activityLibrary !=null)
                        //Log.Logger.LogData("Error while loading activity library:" + activityLibrary.GetName().Name + " Details:" + ex.Message, LogLevel.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                if (!currentassembly.ToLower().Contains("microsoft.generatedcode"))
                    Log.Logger.LogData("Error while loading assembly:" + currentassembly + " Details:" + ex.Message, LogLevel.Error);
            }
            
            try
            {
                using (var fileStream = File.OpenRead("LoadToolBox.txt"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if ((!string.IsNullOrEmpty(line)) && (line.Trim().Length != 0))
                        {
                            line = line.Trim();
                            string lineComment = line.Substring(0, 2);
                            if (lineComment.Contains("//"))
                            {
                                //string categoryname = line.Replace(Path.DirectorySeparatorChar.ToString(), "");
                                string categoryname = line.Replace("//", "");
                                if (_wfToolbox.Categories.Any(p => p.CategoryName == categoryname))
                                {
                                    _wfToolbox.Categories.Remove(_wfToolbox.Categories.First(p => p.CategoryName == categoryname));
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            WfToolboxBorder.Child = _wfToolbox;
        }




        /// <summary>
        /// Retrieve Workflow Execution Logs and Workflow Execution Outputs
        /// </summary>
        private void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs ev)
        {
            try
            {    
                //retrieve & display execution log
                _timer.Stop();
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    UpdateTrackingData();
                    SelectHelper.DialogWacher = false;
                    if ((SelectHelper._wfDesigner.DebugManagerView != null) && (SelectHelper._wfDesigner.DebugManagerView.CurrentLocation != null))
                    {
                        SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = null;                    
                    }
                    //else
                    //{
                    //    if (this.WindowState != WindowState.Normal)
                    //    {
                    //        this.WindowState = WindowState.Normal;
                    //    }
                    //}
                });
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                {
                   SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = null;
                }
                Logger.Log.Logger.LogData("Process Completed ", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Process Failed to Complete due to Exception: "+ex.Message, LogLevel.Error);
            }
        }


        #region Commands Handlers - Executed - New, Open, Save, Run
       
       
        /// <summary>
        /// Creates a new Workflow Application instance and executes the Current Workflow
        /// </summary>

        private void CmdWorkflowRun(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (CustomWfDesigner.Instance == null)
                    return;
                SelectHelper.DialogWacher = true;
                //get workflow source from designer
                // Logger.Log.DatatableLog.Rows.Clear();
                CustomWfDesigner.Instance.Save(_currentWorkflowFile);
                SelectHelper.Border.Child.IsEnabled = false;
                Logger.Log.Logger.DatatableLog.Clear();
                Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = string.Empty;
                SelectHelper.CurrentRuntimeApplicationHelper = new RuntimeApplicationHelper();
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                {   
                    SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = SelectHelper.CurrentRuntimeApplicationHelper;

                }
                else
                {
                    SelectHelper.RuntimeApplicationHelperDictionary.Add(_currentWorkflowFile, SelectHelper.CurrentRuntimeApplicationHelper);

                }
                // SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile];

                CustomWfDesigner.Instance.Flush();
                MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
                //configure workflow application
                consoleExecutionLog.Text = String.Empty;
                //consoleOutput.Text = String.Empty;
                _executionLog = new CustomTrackingParticipant();
                 _wfApp = new WorkflowApplication(activityExecute);
                _wfApp.Extensions.Add(_executionLog);
                _wfApp.Completed = WfExecutionCompleted;

                SelectHelper._wfApplication = _wfApp;

                // this.WindowState = WindowState.Minimized;
                //Thread.Sleep(1000);

                //execute 
                Logger.Log.Logger.LogData("Process Started",LogLevel.Info);
                try
                {
                    _wfApp.Run();
                }
                catch (Exception b)
                {
                    Logger.Log.Logger.LogData("Exception while Run()" + b.InnerException, LogLevel.Error);
                    if (_wfApp != null)
                    {
                        SelectHelper.DialogWacher = false;
                        // _wfApp.Terminate("");
                        _wfApp.Abort("Stopped by Exception");
                        _timer.Stop();
                        UpdateTrackingData();

                        if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                        {
                            SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = null;
                        }
                        if ((SelectHelper._wfDesigner.DebugManagerView != null) && (SelectHelper._wfDesigner.DebugManagerView.CurrentLocation != null))
                        {
                            SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = null;
                        }
                        GC.Collect(0);
                        Log.Logger.LogData("Process stopped due to Exception", LogLevel.Info);
                    }
                    }

                    //enable timer for real-time logging
                    _timer.Start();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                //Log.Logger.LogData("Process Stopped", LogLevel.Info);
                //_wfApp.Abort("Process stopped due to Exception");
                //_wfApp.Terminate("Process Stopped");
                //_timer.Stop();
                //UpdateTrackingData();
                //GC.Collect(0);
                //Logger.Log.Logger.LogData("Process Stopped", LogLevel.Info);
                //Log.Logger.LogData(ex.Message, LogLevel.Error);
                Logger.Log.Logger.LogData("Process Stopping", LogLevel.Info);
                if (_wfApp != null)
                {
                    SelectHelper.DialogWacher = false;
                    _wfApp.Abort("Stopped by User");
                    _timer.Stop();
                    UpdateTrackingData();

                    if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                    {
                        SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = null;
                    }
                    if ((SelectHelper._wfDesigner.DebugManagerView != null) && (SelectHelper._wfDesigner.DebugManagerView.CurrentLocation != null))
                    {
                        SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = null;
                    }
                    GC.Collect(0);
                    Logger.Log.Logger.LogData("Process Stopped", LogLevel.Info);
                }
            }
        }


        /// <summary>
        /// Stops the Current Workflow
        /// </summary>
        /// 

            //MOuse whell roller for dll
        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }


        private void CmdWorkflowStop(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //manual stop
               Logger.Log.Logger.LogData("Process Stopping", LogLevel.Info);
                if (_wfApp != null)
                {  
                    SelectHelper.DialogWacher = false;
                    _wfApp.Abort("Stopped by User");
                    _timer.Stop();
                    UpdateTrackingData();

                    if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                    {
                        SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = null;
                    }
                    if ((SelectHelper._wfDesigner.DebugManagerView != null) && (SelectHelper._wfDesigner.DebugManagerView.CurrentLocation != null))
                    {
                        SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = null;
                    }
                    GC.Collect(0);
                    Logger.Log.Logger.LogData("Process Stopped", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        //private bool CheckLicenseValidity()
        //{
        //    Portable.Licensing.License ulicense = null;
        //    Stream myStream = null;
        //    try
        //    {
        //        // myStream = File.OpenRead("lic.lic");
        //        string text = File.ReadAllText("lic.lic");
        //        string decrypted = SecurityEncryptHelper.GetSecurityEncryptHelper().DecryptText(text);
        //        ulicense = Portable.Licensing.License.Load(decrypted);
        //        string publickey = File.ReadAllText("PublicKey.txt");

        //        LicenseHelper lf = new LicenseHelper();
        //        var str = lf.ValidateLicense(ulicense, publickey);
        //        if (str == "Activated: License is Valid")
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        Log.Logger.LogData("Error in validating license", LogLevel.Info);
        //    }
        //}

        private void CmdWorkflowShowLog(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                filePath = filePath + Path.DirectorySeparatorChar + "E2EBot" + Path.DirectorySeparatorChar + "Logs";

                if (Directory.Exists(filePath))
                {
                    Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void CmdWorkflowClearLog(object sender, ExecutedRoutedEventArgs e)
        {
            string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            filePath = filePath + Path.DirectorySeparatorChar + "E2EBot" + Path.DirectorySeparatorChar + "Logs";
            try
            {
                if (Directory.Exists(filePath))
                {
                    DirectoryInfo directory = new DirectoryInfo(filePath);
                    foreach (System.IO.FileInfo file in directory.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception)
                        {                           
                            //Current file - need to clear
                            //this.Dispatcher.Invoke(DispatcherPriority.Render
                            //, (Action)(() =>
                            //{
                            //    try
                            //    { 
                            //        StreamWriter strm = file.CreateText();
                            //        strm.Flush();
                            //       strm.Close();
                            //    }
                            //    catch (Exception ex1)
                            //    {
                            //        Log.Logger.LogData(ex1.Message, LogLevel.Info);
                            //    }
                            //}));
                        }
                    }
                    //delete directories in this directory:
                    foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
                    {
                        directory.Delete(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Info);
            }

        }
        private void CmdWorkflowDev(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Not available in trial version!");
        }
        private void CmdLogout(object sender, ExecutedRoutedEventArgs e)
        {
            ////MessageBox.Show("Logout!");
            //LoginWindow login = new LoginWindow();
            //login.Show();

            //LoginWindow login = new LoginWindow();
            //login.Show();
            //this.Hide();

            //  ((App)Application.Current).OnLogout();

            Window_Closed(sender,e);


        }
        private void CmdWorkflowPublish(object sender, ExecutedRoutedEventArgs e)
        {
            int flag = 0;
            try
            {
                
                string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                filePath = filePath + Path.DirectorySeparatorChar + "E2EBot" + Path.DirectorySeparatorChar + "Publish";
                try
                {
                    if(!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string[] strDirArray = null;
                    string[] currentDir = projectLocation.Split(Path.DirectorySeparatorChar);
                    if (currentDir.Count() > 0)
                    {
                        string strversion = GetVersion();
                        if (currentDir.Count() > 0)
                        {
                            string strDir = currentDir[currentDir.Count() - 1];

                            //it will fail if strDir is having . in it
                            strDirArray = strDir.Split('.');
                            if ((strDirArray.Length == 1) || (strDirArray.Length == 5))
                            {
                                string destpath = filePath + Path.DirectorySeparatorChar + strDirArray[0] + strversion + ".zip";
                                ZipFile.CreateFromDirectory(projectLocation, destpath);
                                flag = 1;
                            }
                            else
                            {
                                // MessageBoxResult result = MessageBox.Show("Error while Publishing !!");
                                flag = 0;
                                Log.Logger.LogData("Need to write logic in code to handle . in directory name", LogLevel.Error);
                            }
                        }
                    }
                    else
                    {
                        // MessageBoxResult result = MessageBox.Show("Error while Publishing !!");
                        flag = 0;
                        Log.Logger.LogData("Current directory is not set.", LogLevel.Error);
                    }
                    
                }
                catch (Exception ex)
                {
                    flag = 0;
                    Log.Logger.LogData("Error in Export : Details - " + ex.Message, LogLevel.Error);
                }
                //Publish publish = null;
                //if (string.IsNullOrEmpty(projectName))
                //{
                //    publish = new Publish(ProjectLocation);
                //}
                //else
                //{
                //    publish = new Publish(projectName);
                //}
                //publish.ShowDialog();
                //if (publish.DialogResult == true)
                //{
                //    string[] currentDir = projectLocation.Split(Path.DirectorySeparatorChar);
                //    if (currentDir.Count() > 0)
                //    {
                //        byte[] fileBytes = null;
                //        string destpath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + currentDir[currentDir.Count() - 1] + ".zip";
                //        ZipFile.CreateFromDirectory(projectLocation, destpath);
                //        using (FileStream stream = File.OpenRead(destpath))
                //        {
                //            fileBytes = new byte[stream.Length];

                //            stream.Read(fileBytes, 0, fileBytes.Length);
                //            stream.Close();
                //        }
                //        File.Delete(destpath);
                //        ServiceReference1.BOTServiceClient bOTServiceClient = new ServiceReference1.BOTServiceClient();
                //        PublishProcessEntity publishProcessEntity = new PublishProcessEntity();
                //        publishProcessEntity.processname = currentDir[currentDir.Count() - 1];
                //        publishProcessEntity.processfiles = fileBytes;
                //        publishProcessEntity.version = DateTime.Now.ToLongDateString();
                //        publishProcessEntity.createdby = Environment.UserName;
                //        publishProcessEntity.createddate = DateTime.Now.ToShortDateString();
                //        publishProcessEntity.updatedby = Environment.UserName;
                //        publishProcessEntity.updateddate = DateTime.Now.ToShortDateString();
                //        bOTServiceClient.PublishProcess(publishProcessEntity);

                //    }
                //}
            }
            catch (Exception ex)
            {
                flag = 0;
                Log.Logger.LogData("Error in Export : Details - " + ex.Message, LogLevel.Error);
            }

            ((App)Application.Current).OnPublish(flag);
        }
        private void CmdWorkflowImport(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Import import = new Import();
                import.ShowDialog();
                if (import.DialogResult == true)
                {
                    string destpath = Path.GetDirectoryName(import.ImportFileName);
                    string[] currentDir = import.ImportFileName.Split(Path.DirectorySeparatorChar);
                    string getfilename = Path.GetFileName(import.ImportFileName);
                    if (!string.IsNullOrEmpty(getfilename))
                    {
                        getfilename = getfilename.Replace(Path.GetExtension(getfilename), "");
                        destpath = destpath + Path.DirectorySeparatorChar + getfilename;
                        ZipFile.ExtractToDirectory(import.ImportFileName, destpath);
                        //using (ZipArchive archive = ZipFile.OpenRead(import.ImportFileName))
                        //{
                        //    foreach (ZipArchiveEntry entry in archive.Entries)
                        //    {
                        //        if(entry.)
                        //         entry.ExtractToFile(Path.Combine(destpath, entry.FullName));
                        //    }
                        //}
                        LoadProject(destpath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Error in import : Details - " + ex.Message, LogLevel.Error);
            }
        }
        private void CmdWorkflowExport(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string[] strDirArray = null;
                Export export = new Export();
                export.ShowDialog();
                if (export.DialogResult == true)
                {

                    string[] currentDir = projectLocation.Split(Path.DirectorySeparatorChar);
                    if (currentDir.Count() > 0)
                    {
                        string strversion = GetVersion();
                        if(currentDir.Count() > 0)
                        {
                            string strDir = currentDir[currentDir.Count() - 1];

                            //it will fail if strDir is having . in it or if the naming format is changed
                            strDirArray = strDir.Split('.');
                            if((strDirArray.Length==1)||(strDirArray.Length==5))
                            {
                                string destpath = export.ProjectName + Path.DirectorySeparatorChar + strDirArray[0] + strversion + ".zip";
                                ZipFile.CreateFromDirectory(projectLocation, destpath);
                            }
                            else
                            {
                                Log.Logger.LogData("Need to write logic in code to handle . in directory name", LogLevel.Error);
                            }                    
                           
                        }                        
                    }
                    else
                    {
                        Log.Logger.LogData("Current directory is not set." , LogLevel.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Error in Export : Details - " + ex.Message, LogLevel.Error);
            }
        }

        private string GetVersion()
        {
            //ip address 127.0.0.1 means you are disconnected and not in network!!!

            DateTime date = DateTime.Now;
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            string[] stip = myIP.Split('.');
            string timestamp = date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            //string strversion = "1.0." + stip[stip.Length - 1] + "." + date.Ticks.ToString();
            string strversion = ".1.0." + stip[stip.Length - 1] + "." + timestamp;
            return strversion;
        }
        private void HideAllAnchorWindows()
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
               
                for (int i = 0; i <= LayoutDocuments.Count(); i++)
                {
                    ILayoutElement item = LayoutDocuments.ElementAt(i);
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable laProp = (LayoutAnchorable)item;
                        laProp.Hide();  //hide
                    }
                }
            }
            catch (Exception)
            {
               // Log.Logger.LogData(ex.Message, LogLevel.Error); --No need to log it : Ajit
            }
        }
        //private void UnHideAllAnchorWindows()
        //{
        //    try
        //    {
        //        IEnumerable<ILayoutElement> LayoutDocuments = null;
        //        LayoutDocuments = this.dockManager.Layout.Descendents();
        //        for (int i = 0; i < LayoutDocuments.Count(); i++)
        //        {
        //            ILayoutElement item = LayoutDocuments.ElementAt(i);
        //            if (item is LayoutAnchorable)
        //            {
        //                LayoutAnchorable laProp = (LayoutAnchorable)item;
        //                laProp.Show();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
        //}


        //hide

            //hide
        private void DeActivateWindow(int contentid)
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable laProp = (LayoutAnchorable)item;
                        if (laProp.ContentId == contentid)
                        {
                            if ((laProp != null))
                            {
                                laProp.Hide();
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        //hide
        public void UnHideAllAnchorWindows()
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();

                for (int i = 0; i < LayoutDocuments.Count(); i++)
                {
                    ILayoutElement item = LayoutDocuments.ElementAt(i);
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable laProp = (LayoutAnchorable)item;
                        laProp.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        bool bmax = false;
        private void CmdWorkflowMax(object sender, ExecutedRoutedEventArgs e)
        {
            if (!bmax)
            {
                bmax = true;
                wfGrid.RowDefinitions[0].Height = new GridLength(0);
                // wfGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                HideAllAnchorWindows();
                HideAllAnchorWindows(); //need to call two times as last pane remains  as visible
            }
            else
            {
                bmax = false;
                wfGrid.RowDefinitions[0].Height = new GridLength(133);
                //wfGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                UnHideAllAnchorWindows();
            }

        }
        private void CmdWorkflowMin(object sender, ExecutedRoutedEventArgs e)
        {
            wfGrid.RowDefinitions[0].Height = new GridLength(133);
            //wfGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            UnHideAllAnchorWindows();
            //UnHideAllAnchorWindows();
        }
        private void CmdWorkflowSettigs(object sender, ExecutedRoutedEventArgs e)
        {
            //Settings settings = new Settings();
            //settings.Show();
        }
     
        /// <summary>
        /// Save the current state of a Workflow
        /// </summary>
        private void CmdWorkflowSave(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                CustomWfDesigner.Instance = SelectHelper.WorkflowDictionary[_currentWorkflowFile];
                CustomWfDesigner.Instance.Flush();
                CustomWfDesigner.Instance.Save(_currentWorkflowFile);
                SelectHelper._wfDesigner = CustomWfDesigner.Instance;
                  //SelectHelper._wfDesigner.Save(_currentWorkflowFile);
                  //SelectHelper._wfDesigner.Flush();
                  //CustomWfDesigner.Instance.OutlineView.UpdateLayout();
                  //SelectHelper._wfDesigner.OutlineView.UpdateLayout();
                  ////LoadOutline();
                  //ActivateWindow(6030);
                  //  ReloadDesigner();
                  //LoadOutline();

                Logger.Log.Logger.LogData("Save Successfull....", Logger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Save Error: "+ex.Message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Save the current state of a Workflow
        /// </summary>
        private void CmdWorkflowSaveAll(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
               // ModifierKeys.Control
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutDocumentPane)
                    {
                        LayoutDocumentPane la = (LayoutDocumentPane)item;
                        foreach (LayoutDocument ld in la.Children)
                        {
                            if (ld.ToolTip != null)
                            {
                                string strFullDirectoryName = ld.ToolTip.ToString();
                                if (SelectHelper.WorkflowDictionary.Keys.Contains(strFullDirectoryName))
                                {
                                    WorkflowDesigner cd = (WorkflowDesigner)SelectHelper.WorkflowDictionary[strFullDirectoryName];
                                    cd.Save(strFullDirectoryName);
                                }
                            }
                        }
                        // ctr.LibraryPath = @"pack://application:,/Library";

                    }
                }
                CustomWfDesigner.Instance.Save(_currentWorkflowFile);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Save All Exception: "+ex.Message, LogLevel.Info);

            }

        }

        /// <summary>
        /// Creates a new Workflow Designer instance and loads the Default Workflow 
        /// </summary>
        private void CmdWorkflowNew(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewDiagram(ProjectType.sequence);

        }
        private void CmdNewFlowChart(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewDiagram(ProjectType.workflow);

        }
        private void CmdNewStateMachine(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewDiagram(ProjectType.statemachine);

        }
        private void CreateNewDiagram(ProjectType diagramType)
        {
            try
            {
                NewProject newProject = new NewProject("New Diagram");
                newProject.Location = ProjectLocation;
                newProject.ShowDialog();
                if (newProject.DialogResult == true)
                {
                    string diagramName = newProject.ProjectName; //diagram name
                    string Location = newProject.Location;
                    string strNewWorkflow = Location + Path.DirectorySeparatorChar + diagramName + ".xaml";
                    if (File.Exists(strNewWorkflow))
                    {
                        MessageBox.Show("File already exists!");
                        return;
                    }
                    else
                    {
                        //Adding Piyush
                        ProjectLocation =  newProject.Location;
                        if (diagramType == ProjectType.sequence)
                        {
                            File.Copy(_defaultSequence, strNewWorkflow);
                        }
                        else if (diagramType == ProjectType.workflow)
                        {
                            File.Copy(_defaultWorkflow, strNewWorkflow);
                        }
                        else if (diagramType == ProjectType.statemachine)
                        {
                            File.Copy(_defaultSM, strNewWorkflow);
                        }
                    }
                    AddWorkFlow(strNewWorkflow); // Need to finetune
                                                 // AddWorkFlow(Location + "\\" + diagramName + ".xaml");
                    ReloadTree();
                    //LoadOutline();

                    //READ:-below line/logic give problem in case of adding file in new folder which is created and not in treeview
                    // ctr.AddingNewItem(Location, diagramName + ".xaml",ctr.foldersItem.Items);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        public void AddWorkFlow(string sourcename, LayoutDocumentPane laDocument = null)
        {
            try
            {
                if (laDocument != null)
                {
                    AddDocumentObject(sourcename, laDocument);
                    return;
                }
                LayoutDocumentPane la = null;
                LayoutDocumentPaneGroup ldGr = null;
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutPanel)
                    {
                        LayoutPanel lp = (LayoutPanel)item;
                        Orientation o = lp.Orientation;
                        if (o.ToString() == "Horizontal")
                        {
                            if (lp.Children.Count > 0)
                            {
                                foreach (var itemIn1 in lp.Children)
                                {
                                    if (itemIn1 is LayoutDocumentPaneGroup)
                                    {
                                        LayoutDocumentPaneGroup ldpg = (LayoutDocumentPaneGroup)itemIn1;
                                        if (ldpg.Children.Count > 0)
                                        {
                                            foreach (var itemin2 in ldpg.Children)
                                            {
                                                bool isDocumentpane = true;
                                                la = (LayoutDocumentPane)ldpg.Children[0];
                                                foreach (var itemin3 in la.Children)
                                                {
                                                    if (itemin3 is LayoutAnchorable)
                                                    {
                                                        LayoutAnchorable itemAL = (LayoutAnchorable)itemin3;
                                                        if ((itemAL.ContentId == 100) || (itemAL.ContentId == 200))
                                                        {
                                                            isDocumentpane = false;
                                                            break;
                                                        }

                                                    }
                                                }
                                                if (isDocumentpane == true)
                                                {
                                                    AddDocumentObject(sourcename, la);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void AddDocumentObject(string sourcename, LayoutDocumentPane la)
        {
            try
            {
                LayoutDocument ld = new LayoutDocument();
                ld.IsActiveChanged += layoutDocument_IsActiveChanged;
                //  ld.Closed += Ld_Closed;
                ld.Closing += Ld_Closed;
                la.Children.Add(ld);
                // ld.Title = sourcename.Substring(sourcename.LastIndexOf("\\") + 1);
                ld.Title = sourcename.Substring(sourcename.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                
                //ld.Title = sourcename;
                SelectHelper.CurrentProcessName = ld.Title;
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(currentLayoutDocument.Title))
                {
                    SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[currentLayoutDocument.Title];
                }
                _currentWorkflowFile = sourcename;
                ld.ToolTip = sourcename;
                Border WfDesignerBorder = new Border();
                //  string strid = DateTime.Now.ToString("yyMMddHHmmss");
                WfDesignerBorder.Tag = sourcename;
                // WfDesignerBorder.ToolTip = sourcename;
                ld.Content = WfDesignerBorder;
                SelectHelper.Border = WfDesignerBorder;
              
                CustomWfDesigner.NewInstanceVB(sourcename);
                SelectHelper._wfPropertyBorder = WfPropertyBorder;
                SelectHelper._wfOutlineBorder = WfOutlineBorder;
                SelectHelper.Border.Child = CustomWfDesigner.Instance.View;
                WfPropertyBorder.Child = CustomWfDesigner.Instance.PropertyInspectorView;
                WfOutlineBorder.Child = CustomWfDesigner.Instance.OutlineView;
                /*     selecthelper._wfoutlineborder.child = customwfdesigner.instance.outlineview; */ //ak
                ld.IsSelected = true;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return;
        }

        private void Ld_Closed(object sender, EventArgs e)
        {
           // MessageBox.Show(currentWorkflowFile+","+currentLayoutDocument);
            //Save file on the close of a tab------------------------
            if (MessageBox.Show("Do you want to save before exit?",
"", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //save work flow
              //  MessageBoxResult result = MessageBox.Show("User wants to save his workflow");
                CustomWfDesigner.Instance.Save(_currentWorkflowFile);
               // currentWorkflowFile = "";
            }
            else
            {
                //workflow exists but user does not want to save  
               // MessageBoxResult result = MessageBox.Show("User does not want to save his workflow");

            }

            //--------------------------------------------------------
            if (sender is LayoutDocument)
            {
                LayoutDocument ld = (LayoutDocument)sender;
                string strDocName = (string)ld.ToolTip;
                if (SelectHelper.WorkflowDictionary.ContainsKey(strDocName))
                {
                    SelectHelper.WorkflowDictionary.Remove(strDocName);
                }
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(strDocName))
                {
                    SelectHelper.RuntimeApplicationHelperDictionary.Remove(strDocName);
                    SelectHelper.CurrentRuntimeApplicationHelper = null;
                }
            }
        }

        private void ActivateWindow(int contentid)
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable laProp = (LayoutAnchorable)item;
                        if (laProp.ContentId == contentid)
                        {
                            if ((laProp != null) && (!laProp.IsSelected))
                            {
                                try
                                {
                                    laProp.IsSelected = true;
                                    laProp.Show();
                                }
                                catch (Exception)
                                {
                                    //no idea why it was failing...do not add it in log
                                }
                            }
                            break;

                        }
                    }
                }
                //LoadOutline();


            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        //void layoutDocument_IsActiveChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (sender.GetType().Name == "LayoutDocument")
        //        {
        //            currentLayoutDocument = (LayoutDocument)sender;
        //            if (currentLayoutDocument.ContentId == 1000) //Dashboard
        //            {
        //                return;
        //            }

        //            if (currentLayoutDocument.Content != null)
        //            {

        //                currentBorder = (Border)currentLayoutDocument.Content;
        //                string strSource1 = (string)currentBorder.Tag;
        //                //  CustomWfDesigner.Instance = be.WorkflowDesignerObj;
        //                if (strSource == strSource1)
        //                {
        //                    return;
        //                }
        //                else
        //                {
        //                    strSource = strSource1;
        //                }
        //                if (currentBorder.Tag != null)
        //                {
        //                    string strSource = (string)currentBorder.Tag;

        //                    if ((strSource != null) && (strSource.Trim().Length > 0))
        //                    {

        //                        if (SelectHelper.WorkflowDictionary.ContainsKey(strSource))
        //                        {
        //                            CustomWfDesigner.Instance = SelectHelper.WorkflowDictionary[strSource];
        //                            SelectHelper._wfDesigner = SelectHelper.WorkflowDictionary[strSource];
        //                            SelectHelper.CurrentProcessName = currentLayoutDocument.Title;
        //                            if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(currentLayoutDocument.Title))
        //                            {
        //                                SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[currentLayoutDocument.Title];
        //                            }
        //                            _currentWorkflowFile = currentLayoutDocument.ToolTip.ToString();
        //                            CustomWfDesigner.Instance.View.UpdateLayout();
        //                            currentBorder.Child = CustomWfDesigner.Instance.View;
        //                            WfPropertyBorder.Child = CustomWfDesigner.Instance.PropertyInspectorView;

        //                            //Need to create new list new List<ModelItem> and store list in Dictionary
        //                            // treeView1.DataContext = new List<ModelItem> { wd.Context.Services.GetService<ModelService>().Root };

        //                            if ((!string.IsNullOrEmpty(SelectHelper._currentscrapfile)) && (SelectHelper._currentscrapfile != strSource))
        //                            {
        //                                ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(3);
        //                            }
        //                            EnableDebugging();

        //                        }
        //                    }
        //                }
        //            }
        //            ActivateWindow(5); //properties window

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
        //}

        //hide
        //void layoutDocument_IsActiveChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string strSource1 = string.Empty;

        //        if (sender.GetType().Name == "LayoutDocument")
        //        {
        //            currentLayoutDocument = (LayoutDocument)sender;

        //            if (currentLayoutDocument.ContentId == 1000) //Dashboard
        //            {
        //                HideAllAnchorWindows();
        //                DeActivateWindow(4000);
        //                DeActivateWindow(5000);
        //                DeActivateWindow(5);
        //                DeActivateWindow(200);
        //                DeActivateWindow(100);
        //                DeActivateWindow(6000);

        //                return;
        //            }

        //            if (currentLayoutDocument.Content != null)
        //            {

        //                currentBorder = (Border)currentLayoutDocument.Content;
        //                strSource1 = (string)currentBorder.Tag;

        //                if (strSource == strSource1)
        //                {
        //                    UnHideAllAnchorWindows();
        //                    return;
        //                }
        //                else
        //                {
        //                    strSource = strSource1;
        //                }
        //                if (currentBorder.Tag != null)
        //                {
        //                    strSource = (string)currentBorder.Tag;
        //                    if ((strSource != null) && (strSource.Trim().Length > 0))
        //                    {

        //                        if (SelectHelper.WorkflowDictionary.ContainsKey(strSource))
        //                        {
        //                            CustomWfDesigner.Instance = SelectHelper.WorkflowDictionary[strSource];
        //                            SelectHelper._wfDesigner = SelectHelper.WorkflowDictionary[strSource];
        //                            SelectHelper.CurrentProcessName = currentLayoutDocument.Title;
        //                            if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(currentLayoutDocument.Title))
        //                            {
        //                                SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[currentLayoutDocument.Title];
        //                            }
        //                            _currentWorkflowFile = currentLayoutDocument.ToolTip.ToString();
        //                            CustomWfDesigner.Instance.View.UpdateLayout();
        //                            currentBorder.Child = CustomWfDesigner.Instance.View;
        //                            WfPropertyBorder.Child = CustomWfDesigner.Instance.PropertyInspectorView;
        //                            WfOutlineBorder.Child = CustomWfDesigner.Instance.OutlineView;

        //                            //Need to create new list new List<ModelItem> and store list in Dictionary
        //                            // treeView1.DataContext = new List<ModelItem> { wd.Context.Services.GetService<ModelService>().Root };

        //                            if ((!string.IsNullOrEmpty(SelectHelper._currentscrapfile)) && (SelectHelper._currentscrapfile != strSource))
        //                            {
        //                                ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(3);
        //                            }
        //                            EnableDebugging();
        //                        }
        //                    }
        //                }
        //            }

        //            if (strSource != strSource1 && strSource != null)
        //            {
        //                HideAllAnchorWindows();
        //                DeActivateWindow(4000);
        //                DeActivateWindow(5000);
        //                DeActivateWindow(5);
        //                DeActivateWindow(200);
        //                DeActivateWindow(100);
        //                DeActivateWindow(6000);
        //            }
        //            else
        //            {
        //                UnHideAllAnchorWindows();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
        //}

    private void DisableMenuButtons()
        {
            ButtonWorkflowRun.IsEnabled = false;
            ButtonWorkflowStop.IsEnabled = false;
            ButtonDebug.IsEnabled = false;


            ButtonWorkflowNew.IsEnabled = false;
            ButtonWorkflowNewSeq.IsEnabled = false;
            ButtonWorkflowNewFC.IsEnabled = false;
            ButtonWorkflowNewSM.IsEnabled = false;


            //ButtonWorkflowNew.IsEnabled = true;
            //ButtonWorkflowNewSeq.IsEnabled = true;
            //ButtonWorkflowNewFC.IsEnabled = true;
            //ButtonWorkflowNewSM.IsEnabled = true;


            ButtonWorkflowSave.IsEnabled = false;


            ButtonWorkflowImport.IsEnabled = true;
            ButtonWorkflowExport.IsEnabled = false;
            ButtonWorkflowPublish.IsEnabled = false;
        }


        private void EnableMenuButtons()
        {
            ButtonWorkflowRun.IsEnabled = true;
            ButtonWorkflowStop.IsEnabled = true;
            ButtonDebug.IsEnabled = true;


            ButtonWorkflowNew.IsEnabled = true;
            ButtonWorkflowNewSeq.IsEnabled = true;
            ButtonWorkflowNewFC.IsEnabled = true;
            ButtonWorkflowNewSM.IsEnabled = true;


            //ButtonWorkflowNew.IsEnabled = true;
            //ButtonWorkflowNewSeq.IsEnabled = true;
            //ButtonWorkflowNewFC.IsEnabled = true;
            //ButtonWorkflowNewSM.IsEnabled = true;


            ButtonWorkflowSave.IsEnabled = true;


            ButtonWorkflowImport.IsEnabled = true;
            ButtonWorkflowExport.IsEnabled = true;
            ButtonWorkflowPublish.IsEnabled = true;


        }














        void layoutDocument_IsActiveChanged(object sender, EventArgs e)
        {
            try
            {
                string strSource1 = string.Empty;



                if (sender.GetType().Name == "LayoutDocument")
                {
                    currentLayoutDocument = (LayoutDocument)sender;



                    if (currentLayoutDocument.ContentId == 1000) //Dashboard
                    {
                        HideAllAnchorWindows();
                        DeActivateWindow(4000);
                        DeActivateWindow(5000);
                        DeActivateWindow(5);
                        DeActivateWindow(200);
                        DeActivateWindow(100);
                        DeActivateWindow(6000);
                        DisableMenuButtons();
                        return;
                    }



                    if (currentLayoutDocument.Content != null)
                    {
                        EnableMenuButtons();
                        currentBorder = (Border)currentLayoutDocument.Content;
                        strSource1 = (string)currentBorder.Tag;



                        if (strSource == strSource1)
                        {
                            UnHideAllAnchorWindows();
                            return;
                        }
                        else
                        {
                            strSource = strSource1;
                        }
                        if (currentBorder.Tag != null)
                        {
                            strSource = (string)currentBorder.Tag;
                            if ((strSource != null) && (strSource.Trim().Length > 0))
                            {



                                if (SelectHelper.WorkflowDictionary.ContainsKey(strSource))
                                {
                                    CustomWfDesigner.Instance = SelectHelper.WorkflowDictionary[strSource];
                                    SelectHelper._wfDesigner = SelectHelper.WorkflowDictionary[strSource];
                                    SelectHelper.CurrentProcessName = currentLayoutDocument.Title;
                                    if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(currentLayoutDocument.Title))
                                    {
                                        SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[currentLayoutDocument.Title];
                                    }
                                    _currentWorkflowFile = currentLayoutDocument.ToolTip.ToString();
                                    CustomWfDesigner.Instance.View.UpdateLayout();
                                    currentBorder.Child = CustomWfDesigner.Instance.View;
                                    WfPropertyBorder.Child = CustomWfDesigner.Instance.PropertyInspectorView;
                                    WfOutlineBorder.Child = CustomWfDesigner.Instance.OutlineView;



                                    //Need to create new list new List<ModelItem> and store list in Dictionary
                                    // treeView1.DataContext = new List<ModelItem> { wd.Context.Services.GetService<ModelService>().Root };



                                    if ((!string.IsNullOrEmpty(SelectHelper._currentscrapfile)) && (SelectHelper._currentscrapfile != strSource))
                                    {
                                        ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(3);
                                    }
                                    EnableDebugging();
                                }
                            }
                        }
                    }



                    if (strSource != strSource1 && strSource != null)
                    {
                        HideAllAnchorWindows();
                        DeActivateWindow(4000);
                        DeActivateWindow(5000);
                        DeActivateWindow(5);
                        DeActivateWindow(200);
                        DeActivateWindow(100);
                        DeActivateWindow(6000);
                    }
                    else
                    {
                        UnHideAllAnchorWindows();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private LayoutDocumentPane ClearAvalonDocTabs()
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();

                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutPanel)
                    {
                        LayoutPanel lp = (LayoutPanel)item;
                        Orientation o = lp.Orientation;
                        if (o.ToString() == "Horizontal")
                        {
                            if (lp.Children.Count > 0)
                            {
                                foreach (var itemIn1 in lp.Children)
                                {
                                    if (itemIn1 is LayoutDocumentPaneGroup)
                                    {
                                        LayoutDocumentPaneGroup ldpg = (LayoutDocumentPaneGroup)itemIn1;
                                        if (ldpg.Children.Count > 0)
                                        {
                                            foreach (var itemin2 in ldpg.Children)
                                            {
                                                bool isDocumentpane = true;
                                                LayoutDocumentPane la = (LayoutDocumentPane)(ldpg.Children[0]);
                                                foreach (var itemin3 in la.Children)
                                                {
                                                    if (itemin3 is LayoutAnchorable)
                                                    {
                                                        LayoutAnchorable itemAL = (LayoutAnchorable)itemin3;
                                                        if ((itemAL.ContentId == 100) || (itemAL.ContentId == 200))
                                                        {
                                                            isDocumentpane = false;
                                                            break;
                                                        }

                                                    }
                                                }
                                                if (isDocumentpane == true)
                                                {
                                                    la.Children.Clear();
                                                    la.Children.Add(ldashboard);
                                                    la.Children[0].IsSelected = true;
                                                    return la;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return null;
        }
        public void OpenBlankProject(object sender, NewProjectEventArgs e)
        {
            try
            {
                // ClearAvalonDocTabs();
                string strNewWorkflow = e.Path + Path.DirectorySeparatorChar + "Main.xaml";
                int iCount = 0;
                while ((true) && (iCount < 100))
                {
                    iCount = iCount + 1;
                    if (File.Exists(strNewWorkflow))
                    {
                        strNewWorkflow = e.Path + Path.DirectorySeparatorChar + "Main" + iCount + ".xaml";

                    }
                    else
                    {
                        if (e.projectType == ProjectType.sequence)
                        {
                            File.Copy(_defaultSequence, strNewWorkflow);
                        }
                        else if (e.projectType == ProjectType.workflow)
                        {
                            File.Copy(_defaultWorkflow, strNewWorkflow);
                        }
                        else if (e.projectType == ProjectType.statemachine)
                        {
                            File.Copy(_defaultSM, strNewWorkflow);
                        }
                        break;
                    }
                }
                LoadProject(strNewWorkflow);
                //  AddWorkFlow(strNewWorkflow);
                //Ribbon.Title = DirPath.Substring(DirPath.LastIndexOf("\\") + 1);

                //hide
                UnHideAllAnchorWindows();
                ActivateWindow(6000);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        public void OpenProject(object sender, NewProjectEventArgs e)
        {
            try
            {
                if (e.projectType == ProjectType.openproject)
                {

                    if (e.Path.ToLower().Contains(".aut") || e.Path.ToLower().Contains(".xaml"))
                    {
                        LoadProject(e.Path);
                    }
                    else
                    {
                        LoadProject(e.Path, true);
                    }
                }
                else if (e.projectType == ProjectType.openexplorer)
                {
                    if ((ProjectLocation == null) || (ProjectLocation == string.Empty))
                    {
                        Process.Start(Environment.CurrentDirectory);
                    }
                    else
                    {
                        Process.Start(ProjectLocation);
                    }
                }
                //jhide
                UnHideAllAnchorWindows();
                ActivateWindow(6000);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void OpenWorkflowFile(object sender, OpenXamlFileEventArgs e)
        {
            OpenFile(e.XamlFileNameWithPath);
        }

        //private void OpenFile(string FileName, LayoutDocumentPane laDocuments = null)
        //{
        //    try
        //    {
        //        Mouse.OverrideCursor = Cursors.Wait;
        //        string onlyFileName = Path.GetFileName(FileName);
        //        string dirPath = Path.GetDirectoryName(FileName);
        //        if (onlyFileName.Contains(".xaml"))
        //        {
        //            if (CheckDuplicate(FileName))
        //            {
        //                AddWorkFlow(FileName, laDocuments);
        //                // Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf("\\") + 1);
        //                //Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        //            }
        //        }
        //        if (onlyFileName.Contains(".aut"))
        //        {
        //            // Ribbon.Title = onlyFileName.Remove(onlyFileName.LastIndexOf(".aut"));
        //            Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        //        }
        //        if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
        //        {
        //            SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile];

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
        //    finally
        //    {
        //        Mouse.OverrideCursor = null;
        //    }
        //}

    private void OpenFile(string FileName, LayoutDocumentPane laDocuments = null)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                string onlyFileName = Path.GetFileName(FileName);
                string dirPath = Path.GetDirectoryName(FileName);
                string[] XamlFiles = Directory.GetFiles(dirPath, "*.xaml", SearchOption.AllDirectories);
                if (onlyFileName.Contains(".xaml"))
                {
                    if (CheckDuplicate(FileName))
                    {
                        if (!XamlFiles.Contains(FileName))
                        {
                            FileName = XamlFiles[0];
                        }
                        AddWorkFlow(FileName, laDocuments);
                        // Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf("\\") + 1);
                        //Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    }
                }
                if (onlyFileName.Contains(".aut"))
                {
                    if (CheckDuplicate(FileName))
                    {
                        string MainPath = dirPath + "\\Main.xaml";
                        if (!XamlFiles.Contains(MainPath))
                        {
                            FileName = XamlFiles[0];
                        }
                        else
                        {
                            FileName = MainPath;
                        }
                        AddWorkFlow(FileName, laDocuments);
                        // Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf("\\") + 1);
                        //Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    }
                    // Ribbon.Title = onlyFileName.Remove(onlyFileName.LastIndexOf(".aut"));
                    Ribbon.Title = "Current Project folder:" + dirPath.Substring(dirPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                }
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                {
                    SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile];


                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }





        private bool CheckDuplicate(string FileName)
        {
            try
            {
                // FileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutDocumentPane)
                    {
                        LayoutDocumentPane la = (LayoutDocumentPane)item;
                        try
                        {
                            if (la.Children.Any(p => p.ToolTip.ToString() == FileName))
                            {
                                la.Children.First(p => p.ToolTip.ToString() == FileName).IsSelected = true;
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        catch (Exception)
                        {
                            //return false;
                            //error at loading if tooltipis not set
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return true;
        }

        public void LoadProject(string FullFileName, bool IsDirectory = false)
        {
            string ProjectPath = string.Empty;
            try
            {
                dashboard.AddToRecentTable(FullFileName); //to add to recent projects list in dashboard

                LayoutDocumentPane laDocuments = ClearAvalonDocTabs();

                OpenFile(FullFileName, laDocuments);
                if (IsDirectory == false)
                {

                    //  projectName = FullFileName;
                    ProjectPath = System.IO.Path.GetDirectoryName(FullFileName);

                    projectName = ProjectPath;
                    ProjectLocation = ProjectPath;
                }
                else
                {
                    ProjectPath = FullFileName;
                    ProjectLocation = FullFileName;
                }

                SelectHelper.ProjectLocation = ProjectPath;
                //Task.Run(() =>
                //{
                ReloadTree();
                //LoadOutline();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void ReloadTree()
        {
            IEnumerable<ILayoutElement> LayoutDocuments = null;
            ThreadInvoker.Instance.RunByUiThread(() =>
            {
                LayoutDocuments = this.dockManager.Layout.Descendents();
                //   string ProjectPath = FullFileName.Substring(0, FullFileName.LastIndexOf("\\"));

                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable la = (LayoutAnchorable)item;

                        if (la.ContentId == 6000)
                        {
                            ctr = new CommonTreeView();

                            ctr.ProjectOrLibraryPath = ProjectLocation;
                            la.Content = ctr;
                            la.IsSelected = true;
                            break;
                        }
                        //if (la.ContentId == 6030)
                        //{
                        //    ctr = new CommonTreeView();
                            
                        //    ctr.ProjectOrLibraryPath = ProjectLocation;
                        //    la.Content = ctr;
                        //    la.IsSelected = true;
                        //    break;
                        //}

                    }
                }

            });

        }

        private void LoadOutline()
        {
            //try
            //{
            //    if (SelectHelper._wfDesigner != null)
            //        Outline.Content = SelectHelper._wfDesigner.OutlineView;
            //}
            //catch (Exception ex)
            //{

            //}
            //  Outline.Content = SelectHelper._wfDesigner.OutlineView;
            try {
                
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                ThreadInvoker.Instance.RunByUiThread(() =>
                {

                    LayoutDocuments = this.dockManager.Layout.Descendents();


                    foreach (ILayoutElement item in LayoutDocuments)
                    {
                        if (item is LayoutAnchorable)
                        {
                            LayoutAnchorable la = (LayoutAnchorable)item;

                            if (la.ContentId == 6030)
                            {
                                //ctr = new CommonTreeView();

                                //ctr.ProjectOrLibraryPath = ProjectLocation;
                                SelectHelper._wfDesigner = SelectHelper.WorkflowDictionary[currentWorkflowFile]; ;
                                la.Content = SelectHelper._wfDesigner.OutlineView;
                            
                            //    la.Content = CustomWfDesigner.Instance.OutlineView;
                                la.IsSelected = true;
                                break;
                            }


                        }
                    }

                });

            }
            catch (Exception)
            {

            }


            //try
            //{
            //    string strLibraryPath = ConfigurationManager.AppSettings["LibraryPath"];
            //    if ((string.IsNullOrEmpty(strLibraryPath)) || (strLibraryPath.Trim().Length == 0))
            //    {
            //        strLibraryPath = Path.Combine(SelectHelper.CurrentExecutablepath, "Library");
            //    }
            //    IEnumerable<ILayoutElement> LayoutDocuments = null;
            //    LayoutDocuments = this.dockManager.Layout.Descendents();
            //    foreach (ILayoutElement item in LayoutDocuments)
            //    {
            //        if (item is LayoutAnchorable)
            //        {
            //            LayoutAnchorable la = (LayoutAnchorable)item;
            //            if (la.ContentId == 6030)
            //            {
            //                CommonTreeView ctr = new CommonTreeView();
            //                // ctr.LibraryPath = @"pack://application:,/Library";
            //                ctr.ProjectOrLibraryPath = strLibraryPath;
            //                la.Content = ctr;
            //                la.IsSelected = true;
            //                break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Logger.LogData(ex.Message, LogLevel.Error);
            //}
        }
        private void LoadLibrary(string FullDirectoryName)
        {
            try
            {
                string strLibraryPath = ConfigurationManager.AppSettings["LibraryPath"];
                if ((string.IsNullOrEmpty(strLibraryPath)) || (strLibraryPath.Trim().Length == 0))
                {
                    strLibraryPath = Path.Combine(SelectHelper.CurrentExecutablepath, "Library");
                }
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();
                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutAnchorable)
                    {
                        LayoutAnchorable la = (LayoutAnchorable)item;
                        if (la.ContentId == 4000)
                        {
                            CommonTreeView ctr = new CommonTreeView();
                            // ctr.LibraryPath = @"pack://application:,/Library";
                            ctr.ProjectOrLibraryPath = strLibraryPath;
                            la.Content = ctr;
                            la.IsSelected = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        #endregion

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
     
        private void Ribbon_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        private bool CanLoadLayout(object parameter)
        {
            return File.Exists(@".\AvalonDock.Layout.config");
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //code for saving the file on exit-----------
                //MessageBox.Show(currentLayoutDocument.IsActive.ToString());
                // MessageBox.Show(currentLayoutDocument.IsVisible.ToString());
                // MessageBox.Show(currentLayoutDocument.IsLastFocusedDocument.ToString());
                //  MessageBox.Show(currentLayoutDocument.IsSelected.ToString());
               // if (currentLayoutDocument.IsActive)
                    if (currentWorkflowFile != "" && currentLayoutDocument.IsActive)
                {

                    //workflow exists
                    if (MessageBox.Show("Do you want to save before exit?",
    "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        //save work flow
                     //   MessageBoxResult result = MessageBox.Show("User wants to save his workflow");
                        CustomWfDesigner.Instance.Save(_currentWorkflowFile);
                       
                    }
                    else
                    {
                        //workflow exists but user does not want to save  
                       // MessageBoxResult result = MessageBox.Show("User does not want to save his workflow");

                    }

                }
                else
                {  //workflow does not exist  
                    //MessageBoxResult result = MessageBox.Show("Okay to close because there is no active workflow");
                }

                //hide
                HideAllAnchorWindows();

                //-------------------------------------
                dashboard.CreateRecentTableFile();
                ClearAvalonDocTabs();
                var serializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
                //serializer.Serialize(@".\AvalonDock.config");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            //mainDashBoard.Visibility = Visibility.Visible;
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
            //dgInfoErrorWarnings.ItemsSource = Logger.Log.DatatableLog.DefaultView;
        }

        private void btnImportActivity_Click(object sender, RoutedEventArgs e)
        {
            AddRemoveActivities addRemoveActivities = new AddRemoveActivities();
            if (addRemoveActivities.ShowDialog() == true)
            {
                InitializeActivitiesToolbox();
            }

        }
        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {

            _wfToolbox.CategoryItemStyle = new System.Windows.Style(typeof(TreeViewItem))
            {
                Setters = {
                                new Setter(TreeViewItem.IsExpandedProperty, true)
                }
            };
            _wfToolbox.UpdateDefaultStyle();
            _wfToolbox.UpdateLayout();
            WfToolboxBorder.Child = null;
            WfToolboxBorder.Child = _wfToolbox;
        }
        private void btnCollapseActivity_Click(object sender, RoutedEventArgs e)
        {

            //ctr.Collapse();
            _wfToolbox.CategoryItemStyle = new System.Windows.Style(typeof(TreeViewItem))
            {
                Setters = {
                    new Setter(TreeViewItem.IsExpandedProperty, false)
                }
            };
            _wfToolbox.UpdateDefaultStyle();
            _wfToolbox.UpdateLayout();
            WfToolboxBorder.Child = null;
            WfToolboxBorder.Child = _wfToolbox;
        }

        private void ConsoleOutPut_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }


        //#region "Debug
        ////Dictionary<object, SourceLocation> wfElementToSourceLocationMap = null;
        //WorkflowEntity workflowEntity = null;
        //private void EnableDebugging()
        //{
        //    try
        //    {
        //        this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
        //        if (SelectHelper.WorkflowEntityDictionary.ContainsKey(_currentWorkflowFile))
        //        {
        //            workflowEntity = (WorkflowEntity)SelectHelper.WorkflowEntityDictionary[_currentWorkflowFile];
        //            _wfApp = workflowEntity._wfApp;
        //        }

        //        //MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
        //        //DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;

        //        ////_wfApp = new WorkflowApplication(activityExecute);
        //        ////try
        //        ////{
        //        ////    _wfApp.Extensions.Add(_executionLog);
        //        ////}
        //        ////catch (Exception ex) { }
        //        ////_wfApp.Completed = WfExecutionCompleted;
        //        //Dictionary<object, SourceLocation> wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService(activityExecute);
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log.Logger.LogData("Error in EnableDebugging  in MainWindow. Details: " + ex.Message, LogLevel.Error);
        //    }

        //}

        //private void CmdWorkflowDebug(object sender, ExecutedRoutedEventArgs e)
        //{

        //    try
        //    {
        //        if (CustomWfDesigner.Instance == null)
        //            return;
        //        //get workflow source from designer
        //        // Logger.Log.DatatableLog.Rows.Clear();
        //        CustomWfDesigner.Instance.Save(_currentWorkflowFile);
        //        Logger.Log.Logger.DatatableLog.Clear();
        //        Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = string.Empty;


        //        CustomWfDesigner.Instance.Flush();
        //      //  MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
        //       // DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
        //        //configure workflow application
        //        consoleExecutionLog.Text = String.Empty;
        //        //consoleOutput.Text = String.Empty;
        //        this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
        //        if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
        //        {
        //            SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = new RuntimeApplicationHelper();
        //            SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile];
        //        }
        //        //to clean stored runtime objects from previous run
        //        if (SelectHelper.WorkflowEntityDictionary.ContainsKey(_currentWorkflowFile))
        //        {
        //            workflowEntity = (WorkflowEntity)SelectHelper.WorkflowEntityDictionary[_currentWorkflowFile];
        //            SelectHelper._timerExecution = _timer;
        //            this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
        //            workflowEntity.InitStartDebuggingForWorkflow();
        //            workflowEntity._wfApp.Completed = WfExecutionCompleted;
        //            workflowEntity.dispatcher = this.Dispatcher;
        //            workflowEntity._wfApp.Completed = WfExecutionCompleted;
        //            workflowEntity._currentWorkflowFile = _currentWorkflowFile;
        //            workflowEntity.consoleExecutionLog = consoleExecutionLog;
        //            workflowEntity.dgInfoErrorWarnings = dgInfoErrorWarnings;

        //            _wfApp = workflowEntity._wfApp;
        //            workflowEntity.StartDebuggingForWorkflow();
        //        }
        //        else
        //        {
        //            Log.Logger.LogData("Debug service is not loaded for workflow", LogLevel.Error);

        //        }
        //        //else
        //        //{
        //        //    workflowEntity = new WorkflowEntity();
        //        //    workflowEntity.DebuggerService = this.DebuggerService;
        //        //    workflowEntity.dispatcher = this.Dispatcher;
        //        //    workflowEntity._wfApp.Completed = WfExecutionCompleted;
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData("Error in method cmdWorkflowDebug. Details: " + ex.Message, LogLevel.Error);
        //    }
        //}



        ////Re-hosted debugging - F5/F9 behavior
        //protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        //{

        //    if (e.Key == Key.F9)
        //    {
        //        ModelItem mi = SelectHelper._wfDesigner.Context.Items.GetValue<Selection>().PrimarySelection;
        //        Activity a = mi.GetCurrentValue() as Activity;

        //        if (a != null)
        //        {
        //            SourceLocation srcLoc = workflowEntity.designerSourceLocationMapping[a];
        //            if (!workflowEntity.breakpointList.Contains(srcLoc))
        //            {
        //                // SelectHelper._wfDesigner.Context.Services.GetService<IDesignerDebugView>().UpdateBreakpoint(srcLoc, BreakpointTypes.Bounded | BreakpointTypes.Enabled);
        //                workflowEntity.DebuggerService.UpdateBreakpoint(srcLoc, BreakpointTypes.Bounded | BreakpointTypes.Enabled);
        //                workflowEntity.breakpointList.Add(srcLoc);
        //            }
        //            else
        //            {
        //                // SelectHelper._wfDesigner.Context.Services.GetService<IDesignerDebugView>().UpdateBreakpoint(srcLoc, BreakpointTypes.None);
        //                workflowEntity.DebuggerService.UpdateBreakpoint(srcLoc, BreakpointTypes.Bounded | BreakpointTypes.Enabled);
        //                workflowEntity.breakpointList.Remove(srcLoc);
        //            }
        //        }
        //    }
        //    else if (e.Key == Key.F8)
        //    {
        //        workflowEntity.resumeRuntimeFromHost.Set();
        //        workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
        //    }
        //    else if (e.SystemKey == Key.X)

        //    {
        //        ActivateWindow(5000); //toolbox
        //    }
        //    else if (e.Key == Key.F4)
        //    {
        //        ActivateWindow(5);//properties window
        //    }
        //    else if (e.SystemKey == Key.P)

        //    {
        //        ActivateWindow(6000);//Project window
        //    }
        //    else if (e.SystemKey == Key.L)
        //    {
        //        ActivateWindow(4000);//Library window
        //    }
        //}
        //private void CmdWorkflowResumeBreakPoint(object sender, ExecutedRoutedEventArgs e)
        //{
        //    workflowEntity.resumeRuntimeFromHost.Set();
        //    workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
        //}
        //#endregion "Debug

        #region "Debug
        //Dictionary<object, SourceLocation> wfElementToSourceLocationMap = null;
        WorkflowEntity workflowEntity = null;
        private void EnableDebugging()
        {
            try
            {
                this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
                if (SelectHelper.WorkflowEntityDictionary.ContainsKey(_currentWorkflowFile))
                {
                    workflowEntity = (WorkflowEntity)SelectHelper.WorkflowEntityDictionary[_currentWorkflowFile];
                    _wfApp = workflowEntity._wfApp;
                }

                //MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                //DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;

                ////_wfApp = new WorkflowApplication(activityExecute);
                ////try
                ////{
                ////    _wfApp.Extensions.Add(_executionLog);
                ////}
                ////catch (Exception ex) { }
                ////_wfApp.Completed = WfExecutionCompleted;
                //Dictionary<object, SourceLocation> wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService(activityExecute);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in EnableDebugging  in MainWindow. Details: " + ex.Message, LogLevel.Error);
            }

        }

        private void CmdWorkflowDebug(object sender, ExecutedRoutedEventArgs e)
        {

            try
            {
                if (CustomWfDesigner.Instance == null)
                    return;
                //get workflow source from designer
                // Logger.Log.DatatableLog.Rows.Clear();
                CustomWfDesigner.Instance.Save(_currentWorkflowFile);
                Logger.Log.Logger.DatatableLog.Clear();
                Logger.Log.Logger.DatatableLog.DefaultView.RowFilter = string.Empty;


                CustomWfDesigner.Instance.Flush();
                //  MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(CustomWfDesigner.Instance.Text));
                // DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
                //configure workflow application
                consoleExecutionLog.Text = String.Empty;
                //consoleOutput.Text = String.Empty;
                this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
                if (SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(_currentWorkflowFile))
                {
                    SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile] = new RuntimeApplicationHelper();
                    SelectHelper.CurrentRuntimeApplicationHelper = SelectHelper.RuntimeApplicationHelperDictionary[_currentWorkflowFile];
                }
                //to clean stored runtime objects from previous run
                if (SelectHelper.WorkflowEntityDictionary.ContainsKey(_currentWorkflowFile))
                {
                    workflowEntity = (WorkflowEntity)SelectHelper.WorkflowEntityDictionary[_currentWorkflowFile];
                    SelectHelper._timerExecution = _timer;
                    this.DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
                    workflowEntity.InitStartDebuggingForWorkflow();
                    workflowEntity._wfApp.Completed = WfExecutionCompleted;
                    workflowEntity.dispatcher = this.Dispatcher;
                    workflowEntity._wfApp.Completed = WfExecutionCompleted;
                    workflowEntity._currentWorkflowFile = _currentWorkflowFile;
                    workflowEntity.consoleExecutionLog = consoleExecutionLog;
                    workflowEntity.dgInfoErrorWarnings = dgInfoErrorWarnings;

                    _wfApp = workflowEntity._wfApp;
                    workflowEntity.StartDebuggingForWorkflow();
                }
                else
                {
                    Log.Logger.LogData("Debug service is not loaded for workflow", LogLevel.Error);

                }
                //else
                //{
                //    workflowEntity = new WorkflowEntity();
                //    workflowEntity.DebuggerService = this.DebuggerService;
                //    workflowEntity.dispatcher = this.Dispatcher;
                //    workflowEntity._wfApp.Completed = WfExecutionCompleted;
                //}

            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Error in method cmdWorkflowDebug. Details: " + ex.Message, LogLevel.Error);
            }
        }



        //Re-hosted debugging - F5/F9 behavior
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.F9)
            {

                if (SelectHelper.WorkflowEntityDictionary.ContainsKey(_currentWorkflowFile))
                {
                    workflowEntity = (WorkflowEntity)SelectHelper.WorkflowEntityDictionary[_currentWorkflowFile];
                    // _wfApp = workflowEntity._wfApp;
                }



                ModelItem mi = SelectHelper._wfDesigner.Context.Items.GetValue<Selection>().PrimarySelection;
                Activity a = mi.GetCurrentValue() as Activity;

                if (a != null)
                {
                    // SourceLocation srcLoc = workflowEntity.designerSourceLocationMapping[a];
                    SourceLocation srcLoc = workflowEntity.designerSourceLocationMapping[a];
                    if (!workflowEntity.breakpointList.Contains(srcLoc))
                    {
                        // SelectHelper._wfDesigner.Context.Services.GetService<IDesignerDebugView>().UpdateBreakpoint(srcLoc, BreakpointTypes.Bounded | BreakpointTypes.Enabled);
                        workflowEntity.DebuggerService.UpdateBreakpoint(srcLoc, BreakpointTypes.Bounded | BreakpointTypes.Enabled);
                        workflowEntity.breakpointList.Add(srcLoc);
                    }
                    else
                    {
                        // SelectHelper._wfDesigner.Context.Services.GetService<IDesignerDebugView>().UpdateBreakpoint(srcLoc, BreakpointTypes.None);
                        workflowEntity.DebuggerService.UpdateBreakpoint(srcLoc, BreakpointTypes.None);
                        workflowEntity.breakpointList.Remove(srcLoc);
                    }
                }
            }
            else if (e.Key == Key.F8)    //continue
            {
                string debugtype = "continue";
                workflowEntity.DebugType = debugtype;
                //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
                workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
            }
            else if (e.Key == Key.F11)    //step into
            {
                string debugtype = "stepinto";
                workflowEntity.DebugType = debugtype;
                //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
                workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
            }
            else if (e.Key == Key.F7)    //step into
            {
                string debugtype = "stepover";
                workflowEntity.DebugType = debugtype;
                //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
                // workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
                //while (workflowEntity.isParent!=true)
                //{
                //    workflowEntity.resumeRuntimeFromHost.Set();
                //}
                workflowEntity.resumeRuntimeFromHost.Set();

            }
            else if (e.SystemKey == Key.X)

            {
                ActivateWindow(5000); //toolbox
            }
            else if (e.Key == Key.F4)
            {
                ActivateWindow(5);//properties window
            }
            else if (e.SystemKey == Key.P)

            {
                ActivateWindow(6000);//Project window
            }
            else if (e.SystemKey == Key.L)
            {
                ActivateWindow(4000);//Library window
            }
        }
        private void CmdWorkflowResumeBreakPoint(object sender, ExecutedRoutedEventArgs e)
        {
            //  workflowEntity.resumeRuntimeFromHost.Set();
            //  workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one

            string debugtype = "continue";
            workflowEntity.DebugType = debugtype;
            //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
            workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one

        }
        private void CmdWorkflowStepInto(object sender, ExecutedRoutedEventArgs e)
        {
            string debugtype = "stepinto";
            workflowEntity.DebugType = debugtype;
            //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
            workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
        }

        private void CmdWorkflowStepOver(object sender, ExecutedRoutedEventArgs e)
        {
            string debugtype = "stepover";
            workflowEntity.DebugType = debugtype;
            //   workflowEntity.resumeRuntimeFromHost.Set();  // use of set depends on how many times ShowDebug() is called.
            // workflowEntity.resumeRuntimeFromHost.Set(); //Need to set two times as its not working with one
            //while (workflowEntity.isParent!=true)
            //{
            //    workflowEntity.resumeRuntimeFromHost.Set();
            //}
            workflowEntity.resumeRuntimeFromHost.Set();
        }
        #endregion "Debug

        private void StackPanel_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Ribbon_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            RowDefinitionCollection rowdefcollection = null;
            int tabno = ((Ribbon)sender).SelectedIndex;
            switch (tabno)
            {
                case 0:
                    rowdefcollection = wfGrid.RowDefinitions;
                    rowdefcollection.ElementAt(0).Height = new GridLength(25);
                    HomeTabHeader.Background = Brushes.White;
                    HomeTabHeader.Foreground = SystemColors.HighlightBrush;
                    //other tabs
                    WorkflowTabHeader.Background = SystemColors.HighlightBrush;
                    WorkflowTabHeader.Foreground = Brushes.White;
                    DebugTabHeader.Background = SystemColors.HighlightBrush;
                    DebugTabHeader.Foreground = Brushes.White;
                    if (!IsFirstLaunch)
                    {
                        GetDashboard();
                    }
                    break;
                case 1:
                   // workflowfalseribbon.Width = SystemParameters.MaximizedPrimaryScreenWidth - 600;
                    rowdefcollection = wfGrid.RowDefinitions;
                    rowdefcollection.ElementAt(0).Height = new GridLength(95);
                    WorkflowTabHeader.Background = Brushes.White;
                    WorkflowTabHeader.Foreground = SystemColors.HighlightBrush;
                    //other tabs
                    HomeTabHeader.Background = SystemColors.HighlightBrush;
                    HomeTabHeader.Foreground = Brushes.White;
                    DebugTabHeader.Background = SystemColors.HighlightBrush;
                    DebugTabHeader.Foreground = Brushes.White;
                    break;
                case 2:
                   // debugfalseribbon.Width = SystemParameters.MaximizedPrimaryScreenWidth - 125;
                    rowdefcollection = wfGrid.RowDefinitions;
                    rowdefcollection.ElementAt(0).Height = new GridLength(95);
                    DebugTabHeader.Background = Brushes.White;
                    DebugTabHeader.Foreground = SystemColors.HighlightBrush;
                    //other tabs
                    WorkflowTabHeader.Background = SystemColors.HighlightBrush;
                    WorkflowTabHeader.Foreground = Brushes.White;
                    HomeTabHeader.Background = SystemColors.HighlightBrush;
                    HomeTabHeader.Foreground = Brushes.White;
                    break;
                case 3:
                    //theFrame.Navigate(new StarterPage(theFrame));
                    //var window1 = new StarterWindow();
                    //window1.ShowDialog();
                    //Page a = new StarterPage();
                    //a.
                    //this.NavigationService.GoForward();
                    //or
                    //navigator = new NavigationService();
                    //this.NavigationService.Navigate("StarterWindow.xaml");
                    //NavigationService ns = NavigationService.GetNavigationService(this);
                    //ns.Navigate("StarterWindow.xaml");
                    //MainPage.NavigationService.Navigate(new Uri("StarterPage.xaml", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }


        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogOpen = new OpenFileDialog();
                dialogOpen.Title = "Open Workflow";
                //dialogOpen.Filter = "Project|*.aut|Workflows (.xaml)|*.xaml";
                dialogOpen.Filter = "Project|*.aut";



                if (dialogOpen.ShowDialog() == true)
                {
                    // using (var file = new StreamReader(dialogOpen.FileName, true))
                    //{
                    NewProjectEventArgs ne = new NewProjectEventArgs();
                    ne.Path = dialogOpen.FileName;
                    ne.projectType = ProjectType.openproject;
                    ne.projectFullName = dialogOpen.FileName;
                    OpenProject(sender, ne);
                    // }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }



        private void btnOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewProjectEventArgs ne = new NewProjectEventArgs();
                ne.projectType = ProjectType.openexplorer;
                OpenProject(sender, ne);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void CmdWorkflowXPath(object sender, ExecutedRoutedEventArgs e)
        {

            //GetWinXPath window = new GetWinXPath();
            //window.Show();
           
            
                ThreadInvoker.Instance.RunByUiThread(() =>
            {
                Bot.Activity.WinDriverPlugin.MainWindow uirecorder = new Bot.Activity.WinDriverPlugin.MainWindow();
                uirecorder.Show();
            });
        }
            private void ReloadDesigner(int action = 0)
        {

            //lock (obj)
            //{
           
            if (string.IsNullOrEmpty(currentWorkflowFile))
                return;

            if (action == 1)
            {
                if (File.Exists(currentWorkflowFile))
                {
                    File.Delete(currentWorkflowFile);
                }
            }

            if (!string.IsNullOrEmpty(currentWorkflowFile))
            {
                if (SelectHelper.WorkflowDictionary.ContainsKey(currentWorkflowFile))
                {
                    WorkflowDesigner _wfDesigner = SelectHelper.WorkflowDictionary[currentWorkflowFile];
                   
                   
                        _wfDesigner.Save(currentWorkflowFile);
                   

                    _wfDesigner = new WorkflowDesigner();
                    _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
                    _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;

                    //associates all of the basic activities with their designers
                    new DesignerMetadata().Register();
                    _wfDesigner.Load(currentWorkflowFile);

                    if (SelectHelper.WorkflowDictionary.ContainsKey(currentWorkflowFile))
                    {
                        SelectHelper.WorkflowDictionary[currentWorkflowFile] = _wfDesigner;

                    }

                    if (currentWorkflowFile == SelectHelper._currentworkflowfile)
                    {
                        _wfDesigner.View.UpdateLayout();
                        _wfDesigner.OutlineView.UpdateLayout();
                        System.Windows.Controls.Border WfDesignerBorder = (System.Windows.Controls.Border)SelectHelper.Border;
                        WfDesignerBorder.Child = _wfDesigner.View;
                        SelectHelper._wfPropertyBorder.Child = _wfDesigner.PropertyInspectorView;
                        SelectHelper._wfDesigner = _wfDesigner;
                        //LoadOutline();
                    }
                }
            }
            //}
        }

        //close , maximize , minimize
        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RibbonWindowMaximize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsMaximized == true)
                {
                    SystemCommands.RestoreWindow(this);
                    //falseribbon.MinWidth = 210;
                    maxrestoreimg.Source = new BitmapImage(new Uri(@"/BOTDesigner;component/Images/maximizewhite.png", UriKind.Relative));
                    btnmaxrestore.ToolTip = "Maximize";
                    IsMaximized = false;
                }
                else
                {
                    //SystemCommands.MaximizeWindow(this);
                    //this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                    //this.Width= System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                    if (this.WindowState == WindowState.Normal)
                    {
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                        this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //addednow
                        this.WindowState = WindowState.Maximized;
                        this.WindowStyle = WindowStyle.None;
            
                        //falseribbon.MinWidth = 545;
                        maxrestoreimg.Source = new BitmapImage(new Uri(@"/BOTDesigner;component/Images/restorewhite.png", UriKind.Relative));
                        btnmaxrestore.ToolTip = "Restore";
                    }
                    IsMaximized = true;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void RibbonWindowMinimize_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void RibbonWindowClose_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (IsFirstLaunch == false)
                {
                    if (e.PreviousSize.Width < e.NewSize.Width)
                    {
                        workflowfalseribbon.Width = workflowfalseribbon.Width + (e.NewSize.Width - e.PreviousSize.Width);
                        debugfalseribbon.Width = debugfalseribbon.Width + (e.NewSize.Width - e.PreviousSize.Width);
                        Thickness margin = lbl_Title.Margin;
                        margin.Left = margin.Left - (e.NewSize.Width - e.PreviousSize.Width) * 0.65;
                        margin.Right = margin.Right + (e.NewSize.Width - e.PreviousSize.Width) * 0.65;
                        lbl_Title.Margin = margin;
                    }
                    else if (e.PreviousSize.Width > e.NewSize.Width)
                    {
                        workflowfalseribbon.Width = workflowfalseribbon.Width - (e.PreviousSize.Width - e.NewSize.Width);
                        debugfalseribbon.Width = debugfalseribbon.Width - (e.PreviousSize.Width - e.NewSize.Width);
                        Thickness margin = lbl_Title.Margin;
                        margin.Left = margin.Left + (e.PreviousSize.Width - e.NewSize.Width) * 0.65;
                        margin.Right = margin.Right - (e.PreviousSize.Width - e.NewSize.Width) * 0.65;
                        lbl_Title.Margin = margin;
                    }
                }
            }
            catch (Exception)
            {


            }


        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                //SystemCommands.MaximizeWindow(this);
                //this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                //this.Width= System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //addednow
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                //falseribbon.MinWidth = 545;
                maxrestoreimg.Source = new BitmapImage(new Uri(@"/BOTDesigner;component/Images/restorewhite.png", UriKind.Relative));
                btnmaxrestore.ToolTip = "Restore";

                IsMaximized = true;
            }
            }

            private LayoutDocumentPane GetDashboard()
        {
            try
            {
                IEnumerable<ILayoutElement> LayoutDocuments = null;
                LayoutDocuments = this.dockManager.Layout.Descendents();

                foreach (ILayoutElement item in LayoutDocuments)
                {
                    if (item is LayoutPanel)
                    {
                        LayoutPanel lp = (LayoutPanel)item;
                        Orientation o = lp.Orientation;
                        if (o.ToString() == "Horizontal")
                        {
                            if (lp.Children.Count > 0)
                            {
                                foreach (var itemIn1 in lp.Children)
                                {
                                    if (itemIn1 is LayoutDocumentPaneGroup)
                                    {
                                        LayoutDocumentPaneGroup ldpg = (LayoutDocumentPaneGroup)itemIn1;
                                        if (ldpg.Children.Count > 0)
                                        {
                                            foreach (var itemin2 in ldpg.Children)
                                            {
                                                bool isDocumentpane = true;
                                                LayoutDocumentPane la = (LayoutDocumentPane)(ldpg.Children[0]);
                                                foreach (var itemin3 in la.Children)
                                                {
                                                    if (itemin3 is LayoutAnchorable)
                                                    {
                                                        LayoutAnchorable itemAL = (LayoutAnchorable)itemin3;
                                                        if ((itemAL.ContentId == 100) || (itemAL.ContentId == 200))
                                                        {
                                                            isDocumentpane = false;
                                                            break;
                                                        }

                                                    }
                                                }
                                                if (isDocumentpane == true)
                                                {
                                                    foreach (var dash in la.Children)
                                                    {
                                                        if (dash.Title.Equals("Dashboard", StringComparison.CurrentCulture))
                                                        {
                                                            dash.IsSelected = true;
                                                        }
                                                    }



                                                    return la;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return null;
        }

    }
}
