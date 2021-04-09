using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DataTableOutput_ActivityDesigner))]
    [ToolboxBitmap("Resources/DataTableOutput.png")]
    public class DataTableOutput : BaseNativeActivity
    {
        public DataTableOutput()
        {
            ViewHeader = true;
        }

        [Category("Input")]
        [DisplayName("DataTable")]
        [Description("Enter Input datatable")]
        public InArgument<System.Data.DataTable> dt { get; set; }

        [Category("Options")]
        [DisplayName("ViewHeader")]
        [Description("")]
        public Boolean ViewHeader { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<string> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                StringBuilder output = new StringBuilder();
                System.Data.DataTable indt = dt.Get(context);

                if (ViewHeader == true)
                {
                    foreach (DataColumn column in indt.Columns)
                    {
                        output.AppendFormat("{0} ", column.ColumnName);
                    }
                    output.AppendLine();

                }




                foreach (DataRow row in indt.Rows)
                {


                    foreach (DataColumn col in indt.Columns)
                    {
                        output.AppendFormat("{0} ", row[col]);
                        // Logger.Log.Logger.LogData("Output " + output.ToString(), LogLevel.Info);
                    }

                    output.AppendLine();
                    Result.Set(context, output.ToString());

                }


            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception " + e.Message, LogLevel.Error);
            }

        }

    }
}

