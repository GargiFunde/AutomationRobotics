namespace MRUModelLib.Models
{
    using MRUModelLib.Interfaces;
    using MRUModelLib.Models.Persistables;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Models the object for the list of MRU entries.
    /// </summary>
    internal class MRUList : IMRUList
    {
        #region constructor
        /// <summary>
        /// Class constructor.
        /// </summary>
        public MRUList()
        {
            this.Entries = new List<IMRUEntry>();
        }

        /// <summary>
        /// Copy class constructor.
        /// </summary>
        public MRUList(IMRUList copySource)
            : this()
        {
            if (copySource == null)
                return;

            foreach (var item in copySource.Entries)
                this.Add(item.Clone() as IMRUEntry);
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Get/set list of MRU entries for this object.
        /// </summary>
        public List<IMRUEntry> Entries { get; protected set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Adds an MRU entry into the list of MRU entries.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isPinned"></param>
        /// <param name="addInSpot"></param>
        /// <returns></returns>
        public void Add(string fileName,
                        bool isPinned = false,
                        Spot addInSpot = Spot.Last)
        {
            Add(new MRUEntry(fileName, isPinned), addInSpot);
        }

        /// <summary>
        /// Adds an MRU entry into the list of MRU entries.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="addInSpot"></param>
        /// <returns></returns>
        public bool Add(IMRUEntry emp,
                        Spot addInSpot = Spot.Last)
        {
            // Remove all entries that point to the path we are about to insert
            var e = this.Entries.SingleOrDefault(item => emp.PathFileName == item.PathFileName);

            if (e != null)
            {
                // Do not change an entry that has already been pinned -> its pinned in place :)
                if (e.IsPinned == true)
                    return true;

                this.Remove(e.PathFileName);
            }

            if (emp == null)
                return false;

            if (this.Entries == null)
                this.Entries = new List<IMRUEntry>();

            switch (addInSpot)
            {
                case Spot.First:
                    this.Entries.Insert(0, emp.Clone() as IMRUEntry);
                    return true;

                case Spot.Last:
                    this.Entries.Add(emp.Clone() as IMRUEntry);
                    return true;

                default:
                    throw new NotImplementedException(addInSpot.ToString());
            }
        }

        /// <summary>
        /// Removes the first or last entry in the MRU List.
        /// </summary>
        /// <param name="spotInPosition"></param>
        public void Remove(Spot spotInPosition)
        {
            if (this.Entries == null)
                return;

            if (this.Entries.Count == 0)
                return;

            switch (spotInPosition)
            {
                case Spot.First:
                    this.Entries.RemoveAt(0);
                    break;

                case Spot.Last:
                    this.Entries.RemoveAt(this.Entries.Count - 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Remove an MRU entry by its path value.
        /// </summary>
        /// <param name="path"></param>
        public void Remove(string path)
        {
            if (this.Entries != null && path != null)
                this.Entries.RemoveAll(item => path == item.PathFileName);
        }

        /// <summary>
        /// Removes all MRU entries.
        /// </summary>
        public void Clear()
        {
            Entries.Clear();
        }

        /// <summary>
        /// <seealso cref="ICloneable"/>
        /// Supports cloning, which creates a new instance of a class with the same value
        /// as an existing instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new MRUList(this);
        }

        #region IPersistable Interface
        /// <summary>
        /// Returns the current model in an pojo transport persistable fashion.
        /// </summary>
        /// <returns></returns>
        MRUListPersistable IPersistable.GetObjectForPersistence()
        {
            var ret = new MRUListPersistable();

            if (Entries != null)
            {
                foreach (var item in Entries)
                {
                    var ipersist = item as IPersistableItem;
                    ret.Entries.Add(ipersist.GetObjectForPersistence());
                }
            }

            return ret;
        }

        /// <summary>
        /// Set current object states from an object that represents
        /// the persisted values (from standard pojo transport, eg.: from XML serialization).
        /// </summary>
        /// <returns></returns>
        void IPersistable.SetObjectFromPersistence(MRUListPersistable persist)
        {
            Entries.Clear();

            if (persist == null)
                return;

            foreach (var item in persist.Entries)
            {
                var entry = new MRUEntry();

                (entry as IPersistableItem).SetObjectFromPersistence(item);
                Add(entry);
            }
        }
        #endregion IPersistable Interface
        #endregion methods
    }
}
