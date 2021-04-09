﻿using System;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;


[assembly: OwinStartup(typeof(FrontOfficeBotWebApp.Startup))]

namespace FrontOfficeBotWebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
