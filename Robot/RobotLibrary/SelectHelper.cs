// <copyright file=SelectHelper company=E2E BOTS>
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
    public static class SelectHelper
    {

        public static event EventHandler<RaiseErrorToTrayEventArgs> RaiseErrorToTray;
        public static void OnRaiseErrorToTray(RaiseErrorToTrayEventArgs e)
        {
            EventHandler<RaiseErrorToTrayEventArgs> handler = RaiseErrorToTray;
            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
    public class RaiseErrorToTrayEventArgs : EventArgs
    {
        public string ErrorNumber;
        public string ErrorDescription;
    }
}
