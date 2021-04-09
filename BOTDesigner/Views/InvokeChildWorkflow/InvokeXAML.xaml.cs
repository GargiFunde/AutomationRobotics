using CommonLibrary;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using BOTDesigner.Views;

namespace BOTDesigner.Views.InvokeChildWorkflow
{

    // Interaction logic for InvokeXAML.xaml
    public partial class InvokeXAML
    {

        /// <summary>
        /// The file path of a the currently specified child workflow.
        /// </summary>
        private string currentlyLoadedWorkflowPath;
        public string workflowPath = string.Empty;
        public event OpenProjectEventHandler OpenProject;
        public delegate void OpenProjectEventHandler(object sender, NewProjectEventArgs e);
        Dictionary<string, Argument> argumentDictionary = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteXamlWorkflowDesigner"/> class.
        /// </summary>

        public InvokeXAML()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(this.ExecuteXamlWorkflowDesigner_Loaded);
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
                argumentDictionary = new Dictionary<string, Argument>();
            }
            else
            {
                argumentDictionary = (Dictionary<string, Argument>)modelItem.GetCurrentValue();
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

        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                this.ModelItem.Properties["WorkflowPath"].SetValue(fileDialog.FileName);
                workflowPath = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Handles the Click event of the DefineArgsButton control to launch a DynamicArgumentDialog instance for argument editing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DefineArgsButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
            {
                Title = Bot.Activity.ActivityLibrary.Properties.Resources.DynamicArgumentDialogTitle
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
            workflowPath = this.ModelItem.Properties["WorkflowPath"].Value.GetCurrentValue() as string;
            if (!workflowPath.Contains(":"))
            {
                workflowPath = SelectHelper.ProjectLocation + Path.DirectorySeparatorChar + workflowPath;
            }

            if ((!string.IsNullOrEmpty(workflowPath)) && workflowPath.Trim().Length > 0)
            {
                argumentDictionary = new Dictionary<string, Argument>();

                //this.currentlyLoadedWorkflowPath = workflowPath;

                try
                {
                    DynamicActivity dynamicActivity = DynamicActivityStore.GetActivity(workflowPath, 1); //Designer mode
                    if (dynamicActivity != null)
                    {
                        foreach (DynamicActivityProperty property in dynamicActivity.Properties)
                        {
                            Argument newArgument = null;
                            if (property.Type.GetGenericTypeDefinition().BaseType == typeof(InArgument))
                            {
                                newArgument = Argument.Create(property.Type.GetGenericArguments()[0], ArgumentDirection.In);
                            }

                            if (property.Type.GetGenericTypeDefinition().BaseType == typeof(OutArgument))
                            {
                                newArgument = Argument.Create(property.Type.GetGenericArguments()[0], ArgumentDirection.Out);
                            }

                            if (property.Type.GetGenericTypeDefinition().BaseType == typeof(InOutArgument))
                            {
                                newArgument = Argument.Create(property.Type.GetGenericArguments()[0], ArgumentDirection.InOut);
                            }

                            if (newArgument != null)
                            {
                                argumentDictionary.Add(property.Name, newArgument);
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

        private void OpenWorkflow(object sender, OpenXamlFileEventArgs e)
        {
            //Code for Opening Workflow
            try
            {
                MainWindow mw = new MainWindow();
                //mw.OpenProject(sender, e);
                mw.OpenFile(e.XamlFileNameWithPath);
                NewProjectEventArgs ne = new NewProjectEventArgs();
                ne.Path = workflowPath;
                //BOTDesigner.Views.DashBoard.

                ne.projectType = ProjectType.openproject;
                //OpenProject(null, ne);
                //OpenFile(e.XamlFileNameWithPath);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception while Opening Workflow: " + ex.Message, Logger.LogLevel.Error);
            }
        }
        public class NewProjectEventArgs : EventArgs
        {
            public string Path { get; set; }
            public ProjectType projectType;
            public string projectFullName;
        }
        public enum ProjectType
        {
            sequence = 1,
            workflow = 2,
            statemachine = 3,
            openproject = 4,
            openexplorer = 5

        }
    }
}
