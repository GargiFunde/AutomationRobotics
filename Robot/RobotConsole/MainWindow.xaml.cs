// <copyright file=MainWindow.xaml company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RobotAutomationFramework;
using RobotLibrary.Interfaces;
using System.Threading;
using RobotLibrary;
using System.ServiceModel;
using Newtonsoft.Json;
namespace RobotConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window//, IMessagingComponent
    {
        IntPtr AgentHandle = IntPtr.Zero;
        BaseRobotFramework roboFramework = null;
        bool IsRobotReady = false;
        
        public MainWindow()
        {
            InitializeComponent();
           // log4net.Config.XmlConfigurator.Configure();
            //   Log log = new Log();
            roboFramework = new BaseRobotFramework();      
        }
        
      
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            roboFramework.ShutDownCurrentApplications();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
