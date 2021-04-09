using Microsoft.CSharp.Activities;
using BOTDesigner.CSharpExpressionEditor;
using BOTDesigner.VbExpressionEditor;
using System;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using CommonLibrary;
using System.Collections;
using System.Windows;

namespace BOTDesigner.Helpers
{
    /// <summary>
    /// Workflow Designer Wrapper
    /// </summary>
    static class CustomWfDesigner
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
            _wfDesigner = new WorkflowDesigner();
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().AutoConnectEnabled = true;
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
        /// Creates a new Workflow Designer instance (VB) with Intellisense
        /// </summary>
        /// <param name="sourceFile">Workflow FileName</param>
        public static void NewInstanceVB(string sourceFile = _defaultWorkflow)
        {
            _expressionEditorServiceVB = new VbExpressionEditorService();
            
            _wfDesigner = new WorkflowDesigner();

            Hashtable hashTable = new Hashtable
{
    { WorkflowDesignerColors.DesignerViewShellBarControlBackgroundColorKey, SystemColors.HighlightBrush},
     { WorkflowDesignerColors.DesignerViewShellBarColorGradientBeginKey, SystemColors.HighlightBrush},
      { WorkflowDesignerColors.DesignerViewShellBarColorGradientEndKey, SystemColors.HighlightBrush },
};

            _wfDesigner.PropertyInspectorFontAndColorData = System.Xaml.XamlServices.Save(hashTable);

            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().AutoConnectEnabled = true;
            _wfDesigner.Context.Services.GetService<DesignerConfigurationService>().RubberBandSelectionEnabled = true;
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

            WorkflowEntity workflowEntity = new WorkflowEntity();
            if (!SelectHelper.WorkflowEntityDictionary.ContainsKey(sourceFile))
            {
               
                SelectHelper.WorkflowEntityDictionary.Add(sourceFile, workflowEntity);
            }
            else
            {
                SelectHelper.WorkflowEntityDictionary[sourceFile] = workflowEntity;
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
