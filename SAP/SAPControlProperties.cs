using CommonLibrary;
using Logger;
using sapfewse;
using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Bot.Activity.SAP
{
    [Designer(typeof(SapControlProperties1))]
    public class SAP_ControlProperties:ActivityExtended
    {

        [DisplayName("Set focus")]
        [Description("Set focus")]
        public bool Activate { get; set; }

        [DisplayName("Active Window")]
        [Description("Active Window to find control")]
        public InArgument<string> ActiveWindow { get; set; }
     
        [DisplayName("Control Type")]
        [Description("Control Type to find control")]
        public InArgument<string> ControlType { get; set; }


        [DisplayName("Row Number")]
        [Description("Row Number to find control")]
        public InArgument<int> RowNumber { get; set; }

        [DisplayName("Cell Name")]
        [Description("Cell Name to find control")]
        public InArgument<string> CellName { get; set; }


        [DisplayName("Node Key")]
        [Description("Node Key of GUI Tree")]
        [CategoryAttribute("Gui Tree")]
        public InArgument<string> NodeKey { get; set; }

        [DisplayName("Double Click")]
        [Description("Click The GuiTree Node")]
        [CategoryAttribute("Gui Tree")]
        public bool IsDoubleClick { get; set; }


        GuiSession SapSession = null;
        bool UseNextPriority = true;
        protected override void Execute(NativeActivityContext context)
        {
            string returnvalue = string.Empty;
            string value = string.Empty;
            try
            {

                Thread.Sleep(500);
                try
                {
                    if (SetControlValue.Get(context) != null)
                    {
                        value = SetControlValue.Get(context).ToString();

                    }
                }
                catch (Exception ex)
                {
                    //no error added in log
                }

                PerformActionOncontrol(context, ref returnvalue, ref value);

                if ((value == null) || (value == string.Empty) || (value.Trim().Length == 0))
                {
                    if ((returnvalue != string.Empty) && (returnvalue != null) && (returnvalue.Trim().Length > 0) && (IsEventField == false))
                    {
                        GetControlValue.Set(context, returnvalue);
                        Log.Logger.LogData("'Get Value' is set", LogLevel.Info);
                    }
                }
                //int iSleepInSecond = SleepInSeconds.Get(context);
                //if (iSleepInSecond > 0)
                //{
                //    Thread.Sleep(iSleepInSecond * 1000);
                //}
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
        private void PerformActionOncontrol(NativeActivityContext context, ref string returnvalue, ref string value)
        {
            try
            {
                UseNextPriority = true;
                string sControlId = ControlId.Get(context);
                string sControlName = ControlName.Get(context);
                string sTagName = ControlType.Get(context);
                string activewindow = ActiveWindow.Get(context);
                int rowNumber = RowNumber.Get(context);
                string cellName = CellName.Get(context);

                string sImagePath = ImagePath.Get(context);
                int iX = X.Get(context);
                int iY = Y.Get(context);
                //int iTimeOutInSeconds = TimeOutInSeconds.Get(context);



                //Keys
                string key = NodeKey.Get(context); 
             
                
           
                bool bRaiseErrorIfIDNameClassSibling = false;

                if ((sTagName == null) || (sTagName == string.Empty) || (sTagName.Trim().Length == 0))
                {
                    bRaiseErrorIfIDNameClassSibling = true;
                }
                if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
                {
                    if (bRaiseErrorIfIDNameClassSibling == true)
                    {
                        RaiseErrorIfControlTypeMissing(context, "ID: " + sControlId);
                    }
                    //  returnvalue = SAPHelper.TakeActionOnControlByID(SapSession, sControlId, sTagName, IsEventField, key, IsDoubleClick, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);

                    returnvalue = SAPHelper.TakeActionOnControlByID(SapSession, sControlId, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value,key,IsDoubleClick, ref UseNextPriority);

                    if (!UseNextPriority)
                        return;
                }

                //if ((sControlName != string.Empty) && (sControlName != null) && (sControlName.Trim().Length > 0))
                //{
                //    if (bRaiseErrorIfIDNameClassSibling == true)
                //    {
                //        RaiseErrorIfControlTypeMissing(context, "Name: " + sControlName);
                //    }
                //    returnvalue = SAPHelper.TakeActionOnControlByName(IEWATIN, sControlName, sTagName, IsEventField, Activate, ClickBeforeValueSet, ContinueOnError, value, ref UseNextPriority);

                //    if (!UseNextPriority)
                //        return;
                //}
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
                      
                        if (SapSession != null)
                        {
                            GuiCollection targetElement = SapSession.FindByPosition(iX, iY);
                            if (IsEventField)
                            {
                                
                            }
                            else if ((value != null) && (value != string.Empty))
                            {

                              //  targetElement.setAttribute("value", value);
                                Log.Logger.LogData("Set data for control by using co-ordinates", LogLevel.Info);
                            }
                            else
                            {
                               // GetControlValue.Set(context, targetElement.getAttribute("value").ToString());
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
        public override void ExecuteMe(NativeActivityContext context, string ApplicationIDToAttach,string Title="")
        {
            try
            {
                // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationIDToAttach))
                {
                    SAPEntity sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[ApplicationIDToAttach];
                    SapSession = sapEntity.SapSession;
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
