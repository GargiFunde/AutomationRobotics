// <copyright file=RouteConfig company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:52</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace RobotsController
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
