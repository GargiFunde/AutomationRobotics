// <copyright file=ClientValidatingEventArgs company=E2E BOTS>
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
using TCP.Shared.Messages;

namespace TCP.Server.EventArguments
{
    public class ClientValidatingEventArgs
    {
        public Receiver Receiver { get; set; }
        public ValidationRequest Request { get; set; }
        private Action ConfirmAction;
        private Action RefuseAction;

        public ClientValidatingEventArgs(Action confirmAction,Action refuseAction)
        {
            ConfirmAction = confirmAction;
            RefuseAction = refuseAction;
        }

        public void Confirm()
        {
            ConfirmAction();
        }

        public void Refuse()
        {
            RefuseAction();
        }
    }
}
