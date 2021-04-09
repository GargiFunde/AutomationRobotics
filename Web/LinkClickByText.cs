
using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Data;
using OpenQA.Selenium;
using System.Threading;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(LinkClickByTextActivityDesigner))]
    [ToolboxBitmap("Resources/WebLinkByText.png")]
    public class Web_LinkByText : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [DisplayName("Link Text")]
        public InArgument<string> LinkText { get; set; }    
        public bool IsPartial { get; set; }

        [Category("Input")]
        [DisplayName("Wait Before")]
        [Description("in milliseconds")]
        public InArgument<int> WaitBefore { get; set; }

        [Category("Input")]
        [DisplayName("WaitAfter")]
        [Description("in milliseconds")]
        public InArgument<int> WaitAfter { get; set; }

        //[RequiredArgument]
        //public InArgument<string> TableBody { get; set; }

        //public InArgument<int> TimeOutInSecond { get; set; }
        [Category("Input")]
        public OutArgument<DataTable> TableResult { get; set; }
        [Category("Input")]
        public bool IsHeader { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
           // Table tableResult = null; 
            string AppId = ApplicationIDToAttach.Get(context);
            int iwaitbefore = WaitBefore.Get(context);
            int iwaitafter = WaitAfter.Get(context);
            string sLinkText = LinkText.Get(context);

            //string tableBody = TableBody.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        IWebElement linkWeb = null;
                        if(iwaitbefore >0)
                        {
                            Thread.Sleep(iwaitbefore);
                        }
                        CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                        if(IsPartial)
                        {
                            commonWebBrowser_Current.driver.FindElement(By.PartialLinkText(sLinkText)).Click();
                        }
                        else
                        {
                            commonWebBrowser_Current.driver.FindElement(By.LinkText(sLinkText)).Click();
                        }

                        if (iwaitafter > 0)
                        {
                            Thread.Sleep(iwaitafter);
                        }
                    }
                }
                      
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in Web activity GetTable", Logger.LogLevel.Error);
             
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
