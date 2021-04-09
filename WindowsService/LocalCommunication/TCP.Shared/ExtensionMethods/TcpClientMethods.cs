// <copyright file=TcpClientMethods company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:07</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public static class TcpClientMethods
{
    public static String GetIP(this TcpClient client)
    {
        return ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
    }
}



