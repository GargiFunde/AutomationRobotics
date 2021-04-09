using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DataTableRemoveDuplicateRows_ActivityDesigner))]
    [ToolboxBitmap("Resources/DataTableRemoveDuplicateRows.png")]
    public class DataTableRemoveDuplicateRows : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("DataTable")]
        [Description("Enter input datatable")]
        public InOutArgument<System.Data.DataTable> dt { get; set; }



        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<System.Data.DataTable> Result { get; set; }




        protected override void Execute(NativeActivityContext context)
        {
            DataTable indt = dt.Get(context);
            DataTable dtRemoveDuplicate = new DataTable();

            try
            {

                Logger.Log.Logger.LogData("Rows Before dup remove " + indt.Rows.Count, LogLevel.Error);

                DataView dView = new DataView(indt);
                string[] arrColumns = indt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();


                dtRemoveDuplicate = dView.ToTable(true, arrColumns);
                Logger.Log.Logger.LogData("Rows after dup remove " + dtRemoveDuplicate.Rows.Count, LogLevel.Error);
                Result.Set(context, dtRemoveDuplicate);
            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception " + e.Message, LogLevel.Error);
            }


        }
    }
}
