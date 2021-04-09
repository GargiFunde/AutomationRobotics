// <copyright file=CustomWfDesigner company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using CommonLibrary;
using CommonLibrary.CSharpExpressionEditor;
using Microsoft.CSharp.Activities;
//using RehostedWorkflowDesigner.VbExpressionEditor;
using System;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.View;
using System.Activities.Statements;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// Workflow Designer Wrapper
    /// </summary>
    public static class CustomWfDesigner
    {
        private static WorkflowDesigner _wfDesigner;
      //  private const String _defaultWorkflow = "defaultWorkflow.xaml";
      //  private const String _defaultWorkflowCSharp = "defaultWorkflowCSharp.xaml";
        private static RoslynExpressionEditorService _expressionEditorService;
       // private static VbExpressionEditorService _expressionEditorServiceVB;

        /// <summary>
        /// Gets the current WorkflowDesigner Instance
        /// </summary>
        public static WorkflowDesigner Instance 
        {
            get
            {
                //if (_wfDesigner == null)
                //    NewInstance(_defaultWorkflow);
                return _wfDesigner;
            }
            set
            {
                _wfDesigner = value;
            }
        }

        /// <summary>
        /// Creates a new Workflow Designer instance (VB)
        /// </summary>
        /// <param name="sourceFile">Workflow FileName</param>
        public static void NewInstance(string sourceFile)
        {

            //_expressionEditorService = new RoslynExpressionEditorService();
            //ExpressionTextBox.RegisterExpressionActivityEditor(new CSharpValue<string>().Language, typeof(RoslynExpressionEditor), CSharpExpressionHelper.CreateExpressionFromString);

            //_wfDesigner = new WorkflowDesigner();
            //_wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            //_wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            //_wfDesigner.Context.Services.Publish<IExpressionEditorService>(_expressionEditorService);
            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;

            //associates all of the basic activities with their designers
           

            new DesignerMetadata().Register();

            //load Workflow Xaml
            _wfDesigner.Load(sourceFile);

            SelectHelper._wfDesigner = _wfDesigner;
            if (!SelectHelper.WorkflowDictionary.ContainsKey(sourceFile))
            {
                SelectHelper.WorkflowDictionary.Add(sourceFile, _wfDesigner);
            }
            else
            {
                SelectHelper.WorkflowDictionary[sourceFile] = _wfDesigner;
            }
        }
      

    }
}
