using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using Logger;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Icons/ActivityLibrary/SendKey.png")]
    [Designer(typeof(AttachApplicationActivitiesDesigner))]
    public class AttachApplication : BaseNativeActivity
    {
        //private readonly CompletionCallback onCompleted;
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Attach by ApplicationID*")]
        [Description("ID of Open Application")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [Description("ID of Open Application")]
        [DisplayName("Attach by Title/URL")]
        public InArgument<string> TitleOrUrlToAttach { get; set; }

        [Category("Input")]
        [Description("Reset Application")]
        [DisplayName("Reset Application")]
        public bool ResetApplication { get; set; }

        [Browsable(false)]
        public Collection<System.Activities.Activity> Activities { get; set; }

        [Browsable(false)]
        public string strUniqueControlld { get; set; }

        [Browsable(false)]
        public string SearchUrl { get; set; }

        public AttachApplication()
        {
            if (Activities == null)
            {
                Activities = new Collection<System.Activities.Activity>();
            }
            //Anything instantiated in constructor will not able to get persist or reassign after restart
        }
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
             base.CacheMetadata(metadata);
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string strAppIdToAttach = ApplicationIDToAttach.Get(context);
                string strTitleOrUrlToAttach = TitleOrUrlToAttach.Get(context);



                if ((strAppIdToAttach == null) && (strTitleOrUrlToAttach == null))
                {
                    Logger.Log.Logger.LogData("''Attach by ApplicationID'  or 'Attach by Title/URL' Not Set", LogLevel.Error);
                    context.Abort();
                }
                activitycount = 0;
                var item = Activities[activitycount];
                if (item is ActivityExtended)
                {
                    ActivityExtended extended = (ActivityExtended)item;

                    extended.ExecuteMe(context, strAppIdToAttach, strTitleOrUrlToAttach);

                }

                context.ScheduleActivity(item, onCompleted, onFaulted);

                //  context.ScheduleActivity(item, onCompleted, onFaulted);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity AttachApplication", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        private void onFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            throw new NotImplementedException();
        }

        int activitycount = 0;
        private void onCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            try
            {
               // Thread.Sleep(500);
                activitycount = activitycount + 1;
                if (activitycount >= Activities.Count)
                {
                   if(ResetApplication)
                    {
                        //Thread.Sleep(5000); //replace by appropriate logic
                        Reset();
                     //   Thread.Sleep(7000); //replace by appropriate logic
                       
                    }
                    return;
                }
                var itemInner = Activities[activitycount];
                if (itemInner is ActivityExtended)
                {
                    ActivityExtended extended = (ActivityExtended)itemInner;
                    string strAppIdToAttach = ApplicationIDToAttach.Get(context);
                    string strTitleorUrl = TitleOrUrlToAttach.Get(context);
                    extended.ExecuteMe(context, strAppIdToAttach, strTitleorUrl);
                }
                //Thread.Sleep(500);
                context.ScheduleActivity(itemInner, onCompleted);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        private void Reset()
        {
            
            ResetEventArgs resetEventArgs = new ResetEventArgs();

            resetEventArgs.ApplicationID = ApplicationIDToAttach.Expression.ToString();// inarg.Expression.ToString();
            resetEventArgs.UniqueActivityId = strUniqueControlld;
            SelectHelper.OnReset(resetEventArgs);
        }
    }
}
