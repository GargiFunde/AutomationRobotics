namespace MRUModelLib.Interfaces
{
    using MRUModelLib.Models.Persistables;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implement an interface for an object that manages MRU file entries.
    /// </summary>
    public interface IMRUList : ICloneable, IPersistable
    {
        #region properties
        /// <summary>
        /// Get/set list of MRU entries for this object.
        /// </summary>
        List<IMRUEntry> Entries { get; }
        #endregion properties

        #region methods
        /// <summary>
        /// Adds an MRU entry into the list of MRU entries.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isPinned"></param>
        /// <param name="addInSpot"></param>
        /// <returns></returns>
        void Add(string fileName, bool isPinned = false, Spot addInSpot = Spot.Last);

        /// <summary>
        /// Adds an MRU entry into the list of MRU entries.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="addInSpot"></param>
        /// <returns></returns>
        bool Add(IMRUEntry emp, Spot addInSpot = Spot.Last);

        /// <summary>
        /// Removes the first or last entry in the MRU List.
        /// </summary>
        /// <param name="spotInPosition"></param>
        void Remove(Spot spotInPosition);

        /// <summary>
        /// Remove an MRU entry by its path value.
        /// </summary>
        /// <param name="path"></param>
        void Remove(string path);

        /// <summary>
        /// Removes all MRU entries.
        /// </summary>
        /// <param name="path"></param>
        void Clear();
        #endregion methods
    }
}
