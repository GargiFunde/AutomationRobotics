// <copyright file=VisualTrackingParticipant company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:47</date>
// <summary></summary>

using Logger;
using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;

namespace BOTDesigner
{
    public class VisualTrackingParticipant : System.Activities.Tracking.TrackingParticipant
    {
        public event EventHandler<TrackingEventArgs> TrackingRecordReceived;
        public Dictionary<string, Activity> ActivityIdToWorkflowElementMap { get; set; }


        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            OnTrackingRecordReceived(record, timeout);
        }

        //On Tracing Record Received call the TrackingRecordReceived with the record received information from the TrackingParticipant. 
        //We also do not worry about Expressions' tracking data
        protected void OnTrackingRecordReceived(TrackingRecord record, TimeSpan timeout)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(
                    String.Format("Tracking Record Received: {0} with timeout: {1} seconds.", record, timeout.TotalSeconds)
                );

                if (TrackingRecordReceived != null)
                {
                    ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
                    ActivityScheduledRecord activityScheduledRecord = record as ActivityScheduledRecord;
                    
                    if (((activityStateRecord != null) && (!activityStateRecord.Activity.TypeName.Contains("System.Activities.Expressions")))||(activityScheduledRecord!=null))
                    {
                        string activId = null;
                        if (activityStateRecord != null)
                        {
                            activId = activityStateRecord.Activity.Id ;
                        }
                        else if (activityScheduledRecord != null)
                        {
                            activId = activityScheduledRecord.Child.Id;
                        }

                        if ((ActivityIdToWorkflowElementMap.ContainsKey(activId)))
                        {
                            
                            TrackingRecordReceived(this, new TrackingEventArgs(
                                                            record,
                                                            timeout,
                                                            ActivityIdToWorkflowElementMap[activId]
                                                            )

                                );
                        }

                    }
                    else
                    {
                        TrackingRecordReceived(this, new TrackingEventArgs(record, timeout, null));
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

        }
    }
    //Custom Tracking EventArgs
    public class TrackingEventArgs : EventArgs
    {
        public TrackingRecord Record { get; set; }
        public TimeSpan Timeout { get; set; }
        public Activity Activity { get; set; }

        public TrackingEventArgs(TrackingRecord trackingRecord, TimeSpan timeout, Activity activity)
        {
            this.Record = trackingRecord;
            this.Timeout = timeout;
            this.Activity = activity;
        }
    }
}
