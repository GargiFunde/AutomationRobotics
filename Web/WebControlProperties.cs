using CommonLibrary;
using Logger;
using mshtml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using LogLevel = Logger.LogLevel;

namespace Bot.Activity.Web
{
    [Designer(typeof(WebControlProperties1))]
    [ToolboxBitmap("Resources/WebControlProperties.png")]
    public class WebControlProperties : ActivityExtended
    {
        CommonWebBrowser IEWATIN = null;
        System.Timers.Timer whiletimer = null;
        bool running = true;
        bool UseNextPriority = true;
        public WebControlProperties() : base()
        {
            whiletimer = new System.Timers.Timer();
            whiletimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            running = false;
        }
        [DisplayName("Wait Until Contain Text")]
        public InArgument<string> WaitUntilContainText { get; set; }

        [DisplayName("Set focus")]
        //  //[Description("Set focus")]
        public bool Activate { get; set; }

        [DisplayName("Class Name [Priority:3]")]
        // //[Description("Classs to find control")]
        public InArgument<string> ClassName { get; set; }

        [DisplayName("CSS Selector [Priority:4]")]
        // //[Description("Classs to find control")]
        public InArgument<string> CSSSelector { get; set; }

        [DisplayName("Control Type [To Identify]")]
        // //[Description("Tag Name of control")]
        public InArgument<string> TagName { get; set; }

        //[DisplayName("Custom Attribute Name")]
        //// //[Description("Parent Sibling Id")]
        //[CategoryAttribute("Custom Attribute [Priority:5]")]
        //public InArgument<string> CustomAttributeName { get; set; }


        //[DisplayName("Custom  Attribute Value")]
        ////  //[Description("Parent Sibling Name")]
        //[CategoryAttribute("Custom Attribute [Priority:5]")]
        //public InArgument<string> CustomAttributeValue { get; set; }


        [DisplayName("Parent Id")]
        //[Description("Parent Sibling Class Name")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentId { get; set; }

        [DisplayName("Parent Name")]
        //[Description("Parent Sibling Class Name")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentName { get; set; }


        [DisplayName("Parent Class Name")]
        //[Description("Parent Sibling Class Name")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentClassName { get; set; }

        [DisplayName("Child Count")]
        //[Description("Parent Sibling Class Name")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<int> ChildCount { get; set; }

        //[DisplayName("Parent Sibling XPath")]
        // //[Description("Parent Sibling Class Name")]
        //[CategoryAttribute("Parent Sibling [Priority:5]")]
        //public string ParentSiblingXPath { get; set; }

        [DisplayName("Previous Sibling Id")]
        //[Description("Previous Sibling Id")]
        [CategoryAttribute("Previous Sibling [Priority:6]")]
        public InArgument<string> PreviousSiblingId { get; set; }


        [DisplayName("Previous Sibling Name")]
        //[Description("Previous Sibling Name")]
        [CategoryAttribute("Previous Sibling [Priority:6]")]
        public InArgument<string> PreviousSiblingName { get; set; }


        [DisplayName("Previous Sibling Class Name")]
        //[Description("Previous Sibling Class Name")]
        [CategoryAttribute("Previous Sibling [Priority:6]")]
        public InArgument<string> PreviousSiblingClassName { get; set; }

        [DisplayName("Next Sibling Id")]
        //[Description("Next Sibling Id")]
        [CategoryAttribute("Next Sibling [Priority:7]")]
        public InArgument<string> NextSiblingId { get; set; }

        [DisplayName("Next Sibling Name")]
        //[Description("Next Sibling Name")]
        [CategoryAttribute("Next Sibling [Priority:7]")]
        public InArgument<string> NextSiblingName { get; set; }

        [DisplayName("Next Sibling Class Name")]
        //[Description("Next Sibling Class Name")]
        [CategoryAttribute("Next Sibling [Priority:7]")]
        public InArgument<string> NextSiblingClassName { get; set; }

        [DisplayName("XPath [Priority:8]")]
        //[Description("XPath to find control")]

        public InArgument<string> XPath { get; set; }

        [DisplayName("SourceIndex [Priority:9]")]
        //[Description("XPath to find control")]

        public InArgument<int> SourceIndex { get; set; }

        [DisplayName("Title [Priority:10]")]
        //[Description("XPath to find control")]
        public InArgument<string> Title { get; set; }



        [DisplayName("JavaScript [Always]")]
        //[Description("JavaScript to find control")]
        public InArgument<string> JavaScript { get; set; }

        [DisplayName("Scroll To View")]
        //[Description("XPath to find control")]
        public bool scroll { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            string returnvalue = string.Empty;
            string value = string.Empty;
            string strWaitUntilContainText = string.Empty;
            UseNextPriority = true;
            try
            {
                try
                {
                    if (SetControlValue.Get(context) != null)
                    {
                        value = SetControlValue.Get(context).ToString();
                    }
                    if (WaitUntilContainText.Get(context) != null)
                    {
                        strWaitUntilContainText = WaitUntilContainText.Get(context).ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //no error added in log
                }
                int waitBefore = WaitBefore.Get(context);
                if (waitBefore > 0)
                {
                    Thread.Sleep(waitBefore);
                }
                int waitMaxTimeWhileLoop = WaitMaxTimeWhileLoop.Get(context);
                if (waitMaxTimeWhileLoop > 0)
                {
                    bool running = true;
                    running = true;
                    whiletimer.Interval = waitMaxTimeWhileLoop;
                   // whiletimer.Enabled = true;
                    //while ((running) && (UseNextPriority == true))
                    //{
                        PerformActionOncontrol(context, ref returnvalue, ref value, strWaitUntilContainText, waitMaxTimeWhileLoop);


                }
                if ((value == null) || (value == string.Empty) || (value.Trim().Length == 0))
                {
                    if ((returnvalue != string.Empty) && (returnvalue != null) && (returnvalue.Trim().Length > 0) && (IsEventField == false))
                    {
                        GetControlValue.Set(context, returnvalue);
                        Log.Logger.LogData("'Get Value' is set", LogLevel.Info);
                    }
                }
                if (JavaScript.Get(context) != null)
                {
                    string strJavascript = JavaScript.Get(context).ToString();
                    if ((strJavascript != string.Empty) && (strJavascript != null) && (strJavascript.Trim().Length > 0))
                    {
                        ThreadInvoker.Instance.RunByUiThread(() =>
                        {
                           // int poNo = Convert.ToInt32(IEWATIN.DomContainer.Eval(strJavascript));
                            
                        });
                    }
                }

                //if ((!string.IsNullOrEmpty(strWaitUntilContainText)) && (strWaitUntilContainText != ""))
                //{
                //    IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                //}
                //else
                //{
                //    IEWATIN.WaitForComplete(5000);
                //}

                int waitAfter = WaitAfter.Get(context);
                if (waitAfter > 0)
                {
                    Thread.Sleep(waitAfter);
                }
                if (UseNextPriority)
                {
                    if ((ErrorMessage != null) && (ErrorMessage.Expression != null))
                    {
                        Log.Logger.LogData(ErrorMessage.Expression.ToString(), LogLevel.Error);
                    }
                    if (!ContinueOnError)
                    {
                        context.Abort();
                        if (SelectHelper._timerExecution != null)
                        {
                            SelectHelper._timerExecution.Stop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                    if (SelectHelper._timerExecution != null)
                    {
                        SelectHelper._timerExecution.Stop();
                    }
                }
            }
        }

        private void PerformActionOncontrol(NativeActivityContext context, ref string returnvalue, ref string value, string strWaitUntilContainText, int waitMaxTimeWhileLoop)
        {
            try
            {

                string sControlId = ControlId.Get(context);
                string sControlName = ControlName.Get(context);
                string sClassName = ClassName.Get(context);
                string sCSSSelector = CSSSelector.Get(context);
                string sTagName = TagName.Get(context);
                //  bool bIsEventField = IsEventField.Get(context);
                //string sCustomAttributeName = CustomAttributeName.Get(context);
                //string sCustomAttributeValue = CustomAttributeValue.Get(context);
                //string sParentId = ParentId.Get(context);
                //string sParentName = ParentName.Get(context);
                //string sParentClassName = ParentClassName.Get(context);
                string sPreviousSiblingId = PreviousSiblingId.Get(context);
                string sPreviousSiblingName = PreviousSiblingName.Get(context);
                string sPreviousSiblingClassName = PreviousSiblingClassName.Get(context);
                string sNextSiblingId = NextSiblingId.Get(context);
                string sNextSiblingName = NextSiblingName.Get(context);
                string sNextSiblingClassName = NextSiblingClassName.Get(context);
                string sXPath = XPath.Get(context);
                int iSourceIndex = SourceIndex.Get(context);
                string sImagePath = ImagePath.Get(context);
                int iX = X.Get(context);
                int iY = Y.Get(context);
                int iTimeOutInSeconds = WaitMaxTimeWhileLoop.Get(context);
                string sTitle = Title.Get(context);
                bool bRaiseErrorIfIDNameClassSibling = false;

                if ((sTagName == null) || (sTagName == string.Empty) || (sTagName.Trim().Length == 0))
                {
                    bRaiseErrorIfIDNameClassSibling = true;
                }
                //if ((sCustomAttributeName != string.Empty) && (sCustomAttributeName != null) && (sCustomAttributeName.Trim().Length > 0))
                //{
                //    if (bRaiseErrorIfIDNameClassSibling == true)
                //    {
                //        RaiseErrorIfControlTypeMissing(context, "Custom Attribute: " + sCustomAttributeName);
                //    }
                //    // Element element = IEWATIN.Element(Find.ById(ParentSiblingId));
                //    //if ((sCustomAttributeValue != string.Empty) && (sCustomAttributeValue != null) && (sCustomAttributeValue.Trim().Length > 0))
                //    //{
                //    returnvalue = WebHelper.TakeActionOnControlInBrowserByCustomAttribute(IEWATIN, sCustomAttributeName, sCustomAttributeValue, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                //    if (!UseNextPriority)
                //        return;
                //    // }
                //}
                if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "ID: " + sControlId);
                    }
                    returnvalue = WebHelper.TakeActionOnControlInBrowserByID(IEWATIN, sControlId, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    if (!UseNextPriority)
                        return;
                }

                if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Name: " + sControlName);
                    }
                    returnvalue = WebHelper.TakeActionOnControlInBrowserByName(IEWATIN, sControlName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);

                    if (!UseNextPriority)
                        return;
                }

                if ((sClassName != string.Empty) && (sClassName != null) && (sClassName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Class Name: " + sClassName);
                    }
                    returnvalue = WebHelper.TakeActionOnControlInBrowserByClassName(IEWATIN, sClassName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    if (!UseNextPriority)
                        return;
                }
                if ((sCSSSelector != string.Empty) && (sCSSSelector != null) && (sCSSSelector.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "CSS Selector: " + sCSSSelector);
                    }
                    //returnvalue = WebHelper.TakeActionOnControlInBrowserByClassName(IEWATIN, sCSSSelector, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    returnvalue = WebHelper.TakeActionOnControlInBrowserByCSSSelector(IEWATIN, sCSSSelector, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    if (!UseNextPriority)
                        return;
                }
                
                //if (!UseNextPriority)
                //    return;

                //if ((sParentId != string.Empty) && (sParentId != null) && (sParentId.Trim().Length > 0))
                //{
                //    if (bRaiseErrorIfIDNameClassSibling == true)
                //    {
                //        RaiseErrorIfControlTypeMissing(context, "Parent Id: " + sParentId);
                //    }
                //    Element parentsiblingelement = IEWATIN.Element(Find.ById(sParentId));
                //    returnvalue = ActionOnParentElment(context, parentsiblingelement, value, ref UseNextPriority);
                //    if (!UseNextPriority)
                //        return;
                //}
                //if ((sParentName != string.Empty) && (sParentName != null) && (sParentName.Trim().Length > 0))
                //{
                //    if (bRaiseErrorIfIDNameClassSibling == true)
                //    {
                //        RaiseErrorIfControlTypeMissing(context, "Parent Name: " + sParentName);
                //    }
                //    Element parentsiblingelement = IEWATIN.Element(Find.ByName(sParentName));
                //    returnvalue = ActionOnParentElment(context, parentsiblingelement, value, ref UseNextPriority);
                //    if (!UseNextPriority)
                //        return;

                //}
                //if ((sParentClassName != string.Empty) && (sParentClassName != null) && (sParentClassName.Trim().Length > 0))
                //{
                //    if (bRaiseErrorIfIDNameClassSibling == true)
                //    {
                //        RaiseErrorIfControlTypeMissing(context, "Parent Class Name: " + sParentClassName);
                //    }
                //    Element parentsiblingelement = IEWATIN.Element(Find.ByClass(sParentClassName));
                //    returnvalue =  ActionOnParentElment(context, parentsiblingelement ,value, ref UseNextPriority);
                //    if (!UseNextPriority)
                //        return;
                //}

                if ((sPreviousSiblingId != string.Empty) && (sPreviousSiblingId != null) && (sPreviousSiblingId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Previous Sibling: " + sPreviousSiblingId);
                    }
                    RemoteWebElement previoussiblingelement =(RemoteWebElement) IEWATIN.driver.FindElementById(sPreviousSiblingId);

                    IWebElement currentelement = previoussiblingelement.FindElementByXPath("following-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }

                    if (!UseNextPriority)
                        return;
                }

                if ((sPreviousSiblingName != string.Empty) && (sPreviousSiblingName != null) && (sPreviousSiblingName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Previous Sibling: " + sPreviousSiblingName);
                    }
                    RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementByName(sPreviousSiblingName);

                    IWebElement currentelement = previoussiblingelement.FindElementByXPath("following-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                                      
                    if (!UseNextPriority)
                        return;
                }

                if ((sPreviousSiblingClassName != string.Empty) && (sPreviousSiblingClassName != null) && (sPreviousSiblingClassName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Previous Sibling: " + sPreviousSiblingClassName);
                    }
                    RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementByClassName(sPreviousSiblingClassName);

                    IWebElement currentelement = previoussiblingelement.FindElementByXPath("following-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    if (!UseNextPriority)
                        return;
                }
                if ((sNextSiblingId != string.Empty) && (sNextSiblingId != null) && (sNextSiblingId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Next Sibling: " + sNextSiblingId);
                    }
                    RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementById(sPreviousSiblingClassName);

                    IWebElement currentelement = previoussiblingelement.FindElementByXPath("preceding-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    if (!UseNextPriority)
                        return;
                    
                }

                if ((sNextSiblingName != string.Empty) && (sNextSiblingName != null) && (sNextSiblingName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Next Sibling: " + sNextSiblingName);
                    }
                    RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementByName(sPreviousSiblingClassName);

                    IWebElement currentelement = previoussiblingelement.FindElementByXPath("preceding-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    if (!UseNextPriority)
                        return;
                }

                if ((sNextSiblingClassName != string.Empty) && (sNextSiblingClassName != null) && (sNextSiblingClassName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Next Sibling: " + sNextSiblingClassName);
                    }
                    RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementById(sPreviousSiblingClassName);

                    IWebElement currentelement = previoussiblingelement.FindElementByClassName("preceding-sibling::*[1]"); ;
                    string strName = string.Empty;

                    if (currentelement.GetAttribute("id") != null)
                    {
                        strName = currentelement.GetAttribute("id");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.id, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("name") != null)
                    {
                        strName = currentelement.GetAttribute("name");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.name, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll, ref UseNextPriority);
                    }
                    else if (currentelement.GetAttribute("class") != null)
                    {
                        strName = currentelement.GetAttribute("class");
                        returnvalue = WebHelper.TakeAction(currentelement, IEWATIN, controlType.className, strName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, scroll,ref UseNextPriority);
                    }
                    if (!UseNextPriority)
                        return;
                }
                if ((sXPath != string.Empty) && (sXPath != null) && (sXPath.Trim().Length > 0))
                {
                    try
                    {
                        UseNextPriority = false;

                        //// service now use case


                        //if (IEWATIN.driver.FindElements(By.XPath(sXPath)).Count == 0)
                        //{
                        //    int size = IEWATIN.driver.FindElements(By.TagName("iframe")).Count;
                        //    for (int iFrameCounter = 0; iFrameCounter < size; iFrameCounter++)
                        //    {
                        //        IEWATIN.driver.SwitchTo().Frame(iFrameCounter);
                        //        if (IEWATIN.driver.FindElements(By.XPath(sXPath)).Count > 0){
                        //           Console.WriteLine("***************found the element in iframe:" + iFrameCounter.ToString());
                        //            // perform the actions on element here
                        //            RemoteWebElement element1 = (RemoteWebElement)IEWATIN.driver.FindElementByXPath(sXPath);
                        //            element1.SendKeys(value);
                        //        }
                        //        IEWATIN.driver.SwitchTo().DefaultContent();
                        //    }
                        //}





                        ////service now use case  
                  
                      RemoteWebElement element = (RemoteWebElement)IEWATIN.driver.FindElementByXPath(sXPath);
                        if (IsEventField)
                        {
                            element.Click();// setAttribute("value", value);   
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else if ((value != null) && (value != string.Empty))
                        {
                            //ie.driver.ExecuteScript("document.getElementById('" + strName + "').value='" + setValue + "'");
                            element.SendKeys(value);
                            Log.Logger.LogData("Set data for control : " + sXPath, LogLevel.Error);

                        }
                        else
                        {
                            //Need for 2 cases - element.Text and element.GetAttribute("value")
                            if (element.Text == null || element.Text.Equals(""))
                            {
                                GetControlValue.Set(context, element.GetAttribute("value").ToString());
                            }
                            else
                            {
                                GetControlValue.Set(context, element.Text);
                            }
                           
                            Log.Logger.LogData("Get data from control : " + sXPath, LogLevel.Error);
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Error);
                        UseNextPriority = true;
                    }

                }
                //if ((iSourceIndex > 0) && (UseNextPriority))
                //{
                //    try
                //    {
                //        UseNextPriority = false;
                //        IHTMLDocument2 HtmlDoc = (IHTMLDocument2)IEWATIN;
                //        if (HtmlDoc != null)
                //        {
                //            //IHTMLElement targetElement = null;
                //            Element ele = IEWATIN.Elements.First(Find.ByIndex(iSourceIndex));
                //            if (ele == null)
                //            {
                //                Log.Logger.LogData("Source Index element not found", LogLevel.Error);
                //            }
                //            else
                //            {

                //                returnvalue = WebHelper.TakeAction(le, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);

                //            }  

                //        }
                //        return;
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Error);
                //        UseNextPriority = true;
                //    }
                //}
                if ((sImagePath != null) && (sImagePath != string.Empty) && (sImagePath.Length > 0) && (UseNextPriority))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CommonLibrary.SelectHelper.ProjectLocation))
                        {
                            string s = CommonLibrary.SelectHelper.ProjectLocation + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar;
                            sImagePath = s + sImagePath;
                        }

                        if (File.Exists(sImagePath))
                        {
                            value = string.Empty;
                            GetSetClick getSetClick = new GetSetClick();

                            ImageRecognition imgRecognition = new ImageRecognition();
                            if (IsEventField)
                            {
                                getSetClick = GetSetClick.Click;
                            }
                            else
                            {
                                value = SetControlValue.Get(context).ToString();
                                getSetClick = GetSetClick.GetSet;

                            }
                            UseNextPriority = imgRecognition.GetSetClickImage(sImagePath, getSetClick, value, 0);
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Error);
                        UseNextPriority = true;
                        //Automation Failed
                        Log.Logger.LogData("Automation failed", LogLevel.Error);
                    }
                }
                if ((iX > 0) && (iY > 0) && (UseNextPriority))
                {
                    try
                    {
                        IHTMLDocument2 HtmlDoc = (IHTMLDocument2)IEWATIN;
                        if (HtmlDoc != null)
                        {
                            IHTMLElement targetElement = HtmlDoc.elementFromPoint(iX, iY);
                            if (IsEventField)
                            {
                                targetElement.click();
                            }
                            else if ((value != null) && (value != string.Empty))
                            {

                                targetElement.setAttribute("value", value);
                                Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
                            }
                            else
                            {
                                GetControlValue.Set(context, targetElement.getAttribute("value").ToString());
                                Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
                            }
                            //  MessageBox.Show(targetElement.innerText); //range.parentElement().sourceIndex
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Log.Logger.LogData("Error in set data for control by using co - ordinates", LogLevel.Error);
                        UseNextPriority = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach, string strTitleOrUrlToAttach)
        {
            try
            {
                bool isIEAvailable = false;
                if ((strTitleOrUrlToAttach != "") && (!string.IsNullOrEmpty(strTitleOrUrlToAttach)))
                {
                    //IEWATIN = IE.AttachTo<IE>(Find.ByUrl(strTitleOrUrlToAttach));
                    //if (IEWATIN == null)
                    //{
                    //    IEWATIN = IE.AttachTo<IE>(Find.ByTitle(strTitleOrUrlToAttach));
                    //    if (IEWATIN == null)
                    //    {
                    //        Log.Logger.LogData("Not able to attach by titile or url, proceeding with creating new instance:" + strTitleOrUrlToAttach, LogLevel.Error);
                    //    }
                    //}
                    //if (IEWATIN != null)
                    //{
                    //    IEWATIN.ApplicationId = ApplicationIDToAttach;
                    //    isIEAvailable = true;
                    //}
                }

                if (isIEAvailable == false)
                {
                    // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
                    {
                        IEWATIN = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationIDToAttach];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void RaiseErrorIfControlTypeMissing(NativeActivityContext context, string control)
        {
            try
            {
                Log.Logger.LogData("Please mention Control Type for " + control, LogLevel.Error);
                context.Abort();
                if (SelectHelper._timerExecution != null)
                {
                    SelectHelper._timerExecution.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

        }

    }
}
