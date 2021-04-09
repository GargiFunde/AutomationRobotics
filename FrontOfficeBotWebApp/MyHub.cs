using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FrontOfficeBotWebApp
{
    public class MyHub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.addNewMessageToPage(message);
            
        }
        //public void Send(string message)
        //{
        //    // Call the broadcastMessage method to update clients.
        //    Clients.All.addNewMessageToPage(message);
        //    Clients.Client
        //}
    }
}