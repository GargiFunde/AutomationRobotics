//-----------------------------------------------------------------------
// <copyright file="ExecuteXamlWorkflowDesigner.xaml.cs" company="Microsoft Corporation">
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
    using CommonLibrary;
    using System;
    using System.Activities;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;

    /// <summary>
    /// Implements a custom designer for the ExecuteXamlWorkflow activity.
    /// </summary>
    public partial class SOAPRequestDesigner
    {
        /// <summary>
        /// The file path of a the currently specified child workflow.
        /// </summary>
        private string currentlyLoadedWorkflowPath;

        SoapWizard soapWizard = null;

        Dictionary<string, InArgument> argumentDictionary = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteXamlWorkflowDesigner"/> class.
        /// </summary>
        public SOAPRequestDesigner()
        {
            InitializeComponent();
           
            this.Loaded += new RoutedEventHandler(this.ExecuteXamlWorkflowDesigner_Loaded);

            if (soapWizard == null)
            {
                soapWizard = new SoapWizard();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the ExecuteXamlWorkflowDesigner control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        public void ExecuteXamlWorkflowDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            this.ModelItem.PropertyChanged += new PropertyChangedEventHandler(this.ModelItem_PropertyChanged);

            ModelItem modelItem = this.ModelItem.Properties["ChildArguments"].Dictionary;
            if (modelItem == null)
            {
                argumentDictionary = new Dictionary<string, InArgument>();
            }
            else
            {
                argumentDictionary = (Dictionary<string, InArgument>)modelItem.GetCurrentValue();
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ModelItem control and detects if the workflow path has been modified. If changed, initialises
        /// the DynamicArgumentDialog with the newly identified child's workflow arguments.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ModelItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("WorkflowPath"))
            //{
            //    this.InitImportDynamicArgumentDialog();
            //}
            // argumentDictionary = null;
        }

        /// <summary>
        /// Handles the Click event of the FileDialogButton control to launch an OpenFileDialog instance.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                this.ModelItem.Properties["WorkflowPath"].SetValue(fileDialog.FileName);       
            }
        }

        /// <summary>
        /// Handles the Click event of the DefineArgsButton control to launch a DynamicArgumentDialog instance for argument editing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DefineArgsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult dialogResult = soapWizard.ShowDialog();
                if (dialogResult == DialogResult.Yes)
                {
                    DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
                    {
                        Title = SOAP.Properties.Resources.DynamicArgumentDialogTitle
                    };

                    this.InitImportDynamicArgumentDialog();

                    this.ModelItem.Properties["ChildArguments"].SetValue(argumentDictionary);
                    ModelItem modelItem = this.ModelItem.Properties["ChildArguments"].Dictionary;


                    using (ModelEditingScope change = modelItem.BeginEdit("ChildArgumentEditing"))
                    {

                        ThreadInvoker.Instance.RunByUiThread(() =>
                        {

                            if (DynamicArgumentDialog.ShowDialog(this.ModelItem, modelItem, Context, this.ModelItem.View, options))
                            {
                                change.Complete();
                            }
                            else
                            {
                                change.Revert();
                            }
                        });
                    }
                }
            }catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in Import:" + ex.Message,Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// Handles the Click event of the DefineArgsButton control to launch a DynamicArgumentDialog instance for argument editing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EditArgsButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
            {
                Title = Bot.Activity.ActivityLibrary.Properties.Resources.DynamicArgumentDialogTitle
            };

            ModelItem modelItem = this.ModelItem.Properties["ChildArguments"].Dictionary;
            using (ModelEditingScope change = modelItem.BeginEdit("ChildArgumentEditing"))
            {

                ThreadInvoker.Instance.RunByUiThread(() =>
                {

                    if (DynamicArgumentDialog.ShowDialog(this.ModelItem, modelItem, Context, this.ModelItem.View, options))
                    {
                        change.Complete();
                    }
                    else
                    {
                        change.Revert();
                    }
                });
            }
        }

        /// <summary>
        /// Initialises the DynamicArgumentDialog instance with child workflow arguments by loading the specified child workflow then deriving its arguments.
        /// </summary>
        private void InitImportDynamicArgumentDialog()
        {
            argumentDictionary = new Dictionary<string, InArgument>();
            if (!string.IsNullOrEmpty(soapWizard.EndPoint))
            {
                InArgument<string> newArgEndPoint = soapWizard.EndPoint;
                this.ModelItem.Properties["EndPoint"].SetValue(newArgEndPoint);
            }
            else
            {
                InArgument<string> newArgEndPoint = "";
                this.ModelItem.Properties["EndPoint"].SetValue(newArgEndPoint);
            }
            if (!string.IsNullOrEmpty(soapWizard.Service))
            {
                InArgument<string> newArgService = soapWizard.Service;
                this.ModelItem.Properties["ServiceName"].SetValue(newArgService);
            }
            else
            {
                InArgument<string> newArgService = "";
                this.ModelItem.Properties["ServiceName"].SetValue(newArgService);
            }
            if (!string.IsNullOrEmpty(soapWizard.ServiceMethodName))
            {
                InArgument<string> newArgMethod = soapWizard.ServiceMethodName;
                this.ModelItem.Properties["Method"].SetValue(newArgMethod);
            }
            else
            {
                InArgument<string> newArgMethod = "";
                this.ModelItem.Properties["Method"].SetValue(newArgMethod);
            }
            if (soapWizard.method == null)
            {
                 return;
            }
              
            WebMethod method = soapWizard.method;
            object obj = method.ConfigureArguments;
            if (obj != null)
            {
              

                Type t = obj.GetType();
                // Get the public properties.
                PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                try
                {
                    if ((propInfos != null) && (propInfos.Length > 0))
                    {
                        for (int i = 0; i < propInfos.Length; i++)
                        {
                            InArgument newArgument = null;
                            newArgument = (InArgument)Argument.Create(propInfos[i].PropertyType, ArgumentDirection.In);
                            //newArgument.Set(null,propInfos[i].GetValue(obj)); - not working, dynamic type
                            argumentDictionary.Add(propInfos[i].Name, newArgument);
                        }
                    }
                }
                catch
                {
                    // ignore load failures - leave to handle in CacheMetadata
                }
            }
            object ServiceArguments = method.ConfigureService;
            if (ServiceArguments != null)
            {
                SetServiceProperties(ServiceArguments);
            }
        }

        private void SetServiceProperties(object serviceArguments)
        {
            Type t = serviceArguments.GetType();
            // Get the public properties.
            PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                if ((propInfos != null) && (propInfos.Length > 0))
                {
                    for (int i = 0; i < propInfos.Length; i++)
                    {
                        if(propInfos[i].Name == "ClientCertificates")
                        {
                          //  InArgument<string> newArgEndPoint = propInfos[i].GetValue(serviceArguments);
                          //  this.ModelItem.Properties[""].SetValue(newArgEndPoint);
                        }
                        if (propInfos[i].Name == "Credentials")
                        {

                        }
                        if (propInfos[i].Name == "UseDefaultCredentials")
                        {
                            bool bUseDefaultCredentials =(bool )propInfos[i].GetValue(serviceArguments);
                            this.ModelItem.Properties["UseWindowsCredentials"].SetValue(bUseDefaultCredentials);
                        }
                        if (propInfos[i].Name == "Url")
                        {
                            
                        }
                        if (propInfos[i].Name == "Timeout")
                        {

                        }
                       
                    }
                }
            }
            catch
            {
                // ignore load failures - leave to handle in CacheMetadata
            }
        }
    }
}
