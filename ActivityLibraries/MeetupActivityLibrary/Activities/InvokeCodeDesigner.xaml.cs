using Bot.Activity.ActivityLibrary.Activities;
using BotDesignCommon;
using CommonLibrary;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bot.Activity.ActivityLibrary
{
    // Interaction logic for InvokeCode.xaml
    public partial class InvokeCodeDesigner
    {
        static InvokeCodeDesigner currinst ;
        public static string CodeText;
        Dictionary<string, Argument> argumentDictionary = null;
   
        public InvokeCodeDesigner()
        {
            currinst = this;
            InitializeComponent();
              this.Loaded += new RoutedEventHandler(this.ExecuteXamlWorkflowDesigner_Loaded);
      
        }

        public static InvokeCodeDesigner getcurrinst()
        {

            return currinst;

        }

        private void EditCode_Click(object sender, RoutedEventArgs e)
        {
            //CodeEditor editor = new CodeEditor();
            //editor.Show();
            //VbExpressionEditorInstance editor = new VbExpressionEditorInstance();
            //editor. 

            // var editor = new VbExpressionEditorInstance();
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
            //Test.Main form = new Test.Main();
            //form.Show();
           //});
            

        }
        private void EditArgsButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
            {
                Title = Bot.Activity.ActivityLibrary.Properties.Resources.DynamicArgumentDialogTitle
            };

         
            ModelItem modelItem = this.ModelItem.Properties["ArgumentsPassed"].Dictionary;
            using (ModelEditingScope change = modelItem.BeginEdit("ArgumentEditing"))
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

        public void ExecuteXamlWorkflowDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            this.ModelItem.PropertyChanged += new PropertyChangedEventHandler(this.ModelItem_PropertyChanged);

            ModelItem modelItem = this.ModelItem.Properties["ArgumentsPassed"].Dictionary;
            if (modelItem == null)
            {
                argumentDictionary = new Dictionary<string, Argument>();

            }
            else
            {
                argumentDictionary = (Dictionary<string, Argument>)modelItem.GetCurrentValue();
            }
            this.ModelItem.Properties["ArgumentsPassed"].SetValue(argumentDictionary);
        }

        private void ModelItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("WorkflowPath"))
            //{
            //    this.InitImportDynamicArgumentDialog();
            //}
            // argumentDictionary = null;
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            TextRange textRange = new TextRange(
        // TextPointer to the start of content in the RichTextBox.
        RichTxtBox.Document.ContentStart,
        // TextPointer to the end of content in the RichTextBox.
        RichTxtBox.Document.ContentEnd
    );
            CodeText = textRange.Text;

            RichTxtBox.Document.Blocks.Clear();
            RichTxtBox.Document.Blocks.Add(new Paragraph(new Run(CodeText)));
            this.ModelItem.Properties["Code"].SetValue(new InArgument<String>(CodeText));
        }

        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ModelItem.Properties["Code"].Value != null)
            {
                RichTxtBox.Document.Blocks.Clear();
                string txtcontent = this.ModelItem.Properties["Code"].Value.Content.ComputedValue.ToString();
                CodeText = txtcontent;
                RichTxtBox.Document.Blocks.Add(new Paragraph(new Run(txtcontent)));
            }


            //if (this.ModelItem.Properties["ArgumentsPassed"].Dictionary == null)
            //{
            //    this.ModelItem.Properties["ArgumentsPassed"].SetValue(new Dictionary<string, Argument>()); 
            //}
        }

        public static string GetCode()
        {
            
            return CodeText;
        }

        public static string SetCode(string newcode)
        {
            CodeText = newcode;
            return null;
        }

        private void RichTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
          //string updatedcode   = BotDesignCommon.Helpers.ActivityHelper.InvokeCodeUpdate;
          //  RichTxtBox.Document.Blocks.Clear();
          //  RichTxtBox.Document.Blocks.Add(new Paragraph(new Run(updatedcode)));
          //  this.ModelItem.Properties["Code"].SetValue(new InArgument<String>(updatedcode));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (BotDesignCommon.Helpers.ActivityHelper.InvokeCodeUpdate!=null && BotDesignCommon.Helpers.ActivityHelper.InvokeCodeUpdate != "")
            //{
            //    string updatedcode = BotDesignCommon.Helpers.ActivityHelper.InvokeCodeUpdate;
            //    BotDesignCommon.Helpers.ActivityHelper.InvokeCodeUpdate = string.Empty;
            //    RichTxtBox.Document.Blocks.Clear();
            //    RichTxtBox.Document.Blocks.Add(new Paragraph(new Run(updatedcode)));
            //    this.ModelItem.Properties["Code"].SetValue(new InArgument<String>(updatedcode)); 
            //}
        }
    }
}
