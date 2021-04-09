// <copyright file=SignIn.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

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
//using System.Windows.Shapes;
//using Entities;
//using ExecutorViewModel;
//using CommonLibrary;
//using CommonLibrary.Interfaces;

//namespace RobotConsole
//{
//    /// <summary>
//    /// Interaction logic for SignIn.xaml
//    /// </summary>
//    public partial class SignIn : Window
//    {
       
//        public List<ApplicationDetails> Applist;
//        public SignInViewModel VM = null;
//        public SignIn(IRobotExecutorFramework BaseRobotFramework)
//        {
//            InitializeComponent();

//            cmbGroupList.SelectedIndex = 0;
//            cmbLanguage.SelectedIndex = 0;
//            VM = new SignInViewModel(BaseRobotFramework);
//            SelectApplicationsForGroup();
//            this.DataContext = VM;
           
           
//        }
        
//        List<ApplicationDetails> Applisttmp = null;
//        int iCurrentGroupId = 0;
//        private void cmbGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            SelectApplicationsForGroup();
//        }

//        private void SelectApplicationsForGroup()
//        {
//            SignInMgeAppDetails signInMgeAppDetails = null;
//            if (cmbGroupList.SelectedItem == null)
//                return;
//            string strGroupName = ((GroupDetails)cmbGroupList.SelectedItem).GroupName;
//            iCurrentGroupId = ((GroupDetails)cmbGroupList.SelectedItem).GroupId;
//            signInMgeAppDetails = VM.SetGroupApplications(iCurrentGroupId, strGroupName);
//            Applist = signInMgeAppDetails.AppRequireSignIn;
//            Applisttmp = new List<ApplicationDetails>();
//            foreach (ApplicationDetails item in Applist)
//            {
//                ApplicationDetails ad = new ApplicationDetails(); //Only wrapper is created and excluding Button type inside objects are added so that they should not be visible in SignIn window
//                ad.ApplicationId = item.ApplicationId;
//                ad.ApplicationName = item.ApplicationName;
//                Applisttmp.Add(ad);
//                foreach (ApplicationSignInScrapingProperties itemSignInScraping in item.AppSignInScrapingProperties)
//                {
//                    if (itemSignInScraping.LoginFieldProperties.Type != "Button")
//                        ad.AppSignInScrapingProperties.Add(itemSignInScraping);
//                    //if (itemSignInScraping.LoginFieldProperties.Type == "DropDown")
//                    //    itemSignInScraping.PopulteLoginFields(itemSignInScraping.LoginFieldProperties);
//                }
//            }
//            lstSignInApplications.ItemsSource = Applisttmp;
//            lstNonSignInApplications.ItemsSource = signInMgeAppDetails.AppWithoutSignIn;
//        }
//        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
//        {
//            Log.Logger.LogData("Robot: Sign In Started", LogLevel.Debug);
//            VM.SaveSignInDetails(iCurrentGroupId,Applisttmp);
//            this.DialogResult = true;
//            this.Close();
//        }

//        private void buttonCancel_Click(object sender, RoutedEventArgs e)
//        {
//            this.DialogResult = false;
//            this.Close();
//        }
       

//    }
//}
