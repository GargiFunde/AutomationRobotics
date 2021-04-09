// <copyright file=GetRowCountInteraction company=E2E Robotics>
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
//using Utilities.Logging;

namespace Studio.SAP.Interactions
{
    public class GetRowCountInteraction : InteractionBase
    {      
        private readonly string logSourceModule = "SAPAutomation.ExtractValueInteraction";       

        private string rowCount = "0";
        public override void DoAutomation<T>(T configuredData)
        {
            try
            {
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Enter Method");

                rowCount = "0";
                SAPControlDetails controlDetails = configuredData as SAPControlDetails;
                int rowNumber = 0;
                string cellName = "NA";
                string propertyToFetch = "RowCount";
                string arguments =
                $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} { propertyToFetch} { rowNumber} { cellName}";
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Exit Method");
                rowCount = ExecuteScript(arguments);
            }
            catch (Exception ex)
            {
                //Logging.LogExceptionMsg(logSourceModule, "DoAutomation", ex.Message, ex, true);
                throw;
            }
        }

        public override string GetExtractedValue()
        {
            return rowCount.ToString();
        }

        /// <summary>
        /// Overriding ToString to show interaction type
        /// </summary>
        /// <returns>interaction</returns>
        public override string ToString()
        {
            return "Get Row Count";
        }
    }
   
}
