using CommonLibrary;
using System;
using System.Activities.Presentation;
using System.Windows;
using System.Windows.Data;
using System.Activities.Core.Presentation;
using System.IO;
using System.Globalization;
using CommonLibrary.Interfaces;
using Bot.Activity.Windows;
using System.Windows.Input;
using System.Windows.Automation;

namespace Bot.Activity.ActivityLibrary
{
    /// <summary>
    /// Interaction logic for ScrapingWindow.xaml
    /// </summary>
    public partial class ScrapingWindow : Window
    {
        int iOpenFile = 0;
        int iSaveFieldOperations = 0;
        private TestOptions options = null;
        string currentFile = string.Empty;
        ScrapMode scrapIntermediateMode = ScrapMode.None;
        public string ApplicationId = string.Empty;
        private VisualUIAVerify.Controls.AutomationElementTreeControl _automationElementTree;
        public ScrapingWindow()
        {
            InitializeComponent();
            if (iOpenFile > 0)
            {
                for (int i = 0; i < iOpenFile; i++)
                {
                    SelectHelper.Scraping -= Scraping;
                }
            }
            iOpenFile = 0;
            SelectHelper.Scraping += Scraping;
            iOpenFile = iOpenFile + 1;

            iSaveFieldOperations = 0;
            if (iSaveFieldOperations > 0)
            {
                for (int i = 0; i < iSaveFieldOperations; i++)
                {
                    SelectHelper.SaveFieldOperations -= SaveFieldOperations;
                }
            }
            iSaveFieldOperations = 0;
            SelectHelper.SaveFieldOperations += SaveFieldOperations;
            iSaveFieldOperations = iSaveFieldOperations + 1;

            this.options = new TestOptions();
            this.options.BooleanProperty = true;

            //if (SelectHelper._automationElementTree == null)
            //{
                _automationElementTree = new VisualUIAVerify.Controls.AutomationElementTreeControl();
            //}
            //else
            //{
            //    _automationElementTree = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
            //    _automationElementTree.Refresh();
            //}
            _automationElementTree.RootElement = AutomationElement.RootElement;
            SelectHelper._automationElementTree = _automationElementTree;
        }

        public void ScrapingWindowInitialize(string applicationId)
        {
            //if (File.Exists(SelectHelper._currentscrapfile))
            //{
            //    File.Delete(SelectHelper._currentscrapfile);
            //}
            //SelectHelper._wfDesigner.Save(SelectHelper._currentscrapfile);
            btnSimulation.Content = "Start Simulation";
            currentFile = SelectHelper._currentscrapfile;
           
           
            ApplicationId = applicationId;

            if (SelectHelper.CurrentScrapMode == ScrapMode.None)
            {
                // this.options.EnumProperty = ScrapMode.Windows;
                scrapIntermediateMode = ScrapMode.Windows;
                WindowsRadioButton.IsChecked = true;
                WebRadioButton.Visibility = Visibility.Hidden;
            }
            else if (SelectHelper.CurrentScrapMode == ScrapMode.Windows)
            {
                scrapIntermediateMode = ScrapMode.Windows;
                WindowsRadioButton.IsChecked = true;
                // this.options.EnumProperty = ScrapMode.Windows;
                WebRadioButton.Visibility = Visibility.Hidden;
            }
            else if (SelectHelper.CurrentScrapMode == ScrapMode.Web)
            {
                scrapIntermediateMode = ScrapMode.Web;
                SelectHelper.DialogWacher = true;
                WebRadioButton.Visibility = Visibility.Visible;
                WebRadioButton.IsChecked = true;
                //this.options.EnumProperty = ScrapMode.Web;

            }
            else if (SelectHelper.CurrentScrapMode == ScrapMode.SAP)
            {
                scrapIntermediateMode = ScrapMode.SAP;

                WebRadioButton.Content = "SAP";
                WebRadioButton.Visibility = Visibility.Visible;
                WebRadioButton.IsChecked = true;
                //  this.options.EnumProperty = ScrapMode.SAP;
            }
            else if (SelectHelper.CurrentScrapMode == ScrapMode.Mainframe)
            {
                scrapIntermediateMode = ScrapMode.Mainframe;
                WebRadioButton.Content = "Mainframe";
                WebRadioButton.Visibility = Visibility.Visible;
                WebRadioButton.IsChecked = true;
                // this.options.EnumProperty = ScrapMode.Mainframe;
            }
            lstScrapFields.ItemsSource = null;
            lstScrapFields.ItemsSource = SelectHelper.CurrentPluginScrapeProperties;
        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.options;

            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            //WindowsPlugin.Instance.Initialize();
            host.Child = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
            this.wfGrid.Children.Add(host);

            TabWindowsProperties.IsSelected = true;
            this.wfGrid.IsEnabled = true;
            TabFields.IsSelected = true;

            try
            {
                SelectHelper.scrapingWindow = this;
            }
            catch (Exception)
            {

            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectHelper.StartSimulation = true;
            if (btnSimulation.Content.ToString() == "Start Simulation")
            {
                if (WindowsRadioButton.IsChecked.Value)
                {
                    if ((SelectHelper.CurrentPlugin != null) && (!(SelectHelper.CurrentPlugin is Windows.WindowsPlugin)))
                    {
                        ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                        icustplugin.StopScraping(ApplicationId);
                    }
                    SelectHelper.CurrentScrapMode = ScrapMode.Windows;
                    TabWindowsProperties.IsSelected = true;
                    VisualUIAVerify.Controls.AutomationElementTreeControl vc = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
                    VisualUIAVerify.Controls.TreeHelper.flag = false;
                   vc.Refresh();
                    WindowsPlugin.Instance.StartScraping(ApplicationId);
                    TabFields.IsSelected = true;
                }
                else
                {
                    WindowsPlugin.Instance.StopScraping(ApplicationId);
                    if (SelectHelper.CurrentPlugin != null)
                    {
                        ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                        icustplugin.StartScraping(ApplicationId);
                    }
                }

                btnSimulation.Content = "Stop Simulation";

                //iSimulatApp.StartSimultion(currentISimulationAppKey);
            }
            else
            {
                SelectHelper.StartSimulation = false;
                btnSimulation.Content = "Start Simulation";
                //iSimulatApp.StopSimultion(currentISimulationAppKey);
                WindowsPlugin.Instance.StopScraping(ApplicationId);
                if (SelectHelper.CurrentPlugin != null)
                {
                    ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                    icustplugin.StopScraping(ApplicationId);

                }
            }
        }
       
        public void Scraping(object sender, ScrapingEventArgs e)
        {
            if (e.Type == 0)
            {
                // lstScrapFields.ItemsSource = e.ActivityCollection;
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                  
                    lstScrapFields.ItemsSource = null;
                    lstScrapFields.ItemsSource = SelectHelper.CurrentPluginScrapeProperties;
                });
            }
            else
            {
               
                iAction = 0;
                currentFile = SelectHelper._currentscrapfile;
               this.Close();
            }

        }

        public void SaveFieldOperations(object sender, SaveFieldEventArgs e)
        {
            try
            {
                if (e.iSaveFieldOperation == 1)
                { 
                    PropertyGrid1.SelectedObject = e.activity;
                    TabProperties.IsSelected = true;
                }
                if (e.iSaveFieldOperation == 2)
                {
                    SelectHelper.CurrentPluginScrapeProperties.Remove(e.activity);
                    lstScrapFields.ItemsSource = null;
                    lstScrapFields.ItemsSource = SelectHelper.CurrentPluginScrapeProperties;
                }
            }
            catch (Exception)
            {

            }
        }
        //object obj = new object();
        private void ReloadDesigner(int action = 0)
        {

            //lock (obj)
            //{
            SelectHelper.StartSimulation = false;
                if (string.IsNullOrEmpty(currentFile))
                    return;

                if (action == 1)
                {
                    if (File.Exists(currentFile))
                    {
                        File.Delete(currentFile);
                    }
                }

                if (!string.IsNullOrEmpty(currentFile))
                {
                    if (SelectHelper.WorkflowDictionary.ContainsKey(currentFile))
                    {
                        WorkflowDesigner _wfDesigner = SelectHelper.WorkflowDictionary[currentFile];
                        if (action == 1)
                        {
                            _wfDesigner.Save(currentFile);
                        }

                        _wfDesigner = new WorkflowDesigner();
                        _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
                        _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;

                        //associates all of the basic activities with their designers
                        new DesignerMetadata().Register();
                        _wfDesigner.Load(currentFile);

                        if (SelectHelper.WorkflowDictionary.ContainsKey(currentFile))
                        {
                            SelectHelper.WorkflowDictionary[currentFile] = _wfDesigner;
                        }

                        if (currentFile == SelectHelper._currentworkflowfile)
                        {
                          _wfDesigner.View.UpdateLayout();
                        _wfDesigner.OutlineView.UpdateLayout();
                        System.Windows.Controls.Border WfDesignerBorder = (System.Windows.Controls.Border)SelectHelper.Border;
                            WfDesignerBorder.Child = _wfDesigner.View;
                            SelectHelper._wfPropertyBorder.Child = _wfDesigner.PropertyInspectorView;
                        SelectHelper._wfOutlineBorder.Child = _wfDesigner.OutlineView;
                        }
                    }
                }
            //}
        }

        private void WebRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if ((WebRadioButton.IsChecked.Value))
            {
                SelectHelper.CurrentScrapMode = scrapIntermediateMode;
                WindowsPlugin.Instance.StopScraping(ApplicationId);

                if (SelectHelper.CurrentPlugin != null)
                {
                    ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                    icustplugin.StartScraping(ApplicationId);
                }
            }
        }
        private void WindowsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowsRadioButton.IsChecked.Value)
            {
                if (SelectHelper.CurrentScrapMode != ScrapMode.Windows)
                {
                    SelectHelper.CurrentScrapMode = ScrapMode.Windows;
                }
                if (SelectHelper.CurrentPlugin != null)
                {
                    ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                    icustplugin.StopScraping(ApplicationId);
                }
                if (SelectHelper.StartSimulation == true)
                {
                    TabWindowsProperties.IsSelected = true;
                    VisualUIAVerify.Controls.AutomationElementTreeControl vc = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
                    vc.Refresh();
                    WindowsPlugin.Instance.StartScraping(ApplicationId);
                    TabFields.IsSelected = true;
                }
                else
                {
                    WindowsPlugin.Instance.StopScraping(ApplicationId);
                }
            }
            else
            {
                WindowsPlugin.Instance.StopScraping(ApplicationId);
                if (SelectHelper.CurrentPlugin != null)
                {
                    ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                    icustplugin.StartScraping(ApplicationId);
                }
            }
        }

       
        int iAction = 0;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            iAction = 1;
            this.Close();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            StopScraping(1);
            ReloadDesigner(iAction);
            SelectHelper.scrapingWindow = null;
        }

        
        private void Window_Closed(object sender, EventArgs e)
        {
            SelectHelper.scrapingWindow = null;
            SelectHelper._automationElementTree = null;
            if (iOpenFile > 0)
            {
                for (int i = 0; i < iOpenFile; i++)
                {
                    SelectHelper.Scraping -= Scraping;
                }
            }
        }
        private void StopScraping(int type)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                SelectHelper.StartSimulation = false;
                SelectHelper.CurrentScrapMode = ScrapMode.None;
                SelectHelper._currentscrapfile = string.Empty;
                //this.Visibility = Visibility.Hidden;
                WindowsPlugin.Instance.StopScraping(ApplicationId);
                SelectHelper.DialogWacher = false;
                if (btnSimulation.Content.ToString() == "Stop Simulation")
                {
                    btnSimulation.Content = "Start Simulation";
                   
                }
                if ((SelectHelper.CurrentPlugin != null) && (type == 1))
                {
                    ICustomPluginInterface icustplugin = (ICustomPluginInterface)SelectHelper.CurrentPlugin;
                    icustplugin.StopScraping(ApplicationId);
                    icustplugin.CloseApplication(ApplicationId);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }
            finally
            {

                //if(Mouse.OverrideCursor == Cursors.Wait)
                //{
                //  Mouse.OverrideCursor = Cursors.Arrow;
                //}
            }
        }
    }


    public class TestOptions
    {
        public ScrapMode EnumProperty { get; set; }
        public bool BooleanProperty { get; set; }
    }

    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }
}
