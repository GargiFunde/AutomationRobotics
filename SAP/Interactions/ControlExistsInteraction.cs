// <copyright file=ControlExistsInteraction company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

using Automation.Common.SAP;
using SE.Core.Automation.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Utilities.Logging;

namespace Studio.SAP.Interactions
{
    public class ControlExistsInteraction : InteractionBase
    {
        #region Constants
        private readonly string logSourceModule = "SAPAutomation.ControlExistsInteraction";
        #endregion
        private string controlExists;
        public override void DoAutomation<T>(T configuredData)
        {
            try
            {
                Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Enter Method");
                SAPControlDetails controlDetails = configuredData as SAPControlDetails;
                controlExists = "false";

                string propertyToFetch = "ControlExists";
                int rowNumber = 0;
                string cellName = "NA";
                string arguments =
                $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} { propertyToFetch} { rowNumber} { cellName}";

                controlExists = ExecuteScript(arguments);
                Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Exit Method");
            }
            catch (Exception ex)
            {
                Logging.LogExceptionMsg(logSourceModule, "DoAutomation", ex.Message, ex, true);
                throw;
            }
        }

        public override string GetExtractedValue()
        {
            return controlExists.ToString();
        }

        /// <summary>
        /// Overriding ToString to show interaction type
        /// </summary>
        /// <returns>interaction</returns>
        public override string ToString()
        {
            return "Control Exists";
        }
    }
}
