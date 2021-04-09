// <copyright file=HeartBeat company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    [DataContract]
    public class HeartBeat
    {
        //  public long RobotId { get; set; }
        [DataMember]
        public string RobotName { get; set; }

        [DataMember]
        public string MachineName { get; set; }
       
        [DataMember]
        public string UserName { get; set; }
       
        [DataMember]
        public string ProcessName { get; set; }
        [DataMember]
        public RoboExecutionStatus roboExecutionStatus { get; set; }
        [DataMember]
        public string RoboColor { get; set; }
        [DataMember]
        public int TenantId { get; set; }
        [DataMember]
        public int TotalRequestServed { get; set; }
        [DataMember]
        public string LastRequestTimeInSeconds { get; set; }
    }

    public enum RoboExecutionStatus
    {
        RobotLaunching = 1,
        RobotLaunchFailed = 2,
        RobotReadyState = 3,
        RobotProcessingAutomation = 4,
        RobotCompetedAutomation = 5,
        RobotAutomationFailed = 6,
        RobotStopped = 7,
    }

    public enum RoboAgentActions
    {
        None = 0,
        Start = 1,
        Stop = 2,
        Upgrade = 3
    }
    public enum AutomationExecutionState
    {
        AllAppPluginLaunching = 1,
        AllAppPluginLaunchSignedInCompleted = 2,
        AllAppPluginSearchStarted = 3,
        AllAppPluginSearchCompleted = 4,
        AllAppPluginResetInitiated = 5,
        AllAppPluginResetCompleted = 6
    }
    public enum SearchStatus
    {
        NotInitiated = 0,
        Initiated = 1,
        Success = 2,
        Failed = 3
    }
    public enum ResetStatus
    {
        NotInitiated = 0,
        Initiated = 1,
        Success = 2,
        Failed = 3
    }
}
