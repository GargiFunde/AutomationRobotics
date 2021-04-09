// <copyright file=NestedActivity company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Activities.XamlIntegration;
using System.Activities.Presentation.Model;
using System.Activities.Presentation;
using System.IO;
using System.Activities.Statements;
using System.Xaml;

namespace Core.ActivityLibrary.Activities
{
    [Designer(typeof(NestedActivity1))]
    public class NestedActivity : NativeActivity
    {
        [RequiredArgument]
        public Dictionary<string, Argument> Arguments { get; set; }

        string activityPath = string.Empty;
        [RequiredArgument]
        public string ActivityPath
        {
            get
            {
                //if (_wfDesigner == null)
                //    NewInstance(_defaultWorkflow);
                return activityPath;
            }
            set
            {
                activityPath = value;
                //if (activityPath != string.Empty)
                //{
                //    if (File.Exists(activityPath))
                //    {
                //        NestedActivityCreated = ActivityXamlServices.Load(@"E:\Work\BOTDesignerMaster\TestArg.xaml");

                //        //string activityxaml = File.ReadAllText(@"E:\Work\BOTDesignerMaster\TestArg.xaml");
                //        //ActivityXamlServicesSettings settings = new ActivityXamlServicesSettings
                //        //{
                //        //    CompileExpressions = true
                //        //};
                //        // NestedActivityCreated = ActivityXamlServices.Load(new StringReader(activityxaml), settings);

                //        //NestedActivityCreated = ActivityXamlServices.Load(ActivityXamlServices.CreateBuilderReader(new XamlXmlReader( new StringReader(activityxaml))));
                //        //if (XAMLActivity == null)
                //        //    XAMLActivity = new List<Activity>();
                //        //  XAMLActivity.Add(activity);

                //        //ModelItem mi = SelectHelper._wfDesigner.Context.Services.GetService<ModelTreeManager>().CreateModelItem(null, activity);


                //        //testmetadata.AddChild(XAMLActivity);

                //    }
                //}

            }
        }
        //[RequiredArgument]
        //public OutArgument<NestedActivity> SubActivity { get; set; }
        public Activity NestedActivityCreated { get; set; }



        public NestedActivity()
        {
            if (activityPath != string.Empty)
            {
                if (File.Exists(activityPath))
                {
                    // @"E:\Work\BOTDesignerMaster\TestArg.xaml");
                    //EditingContext.Services.GetService<ModelTreeManager>().CreateModelItem(null, MYwf);
                    // SelectHelper.MyProperty.Add("1", new List<Activity>());
                }
            }
        }

        //NativeActivityMetadata testmetadata;
        //protected override void CacheMetadata(NativeActivityMetadata metadata)
        //{
        //    base.CacheMetadata(metadata);
        //    testmetadata = metadata;
        //}

        protected override void Execute(NativeActivityContext context)
        {
            //  context.SetValue(SubActivity, MYwf);
            context.ScheduleActivity(NestedActivityCreated, onCompleted);
            //throw new NotImplementedException();
        }
        private void onCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {

        }
    }
}
