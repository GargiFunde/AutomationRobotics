// <copyright file=IProcessObject company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAgent
{
    interface IProcessObject
    {
        ArrayList RunningProcesses();
        ArrayList ProcessProperties(string processName);
        string CreateProcess(string processPath);
        void TerminateProcess(string processName);
        void SetPriority(string processName, ProcessPriority.priority priority);
        string GetProcessOwner(string processName);
        string GetProcessOwnerSID(string processName);
    }
}
