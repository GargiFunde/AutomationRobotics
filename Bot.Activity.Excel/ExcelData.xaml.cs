using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Bot.Activity.Excel
{
    /// <summary>
    /// Interaction logic for ExcelData.xaml
    /// </summary>
    public partial class ExcelData : Window
    {
        public ExcelData()
        {
            InitializeComponent();
        }

        private DataTable _dataTable;

        public DataTable GridDataTable
        {
            get { return _dataTable; }
            set { _dataTable = value; }
        }

        public void SetData(DataTable dt)
        {
            GridDataTable = dt;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExcelDataGrid.DataContext = GridDataTable.DefaultView;
        }
    }
}
