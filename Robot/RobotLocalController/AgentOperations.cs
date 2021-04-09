// <copyright file=AgentOperations company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using RobotLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using CommonLibrary;
namespace RobotLocalController
{
    public class AgentOperations //DefaultAgent
    {
        public AgentOperations()
        {
            interprocessCommunicator = new InterProcessCommunicator();
            interprocessCommunicator.communicationMessageReceived -= InterprocessCommunicator_CommunicationMessageReceived;
            interprocessCommunicator.communicationMessageReceived += InterprocessCommunicator_CommunicationMessageReceived;
        }

        string sourcePath;
        public string SourcePath { 
            get
            {
                if(string.IsNullOrEmpty(sourcePath))
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

        //It will not receive any messagges from robot ..please go to main windo code behind for the same
        void InterprocessCommunicator_CommunicationMessageReceived(object sender, CommunicatorMessageEventArgs e)
        {
        }
        public InterProcessCommunicator interprocessCommunicator { get; set; }


        public bool StartRobot(RobotDetails robotDetails, bool updateConfigMode = false)
        {
            bool exists = CheckRobotState.CheckIfRobotIsRunning(robotDetails.ProcessId, robotDetails.RobotPath);
            if(exists)
            {
                throw new InvalidOperationException(string.Format("Robot instance is already running with processid : {0}.", robotDetails.ProcessId));
            }
            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = robotDetails.RobotFullName,
                WorkingDirectory = robotDetails.RobotPath,
                UseShellExecute = false
            };

            if(interprocessCommunicator !=null && interprocessCommunicator.ProcessIdentifier !=null)
                processStartInfo.Arguments += ((IntPtr)interprocessCommunicator.ProcessIdentifier).ToString();

            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForInputIdle();
            while ((process.Id == 0) || (process.MainWindowHandle==IntPtr.Zero))
            {
                Thread.Sleep(500);
                process.Refresh();
            }
            robotDetails.ProcessId = process.Id;
            robotDetails.MainWindowHandle = process.MainWindowHandle;
            robotDetails.Message = "LaunchCompleted!";
            robotDetails.IsLaunched = true;
            return true;
        }
        public bool SignInRobot(RobotDetails robotDetails,string message)
        {
            bool exists = CheckRobotState.CheckIfRobotIsRunning(robotDetails.ProcessId, robotDetails.RobotPath);
            if (exists)
            {
                if (interprocessCommunicator != null && interprocessCommunicator.ProcessIdentifier != null)
                {
                    robotDetails.Message = "Robot Sign-in started!";
                    RobotLibrary.InterProcessCommands interprocCmd = new InterProcessCommands();
                    interprocCmd.Message = message;
                    interprocCmd.AgentWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
                    interprocCmd.InterProcessCommandType = InterProcessCmdType.SignIn;
                    interprocessCommunicator.SendMessage(robotDetails.MainWindowHandle, JsonConvert.SerializeObject(interprocCmd));
                    robotDetails.IsSignedIn = true;
                    return true;
                }
                else
                {
                    Log.Logger.LogData("interprocessCommunicator object is null." , LogLevel.Debug);
                }
            }else
            {
                Log.Logger.LogData("Robot not started", LogLevel.Debug);
            }
            return false;
        }
        public bool StopRobot(RobotDetails robotDetails, bool updateConfigMode = false)
        {
            RobotLibrary.InterProcessCommands interprocCmd = new InterProcessCommands();
           // interprocCmd.CommandWaitTime = 6000;
            interprocCmd.InterProcessCommandType = InterProcessCmdType.Stop;
            interprocessCommunicator.SendMessage(robotDetails.MainWindowHandle, JsonConvert.SerializeObject(interprocCmd));
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
