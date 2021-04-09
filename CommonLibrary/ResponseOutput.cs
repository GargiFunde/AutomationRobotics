// <copyright file=ResponseOutput company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CommonLibrary.Entities;

namespace CommonLibrary
{
    [DataContract]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ResponseOutput
    {
        [DataMember]
        [System.Xml.Serialization.XmlElementAttribute()]
        public RequestInput requestInput { get; set; }

        [DataMember]
        [System.Xml.Serialization.XmlElementAttribute()]
        public List<CViewFields> cviewResultsAll { get; set; }
    }
}
