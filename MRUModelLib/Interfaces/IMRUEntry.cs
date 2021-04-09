namespace MRUModelLib.Interfaces
{
    /// <summary>
    /// Models the interface for one MRU entry in a list of entries.
    /// </summary>
    public interface IMRUEntry : System.ICloneable
    {
        #region properties
        /// <summary>
        /// Gets file name and path of this MRU entry.
        /// </summary>
        bool IsPinned { get; }

        /// <summary>
        /// Gets the path and filename of this entry.
        /// </summary>
        string PathFileName { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Resets the IsPinned property value according to the given parameter value.
        /// </summary>
        /// <param name="value"></param>
        void SetIsPinned(bool value);

        /// <summary>
        /// Resets the PathFileName property value according to the given parameter value.
        /// </summary>
        /// <param name="value"></param>
        void SetPathFileName(string value);
        #endregion methods
    }
}
