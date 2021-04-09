using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DataTableMerge_ActivityDesigner))]
    [ToolboxBitmap("Resources/DataTableMerge.png")]
    public class DataTableMerge : BaseNativeActivity
    {
        System.Data.DataTable destination = null;
        System.Data.DataTable source = null;

        [Category("Input")]
        [DisplayName("Source 1")]
        [Description("Set Source  DataTable")]
        [RequiredArgument]
        public InArgument<System.Data.DataTable> Source { get; set; }

        [Category("Input")]
        public MissingSchemaAction MissingSchema_Action { get; set; }

        [Category("Output")]
        [DisplayName("Destination")]
        [Description("Set Destination DataTable")]
        [RequiredArgument]
        public InArgument<System.Data.DataTable> Destination { get; set; }

        public DataTableMerge()
        {
            MissingSchema_Action = MissingSchemaAction.Add;
        }

        protected override void Execute(NativeActivityContext context)
        {
            destination = new System.Data.DataTable();
            source = new System.Data.DataTable();
            try
            {
                destination = Destination.Get(context);
                source = Source.Get(context);

                destination.Merge(source, false, MissingSchema_Action);
                Logger.Log.Logger.LogData(" Datatable merged Successfully", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception: Merging DataTable: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
