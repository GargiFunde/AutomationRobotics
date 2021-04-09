using CommonLibrary;
using Logger;

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Bot.Activity.WinDriverPlugin
{
    [ToolboxBitmap("Resources/WindowsControlProperties.png")]
    [Designer(typeof(WindowsControlProperties1))]
    public class Windows_ControlProperties : ActivityExtended
    {
        System.Timers.Timer whiletimer = null;
        bool running = true;
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
        [DisplayName("Set focus")]
        [Description("Set focus")]
        public bool Activate { get; set; }

        [DisplayName("Class Name")]
        // [Description("Classs to find control")]    
        public InArgument<string> ClassName { get; set; }

        [DisplayName("XPath")]
        // [Description("Classs to find control")]    
        public InArgument<string> XPath { get; set; }

        [DisplayName("Control Type [To Identify]")]
        // [Description("Tag Name of control")]      
        public InArgument<string> ControlType { get; set; }

        //[DisplayName("Window Title")]
        ////  [Description("XPath to find control")]
        //public InArgument<string> WindowTitle { get; set; }

        //[DisplayName("Framework Id")]
        ////   [Description("XPath to find control")]

        //public InArgument<string> FrameworkId { get; set; }
           

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
                catch (Exception)
                {
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
                    whiletimer.Enabled = true;
                    while ((running) && (UseNextPriority == true))
                    {
                        PerformActionOncontrol(context, ref returnvalue, ref value);
                    }
                    whiletimer.Enabled = false;
                }
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

        WindowsInstance window = null;
        private void PerformActionOncontrol(NativeActivityContext context, ref object returnvalue, ref string value)
        {
            UseNextPriority = true;
            //string sWindowTitle = WindowTitle.Get(context);
            string sControlId = ControlId.Get(context);
            string sControlName = ControlName.Get(context);
            string sControlType = ControlType.Get(context);
            string sClassName = ClassName.Get(context);
            string sControlXPath = XPath.Get(context);
            string sImagePath = ImagePath.Get(context);
            int iX = X.Get(context);
            int iY = Y.Get(context);
            //int iTimeOutInSeconds = TimeOutInSeconds.Get(context);
            
            
            if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
            {
               
                returnvalue = WindowsHelper.TakeActionOnControlByID(window, sControlId, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                if (!UseNextPriority)
                    return;
            }
            if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
            {
                returnvalue = WindowsHelper.TakeActionOnControlByName(window, sControlName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                GetControlValue.Set(context, returnvalue);
                if (!UseNextPriority)
                    return;
            }
            if ((sClassName != string.Empty) && (sClassName != null) && (sClassName.Trim().Length > 0))
            {
                returnvalue = WindowsHelper.TakeActionOnControlByClassName(window, sClassName, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                GetControlValue.Set(context, returnvalue);
                if (!UseNextPriority)
                    return;
            }
            if ((sControlXPath != string.Empty) && (sControlXPath != null) && (sControlXPath.Trim().Length > 0))
            {
                returnvalue = WindowsHelper.TakeActionOnControlByXPath(window, sControlXPath, sControlType, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);
                GetControlValue.Set(context, returnvalue);
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
                        UseNextPriority = imgRecognition.GetSetClickImage(sImagePath, getSetClick, value);
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
                catch (Exception)
                {
                    Log.Logger.LogData("Error in set data for control by using co - ordinates", LogLevel.Error);
                    UseNextPriority = true;
                }
            }
        }
        public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach, string strTitleOrUrl)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
            {
                window = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationIDToAttach];
            }
        }   
        
    }
   

}
