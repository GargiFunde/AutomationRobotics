namespace MRUModelLib.Models.Persistables
{
    /// <summary>
    /// Implements a simple MRUList pojo object that can be used for transport
    /// data functions or XML serialization/deserialization.
    /// </summary>
    public class MRUEntryPersistable
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public MRUEntryPersistable()
        {
            PathFileName = string.Empty;
            IsPinned = false;
        }

        /// <summary>
        /// Gets file name and path of this MRU entry.
        /// </summary>
        public string PathFileName { get; set; }

        /// <summary>
        /// Gets whether this entry is pinned or not.
        /// </summary>
        public bool IsPinned { get; set; }
    }
}
