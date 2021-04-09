// <copyright file=CViewFields company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Entities
{
    [DataContract]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class CViewResults //: System.ComponentModel.INotifyPropertyChanged
    {
        List<CViewFields> cviewResultsAll = null;
        public CViewResults()
        {
            CViewResultsAll = new List<CViewFields>();
        }
        [DataMember]
        [System.Xml.Serialization.XmlElementAttribute()]
        public List<CViewFields> CViewResultsAll
        {
            get
            {
                return cviewResultsAll;
            }
            set
            {
                if (cviewResultsAll != value)
                {
                    cviewResultsAll = value;
                    //  FieldName = loginFieldProperties;
                    //  OnStaticPropertyChanged("cviewResultsAllProperties");
                }
            }
        }
       
    }
    [DataContract]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class CViewFields //: INotifyPropertyChanged
    {
        string cviewVal = string.Empty;
        //   ObservableCollection<CViewFields> cviewResultsAll = null;
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long ApplicationId { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ApplicationName { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CViewName { get; set; }
        [DataMember]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CViewValue
        {
            get
            {
                return cviewVal;
            }
            set
            {
                if (cviewVal != value)
                {
                    cviewVal = value;
                    //  FieldName = loginFieldProperties;
                    //cviewResultsAll = CViewResults.CViewResultsAll;
                    //CViewResults.CViewResultsAll = null;
                    //CViewResults.CViewResultsAll = cviewResultsAll;
                    // NotifyPropertyChanged("CViewValueProperties");
                }
            }
        } //for holding combobox values during switching
        //public event PropertyChangedEventHandler PropertyChanged;

        ////Required for ScrapingFieldProperties to reflect on UI after load
        //protected void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
