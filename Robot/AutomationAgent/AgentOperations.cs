// <copyright file=AgentOperations company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using RobotLibrary;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TCP.Server;

namespace AutomationAgent
{
    public class AgentOperations //DefaultAgent
    {
        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformationW(IntPtr hServer,int SessionId,int WTSInfoClass, out IntPtr ppBuffer, out IntPtr pBytesReturned);

        internal static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        //the User Name is the info we want returned by the query.
        internal static int WTS_UserName = 5;

        public AgentOperations()
        {
        }

        string sourcePath;
        public string SourcePath
        {
            get
            {
                if (string.IsNullOrEmpty(sourcePath))
                {
                    sourcePath = System.Configuration.ConfigurationManager.AppSettings["RobotSourcePath"];
                    sourcePath = sourcePath.TrimEnd(new char[] { '/', '\\' });
                }
                return sourcePath;
            }
            set
            {
                sourcePath = value;
            }
        }
           
        public void StopForcefully(string[] list)
        {
            string action = list[0];
            string botname = list[1];
            string botid = list[2];
            string pwd = list[3];
            string tenantid = list[4];
            string applicationName = ConfigurationManager.AppSettings["ExePath"];
            string exename = Path.GetFileName(applicationName);
            if (exename.Substring(exename.Length - 4, 4).ToLower().Contains(".exe"))
            {
                exename = exename.Remove(exename.Length - 4, 4);
            }
            string dir = Path.GetDirectoryName(applicationName);
            Process[] processess = Process.GetProcesses();

            bool processexists = false;
            foreach (Process proc in processess)
            {
                if (proc.ProcessName.Contains(exename))
                {
                    string userName = GetProcessOwner(proc.Id);
                    if (userName.ToLower().Contains(Environment.MachineName.ToLower()))
                    {
                        if (!botid.ToLower().Contains(Environment.MachineName.ToLower()))
                        {
                            botid = Environment.MachineName + "\\" + botid;
                        }
                    }
                    if (userName.ToLower() == botid.ToLower())
                    {
                        //ProcessLocal processtoterminate = new ProcessLocal();
                        //processtoterminate.SetPriority(proc.ProcessName, ProcessPriority.priority.IDLE);
                        //processtoterminate.TerminateProcess(proc.ProcessName);
                        proc.Kill();
                    }
                }
            }
        }

        public bool StartRobot(string[] list)
        {
            try
            {

                //string applicationName = @"C:\Work\BOTDesignerMaster\Build\BackOfficeBot.exe";
                //ProcessExtensions.StartProcessAsCurrentUser(applicationName);


                string action = list[0];
                string botname = list[1];
                string botid = list[2];
                string pwd = list[3];
                string tenantid = list[4];
                //string applicationName = "\"E:\\Work\\BOTDesignerMaster\\Build\\BackOfficeBot.exe\"";
                //ApplicationLoader.PROCESS_INFORMATION procInfo;
                //ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);
                //if (receiver != null)
                //{
                //    bool exists = CheckRobotState.CheckIfRobotIsRunning(receiver.ProcessId, applicationName);
                //    if (exists)
                //    {
                //        throw new InvalidOperationException(string.Format("Robot instance is already running with processid : {0}.", receiver.ProcessId));
                //    }
                //}
                string applicationName = ConfigurationManager.AppSettings["ExePath"];
                string exename = Path.GetFileName(applicationName);
                if(exename.Substring(exename.Length - 4, 4).ToLower().Contains(".exe"))
                {
                    exename = exename.Remove(exename.Length-4, 4);
                }
                string dir = Path.GetDirectoryName(applicationName);
                Process[] processess = Process.GetProcesses();

                bool processexists = false;
                foreach (Process proc in processess)
                {
                    if(proc.ProcessName.Contains(exename))
                    {                        
                        string userName = GetProcessOwner(proc.Id);
                        if(userName.ToLower().Contains(Environment.MachineName.ToLower()))
                        {
                            botid = Environment.MachineName +"\\"+ botid;
                        }

                        if (userName.ToLower() == botid.ToLower())
                        {
                            ProcessLocal processtoterminate = new ProcessLocal();
                            processtoterminate.SetPriority(proc.ProcessName, ProcessPriority.priority.IDLE);
                            processtoterminate.TerminateProcess(proc.ProcessName);
                            Thread.Sleep(1000);
                          //  proc.Kill();
                            break;
                        }
                    }
                }



                ProcessExtensions.StartProcessAsCurrentUser(applicationName);
                //System.Security.SecureString s = new System.Security.SecureString();

                //for (int x = 0; x < pwd.Length; x++)
                //{
                //    s.AppendChar(pwd[x]);
                //}
                //if (!botid.ToLower().Contains(Environment.MachineName.ToLower()))
                //{
                //    botid = Environment.MachineName + "\\" + botid;
                //}
                //Process process = new Process();
                //process.StartInfo.UseShellExecute = false;
                ////Set the working directory if you don't execute something like calc or iisreset but your own exe in which you want to access some files etc..
                ////process.StartInfo.WorkingDirectory = dir;
                ////Full path(e.g.it can be @"C:\Windows\System32\iisreset.exe" OR you can use only file name if the path is included in Environment Variables..)
                //process.StartInfo.FileName = applicationName;
                //process.StartInfo.Domain = ".";
                //process.StartInfo.UserName = botid;
                //process.StartInfo.Password = s;

                ////if(interprocessCommunicator !=null && interprocessCommunicator.ProcessIdentifier !=null)
                ////    processStartInfo.Arguments += ((IntPtr)interprocessCommunicator.ProcessIdentifier).ToString();


                //process.Start();

                //process.WaitForInputIdle();
                //while ((process.Id == 0))
                //{
                //    Thread.Sleep(500);
                //    process.Refresh();
                //}


            }
            catch (Exception ex)
            {

            }
            //robotDetails.ProcessId = process.Id;
            //robotDetails.MainWindowHandle = process.MainWindowHandle;
            //robotDetails.Message = "LaunchCompleted!";
            //robotDetails.IsLaunched = true;
            return true;
        }
       
        
        public static void KillProcess(ManagementScope connectionScope, string processName)
        {
            SelectQuery msQuery = new SelectQuery("SELECT * FROM Win32_Process Where Name = '" + processName + "'");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);
            foreach (ManagementObject item in searchProcedure.Get())
            {
                try
                {
                    item.InvokeMethod("Terminate", null);
                }
                catch (SystemException e)
                {
                    Console.WriteLine("An Error Occurred: " + e.Message.ToString());
                }
            }
        }
       


        public string GetProcessOwner(int processId)
        {
            string MethodResult = null;
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT ");
                sb.Append("     * ");
                sb.Append(" FROM ");
                sb.Append("     WIN32_PROCESS");
                sb.Append(" WHERE ");
                sb.Append("     ProcessId = " + processId);

                string Query = sb.ToString();

                ManagementObjectCollection Processes = new ManagementObjectSearcher(Query).Get();

                foreach (ManagementObject Process in Processes)
                {
                    string[] Args = new string[] { "", "" };

                    int ReturnCode = Convert.ToInt32(Process.InvokeMethod("GetOwner", Args));

                    switch (ReturnCode)
                    {
                        case 0:
                            MethodResult = Args[1] + "\\" + Args[0];
                            break;

                        default:
                            MethodResult = "None";
                            break;

                    }

                }

            }
            catch //(Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }
        public bool StopRobot(RobotDetails robotDetails, bool updateConfigMode = false)
        {
            // RobotLibrary.InterProcessCommands interprocCmd = new InterProcessCommands();
            //// interprocCmd.CommandWaitTime = 6000;
            // interprocCmd.InterProcessCommandType = InterProcessCmdType.Stop;
            // interprocessCommunicator.SendMessage(robotDetails.MainWindowHandle, JsonConvert.SerializeObject(interprocCmd));
            return true;
        }
        
        public void Upgrade(RobotDetails robotDetails)
        {
            //Task closeRobotTask = new Task(() =>
            //    {
            //        InterProcessCommands command = new InterProcessCommands();
            //        if (CheckRobotState.CheckIfRobotIsRunning(robotDetails.ProcessId, robotDetails.RobotPath))
            //        {
            //            ShutDown(robotDetails);
            //            Thread.Sleep((command.CommandWaitTime == 0) ? 50000 : (command.CommandWaitTime + 10000));
            //        }
            //        if (CheckRobotState.CheckIfRobotIsRunning(robotDetails.ProcessId, robotDetails.RobotPath))
            //        {
            //            throw new Exception("Unable to shutdown robot. Please shutdown manually and try again.");
            //        }
            //    });

            //closeRobotTask.ContinueWith(a =>
            //    {
            //        robotDetails.Message = string.Empty;
            //        string destinationPath = robotDetails.RobotPath;
            //        destinationPath = destinationPath.TrimStart(new char[] { '/', '\\' });
            //    });
            //XDocument sourceFileVersion = null;
            //switch(new Uri(sourcePath).Scheme)
            //{
            //    case "file":
            //        if(File.Exists(string.Format("{0}/{1}",SourcePath ,RobotVersionFile)))
            //            throw new InvalidOperationException(string.Format("Could not find {0} at path : {1}.", RobotVersionFile,SourcePath));
            //        break ;
            //    case "http":
            //        using(WebClient client = new WebClient())
            //        {
            //            string versionList = client.DownloadString(string.Format("{0}/{1}",SourcePath ,RobotVersionFile));
            //            sourceFileVersion = XDocument.Parse(versionList);
            //        }
            //        break;
            //}
            //List<Tuple<string, string, string, string>> sourceFileDetails = new System.Collections.Generic.List<Tuple<string, string, string, string>>();
            //foreach (var x in sourceFileVersion.Root.Elements())
            //{
            //    sourceFileDetails.Add(new Tuple<string, string, string, string>(x.Attribute("FileName").Value.ToString(), x.Attribute("FileVersion").Value.ToString(), x.Attribute("FileRelativePath").Value.ToString(), x.Attribute("TimeStamp").Value.ToString()));
            //}

            //List<Tuple<string, string, string, string>> localFileDetails = new System.Collections.Generic.List<Tuple<string, string, string, string>>();
            //if (File.Exists(string.Format("{0}/{1}", DestinationPath, RobotVersionFile)))
            //{
            //    XDocument localFileVersion = XDocument.Load(string.Format("{0}/{1}", DestinationPath, RobotVersionFile));
            //    foreach (var x in localFileVersion.Root.Elements())
            //    {
            //        localFileDetails.Add(new Tuple<string, string, string, string>(x.Attribute("FileName").Value.ToString(), x.Attribute("FileVersion").Value.ToString(), x.Attribute("FileRelativePath").Value.ToString(), x.Attribute("TimeStamp").Value.ToString()));
            //    }
            //}

            //List<Tuple<string, string, string, string>> downloadList = new System.Collections.Generic.List<Tuple<string, string, string, string>>();

            //foreach (var sourceFile in sourceFileDetails)
            //{
            //    var localFile = localFileDetails.Where(p => p.Item1.Equals(sourceFile.Item1)).FirstOrDefault();
            //    if(localFile != null)
            //    {
            //        if (!sourceFile.Item2.Equals(localFile.Item2) || !sourceFile.Item4.Equals(localFile.Item4))
            //            downloadList.Add(sourceFile);
            //        continue;
            //    }
            //    downloadList.Add(sourceFile);
            //}

            //if(downloadList.Count > 0)
            //{
            //    switch(new Uri(SourcePath).Scheme)
            //    {
            //        case "file":
            //            DownloadFromFilePath(SourcePath, DestinationPath, RobotVersionFile);
            //            foreach (var item in downloadList)
            //            {
            //                 DownloadFromFilePath(SourcePath, DestinationPath, item.Item3);
            //            }
            //            break;
            //        case "http":
            //            DownloadFromWeb(SourcePath, DestinationPath, RobotVersionFile);
            //            foreach (var item in downloadList)
            //            {
            //                 DownloadFromWeb(SourcePath, DestinationPath, item.Item3);
            //            }
            //            break;
            //    }
            //    UpdateRobotDetailsInRobotConfigFile(robotDetails.RobotPath, robotConfigurationFileDropFolder, robotDetails);
            //    StartRobot(robotDetails, true);
            //    robotDetails.Message = "Upgrade completed. Robot launched. ";
            //}
            //else
            //{
            //    UpdateRobotDetailsInRobotConfigFile(robotDetails.RobotPath, robotConfigurationFileDropFolder, robotDetails);
            //    StartRobot(robotDetails, true);
            //    robotDetails.Message = "Already latest version is present. Robot launched. ";
            //}

        }
    }
}
