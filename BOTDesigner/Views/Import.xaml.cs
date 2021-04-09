using Logger;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class Import : Window
    {
        public Import()
        {
            InitializeComponent();
        }

        public string ImportFileName { get; set; }
        string projectLocation = string.Empty;
        public string Location
        {
            get
            {
                return projectLocation;
            }
            set
            {
                projectLocation = value;
                if (projectLocation != string.Empty)
                {
                    this.txtLocation.Text = projectLocation;
                }
           
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblProjectError.Visibility = Visibility.Hidden;
                lblLocationError.Visibility = Visibility.Hidden;
                lblError.Visibility = Visibility.Hidden;
                if ((txtLocation.Text.Trim().Length == 0) || ((txtLocation.Text.Trim().Length == 0)))
                {
                    if (txtLocation.Text.Trim().Length == 0)
                    {
                        lblProjectError.Visibility = Visibility.Visible;
                    }
                    if (txtLocation.Text.Trim().Length == 0)
                    {
                        lblLocationError.Visibility = Visibility.Visible;

                    }
                    return;
                }
                if (!File.Exists(txtLocation.Text.Trim()))
                {
                    lblLocationError.Visibility = Visibility.Visible;
                    lblError.Visibility = Visibility.Visible;
                    lblError.Content = "Selected file is not valid";
                    return;
                }
                ImportFileName = txtLocation.Text;
                Location = txtLocation.Text;
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnDirectoryExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogOpen = new OpenFileDialog();
                dialogOpen.Title = "Import Automation";
                dialogOpen.Filter = "Project|*.zip|Workflows (.zip)|*.zip";

                if (dialogOpen.ShowDialog() == true)
                {
                    txtLocation.Text = dialogOpen.FileName;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
    }
}
