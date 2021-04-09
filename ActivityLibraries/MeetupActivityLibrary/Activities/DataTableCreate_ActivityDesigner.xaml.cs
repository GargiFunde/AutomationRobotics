
using Newtonsoft.Json;
using System;
using System.Activities;
using System.Data;
using System.Windows;

namespace Bot.Activity.ActivityLibrary.Activities
{
    // Interaction logic for DataTableCreate_ActivityDesigner.xaml
    public partial class DataTableCreate_ActivityDesigner
    {
        //  DataTableCreateWindow dataTableCreateWindow = null;
      
        public DataTableCreate_ActivityDesigner()
        {
           
            InitializeComponent();

            //DataTableCreateForm dataTableCreateForm = new DataTableCreateForm();
            

            //  this.Loaded += new RoutedEventHandler(this.ExecuteXamlWorkflowDesigner_Loaded);

            //if (dataTableCreateWindow == null)
            //{
            //    dataTableCreateWindow = new DataTableCreateWindow();
            //}
        }
        private void DataTableCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //GetTableDataStringFormat = ModelItem.Properties["TableInfo"].ComputedValue.ToString();
                //abc.ToString(); InArgument<string> abc;
                //GetTableDataStringFormat = ModelItem.Properties["TableInfo"].Value.GetCurrentValue().ToString();
                // DialogResult dialogResult = dataTableCreateWindow.ShowDialog();
                if (ModelItem.Properties["TableInfo"].Value != null)
                {
                    string dtstring = ModelItem.Properties["TableInfo"].Value.Content.ComputedValue.ToString();
                    DataTableCreateForm.GetCreatedDataTable(dtstring);
                }
                else
                {
                    
                    DataTableCreateForm.GetCreatedDataTable(null);
                }
             
                DataTableCreateForm dataTableCreateForm = new DataTableCreateForm();
                dataTableCreateForm.Show();
           
                //ModelItem.Properties["TableInfo"].SetValue(new InArgument<string>("Heloo123!!"));

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in DataTable Create:" + ex.Message, Logger.LogLevel.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DataTableCreateForm dataTableCreateForm = new DataTableCreateForm();
         string temp = dataTableCreateForm.fromDataCreatedFormString();

          
           
            //int Colcount = createdDataTable.Columns.Count;
            //SetTableDataStringFormat += "&&=";
            //foreach (DataColumn column in createdDataTable.Columns)
            //{
            //    SetTableDataStringFormat += column.ColumnName.ToString() + "|";
            //}
            //SetTableDataStringFormat += "=+";
            //foreach (DataRow row in createdDataTable.Rows)
            //{

            //    for (int i=0; i < Colcount;i++)
            //    {
            //        SetTableDataStringFormat += row[i].ToString() + " ";
            //    }

            //    SetTableDataStringFormat += "##";
            //}
            //SetTableDataStringFormat += "+&&";

            ModelItem.Properties["TableInfo"].SetValue(new InArgument<string>(temp));
        }

       


    }
}
