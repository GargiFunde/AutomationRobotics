using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using System.Data;
using Logger;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/DataTableForEach.png")]
    [Designer(typeof(DataTableForEach1))]
    public class DataTableForEach : BaseNativeActivity
    {
        int rowcount = 0;
        int activitycount = 0;
       

       [RequiredArgument]
        [Category("Input")]
        [DisplayName("Data Table Object")]
        [Description("Enter Data Table Object")]
        public InArgument<System.Data.DataTable> DataTableObject { get; set; }

        [Browsable(false)]
        public List<System.Activities.Activity> Activities { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Column Number")]
        [Description("Enter Column Number")]
        public int ColumnNumber { get; set; }

        [Category("Input")]
        [DisplayName("Current Data Row")]
        [Description("Enter Current Data Row")]
        public OutArgument<DataRow> CurrentDataRow { get; set; }

        [Category("Output")]
        [DisplayName("List Result")]
        [Description("Enter List Result variable")]
        public OutArgument<string> ListResult { get; set; }
        public DataTableForEach()
        {
            if (Activities == null)
            {
                Activities = new List<System.Activities.Activity>();
            }
        }
        System.Data.DataTable dtable = null;
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                dtable = DataTableObject.Get(context);
                string lstResult = string.Empty;
                rowcount = 0;
                //  activitycount = Activities.Count - 1;
                activitycount = 0;
                DataRow dataRow = dtable.Rows[rowcount];
                CurrentDataRow.Set(context, dataRow);

                lstResult = Convert.ToString(dataRow[ColumnNumber]);
                ListResult.Set(context, lstResult);
                var itemInner = Activities[activitycount];
                context.ScheduleActivity(itemInner, onCompleted);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity DataTableForEach", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

        }

        bool MoveNextRow = false;
        string lstResult = string.Empty;
        private void onCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            try
            {
                activitycount = activitycount + 1;
                
                if (MoveNextRow)
                {
                    MoveNextRow = false;
                    rowcount = rowcount + 1;
                    if (dtable.Rows.Count <= rowcount)
                    {
                        return;
                    }
                    else
                    {
                        DataRow dataRow = dtable.Rows[rowcount];
                        CurrentDataRow.Set(context, dataRow);
                        lstResult = Convert.ToString(dataRow[ColumnNumber]);
                        ListResult.Set(context, lstResult);
                        //activitycount = Activities.Count;
                        activitycount = 0;
                    }
                }
                
                // rowcount =rowcount++;
                
                if (activitycount >= Activities.Count)
                {
                    MoveNextRow = true;
                    onCompleted(context, completedInstance);
                    return;
                }
                if (activitycount == Activities.Count-1)
                {
                    MoveNextRow = true;
                }
                var itemInner = Activities[activitycount];
                context.ScheduleActivity(itemInner, onCompleted);
            }catch(Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
    }
}
