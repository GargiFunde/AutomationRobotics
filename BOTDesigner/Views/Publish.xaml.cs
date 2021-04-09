// <copyright file=Publish.xaml company=E2E BOTS>
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
    public partial class Publish : Window
    {
        public Publish(string projectName)
        {
            InitializeComponent();
            lblPublishAutomation.Content = projectName;
        }

        public string ProjectName { get; set; }
       
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {             
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

       
    }
}
