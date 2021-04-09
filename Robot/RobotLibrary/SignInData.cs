// <copyright file=SignInData company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLibrary
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class SignInData
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long FieldId { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FieldRuntimeValue { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class SignInApplication
     {
        public SignInApplication()
         {
             ApplicationSignInData = new List<SignInData>();
             
         }
         [System.Xml.Serialization.XmlAttributeAttribute()]
         public int ApplicationId { get; set; }
     
         [System.Xml.Serialization.XmlElementAttribute()]
         public List<SignInData> ApplicationSignInData { get; set; }
     }
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class SignInGroup
    {
        public SignInGroup()
        {
            SignInApplicationData = new List<SignInApplication>();
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int GroupId { get; set; }
        [System.Xml.Serialization.XmlElementAttribute()]
        public List<SignInApplication> SignInApplicationData { get; set; }
    }
}
