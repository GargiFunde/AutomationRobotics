// <copyright file=ProcessLocal company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace AutomationAgent
{
    class ProcessLocal : IProcessObject
    {
        #region "fields"
        ConnectionOptions options;
        ManagementScope connectionScope;
        #endregion

        #region "constructors"
        public ProcessLocal()
        {
            options = ProcessConnection.ProcessConnectionOptions();
            connectionScope = ProcessConnection.ConnectionScope(
                                     Environment.MachineName, options);
        }
        #endregion

        #region "polymorphic methods"
        public ArrayList RunningProcesses()
        {
            ArrayList alProcesses = new ArrayList();
            alProcesses = ProcessMethod.RunningProcesses(connectionScope);
            return alProcesses;
        }
        public ArrayList ProcessProperties(string processName)
        {
            ArrayList alProperties = new ArrayList();
            alProperties = ProcessMethod.ProcessProperties(connectionScope,
                                                           processName);
            return alProperties;
        }
        public string CreateProcess(string processPath)
        {
            return ProcessMethod.StartProcess(Environment.MachineName, processPath);
        }
        public void TerminateProcess(string processName)
        {
            ProcessMethod.KillProcess(connectionScope, processName);
        }
        public void SetPriority(string processName, ProcessPriority.priority priority)
        {
            ProcessMethod.ChangePriority(connectionScope, processName, priority);
        }
        public string GetProcessOwner(string processName)
        {
            return ProcessMethod.ProcessOwner(connectionScope, processName);
        }
        public string GetProcessOwnerSID(string processName)
        {
            return ProcessMethod.ProcessOwnerSID(connectionScope, processName);
        }
        #endregion
    }
}
