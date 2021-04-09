using Logger;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace Bot.Activity.WinDriverPlugin
{
    public class WindowsHelper
    {
        public static object TakeActionOnControlByID(WindowsInstance window, string strID, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            WindowsElement element = window._driver.FindElementById(strID);
            return TakeActionOnControl(window, element, strID, ctrl, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, ref UseNextPriority);
        }
        public static object TakeActionOnControlByName(WindowsInstance window, string strName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            WindowsElement element = window._driver.FindElementByName(strName);
            return TakeActionOnControl(window, element, strName, ctrl, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, ref UseNextPriority);
            
        }
        public static object TakeActionOnControlByClassName(WindowsInstance window, string strClassName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            WindowsElement element = window._driver.FindElementByClassName(strClassName);
            return TakeActionOnControl(window, element, strClassName, ctrl, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, ref UseNextPriority);
        }
        public static object TakeActionOnControlByXPath(WindowsInstance window,string strXPath, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {

           
            WindowsElement element = window._driver.FindElementByXPath(strXPath);
            return TakeActionOnControl(window, element, strXPath, ctrl, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, setValue, ref UseNextPriority);
        
        }



        private static string TakeActionOnControl(WindowsInstance window, WindowsElement element, string strName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            try
            {
                // WindowsElement element = window.Get<White.Core.UIItems.RadioButton>(SearchCriteria.ByText(strName));
                UseNextPriority = false;
                if (Activate)
                {
                    try
                    {
                        Logger.Log.Logger.LogData("RadioButton control activated:" + strName, LogLevel.Info);
                    }
                    catch (Exception)
                    {
                        Logger.Log.Logger.LogData("Error while setting focus on RadioButton by Name: " + strName, LogLevel.Warning);
                    }
                    Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                }
                if (ClickBeforeValueSet)
                {
                    try
                    {
                        element.Click();
                    }
                    catch (Exception )
                    {
                        Logger.Log.Logger.LogData("Error while click before value set on RadioButton by Name: " + strName, LogLevel.Warning);
                    }
                }
                if (!string.IsNullOrEmpty(setValue))
                {
                    
                     element.SendKeys(setValue);
                    
                }
                if (IsEventField)
                {
                    element.Click();
                    Logger.Log.Logger.LogData("RadioButton control clicked:" + strName, LogLevel.Info);
                }
               
                return element.Text;
            }
            catch (Exception )
            { UseNextPriority = true; }
            return string.Empty;

        }
    }
}
