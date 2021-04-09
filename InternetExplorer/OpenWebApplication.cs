using CommonLibrary.Interfaces;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Activity.InternetExplorer
{

    [Designer(typeof(IE_OpenWebApplicationActivityDesigner))]
    [ToolboxBitmap("Resources/IE_OpenApplication.png")]
    public class IE_OpenApplication : NativeActivity, IOpenApplicationInterface
    {
        [RequiredArgument]
        [DefaultValue(null)]
        [Category("Input")]
        [DisplayName("Application ID*")]
        [Description("ID that will be used across Sequence")]
        public InArgument<string> ApplicationID { get; set; }
       
        [RequiredArgument]
        [DefaultValue(null)]
        [Category("Input")]
        [DisplayName("Url*")]
        [Description("Url that you want to open in Internet Explorer")]
        public InArgument<string> Url { get; set; }

        //[RequiredArgument]
        [DefaultValue(30000)]
        [Category("Input")]
        [DisplayName("Time Out(In Seconds)")]
        [Description("Enter TimeOut Value in Seconds")]
        public InArgument<int> TimeOutInSecond { get; set; }

        // public string UniqueApplicationID { get; set; }
        [Category("Error")]
        [DisplayName("Continue On Error")]
        public bool ContinueOnError { get; set; }

        [Category("Wait")]
        [DisplayName("Wait Until Contain Text")]
        [Description("Wait Until Contain Text")]
        public InArgument<string> WaitUntilContainText { get; set; }

        [Category("Output")]
        [DisplayName("Launch Result")]
        [Description("Launch Result")]
        public OutArgument<bool> LaunchResult { get; set; }
        IExplorer explorer = null;
        public Task taskA = null;
        public WatiN.Core.IE IEWATIN = null;

        [Category("Wait")]
        [DisplayName("Wait Before")]
        [Description("in milliseconds")]
        public InArgument<int> WaitBefore { get; set; }

        [Category("Wait")]
        [DefaultValue("10000")]
        [DisplayName("Wait After")]
        [Description("in milliseconds")]

        public InArgument<int> WaitAfter { get; set; }
        public IE_OpenApplication()
        {
             explorer = IExplorer.Instance;
            //explorer = new IExplorer();
            // UniqueApplicationID = DateTime.Now.ToString("yyMMddHHmmss");
        }
        //public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        //{
        //    try
        //    {
        //        string LaunchUrl = string.Empty;
        //        string ApplicationId = string.Empty;
        //        int iTimeInSec = 0;
        //        bool launchResult = false;
        //        var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
        //        IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, System.Activities.Activity));

        //        foreach (ModelItem item in modelItems)
        //        {
        //            if (item.ItemType.FullName == "InternetExplorer.IEOpenApplication")
        //            {
        //                IE_OpenApplication owa = (IE_OpenApplication)item.GetCurrentValue();
        //                ApplicationId = owa.ApplicationID.Expression.ToString();
        //                if (ApplicationId == e.ApplicationID)
        //                {
        //                    LaunchUrl = owa.Url.Expression.ToString();
        //                    // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
        //                    iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
        //                    launchResult = explorer.LaunchIE(string.Empty, LaunchUrl, iTimeInSec, launchResult);
        //                }
        //            }
        //        }
        //    }catch (Exception ex)
        //    {
        //        Log.Logger.LogData(ex.Message, LogLevel.Error);
        //    }
           
        //}
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                bool launchResult = false;

                string LaunchUrl = string.Empty;
                string AppId = string.Empty;
                string strWaitUntilContainText = string.Empty;
                int iTimeInSec = 0;
                AppId = ApplicationID.Get(context);
              
                LaunchUrl = Url.Get(context);
                
                iTimeInSec = TimeOutInSecond.Get(context);
                if (0 == iTimeInSec) {
                    iTimeInSec = 10000;
                }
                if (WaitUntilContainText.Get(context) != null)
                {
                    strWaitUntilContainText = WaitUntilContainText.Get(context).ToString();
                }
                int waitBefore = WaitBefore.Get(context);
                if (waitBefore > 0)
                {
                    Thread.Sleep(waitBefore);
                }
                //SelectHelper.CurrentRuntimeApplicationUniqueId = UniqueApplicationID;
                launchResult = explorer.LaunchIE(AppId, LaunchUrl, iTimeInSec, launchResult, true, strWaitUntilContainText);
                LaunchResult.Set(context, launchResult);
                int waitAfter = WaitAfter.Get(context);

                //if (waitAfter == null)
                //{
                //    waitAfter = 10000; //Default Value
                //}

                if (waitAfter > 0)
                {
                    Thread.Sleep(waitAfter);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity IE_OpenApplication", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OpenWebApplication() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
