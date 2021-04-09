using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Data;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for AddRemoveActivities.xaml
    /// </summary>
    public partial class AddRemoveActivities : Window
    {
        DataTable DataTableActivities = null;
        public DataTable AddRemoveActivites { get; set; }
        public AddRemoveActivities()
        {
            InitializeComponent();
            try
            { 
            DataTableActivities = new DataTable();
            DataTableActivities.TableName = "DataTableActivities";
            DataTableActivities.Columns.Add("Select", typeof(System.Boolean));
            DataTableActivities.Columns.Add("DllName", typeof(System.String));
            DataTableActivities.Columns.Add("Delete", typeof(System.String));
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("LoadToolBox.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if ((!string.IsNullOrEmpty(line)) && (line.Trim().Length != 0))
                    {
                        DataRow dataRow =  DataTableActivities.NewRow();
                        line = line.Trim();
                        string lineComment = line.Substring(0, 2);
                        if (lineComment.Contains("//"))
                        {
                            dataRow[0] = false;
                            dataRow[1] = line.Remove(0,2);
                            DataTableActivities.Rows.Add(dataRow);
                            continue; //it will not execute above lines if put outside
                        }
                        else
                        {
                            dataRow[0] = true;
                            //AppDomain.CurrentDomain.Load(line);
                            dataRow[1] = line;
                            DataTableActivities.Rows.Add(dataRow);
                        }
                       
                    }
                }
            }
            dgActivities.DataContext = DataTableActivities;
            dgActivities.ItemsSource = DataTableActivities.DefaultView;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }
        }
        private void btnDirectoryExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogOpen = new OpenFileDialog();
                dialogOpen.Title = "Open Workflow";
                dialogOpen.Filter = "Activities|*.dll|Activities (.dll)|*.dll";

                if (dialogOpen.ShowDialog() == true)
                {
                    string destFile = System.IO.Path.GetFileName(dialogOpen.FileName);
                    destFile = destFile.Replace(".dll","");
                    destFile = destFile.Replace(".Dll", "");
                    destFile = destFile.Replace(".DLL", "");

                    string compair = Environment.CurrentDirectory + Path.DirectorySeparatorChar + destFile;
                    if (dialogOpen.FileName != compair)
                    {
                        File.Copy(dialogOpen.FileName, destFile);
                    }
                    DataRow dataRow = DataTableActivities.NewRow();
                    dataRow[0] = true;
                    dataRow[1] = destFile;
                    DataTableActivities.Rows.Add(dataRow);
                    dgActivities.DataContext = DataTableActivities;
                    dgActivities.ItemsSource = DataTableActivities.DefaultView;
                }
            }catch(Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }
        }
        
        private void OnDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.Source is System.Windows.Controls.Button)
                {
                    System.Windows.Controls.Button b = (System.Windows.Controls.Button) e.Source;
                    string dllName = b.Tag.ToString();

                    foreach (DataRow row in DataTableActivities.Rows)
                    {
                        if (row[1].ToString() == dllName)
                        {
                            DataTableActivities.Rows.Remove(row);
                            break;
                        }
                    }
                    dgActivities.DataContext = DataTableActivities;
                    dgActivities.ItemsSource = DataTableActivities.DefaultView;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter sw = File.CreateText("LoadToolBox.txt"))
                {
                    foreach (DataRow row in DataTableActivities.Rows)
                    {
                        bool selected = (bool)row[0];
                        string dllName = row[1].ToString();
                        string fileEntry = string.Empty;
                        if (!selected)
                        {
                            fileEntry = "//";
                        }
                        fileEntry = fileEntry + dllName;
                        sw.WriteLine(fileEntry);
                    }
                }
                this.DialogResult = true;
                this.Close();
            }catch(Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }

        }

        private void DgActivities_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
