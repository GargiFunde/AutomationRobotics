// <copyright file=ProcessConnection company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace AutomationAgent
{
    class ProcessConnection
    {
        public static ConnectionOptions ProcessConnectionOptions()
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Default;
            options.EnablePrivileges = true;
            return options;
        }

        public static ManagementScope ConnectionScope(string machineName,
                                                      ConnectionOptions options)
        {
            ManagementScope connectScope = new ManagementScope();
            connectScope.Path = new ManagementPath(@"\\" + machineName + @"\root\CIMV2");
            connectScope.Options = options;

            try
            {
                connectScope.Connect();
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message.ToString());
            }
            return connectScope;
        }
    }
}
