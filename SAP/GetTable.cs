using CommonLibrary;
using System;
using System.Activities;
using System.ComponentModel;
using sapfewse;
using Logger;

namespace Bot.Activity.SAP
{
    [Designer(typeof(GetTable1))]
    public class SAP_GetTable : BaseNativeActivity
    {
        [DisplayName("Attach by ApplicationID")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [DisplayName("Id")]
        [Description("Id to find Table control")]
        public InArgument<string> ControlId { get; set; }

        [DisplayName("DataTable")]
        [Description("DataTable ")]
        public OutArgument<string> Table { get; set; }


        public SAP_GetTable()
        {
           
        }
             
        protected override void Execute(NativeActivityContext context)
        {
            GuiSession SapSession = null;
            try
            {
                string controlId = ControlId.Get(context);
                string strAppIdToAttach = ApplicationIDToAttach.Get(context);
                // string AppId = getValue<string>(context, "ApplicationIDToAttach"); // Get the SomeID variable setValue(context, "SomeID", 1234); // Set SomeID to 1234 }
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strAppIdToAttach))
                {
                    SapSession = (GuiSession)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strAppIdToAttach];
                    GuiGridView guiGridView = (GuiGridView)SapSession.FindById(controlId);
                    for (int i = 0; i < guiGridView.RowCount + 1; i++)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
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
