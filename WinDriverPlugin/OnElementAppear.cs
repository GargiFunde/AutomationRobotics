///<summary>
///<para>GetTable Testing Note:: </para>
///<para> If want to extract single row data from table then sometimes both Table and Row Xpath are require.</para>
///<para>Table CSS Selector property: sometimes  illegal selector error occurs then that time manually create selctor.</para>
///</summary>

using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using OpenQA.Selenium.Appium.Windows;
using CommonLibrary;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Bot.Activity.WinDriverPlugin
{
    [ToolboxBitmap("Resources/OnElementAppear.png")]
    [Designer(typeof(OnElementAppear_ActivityDesigner))]
    public class OnElementAppear : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Application ID To Attach")]
        [Description("Set Application ID")]
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("XPath")]
        [Description("Set XPath")]
        public InArgument<string> XPath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Text To Search")]
        [Description("Set File Path")]
        public InArgument<string> TextToSearch { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Time Out In Second")]
        [Description("eg.10")]
        [RequiredArgument]
        public InArgument<int> TimeOutInSecond { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Element Exists")]
        [Description("Get Element Exists")]
        public OutArgument<bool> ElementExists { get; set; }
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                string elementXPath = XPath.Get(context);
                int timeOutInSecond = TimeOutInSecond.Get(context);
                string textToSearch = TextToSearch.Get(context);
                string AppId = ApplicationIDToAttach.Get(context);
                WindowsInstance windowObj = null;
                bool exists = false;


                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                {
                    //To create window object of application using AppId
                    windowObj = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                    DateTime timeout = DateTime.Now.AddSeconds(timeOutInSecond);
                    bool TextToSearch_PresentIn_ElementXPath = false;

                    //If Both are given then Check textToSearch present in XPath 

                    if ((elementXPath != null) && (textToSearch != null))
                    {
                        TextToSearch_PresentIn_ElementXPath = elementXPath.Contains(textToSearch);
                        if (true == TextToSearch_PresentIn_ElementXPath)
                        {
                            exists = XPathExists(windowObj, elementXPath);
                            while (!exists && DateTime.Now < timeout)
                            {
                                exists = XPathExists(windowObj, elementXPath);
                            }

                            if (false == exists)
                            {
                                Log.Logger.LogData("The \"" + textToSearch + "\" element does not exist in activity OnElementAppear", Logger.LogLevel.Info);
                                if (context != null)
                                {
                                    context.Abort();
                                }
                            }
                        }

                        if (false == TextToSearch_PresentIn_ElementXPath)
                        {
                            Log.Logger.LogData("The \"" + textToSearch + "\" element does not exist in XPath in activity OnElementAppear", Logger.LogLevel.Info);
                            if (context != null)
                            {
                                context.Abort();
                            }
                        }
                    }
                    else if (elementXPath != null)                  //If XPath is given
                    {
                        exists = XPathExists(windowObj, elementXPath);
                        while (!exists && DateTime.Now < timeout)
                        {
                            exists = XPathExists(windowObj, elementXPath);
                        }

                    }
                    else if (textToSearch != null)                  //If textToSearch is given
                    {
                        exists = TextToSearchExists(windowObj, textToSearch, timeout);
                        if (false == exists)
                        {
                            Log.Logger.LogData("The \"" + textToSearch + "\" element does not exist in activity OnElementAppear", Logger.LogLevel.Info);
                            if (context != null)
                            {
                                context.Abort();
                            }
                        }
                    }
                    else
                    {
                        Log.Logger.LogData("At least give one parameter(\"Text to search\" or \"Xpath\") in activity OnElementAppear", Logger.LogLevel.Info);

                        exists = false;
                    }

                }

                ElementExists.Set(context, exists);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity OnElementAppear", Logger.LogLevel.Error);
                if (context != null)
                {
                    context.Abort();
                }
            }
        }

        // To check TextToSearch (text) exists in given timeout 
        public static bool TextToSearchExists(WindowsInstance windowObj, string textToSearch, DateTime timeout)
        {
            try
            {

                while (DateTime.Now < timeout)
                {
                    WindowsElement we = windowObj._driver.FindElementByName(textToSearch);
                    return true;
                }

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    TextToSearchExists(windowObj, textToSearch, timeout);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity OnElementAppear", Logger.LogLevel.Error);
                }

            }
            return false;

        }


        // To check XPath exists in given timeout 
        public static bool XPathExists(WindowsInstance windowObj, string elementXPath)
        {
            bool exists = false;
            ReadOnlyCollection<WindowsElement> elementExists = windowObj._driver.FindElementsByXPath(elementXPath);

            if (elementExists.Count != 0)
            {
                exists = true;
            }
            return exists;
        }

    }
}
