// <copyright file=RobotDetails company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RobotLibrary
{

    public delegate void DeleteEventHandler(object sender, RoboEventArgs e);

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
     public class RobotDetails
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RobotName { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RobotFullName { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RobotPath { get; set; }
         [XmlIgnore]
        public int RobotHeartBeatInterval { get; set; }
         [XmlIgnore]
        public long RobotId { get; set; }
         [XmlIgnore]
        public IntPtr  MainWindowHandle { get; set; }
         [XmlIgnore]
        public int ProcessId { get; set; }
         [XmlIgnore]
        public string RobotInstalledOnHost { get; set; }
         [XmlIgnore]
        public string Message { get; set; }
         [XmlIgnore]
        public bool IsLaunched { get; set; }
         [XmlIgnore]
         public bool IsSignedIn { get; set; }
         [XmlIgnore]
        public int MaxAutomationFailureAllowed { get; set; }
         [XmlIgnore]
        public long MessageExpTimeout { get; set; }
         [XmlIgnore]
        public long HeartBeatMsgExpTimeout { get; set; }

         public event DeleteEventHandler Delete;
         protected virtual void OnDelete(RoboEventArgs e)
         {
            
             if (Delete != null)
                 Delete(this, e);
         }
        public void DeleteMe()
         {
             RoboEventArgs e = new RoboEventArgs();
             e.RoboId = RobotId;
             OnDelete(e);
         }
    }
    public class RoboEventArgs:EventArgs
    {
        public long RoboId { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30219.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RobotDetailsList
    {
         [System.Xml.Serialization.XmlElementAttribute()]
        public ObservableCollection<RobotDetails> RobotsCollection { get; set; }
    }

   
}
