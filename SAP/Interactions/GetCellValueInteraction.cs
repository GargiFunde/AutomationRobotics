// <copyright file=GetCellValueInteraction company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

using Automation.Common.SAP;
using SE.Core.Automation.Interfaces;
using SE.Core.Automation.Models.Common;
using SE.Core.Automation.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Utilities.Logging;

namespace Studio.SAP.Interactions
{
    [DataContract]
    [Serializable]
    public class GetCellValueInteraction : InteractionBase
    {
        #region Constants
        private readonly string logSourceModule = "SAPAutomation.GetCellValueInteraction";
        #endregion
        private string extractedValue = string.Empty;
        int currentRowNumber = 0;

        private InteractionInput rowNumber;     
        [DataMember]
        [DisplayName("Row Number")]
        public InteractionInput RowNumber
        {
            get
            {
                return rowNumber;
            }
            set
            {
                rowNumber = value;
                OnPropertyChanged("RowNumber");
            }
        }

        string cellName = "NA";        
        [DisplayName("Cell Name")]    
        [DataMember] 
        public string CellName
        {
            get
            {
                return cellName;
            }
            set
            {
                cellName = value;
            }
        }

        public GetCellValueInteraction(string rowNumber, string cellName)
        {
            int rowNum;
            if(int.TryParse(rowNumber,out rowNum))
            {
                this.rowNumber = new InteractionInput()
                {
                    IsDefaultValue = true,
                    Value = rowNumber
                };
            }            
            else
            {
                this.rowNumber = new InteractionInput()
                {
                    IsDefaultValue = false,
                    Value = rowNumber
                };
            }
            this.cellName = cellName;
        }

        public GetCellValueInteraction()
        {

        }

        public override void DoAutomation<T>(T configuredData)
        {
            try
            {
                //Logging.LogDebugMsg(logSourceModule, "DoAutomation", "Enter Method");
                SAPControlDetails controlDetails = configuredData as SAPControlDetails;
                extractedValue = string.Empty;

                int row = 0;
                if (rowNumber.IsDefaultValue)
                    row = Convert.ToInt32(rowNumber.Value);
                else
                    row = currentRowNumber;

                string arguments = string.Empty;

                //if the control is GuiTableControl
                if (controlDetails.ControlType.Equals("GuiTableControl"))
                {          
                    // for scrolling  vertical scroll bar
                    arguments =
                     $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} {"VerticalScroll"} {row} {this.cellName}";
                    ExecuteScript(arguments);
                    arguments =
                    $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} {"ControlValue"} {row} {this.cellName}";
                }
                else //GuiGridView
                {                     
                    arguments =  $".\\SapScripts\\FieldExtractor.vbs {controlDetails.ConnectionId} {controlDetails.SessionId} {controlDetails.ControlId} {controlDetails.ControlType} {"Default"} {row} {this.cellName}";
                }         

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

        public override bool RequiresInput()
        {
            return !rowNumber.IsDefaultValue;
        }

        public override string GetInputVariable()
        {
            return rowNumber.Value;
        }

        public override void SetInputValue(string inputValue)
        {
            currentRowNumber = Convert.ToInt32(inputValue);
        }

        /// <summary>
        /// Overriding ToString to show interaction type
        /// </summary>
        /// <returns>interaction</returns>
        public override string ToString()
        {
            return "Get Cell Value";
        }
    }
}
