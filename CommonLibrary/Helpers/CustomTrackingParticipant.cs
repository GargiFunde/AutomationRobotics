// <copyright file=CustomTrackingParticipant company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities.Tracking;
using System.IO;

namespace CommonLibrary.Helpers
{

    /// <summary>
    /// Workflow Tracking Participant - Custom Implementation
    /// </summary>
   public class CustomTrackingParticipant : TrackingParticipant
    {
        public string TrackData = String.Empty;

        /// <summary>
        /// Appends the current TrackingRecord data to the Workflow Execution Log
        /// </summary>
        /// <param name="trackRecord">Tracking Record Data</param>
        /// <param name="timeStamp">Timestamp</param>
        protected override void Track(TrackingRecord trackRecord, TimeSpan timeStamp)
        {
            ActivityStateRecord recordEntry = trackRecord as ActivityStateRecord;

            if (recordEntry != null)
            {
                TrackData += String.Format("[{0}] [{1}] [{2}]" + Environment.NewLine, recordEntry.EventTime.ToLocalTime().ToString(), recordEntry.Activity.Name, recordEntry.State);
        }
        }
    }
}
