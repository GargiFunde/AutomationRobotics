// <copyright file=IExplorer company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:57</date>
// <summary></summary>

using CommonLibrary;
using CommonLibrary.Interfaces;
using Logger;
using mshtml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using LogLevel = Logger.LogLevel;
//using WatiN.Core;
//using WatiN.Core.Native.Windows;
using Gma.System.MouseKeyHook;
using System.Web;
using OpenQA.Selenium.Interactions;
using System.ComponentModel;


namespace Bot.Activity.Web
{
    [Export(typeof(ICustomPluginInterface))]
    public sealed class Web : ICustomPluginInterface
    {
        private IKeyboardMouseEvents m_GlobalHook;
        public double X { get; set; }
        public double Y { get; set; }
        private static Web instance = null;
        private bool readystate = false;
        public bool NonScrapeMode { get; set; }
        private static readonly object padlock = new object();
        HTMLDocument htmlDoc = null;
        int iOpenFile = 0;
        int iReset = 0;
        int iIECreated = 0;
        //public Actions action;
        // IExplorerHelper IEHelper = new IExplorerHelper();
        string BotBuilderXPath { get; set; }
        private Web()
        {
            try
            {
             //  action = new Actions(commonWebBrowser_Current.driver);
                // AssemblyName = "InternetExplorer";
                //if (iOpenFile > 0)
                //{
                //    for (int i = 0; i <= iOpenFile; i++)
                //    {
                //        SelectHelper.LaunchScraping -= LaunchScraping;
                //    }
                //}
                //iOpenFile = 0;
                SelectHelper.LaunchScraping -= LaunchScraping;
                SelectHelper.LaunchScraping += LaunchScraping;
                // iOpenFile = iOpenFile + 1;

                //if (iReset > 0)
                //{
                //    for (int i = 0; i <= iReset; i++)
                //    {
                //        SelectHelper.Reset -= Reset;
                //    }
                //}
                //iReset = 0;
                //SelectHelper.Reset -= Reset;
                //SelectHelper.Reset += Reset;
                //iReset = iReset + 1;


                // iIECreated = 0;
                // if (iIECreated > 0)
                // {
                //     for (int i = 0; i < iIECreated; i++)
                //     {
                //         SelectHelper.IECreated -= Ie_DocumentComplete;
                //     }
                // }
                // iIECreated = 0;
                // SelectHelper.IECreated -= Ie_DocumentComplete;
                // SelectHelper.IECreated += Ie_DocumentComplete;
                // iIECreated = iIECreated + 1;
                //// CreateBlankIEInstance();

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            // 
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
        Task taskA = null;

        private void RemoveHighlightOfElements()
        {
            //    try
            //    {
            //        int icount = myRedElements.Count;
            //        //foreach (mshtml.IHTMLStyle item in myRedElements)
            //        for (int i = 0; i < icount; i++)
            //        {
            //            mshtml.IHTMLStyle item = myRedElements[i];
            //            if (orgColor.ContainsKey(item))
            //                item.borderColor = orgColor[item];
            //            if (orgBorder.ContainsKey(item))
            //                item.border = orgBorder[item];
            //        }

            //        myRedElements.Clear();
            //        orgColor.Clear();
            //        orgBorder.Clear();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Logger.LogData(ex.Message, LogLevel.Error);
            //    }
        }
        public static Web Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Web();

                        }
                    }
                }
                return instance;
            }
        }
        string ScrapApplicationId = string.Empty;

        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            try
            {
                string LaunchUrl = string.Empty;
                string TitleOrUrl = string.Empty;
                string BrowserType = string.Empty;
                string strWaitUntilContainText = string.Empty;
                int iTimeInSec = 5;
                bool launchResult = false;
                var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
                IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

                if (!string.IsNullOrEmpty(e.strTitleOrUrlToAttach) && (e.strTitleOrUrlToAttach.Trim().Length > 0))
                {
                    launchResult = LaunchIE(e.ApplicationID, /*e.BrowserType*/"IE", TitleOrUrl, iTimeInSec, launchResult, false, e.strTitleOrUrlToAttach, e.strWaitUntilContainText);
                }
                else 
                {
                    foreach (ModelItem item in modelItems)
                    {
                        if (item.ItemType.FullName == "Bot.Activity.Web.Web_OpenApplication")
                        {
                            Web_OpenApplication owa = (Web_OpenApplication)item.GetCurrentValue();
                            ScrapApplicationId = owa.ApplicationID.Expression.ToString();
                            if (owa.WaitUntilContainText != null)
                            {
                                strWaitUntilContainText = owa.WaitUntilContainText.Expression.ToString();
                            }
                            //if (owa.TimeOutInSecond.Expression != null)
                            //{
                            //    int iTimeOut = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                            //}

                            TitleOrUrl = e.strTitleOrUrlToAttach;
                            if (ScrapApplicationId == e.ApplicationID)
                            {
                                LaunchUrl = owa.Url.Expression.ToString();
                                BrowserType = owa.BrowserType;
                                //iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                                launchResult = LaunchIE(ScrapApplicationId, BrowserType, LaunchUrl, iTimeInSec, launchResult, false, TitleOrUrl, strWaitUntilContainText);
                                SelectHelper.CurrentScrapMode = ScrapMode.Web;
                                break;

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
        public void Reset(object sender, ResetEventArgs e)
        {
            try
            {
                //string SearchUrl = string.Empty;
                //string ApplicationId = string.Empty;
                //string TitleorUrl = string.Empty;
                //int iTimeInSec = 0;

                ResetIE(e.ApplicationID);
                //var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
                //IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

                //foreach (ModelItem item in modelItems)
                //{
                //    if (item.ItemType.FullName == "Bot.Activity.Web.IE_OpenApplication")
                //    {
                //        IE_OpenApplication owa = (IE_OpenApplication)item.GetCurrentValue();
                //        ApplicationId = owa.ApplicationID.Expression.ToString();
                //        if (ApplicationId == e.ApplicationID)
                //        {
                //            SearchUrl = owa.Url.Expression.ToString();
                //            iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                //            ResetIE(ApplicationId, SearchUrl, iTimeInSec);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private bool ResetIE(string AppId)
        {
            //WatiN.Core.IE IEWATIN = null;
            //try
            //{
            //    if (AppId != string.Empty) //scraping time
            //    {
            //        if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            //        {

            //            IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
            //            IEWATIN.GoTo(IEWATIN.SearchUrl);
            //            IEWATIN.WaitForComplete(5000);

            //            //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
            //            return true;
            //        }
            //    }
            //    return true;
            //}
            //catch(Exception ex)
            //{
            //    Log.Logger.LogData(ex.Message, LogLevel.Error);
            return false;
            //}
        }


        IntPtr Handle = IntPtr.Zero;

        public bool LaunchIE(string AppId, string browsertype, string launchUrl, int timeInSec, bool launchResult, bool NoScrape = false, string TitleOrUrl = "", string strWaitUntilContainText = "")
        {
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
            CommonWebBrowser commonWebBrowser = null;
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
                    #region MyRegion
                    //IEWATIN = IE.AttachTo<IE>(Find.ByUrl(TitleOrUrl));

                    //if (IEWATIN == null)
                    //{
                    //    IEWATIN = IE.AttachTo<IE>(Find.ByTitle(TitleOrUrl));
                    //    if (IEWATIN == null)
                    //    {
                    //        Log.Logger.LogData("Not able to attach by titile or url:" + TitleOrUrl, LogLevel.Error);
                    //        //return false;
                    //    }
                    //}
                    //IEWATIN.ApplicationId = AppId;
                    //IEWATIN.SearchUrl = TitleOrUrl;
                    //if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    //{
                    //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId] = IEWATIN;
                    //}
                    //else
                    //{
                    //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, IEWATIN);
                    //} 
                    #endregion
                }
                else if (AppId != string.Empty) //scraping time
                {
                    //IE.AttachToIE(Find.ByUrl(""));
                    //IE.AttachToIE(Find.ByUri(""));
                    //IE.AttachToIE(Find.ByTitle(""));
                    //IE.AttachToIE(Find.By("hwnd", windowHandle));
                    //for windows: https://www.codeproject.com/Articles/141842/Automate-your-UI-using-Microsoft-Automation-Framew
                    /**/
                    if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        //if (IEWATIN_New == null)
                        //{
                        //    taskA.Wait();
                        //}
                        //IEWATIN = IEWATIN_New;
                        //IEWATIN_New = null;

                        if (commonWebBrowser != null)
                        {
                            commonWebBrowser.ApplicationId = AppId;
                            //IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.Show);
                            //  IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);
                            //  IEWATIN.BringToFront();
                            //  IEWATIN.GoTo(launchUrl);
                            commonWebBrowser.driver.Url = launchUrl;
                            //if (!string.IsNullOrEmpty(strWaitUntilContainText) && (strWaitUntilContainText != ""))
                            //{
                            //    IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                            //}
                            //else
                            //{
                            //    IEWATIN.WaitForComplete(5000);
                            //}
                          
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, commonWebBrowser);
                            // CreateBlankIEInstance();
                        }
                        else
                        {
                            commonWebBrowser = new CommonWebBrowser(browsertype);
                            //IEWATIN = new IE();
                            commonWebBrowser.SearchUrl = launchUrl;
                            commonWebBrowser.ApplicationId = AppId;
                            commonWebBrowser.LaunchWebBrowser(launchUrl, timeInSec);
                            //IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

                            commonWebBrowser.driver.Manage().Window.Maximize();

                            //IEWATIN.BringToFront();
                            //IEWATIN.GoTo(launchUrl);
                            //if (!string.IsNullOrEmpty(strWaitUntilContainText) && (strWaitUntilContainText != ""))
                            //{
                            //    IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                            //}
                            //else
                            //{
                            //    IEWATIN.WaitForComplete(5000);
                            //}
                            //IEWATIN.WaitForComplete(5000);
                            
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, commonWebBrowser);
                            //CreateBlankIEInstance();
                        }
                        // AutomationElement window = AutomationElement.FromHandle(IEWATIN.hWnd);
                        //  Automation.AddAutomationEventHandler(WindowPattern.WindowClosedEvent, window, TreeScope.Element, IEWATIN.OnApplicationClose);
                        //IEWATIN = new IE(new Uri(launchUrl), true);                        
                    }
                    else
                    {
                         commonWebBrowser = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        commonWebBrowser.driver.Dispose();
                        // IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.Show);
                        //commonWebBrowser.driver.CurrentWindowHandle.
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(AppId);
                        commonWebBrowser = new CommonWebBrowser(browsertype);
                        commonWebBrowser.SearchUrl = launchUrl;
                        commonWebBrowser.ApplicationId = AppId;
                        commonWebBrowser.LaunchWebBrowser(launchUrl, timeInSec);
                        commonWebBrowser.driver.Manage().Window.Maximize();
                        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, commonWebBrowser);
                    }
                }
                else
                {
                    //IEWATIN = new IE(new Uri(launchUrl), true);
                    //IEWATIN.WaitForComplete(5000);
                    Logger.Log.Logger.LogData("Please enter ApplicationID", LogLevel.Error);
                    //  SelectHelper._wfDesigner.Context.

                }
                if (commonWebBrowser != null)
                {
                    //SelectHelper.CurrentAppProcessId = commonWebBrowser.driver.CurrentWindowHandle;
                }
                SelectHelper.CurrentPlugin = this;


                launchResult = true;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    Log.Logger.LogData("Timeout loading page after " + timeInSec + " Second in activity Web_OpenApplication", LogLevel.Info);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity Web_OpenApplication", LogLevel.Error);
                }


            }
            //});
            return launchResult;
        }
        CommonWebBrowser commonWebBrowser_Current = null;
        public void StartScraping(string AppId)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                if ((commonWebBrowser_Current == null) || (commonWebBrowser_Current.driver == null))
                {
                    Log.Logger.LogData("IE Instance is not loadded", LogLevel.Error);
                }
                //action = new Actions(commonWebBrowser_Current.driver);
                AddHandlers();
            }
        }

        public void OpenBrowserXPath(string BrowserName , string url)
        {

            try
            {
                commonWebBrowser_Current = new CommonWebBrowser(BrowserName);

                commonWebBrowser_Current.driver.Navigate().GoToUrl(url);
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Open Browser - " + ex + "\n");
            }
           

        }
        public void StartScrapingforXPath(string AppId)
        {


            try
            {
                if ((commonWebBrowser_Current == null) || (commonWebBrowser_Current.driver == null))
                {
                    Log.Logger.LogData("IE Instance is not loadded", LogLevel.Error);
                }
                //action = new Actions(commonWebBrowser_Current.driver);
                AddHandlersXPath();
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Start Scraping - " + ex + "\n");
            }
            
        }

        private static IKeyboardMouseEvents HookEvents = null;
        private void AddHandlersXPath()
        {
            try
            {


                //m_GlobalHook = Hook.GlobalEvents();
                HookEvents = Hook.GlobalEvents();
               HookEvents.MouseDoubleClick -= M_GlobalHook_MouseDoubleClickXPath;
                HookEvents.MouseDoubleClick += M_GlobalHook_MouseDoubleClickXPath;

                //String strJavaScript = " var style ; function addHighlight(target){ style = target.style; target.style.borderColor  = 'red'; } function removeHighlight(target) { target.style.borderColor  = 'black'; } window.addEventListener('mouseover', function(e) { addHighlight(e.target); targetr =e.target; });window.addEventListener('mouseout', function(e) { removeHighlight(e.target); });";
                String strJavaScript = " var borderColor; var txtColor; var borderSize;function addHighlight(target){ borderColor = target.style.borderColor;borderSize = target.style.borderWidth;txtColor =target.style.color; target.style.borderColor  = 'red';target.style.color  = 'red';target.style.borderWidth='1px solid red'; } function removeHighlight(target) { target.style.borderColor= borderColor;target.style.color =txtColor;target.style.borderWidth=borderSize; } function ABC(e) { addHighlight(e.target); targetr =e.target;sessionStorage.setItem(\"selemtarg\",JSON.stringify(e.target)) ;sessionStorage.setItem(\"selemid\",targetr.id);sessionStorage.setItem(\"selemname\",targetr.name);sessionStorage.setItem(\"selemclass\",targetr.className);sessionStorage.setItem(\"selemX\",e.clientX);sessionStorage.setItem(\"selemY\",e.clientY);sessionStorage.setItem(\"selemTag\",targetr.tagName);} window.addEventListener('mouseover', ABC );function EFG(e) { removeHighlight(e.target); } window.addEventListener('mouseout',EFG );";
                // driver.execute_script("""(function(){ s = document.createElement('script'); s.textContent = 'function momo(){console.log("lalala")};'; document.body.appendChild(s); })();               """)
                ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript);
                //String js1 = " window.addEventListener('mouseover', function(e) {e = e || window.event; var target = e.target || e.srcElement,  text = target.textContent || target.innerText; sessionStorage.setItem(\"elemid\",target.id);sessionStorage.setItem(\"elemname\",target.name);sessionStorage.setItem(\"elemclass\",target.className); }, false); ";
                //String js1 = " window.addEventListener('mouseover', function(e) {e = window.event; var targeta = e.target  ; sessionStorage.setItem(\"elemid\",targeta.id);sessionStorage.setItem(\"elemname\",targeta.name);sessionStorage.setItem(\"elemclass\",targeta.className); }, false); ";
                //((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(js1);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Add Handler - " + ex + "\n");
            }
        }


        private void AddHandlers()
        {
            if ((SelectHelper.StartSimulation) && (SelectHelper.CurrentScrapMode == ScrapMode.Web))
            {
                m_GlobalHook = Hook.GlobalEvents();
                m_GlobalHook.MouseDoubleClick -= M_GlobalHook_MouseDoubleClick;
                m_GlobalHook.MouseDoubleClick += M_GlobalHook_MouseDoubleClick;
                
                //String strJavaScript = " var style ; function addHighlight(target){ style = target.style; target.style.borderColor  = 'red'; } function removeHighlight(target) { target.style.borderColor  = 'black'; } window.addEventListener('mouseover', function(e) { addHighlight(e.target); targetr =e.target; });window.addEventListener('mouseout', function(e) { removeHighlight(e.target); });";
                String strJavaScript = " var borderColor; var txtColor; var borderSize;function addHighlight(target){ borderColor = target.style.borderColor;borderSize = target.style.borderWidth;txtColor =target.style.color; target.style.borderColor  = 'red';target.style.color  = 'red';target.style.borderWidth='1px solid red'; } function removeHighlight(target) { target.style.borderColor= borderColor;target.style.color =txtColor;target.style.borderWidth=borderSize; } function ABC(e) { addHighlight(e.target); targetr =e.target; } window.addEventListener('mouseover', ABC );function EFG(e) { removeHighlight(e.target); } window.addEventListener('mouseout',EFG );";
                // driver.execute_script("""(function(){ s = document.createElement('script'); s.textContent = 'function momo(){console.log("lalala")};'; document.body.appendChild(s); })();               """)
                ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript);







            }
        }


        //private void M_GlobalHook_MouseDown(object sender, MouseEventArgs e)
        //{
        //    IWebElement element = null;
        //    String strJavaScript = "return document.activeElement";
        //    var obj = ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript);

        //}

       void RecordXPath(object sender , DoWorkEventArgs e)
        {
            RemoteWebElement elementatpos = null, activeelement = null;
            string id, name, classname, currtag;
            IWebElement obj = null;
            String strJavaScript = "return document.activeElement";

            String strJavaScript333 = "return document.elementFromPoint(sessionStorage.getItem('selemX'),sessionStorage.getItem('selemY'));";
            //String strJavaScript1 = "function a(){return localStorage.getItem(\"elemname\")}; a();";
            //String strJavaScript2 = "localStorage.getItem(\"elemname\")";
            //String strJavaScript3 = "localStorage.getItem(\"elemclass\")";
            //String TagScript = "localStorage.getItem(\"selemTag\")";

            //String strJavaScript = "return vv;";
            activeelement = (RemoteWebElement)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript);
            elementatpos = (RemoteWebElement)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript333);
            id = (String)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("return sessionStorage.getItem('selemid')");
            name = (String)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("return sessionStorage.getItem('selemname')");
            classname = (String)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("return sessionStorage.getItem('selemclass')");

            currtag = (String)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("return sessionStorage.getItem('selemTag')");


            //classname = ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript3);
            //id = element.GetAttribute("id");
            //name = element.GetAttribute("name");
            //classname = element.GetAttribute("class");

            //id =  commonWebBrowser_Current.driver.WebStorage.LocalStorage.GetItem("elemid");
            //  name = commonWebBrowser_Current.driver.WebStorage.LocalStorage.GetItem("elemname");
            //  classname = commonWebBrowser_Current.driver.WebStorage.LocalStorage.GetItem("elemclass");

            if (elementatpos != null && activeelement != null)
            {
                //logic for Elements fetched with DIV Tag or INPUT Tag
                if (currtag.Equals("DIV") || currtag.Equals("INPUT"))
                {

                    EmulatedXPath(activeelement);

                    //if ((id != "") && (id != "undefined"))
                    //{
                    //    obj = commonWebBrowser_Current.driver.FindElementById(id);
                    //    EmulatedXPath(obj);
                    //}
                    //else if (name != "" && name != "undefined")
                    //{
                    //    obj = commonWebBrowser_Current.driver.FindElementById(name);
                    //    EmulatedXPath(obj);
                    //}
                    //else
                    //{
                    //    EmulatedXPath(activeelement);
                    //}
                }     //logic for Other Elements
                else
                {

                    if ((id != "") && (id != "undefined"))
                    {


                        obj = commonWebBrowser_Current.driver.FindElementById(id);
                        EmulatedXPath(obj);



                    }
                    else if (name != "" && name != "undefined")
                    {
                        obj = commonWebBrowser_Current.driver.FindElementByName(name);
                        EmulatedXPath(obj);
                    }
                    else if (classname != "" && classname != "undefined")
                    {



                        //obj = commonWebBrowser_Current.driver.FindElementByClassName(classname);
                        EmulatedXPath(elementatpos);

                    }
                    else
                    {

                        EmulatedXPath(elementatpos);
                    }
                }


            }

        }

        void UpdateXPath(object sender, RunWorkerCompletedEventArgs e)
        {

            XPathWebWindow.Webwin.Rtxt_Xpath.Document.Blocks.Clear();
            XPathWebWindow.Webwin.Rtxt_Xpath.AppendText(BotBuilderXPath);
        }


        private void M_GlobalHook_MouseDoubleClickXPath(object sender, MouseEventArgs e)
        {
            try
            {
                using (BackgroundWorker work = new BackgroundWorker())
                { 
                    work.DoWork += RecordXPath;
                work.RunWorkerCompleted += UpdateXPath;
                work.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                //if element is not found in case if its image
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Global Hook Mouse Dbl Click - " + ex + "\n");
            }
        }
        private void M_GlobalHook_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
            //action.SendKeys(OpenQA.Selenium.Keys.Escape);
            //action.SendKeys(OpenQA.Selenium.Keys.Escape);
            //action.SendKeys(OpenQA.Selenium.Keys.Escape);
            //action.SendKeys(OpenQA.Selenium.Keys.Escape);
            
            try
            {

                


                //  ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("alert('Welcome to Guru99');");
                //  String strJavaScript2 = "window.stop();";
                //((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("location.reload();");
                RemoteWebElement element = null;
                //String js2 = "return window.stop";
                //((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(js2);
                  String strJavaScript = "return document.activeElement";
             

                element = (RemoteWebElement)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScript);

                if (element != null)
                {
                    EmulatedClickedControl(element);
                }
            }
            catch (Exception ex)
            {
                //if element is not found in case if its image
            }
        }
        void WorkStopXPathScrap(object sender, DoWorkEventArgs e)
        {
            try
            {
                HookEvents.MouseDoubleClick -= M_GlobalHook_MouseDoubleClickXPath;
                //commonWebBrowser_Current.driver.Navigate().Refresh();
                String strJavaScript = "location.reload();";
                ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteAsyncScript(strJavaScript);
                HookEvents.Dispose();
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Stop XPath - " + ex + "\n");
                HookEvents.Dispose();
            }

        }
        public void StopScrapingXPath(string ApplicationId)
        {
            try
            {
                using (BackgroundWorker work = new BackgroundWorker())
                { 
                    work.DoWork += WorkStopXPathScrap;

                work.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {

             
            }
            

           
            
        }
        public void StopScraping(string ApplicationId)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
                if ((commonWebBrowser_Current == null) || (commonWebBrowser_Current.driver == null))
                {
                    Log.Logger.LogData("IE Instance is not loadded", LogLevel.Error);
                }

                //  m_GlobalHook.MouseMove -= M_GlobalHook_MouseMove;
                m_GlobalHook.MouseDoubleClick -= M_GlobalHook_MouseDoubleClick;
                try
                {
                    //String strJavaScript = "window.removeEventListener('mouseover', ABC); window.removeEventListener('mouseout', EFG);"; //not working
                    //String strJavaScript = "window.detachEvent('mouseover', ABC);window.detachEvent('mouseout', EFG);"; //not working
                    String strJavaScript = "location.reload();";
                    ((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteAsyncScript(strJavaScript);
                }
                catch (Exception ex)
                {


                }
            }
        }


        private bool EmulatedClickedControl(RemoteWebElement evtObj)
        {
            bool iDNameClassName = false;
            WebControlProperties spp = new WebControlProperties();
            try
            {
                bool bFound = false;

                string strElement = string.Empty;

                string xPath = string.Empty;

                if ((SelectHelper.StartSimulation) && (SelectHelper.CurrentScrapMode == ScrapMode.Web) && (evtObj != null))
                {
                    SelectHelper.StartSimulation = false;

                    try
                    {
                        spp.ControlId = evtObj.GetAttribute("id");
                        iDNameClassName = true;
                    }
                    catch (Exception ex) { }
                    try
                    {
                        spp.ControlName = evtObj.GetAttribute("name");
                        string currxpath = generateXPATH(evtObj, "");
                        spp.XPath = currxpath;
                        iDNameClassName = true;
                    }
                    catch (Exception ex) { }

                    try
                    {
                        if ((evtObj.GetAttribute("class") != null) && (evtObj.GetAttribute("class") != string.Empty) && (evtObj.GetAttribute("class").Trim().Length > 0))
                            spp.ClassName = evtObj.GetAttribute("class");
                        iDNameClassName = true;
                    }
                    catch (Exception ex) { }

                    spp.TagName = evtObj.TagName;

                    //try
                    //{
                    //    string value = IEWATIN_Current.ActiveElement.GetAttributeValue("Type");
                    //    if (value.ToLower() == "submit")
                    //    {
                    //        spp.IsEventField = true;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    //Type attribute is not available
                    //}

                    try
                    {
                        string innerhtml = evtObj.GetAttribute("innerHTML");
                    }
                    catch (Exception ex) { }


                    spp.DisplayName = evtObj.Text;
                    try
                    {
                        if (iDNameClassName == false)
                        {
                            string strJavaScriptXpath = "return function getPathTo(){var element = document.activeElement; if (element.id !== '') { return \"~~*[@id='\" + element.id + \"']\"; }if (element ==document.body) { return element.tagName.toLowerCase(); }var ix = 0; var siblings = element.parentNode.childNodes;for (var i = 0; i < siblings.length; i++){var sibling = siblings[i];if (sibling ==element) {return getPathTo(element.parentNode) + '/' + element.tagName.toLowerCase() + '[' + (ix + 1) + ']';if (sibling.nodeType ==1 && sibling.tagName ==element.tagName) { ix++; }}}}";
                            string strXpath = (string)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScriptXpath);
                            spp.XPath = strXpath;
                        }


                        IWebElement parent = evtObj.FindElementByXPath("..");
                        if (parent != null)
                        {
                            try
                            {
                                spp.ParentId = parent.GetAttribute("id");
                            }
                            catch (Exception ex) { }
                            try
                            {
                                spp.ParentName = parent.GetAttribute("name");
                            }
                            catch (Exception ex) { }

                            try
                            {
                                if ((parent.GetAttribute("class") != null) && (parent.GetAttribute("class") != string.Empty) && (parent.GetAttribute("class").Trim().Length > 0))
                                    spp.ParentClassName = evtObj.GetAttribute("class");
                            }
                            catch (Exception ex) { }

                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        IWebElement followingSibling = evtObj.FindElementByXPath("following-sibling::*[1]");
                        if (followingSibling != null)
                        {
                            try
                            {
                                spp.NextSiblingId = followingSibling.GetAttribute("id");
                            }
                            catch (Exception ex) { }
                            try
                            {
                                spp.NextSiblingName = followingSibling.GetAttribute("name");
                            }
                            catch (Exception ex) { }

                            //if ((followingSibling.GetAttribute("class") != null) && (followingSibling.GetAttribute("class") != string.Empty) && (followingSibling.GetAttribute("class").Trim().Length > 0))
                            //    spp.ParentSiblingClassName = followingSibling.GetAttribute("class");
                            try
                            {
                                spp.NextSiblingId = followingSibling.GetAttribute("id");
                            }
                            catch (Exception ex) { }
                        }
                    }
                    catch (Exception ex) { }



                    try
                    {
                        IWebElement prevSibling = evtObj.FindElementByXPath("preceding-sibling::*[1]");
                        if (prevSibling != null)
                        {
                            try
                            {
                                spp.PreviousSiblingId = prevSibling.GetAttribute("id");
                            }
                            catch (Exception ex) { }
                            try
                            {
                                spp.PreviousSiblingName = prevSibling.GetAttribute("name");
                            }
                            catch (Exception ex) { }
                            try
                            {
                                if ((prevSibling.GetAttribute("class") != null) && (prevSibling.GetAttribute("class") != string.Empty) && (prevSibling.GetAttribute("class").Trim().Length > 0))
                                    spp.PreviousSiblingClassName = prevSibling.GetAttribute("class");
                            }
                            catch (Exception ex) { }

                            try
                            {
                                if (evtObj.GetAttribute("title") != null)
                                {
                                    spp.Title = evtObj.GetAttribute("title");
                                }
                            }
                            catch (Exception ex) { }
                            spp.Y = evtObj.Location.Y;
                            spp.X = evtObj.Location.X;
                            if (evtObj.GetAttribute("xpath") != null)
                            {
                                spp.XPath = evtObj.GetAttribute("xpath");
                            }
                            // IHTMLElement targetElement = htmlDoc.elementFromPoint(spp.X, spp.Y); //for testing  - it works


                            //if (evtObj != null)
                            //{
                            //    spp.SourceIndex = ;

                            //}
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    SelectHelper.CurrentPluginScrapeProperties.Add(spp);
                    ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
                    scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
                    SelectHelper.OnScraping(scrapingEventArgs);
                    //evtObj.cancelBubble = true;
                    //evtObj.returnValue = false;
                    SelectHelper.StartSimulation = true;

                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
            }
            return false;
        }

        //private void OnCloseIE(IECreatedEventArgs e)
        //{

        //    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(e.ApplicationId))
        //    {
        //        ThreadInvoker.Instance.RunByUiThread(() =>
        //        {
        //            ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(1);
        //        });
        //        WatiN.Core.IE IEWATIN_Old = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[e.ApplicationId];
        //        SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(e.ApplicationId);
        //        RemoveHandlers(IEWATIN_Old, e.ApplicationId);
        //        ReleasetHighlightedItems();
        //        IEWATIN_Old.Dispose();
        //        GC.Collect();
        //    }
        //}

        public void CloseXPathApplication(string ApplicationId)
        {


            try
            {
                HookEvents.Dispose();
                commonWebBrowser_Current.driver.Close();
                commonWebBrowser_Current.driver.Dispose();
              //  GC.Collect();
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Close XPAth - " + ex + "\n");
            }
            
        }
        public void CloseApplication(string ApplicationId)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                CommonWebBrowser IEWATIN_Old = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(ApplicationId);
                // RemoveHandlers(IEWATIN_Old, ApplicationId);
                //        ReleasetHighlightedItems();
                IEWATIN_Old.driver.Close();
                IEWATIN_Old.driver.Dispose();
                GC.Collect();
            }
        }

        public void Dispose()
        {
            SelectHelper.LaunchScraping -= LaunchScraping;
            // SelectHelper.IECreated -= Ie_DocumentComplete;
            SelectHelper.Reset -= Reset;
        }

        private String generateXPATH(IWebElement childElement, String current)
        {
            try
            {
                String childTag = childElement.TagName;  //GetTagName();
                if (childTag.Equals("html"))
                {
                    return "/html[1]" + current;
                }
                IWebElement parentElement = childElement.FindElement(By.XPath(".."));
                IList<IWebElement> childrenElements = parentElement.FindElements(By.XPath("*"));
                int count = 0;
                for (int i = 0; i < childrenElements.Count; i++)
                {
                    IWebElement childrenElement = childrenElements[i];
                    String childrenElementTag = childrenElement.TagName;  //GetTagName();
                    if (childTag.Equals(childrenElementTag))
                    {
                        count++;
                    }
                    if (childElement.Equals(childrenElement))
                    {
                        return generateXPATH(parentElement, "/" + childTag + "[" + count + "]" + current);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "generate xpath - " + ex + "\n");
                return null;
            }
        }

        //for XPath using button on MainWindow
        private bool EmulatedXPath(IWebElement evtObj)
        {
          
          
            try
            {
                string currxpath = generateXPATH(evtObj, "");
                BotBuilderXPath = currxpath;
               

                
            }
            catch (Exception ex)
            {
                // Log.Logger.LogData(ex.Message, LogLevel.Error);
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Emulated XPath - " + ex + "\n");

            }
            return false;
        }
        public bool LaunchBrowser(string AppId, string browsertype, string launchUrl, int timeInSec, bool launchResult, bool NoScrape = false, string TitleOrUrl = "", string strWaitUntilContainText = "")
        {
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
            CommonWebBrowser commonWebBrowser = null;
            try
            {
                int icount = 0;

               
                if ((TitleOrUrl != "") && (!string.IsNullOrEmpty(TitleOrUrl)))
                {
                    
                }
                else if (AppId != string.Empty) //scraping time
                {
                    
                       

                        if (commonWebBrowser != null)
                        {
                            commonWebBrowser.ApplicationId = AppId;
                         
                            commonWebBrowser.driver.Url = launchUrl;
                         
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, commonWebBrowser);
                   
                        }
                        else
                        {
                            commonWebBrowser = new CommonWebBrowser(browsertype);
                    
                            commonWebBrowser.SearchUrl = launchUrl;
                            commonWebBrowser.ApplicationId = AppId;
                            commonWebBrowser.LaunchWebBrowser(launchUrl, timeInSec);
              

                            commonWebBrowser.driver.Manage().Window.Maximize();

                        //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, commonWebBrowser);
                        
                        }
                                        
                    
                   
                }
                else
                {
                 
                    Logger.Log.Logger.LogData("Please enter ApplicationID", LogLevel.Error);
               

                }
                if (commonWebBrowser != null)
                {
                    //SelectHelper.CurrentAppProcessId = commonWebBrowser.driver.CurrentWindowHandle;
                }
              


                launchResult = true;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    Log.Logger.LogData("Timeout loading page after " + timeInSec + " Second in activity Web_OpenApplication", LogLevel.Info);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity Web_OpenApplication", LogLevel.Error);
                }


            }
            //});
            return launchResult;
        }

       
    }
}
