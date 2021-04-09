// <copyright file=OpenWindowsApplication company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:15:03</date>
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

namespace Bot.Activity.Windows
{
    [ToolboxBitmap("Resources/WindowsOpenApplication.png")]
    [Designer(typeof(OpenWindowsApplication1))]
    public class Windows_OpenApplication : CodeActivity, IOpenApplicationInterface
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID")]
        [Description("Enter Application ID")]
        public InArgument<string> ApplicationID { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("EXE Path")]
        [Description("Enter EXE Path")]
        public InArgument<string> EXEPath { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Time Out In Second")]
        [Description("Enter Time Out In Second")]
        public InArgument<int> TimeOutInSecond { get; set; }

        [Category("Input")]
        [DisplayName("Launch Result")]
        [Description("Launch Result(In Boolean)")]
        public OutArgument<bool> LaunchResult { get; set; }
       
        WindowsPlugin windowsPlugin = null;
        public Windows_OpenApplication()
        {
             windowsPlugin = WindowsPlugin.Instance;
            // UniqueApplicationID = DateTime.Now.ToString("yyMMddHHmmss");
        }
        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            string exepath = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
            bool launchResult = false;
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

            foreach (ModelItem item in modelItems)
            {
                if(item.ItemType.FullName == "Windows.Windows_OpenApplication")
                {
                    Windows_OpenApplication owa =(Windows_OpenApplication) item.GetCurrentValue();
                    ApplicationId = owa.ApplicationID.Expression.ToString();
                    if (ApplicationId == e.ApplicationID)
                    {

                        if ((EXEPath != null) && (EXEPath.Expression != null))
                        {
                            exepath = owa.EXEPath.Expression.ToString();
                            iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                            launchResult = windowsPlugin.LaunchWindows(ApplicationId, exepath, iTimeInSec, launchResult);
                            SelectHelper.CurrentScrapMode = ScrapMode.Windows;
                        }
                    }
                }
            }
        }
        protected override void Execute(CodeActivityContext context)
        {
            bool launchResult = false;
            string exepath = string.Empty;
            string AppId = string.Empty;

            int iTimeInSec = 0;
            AppId = ApplicationID.Get(context);
            exepath = EXEPath.Get(context);
            
            iTimeInSec = TimeOutInSecond.Get(context);

            if ((exepath != null) && (exepath != string.Empty) &&(exepath.Length > 0))
            {
                launchResult = windowsPlugin.LaunchWindows(AppId, exepath, iTimeInSec, launchResult, true);
                LaunchResult.Set(context, launchResult);
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
