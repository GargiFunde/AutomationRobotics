namespace MRUModelLib
{
    using MRUModelLib.Interfaces;
    using MRUModelLib.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Determines whether an MRU entry is added in last or first spot
    /// in list of MRU entries.
    /// </summary>
    public enum Spot
    {
        First = 0,
        Last = 1
    }

    /// <summary>
    /// Factory creates new objects from default constructors.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Get a new settingsmanager object.
        /// </summary>
        /// <returns></returns>
        public static IMRUList CreateMRUList()
        {
            return new MRUList();
        }

        /// <summary>
        /// Model item constructor from parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullTime"></param>
        public static IMRUEntry CreateEntry(string name, bool fullTime = false)
        {
            return new MRUEntry(name, fullTime);
        }
    }
}
