using CommonLibrary;
using Logger;
using mshtml;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using WatiN.Core;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/IE_ControlProperties.png")]
    [Designer(typeof(IE_ControlProperties_ActivityDesigner))]
    public class IE_ControlProperties : ActivityExtended
    {

        WatiN.Core.IE IEWATIN = null;
        System.Timers.Timer whiletimer = null;
        bool running = true;
        bool UseNextPriority = true;
        public IE_ControlProperties() : base()
        {
            whiletimer= new System.Timers.Timer();
            whiletimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                      
            //SetControlValue = new Object();
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

        [DisplayName("Control Type [To Identify]")]
       // //[Description("Tag Name of control")]
        public InArgument<string> TagName { get; set; }

        [DisplayName("Custom Attribute Name")]
       // //[Description("Parent Sibling Id")]
        [CategoryAttribute("Custom Attribute [Priority:4]")]
        public InArgument<string> CustomAttributes { get; set; }

        //[DisplayName("Parent Id")]
        ////[Description("Parent Sibling Class Name")]
        //[CategoryAttribute("Parent [Priority:5]")]
        //public InArgument<string> ParentId { get; set; }

        //[DisplayName("Parent Name")]
        ////[Description("Parent Sibling Class Name")]
        //[CategoryAttribute("Parent [Priority:5]")]
        //public InArgument<string> ParentName { get; set; }


        //[DisplayName("Parent Class Name")]
        ////[Description("Parent Sibling Class Name")]
        //[CategoryAttribute("Parent [Priority:5]")]
        //public InArgument<string> ParentClassName { get; set; }

        //[DisplayName("Child Count")]
        ////[Description("Parent Sibling Class Name")]
        //[CategoryAttribute("Parent [Priority:5]")]
        //public InArgument<int> ChildCount { get; set; }

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
                    //no error added in log
                }
                int waitBefore = WaitBefore.Get(context);
                if (waitBefore > 0)
                {
                    Thread.Sleep(waitBefore);
                }
                int waitMaxTimeWhileLoop = 0;
                //int waitMaxTimeWhileLoop = WaitMaxTimeWhileLoop.Get(context);
                //if(waitMaxTimeWhileLoop > 0)
                //{                   
                //    bool running = true;
                //    running = true;
                //    whiletimer.Interval = waitMaxTimeWhileLoop;
                //    whiletimer.Enabled = true;
                //    while ((running)&&(UseNextPriority ==true))
                //    {
                        PerformActionOncontrol(context, ref returnvalue, ref value, strWaitUntilContainText, waitMaxTimeWhileLoop);
                //    }
                //    whiletimer.Enabled = false;

                //}
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
                            int poNo = Convert.ToInt32(IEWATIN.DomContainer.Eval(strJavascript));
                            //IEWATIN.Eval(strJavascript);
                        });
                    }
                }
              
                if((!string.IsNullOrEmpty(strWaitUntilContainText)) && (strWaitUntilContainText != ""))
                {
                    IEWATIN.WaitUntilContainsText(strWaitUntilContainText, 5000);
                }
                else
                {
                    IEWATIN.WaitForComplete(5000);
                }
                              
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
        
        private void PerformActionOncontrol(NativeActivityContext context, ref string returnvalue, ref string value,string strWaitUntilContainText, int waitMaxTimeWhileLoop)
        {
            try
            {
                
                string sControlId = ControlId.Get(context);
                string sControlName = ControlName.Get(context);
                string sClassName = ClassName.Get(context);
                string sTagName = TagName.Get(context);
                //  bool bIsEventField = IsEventField.Get(context);
                string sCustomAttributeName = CustomAttributes.Get(context);
               // string sCustomAttributeValue = CustomAttributeValue.Get(context);
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
                if ((sCustomAttributeName != string.Empty) && (sCustomAttributeName != null) && (sCustomAttributeName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Custom Attribute: " + sCustomAttributeName);
                    }
                    // Element element = IEWATIN.Element(Find.ById(ParentSiblingId));
                    //if ((sCustomAttributeValue != string.Empty) && (sCustomAttributeValue != null) && (sCustomAttributeValue.Trim().Length > 0))
                    //{
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowserByCustomAttribute(IEWATIN, sCustomAttributeName, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
                        if (!UseNextPriority)
                            return;
                   // }
                }
                if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "ID: " + sControlId);
                    }
                    returnvalue = IExplorerHelper.TakeActionOnControlInBrowserByID(IEWATIN, sControlId, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
                    if (!UseNextPriority)
                        return;
                }

                if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Name: " + sControlName);
                    }
                    returnvalue = IExplorerHelper.TakeActionOnControlInBrowserByName(IEWATIN, sControlName, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);

                    if (!UseNextPriority)
                        return;
                }

                if ((sClassName != string.Empty) && (sClassName != null) && (sClassName.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Class Name: " + sClassName);
                    }
                    returnvalue = IExplorerHelper.TakeActionOnControlInBrowserByClassName(IEWATIN, sClassName, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);

                }
                if (!UseNextPriority)
                    return;
             
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
                    Element previoussiblingelement = IEWATIN.Element(Find.ById(sPreviousSiblingId));

                    Element currentelement = previoussiblingelement.NextSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
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
                    Element previoussiblingelement = IEWATIN.Element(Find.ByName(sPreviousSiblingName));
                    Element currentelement = previoussiblingelement.NextSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
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
                    Element previoussiblingelement = IEWATIN.Element(Find.ByClass(sPreviousSiblingClassName));
                    Element currentelement = previoussiblingelement.NextSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
                    }
                }
                if ((sNextSiblingId != string.Empty) && (sNextSiblingId != null) && (sNextSiblingId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Next Sibling: " + sNextSiblingId);
                    }
                    Element nextsiblingelement = IEWATIN.Element(Find.ById(sControlName));
                    Element currentelement = nextsiblingelement.PreviousSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
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
                    Element nextsiblingelement = IEWATIN.Element(Find.ByName(sNextSiblingName));
                    Element currentelement = nextsiblingelement.PreviousSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
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
                    Element nextsiblingelement = IEWATIN.Element(Find.ByClass(sNextSiblingClassName));
                    Element currentelement = nextsiblingelement.PreviousSibling;
                    if (currentelement != null)
                    {
                        returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(currentelement, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);
                    }
                    if (!UseNextPriority)
                        return;
                }
                if ((sXPath != string.Empty) && (sXPath != null) && (sXPath.Trim().Length > 0))
                {
                    try
                    {
                        UseNextPriority = false;
                      //  IExplorerHelper explorerHelper = new IExplorerHelper();
                        IHTMLDocument2 document2 =(IHTMLDocument2) IEWATIN;
                        // IHTMLElement document2 = (IHTMLElement)IEWATIN.Body;
                        mshtml.IHTMLElement elemnt = IExplorerHelper.SelectHtmlElementByXPath(sXPath, document2.body);
                        if (IsEventField)
                        {
                            elemnt.click();// setAttribute("value", value);
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else if ((value != null) && (value != string.Empty))
                        {
                            elemnt.setAttribute("value", value);
                            Log.Logger.LogData("Set data for control : " + sXPath, LogLevel.Error);

                        }
                        else
                        {
                            GetControlValue.Set(context, elemnt.getAttribute("value").ToString());
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
                if ((iSourceIndex > 0) && (UseNextPriority))
                {
                    try
                    {
                        UseNextPriority = false;
                        IHTMLDocument2 HtmlDoc =(IHTMLDocument2) IEWATIN;
                        if (HtmlDoc != null)
                        {
                            //IHTMLElement targetElement = null;
                            Element ele = IEWATIN.Elements.First(Find.ByIndex(iSourceIndex));
                            if (ele == null)
                            {
                                Log.Logger.LogData("Source Index element not found", LogLevel.Error);
                            }
                            else
                            {

                                returnvalue = IExplorerHelper.TakeActionOnControlInBrowser(ele, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority, strWaitUntilContainText, waitMaxTimeWhileLoop);

                            }

                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Error);
                        UseNextPriority = true;
                    }
                }
                if ((sImagePath != null) && (sImagePath != string.Empty) && (sImagePath.Length > 0) && (UseNextPriority))
                {
                    try
                    {
                        if(!string.IsNullOrEmpty(CommonLibrary.SelectHelper.ProjectLocation))
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
                            UseNextPriority = imgRecognition.GetSetClickImage(sImagePath, getSetClick, value,0);
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
                        IHTMLDocument2 HtmlDoc =(IHTMLDocument2) IEWATIN;
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
                    IEWATIN = IE.AttachTo<IE>(Find.ByUrl(strTitleOrUrlToAttach));
                    if (IEWATIN == null)
                    {
                        IEWATIN = IE.AttachTo<IE>(Find.ByTitle(strTitleOrUrlToAttach));
                        if (IEWATIN == null)
                        {
                            Log.Logger.LogData("Not able to attach by titile or url, proceeding with creating new instance:" + strTitleOrUrlToAttach, LogLevel.Error);
                        }
                    }
                   if (IEWATIN != null)
                    {
                        IEWATIN.ApplicationId = ApplicationIDToAttach;
                        isIEAvailable = true;
                    }
                }

                if (isIEAvailable == false)
                {
                    // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
                    {
                        IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationIDToAttach];
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
