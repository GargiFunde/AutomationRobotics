// <copyright file=PerformAutomation company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:03:08</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataConnectorToGenerateRequest
{
    public class PerformAutomation
    {
        public PerformAutomation(string ProcessName, string ProfileName)
        {
            AutomationInputDictionary = new object();
            this.ProcessName = ProcessName;
            this.ProfileName = ProfileName;
            APIVersion = AppId = CommandExecutionWindow = CommandGenerationSource = Country = Instance = PartnerId = Timestamp = ReferenceCode = VID = string.Empty;
            UserName = Environment.UserName;
            Timestamp = DateTime.Now.ToShortTimeString();
        }
        public object AutomationInputDictionary { get; set; }
        public string ProcessName { get; set; }
        public string ProfileName { get; set; }
        public string APIVersion { get; set; }
        public string AppId { get; set; }
        public string CommandExecutionWindow { get; set; }
        public string CommandGenerationSource { get; set; }
        public string Country { get; set; }
        public string Instance { get; set; }
        public string PartnerId { get; set; }
        public string ReferenceCode { get; set; }
        public string Timestamp { get; set; }
        public string UserName { get; set; }
        public string VID { get; set; }
    }
}
