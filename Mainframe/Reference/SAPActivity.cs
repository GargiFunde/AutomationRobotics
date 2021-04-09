// <copyright file=SAPActivity company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

//using EV.AE.Studio.ActivityLibrary.Models;
//using EV.AE.Studio.ActivityLibrary.Utils;
//using EV.AE.Studio.ProcessExecution;
//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EV.AE.Studio.ActivityLibrary.Contracts;
//using SE.Core.Automation.Models.Common;
//using Utilities.Logging;
//using EV.AE.Studio.ActivityLibrary.Constraints;
//using Utilities.EntityModels;
//using CommonLibrary;

//namespace Bot.Activity.SAP
//{
//    public class SAPActivity: ActivityExtended
//    {
//        private WorkflowApplication workflowApplication;
//        private Bookmark bookMarkProcess;
//        public bool IsCompleted = true;
//        ExecutionEngine executionEngine;

//        private string vbScriptFileName;
//        [Browsable(false)]
//        public string VbScriptFileName
//        {
//            get
//            {
//                return vbScriptFileName;
//            }

//            set
//            {
//                vbScriptFileName = value;
//            }
//        }


//        private string parameterizedVBScriptContent;
//        [Browsable(false)]
//        public string ParameterizedVBScriptContent
//        {
//            get
//            {
//                return parameterizedVBScriptContent;
//            }

//            set
//            {
//                parameterizedVBScriptContent = value;
//            }
//        }

//        private List<ParameterBase> inputvariables;
//        [Browsable(false)]
//        public List<ParameterBase> Inputvariables
//        {
//            get
//            {
//                return inputvariables;
//            }

//            set
//            {
//                inputvariables = value;
//            }
//        }

//        [Browsable(false)]
//        public int ApplicationID
//        {
//            get;
//            set;
//        }

//        public SAPActivity()
//        {
//            Inputvariables = new List<ParameterBase>();
//            this.DisplayName = "SAP Recorder";
//            Constraints.Add(ActivityConstraint.CheckParent<SAPActivity, AEApplication>
//                  ("SAP Recorder Activity should be inside Application activity"));

//        }

//        protected override void Execute(NativeActivityContext context)
//        {
//            try
//            {
//                Logging.LogDebugMsg(sourceModule, "Execute", "Enter Method");
//                base.Execute(context);
//                bookMarkProcess = context.CreateBookmark(new BookmarkCallback(ResumeBookmark));
//                executionEngine = context.GetExtension<ExecutionEngine>();
//                WorkflowApplication workflowApplication = context.GetExtension<WorkflowApplication>();
//                string inputParams = $"{string.Join(" ", GenerateCommandFromInputVariables(context))}";
//                string scriptPath = Path.GetFullPath($@".\ProtonFiles\{VbScriptFileName.Split('.')[0]}{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)}.vbs");
//                WfStepDetails.EntityName = scriptPath;
//                Task task = new Task(() =>
//                {
//                    try
//                    {
//                        File.WriteAllText(scriptPath, ParameterizedVBScriptContent);
//                        Process process = new Process();
//                        ProcessStartInfo processinfo = new ProcessStartInfo();
//                        processinfo.WindowStyle = ProcessWindowStyle.Hidden;
//                        processinfo.UseShellExecute = false;
//                        processinfo.RedirectStandardOutput = true;
//                        processinfo.RedirectStandardError = true;
//                        processinfo.FileName = "cscript.exe";
//                        processinfo.Arguments = $"\"{scriptPath}\" {inputParams}";
//                        processinfo.CreateNoWindow = true;
//                        process.StartInfo = processinfo;
//                        process.Start();
//                        process.WaitForExit();
//                        string output = process.StandardOutput.ReadToEnd().ToString();
//                        string error = process.StandardError.ReadToEnd().ToString();
//                        if (!string.IsNullOrEmpty(error))
//                            throw new Exception($"error occured : {error }");
//                        WfStepDetails.Status = StepStatus.Success;
//                    }
//                    catch (Exception ex)
//                    {
//                        WfStepDetails.Status = StepStatus.Fail;
//                        Logging.LogExceptionMsg(sourceModule, "Execute", ex.Message, ex, true);
//                        IsCompleted = false;
//                        executionEngine.IsSearchSuccessful = false;
//                    }
//                    finally
//                    {
//                        try
//                        {
//                            if (File.Exists(scriptPath))
//                            {
//                                File.Delete(scriptPath);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            Logging.LogExceptionMsg(sourceModule, "Execute", ex.Message, ex, true);
//                        }
//                    }
//                });
//                task.ContinueWith(ant =>
//                {
//                    base.OnActivityCompletedAction();
//                    workflowApplication.ResumeBookmark(bookMarkProcess, null);
//                });
//                task.Start();
//                Logging.LogDebugMsg(sourceModule, "Execute", "Exit Method");

//            }
//            catch (Exception ex)
//            {
//                IsCompleted = false;
//                executionEngine.IsSearchSuccessful = false;
//                WfStepDetails.Status = StepStatus.Fail;
//                base.OnActivityCompletedAction();
//                Logging.LogExceptionMsg(sourceModule, "Execute", ex.Message, ex, true);
//            }

//        }

//        private void ResumeBookmark(NativeActivityContext context, Bookmark bookmark, object value)
//        {
//        }

//        private List<string> GenerateCommandFromInputVariables(NativeActivityContext context)
//        {
//            try
//            {
//                Logging.LogDebugMsg(sourceModule, "GenerateCommandFromInputVariables", "Enter Method");
//                ApplicationPluginDetails appPluginDetails = context.GetExtension<ExecutionEngine>()
//                        .GetApplicationPluginDetails(ApplicationID);
//                var plugin = appPluginDetails.Plugin;

//                List<string> inputVar = new List<string>();
//                inputVar.Add(((PluginBase)plugin)
//                    .GetApplicationPropertyValue("connectionid"));
//                inputVar.Add(((PluginBase)plugin)
//                    .GetApplicationPropertyValue("sessionid"));

//                Inputvariables?.ForEach(x =>
//                {

//                    if (x.IsDefaultValue)
//                    {
//                        inputVar.Add($"\"{Convert.ToString(x.Value)}\"");
//                    }
//                    else
//                    {
//                        inputVar.Add($"\"{Convert.ToString(context.DataContext.GetProperties()[x.Name].GetValue(context.DataContext))}\"");
//                    }

//                });
//                Logging.LogDebugMsg(sourceModule, "GenerateCommandFromInputVariables", "Exit Method");
//                return inputVar;
//            }
//            catch (Exception ex)
//            {
//                Logging.LogExceptionMsg(sourceModule, "GenerateCommandFromInputVariables", ex.Message, ex, true);
//                throw;
//            }
//        }
//    }
//}
