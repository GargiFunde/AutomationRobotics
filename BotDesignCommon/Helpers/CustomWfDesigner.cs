// <copyright file=CustomWfDesigner company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:46</date>
// <summary></summary>

using CommonLibrary;
using BotDesignCommon;
using Microsoft.CSharp.Activities;
//using RehostedWorkflowDesigner.VbExpressionEditor;
using System;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Runtime.Versioning;

namespace BotDesignCommon.Helpers
{
    /// <summary>
    /// Workflow Designer Wrapper
    /// </summary>
   public static class CustomWfDesigner
    {
        private static WorkflowDesigner _wfDesigner;
        private const String _defaultWorkflow = "defaultWorkflow.xaml";
        private const String _defaultWorkflowCSharp = "Sequence.xaml";
        private static RoslynExpressionEditorService _expressionEditorService;
        private static VbExpressionEditorService _expressionEditorServiceVB;


        //static CustomWfDesigner()
        //{
        //    _expressionEditorService = new RoslynExpressionEditorService();
        //    ExpressionTextBox.RegisterExpressionActivityEditor(new CSharpValue<string>().Language, typeof(RoslynExpressionEditor), CSharpExpressionHelper.CreateExpressionFromString);

        //}
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
        public static void NewInstance(string sourceFile = _defaultWorkflow)
        {
           // _expressionEditorServiceVB = new VbExpressionEditorService();
            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().AutoConnectEnabled = true;
          //  _wfDesigner.Context.Services.Publish<IExpressionEditorService>(_expressionEditorServiceVB);
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
            if (!SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(sourceFile))
            {
                SelectHelper.RuntimeApplicationHelperDictionary.Add(sourceFile, new RuntimeApplicationHelper());
            }
        }

        /// <summary>
        /// Creates a new Workflow Designer instance (VB) with Intellisense
        /// </summary>
        /// <param name="sourceFile">Workflow FileName</param>
        public static void NewInstanceVB(string sourceFile = _defaultWorkflow)
        {
            _expressionEditorServiceVB = new VbExpressionEditorService();

            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            _wfDesigner.Context.Services.Publish<IExpressionEditorService>(_expressionEditorServiceVB);

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
            if (!SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(sourceFile))
            {
                SelectHelper.RuntimeApplicationHelperDictionary.Add(sourceFile, new RuntimeApplicationHelper());
            }
        }

        /// <summary>
        /// Creates a new Workflow Designer instance with C# Expression Editor
        /// </summary>
        /// <param name="sourceFile">Workflow FileName</param>
        public static void NewInstanceCSharp(string sourceFile = _defaultWorkflowCSharp)
        {
            _expressionEditorService = new RoslynExpressionEditorService();
            ExpressionTextBox.RegisterExpressionActivityEditor(new CSharpValue<string>().Language, typeof(RoslynExpressionEditor), CSharpExpressionHelper.CreateExpressionFromString);

            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            _wfDesigner.Context.Services.Publish<IExpressionEditorService>(_expressionEditorService);

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
            if (!SelectHelper.RuntimeApplicationHelperDictionary.ContainsKey(sourceFile))
            {
                SelectHelper.RuntimeApplicationHelperDictionary.Add(sourceFile, new RuntimeApplicationHelper());
            }
        }
    }
}
