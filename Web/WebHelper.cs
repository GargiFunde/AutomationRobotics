// <copyright file=IExplorerHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:57</date>
// <summary></summary>

using System;
using mshtml;
using System.Text;
using Logger;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using LogLevel = Logger.LogLevel;

namespace Bot.Activity.Web
{
    public class WebHelper
    {
   
        #region "watin"

        public static string TakeActionOnControlInBrowserByID(CommonWebBrowser ie, string strID,bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, bool scroll, ref bool UseNextPriority)
        {
           
            //IWebElement element = ie.driver.FindElementById(strID);
            IWebElement element = null;
            if (strID.Contains(" "))
            {
                strID = "." + strID.Replace(" ", ".");
                // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                element = ie.driver.FindElement(By.CssSelector(strID));
            }
            else
            {
                element = ie.driver.FindElementById(strID);
            }
            return TakeAction(element, ie, controlType.id, strID, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue,scroll, ref UseNextPriority);
        }

        public static string TakeActionOnControlInBrowserByName(CommonWebBrowser ie, string strName, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, bool scroll,ref bool UseNextPriority)
        {
           // IWebElement element = ie.driver.FindElementByName(strName);
            IWebElement element = null;
            if (strName.Contains(" "))
            {
                strName = "." + strName.Replace(" ", ".");
                // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                element = ie.driver.FindElement(By.CssSelector(strName));
            }
            else
            {
                element = ie.driver.FindElementByName(strName);
            }
            return TakeAction(element,ie, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue,scroll, ref UseNextPriority);
        }

        public static string TakeActionOnControlInBrowserByClassName(CommonWebBrowser ie, string strClassName, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue,bool scroll, ref bool UseNextPriority)
        {
            IWebElement element = null;
            if (strClassName.Contains(" "))
            {
                strClassName = "." + strClassName.Replace(" ", ".");
                // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                element = ie.driver.FindElement(By.CssSelector(strClassName));
               
            }
            else
            {
                element = ie.driver.FindElementByClassName(strClassName);
            }
            return TakeAction(element, ie, controlType.className, strClassName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, scroll, ref UseNextPriority);
        }

        public static string TakeActionOnControlInBrowserByCSSSelector(CommonWebBrowser ie, string strCSSSelector, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue,bool scroll, ref bool UseNextPriority)
        {
            IWebElement element = null;
            if (strCSSSelector.Contains(" "))
            {
                strCSSSelector = "." + strCSSSelector.Replace(" ", ".");
                element = ie.driver.FindElement(By.CssSelector(strCSSSelector));
            }
            return TakeAction(element, ie, controlType.className, strCSSSelector, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, scroll, ref UseNextPriority);
            
        }

        //public static string TakeActionOnControlInBrowserByCustomAttribute(CommonWebBrowser ie, string strCustomAttribute,string strToFind, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority) 
        //{

        //    return string.Empty;
        //}

        public static string TakeAction(IWebElement element,CommonWebBrowser ie, controlType ct, string strName, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue,bool scroll, ref bool UseNextPriority)
        {
            try
            {  
               
                UseNextPriority = false;

                if(scroll)
                {
                    try
                    {
                        ie.driver.ExecuteScript("arguments[0].scrollIntoView();", element);
                    }
                   catch(Exception ex)
                    {
                        Logger.Log.Logger.LogData("Error in scroll:" + ex.Message, Logger.LogLevel.Error);
                    }
                }

                //Assert.IsTrue(sp.Exists, "Could not Find: " + strID);
                if (Activate)
                {
                    try
                    {
                        if ("input".Equals(element.TagName))
                        {
                            element.SendKeys("");
                        }
                        else
                        {
                            new Actions(ie.driver).MoveToElement(element).Perform();
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Logger.Log.Logger.LogData("Error while setting focus on " + strName, LogLevel.Warning);
                    }
                    //Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);
                    //   IEWATIN.WaitForComplete(5000);
                }
                if (ClickBeforeValueSet)
                {
                    try
                    {
                        element.Click();
                        //Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Logger.LogData("Error while before click " + strName + ex.Message, LogLevel.Warning);
                    }
                }
                if (IsEventField)
                {
                    element.Click();
                    Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                    //   IEWATIN.WaitForComplete(5000);
                }
               
                    if ((setValue != null) && (setValue != string.Empty))
                    {
                        try
                        {

                            if (ct == controlType.id)
                            {
                                ie.driver.ExecuteScript("document.getElementById('" + strName + "').value='" + setValue + "'");
                            }
                            else if (ct == controlType.name)
                            {
                                ie.driver.ExecuteScript("document.getElementByName('" + strName + "').value='" + setValue + "'");
                            }
                            else if (ct == controlType.className)
                            {
                                ie.driver.ExecuteScript("document.getElementsByClassName('" + strName + "').value='" + setValue + "'");   //Piyush: Added s in element
                            }
                        
                        }
                        catch (Exception ex)
                        {
                        Console.WriteLine(ex.Message);
                        UseNextPriority = true;
                            Log.Logger.LogData("Not able to set data for " + strName, LogLevel.Warning);
                        }
                        //element.Text = setValue;
                    }
                    else
                    {
                        return element.Text;
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                UseNextPriority = true;
            }
            return null;
        }
              
        # endregion
    }

   public enum controlType
    {
        id = 1,
        name = 2,
        className = 3
    }
}
