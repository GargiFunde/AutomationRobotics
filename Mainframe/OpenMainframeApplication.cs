// <copyright file=OpenMainframeApplication company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:58</date>
// <summary></summary>

using CommonLibrary;
using CommonLibrary.Interfaces;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Activity.Mainframe
{
    [Designer(typeof(OpenMainframeApplication1))]
    [ToolboxBitmap("Resources/OpenMainframeApplication.png")]

    public class OpenMainframeApplication : CodeActivity, IOpenApplicationInterface
    {
        [RequiredArgument]
        public InArgument<string> ApplicationID { get; set; }
        [RequiredArgument]
        public InArgument<string> LoginUrl { get; set; }
        [RequiredArgument]
        public InArgument<string> SearchUrl { get; set; }
        [RequiredArgument]
        public InArgument<int> TimeOutInSecond { get; set; }

        
       // public string UniqueApplicationID { get; set; }

        public OutArgument<bool> LaunchResult { get; set; }
       
        //IExplorer explorer = null;
        public OpenMainframeApplication()
        {
           // explorer = IExplorer.Instance;
           // UniqueApplicationID = DateTime.Now.ToString("yyMMddHHmmss");
        }
        //public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        //{
        //    string LaunchUrl = string.Empty;
        //    string ApplicationId = string.Empty;
        //    int iTimeInSec = 0;
        //    bool launchResult = false;
        //    var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
        //    IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

        //    foreach (ModelItem item in modelItems)
        //    {
        //        if(item.ItemType.FullName == "Bot.Activity.Mainframe.OpenMainframeApplication")
        //        {
        //            OpenMainframeApplication owa =(OpenMainframeApplication) item.GetCurrentValue();
        //            ApplicationId = owa.ApplicationID.Expression.ToString();
        //            if (ApplicationId == e.ApplicationID)
        //            {
        //                LaunchUrl = owa.LoginUrl.Expression.ToString();
        //                // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
        //                iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
        //               // launchResult = explorer.LaunchIE(string.Empty,LaunchUrl, iTimeInSec, launchResult);
        //            }
        //        }
        //    }
        //}
        protected override void Execute(CodeActivityContext context)
        {
            bool launchResult = false;

            string LaunchUrl = string.Empty;
            string AppId = string.Empty;

            int iTimeInSec = 0;
            AppId = ApplicationID.Get(context);
            LaunchUrl = LoginUrl.Get(context);
            if ((LaunchUrl == string.Empty) || (LaunchUrl == null) || (LaunchUrl.Length <= 0))
            {
                LaunchUrl = SearchUrl.Get(context);
            }
            iTimeInSec = TimeOutInSecond.Get(context);

           
       //     launchResult = explorer.LaunchIE(AppId, LaunchUrl, iTimeInSec, launchResult,true);
            LaunchResult.Set(context, launchResult);
            
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
