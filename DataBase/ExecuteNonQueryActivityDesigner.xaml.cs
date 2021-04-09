using Microsoft.Data.ConnectionUI;
using System;
using System.Collections.Generic;
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

namespace Database
{
    // Interaction logic for ExecuteNonQueryActivityDesigner.xaml
    public partial class ExecuteNonQueryActivityDesigner
    {
        public ExecuteNonQueryActivityDesigner()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string ConnString = string.Empty;
            bool isSqlServer = false;
            ExecuteNonQuery db = null;
            db = (ExecuteNonQuery)this.ModelItem.GetCurrentValue();
            try
            {

                DataConnectionDialog dlg = new DataConnectionDialog();
                dlg.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.AccessDataSource);
                dlg.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.OdbcDataSource);
                dlg.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.OracleDataSource);
                dlg.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.SqlDataSource);
                dlg.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.SqlFileDataSource);


                dlg.SelectedDataSource = Microsoft.Data.ConnectionUI.DataSource.SqlDataSource;
                dlg.SelectedDataProvider = Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider;
                dlg.Text = "Configure Dataconnection";

                // DataConnectionDialog.Show(dlg);
                // Microsoft.Data.ConnectionUI.DataSource
                if (Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dlg) == System.Windows.Forms.DialogResult.OK)
                {

                    //Connection string "Data Source=LENOVO-PC\\MYSQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True"
                    ConnString = dlg.ConnectionString;
                    isSqlServer = dlg.SelectedDataSource == Microsoft.Data.ConnectionUI.DataSource.SqlDataSource
                   || dlg.SelectedDataSource == Microsoft.Data.ConnectionUI.DataSource.SqlFileDataSource;
                    db.ConnectionString = ConnString;
                  //  db.issqlserver = isSqlServer;
                }


            }
            catch (Exception ex)
            {

            }

        }
    }
}
