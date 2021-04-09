using CommonLibrary;
using CommonLibrary.Interfaces;
using Logger;
using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace Bot.Activity.WinDriverPlugin
{
    [Export(typeof(ICustomPluginInterface))]
    public sealed class WindowsPlugin : ICustomPluginInterface
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll")]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        public double X { get; set; }
        public double Y { get; set; }
        private static WindowsPlugin instance = null;
        private bool readystate = false;
        public bool NonScrapeMode { get; set; }
        private static readonly object padlock = new object();
        
        int iOpenFile = 0;
        int iReset = 0;
        int iIECreated = 0;
        WindowsHelper windowsHelper = new WindowsHelper();
        string appName = string.Empty;

        private WindowsPlugin()
        {
           // AssemblyName = "InternetExplorer";
            //if (iOpenFile > 0)
            //{
            //    for (int i = 0; i <= iOpenFile; i++)
            //    {
            //        SelectHelper.LaunchScraping -= LaunchScraping;
            //    }
            //}
            //iOpenFile = 0;
            SelectHelper.LaunchScraping += LaunchScraping;
          //  iOpenFile = iOpenFile + 1;

            if (iReset > 0)
            {
                for (int i = 0; i <= iReset; i++)
                {
                    SelectHelper.Reset -= Reset;
                }
            }
            iReset = 0;
            SelectHelper.Reset += Reset;
            iReset = iReset + 1;

            string Mode = ConfigurationManager.AppSettings.Get("Mode");
            if ((Mode.ToLower() == "Designer"))
            {
                
            }
           
        }
    

        private void AddField(string propName, string propValue, string fieldType)
        {
            try
            {
               // pluginHelper.AddField(propName, propValue, fieldType, Application.ApplicationId, ApplicationName, RequestNo, "System.String");
            }
            catch (Exception)
            {
                //Log.Logger.LogData("Error In AddField: " + ex.Message, LogLevel.Error, RequestNo);
            }
        }

        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var element = sender as ContentControl;
            if (element != null)
            {
                ShowLocation(element);
            }
        }
        private void ShowLocation(ContentControl element)
        {
            var location = element.PointToScreen(new Point(0, 0));
            X = location.X;
            Y = location.Y;
        }
        public int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    
        public static WindowsPlugin Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WindowsPlugin();
                           
                        }
                    }
                }
                return instance;
            }
        }
       
        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            string ExePath = string.Empty;
            string ApplicationId = string.Empty;
            string TitleOrUrl = string.Empty;
            int iTimeInSec = 0;
            bool launchResult = false;
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));
            if (!string.IsNullOrEmpty(e.strTitleOrUrlToAttach) && (e.strTitleOrUrlToAttach.Trim().Length > 0))
            {
                launchResult = LaunchWindows(e.ApplicationID, TitleOrUrl, iTimeInSec, launchResult, false, e.strTitleOrUrlToAttach);
            }
            else
            {
                foreach (ModelItem item in modelItems)
                {
                    if (item.ItemType.FullName == "Bot.Activity.WinDriverPlugin.Windows_OpenApplication")
                    {
                        Windows_OpenApplication owa = (Windows_OpenApplication)item.GetCurrentValue();
                        ApplicationId = owa.ApplicationID.Expression.ToString();
                        if (ApplicationId == e.ApplicationID)
                        {
                            ExePath = owa.EXEPath.Expression.ToString();
                            // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
                            iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                            launchResult = LaunchWindows(ApplicationId, ExePath, iTimeInSec, launchResult, false);
                            SelectHelper.CurrentScrapMode = ScrapMode.Windows;
                            break;
                        }
                    }
                }
            }
        }
        public void Reset(object sender, ResetEventArgs e)
        {
            string SearchUrl = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
           
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

            foreach (ModelItem item in modelItems)
            {
                if (item.ItemType.FullName == "Windows.OpenWindowsApplication")
                {
                    //OpenMainframeApplication owa = (OpenMainframeApplication)item.GetCurrentValue();
                    //ApplicationId = owa.ApplicationID.Expression.ToString();
                    //if (ApplicationId == e.ApplicationID)
                    //{
                    //    SearchUrl = owa.SearchUrl.Expression.ToString();
                    //    // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
                    //    iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                    //    ResetIE(ApplicationId, SearchUrl, iTimeInSec);
                    //}
                }
            }
        }
        private bool ResetIE(string AppId, string searchUrl, int timeInSec )
        {
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        
                        //IEWATIN = (WatiN.Core.IE)RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId];
                        //IEWATIN.GoTo(searchUrl);
                        //IEWATIN.WaitForComplete(5000);
                        
                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        return true;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                return false;
            }
        }


            IntPtr Handle = IntPtr.Zero;
        WindowsInstance windowsInstance = null;
        public bool LaunchWindows(string AppId, string exepath, int timeInSec, bool launchResult,bool NoScrape = false, string TitleOrUrl = "")
        {
            try
            {
                int icount = 0;

                if (NoScrape == true)
                {
                    SelectHelper.StartSimulation = false;
                    TitleOrUrl = "";
                }
                if ((TitleOrUrl != "") && (!string.IsNullOrEmpty(TitleOrUrl)))
                {
                    //IEWATIN = IE.AttachToIE(Find.ByUrl(TitleOrUrl));
                    //if (IEWATIN == null)
                    //{
                    //    IEWATIN = IE.AttachToIE(Find.ByTitle(TitleOrUrl));
                    //    if (IEWATIN == null)
                    //    {
                    //        Log.Logger.LogData("Not able to attach by titile or url:" + TitleOrUrl, LogLevel.Error);
                    //        return false;
                    //    }
                    //}
                    //IEWATIN.ApplicationId = AppId;
                    //if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    //{
                    //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId] = IEWATIN;
                    //}
                    //else
                    //{
                    //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, IEWATIN);
                    //}
                }

                else if (AppId != string.Empty) //scraping time
                {
                    if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                    
                        windowsInstance = new WindowsInstance(exepath);
                        //windowsInstance.Win WindowTitle = CreateApplicationInNewProcess(exepath);
                        //Added By Piyush
                        //Process m_Proc = Process.Start(exepath);                               //Starts the Process
                        //windowsInstance.ProcessId = m_Proc.Id;
                        //SelectHelper.ApplicationProcessId = m_Proc.Id;

                       
                        appName = GetApplicationName(exepath);
                        //GetLatestProcessHandle("EXCEL");
                        GetLatestProcessHandle(appName);


                        windowsInstance.ApplicationId = AppId;
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, windowsInstance);
                    }
                    else
                    {
                        windowsInstance = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                    }
                }
                else
                {
                    //windowsInstance = new WindowsInstance();
                    //windowsInstance.ProcessId = CreateIEInNewProcess(exepath);     
                    //windowsInstance.ApplicationId = AppId;
                    Logger.Log.Logger.LogData("Please enter ApplicationID", LogLevel.Error);
                }
                if (windowsInstance != null)
                {
                    //SelectHelper.CurrentAppProcessId = windowsInstance.ProcessId;
                }
                SelectHelper.CurrentPlugin = this;
           
                launchResult = true;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Exception in Windows PlugIn : "+ex.Message, LogLevel.Error);
            }
            //});
            return launchResult;
        }

        private string GetApplicationName(string exePath)
        {
            string EXEPATH = string.Empty;
            try
            {
                EXEPATH = exePath.ToString();
                EXEPATH = EXEPATH.Substring(EXEPATH.LastIndexOf("\\") + 1);
                EXEPATH = EXEPATH.Remove(EXEPATH.LastIndexOf("."));
                return EXEPATH;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception in Getting Application Name : "+ex.Message,LogLevel.Error);
                return null;
            }
            finally
            {
                //return EXEPATH;
            }
        }

        public void CloseApplication(string ApplicationId)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                WindowsInstance windowsinstance = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(ApplicationId);
                windowsinstance._driver.Quit();
                GC.Collect();
            }
        }
        public int GetLatestProcessHandle(string processName)
        {
            const int timeout = 5;

            Process[] anotherApps = Process.GetProcessesByName(processName);
            Process tmp = anotherApps[0];
            if (anotherApps.Count() > 1)
            {
                foreach (var item in anotherApps)
                {
                    try
                    {
                        DateTime dt = tmp.StartTime;
                    }
                    catch (Exception)
                    {
                        tmp = item;
                        continue;
                        //it will throw exception - Access denied if process object is not created by this process
                    }

                    try
                    {
                        if (item.StartTime > tmp.StartTime)
                        {
                            tmp = item;
                        }
                    }catch(Exception)
                    {
                        //it will throw exception - Access denied if process object is not created by this process
                    }
                }
            }
            SimpleTimer timeoutTimer = new SimpleTimer(timeout);
            while ((tmp.MainWindowHandle.ToInt32() == 0) && (!timeoutTimer.Elapsed))
            {
                tmp.Refresh();
                Thread.Sleep(500);
            }
            if (tmp.MainWindowHandle.ToInt32() != 0)
            {
                windowsInstance.ProcessId = tmp.Id;
                SelectHelper.ApplicationProcessId = tmp.Id;  // Piyush
                return tmp.MainWindowHandle.ToInt32();
            }
            return 0;
        }
        
        public void Dispose()
        {
            visualDesignerHelper.Unsubscribe();
            visualDesignerHelper.UnloadClientSideProviders();
            GC.Collect(0);
            //throw new NotImplementedException();
        }

        VisualDesignerHelper visualDesignerHelper = null;
        public void StartScraping(string ApplicationId)
        {
            VisualUIAVerify.Controls.AutomationElementTreeControl _automationElementTree = null;
            //if (SelectHelper._automationElementTree != null)
            //{
            //    _automationElementTree = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
            //}
            if (visualDesignerHelper == null)
            {
                visualDesignerHelper = new VisualDesignerHelper();
            }
            //else
            //{
            if (SelectHelper._automationElementTree != null)
            {
                _automationElementTree = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
                _automationElementTree.Refresh();
            }

            //}
            visualDesignerHelper.InitializeVisualDesigner(_automationElementTree);
        }

        public void StopScraping(string ApplicationId)
        {
            try
            {
                if (visualDesignerHelper != null)
                {
                    visualDesignerHelper.StopHighlighting();
                    visualDesignerHelper.Unsubscribe();
                    visualDesignerHelper.UnloadClientSideProviders();
                }
            }finally
            {
                GC.Collect(0);
            }
        }
       // private VisualUIAVerify.Controls.AutomationElementTreeControl _automationElementTree;

        //public void Initialize()
        //{
        //    if (SelectHelper._automationElementTree == null)
        //    {
        //        _automationElementTree = new VisualUIAVerify.Controls.AutomationElementTreeControl();
        //    }
        //    else
        //    {
        //        _automationElementTree = (VisualUIAVerify.Controls.AutomationElementTreeControl)SelectHelper._automationElementTree;
        //        _automationElementTree.Refresh();
        //    }
        //    _automationElementTree.RootElement = AutomationElement.RootElement;
        //    SelectHelper._automationElementTree = _automationElementTree;
        //}
        public void OnCloseForm()
        {
            if (visualDesignerHelper != null)
            {
                visualDesignerHelper.StopHighlighting();
                visualDesignerHelper.Unsubscribe();
                visualDesignerHelper.UnloadClientSideProviders();
            }
           
        }
    }
}
