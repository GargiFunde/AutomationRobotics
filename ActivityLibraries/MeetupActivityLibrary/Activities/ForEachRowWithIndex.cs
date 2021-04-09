using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(ForEachRowWithIndex_ActivityDesigner))]
    [ToolboxBitmap("Resources/ForEachRowWithIndex.png")]
    public class ForEachRowWithIndex : BaseNativeActivity
    {
        public ForEachRowWithIndex()
        {
            IH = true;
        }

        [Category("Input")]
        [DisplayName("DataTable")]
        [Description("Enter input datatable")]
        public InArgument<System.Data.DataTable> dt { get; set; }

        [Category("Input")]
        [DisplayName("Row Index")]
        [Description("Enter row index")]
        public InArgument<Int32> index { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<string> Result { get; set; }

        [Category("Option")]
        [DisplayName("FirstRowAsHeader")]
        [Description("FirstRowAsHeader")]
        public Boolean IH { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            StringBuilder output = new StringBuilder();
            System.Data.DataTable indt = dt.Get(context);
            int i = index.Get(context);
            if (IH != true)
            {
                i = i - 1;
            }

            try
            {
                foreach (DataRow row in indt.Rows)
                {
                    if (i == indt.Rows.IndexOf(row))
                    {
                        foreach (DataColumn col in indt.Columns)
                        {
                            output.AppendFormat("{0} ", row[col]);
                            Logger.Log.Logger.LogData("Output " + output.ToString(), LogLevel.Info);
                        }

                        output.AppendLine();
                        Result.Set(context, output.ToString());
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Logger.Log.Logger.LogData("Exception " + e.Message, LogLevel.Error);
            }
        }
    }
}
