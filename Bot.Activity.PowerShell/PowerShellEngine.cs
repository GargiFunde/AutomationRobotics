// <copyright file=PowerShellEngine company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:46</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;


namespace Bot.Activity.PowerShell
{

    public class PowerShellEngine : IDisposable
    {
        private Dictionary<string, Runspace> _runspaceCache = new Dictionary<string, Runspace>();

        ~PowerShellEngine()
        {
            Clean();
        }

        public Collection<PSObject> ExecuteScriptFile(string scriptFilePath, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            return ExecuteScript(File.ReadAllText(scriptFilePath), arguments, machineAddress);
        }

        public Collection<PSObject> ExecuteScriptFile(string scriptFilePath, string scriptfileParameters, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            return ExecuteScript(File.ReadAllText(scriptFilePath), scriptfileParameters, arguments, machineAddress);


        }
        //scriptParameters = "test ~=~ 123 ~;~ test1 ~=~ 234"
        //in ps file variable name is $test and its value is 123
        //string scriptParameters,
        public Collection<PSObject> ExecuteScript(string script, string scriptfileParameters, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            Runspace runspace = GetOrCreateRunspace(machineAddress);
            using (System.Management.Automation.PowerShell ps = System.Management.Automation.PowerShell.Create())
            {
                ps.Runspace = runspace;
                ps.AddScript(script);
                if (arguments != null)
                {
                    foreach (var argument in arguments)
                    {
                        ps.AddArgument(argument);
                    }
                }

                if (!string.IsNullOrEmpty(scriptfileParameters) && scriptfileParameters.Length > 0)
                {
                    string[] list = scriptfileParameters.Split(new string[] { "~;~" }, StringSplitOptions.None);

                    foreach (string scriptParam in list)
                    {
                        string[] arglist = scriptParam.Split(new string[] { "~=~" }, StringSplitOptions.None);
                        if (arglist.Length == 2)
                            runspace.SessionStateProxy.SetVariable(arglist[0], arglist[1]);
                    }
                }
                return ps.Invoke();
            }
        }

        public Collection<PSObject> ExecuteScript(string script, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            Runspace runspace = GetOrCreateRunspace(machineAddress);
            using (System.Management.Automation.PowerShell ps = System.Management.Automation.PowerShell.Create())
            {
                ps.Runspace = runspace;
                ps.AddScript(script);
                if (arguments != null)
                {
                    foreach (var argument in arguments)
                    {
                        ps.AddArgument(argument);
                    }
                }
                return ps.Invoke();
            }
        }

        public Collection<PSObject> ExecuteScript(string script, PSCredential psc, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            Runspace runspace = GetOrCreateRunspace(machineAddress, psc);
            using (System.Management.Automation.PowerShell ps = System.Management.Automation.PowerShell.Create())
            {
                ps.Runspace = runspace;
                ps.AddScript(script);
                if (arguments != null)
                {
                    foreach (var argument in arguments)
                    {
                        ps.AddArgument(argument);
                    }
                }

                return ps.Invoke();
            }
        }
        public void Dispose()
        {
            Clean();
            GC.SuppressFinalize(this);
        }

        private Runspace GetOrCreateLocalRunspace()
        {
            if (!_runspaceCache.ContainsKey("localhost"))
            {
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                _runspaceCache.Add("localhost", runspace);
            }

            return _runspaceCache["localhost"];
        }

        private Runspace GetOrCreateRunspace(string machineAddress)
        {
            if (string.IsNullOrWhiteSpace(machineAddress))
            {
                return GetOrCreateLocalRunspace();
            }

            machineAddress = machineAddress.ToLowerInvariant();
            if (!_runspaceCache.ContainsKey(machineAddress))
            {
                WSManConnectionInfo connectionInfo = new WSManConnectionInfo();
                connectionInfo.ComputerName = machineAddress;
                Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);
                runspace.Open();
                _runspaceCache.Add(machineAddress, runspace);
            }
            return _runspaceCache[machineAddress];
        }

        private Runspace GetOrCreateRunspace(string machineAddress, PSCredential psc)
        {
            if (string.IsNullOrWhiteSpace(machineAddress))
            {
                return GetOrCreateLocalRunspace();
            }

            machineAddress = machineAddress.ToLowerInvariant();
            var remoteComputer = new Uri(String.Format("http://{0}:5985/wsman", machineAddress));
            string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

            if (!_runspaceCache.ContainsKey(machineAddress))
            {
                WSManConnectionInfo connectionInfo = new WSManConnectionInfo(remoteComputer, shellUri, psc);
                connectionInfo.ComputerName = machineAddress;
                Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);
                runspace.Open();
                _runspaceCache.Add(machineAddress, runspace);
            }

            return _runspaceCache[machineAddress];
        }

        private void Clean()
        {
            foreach (var runspaceEntry in _runspaceCache)
            {
                runspaceEntry.Value.Close();
            }
            _runspaceCache.Clear();
        }
    }
}
