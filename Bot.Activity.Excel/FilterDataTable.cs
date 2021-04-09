using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.Excel
{
    [ToolboxBitmap("Resources/FilterDataTable.png")]
    [Designer(typeof(FilterDataTable_ActivityDesigner))]
    public class FilterDataTable : BaseNativeActivity
    {
        string expression;
        DataRow[] foundRows;
        DataTable temp;
        DataTable temp2;

        [Category("Input ")]
        [DisplayName("DataTable")]
        [Description("Set the DataTable")]
        [RequiredArgument]
        public InArgument<System.Data.DataTable> DataTable { get; set; }


        [Category("Input ")]
        [Description("[Column1 Name]='Value' or [Column2 name]='value'")]
        [DisplayName("Query")]
        [RequiredArgument]
        public InArgument<String> Query { get; set; }


        [Category("Output ")]
        [DisplayName("Result")]
        [Description("Data Table variable")]
        [RequiredArgument]
        public OutArgument<System.Data.DataTable> Result { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {

                temp = DataTable.Get(context);
                expression = Query.Get(context);
                foundRows = temp.Select(expression);

                temp2 = foundRows.CopyToDataTable();
                Result.Set(context, temp2);

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity FilterDataTable", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }
    }
}
