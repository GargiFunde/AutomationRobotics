// <copyright file=ControlHighlighterWpfForm.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AutomationService
{
    public class Client
    {
        static void Main()
        {
            BOTService client = new BOTService();

            // Use the 'client' variable to call operations on the service.

            // Always close the client.
            QuartzHelper quartzHelper = new QuartzHelper();
            quartzHelper.CreateJob("A", "B", "C", "0 0/1 * 1/1 * ? *",2, 1);
        }

    }
}