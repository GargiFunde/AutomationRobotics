// <copyright file=NestedActivity1.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using CommonLibrary;
using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Workflow.Activities;
using System.Xaml;

namespace Core.ActivityLibrary.Activities
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class NestedActivity1
    {
        public NestedActivity1()
        {
            InitializeComponent();
           
        }
        //public void placeholder()
        //{
        //    ModelItem modelItem = this.ModelItem.Properties["Arguments"].Dictionary;
        //    using (ModelEditingScope change = modelItem.BeginEdit("ArgumentsEditing"))
        //    {
        //        if (DynamicArgumentDialog.ShowDialog(this.ModelItem, modelItem, Context, this.ModelItem.View, options))
        //        {
        //            change.Complete();
        //        }
        //        else
        //        {
        //            change.Revert();
        //        }
        //    }
        //}
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.ModelItem.GetCurrentValue() is NestedActivity)
            {

                //string s = @"Test.xaml";
                //if (File.Exists(s))
                //{
                //    File.Delete(s);
                //}
                Activity workflow1 = ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(txtActivityFullName.Text, new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));

                NestedActivity owa = (NestedActivity)this.ModelItem.GetCurrentValue();
                owa.NestedActivityCreated = workflow1;
                owa.ActivityPath = txtActivityFullName.Text;
               // SelectHelper._wfDesigner.Save(s);

                //SelectHelper._wfDesigner = new WorkflowDesigner();
                //SelectHelper._wfDesigner.Context.Services.GetService<DesignerConfigurationService>().TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(".NETFramework", new Version(4, 5));
                //SelectHelper._wfDesigner.Context.Services.GetService<DesignerConfigurationService>().LoadingFromUntrustedSourceEnabled = true;

                ////associates all of the basic activities with their designers
                //new DesignerMetadata().Register();

                //SelectHelper._wfDesigner.Load(s);

                //SelectHelper._wfDesigner.View.UpdateLayout();
                //System.Windows.Controls.Border WfDesignerBorder = (System.Windows.Controls.Border)SelectHelper.Border;
                //WfDesignerBorder.Child = SelectHelper._wfDesigner.View;
                //SelectHelper._wfPropertyBorder.Child = SelectHelper._wfDesigner.PropertyInspectorView;

                EditingContext ed = this.Context; // SelectHelper._wfDesigner.Context;
                ModelTreeManager mtm = new ModelTreeManager(ed);
                ModelItem modelItem = mtm.CreateModelItem(null, owa.NestedActivityCreated);
                //modelItem.Properties["Activities"].Collection.Add(activity);
                //modelItem.ac
                this.ModelItem = modelItem;
            }

        }
    }
}
