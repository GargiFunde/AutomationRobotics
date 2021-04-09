// <copyright file=UserControlSignInField.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

//using Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace RobotConsole
//{
//    /// <summary>
//    /// Interaction logic for UserControlSignInField.xaml
//    /// </summary>
//    public partial class UserControlSignInField : UserControl
//    {
//        public UserControlSignInField()
//        {
//            InitializeComponent();
//        }

//        private void ControlLoaded(object sender, EventArgs e)
//        {
//            if (txtType.Text == "Password")
//            {
//                this.txtPassword.Visibility  = System.Windows.Visibility.Visible;
//              //  txtPassword.Password = txtUser.Text;
//                Entities.ApplicationSignInScrapingProperties sp = (Entities.ApplicationSignInScrapingProperties)this.DataContext;
//                txtPassword.Password = sp.FieldRunTimeValue;
//                this.txtUser.Visibility = System.Windows.Visibility.Hidden;
//                this.cmbDropDown.Visibility = System.Windows.Visibility.Hidden;
//            }
//            else if (txtType.Text == "Text")
//            {
//                this.txtPassword.Visibility = System.Windows.Visibility.Hidden;
//                this.txtUser.Visibility = System.Windows.Visibility.Visible;
//                this.cmbDropDown.Visibility = System.Windows.Visibility.Hidden;
//            }
//            else if (txtType.Text == "DropDown")
//            {
//                this.txtPassword.Visibility = System.Windows.Visibility.Hidden;
//                this.txtUser.Visibility = System.Windows.Visibility.Hidden;
//                this.cmbDropDown.Visibility = System.Windows.Visibility.Visible;
//            }
//        }

//        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
//        {
//           // txtUser.Visibility = System.Windows.Visibility.Visible;
//           // txtUser.Text = txtPassword.Password;
//            Entities.ApplicationSignInScrapingProperties sp =(Entities.ApplicationSignInScrapingProperties)this.DataContext;
//            sp.FieldRunTimeValue = txtPassword.Password;
//          //  txtUser.Visibility = System.Windows.Visibility.Hidden;
//        }

//        private void cmbDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            Entities.ApplicationSignInScrapingProperties sp = (Entities.ApplicationSignInScrapingProperties)this.DataContext;
//            if (cmbDropDown.SelectedValue != null)
//            {
//                sp.FieldRunTimeValue = cmbDropDown.SelectedValue.ToString();
//            }
//        }
//    }
//}
