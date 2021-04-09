//-----------------------------------------------------------------------
// <copyright file="ExecuteXamlWorkflow.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//-----------------------------------------------------------------------
namespace SOAP
{
    using System;
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Validation;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing.Design;
    using System.IO;
   // using System.Workflow.ComponentModel.Design;
    using SOAP.Properties;
    using CommonLibrary;
    using System.Activities.Presentation.PropertyEditing;
    using System.Reflection;
    using System.Drawing;

    /// <summary>
    /// Implements an activity that can execute a XAML file based workflow.
    /// </summary>
    [Designer(typeof(SOAPRequestDesigner))]
    [ToolboxBitmap("Resources/SOAPRequest.png")]

    public sealed class SOAPRequest : BaseNativeActivity //No BaseNativeActivity & Error Handling should be implemented - Ajit
    {

        [Category("Authentication:Client Certificate")]
        [DisplayName("Client Certificate")]
        public InArgument<string> ClientCertificate { get; set; }

        [Category("Authentication:Client Certificate")]
        [DisplayName("Client Certificate Password")]
        public InArgument<string> ClientCertificatePassword { get; set; }

        [Category("Authentication:Simple")]
        [DisplayName("User Name")]
        public InArgument<string> UserName { get; set; }

        [Category("Authentication:Simple")]
        [DisplayName("Password")]
        public InArgument<string>Password { get; set; }

        [Category("Authentication:Windows")]
        [DisplayName("User Windows Credentials")]
        public bool UseWindowsCredentials { get; set; }

        [Category("Input")]
        [DisplayName("Service Name")]
        public InArgument<string> ServiceName { get; set; }

        [Category("Input")]
        [DisplayName("End Point")]
        public InArgument<string> EndPoint { get; set; }

        [Category("Input")]
        [DisplayName("Method")]
        public InArgument<string> Method { get; set; }

        [Category("Input")]
        [DisplayName("Parameters")]
        [Editor(typeof(ArgumentDictionaryEditor), typeof(PropertyValueEditor))]
        [Browsable(true)]
        public IDictionary<string, InArgument> ChildArguments { get; set; }
        //{
        //    get
        //    {
        //        if (this.ChildArguments == null)
        //        {
        //            this.ChildArguments = new Dictionary<string, InArgument>();
        //        }
        //        return this.ChildArguments;
        //    }
        //    set
        //    {
        //        this.ChildArguments = value;
        //    }
        //}


        [Category("Output")]
        [DisplayName("Headers")]
        public OutArgument<string> Headers { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        public OutArgument<string> Result { get; set; }
      //  public Dictionary<string, InArgument> ChildArguments { get; set; }
        string strMode = string.Empty;
        Int32 reloadXaml = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeXamlWorkflow"/> class.
        /// </summary>
        public SOAPRequest()
        {
            ChildArguments = new Dictionary<string, InArgument>();
            strMode = ConfigurationManager.AppSettings["Mode"];
            if(strMode.ToLower()=="designer")
            {
                reloadXaml = 1;
            }
        }

       
        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables, child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.</param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //Need to register all arguments
            //Argument argumentr = Result;
            //RuntimeArgument runtimeArgumentr = new RuntimeArgument("Result", argumentr.ArgumentType, argumentr.Direction);
            //metadata.Bind(argumentr, runtimeArgumentr);
            //metadata.AddArgument(runtimeArgumentr);

            foreach (string argumentKey in this.ChildArguments.Keys)
            {
                Argument argument = this.ChildArguments[argumentKey];
                RuntimeArgument runtimeArgument = new RuntimeArgument(argumentKey, argument.ArgumentType, argument.Direction);
                metadata.Bind(argument, runtimeArgument);
                metadata.AddArgument(runtimeArgument);
            }
        }

        /// <summary>
        /// Execute the child workflow through to completion.
        /// </summary>
        /// <param name="context">The execution context in which the activity executes.</param>
        protected override void Execute(NativeActivityContext context)
        {

            try
            { 
                ExcecuteSOAPRequest( context);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity SoapRequest", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
            //try
            //{
            //Dictionary<string, object> inArgs = new Dictionary<string, object>();

            //foreach (string argumentKey in this.ChildArguments.Keys)
            //{
            //    if (this.ChildArguments[argumentKey].Direction != ArgumentDirection.Out)
            //    {
            //        object value = this.ChildArguments[argumentKey].Get(context);
            //        inArgs.Add(argumentKey, value);
            //    }
            //}

            //IDictionary<string, object> outArgs;

            //DynamicActivity dynamicActivity = this.LoadDynamicActivityFromCache();

            //if (dynamicActivity != null)
            //{
            //    WorkflowInvoker invoker = new WorkflowInvoker(dynamicActivity);
            //    outArgs = invoker.Invoke(inArgs);

            //    foreach (string argumentKey in outArgs.Keys)
            //    {
            //        this.ChildArguments[argumentKey].Set(context, outArgs[argumentKey]);
            //    }
            //}
            //} ----No Error Handling should be implemented----
            //catch (Exception ex)
            //{
            //    Logger.Log.Logger.LogData(ex.Message + " in activity InvokeXamlWorkflow", Logger.LogLevel.Error);
            //    if (!ContinueOnError)
            //    {
            //        context.Abort();
            //    }
            //}
        }

        private void ExcecuteSOAPRequest(NativeActivityContext context)
        {
           
            WebServiceList list = WebServiceList.LoadFromUrl(EndPoint.Expression.ToString());
            foreach (WebService svc in list.Services)
            {
                if (svc.Name == ServiceName.Expression.ToString())
                {
                    foreach (WebMethod method in svc.Methods)
                    {
                        if (method.Name == Method.Expression.ToString())
                        {
                            object obj = method.Arg;
                            Type t = obj.GetType();
                            // Get the public properties.
                            PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            try
                            {
                                if ((propInfos != null) && (propInfos.Length > 0))
                                {
                                    for (int i = 0; i < propInfos.Length; i++)
                                    {
                                        if (ChildArguments.ContainsKey(propInfos[i].Name))
                                        {
                                            InArgument objInputArg = ChildArguments[propInfos[i].Name];
                                            object objInput = objInputArg.Get(context);
                                            propInfos[i].SetValue(obj, Convert.ChangeType(objInput, propInfos[i].PropertyType), null);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // ignore load failures - leave to handle in CacheMetadata
                            }
                            TraceExtension.SoapTextBoxString = "";
                            object objResult = method.Invoke();
                            string strresult = TraceExtension.SoapTextBoxString;
                            if (!string.IsNullOrEmpty(strresult))
                            {
                                Result.Set(context, strresult);
                            }
                            else
                            {
                                Result.Set(context, "No output received.");
                            }
                            break;
                        }
                    }
                }
            }
            
        }
        /// <summary>
        /// Loads the dynamic activity from cache.
        /// </summary>
        /// <returns>The activity or null if WorkflowPath property does not have a value.</returns>
        private DynamicActivity LoadDynamicActivityFromCache()
        {
            //if (this.WorkflowPath != null && !string.IsNullOrEmpty(this.WorkflowPath))
            //{
            //    if(!this.WorkflowPath.Contains(":"))
            //    {
                   //string fullname = SelectHelper.ProjectLocation + Path.DirectorySeparatorChar + this.WorkflowPath;
                   // if(File.Exists(fullname))
                   // {
                   //     return DynamicActivityStore.GetActivity(fullname, reloadXaml); 
                   // }
                   // else
                   // {
                   //     Logger.Log.Logger.LogData( "Please enter valid file path in activity InvokeXamlWorkflow", Logger.LogLevel.Error);
                   //     return null;
                   // }
                //}
                //else
                //{
                //    return DynamicActivityStore.GetActivity(this.WorkflowPath, reloadXaml);
                //}
                
            //}
            //else
            //{
                return null;
            //}
        }

        /// <summary>
        /// Validates the arguments in ChildArguments property against the arguments of specified dynamicActivity instance by adding a validation error
        /// to supplied metadata if the argument is wrong type, direction or does not exist.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="dynamicActivity">The dynamic activity.</param>
        private void Validate(NativeActivityMetadata metadata, DynamicActivity dynamicActivity)
        {
            foreach (string argumentKey in this.ChildArguments.Keys)
            {
                if (dynamicActivity.Properties.Contains(argumentKey))
                {
                    DynamicActivityProperty dynamicActivityProperty = dynamicActivity.Properties[argumentKey];
                    Argument arg = this.ChildArguments[argumentKey];
                    if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(InArgument<>) && arg.Direction != ArgumentDirection.In)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidInArgumentDirectionValidationErrorText, argumentKey)));
                    }
                    else if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(OutArgument<>) && arg.Direction != ArgumentDirection.Out)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidOutArgumentDirectionValidationErrorText, argumentKey)));
                    }
                    else if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(InOutArgument<>) && arg.Direction != ArgumentDirection.InOut)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidInOutArgumentDirectionValidationErrorText, argumentKey)));
                    }

                    if (dynamicActivityProperty.Type.GetGenericArguments()[0] != arg.ArgumentType)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidIArgumentTypeValidationErrorText, argumentKey, dynamicActivityProperty.Type.GetGenericArguments()[0])));
                    }
                }
                else
                {
                    metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidIArgumentValidationErrorText, argumentKey)));
                }
            }
        }
    }
}
