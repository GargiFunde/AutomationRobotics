namespace MRUModelLib.Models.Persistables
{
    /// <summary>
    /// Implements an interface for converting:
    /// 1) a model into a pojo object
    /// 2) a pojo object into a model
    /// 
    /// the pojo object can be used for data transport or XML serialization/de-serialization.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// Returns the current model in an XML persistable fashion.
        /// </summary>
        /// <returns></returns>
        MRUListPersistable GetObjectForPersistence();

        /// <summary>
        /// Set current object states from an object that represents
        /// the persisted values (from standard XML serialization).
        /// </summary>
        /// <returns></returns>
        void SetObjectFromPersistence(MRUListPersistable persist);
    }

    /// <summary>
    /// Implements an interface for converting:
    /// 1) a model into a pojo object
    /// 2) a pojo object into a model
    /// 
    /// the pojo object can be used for data transport or XML serialization/de-serialization.
    /// </summary>
    public interface IPersistableItem
    {
        /// <summary>
        /// Returns the current model in an XML persistable fashion.
        /// </summary>
        /// <returns></returns>
        MRUEntryPersistable GetObjectForPersistence();

        /// <summary>
        /// Set current object states from an object that represents
        /// the persisted values (from standard XML serialization).
        /// </summary>
        /// <returns></returns>
        void SetObjectFromPersistence(MRUEntryPersistable persist);
    }
}
