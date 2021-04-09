// <copyright file=ActivityExtended company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public abstract class ActivityExtended: NativeActivity
    {
        public ActivityExtended() : base()
        {
            UniqueControlld = DateTime.Now.ToString("yyMMddHHmmss");
            WaitBefore = 0;
            WaitAfter = 0;
            WaitMaxTimeWhileLoop = 2000;
            ContinueOnError = true;
            //SetControlValue = new Object();
        }
       
        [Browsable(false)]
        public string UniqueControlld { get; set; }

        [DisplayName("Get Value")]
        [Description("Get Control Value")]
        [CategoryAttribute("Input/Output]")]
        public OutArgument<Object> GetControlValue { get; set; }

        [DisplayName("Set Value")]
        [Description("Set Value to control")]
        [CategoryAttribute("Input/Output]")]
        public InArgument<Object> SetControlValue { get; set; }

        [DisplayName("Id [Priority:1]")]
        [Description("Id to find control")]
        public InArgument<string> ControlId { get; set; }


        [DisplayName("Name [Priority:2]")]
        [Description("Control Name to find control")]
        public InArgument<string> ControlName { get; set; }
                
        [DisplayName("Wait Before")]
        [Description("in milliseconds")]
        public InArgument<int> WaitBefore { get; set; }

        [DisplayName("WaitAfter")]
        [Description("in milliseconds")]

        public InArgument<int> WaitAfter { get; set; }

        [DisplayName("Wait During Text Type")]
        [Description("in milliseconds")]
        public InArgument<int> WaitDuringTextType { get; set; }

        [DisplayName("Wait Maximum Time till")]
        [Description("in milliseconds")]
        public InArgument<int> WaitMaxTimeWhileLoop { get; set; }

        [DisplayName("Error Message")]
        [CategoryAttribute("Input/Output]")]
        [Description("Error Message if error occure during automation of this control")]

        public InArgument<string> ErrorMessage { get; set; }

        //[DisplayName("Step Number")]
        //[CategoryAttribute("Input/Output]")]
        //[Description("Automation Step Number to troubleshoot. It is availabel in request transaction log and Log file.")]
        //public InArgument<int> StepNumber { get; set; }

        //[DisplayName("Is Part Of Retry?")]
        //[CategoryAttribute("Input/Output]")]
        //[Description("Is this action part of retry mechanism")]
        //public InArgument<int> IsPartOfRetry { get; set; }

        [DisplayName("Is Click")]
        [Description("Click on control")]
      
        public bool IsEventField { get; set; }

        [DisplayName("Click Before Value Set")]
        [Description("Click Before Value Set")]

        public bool ClickBeforeValueSet { get; set; }

        [DisplayName("Continue On Error")]
        [Description("Continue Automation Execution On Error")]
        public bool ContinueOnError { get; set; }

        [DisplayName("Image Path [Priority:11]")]
        [Description("XPath to find control")]

        public InArgument<string> ImagePath { get; set; }

        [DisplayName("X co-ordinate")]
        [Description("X co-ordinate")]
        [CategoryAttribute("Location [Priority:Last]")]
        public InArgument<int> X { get; set; }

        [DisplayName("Y co-ordinate")]
        [Description("Y co-ordinate")]
        [CategoryAttribute("Location [Priority:Last]")]
        public InArgument<int> Y { get; set; }

        public virtual void ExecuteMe(NativeActivityContext context,string ApplicationIDToAttach, string strTitleOrUrlToAttach)
        {


        }
        
    }
}
