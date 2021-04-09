// <copyright file=ExtractValueInteraction company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>


using Automation.Common.SAP;
using SE.Core.Automation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAP.Interactions
{
    public class ExtractValueInteraction : InteractionBase
    {
        #region Constants
        private readonly string logSourceModule = "SAPAutomation.ExtractValueInteraction";
        #endregion
        private string extractedValue = string.Empty;
        public override void DoAutomation<T>(T configuredData)
        {
            try
            {
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Enter Method");
                SAPControlDetails controlDetails = configuredData as SAPControlDetails;
                extractedValue = string.Empty;

                string propertyToFetch = "Default";
                int rowNumber = 0;
                string cellName = "NA";
                string arguments =
                $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} { propertyToFetch} { rowNumber} { cellName}";

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
