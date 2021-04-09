// <copyright file=MainframePlugin company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:58</date>
// <summary></summary>

using CommonLibrary;
using CommonLibrary.Interfaces;
using Logger;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Bot.Activity.Mainframe
{
    [Export(typeof(ICustomPluginInterface))]
    public sealed class MainframePlugin: ICustomPluginInterface
    {
        public double X { get; set; }
        public double Y { get; set; }
        private static MainframePlugin instance = null;
        private bool readystate = false;
        public bool NonScrapeMode { get; set; }
        private static readonly object padlock = new object();
        
        int iOpenFile = 0;
        int iReset = 0;
        int iIECreated = 0;
        MainframeHelper IEHelper = new MainframeHelper();


        private readonly KeyboardHookListener m_KeyboardHookManager;
        private readonly MouseHookListener m_MouseHookManager;

        private MainframePlugin()
        {
           // AssemblyName = "InternetExplorer";
            if (iOpenFile > 0)
            {
                for (int i = 0; i <= iOpenFile; i++)
                {
                    SelectHelper.LaunchScraping -= LaunchScraping;
                }
            }
            iOpenFile = 0;
            SelectHelper.LaunchScraping += LaunchScraping;
            iOpenFile = iOpenFile + 1;

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


            iIECreated = 0;
            if (iIECreated > 0)
            {
                for (int i = 0; i < iIECreated; i++)
                {
                    SelectHelper.IECreated -= Ie_DocumentComplete;
                }
            }
            iIECreated = 0;
            SelectHelper.IECreated += Ie_DocumentComplete;
            iIECreated = iIECreated + 1;

           string Mode =  ConfigurationManager.AppSettings.Get("Mode");
            if ((Mode.ToLower() == "Designer"))
            {
                m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
                m_KeyboardHookManager.Enabled = true;
                m_KeyboardHookManager.KeyPress += M_KeyboardHookManager_KeyPress;

                m_MouseHookManager = new MouseHookListener(new GlobalHooker());
                m_MouseHookManager.Enabled = true;
                m_MouseHookManager.MouseMove += M_MouseHookManager_MouseMove;
                m_MouseHookManager.MouseClick += M_MouseHookManager_MouseClick;
            }
        }

        private void M_MouseHookManager_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void M_MouseHookManager_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void M_KeyboardHookManager_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            throw new NotImplementedException();
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
        private void RemoveHighlightOfElements()
        {
            //int icount = myRedElements.Count;
            
            //for (int i = 0; i < icount; i++)
            //{
            //    mshtml.IHTMLStyle item = myRedElements[i];
            //    if (orgColor.ContainsKey(item))
            //        item.borderColor = orgColor[item];
            //    if (orgBorder.ContainsKey(item))
            //        item.border = orgBorder[item];
            //}
           
            //myRedElements.Clear();
            //orgColor.Clear();
            //orgBorder.Clear();
        }
        public static MainframePlugin Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MainframePlugin();
                           
                        }
                    }
                }
                return instance;
            }
        }
       
        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            string LaunchUrl = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
            bool launchResult = false;
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

            foreach (ModelItem item in modelItems)
            {
                if (item.ItemType.FullName == "Bot.Activity.Mainframe.OpenMainframeApplication")
                {
                    //OpenMainframeApplication owa = (OpenMainframeApplication)item.GetCurrentValue();
                    //ApplicationId = owa.ApplicationID.Expression.ToString();
                    //if (ApplicationId == e.ApplicationID)
                    //{
                    //    LaunchUrl = owa.LoginUrl.Expression.ToString();
                    //    // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
                    //    iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                    //    launchResult = LaunchMainframe(ApplicationId,LaunchUrl, iTimeInSec, launchResult,false);
                    //}
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
                if (item.ItemType.FullName == "Mainframe.OpenMainframeApplication")
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
        //WatiN.Core.IE IEWATIN = null;
        public bool LaunchMainframe(string AppId, string launchUrl, int timeInSec, bool launchResult,bool NoScrape = false)
        {
            NonScrapeMode = NoScrape;
            //if (AppId != string.Empty) //scraping time
            //{
            //    if (!RuntimeApplicationHelper.Instance.RuntimeApplicationObjects.ContainsKey(AppId))
            //    {
            //        IEWATIN = new IE(new Uri(launchUrl), true);                   
            //        RuntimeApplicationHelper.Instance.RuntimeApplicationObjects.Add(AppId, IEWATIN);
            //    }
            //    else
            //    {
            //        IEWATIN =(WatiN.Core.IE) RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId];
            //    }
            //}
            //else
            //{
            //    IEWATIN = new IE(new Uri(launchUrl), true);
            //}
            //IEWATIN.WaitForComplete(5000);

            SelectHelper.CurrentPlugin = this;
            if (NoScrape == true)
            {
                SelectHelper.StartSimulation = false;
            }
            else
            {
                // ie.DocumentComplete += Ie_DocumentComplete;
            }
            launchResult = true;
              
            //});
            return launchResult;
        }

       // HTMLDocumentEvents2_Event htmldoc2;
        bool completed = false;
        private void Ie_DocumentComplete(object sender, IECreatedEventArgs e)
        {
            if (completed == true)
                return;
            try
            {
               
                int iLoad = 0;
                //if (e.ie == null || e.ie.Document == null) return;

                //while (iLoad < 50)
                //{
                //    if (e.ie.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)
                //    {
                //        Thread.Sleep(500);
                //        iLoad = iLoad + 1;
                //        continue;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                //Thread.Sleep(1000);
                //completed = true;
                //Handle = (IntPtr)e.ie.HWND;
                //htmldoc2 = (HTMLDocumentEvents2_Event)e.ie.Document;
                //htmlDoc = e.ie.Document;
                //if (NonScrapeMode == false)
                //{
                //    htmldoc2.onmousemove -= new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                //    htmldoc2.ondblclick -= new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                //    htmldoc2.onfocusout -= new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);


                //    htmldoc2.onmousemove += new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                //    htmldoc2.ondblclick += new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                //    htmldoc2.onfocusout += new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);
                //}
                
            }
            catch (Exception ex)
            {
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                return;
            }
        }
       
        //List<mshtml.IHTMLStyle> myRedElements = new List<mshtml.IHTMLStyle>();
        //Dictionary<mshtml.IHTMLStyle, string> orgColor = new Dictionary<mshtml.IHTMLStyle, string>();
        //Dictionary<mshtml.IHTMLStyle, string> orgBorder = new Dictionary<mshtml.IHTMLStyle, string>();
        //private IDictionary<mshtml.IHTMLElement, string> htmlelementStyles = new Dictionary<mshtml.IHTMLElement, string>();
        //private void Document_MouseMove(IHTMLEventObj pEvtObj)
        //{
        //    IHTMLElement htmlElement = null;

        //    try
        //    {
        //        if (SelectHelper.StartSimulation)
        //        {
                    
        //            if (pEvtObj.srcElement != null)
        //            {
        //                htmlElement = (IHTMLElement)pEvtObj.srcElement;

        //                //Neeed to return for unwanted elements
        //                //if (htmlElement.tagName.ToLower().Contains("frame"))
        //                //{

        //                //    return;
        //                //}
        //                RemoveHighlightOfElements();
        //                if (!this.htmlelementStyles.ContainsKey(htmlElement))
        //                {
        //                    string elestyle = pEvtObj.srcElement.style.toString();

        //                    mshtml.IHTMLStyle istyle = (mshtml.IHTMLStyle)pEvtObj.srcElement.style;
        //                    HighlightElement(istyle);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //        //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
        //    }
        //}


        //private void HighlightElement(IHTMLStyle istyle)
        //{
        //    if (istyle.border == null)
        //    {
        //        orgBorder.Add(istyle, "solid 0px");
        //    }
        //    else
        //    {
        //        orgBorder.Add(istyle, istyle.border);
        //    }
        //    orgColor.Add(istyle, istyle.borderColor);
        //    istyle.border = "solid 1px";
        //    istyle.borderColor = "red";

        //    myRedElements.Add(istyle);
        //}
        //private bool idocEvent_onclick(IHTMLEventObj pEvtObj)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = EmulatedClickedControl(pEvtObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //        //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
        //    }
        //    return result;
        //}
        //private void Document_Onfocusout(IHTMLEventObj pEvtObj)
        //{
        //    foreach (mshtml.IHTMLStyle item in myRedElements)
        //    {
        //        try
        //        {
        //            if (orgColor.ContainsKey(item))
        //                item.borderColor = orgColor[item];
        //            if (orgBorder.ContainsKey(item))
        //                item.border = orgBorder[item];
        //        }
        //        catch (Exception ex)
        //        {
        //            //Error
        //        }
        //    }
        //    myRedElements.Clear();
        //    orgColor.Clear();
        //    orgBorder.Clear();

        //}
        //private bool EmulatedClickedControl(IHTMLEventObj evtObj)
        //{
            
        //    WebControlProperties spp = new WebControlProperties();
        //    try
        //    {
        //        bool bFound = false;
              
        //        string strElement = string.Empty;
        //        if (evtObj.toElement != null)
        //        {
        //            strElement = evtObj.toElement.ToString();
        //        }
        //        string xPath = string.Empty;

        //        if (SelectHelper.StartSimulation && (evtObj != null))
        //        {
        //            SelectHelper.StartSimulation = false;
        //            //dynamic element = (IHTMLElement)evtObj.srcElement;
        //            IHTMLElement element = (IHTMLElement)evtObj.srcElement;


        //            IHTMLDocument2 htmdoc = (IHTMLDocument2)evtObj.srcElement.document;
        //            //htmlDoc.documentElement.innerHTML;
        //            //spp.ControlId = element.id;
        //            //spp.ControlName = element.name;
        //            //spp.AttributeNames = "class = abc";
        //            //bool attributeUnique = true;

        //            spp.ControlId = IEWATIN.ActiveElement.Id;
        //            spp.ControlName = IEWATIN.ActiveElement.GetAttributeValue("name");
        //            if((IEWATIN.ActiveElement.ClassName != null)&&(IEWATIN.ActiveElement.ClassName!= string.Empty)&&(IEWATIN.ActiveElement.ClassName.Trim().Length >0))
        //                 spp.ClassName = IEWATIN.ActiveElement.ClassName;
        //            spp.TagName = IEWATIN.ActiveElement.TagName;
        //            spp.DisplayName = IEWATIN.ActiveElement.Text;
        //            if (IEWATIN.ActiveElement.Parent != null)
        //            {
        //                spp.ParentSiblingId = IEWATIN.ActiveElement.Parent.Id;
        //                spp.ParentSiblingName = IEWATIN.ActiveElement.Parent.GetAttributeValue("name");
        //                if ((IEWATIN.ActiveElement.Parent.ClassName != null) && (IEWATIN.ActiveElement.Parent.ClassName != string.Empty) && (IEWATIN.ActiveElement.Parent.ClassName.Trim().Length > 0))
        //                    spp.ParentSiblingClassName = IEWATIN.ActiveElement.Parent.ClassName;

        //                //Need to get parent xpath

        //            }
        //            if (IEWATIN.ActiveElement.NextSibling != null)
        //            {
        //                spp.NextSiblingId = IEWATIN.ActiveElement.NextSibling.Id;
        //                spp.NextSiblingName = IEWATIN.ActiveElement.NextSibling.GetAttributeValue("name");
        //                if ((IEWATIN.ActiveElement.NextSibling.ClassName != null) && (IEWATIN.ActiveElement.NextSibling.ClassName != string.Empty) && (IEWATIN.ActiveElement.NextSibling.ClassName.Trim().Length > 0))
        //                    spp.ParentSiblingClassName = IEWATIN.ActiveElement.NextSibling.ClassName;

        //            }
        //            if (IEWATIN.ActiveElement.PreviousSibling != null)
        //            {
        //                spp.PreviousSiblingId = IEWATIN.ActiveElement.PreviousSibling.Id;
        //                spp.PreviousSiblingName = IEWATIN.ActiveElement.PreviousSibling.GetAttributeValue("name");
        //                if ((IEWATIN.ActiveElement.PreviousSibling.ClassName != null) && (IEWATIN.ActiveElement.PreviousSibling.ClassName != string.Empty) && (IEWATIN.ActiveElement.PreviousSibling.ClassName.Trim().Length > 0))
        //                    spp.PreviousSiblingClassName = IEWATIN.ActiveElement.PreviousSibling.ClassName;
        //            }
        //            if (IEWATIN.ActiveElement.Title != null)
        //            {
        //                spp.Title = IEWATIN.ActiveElement.Title;
        //                spp.Title = IEWATIN.ActiveElement.Title;
        //            }
        //            if (element != null)
        //            {

        //                spp.Y = element.offsetTop;
        //                spp.X = element.offsetLeft;
        //                // IHTMLElement targetElement = htmlDoc.elementFromPoint(spp.X, spp.Y); //for testing  - it works

        //            }
                   

        //            if (element != null)
        //            {
        //               spp.SourceIndex = element.sourceIndex;
        //               //Element ele = IEWATIN.Elements.First(Find.ByIndex(spp.SourceIndex));//for testing  - it works

        //            }
                   
        //            IHTMLElement elementIHtml = (IHTMLElement)evtObj.srcElement;
        //            string fieldType = element.GetType().ToString();
        //            fieldType = fieldType.Replace("HTML", "");
        //            fieldType = fieldType.Contains("ElementClass") ? fieldType.Replace("ElementClass", "").ToLower() : fieldType.Replace("Class", "").ToLower();
        //            string outerxml = string.Empty;

        //            bool prioritydummy = false;
        //            xPath = IExplorerHelper.GetXPath(element,ref prioritydummy);
                   
        //            xPath = string.IsNullOrEmpty(xPath) ? xPath : xPath.Remove(xPath.Length - 1, 1);
        //            if (xPath.Trim().Length > 0)
        //            {
        //                //int xpathindex = GetIndexByXPath(xPath);
        //                spp.XPath =Convert.ToString(xPath);
                       
        //            }
                
        //            SelectHelper.CurrentPluginScrapeProperties.Add(spp);
        //            ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
        //            scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
        //            SelectHelper.OnScraping(scrapingEventArgs);
        //            evtObj.cancelBubble = true;
        //            evtObj.returnValue = false;
        //            SelectHelper.StartSimulation = true;
                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //        //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
        //    }

        //    return false;
        //}
      
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void CloseApplication(string ApplicationId)
        {
            throw new NotImplementedException();
        }

        public void StartScraping( string ApplicationId)
        {
            throw new NotImplementedException();
        }

        public void StopScraping( string ApplicationId)
        {
            throw new NotImplementedException();
        }
    }
}
