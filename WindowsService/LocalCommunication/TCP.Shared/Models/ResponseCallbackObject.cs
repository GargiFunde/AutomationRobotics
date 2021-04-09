// <copyright file=ResponseCallbackObject company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:07</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP.Shared.Models
{
    public class ResponseCallbackObject
    {
        public Delegate CallBack { get; set; }
        public Guid ID { get; set; }
    }
}
