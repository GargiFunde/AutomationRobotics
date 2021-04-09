// <copyright file=SAPFieldExtractorActivity company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

//using EV.AE.Automation.Common.SAP;
//using EV.AE.Studio.ActivityLibrary.Models;
//using EV.AE.Studio.ActivityLibrary.Utils;
//using EV.AE.Studio.ProcessExecution;
//using SE.Core.Automation.Interfaces;
//using SE.Core.Automation.IoCContainer;
//using SE.Core.Automation.Models.Common;
//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using Utilities.Logging;
//using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
//using EV.AE.Studio.ActivityLibrary.Contracts;
//using EV.AE.Studio.ActivityLibrary.Constraints;
//using Utilities.EntityModels;;

//namespace Bot.Activity.SAP
//{
//   public class SAPFieldExtractorActivity
//    {
//        public bool IsCompleted = true;
//        ExecutionEngine executionEngine;

//        SAPControlData sapControlData = new SAPControlData();
//        public SAPControlData SAPControlData
//        {
//            get
//            {
//                return sapControlData;
//            }
//            set
//            {
//                sapControlData = value;
//            }
//        }


//        [Category("Contol Execution")]
//        [DisplayName("Ignore Error")]
//        public IgnoreError IgnoreError { get; set; }

//        Parameter parameter;
//        [Browsable(false)]
//        public Parameter Parameter
//        {
//            get
//            {
//                return parameter;
//            }
//            set
//            {
//                parameter = value;
//            }
//        }

//        [Browsable(false)]
//        public int ApplicationID
//        {
//            get;
//            set;
//        }

//        public SAPFieldExtractorActivity()
//        {
//            this.DisplayName = "SAP Field Extractor";
//            this.IgnoreError = IgnoreError.No;
//            this.parameter = new Parameter();
//            Constraints.Add(ActivityConstraint.CheckParent<SAPFieldExtractorActivity, AEApplication>
//                 ("SAP Field Extractor Activity should be inside Application activity"));
//        }

//        protected override void Execute(NativeActivityContext context)
//        {
//            try
//            {
//                Logging.LogDebugMsg(sourceModule, "Execute", "Entered");
//                executionEngine = context.GetExtension<ExecutionEngine>();
//                base.Execute(context);

//                IAutomatedAction actionToPerform = this.sapControlData.InteractionClass;
//                if (actionToPerform.RequiresInput())
//                {
//                    string inputVariableName = actionToPerform.GetInputVariable();
//                    if (context != null && !string.IsNullOrEmpty(inputVariableName))
//                    {
//                        string inputValue = context.DataContext.GetProperties()[inputVariableName].GetValue(context.DataContext).ToString();
//                        actionToPerform.SetInputValue(inputValue);
//                    }
//                }
//                ApplicationPluginDetails appPluginDetails = context.GetExtension<ExecutionEngine>()
//                    .GetApplicationPluginDetails(ApplicationID);
//                var plugin = appPluginDetails.Plugin;
//                WfStepDetails.EntityName = appPluginDetails.AppDisplayName;

//                this.SAPControlData.ConnectionId = Convert.ToInt32(plugin.GetApplicationPropertyValue("connectionid"));

//                this.SAPControlData.SessionId = Convert.ToInt32(plugin.GetApplicationPropertyValue("sessionid"));

//                SAPControlDetails sapControlDetails = new SAPControlDetails()
//                {
//                    ConnectionId = this.sapControlData.ConnectionId,
//                    SessionId = this.sapControlData.SessionId,
//                    ControlId = this.sapControlData.ControlId,
//                    ControlType = this.sapControlData.ControlType
//                };
//                actionToPerform.DoAutomation<SAPControlDetails>(sapControlDetails);

//                string extractedOutput = actionToPerform.GetExtractedValue();


//                if (extractedOutput != null)
//                {
//                    PropertyDescriptor outParameter = context.DataContext.GetProperties().Find(parameter.Name, true);
//                    bool canParse;
//                    object parsedData = extractedOutput.ParseAttributeValue(sapControlData.ParameterDataType, outParameter.PropertyType, out canParse);
//                    if (canParse)
//                        outParameter.SetValue(context.DataContext, parsedData);
//                }


//                executionEngine.FillExtractedFeilds(ApplicationID, new Dictionary<string, string>() { { parameter.Name, extractedOutput } });
//                if (parameter.IsCview)
//                {
//                    executionEngine.FillCView(ApplicationID, new Dictionary<string, string>() { { parameter.Name, extractedOutput } });
//                }
//                WfStepDetails.Status = StepStatus.Success;
//            }
//            catch (Exception ex)
//            {
//                WfStepDetails.Status = StepStatus.Fail;
//                Logging.LogExceptionMsg(sourceModule, "Execute", ex.Message, ex, logStackTrace: true);
//                IsCompleted = false;
//                executionEngine.IsSearchSuccessful = false;
//                if (IgnoreError == ActivityLibrary.IgnoreError.No)
//                {
//                    throw;
//                }
//            }
//            finally
//            {
//                base.OnActivityCompletedAction();
//            }

//            Logging.LogDebugMsg(sourceModule, "Execute", "Exit");

//        }

//        protected override void CacheMetadata(NativeActivityMetadata metadata)
//        {
//            base.CacheMetadata(metadata);
//            if (string.IsNullOrEmpty(this.sapControlData.InteractionAction)
//                || string.IsNullOrEmpty(this.sapControlData.ControlId)
//                || string.IsNullOrEmpty(this.sapControlData.ControlType)
//                || string.IsNullOrEmpty(this.sapControlData.OutParameter))
//            {
//                metadata.AddValidationError("One of the required field is not configured");
//            }


//            if (this.sapControlData.IsCView && string.IsNullOrEmpty(this.sapControlData.CViewText))
//            {
//                metadata.AddValidationError("One of the required field is not configured");
//            }

//        }


//        #region INotifyPropertyChanged

//        public event PropertyChangedEventHandler PropertyChanged = delegate { };

//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//        }

//        #endregion INotifyPropertyChanged

//        //public override string ToString()
//        //{
//        //    return "EV.AE.Studio.ActivityLibrary.SapFieldExtractorActivity";
//        //}
//    }
//}
