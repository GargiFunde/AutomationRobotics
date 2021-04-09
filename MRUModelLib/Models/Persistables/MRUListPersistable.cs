namespace MRUModelLib.Models.Persistables
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements a simple MRUList pojo object that can be used for transport
    /// data functions or XML serialization/deserialization.
    /// </summary>
    public class MRUListPersistable
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public MRUListPersistable()
        {
            this.Entries = new List<MRUEntryPersistable>();
        }

        [XmlArray(ElementName = "Entries", Namespace = "MRUList")]
        [XmlArrayItem(ElementName = "Entry", Namespace = "MRUList")]
        public List<MRUEntryPersistable> Entries { get; set; }
    }
}
