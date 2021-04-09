// <copyright file=ValidationResponse company=E2E BOTS>
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

namespace TCP.Shared.Messages
{
    [Serializable]
    public class ValidationResponse : ResponseMessageBase
    {
        public ValidationResponse(RequestMessageBase request)
            : base(request)
        {

        }

        public bool IsValid { get; set; }
    }
}
