// <copyright file=RobotControl.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
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
using RobotLibrary;

namespace RobotLocalController
{
    /// <summary>
    /// Interaction logic for RobotControl.xaml
    /// </summary>
    public partial class RobotControl : UserControl 
    {
        //public InterProcessCommunicator interprocessCommunicator { get; set; }
        AgentOperations agentOperations = null;
        public RobotControl()
        {
            InitializeComponent();
            agentOperations = new AgentOperations(); 
        }
       
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            string RobotId = lblRobotIdValue.Content.ToString();
            RobotDetails robotDetails= (RobotDetails)this.DataContext;
            agentOperations.StartRobot(robotDetails);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            string RobotId = lblRobotIdValue.Content.ToString();
            RobotDetails robotDetails = (RobotDetails)this.DataContext;
            agentOperations.StopRobot(robotDetails);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string RobotId = lblRobotIdValue.Content.ToString();
            RobotDetails robotDetails = (RobotDetails)this.DataContext;
            robotDetails.DeleteMe();
        }
 
    }
}
