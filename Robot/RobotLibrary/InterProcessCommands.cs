// <copyright file=InterProcessCommands company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLibrary
{
    public class InterProcessCommands
    {
        public InterProcessCmdType InterProcessCommandType { get; set; }
        public string TicketID { get; set; }
        public int MainWindowHandle { get; set; }
        public IntPtr AgentWindowHandle { get; set; }
        public string SenderUniqueId { get; set; }
        public int CommandWaitTime { get; set; }
        public object Message { get; set; }
        public string CorreletionId { get; set; }
        public string ReplyTo { get; set; }
        public string TransactionType { get; set; }
        public int InstanceId { get; set; }
    }

    public enum InterProcessCmdType
    {
        Start,
        Handshake,
        Authenticate,
        ConnectToQueue,
        DisconnectFromQueue,
        SignIn,
        Acknowledgement,
        Heartbeat,
        Stop
    }
}
