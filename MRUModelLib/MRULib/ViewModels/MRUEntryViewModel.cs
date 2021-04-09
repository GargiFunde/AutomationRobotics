namespace MRULib.MRU.ViewModels
{
    using MRUModelLib.Interfaces;
    using MRUModelLib.Models;

    /// <summary>
    /// Implements an object that manages all properties for
    /// 1 recent file entry.
    /// </summary>
    public class MRUEntryViewModel : Base.BaseViewModel
    {
        #region fields
        private IMRUEntry mMRUEntry;
        #endregion fields

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MRUEntryViewModel()
        {
            this.mMRUEntry = MRUModelLib.Factory.CreateEntry("");
            this.IsPinned = false;
        }

        /// <summary>
        /// Constructor from model
        /// </summary>
        /// <param name="model"></param>
        public MRUEntryViewModel(IMRUEntry model)
            : this()
        {
            if (model != null)
                this.mMRUEntry = model.Clone() as IMRUEntry;
            else
                this.mMRUEntry = MRUModelLib.Factory.CreateEntry("");
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copySource"></param>
        public MRUEntryViewModel(MRUEntryViewModel copySource)
            : this()
        {
            this.mMRUEntry = copySource.mMRUEntry.Clone() as IMRUEntry;
            this.IsPinned = copySource.IsPinned;
        }
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Gets the complete path of this viewmodel item.
        /// </summary>
        public string PathFileName
        {
            get
            {
                return this.mMRUEntry.PathFileName;
            }

            set
            {
                if (this.mMRUEntry.PathFileName != value)
                {
                    this.mMRUEntry.SetPathFileName(value);
                    this.RaisePropertyChanged(() => this.PathFileName);
                    this.RaisePropertyChanged(() => this.DisplayPathFileName);
                }
            }
        }

        /// <summary>
        /// Gets a partial path of this viewmodel item.
        /// </summary>
        public string DisplayPathFileName
        {
            get
            {
                if (this.mMRUEntry == null)
                    return string.Empty;

                if (this.mMRUEntry.PathFileName == null)
                    return string.Empty;

                int n = 20;
                return (this.mMRUEntry.PathFileName.Length > n && (this.mMRUEntry.PathFileName.Length <= (n + 4) == false) ?
                        this.mMRUEntry.PathFileName.Substring(0, 3) + "... " + this.mMRUEntry.PathFileName.Substring(this.mMRUEntry.PathFileName.Length - n)
                      : this.mMRUEntry.PathFileName);
            }
        }

        /// <summary>
        /// Gets whether the item is currently pinned or not.
        /// </summary>
        public bool IsPinned
        {
            get
            {
                return this.mMRUEntry.IsPinned;
            }

            set
            {
                if (this.mMRUEntry.IsPinned != value)
                {
                    this.mMRUEntry.SetIsPinned(value);
                    this.RaisePropertyChanged(() => this.IsPinned);
                }
            }
        }

        /// <summary>
        /// Gets a model entry copy for this entry.
        /// </summary>
        /// <returns></returns>
        internal IMRUEntry GetModel()
        {
            return MRUModelLib.Factory.CreateEntry(this.PathFileName, this.IsPinned);
        }
        #endregion Properties
    }
}
