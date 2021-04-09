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
using System;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using WatiN.Core;
using WatiN.Core.Native.Windows;

namespace Bot.Activity.InternetExplorer
{
    [Export(typeof(ICustomPluginInterface))]
    public sealed class IExplorer: ICustomPluginInterface
    {
        public double X { get; set; }
        public double Y { get; set; }
        private static IExplorer instance = null;
        private bool readystate = false;
        public bool NonScrapeMode { get; set; }
        private static readonly object padlock = new object();
        HTMLDocument htmlDoc = null;
        int iOpenFile = 0;
        int iReset = 0;
        int iIECreated = 0;
       // IExplorerHelper IEHelper = new IExplorerHelper();
        private IExplorer()
        {
            try
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
                SelectHelper.LaunchScraping -= LaunchScraping;
                SelectHelper.LaunchScraping += LaunchScraping;
               // iOpenFile = iOpenFile + 1;

                if (iReset > 0)
                {
                    for (int i = 0; i <= iReset; i++)
                    {
                        SelectHelper.Reset -= Reset;
                    }
                }
                iReset = 0;
                SelectHelper.Reset -= Reset;
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
                SelectHelper.IECreated -= Ie_DocumentComplete;
                SelectHelper.IECreated += Ie_DocumentComplete;
                iIECreated = iIECreated + 1;
               // CreateBlankIEInstance();
                
            }
            catch(Exception ex)
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
            try
            {
                int icount = myRedElements.Count;
                //foreach (mshtml.IHTMLStyle item in myRedElements)
                for (int i = 0; i < icount; i++)
                {
                    mshtml.IHTMLStyle item = myRedElements[i];
                    if (orgColor.ContainsKey(item))
                        item.borderColor = orgColor[item];
                    if (orgBorder.ContainsKey(item))
                        item.border = orgBorder[item];
                }

                myRedElements.Clear();
                orgColor.Clear();
                orgBorder.Clear();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        public static IExplorer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new IExplorer();
                           
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
                string strWaitUntilContainText = string.Empty;
                int iTimeInSec = 0;
                bool launchResult = false;
                var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
                IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

                if (!string.IsNullOrEmpty(e.strTitleOrUrlToAttach) && (e.strTitleOrUrlToAttach.Trim().Length > 0))
                {
                    launchResult = LaunchIE(e.ApplicationID, TitleOrUrl, iTimeInSec, launchResult, false, e.strTitleOrUrlToAttach,e.strWaitUntilContainText);
                }
                else
                {
                    foreach (ModelItem item in modelItems)
                    {
                        if (item.ItemType.FullName == "Bot.Activity.InternetExplorer.IE_OpenApplication")
                        {
                            IE_OpenApplication owa = (IE_OpenApplication)item.GetCurrentValue();
                            ScrapApplicationId = owa.ApplicationID.Expression.ToString();
                            if (owa.WaitUntilContainText != null)
                            {
                                strWaitUntilContainText = owa.WaitUntilContainText.Expression.ToString();
                            }
                                if (owa.TimeOutInSecond.Expression != null)
                            {
                                int iTimeOut = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                            }
                           
                            TitleOrUrl = e.strTitleOrUrlToAttach;
                            if (ScrapApplicationId == e.ApplicationID)
                            {
                                LaunchUrl = owa.Url.Expression.ToString();
                                iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                                launchResult = LaunchIE(ScrapApplicationId, LaunchUrl, iTimeInSec, launchResult, false, TitleOrUrl, strWaitUntilContainText);
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
                //    if (item.ItemType.FullName == "Bot.Activity.InternetExplorer.IE_OpenApplication")
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
        private bool ResetIE(string AppId )
        {
            WatiN.Core.IE IEWATIN = null;
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        IEWATIN.GoTo(IEWATIN.SearchUrl);
                        IEWATIN.WaitForComplete(5000);
                        
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
        
        public bool LaunchIE(string AppId, string launchUrl, int timeInSec, bool launchResult,bool NoScrape = false,string TitleOrUrl="", string strWaitUntilContainText = "")
        {
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
                WatiN.Core.IE IEWATIN = null;
                try
                {
                    int icount = 0;

                    if (NoScrape == true)
                    {
                    SelectHelper.StartSimulation = false;
                        TitleOrUrl = "";
                    }                
                    if ((TitleOrUrl != "")&&(!string.IsNullOrEmpty(TitleOrUrl)))
                    {
                        IEWATIN = IE.AttachTo<IE>(Find.ByUrl(TitleOrUrl));
                        
                        if (IEWATIN == null)
                        {
                            IEWATIN = IE.AttachTo<IE>(Find.ByTitle(TitleOrUrl));
                            if (IEWATIN == null)
                            {
                                Log.Logger.LogData("Not able to attach by titile or url:" + TitleOrUrl, LogLevel.Error);
                                //return false;
                            }
                        }
                        IEWATIN.ApplicationId = AppId;
                        IEWATIN.SearchUrl = TitleOrUrl;
                        if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                        {
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId] = IEWATIN;
                        }
                        else
                        {
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, IEWATIN);
                        }
                    }
                    else if (AppId != string.Empty) //scraping time
                    {
                        //IE.AttachToIE(Find.ByUrl(""));
                        //IE.AttachToIE(Find.ByUri(""));
                        //IE.AttachToIE(Find.ByTitle(""));
                        //IE.AttachToIE(Find.By("hwnd", windowHandle));
                        //for windows: https://www.codeproject.com/Articles/141842/Automate-your-UI-using-Microsoft-Automation-Framew
                   
                        if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                        {
                            //if (IEWATIN_New == null)
                            //{
                            //    taskA.Wait();
                            //}
                            //IEWATIN = IEWATIN_New;
                            //IEWATIN_New = null;
                       
                            if (IEWATIN != null)
                            {
                                IEWATIN.ApplicationId = AppId;
                                //IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.Show);
                                IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);
                                IEWATIN.BringToFront();
                                IEWATIN.GoTo(launchUrl);
                            if (!string.IsNullOrEmpty(strWaitUntilContainText) && (strWaitUntilContainText != ""))
                            {
                                IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                            }
                            else
                            {
                                IEWATIN.WaitForComplete(5000);
                            }
                            SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, IEWATIN);
                               // CreateBlankIEInstance();
                            }
                            else
                            {
                                IEWATIN = new IE();
                                IEWATIN.SearchUrl = launchUrl;
                                IEWATIN.ApplicationId = AppId;
                                IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);
                                IEWATIN.BringToFront();
                                IEWATIN.GoTo(launchUrl);
                            if (!string.IsNullOrEmpty(strWaitUntilContainText) && (strWaitUntilContainText != ""))
                            {
                                IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                            }
                            else
                            {
                                IEWATIN.WaitForComplete(5000);
                            }
                            //IEWATIN.WaitForComplete(5000);
                                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(AppId, IEWATIN);
                                //CreateBlankIEInstance();
                            }
                            AutomationElement window = AutomationElement.FromHandle(IEWATIN.hWnd);
                          //  Automation.AddAutomationEventHandler(WindowPattern.WindowClosedEvent, window, TreeScope.Element, IEWATIN.OnApplicationClose);
                            //IEWATIN = new IE(new Uri(launchUrl), true);                        
                        }
                        else
                        {
                            IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                            // IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.Show);
                            IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);
                            IEWATIN.BringToFront();
                    }
                    }
                    else
                    {
                        //IEWATIN = new IE(new Uri(launchUrl), true);
                        //IEWATIN.WaitForComplete(5000);
                        Logger.Log.Logger.LogData("Please enter ApplicationID", LogLevel.Error);
                      //  SelectHelper._wfDesigner.Context.
                        
                    }
                
                    SelectHelper.CurrentPlugin = this;
               
            
                launchResult = true;
                }catch(Exception ex)
                {
                    Log.Logger.LogData(ex.Message, LogLevel.Error);
                }
           //});
            return launchResult;
        }
        WatiN.Core.IE IEWATIN_Current = null;
        public void StartScraping( string AppId)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                IEWATIN_Current = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                if ((IEWATIN_Current == null) || (IEWATIN_Current.InternetExplorer == null))
                {
                    Log.Logger.LogData("IE Instance is not loadded", LogLevel.Error);
                }

                AddHandlers(IEWATIN_Current);
            }
        }

        HTMLDocumentEvents2_Event htmldoc2;
        private void AddHandlers(IE IEWATIN_Current)
        {
            bool readystate = false;
           
            SHDocVw.InternetExplorer iexplorer = (SHDocVw.InternetExplorer)IEWATIN_Current.InternetExplorer;
            htmldoc2 = (HTMLDocumentEvents2_Event)iexplorer.Document;

            if (htmldoc2 != null)
            {
                //if(IEWATIN==null)
                //{
                //    if(IETasks.ContainsKey(ScrapApplicationId))
                //    {
                //        Task t = IETasks[ScrapApplicationId];
                //        t.Wait();
                //    }
                //}
                //if (IEWATIN != null)
                //{
                //    IEWATIN.ShowWindow(NativeMethods.WindowShowStyle.Show);
                //    IEWATIN.WaitForComplete(5000);
                try
                {
                    htmldoc2.onmousemove -= new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                    htmldoc2.ondblclick -= new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                    htmldoc2.onfocusout -= new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);


                    htmldoc2.onmousemove += new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                    htmldoc2.ondblclick += new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                    htmldoc2.onfocusout += new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);
                }
                catch (Exception ex)
                {

                }
                //}else
                //{
                //    Log.Logger.LogData("Error in launch", LogLevel.Error);

            }
        }

        private void RemoveHandlers(IE IEWATIN_Current, string ApplicationId)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                SHDocVw.InternetExplorer iexplorer = (SHDocVw.InternetExplorer)IEWATIN_Current.InternetExplorer;
                htmldoc2 = (HTMLDocumentEvents2_Event)iexplorer.Document;
                if (htmldoc2 != null)
                {
                    try
                    {
                        htmldoc2.onmousemove -= new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                        htmldoc2.ondblclick -= new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                        htmldoc2.onfocusout -= new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);

                    }
                    catch (Exception ex)
                    {

                    }

                }//}
            }
        }
        public void StopScraping( string ApplicationId)
        {
           
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {
                WatiN.Core.IE IEWATIN_Current = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
                if ((IEWATIN_Current == null) || (IEWATIN_Current.InternetExplorer == null))
                {
                    Log.Logger.LogData("IE Instance is not loadded", LogLevel.Error);
                }
                SHDocVw.InternetExplorer iexplorer = (SHDocVw.InternetExplorer)IEWATIN_Current.InternetExplorer;
                htmldoc2 = (HTMLDocumentEvents2_Event)iexplorer.Document;

                if (htmldoc2 != null)
                {
                    htmldoc2.onmousemove -= new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(Document_MouseMove);
                    htmldoc2.ondblclick -= new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(idocEvent_onclick);
                    htmldoc2.onfocusout -= new mshtml.HTMLDocumentEvents2_onfocusoutEventHandler(Document_Onfocusout);
                    RemoveHighlightOfElements();
                }
            }
        }

        
        //bool completed = false;
        private void Ie_DocumentComplete(object sender, IECreatedEventArgs e)
        {
            if(e.Type == 1)
            {
                OnCloseIE(e);
                return;
            }
            if (e.Type == 2)
            {
                Logger.Log.Logger.LogData("Dynamic DOM,getting closed to prevent automation on this site. Scraping automation mode should be by windows or automation should be done by using tab or image based automation", LogLevel.Warning);
                //if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(e.ApplicationId))
                //{
                //    WatiN.Core.IE IEWATINCurrent = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[e.ApplicationId];
                //    WatiN.Core.IE IEWATINNew = IE.AttachTo<IE>(Find.ByTitle(IEWATINCurrent.Title));
                //    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[e.ApplicationId] = IEWATINNew;
                //    AddHandlers(IEWATINNew);
                //}
            }
        }

          
       
        List<mshtml.IHTMLStyle> myRedElements = new List<mshtml.IHTMLStyle>();
        Dictionary<mshtml.IHTMLStyle, string> orgColor = new Dictionary<mshtml.IHTMLStyle, string>();
        Dictionary<mshtml.IHTMLStyle, string> orgBorder = new Dictionary<mshtml.IHTMLStyle, string>();
        private IDictionary<mshtml.IHTMLElement, string> htmlelementStyles = new Dictionary<mshtml.IHTMLElement, string>();
        private void Document_MouseMove(IHTMLEventObj pEvtObj)
        {
            IHTMLElement htmlElement = null;
            try
            {
                if ((SelectHelper.StartSimulation) && (SelectHelper.CurrentScrapMode == ScrapMode.Web))
                {
                    
                    if (pEvtObj.srcElement != null)
                    {
                        htmlElement = (IHTMLElement)pEvtObj.srcElement;

                        //Neeed to return for unwanted elements
                        //if (htmlElement.tagName.ToLower().Contains("frame"))
                        //{

                        //    return;
                        //}
                        RemoveHighlightOfElements();
                        if (!this.htmlelementStyles.ContainsKey(htmlElement))
                        {
                            string elestyle = pEvtObj.srcElement.style.toString();

                            mshtml.IHTMLStyle istyle = (mshtml.IHTMLStyle)pEvtObj.srcElement.style;
                            HighlightElement(istyle);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
            }
        }


        private void HighlightElement(IHTMLStyle istyle)
        {
            try
            {
                if (istyle.border == null)
                {
                    orgBorder.Add(istyle, "solid 0px");
                }
                else
                {
                    orgBorder.Add(istyle, istyle.border);
                }
                orgColor.Add(istyle, istyle.borderColor);
                istyle.border = "solid 1px";
                istyle.borderColor = "red";

                myRedElements.Add(istyle);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void ReleasetHighlightedItems()
        {
            myRedElements = new List<mshtml.IHTMLStyle>();
            orgColor = new Dictionary<mshtml.IHTMLStyle, string>();
            orgBorder = new Dictionary<mshtml.IHTMLStyle, string>();
            htmlelementStyles = new Dictionary<mshtml.IHTMLElement, string>();
        }
        private bool idocEvent_onclick(IHTMLEventObj pEvtObj)
        {
            bool result = false;
            try
            {
                result = EmulatedClickedControl(pEvtObj);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
            }
            return result;
        }
        private void Document_Onfocusout(IHTMLEventObj pEvtObj)
        {
            try
            {
                foreach (mshtml.IHTMLStyle item in myRedElements)
                {
                    try
                    {
                        if (orgColor.ContainsKey(item))
                            item.borderColor = orgColor[item];
                        if (orgBorder.ContainsKey(item))
                            item.border = orgBorder[item];
                    }
                    catch (Exception ex)
                    {
                        //Error
                    }
                }
                myRedElements.Clear();
                orgColor.Clear();
                orgBorder.Clear();
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

        }
        private bool EmulatedClickedControl(IHTMLEventObj evtObj)
        {
            
            IE_ControlProperties spp = new IE_ControlProperties();
            try
            {
                bool bFound = false;
              
                string strElement = string.Empty;
                if (evtObj.toElement != null)
                {
                    strElement = evtObj.toElement.ToString();
                }
                string xPath = string.Empty;

                if ((SelectHelper.StartSimulation) && (SelectHelper.CurrentScrapMode == ScrapMode.Web) && (evtObj != null))
                {
                    SelectHelper.StartSimulation = false;
                    //dynamic element = (IHTMLElement)evtObj.srcElement;
                    IHTMLElement element = (IHTMLElement)evtObj.srcElement;


                    IHTMLDocument2 htmdoc = (IHTMLDocument2)evtObj.srcElement.document;
                    //htmlDoc.documentElement.innerHTML;
                    //spp.ControlId = element.id;
                    //spp.ControlName = element.name;
                    //spp.AttributeNames = "class = abc";
                    //bool attributeUnique = true;

                    spp.ControlId = IEWATIN_Current.ActiveElement.Id;
                    spp.ControlName = IEWATIN_Current.ActiveElement.GetAttributeValue("name");
                    if((IEWATIN_Current.ActiveElement.ClassName != null)&&(IEWATIN_Current.ActiveElement.ClassName!= string.Empty)&&(IEWATIN_Current.ActiveElement.ClassName.Trim().Length >0))
                         spp.ClassName = IEWATIN_Current.ActiveElement.ClassName;
                    spp.TagName = IEWATIN_Current.ActiveElement.TagName;

                    //try
                    //{
                    //  string value=  IEWATIN_Current.ActiveElement.GetAttributeValue("Type");
                    //    if(value.ToLower() == "submit")
                    //    {
                    //        spp.IsEventField = true;
                    //    }
                    //}catch(Exception ex)
                    //{
                    //    //Type attribute is not available
                    //}


                    string innerhtml = IEWATIN_Current.ActiveElement.InnerHtml;

                    spp.DisplayName = IEWATIN_Current.ActiveElement.Text;
                    if (IEWATIN_Current.ActiveElement.Parent != null)
                    {
                        //spp.ParentSiblingId = IEWATIN_Current.ActiveElement.Parent.Id;
                        //spp.ParentSiblingName = IEWATIN_Current.ActiveElement.Parent.GetAttributeValue("name");
                        //if ((IEWATIN_Current.ActiveElement.Parent.ClassName != null) && (IEWATIN_Current.ActiveElement.Parent.ClassName != string.Empty) && (IEWATIN_Current.ActiveElement.Parent.ClassName.Trim().Length > 0))
                        //    spp.ParentSiblingClassName = IEWATIN_Current.ActiveElement.Parent.ClassName;

                        //Need to get parent xpath

                    }
                    if (IEWATIN_Current.ActiveElement.NextSibling != null)
                    {
                        spp.NextSiblingId = IEWATIN_Current.ActiveElement.NextSibling.Id;
                        spp.NextSiblingName = IEWATIN_Current.ActiveElement.NextSibling.GetAttributeValue("name");
                        //if ((IEWATIN_Current.ActiveElement.NextSibling.ClassName != null) && (IEWATIN_Current.ActiveElement.NextSibling.ClassName != string.Empty) && (IEWATIN_Current.ActiveElement.NextSibling.ClassName.Trim().Length > 0))
                        //    spp.ParentSiblingClassName = IEWATIN_Current.ActiveElement.NextSibling.ClassName;

                    }
                    if (IEWATIN_Current.ActiveElement.PreviousSibling != null)
                    {
                        spp.PreviousSiblingId = IEWATIN_Current.ActiveElement.PreviousSibling.Id;
                        spp.PreviousSiblingName = IEWATIN_Current.ActiveElement.PreviousSibling.GetAttributeValue("name");
                        if ((IEWATIN_Current.ActiveElement.PreviousSibling.ClassName != null) && (IEWATIN_Current.ActiveElement.PreviousSibling.ClassName != string.Empty) && (IEWATIN_Current.ActiveElement.PreviousSibling.ClassName.Trim().Length > 0))
                            spp.PreviousSiblingClassName = IEWATIN_Current.ActiveElement.PreviousSibling.ClassName;
                    }
                    if (IEWATIN_Current.ActiveElement.Title != null)
                    {
                        spp.Title = IEWATIN_Current.ActiveElement.Title;
                        spp.Title = IEWATIN_Current.ActiveElement.Title;
                    }
                    if (element != null)
                    {

                        spp.Y = element.offsetTop;
                        spp.X = element.offsetLeft;
                        // IHTMLElement targetElement = htmlDoc.elementFromPoint(spp.X, spp.Y); //for testing  - it works

                    }
                   

                    if (element != null)
                    {
                       spp.SourceIndex = element.sourceIndex;
                       //Element ele = IEWATIN_Current.Elements.First(Find.ByIndex(spp.SourceIndex));//for testing  - it works

                    }
                   
                    IHTMLElement elementIHtml = (IHTMLElement)evtObj.srcElement;
                    string fieldType = element.GetType().ToString();
                    fieldType = fieldType.Replace("HTML", "");
                    fieldType = fieldType.Contains("ElementClass") ? fieldType.Replace("ElementClass", "").ToLower() : fieldType.Replace("Class", "").ToLower();
                    string outerxml = string.Empty;

                    bool prioritydummy = false;
                    xPath = IExplorerHelper.GetXPath(element,ref prioritydummy);
                   
                    xPath = string.IsNullOrEmpty(xPath) ? xPath : xPath.Remove(xPath.Length - 1, 1);
                    if (xPath.Trim().Length > 0)
                    {
                        //int xpathindex = GetIndexByXPath(xPath);
                        spp.XPath =Convert.ToString(xPath);
                       
                    }
                
                    SelectHelper.CurrentPluginScrapeProperties.Add(spp);
                    ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
                    scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
                    SelectHelper.OnScraping(scrapingEventArgs);
                    evtObj.cancelBubble = true;
                    evtObj.returnValue = false;
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

        private void OnCloseIE(IECreatedEventArgs e)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(e.ApplicationId))
            {
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(1);
                });
                WatiN.Core.IE IEWATIN_Old = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[e.ApplicationId];
                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(e.ApplicationId);
                RemoveHandlers(IEWATIN_Old, e.ApplicationId);
                ReleasetHighlightedItems();
                IEWATIN_Old.Dispose();
                GC.Collect();
            }
        }
        public void CloseApplication(string ApplicationId)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            {               
                WatiN.Core.IE IEWATIN_Old = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationId];
                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(ApplicationId);
                RemoveHandlers(IEWATIN_Old, ApplicationId);
                ReleasetHighlightedItems();
                IEWATIN_Old.Close();
                IEWATIN_Old.Dispose();
                GC.Collect();
            }
        }

        public void Dispose()
        {
            SelectHelper.LaunchScraping -= LaunchScraping;
             SelectHelper.IECreated -= Ie_DocumentComplete;
            SelectHelper.Reset -= Reset;
        }

      
    }
}
