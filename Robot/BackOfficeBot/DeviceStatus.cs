// <copyright file=DeviceStatus company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:49</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOfficeBot
{
    public enum DeviceStatus
    {
        Uninitialised,
        Initialised,
        Starting,
        Running,
        Error,
    }
}
