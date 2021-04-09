// <copyright file=MainframeEntity company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:58</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Mainframe
{
    public class MainframeEntity : CommonLibrary.Interfaces.IApplicationInterface
    {
        public string AppId { get; set; }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
