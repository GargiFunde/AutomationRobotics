#region Headers
using CommonLibrary;
using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Automation;
//using White.Core;
//using White.Core.UIItems;
//using White.Core.UIItems.Finders;
//using White.Core.UIItems.TableItems;
//using White.Core.UIItems.WPFUIItems;
using System.Linq;
using TestStack.White;
using TestStack.White.UIItems.Finders;
#endregion

namespace Bot.Activity.Windows
{
    [ToolboxBitmap("Resources/WindowsControlProperties.png")]
    [Designer(typeof(WindowsControlProperties1))]
    public class Windows_ControlProperties : ActivityExtended
    {
        System.Timers.Timer whiletimer = null;
#pragma warning disable CS0414 // The field 'Windows_ControlProperties.running' is assigned but its value is never used
        bool running = true;
#pragma warning restore CS0414 // The field 'Windows_ControlProperties.running' is assigned but its value is never used
        public Windows_ControlProperties() : base()
        {
            whiletimer = new System.Timers.Timer();
            whiletimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            // SetControlValue = new Object();
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            running = false;
        }


        #region Declaration
        [DisplayName("Set focus")]
        [Description("Set focus")]
        public bool Activate { get; set; }

        [DisplayName("Class Name")]
        // [Description("Classs to find control")]    
        public InArgument<string> ClassName { get; set; }

        [DisplayName("Control Type [To Identify]")]
        // [Description("Tag Name of control")]      
        public InArgument<string> ControlType { get; set; }

        [DisplayName("Window Title")]
        //  [Description("XPath to find control")]
        public InArgument<string> WindowTitle { get; set; }

        [DisplayName("Framework Id")]
        //   [Description("XPath to find control")]

        public InArgument<string> FrameworkId { get; set; }

        [DisplayName("Parent Id")]
        //[Description("Parent ")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentId { get; set; }

        [DisplayName("Parent Name")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentName { get; set; }


        [DisplayName("Parent Class Name")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ParentClassName { get; set; }

        [DisplayName("Child Count")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<int> ChildCount { get; set; }

        [DisplayName("Child Type")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ChildType { get; set; }

        [DisplayName("Child Id RegX")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ChildIdRegx { get; set; }
        
        [DisplayName("Child Name RegX")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ChildNameRegx { get; set; }

        [DisplayName("Child Class Name RegX")]
        //[Description("Parent")]
        [CategoryAttribute("Parent [Priority:5]")]
        public InArgument<string> ChildClassNameRegx { get; set; }

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

        #endregion
        protected override void Execute(NativeActivityContext context)
        {
            object returnvalue = string.Empty;
            string value = string.Empty;
            try
            {
                try
                {
                    if (SetControlValue.Get(context) != null)
                    {
                        value = SetControlValue.Get(context).ToString();
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //no error added in log
                }

                int waitBefore = WaitBefore.Get(context);
                if (waitBefore > 0)
                {
                    Thread.Sleep(waitBefore);
                }
                int waitMaxTimeWhileLoop = WaitMaxTimeWhileLoop.Get(context);
                //if (waitMaxTimeWhileLoop > 0)
                //{
                //    bool running = true;
                //    running = true;
                //    whiletimer.Interval = waitMaxTimeWhileLoop;
                //    whiletimer.Enabled = true;
                //    while ((running) && (UseNextPriority == true))
                //    {
                        PerformActionOncontrol(context, ref returnvalue, ref value);
                //    }
                //    whiletimer.Enabled = false;
                //}
                if ((value == null) || (value == string.Empty) || (value.Trim().Length == 0))
                {          
                    if ((returnvalue != null) && (IsEventField == false))
                    {
                        GetControlValue.Set(context, returnvalue);
                        Log.Logger.LogData("'Get Value' is set", LogLevel.Info);
                    }
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
                int waitAfter = WaitAfter.Get(context);
                if (waitAfter > 0)
                {
                    Thread.Sleep(waitAfter);
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
        bool UseNextPriority = true;
        private void PerformActionOncontrol(NativeActivityContext context, ref object returnvalue, ref string value)
        {
            UseNextPriority = true;
            string sWindowTitle = WindowTitle.Get(context);
            string sControlId = ControlId.Get(context);
            string sControlName = ControlName.Get(context);
            string sControlType = ControlType.Get(context);
            string sClassName = ClassName.Get(context);

            string sParentControlId = ParentId.Get(context);
            string sParentControlName = ParentName.Get(context);
            string sParentClassName = ParentClassName.Get(context);

            int childCount = ChildCount.Get(context);
            string sChildControlIdRegx = ChildIdRegx.Get(context);
            string sChildControlNameRegx = ChildNameRegx.Get(context);
            string sChildControlTypeRegx = ChildType.Get(context);
            string sChildClassNameRegx = ClassName.Get(context);


            string sImagePath = ImagePath.Get(context);
            int iX = X.Get(context);
            int iY = Y.Get(context);
          //  int iTimeOutInSeconds = TimeOutInSeconds.Get(context);
            bool bRaiseErrorIfIDNameClass = false;
            TestStack.White.UIItems.WindowItems.Window window = null;
                        
            window = WindowsHelper.GetApplicationObject(sWindowTitle,  context);     //First Priority: First get the Title

            //#region Piyush Testing
            ////Added By Piyush Testing
            //var application = Application.Attach("SQL Server 2014 Management Studio");
            //var windowScreen = application.GetWindow("File");
            //var datagrid = window.Get<White.Core.UIItems.TableItems.Table>("dataGridAutomationId").AutomationElement;

            //// Now it's using UI Automation
            //var headerLine = datagrid.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Header));
            //var cacheRequest = new CacheRequest { AutomationElementMode = AutomationElementMode.Full, TreeScope = TreeScope.Children };
            //cacheRequest.Add(AutomationElement.NameProperty);
            //cacheRequest.Add(ValuePattern.Pattern);
            //cacheRequest.Push();
            //var gridLines = datagrid.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
            //cacheRequest.Pop();

            //Console.WriteLine(headerLine.Count + " columns");
            //Console.WriteLine(gridLines.Count + " rows");

            //var gridData = new string[headerLine.Count, gridLines.Count];

            //var headerIndex = 0;
            //foreach (AutomationElement header in headerLine)
            //{
            //    gridData[headerIndex++, 0] = header.Current.Name;
            //}

            //var rowIndex = 1;
            //foreach (AutomationElement row in gridLines)
            //{
            //    foreach (AutomationElement col in row.CachedChildren)
            //    {
            //        // Marry up data with headers (for some reason the orders were different
            //        // when viewing in something like UISpy so this makes sure it's correct
            //        headerIndex = 0;
            //        for (headerIndex = 0; headerIndex < headerLine.Count; headerIndex++)
            //        {
            //            if (gridData[headerIndex, 0] == col.Cached.Name)
            //                break;
            //        }

            //        gridData[headerIndex, rowIndex] = (col.GetCachedPattern(ValuePattern.Pattern) as ValuePattern).Current.Value;
            //    }
            //    rowIndex++;
            //}
            //#endregion

            //TableRow tr = testTable.Row("Column Name", "Value in the cell");
            //tr.DrawHighlight();












            if (window == null)
            {
                Logger.Log.Logger.LogData("No desktop application window found with title :" + sWindowTitle, Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                    if (SelectHelper._timerExecution != null)
                    {
                        SelectHelper._timerExecution.Stop();
                    }
                }
            }

            if ((sControlType == null) || (sControlType == string.Empty) || (sControlType.Trim().Length == 0))
            {
                bRaiseErrorIfIDNameClass = true;
            }
            if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "ID: " + sControlId);
                }
                returnvalue = WindowsHelper.TakeActionOnControlByID(window, sControlId, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "Name: " + sControlName);
                }
                returnvalue = WindowsHelper.TakeActionOnControlByName(window, sControlName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if ((sClassName != string.Empty) && (sClassName != null) && (sClassName.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "Name: " + sClassName);
                }
                returnvalue = WindowsHelper.TakeActionOnControlByClassName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if ((sParentControlId != string.Empty) && (sParentControlId != null) && (sParentControlId.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "Name: " + sParentControlId);
                }
                TestStack.White.UIItems.UIItem  item =(TestStack.White.UIItems.UIItem) window.Get(SearchCriteria.ByAutomationId(sParentControlId));
                
                List< TestStack.White.UIItems.UIItem> items =    window.ItemsWithin(item);
                if (childCount > 0)
                {
                    TestStack.White.UIItems.UIItem itemAutomation = items[childCount - 1];
                    if (itemAutomation != null)
                    {
                        sControlType = itemAutomation.GetType().ToString();
                        returnvalue = WindowsHelper.TakeActionOnControl(window, itemAutomation, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                    }
                }
                //     returnvalue = WindowsHelper.TakeActionOnControlByID(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if ((sParentControlName != string.Empty) && (sParentControlName != null) && (sParentControlName.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "Name: " + sParentControlName);
                }

                

                 TestStack.White.UIItems.UIItem item = (TestStack.White.UIItems.UIItem)window.Get(SearchCriteria.ByAutomationId(sParentControlName));
                //item.
                List<TestStack.White.UIItems.UIItem> items = window.ItemsWithin(item);
                if(childCount >0)
                {
                    TestStack.White.UIItems.UIItem itemAutomation = items[childCount - 1];
                    if(itemAutomation != null)
                    {
                        sControlType = itemAutomation.GetType().ToString();
                        returnvalue = WindowsHelper.TakeActionOnControl(window, itemAutomation, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                    }
                }
                // returnvalue = WindowsHelper.TakeActionOnControlByName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }


            #region Regular Expression(RegEx) Logic.
            if ((sChildControlIdRegx != string.Empty) && (sChildControlIdRegx != null) && (sChildControlIdRegx.Trim().Length > 0))
            {
                try
                {
                    if (bRaiseErrorIfIDNameClass == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "Name: " + sParentControlName);
                    }
                    TestStack.White.UIItems.UIItem item = (TestStack.White.UIItems.UIItem)window.Get(SearchCriteria.ByAutomationId(sChildControlNameRegx));
                    //item.
                    List<TestStack.White.UIItems.UIItem> items = window.ItemsWithin(item);
                    if (childCount > 0)
                    {
                        TestStack.White.UIItems.UIItem itemAutomation = items[childCount - 1];
                        if (itemAutomation != null)
                        {
                            sControlType = itemAutomation.GetType().ToString();
                            returnvalue = WindowsHelper.TakeActionOnControl(window, itemAutomation, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                        }
                    }
                    // returnvalue = WindowsHelper.TakeActionOnControlByName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                    if (!UseNextPriority)
                        return;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
            }
            #endregion

            if ((sParentClassName != string.Empty) && (sParentClassName != null) && (sParentClassName.Trim().Length > 0))
            {
                if (bRaiseErrorIfIDNameClass == true)
                {
                    RaiseErrorIfControlTypeMissing(context, "Name: " + sParentClassName);
                }
                TestStack.White.UIItems.UIItem item = (TestStack.White.UIItems.UIItem)window.Get(SearchCriteria.ByAutomationId(sParentClassName));
                List<TestStack.White.UIItems.UIItem> items = window.ItemsWithin(item);
              //  returnvalue = WindowsHelper.TakeActionOnControlByClassName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if (!UseNextPriority)
                return;
                   
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
#pragma warning disable CS0219 // The variable 'getSetClick' is assigned but its value is never used
                        GetSetClick getSetClick = new GetSetClick();
#pragma warning restore CS0219 // The variable 'getSetClick' is assigned but its value is never used

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
              //          UseNextPriority = imgRecognition.GetSetClickImage(sImagePath, getSetClick, value, iTimeOutInSeconds);
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
                    //IHTMLDocument2 HtmlDoc = IEWATIN.HtmlDocument;
                    //if (HtmlDoc != null)
                    //{
                    //    IHTMLElement targetElement = HtmlDoc.elementFromPoint(iX, iY);
                    //    if (IsEventField)
                    //    {
                    //        targetElement.click();
                    //    }
                    //    else if ((value != null) && (value != string.Empty))
                    //    {

                    //        targetElement.setAttribute("value", value);
                    //        Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
                    //    }
                    //    else
                    //    {
                    //        GetControlValue.Set(context, targetElement.getAttribute("value").ToString());
                    //        Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
                    //    }
                     
                    //}
                    return;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in set data for control by using co - ordinates", LogLevel.Error);
                    UseNextPriority = true;
                }
            }
        }
        public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach, string strTitleOrUrl)
        {
            // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
            {
               // IEWATIN = (WatiN.Core.IE)RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[ApplicationIDToAttach];
            }
            
        }
        private void RaiseErrorIfControlTypeMissing(NativeActivityContext context, string control)
        {
            Log.Logger.LogData("Please mention Control Type for " + control, LogLevel.Error);
            context.Abort();
            if (SelectHelper._timerExecution != null)
            {
                SelectHelper._timerExecution.Stop();
            }
        }

    }
   

}
