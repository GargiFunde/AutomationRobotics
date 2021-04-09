using CommonLibrary;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Bot.RabbitMQ
{
    // Interaction logic for ActivityDesigner1.xaml
    public partial class ActivityDesigner1
    {
        Dictionary<string,InArgument>argumentDictionary = null;
        public ActivityDesigner1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(this.ExecuteXamlWorkflowDesigner_Loaded);
        }

        private void ExecuteXamlWorkflowDesigner_Loaded(object sender, RoutedEventArgs e)
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

        private void ModelItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DynamicArgumentDesignerOptions options = new DynamicArgumentDesignerOptions()
            {
                Title = Bot.RabbitMQ.Properties.Resources.DynamicArgumentDialogTitle
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
    }
}
