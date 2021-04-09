// <copyright file=Export.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using Logger;
using System;
using System.IO;
using System.Windows;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class Export : Window
    {
        public Export()
        {
            InitializeComponent();
                    
        }

        public string ProjectName { get; set; }
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
                if (!Directory.Exists(txtLocation.Text.Trim()))
                {
                    lblLocationError.Visibility = Visibility.Visible;
                    lblError.Visibility = Visibility.Visible;
                    lblError.Content = "Location folder is not valid";
                    return;
                }
                ProjectName = txtLocation.Text;
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
                using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        txtLocation.Text = folderDialog.SelectedPath;
                        // folderDialog.SelectedPath -- your result
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
    }
}
