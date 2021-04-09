// <copyright file=CustomCommands company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonLibrary.Helpers
{
    public static class CustomCommands
    {
        public static ICommand CmdWfNewCSharp = new RoutedCommand("CmdWfNewCSharp", typeof(CustomCommands));
        public static ICommand CmdWfNewVB = new RoutedCommand("CmdWfNewVB", typeof(CustomCommands));
        public static ICommand CmdWfNew = new RoutedCommand("CmdWfNew", typeof(CustomCommands));
        public static ICommand CmdWfRun = new RoutedCommand("CmdWfRun", typeof(CustomCommands));
        public static ICommand CmdWfStop = new RoutedCommand("CmdWfStop", typeof(CustomCommands));
        public static ICommand CmdWfDebug = new RoutedCommand("CmdWfDebug", typeof(CustomCommands));
        public static ICommand CmdWfShowLog = new RoutedCommand("CmdWfShowLog", typeof(CustomCommands));
        public static ICommand CmdWfClearLog = new RoutedCommand("CmdWfClearLog", typeof(CustomCommands));
        public static ICommand CmdWfSaveAll = new RoutedCommand("CmdWfSaveAll", typeof(CustomCommands));
        public static ICommand CmdWfDev = new RoutedCommand("CmdWfDev", typeof(CustomCommands));
        public static ICommand CmdWfUAT = new RoutedCommand("CmdWfUAT", typeof(CustomCommands));
        public static ICommand CmdWfPreProd = new RoutedCommand("CmdWfPreProd", typeof(CustomCommands));
        public static ICommand CmdWfProd = new RoutedCommand("CmdWfProd", typeof(CustomCommands));

        public static ICommand CmdWfNewFlowChart = new RoutedCommand("CmdWfNewFlowChart", typeof(CustomCommands));
        public static ICommand CmdWfNewStateMachine = new RoutedCommand("CmdWfNewStateMachine", typeof(CustomCommands));

      
    }
}
