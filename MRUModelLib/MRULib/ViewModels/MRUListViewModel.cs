namespace MRULib.MRU.ViewModels
{
    using MRULib.ViewModels.Base;
    using MRUModelLib.Interfaces;
    using MRUModelLib.Models.Persistables;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// This enumeration is used to control the behaviour of pinned entries.
    /// </summary>
    public enum MRUSortMethod
    {
        /// <summary>
        /// Pinned entries are sorted and displayed at the beginning of the list or just be bookmarked
        /// and stay wehere they are in the list.
        /// </summary>
        PinnedEntriesFirst = 0,

        /// <summary>
        /// Pinned entries are just be bookmarked and stay wehere they are in the list. This can be useful
        /// for a list of favourites (which stay if pinned) while other unpinned entries are changed as the
        /// user keeps opening new items and thus, changing the MRU list...
        /// </summary>
        UnsortedFavourites = 1
    }

    /// <summary>
    /// Defines a delegate methode that is called when a recent file list entry is clicked.
    /// </summary>
    /// <param name="LoadFile"></param>
    public delegate void LoadFileCommandExecuted(string LoadFile);

    /// <summary>
    /// Implements an object that manages all properties for
    /// a list of recent file entries.
    /// </summary>
    public class MRUListViewModel : Base.BaseViewModel
    {
        #region Fields
        private MRUSortMethod mPinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst;

        private ObservableCollection<MRUEntryViewModel> mListOfMRUEntries;

        private int mMaxMruEntryCount;

        private ICommand mCopyUriCommand;
        private ICommand mRemoveItemCommand;
        private ICommand mClearItemsCommand;
        private ICommand mOpenWithWindowsAppCommand;
        private RelayCommand<object> mOpenInContainingFolderCommand;

        private string mLoadFileCommandHeader = null;
        private string mLoadFileCommandHeaderDescription = null;
        private ICommand mLoadFileCommand;
        private ICommand mPinUnpinEntryCommand;
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Construct ViewModel from Model
        /// </summary>
        /// <param name="listModel"></param>
        /// <param name="pinEntryAtHeadOfList"></param>
        public MRUListViewModel(IMRUList listModel,
                               MRUSortMethod pinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst)
            : this(listModel)
        {
            this.mPinEntryAtHeadOfList = pinEntryAtHeadOfList;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public MRUListViewModel(MRUListViewModel copy)
            : this()
        {
            if (copy == null)
                return;

            this.mPinEntryAtHeadOfList = copy.mPinEntryAtHeadOfList;

            if (copy.mListOfMRUEntries != null)
                this.mListOfMRUEntries = new ObservableCollection<MRUEntryViewModel>(copy.mListOfMRUEntries);

            this.mMaxMruEntryCount = copy.mMaxMruEntryCount;
        }

        /// <summary>
        /// Default constructor (optional from list of models)
        /// </summary>
        /// <param name="listModel"></param>
        public MRUListViewModel(IMRUList listModel = null)
        {
            this.mListOfMRUEntries = new ObservableCollection<MRUEntryViewModel>();
            this.mMaxMruEntryCount = 25;

            if (listModel != null)
            {
                foreach (var item in listModel.Entries)
                    this.AddMRUEntry(new MRUEntryViewModel(item));
            }

            this.mPinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst;
        }
        #endregion Constructor

        #region Properties
        public int MinValidMruEntryCount
        {
            get
            {
                return 5;
            }
        }

        public int MaxValidMruEntryCount
        {
            get
            {
                return 256;
            }
        }

        public int MaxMruEntryCount
        {
            get
            {
                return this.mMaxMruEntryCount;
            }

            set
            {
                if (this.mMaxMruEntryCount != value)
                {
                    if (value < this.MinValidMruEntryCount || value > this.MaxValidMruEntryCount)
                        throw new ArgumentOutOfRangeException("MaxMruEntryCount", value, "Valid values are: value >= 5 and value <= 256");

                    this.mMaxMruEntryCount = value;

                    this.RaisePropertyChanged(() => this.MaxMruEntryCount);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine whether a pinned entry is shown
        /// 1> at the beginning of the MRU list
        /// or
        /// 2> remains where it currently is.
        /// </summary>
        public MRUSortMethod PinSortMode
        {
            get
            {
                return this.mPinEntryAtHeadOfList;
            }

            set
            {
                if (this.mPinEntryAtHeadOfList != value)
                {
                    this.mPinEntryAtHeadOfList = value;
                    this.RaisePropertyChanged(() => this.PinSortMode);
                }
            }
        }

        /// <summary>
        /// Gets a list of MRU file entries for display in listbox.
        /// </summary>
        public ObservableCollection<MRUEntryViewModel> ListOfMRUEntries
        {
            get
            {
                return this.mListOfMRUEntries;
            }
        }

        /// <summary>
        /// Gets the count of the items in the MRU items list.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.mListOfMRUEntries == null)
                    return 0;

                return this.mListOfMRUEntries.Count;
            }
        }

        /// <summary>
        /// Gets/Sets the caption of the Load file command.
        /// Reset this value if the default string is not appropriate.
        /// Override this property if you want to implement your own logic.
        /// </summary>
        public virtual string LoadFileCommandHeader
        {
            get
            {
                if (mLoadFileCommandHeader != null)
                    return mLoadFileCommandHeader;

                return Localiz.Strings.STR_LOAD_FILE;
            }

            set
            {
                if (mLoadFileCommandHeader != value)
                {
                    mLoadFileCommandHeader = value;
                    this.RaisePropertyChanged(() => this.LoadFileCommandHeader);
                }
            }
        }

        /// <summary>
        /// Gets/Sets the description of the Load file command (displayed as tooltip).
        /// Reset this value if the default string is not appropriate.
        /// Override this property if you want to implement your own logic.
        /// </summary>
        public virtual string LoadFileCommandHeaderDescription
        {
            get
            {
                if (mLoadFileCommandHeaderDescription != null)
                    return mLoadFileCommandHeaderDescription;

                return Localiz.Strings.STR_LOAD_FILE_TT;
            }

            set
            {
                if (mLoadFileCommandHeaderDescription != value)
                {
                    mLoadFileCommandHeaderDescription = value;
                    this.RaisePropertyChanged(() => this.LoadFileCommandHeaderDescription);
                }
            }
        }

        /// <summary>
        /// Gets a (Load File) command that executes when a file entry is clicked.
        /// Override this property if you want to implement you own command framework.
        /// </summary>
        public virtual ICommand LoadFileCommand
        {
            get
            {
                if (this.mLoadFileCommand == null)
                    this.mLoadFileCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        if (LoadFileCommandDelegate != null)
                        {
                            LoadFileCommandDelegate(param.PathFileName);
                            return;
                        }

                        throw new Exception("Delegate command in viewmodel code is not set.");
                    });

                return this.mLoadFileCommand;
            }
        }

        /// <summary>
        /// Gets/sets the delegate method that is called when an entry is clicked.
        /// </summary>
        public LoadFileCommandExecuted LoadFileCommandDelegate { get; set; }

        /// <summary>
        /// Gets a (Load File) command that executes when a file entry is clicked.
        /// Override this property if you want to implement you own command framework.
        /// </summary>
        public virtual ICommand PinUnpinEntryCommand
        {
            get
            {
                if (this.mPinUnpinEntryCommand == null)
                    this.mPinUnpinEntryCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        this.PinUnpinEntry(!param.IsPinned, param);
                    });

                return this.mPinUnpinEntryCommand;
            }
        }

        /// <summary>
        /// Gets a command that removes the item in the
        /// parameter from the list of items.
        /// </summary>
        public ICommand RemoveItemCommand
        {
            get
            {
                if (this.mRemoveItemCommand == null)
                    this.mRemoveItemCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        this.mListOfMRUEntries.Remove(param);
                        this.RaisePropertyChanged(() => Count);
                    });

                return this.mRemoveItemCommand;
            }
        }

        /// <summary>
        /// Gets a command that clears the list of items.
        /// </summary>
        public ICommand ClearItemsCommand
        {
            get
            {
                if (this.mClearItemsCommand == null)
                    this.mClearItemsCommand = new RelayCommand<object>((p) =>
                    {
                        ClearItemsCommandExecuted();
                    });

                return this.mClearItemsCommand;
            }
        }

        /// <summary>
        /// Gets a command that copies the path of the item into the clipboard.
        /// </summary>
        public ICommand CopyUriCommand
        {
            get
            {
                if (this.mCopyUriCommand == null)
                    this.mCopyUriCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        System.Windows.Clipboard.SetText(param.PathFileName);
                    });

                return this.mCopyUriCommand;
            }
        }

        /// <summary>
        /// Gets a command that will execute the associated
        /// Windows application with the selected file.
        /// </summary>
        public ICommand OpenWithWindowsAppCommand
        {
            get
            {
                if (this.mOpenWithWindowsAppCommand == null)
                    this.mOpenWithWindowsAppCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        try
                        {
                            Process.Start(new ProcessStartInfo(param.PathFileName));
                        }
                        catch
                        {
                        }
                    });

                return this.mOpenWithWindowsAppCommand;
            }
        }

        /// <summary>
        /// Gets a command that will display the selected file in Windows Explorer folder.
        /// </summary>
        public ICommand OpenInContainingFolderCommand
        {
            get
            {
                if (this.mOpenInContainingFolderCommand == null)
                    this.mOpenInContainingFolderCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as MRUEntryViewModel;

                        if (param == null)
                            return;

                        OpenFileLocationInWindowsExplorer(param.PathFileName);
                    });

                return this.mOpenInContainingFolderCommand;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Adjust the pinned state from pinned to unpinned or vice versa
        /// depending on actual pin state.
        /// </summary>
        /// <param name="bPinOrUnPinMruEntry"></param>
        /// <param name="mruEntry"></param>
        public bool PinUnpinEntry(bool bPinOrUnPinMruEntry, MRUEntryViewModel mruEntry)
        {
            if (this.mListOfMRUEntries == null)
                return false;

            int pinnedMruEntryCount = this.CountPinnedEntries();

            // pin an MRU entry into the next available pinned mode spot
            if (bPinOrUnPinMruEntry == true)
            {
                MRUEntryViewModel e = this.mListOfMRUEntries.Single(mru => mru.IsPinned == false && mru.PathFileName == mruEntry.PathFileName);

                if (this.PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                    this.mListOfMRUEntries.Remove(e);

                e.IsPinned = true;

                if (this.PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                    this.mListOfMRUEntries.Insert(pinnedMruEntryCount, e);

                pinnedMruEntryCount += 1;
                //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                return true;
            }
            else
            {
                // unpin an MRU entry into the next available unpinned spot
                MRUEntryViewModel e = this.mListOfMRUEntries.Single(mru => mru.IsPinned == true && mru.PathFileName == mruEntry.PathFileName);

                if (this.PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                    this.mListOfMRUEntries.Remove(e);

                e.IsPinned = false;
                pinnedMruEntryCount -= 1;

                if (this.PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                    this.mListOfMRUEntries.Insert(pinnedMruEntryCount, e);

                //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                return true;
            }
        }

        /// <summary>
        /// Standard short-cut method to add a new unpinned entry from a string
        /// </summary>
        /// <param name="newEntry">File name and path file</param>
        public void AddMRUEntry(string newEntry)
        {
            if (newEntry == null || newEntry == string.Empty)
                return;

            this.AddMRUEntry(new MRUEntryViewModel()
            {
                IsPinned = false,
                PathFileName = newEntry });
        }

        /// <summary>
        /// Standard add a new entry from an existing viewmodel entry.
        /// </summary>
        /// <param name="newEntry"></param>
        public void AddMRUEntry(MRUEntryViewModel newEntry)
        {
            if (newEntry == null)
                return;

            if (this.mListOfMRUEntries == null)
                this.mListOfMRUEntries = new ObservableCollection<MRUEntryViewModel>();

            // Remove all entries that point to the path we are about to insert
            MRUEntryViewModel e = this.mListOfMRUEntries.SingleOrDefault(item => newEntry.PathFileName == item.PathFileName);

            if (e != null)
            {
                // Do not change an entry that has already been pinned -> its pinned in place :)
                if (e.IsPinned == true)
                    return;

                this.mListOfMRUEntries.Remove(e);
            }

            // Remove last entry if list has grown too long
            if (this.MaxMruEntryCount <= this.mListOfMRUEntries.Count)
                this.mListOfMRUEntries.RemoveAt(this.mListOfMRUEntries.Count - 1);

            // Add model entry in ViewModel collection (First pinned entry or first unpinned entry)
            if (newEntry.IsPinned == true)
                this.mListOfMRUEntries.Insert(0, new MRUEntryViewModel(newEntry));
            else
            {
                this.mListOfMRUEntries.Insert(this.CountPinnedEntries(), new MRUEntryViewModel(newEntry));
            }

            this.RaisePropertyChanged(() => Count);
        }

        /// <summary>
        /// Remove an entry if it points to this path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool RemoveEntry(string fileName)
        {
            if (this.mListOfMRUEntries == null)
                return false;

            MRUEntryViewModel e = this.mListOfMRUEntries.Single(mru => mru.PathFileName == fileName);

            this.mListOfMRUEntries.Remove(e);
            this.RaisePropertyChanged(() => Count);

            return true;
        }

        /// <summary>
        /// Find the MRU entry with this filePathName and return it.
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public MRUEntryViewModel FindMRUEntry(string filePathName)
        {
            if (this.mListOfMRUEntries == null)
                return null;

            return this.mListOfMRUEntries.SingleOrDefault(mru => mru.PathFileName == filePathName);
        }

        /// <summary>
        /// Count number of entries in pinnded state.
        /// </summary>
        /// <returns></returns>
        private int CountPinnedEntries()
        {
            if (this.mListOfMRUEntries != null)
                return this.mListOfMRUEntries.Count(mru => mru.IsPinned == true);

            return 0;
        }

        /// <summary>
        /// Convinience method to open Windows Explorer with a selected file (if it exists).
        /// Otherwise, Windows Explorer is opened in the location where the file should be at.
        /// </summary>
        /// <param name="oFileName"></param>
        /// <returns></returns>
        public static bool OpenFileLocationInWindowsExplorer(string sFileName)
        {
            if ((sFileName == null ? string.Empty : sFileName).Length == 0)
                return true;

            if (System.IO.File.Exists(sFileName) == true)
            {
                // combine the arguments together it doesn't matter if there is a space after ','
                string argument = @"/select, " + sFileName;

                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
                catch
                {
                }

                return true;
            }
            else
            {
                string sParentDir = System.IO.Directory.GetParent(sFileName).FullName;

                if (System.IO.Directory.Exists(sParentDir) == false)
                    return false;
                else
                {
                    // combine the arguments together it doesn't matter if there is a space after ','
                    string argument = @"/select, " + sParentDir;

                    try
                    {
                        System.Diagnostics.Process.Start("explorer.exe", argument);
                    }
                    catch
                    {
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// Clears the current list of items and replaces it with content from the
        /// persistence object.
        /// </summary>
        /// <param name="persit"></param>
        public void SetPersistable(MRUListPersistable persit)
        {
            this.ClearItemsCommandExecuted();

            foreach (var item in persit.Entries)
            {
                this.AddMRUEntry(new MRUEntryViewModel(MRUModelLib.Factory.CreateEntry(item.PathFileName, item.IsPinned)));
            }
        }

        /// <summary>
        /// Converts the current content into a persistable object collection.
        /// </summary>
        /// <returns></returns>
        public MRUListPersistable GetPersistable()
        {
            var ret = new MRUListPersistable();

            foreach (var item in GetPeristableItems())
                ret.Entries.Add(item);

            return ret;
        }

        /// <summary>
        /// Gets all items in the viemodel as persistable object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MRUEntryPersistable> GetPeristableItems()
        {
            foreach (var item in mListOfMRUEntries)
                yield return (item.GetModel() as IPersistableItem).GetObjectForPersistence();
        }

        /// <summary>
        /// Gets the entire model object graph for this viewmodel.
        /// </summary>
        /// <returns></returns>
        public IMRUList GetModelList()
        {
            IMRUList ret = MRUModelLib.Factory.CreateMRUList();

            foreach (var item in mListOfMRUEntries)
                ret.Add(item.GetModel());

            return ret;
        }

        /// <summary>
        /// Gets all items in the viemodel as model object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMRUEntry> GetModelItems()
        {
            foreach (var item in mListOfMRUEntries)
                yield return item.GetModel();
        }

        private void ClearItemsCommandExecuted()
        {
            this.mListOfMRUEntries.Clear();
            this.RaisePropertyChanged(() => Count);
        }
        #endregion Methods
    }
}
