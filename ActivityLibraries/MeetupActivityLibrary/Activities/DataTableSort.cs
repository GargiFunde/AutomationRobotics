using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DataTableSort_ActivityDesigner))]
    [ToolboxBitmap("Resources/DataTableSort.png")]
    public class DataTableSort : BaseNativeActivity
    {
        public DataTableSort()
        {
            // so = order.Ascending;
        }
        public enum order
        {
            Ascending, Descending

        }

        [Category("Options")]
        [DisplayName("SortOrder")]
        public order so { get; set; }


        [Category("Input")]
        [DisplayName("DataTable")]
        [Description("Enter input datatable")]
        public InArgument<System.Data.DataTable> dt { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<System.Data.DataTable> Result { get; set; }

        [Category("Options")]
        [DisplayName("Columan Name")]
        [Description("")]
        public InArgument<string> ColName { get; set; }

        string s;

        protected override void Execute(NativeActivityContext context)
        {

            DataView dv = new DataView(dt.Get(context));
            if (order.Ascending == so)
            {
                s = "ASC";
            }
            else
            {
                s = "DESC";
            }
            String finalString = ColName.Get(context) + " " + s;
            dv.Sort = finalString;
            Result.Set(context, dv.ToTable());
        }
    }
}
