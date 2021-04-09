// <copyright file=WindowsControlProperties company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:13</date>
// <summary></summary>

//using CommonLibrary;
//using Logger;

//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;


//namespace Core.ActivityLibrary
//{
//    [Designer(typeof(WindowsControlProperties1))]
//    public class WindowsControlProperties : ActivityExtended
//    {
       
//        public WindowsControlProperties() : base()
//        {
//           // SetControlValue = new Object();
//        }

//        [DisplayName("Set focus")]
//        [Description("Set focus")]
//        public bool Activate { get; set; }

//        [DisplayName("Class Name")]
//        // [Description("Classs to find control")]    
//        public InArgument<string> ClassName { get; set; }

//        [DisplayName("Control Type [To Identify]")]
//        // [Description("Tag Name of control")]      
//        public InArgument<string> ControlType { get; set; }

//        [DisplayName("Window Title")]
//        //  [Description("XPath to find control")]
//        public InArgument<string> WindowTitle { get; set; }

//        [DisplayName("Framework Id")]
//        //   [Description("XPath to find control")]

//        public InArgument<string> FrameworkId { get; set; }
           

//        protected override void Execute(NativeActivityContext context)
//        {

//            object returnvalue = string.Empty;
//            string value = string.Empty;
//            try
//            {
//                try
//                {
//                    if (SetControlValue.Get(context) != null)
//                    {
//                        value = SetControlValue.Get(context).ToString();

//                    }
//                }
//                catch (Exception ex)
//                {
//                    //no error added in log
//                }

//                PerformActionOncontrol(context, ref returnvalue, ref value);

//                if ((value == null) || (value == string.Empty) || (value.Trim().Length == 0))
//                {          
//                    if ((returnvalue != null) && (IsEventField == false))
//                    {
//                        GetControlValue.Set(context, returnvalue);
//                        Log.Logger.LogData("'Get Value' is set", LogLevel.Info);
//                    }
//                }
//                 // IEWATIN.WaitForComplete(5000);
//                int iSleepInSecond = SleepInSeconds.Get(context);
//                if (iSleepInSecond > 0)
//                {
//                    Thread.Sleep(iSleepInSecond * 1000);
//                }
//                if (UseNextPriority)
//                {
//                    if ((ErrorMessage != null) && (ErrorMessage.Expression != null))
//                    {
//                        Log.Logger.LogData(ErrorMessage.Expression.ToString(), LogLevel.Error);
//                    }
//                    if (!ContinueOnError)
//                    {
//                        context.Abort();
//                        if (SelectHelper._timerExecution != null)
//                        {
//                            SelectHelper._timerExecution.Stop();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Logger.LogData(ex.Message, LogLevel.Error);
//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                    if (SelectHelper._timerExecution != null)
//                    {
//                        SelectHelper._timerExecution.Stop();
//                    }
//                }
//            }

//        }
//        bool UseNextPriority = true;
//        private void PerformActionOncontrol(NativeActivityContext context, ref object returnvalue, ref string value)
//        {
//            UseNextPriority = true;
//            string sWindowTitle = WindowTitle.Get(context);
//            string sControlId = ControlId.Get(context);
//            string sControlName = ControlName.Get(context);
//            string sControlType = ControlType.Get(context);
//            string sClassName = ClassName.Get(context);
//            string sImagePath = ImagePath.Get(context);
//            int iX = X.Get(context);
//            int iY = Y.Get(context);
//            int iTimeOutInSeconds = TimeOutInSeconds.Get(context);
//            bool bRaiseErrorIfIDNameClass = false;
//            White.Core.UIItems.WindowItems.Window window = null;

//            //if (RuntimeApplicationHelper.Instance.RuntimeApplicationObjects.ContainsKey(AppId))
//            //{
//            //    window = WindowsHelper.GetApplicationObject(sWindowTitle);
//            //}
//            //else
//            //{
//                window = WindowsHelper.GetApplicationObject(sWindowTitle);
//            //}
          
//            if (window == null)
//            {
//                Logger.Log.Logger.LogData("No desktop application window found with title :" + sWindowTitle, Logger.LogLevel.Error);
//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                    if (SelectHelper._timerExecution != null)
//                    {
//                        SelectHelper._timerExecution.Stop();
//                    }
//                }
//            }

//            if ((sControlType == null) || (sControlType == string.Empty) || (sControlType.Trim().Length == 0))
//            {
//                bRaiseErrorIfIDNameClass = true;
//            }
//            if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
//            {
//                if (bRaiseErrorIfIDNameClass == true)
//                {
//                    RaiseErrorIfControlTypeMissing(context, "ID: " + sControlId);
//                }
//                returnvalue = WindowsHelper.TakeActionOnControlByID(window, sControlId, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
//                if (!UseNextPriority)
//                    return;
//            }
//            if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
//            {
//                if (bRaiseErrorIfIDNameClass == true)
//                {
//                    RaiseErrorIfControlTypeMissing(context, "Name: " + sControlName);
//                }
//                returnvalue = WindowsHelper.TakeActionOnControlByName(window, sControlName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
//                if (!UseNextPriority)
//                    return;
//            }
//            if ((sClassName != string.Empty) && (sClassName != null) && (sClassName.Trim().Length > 0))
//            {
//                if (bRaiseErrorIfIDNameClass == true)
//                {
//                    RaiseErrorIfControlTypeMissing(context, "Name: " + sClassName);
//                }
//                returnvalue = WindowsHelper.TakeActionOnControlByClassName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
//                if (!UseNextPriority)
//                    return;
//            }
//            if (!UseNextPriority)
//                return;
                   
//            if ((sImagePath != null) && (sImagePath != string.Empty) && (sImagePath.Length > 0) && (UseNextPriority))
//            {
//                try
//                {

//                    if (File.Exists(sImagePath))
//                    {
//                        value = string.Empty;
//                        GetSetClick getSetClick = new GetSetClick();

//                        ImageRecognition imgRecognition = new ImageRecognition();
//                        if (IsEventField)
//                        {
//                            getSetClick = GetSetClick.Click;
//                        }
//                        else
//                        {
//                            value = SetControlValue.Get(context).ToString();
//                            getSetClick = GetSetClick.GetSet;

//                        }
//                        UseNextPriority = imgRecognition.GetSetClickImage(sImagePath, getSetClick, value, iTimeOutInSeconds);
//                    }
//                    return;
//                }
//                catch (Exception ex)
//                {
//                    Log.Logger.LogData(ex.Message, LogLevel.Error);
//                    UseNextPriority = true;
//                    //Automation Failed
//                    Log.Logger.LogData("Automation failed", LogLevel.Error);
//                }
//            }
//            if ((iX > 0) && (iY > 0) && (UseNextPriority))
//            {
//                try
//                {
//                    //IHTMLDocument2 HtmlDoc = IEWATIN.HtmlDocument;
//                    //if (HtmlDoc != null)
//                    //{
//                    //    IHTMLElement targetElement = HtmlDoc.elementFromPoint(iX, iY);
//                    //    if (IsEventField)
//                    //    {
//                    //        targetElement.click();
//                    //    }
//                    //    else if ((value != null) && (value != string.Empty))
//                    //    {

//                    //        targetElement.setAttribute("value", value);
//                    //        Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
//                    //    }
//                    //    else
//                    //    {
//                    //        GetControlValue.Set(context, targetElement.getAttribute("value").ToString());
//                    //        Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
//                    //    }
                     
//                    //}
//                    return;
//                }
//                catch (Exception ex)
//                {
//                    Log.Logger.LogData("Error in set data for control by using co - ordinates", LogLevel.Error);
//                    UseNextPriority = true;
//                }
//            }
//        }
//        public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach)
//        {
//            // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
//            if (RuntimeApplicationHelper.Instance.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
//            {
//               // IEWATIN = (WatiN.Core.IE)RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[ApplicationIDToAttach];
//            }
//        }
//        private void RaiseErrorIfControlTypeMissing(NativeActivityContext context, string control)
//        {
//            Log.Logger.LogData("Please mention Control Type for " + control, LogLevel.Error);
//            context.Abort();
//            if (SelectHelper._timerExecution != null)
//            {
//                SelectHelper._timerExecution.Stop();
//            }
//        }

//    }
   

//}
