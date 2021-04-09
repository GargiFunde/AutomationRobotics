// <copyright file=ActivityDesigner company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:13</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;

namespace Core.ActivityLibrary.Activities
{
    [Designer(typeof(ActivityDesigner1))]
    public class ActivityDesigner : ActivityExtended
    {
        [RequiredArgument]
        public string ApplicationID { get; set; }
        [RequiredArgument]
        public string LoginUrl { get; set; }
        [RequiredArgument]
        public string SearchUrl { get; set; }
        [RequiredArgument]
        public int TimeOutInSecond { get; set; }

        public OutArgument<bool> LaunchResult { get; set; }
        public ActivityDesigner ()
        {
           // SelectHelper.MyProperty.Add("1", new List<Activity>());
        }
        protected override void Execute(NativeActivityContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
