using CommonLibrary;
using CommonLibrary.Interfaces;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.ComponentModel;
using sapfewse;

namespace Bot.Activity.SAP
{
    [Designer(typeof(OpenSAPApplication1))]
    public class SAP_OpenApplication : BaseNativeActivity, IOpenApplicationInterface
    {
        private saprotwr.net.CSapROTWrapper sapROTWrapper;

        [RequiredArgument]
        public InArgument<string> ApplicationID { get; set; }

        [RequiredArgument]
        public InArgument<string> SapPath { get; set; }
        
        [RequiredArgument]
        public InArgument<string> ConnectionString { get; set; }

        //[RequiredArgument]
        //public InArgument<string> CompanyName { get; set; }

        //[RequiredArgument]
        //public InArgument<string> CompanyCode { get; set; }

        //[RequiredArgument]
        //public InArgument<string> ClientId { get; set; }

        //[RequiredArgument]
        //public InArgument<string> UserName { get; set; }

        //[RequiredArgument]
        //public InArgument<string> Password { get; set; }     

        [RequiredArgument]
        public InArgument<string> SAPROTEntry { get; set; }

        //[RequiredArgument]
        //public InArgument<string> Environment { get; set; }
        [RequiredArgument]
        public InArgument<int> TimeOutInSecond { get; set; }

        //[RequiredArgument]
        //public InArgument<string> Language { get; set; }
        
        public OutArgument<bool> LaunchResult { get; set; }
        
        private GuiMainWindow SapMainWindow { get; set; }
        private IntPtr SapWindowHandle { get; set; }

        [Browsable(false)]
        GuiApplication SapGuiApp { get; set; }
        [Browsable(false)]
        GuiConnection SapConnection { get; set; }
        [Browsable(false)]
        GuiSession SapSession { get; set; }

        public SAPEntity sapEntity = null;
        SAPPlugin sapPlugin = null;

        public SAP_OpenApplication()
        {
             sapPlugin = SAPPlugin.Instance;
             SAPROTEntry = "SAPGUI";
             SapPath = "C:\\Program Files (x86)\\SAP\\FrontEnd\\SAPgui\\saplogon.exe";
            TimeOutInSecond = 30;
        }
        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            string environment = string.Empty;
            string connectionString = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
            bool launchResult = false;

            
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

            sapEntity = new SAPEntity();
            sapEntity.AppId = ApplicationID.Expression.ToString();
            sapEntity.sapPath = SapPath.Expression.ToString();//  "C:\\Program Files (x86)\\SAP\\FrontEnd\\SAPgui\\saplogon.exe";
            sapEntity.rotEntry = SAPROTEntry.Expression.ToString();
            sapEntity.connectString = ConnectionString.Expression.ToString();
            //sapEntity.userName = UserName.Expression.ToString();
            //sapEntity.pwd = Password.Expression.ToString();
            //sapEntity.clientid = ClientId.Expression.ToString();
            //sapEntity.companyname = CompanyName.Expression.ToString();
            //sapEntity.companycode = CompanyCode.Expression.ToString();
            sapEntity.iTimeInSec = Convert.ToInt32(TimeOutInSecond.Expression.ToString());
            //sapEntity.language = Language.Expression.ToString();


            foreach (ModelItem item in modelItems)
            {
                if (item.ItemType.FullName == "Bot.Activity.SAP.SAP_OpenApplication")
                {
                    SAP_OpenApplication owa = (SAP_OpenApplication)item.GetCurrentValue();
                    owa.sapEntity = sapEntity;
                    ApplicationId = owa.ApplicationID.Expression.ToString();
                    if (ApplicationId == e.ApplicationID)
                    {
                        
                        launchResult = sapPlugin.LaunchSap(sapEntity, launchResult);
                    }
                }
            }
        }

       
        protected override void Execute(NativeActivityContext context)
        {
            bool launchResult = false;
            try
            {
              
                sapEntity = new SAPEntity();
                sapEntity.AppId = ApplicationID.Get(context);
                sapEntity.sapPath = SapPath.Get(context);//  "C:\\Program Files (x86)\\SAP\\FrontEnd\\SAPgui\\saplogon.exe";
                sapEntity.rotEntry = SAPROTEntry.Get(context);
                sapEntity.connectString = ConnectionString.Get(context);
                //sapEntity.userName = UserName.Get(context);
                //sapEntity.pwd = Password.Get(context);
                //sapEntity.clientid = ClientId.Get(context);
                //sapEntity.companyname = CompanyName.Get(context);
                //sapEntity.companycode = CompanyCode.Get(context);
                sapEntity.iTimeInSec = TimeOutInSecond.Get(context);
               // sapEntity.language = Language.Get(context);
               
             

                sapPlugin.LaunchSap(sapEntity, launchResult, true);

                //LaunchResult.Set(context,SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null) as GuiApplication);
               
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity OpenSAPApplication", Logger.LogLevel.Error);
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
