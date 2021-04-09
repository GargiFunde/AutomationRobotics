// <copyright file=ProcessPriority company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationAgent
{
    public struct ProcessPriority
    {
        public enum priority : uint
        {
            IDLE = 0x40,
            BELOW_NORMAL = 0x4000,
            NORMAL = 0x20,
            ABOVE_NORMAL = 0x8000,
            HIGH_PRIORITY = 0x80,
            REALTIME = 0x100
        }
    }
}
