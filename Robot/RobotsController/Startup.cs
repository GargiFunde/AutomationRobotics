// <copyright file=Startup company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:52</date>
// <summary></summary>

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RobotsController.Startup))]
namespace RobotsController
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
