namespace Bot.Activity.ActivityLibrary.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Validation;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;
    using Bot.Activity.ActivityLibrary.Properties;
    using CommonLibrary;
    using System.Drawing;

    /// <summary>
    /// Implements an activity that can execute a XAML file based workflow.
    /// </summary>
    [ToolboxBitmap("Resources/InvokeXamlWorkflow.png")]
    [Designer(typeof(InvokeXamlWorkflowDesigner))]
    public sealed class InvokeXamlWorkflow : NativeActivity //No BaseNativeActivity & Error Handling should be implemented - Ajit
    {
        public Dictionary<string, Argument>ChildArguments { get; set; }
        string strMode = string.Empty;
        Int32 reloadXaml = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeXamlWorkflow"/> class.
        /// </summary>
        public InvokeXamlWorkflow()
        {
            this.ChildArguments = new Dictionary<string, Argument>();
            strMode = ConfigurationManager.AppSettings["Mode"];
            if(strMode.ToLower()=="designer")
            {
                reloadXaml = 1;
            }
        }

        /// <summary>
        /// Gets or sets the file path that specifies the location of the child XAML workflow to execute.
        /// </summary>
        /// <value>
        /// The workflow path.
        /// </value>
        [Category("Input")]
        [DisplayName("Workflow Path")]
        [Description("Enter Workflow Path")]
        public string WorkflowPath { get; set; }

        /// <summary>
        /// Gets or sets the arguments of the child workflow.
        /// </summary>
        /// <value>
        /// The child arguments.
        /// </value>
       

        /// <summary>
        /// Creates and validates a description of the activity’s arguments, variables, child activities, and activity delegates.
        /// </summary>
        /// <param name="metadata">The activity’s metadata that encapsulates the activity’s arguments, variables, child activities, and activity delegates.</param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
        
            foreach (string argumentKey in this.ChildArguments.Keys)
            {
                Argument argument = this.ChildArguments[argumentKey];
                RuntimeArgument runtimeArgument = new RuntimeArgument(argumentKey, argument.ArgumentType, argument.Direction);
                metadata.Bind(argument, runtimeArgument);
                metadata.AddArgument(runtimeArgument);
            }

            try
            {
                DynamicActivity dynamicActivity = this.LoadDynamicActivityFromCache();

                if (dynamicActivity != null)
                {
                    this.Validate(metadata, dynamicActivity);
                }
                else
                {
                    metadata.AddValidationError(Resources.SpecifyValidWorkflowValidationErrorText);
                }
            }
            catch (Exception ex)
            {
                metadata.AddValidationError(string.Format(Resources.FailedToLoadWorkflowValidationErrorText, ex.Message));
            }
        }

        /// <summary>
        /// Execute the child workflow through to completion.
        /// </summary>
        /// <param name="context">The execution context in which the activity executes.</param>
        protected override void Execute(NativeActivityContext context)
        {
            //try
            //{
                Dictionary<string, object> inArgs = new Dictionary<string, object>();

                foreach (string argumentKey in this.ChildArguments.Keys)
                {
                    if (this.ChildArguments[argumentKey].Direction != ArgumentDirection.Out)
                    {
                        object value = this.ChildArguments[argumentKey].Get(context);
                        inArgs.Add(argumentKey, value);
                    }
                }

                IDictionary<string, object> outArgs;

                DynamicActivity dynamicActivity = this.LoadDynamicActivityFromCache();

                if (dynamicActivity != null)
                {
                    WorkflowInvoker invoker = new WorkflowInvoker(dynamicActivity);
                    outArgs = invoker.Invoke(inArgs);
                    
                    foreach (string argumentKey in outArgs.Keys)
                    {
                        this.ChildArguments[argumentKey].Set(context, outArgs[argumentKey]);
                    }
                }
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

        /// <summary>
        /// Loads the dynamic activity from cache.
        /// </summary>
        /// <returns>The activity or null if WorkflowPath property does not have a value.</returns>
        private DynamicActivity LoadDynamicActivityFromCache()
        {
            if (this.WorkflowPath != null && !string.IsNullOrEmpty(this.WorkflowPath))
            {
                if(!this.WorkflowPath.Contains(":"))
                {
                   string fullname = SelectHelper.ProjectLocation + Path.DirectorySeparatorChar + this.WorkflowPath;
                    if(File.Exists(fullname))
                    {
                        return DynamicActivityStore.GetActivity(fullname, reloadXaml); 
                    }
                    else
                    {
                        Logger.Log.Logger.LogData( "Please enter valid file path in activity InvokeXamlWorkflow", Logger.LogLevel.Error);
                        return null;
                    }
                }else
                {
                    return DynamicActivityStore.GetActivity(this.WorkflowPath, reloadXaml);
                }
                
            }
            else
            {
                return null;
            }
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
