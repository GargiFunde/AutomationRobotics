// <copyright file=WindowNotFoundException company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:15:03</date>
// <summary></summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPath_Recorder
{
    public class WindowNotFoundException: Exception
    { 
        public WindowNotFoundException(string constraint, int waitTimeInSeconds) :
            base("Could not find an window matching constraint: " + constraint + ". Search expired after '" + waitTimeInSeconds.ToString() + "' seconds.")
        { }
    }
}
