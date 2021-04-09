// <copyright file=VbExpressionEditorService company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:55</date>
// <summary></summary>

using Microsoft.CodeAnalysis;
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CommonLibrary.VbExpressionEditor
{
    public class VbExpressionEditorService : IExpressionEditorService
    {
        public void CloseExpressionEditors()
        {

        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, System.Windows.Size initialSize)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType, System.Windows.Size initialSize)
        {
            VbExpressionEditorInstance instance = new VbExpressionEditorInstance();
            return instance;
        }
        public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
        {

        }
    }
}
