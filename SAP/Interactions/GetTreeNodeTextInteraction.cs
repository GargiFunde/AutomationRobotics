// <copyright file=GetTreeNodeTextInteraction company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Automation.Common.SAP;
using Studio.SAP;
//using Utilities.Logging;

namespace Studio.SAPAutomation.Interactions
{
    public class GetTreeNodeTextInteraction : InteractionBase
    {
        #region Constants
        private readonly string logSourceModule = "SAPAutomation.GetTreeNodeTextInteraction";
        #endregion
        private string extractedValue = string.Empty;

        string nodeKey;
        [DisplayName("Node Key")]
        [DataMember]
        public string NodeKey
        {
            get
            {
                return nodeKey;
            }
            set
            {
                nodeKey = value;
            }
        }


        public GetTreeNodeTextInteraction()
        {

        }

        public GetTreeNodeTextInteraction(string nodeKey)
        {
            this.nodeKey = nodeKey;
        }
        public override void DoAutomation<T>(T configuredData)
        {
            try
            {
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Enter Method");
                SAPControlDetails controlDetails = configuredData as SAPControlDetails;
                extractedValue = string.Empty;
                var treeDetails = nodeKey.Split('?');
                var cellName = $"\"{treeDetails[0]}\"";                
                var row = treeDetails.Length==2 ? $"\"{treeDetails[1]}\"" : "1" ;
                string propertyToFetch = "Default";
                string arguments =
                $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} { propertyToFetch} {row} {cellName}";

                extractedValue = ExecuteScript(arguments);
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Exit Method");
            }
            catch (Exception ex)
            {
                //Logging.LogExceptionMsg(logSourceModule, "DoAutomation", ex.Message, ex, true);
                throw;
            }
        }

        public override string GetExtractedValue()
        {
            return extractedValue;
        }

        /// <summary>
        /// Overriding ToString to show interaction type
        /// </summary>
        /// <returns>interaction</returns>
        public override string ToString()
        {
            return "Extract Value";
        }
    }
}
