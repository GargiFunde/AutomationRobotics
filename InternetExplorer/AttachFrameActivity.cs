using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using Logger;
using System.Drawing;
using System.Windows;


namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/attach.png")]
    [Designer(typeof(AttachFrameDesigner))]
    public class AttachFrame : BaseNativeActivity
    {
        //private readonly CompletionCallback onCompleted;
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID To Attach")]
        [Description("Browser application containing frame")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

       
        //[Category("Input")]
        //[DisplayName("Browser")]
        //[Description("Browser application containing frame")]
        //public InArgument<string> TitleOrUrlToAttach { get; set; }

        [Category("Frame Id or Name")]
        [Description("Frame id or name to attach")]
        [DisplayName("Attach Frame")]
        public InArgument<string> AttachFrameByIdOrName { get; set; }

        
        [Browsable(false)]
        public List<System.Activities.Activity> Activities { get; set; }

        [Browsable(false)]
        public string strUniqueControlld { get; set; }

     
        public AttachFrame()
        {
            if (Activities == null)
            {
                Activities = new List<System.Activities.Activity>();
            }
            //Anything instantiated in constructor will not able to get persist or reassign after restart
        }
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
             base.CacheMetadata(metadata);
        }
        string strApplicationIDToAttach = string.Empty;
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                WatiN.Core.IE commonBrowser = null;
                string strAttachFrameByIdOrName = AttachFrameByIdOrName.Get(context);
                string strApplicationIDToAttach = ApplicationIDToAttach.Get(context);
              //  string strTitleOrUrlToAttach = TitleOrUrlToAttach.Get(context);
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strApplicationIDToAttach))
                {
                    commonBrowser = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strApplicationIDToAttach];
                }

                if (string.IsNullOrEmpty(strAttachFrameByIdOrName))
                {
                    Logger.Log.Logger.LogData("''Browser' is Not Set", LogLevel.Error);
                    context.Abort();
                }

                try
                {
                    int iframeId = Convert.ToInt32(strAttachFrameByIdOrName); //by Id
                   // commonBrowser.driver.SwitchTo().Frame(iframeId);
                    commonBrowser.Frame(iframeId.ToString());
                }
                catch (Exception ex)
                {
                    try
                    {
                        //   commonBrowser.driver.SwitchTo().Frame(strAttachFrameByIdOrName); //By name
                        WatiN.Core.Frame a = commonBrowser.Frame(strAttachFrameByIdOrName);
                        WatiN.Core.Element el1 =  a.Element(WatiN.Core.Find.ById("user_name"));
                        el1.SetAttributeValue("value","admin");
                        WatiN.Core.Element el2 = a.Element(WatiN.Core.Find.ById("user_password"));
                        el2.SetAttributeValue("value", "PAXrvl4cwX0U");
                        WatiN.Core.Element el3 = a.Element(WatiN.Core.Find.ById("sysverb_login"));
                        el3.Click();
                    }
                    catch (Exception exInner)
                    {
                        Log.Logger.LogData("AttachFrameByIdOrName is not valid", LogLevel.Error);
                        context.Abort();
                    }

                }

                activitycount = 0;
                var item = Activities[activitycount];
                if (item is ActivityExtended)
                {
                    ActivityExtended extended = (ActivityExtended)item;

                    // extended.ExecuteMe(context, strApplicationIDToAttach, strTitleOrUrlToAttach);
                    extended.ExecuteMe(context, strApplicationIDToAttach, "");
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
                strApplicationIDToAttach = ApplicationIDToAttach.Get(context);
                // Thread.Sleep(500);
                activitycount = activitycount + 1;
                if (activitycount >= Activities.Count)
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strApplicationIDToAttach))
                    {
                        WatiN.Core.IE commonBrowser = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strApplicationIDToAttach];
                       // commonBrowser.driver.SwitchTo().DefaultContent();
                       
                    }
                  
                    return;
                }
                var itemInner = Activities[activitycount];
                if (itemInner is ActivityExtended)
                {
                    ActivityExtended extended = (ActivityExtended)itemInner;
                    string strAppIdToAttach = ApplicationIDToAttach.Get(context);
                    //string strTitleorUrl = TitleOrUrlToAttach.Get(context);
                    string strTitleorUrl = "";
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

    }
}
