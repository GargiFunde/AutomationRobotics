// <copyright file=NewProject.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using Logger;
using System;
using System.IO;
using System.Windows;

namespace Bot.Activity.UserInputs
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class UserIdPasswordParameter : Window
    {
        public UserInputEntity userInput { get; set; }
        public UserIdPasswordParameter(ref UserInputEntity userInputentity)
        {
            InitializeComponent();
            userInput = userInputentity;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Content = "";
                lblError.Visibility = Visibility.Hidden;
                if ((txtuserInput.Text.Trim().Length == 0)||(txtPassword.Password.Trim().Length == 0)||(txtParameter.Text.Trim().Length==0))
                {
                    lblError.Content = "Please enter input";
                    lblError.Visibility = Visibility.Visible;
                    return;
                }

                userInput.userid = txtuserInput.Text;
                userInput.password = txtPassword.Password;
                userInput.param1 = txtParameter.Text;
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
