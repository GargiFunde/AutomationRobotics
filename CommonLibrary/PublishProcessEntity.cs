// <copyright file=PublishProcessEntity company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System.Runtime.Serialization;

namespace CommonLibrary
{
    [DataContract]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class PublishProcessEntity
    {
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string processname { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte[] processfiles { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string createdby { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string createddate { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string updatedby { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string updateddate { get; set; }
    }
}
