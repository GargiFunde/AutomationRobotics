namespace MRUModelLib.Models
{
    using MRUModelLib.Models.Persistables;
    using System.Globalization;

    /// <summary>
    /// Models the object for one MRU entry in a list of entries.
    /// </summary>
    internal class MRUEntry : MRUModelLib.Interfaces.IMRUEntry, IPersistableItem
    {
        #region constructors
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public MRUEntry()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public MRUEntry(MRUEntry copyFrom)
        {
            if (copyFrom == null) return;

            this.PathFileName = copyFrom.PathFileName;
            this.IsPinned = copyFrom.IsPinned;
        }

        /// <summary>
        /// Convinience constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullTime"></param>
        public MRUEntry(string name, bool fullTime)
        {
            this.PathFileName = name;
            this.IsPinned = fullTime;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets file name and path of this MRU entry.
        /// </summary>
        public string PathFileName { get; protected set; }

        /// <summary>
        /// Gets whether this entry is pinned or not.
        /// </summary>
        public bool IsPinned { get; protected set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Standard to string method overwrite.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Path {0}, IsPinned:{1}", (this.PathFileName == null ? "(null)" : this.PathFileName),
                                                           this.IsPinned);
        }

        /// <summary>
        /// Resets the IsPinned property value according to the given parameter value.
        /// </summary>
        /// <param name="value"></param>
        public void SetIsPinned(bool value)
        {
            this.IsPinned = value;
        }

        /// <summary>
        /// Resets the PathFileName property value according to the given parameter value.
        /// </summary>
        /// <param name="value"></param>
        public void SetPathFileName(string value)
        {
            this.PathFileName = value;
        }

        /// <summary>
        /// <seealso cref="ICloneable"/>
        /// Supports cloning, which creates a new instance of a class with the same value
        /// as an existing instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new MRUEntry(this);
        }

        /// <summary>
        /// Returns the current model in an pojo transport persistable fashion.
        /// </summary>
        /// <returns></returns>
        MRUEntryPersistable IPersistableItem.GetObjectForPersistence()
        {
            var newItem = new MRUEntryPersistable()
            {
                IsPinned = this.IsPinned,
                PathFileName = this.PathFileName
            };

            return newItem;
        }

        /// <summary>
        /// Set current object states from an object that represents
        /// the persisted values (from standard pojo transport, eg.: from XML serialization).
        /// </summary>
        /// <returns></returns>
        void IPersistableItem.SetObjectFromPersistence(MRUEntryPersistable item)
        {
            this.SetPathFileName(item.PathFileName);
            this.SetIsPinned(item.IsPinned);
        }
        #endregion methods
    }
}
