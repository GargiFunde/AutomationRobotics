using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Activity.Web;
using OpenQA.Selenium.Remote;

namespace HotKeys
{
   public  class ControlClick
    {
      
        public CommonWebBrowser commonWebBrowser_CurrentM(string AppId)
        {
            CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
            return commonWebBrowser_Current;
        }


        private bool EmulatedClickedControl(RemoteWebElement evtObj)
        {
            bool iDNameClassName = false;
            SetText spp = new SetText();
           

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
                        spp.text = evtObj.GetAttribute("text");
                        iDNameClassName = true;
                    }
                    catch (Exception ex) { }
                   

                    try
                    {
                        if ((evtObj.GetAttribute("class") != null) && (evtObj.GetAttribute("class") != string.Empty) && (evtObj.GetAttribute("class").Trim().Length > 0))
                            spp.ElementClass = evtObj.GetAttribute("class");
                        iDNameClassName = true;
                    }
                    catch (Exception ex) { }


                    try
                    {
                        string innerhtml = evtObj.GetAttribute("innerHTML");
                    }
                    catch (Exception ex) { }


                    spp.DisplayName = evtObj.Text;
                    try
                    {

                        string strJavaScriptXpath = "return function getPathTo(){var element = document.activeElement; if (element.id !== '') { return \"~~*[@id='\" + element.id + \"']\"; }if (element ==document.body) { return element.tagName.toLowerCase(); }var ix = 0; var siblings = element.parentNode.childNodes;for (var i = 0; i < siblings.length; i++){var sibling = siblings[i];if (sibling ==element) {return getPathTo(element.parentNode) + '/' + element.tagName.toLowerCase() + '[' + (ix + 1) + ']';if (sibling.nodeType ==1 && sibling.tagName ==element.tagName) { ix++; }}}}";
                        //string strXpath = (string)((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript(strJavaScriptXpath);
                        //spp.Xpath = strXpath;



                        IWebElement parent = evtObj.FindElementByXPath("..");
                       
                    }
                    catch (Exception ex) { }
                    try
                    {
                        IWebElement followingSibling = evtObj.FindElementByXPath("following-sibling::*[1]");
                        if (followingSibling != null)
                        {
                            try
                            {
                                spp.FollowingSibling = followingSibling.GetAttribute("id");
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
                                spp.precedingSibling = prevSibling.GetAttribute("id");
                            }
                            catch (Exception ex) { }
                           
                            if (evtObj.GetAttribute("xpath") != null)
                            {
                                spp.Xpath = evtObj.GetAttribute("xpath");
                            }
                           
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
                Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
            }
            return false;
        }
    }
}
