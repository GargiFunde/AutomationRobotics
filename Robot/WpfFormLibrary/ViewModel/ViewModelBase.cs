using System;
using System.ComponentModel;
using System.Diagnostics;

namespace ADR_Library.ViewModel
{
    public abstract class ViewModelBase :INotifyPropertyChanged, IDisposable
    {
        protected bool _disposed; 

        protected ViewModelBase()
        {
            ThrowOnInvalidPropertyName = true;

        }

        ~ViewModelBase()
        {
            //logger.Info(String.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode()));
            Debug.WriteLine(String.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode()));
            Dispose(false);
        }


        /// <summary>
        /// Gets a value indicating whether ThrowOnInvalidPropertyName.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName
        {
            get; private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public virtual string DisplayName
        {
            get; protected set;
        }

        protected void ReportException(Exception exc, String method)
        {
            System.Diagnostics.Trace.WriteLine("Error in class " + this.GetType().Name + " method " + method + " => " + exc.ToString());
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }
        #region Disposable Methods

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!_disposed)
        {
            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.                
               
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
           

            // Note disposing has been done.
            _disposed = true;
        }
    }

        #endregion

    }
}
