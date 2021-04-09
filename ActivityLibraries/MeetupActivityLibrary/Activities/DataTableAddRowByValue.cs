using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/DataTableAddRowByValue.png")]
    [Designer(typeof(DataTableAddRowByValue_ActivityDesigner))]
    public class DataTableAddRowByValue : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Data Table Object")]
        [Description("Enter Data Table Object")]
        public InArgument<System.Data.DataTable> DataTableObject { get; set; }

        //added new Parameter
        [Category("Input")]
        //[RequiredArgument]
        [OverloadGroup("DataRow")]
        [DisplayName("DataRow")]
        public InArgument<System.Data.DataRow> DataRow { get; set; }

        //[RequiredArgument]
        [Category("Input")]
        [DisplayName("Row Values")]
        [Description("Enter Row Values")]
        //public InArgument<string[]> RowValues { get; set; }
        public InArgument<object[]> RowValues { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            System.Data.DataTable dataTable = this.DataTableObject.Get((ActivityContext)context);
            if (this.DataRow.Get((ActivityContext)context) != null)
                dataTable.Rows.Add(this.DataRow.Get((ActivityContext)context));
            else if (this.RowValues.Get((ActivityContext)context) != null)
            {
                dataTable.Rows.Add(this.RowValues.Get((ActivityContext)context));
                this.DataTableObject.Set((ActivityContext)context, dataTable);
            }
            else {
                Logger.Log.Logger.LogData("Enter any one Parameter i.e. Either DataRow or Row Value", Logger.LogLevel.Error);
            }



            //System.Data.DataTable dtable = null;
            //string[] rowValues = RowValues.Get(context);
            //try
            //{
            //    dtable = DataTableObject.Get(context);
            //    DataRow dr = dtable.NewRow();

            //    for (int i = 0; i < rowValues.Length; i++)
            //    {
            //        dr[i] = rowValues[i];
            //    }
            //    if (dr != null)
            //    {
            //        dtable.Rows.Add(dr);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Log.Logger.LogData(ex.Message + " in activity DataTableAddRow", Logger.LogLevel.Error);
            //    if (!ContinueOnError)
            //    {
            //        context.Abort();
            //    }
            //}
        }
    }
}


