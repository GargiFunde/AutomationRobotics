// <copyright file=ErrorMapper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLibrary
{
   public class ErrorMapper
    {
        public static string GetErrorMappingText(int iErrorNo)
        {
            if (iErrorNo == 60665573)
            {
                //	None of the specified endpoints were reachable
                return "Queue System is down";
            }
            return "";
        }       
    }
}
