using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/DataTableSetCurrentRowCellValue.png")]
    [Designer(typeof(DataTableSetCurrentRowCellValue_ActivityDesigner))]
    public class DataTableSetCurrentRowCellValue : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Current Data Row*")]
        [Description("Enter Current Data Row")]
        public InArgument<DataRow> CurrentDataRow { get; set; }
           
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Column Number")]
        [Description("Enter Column Number")]
        public InArgument<int> ColumnNumber { get; set; }

        [Category("Input")]
        [DisplayName("Current Row Column Value")]
        [Description("Enter Current Row Column Value")]
        public InArgument<dynamic> CurrentRowColumnValue { get; set; }
        public DataTableSetCurrentRowCellValue()
        {
           // SelectHelper.MyProperty.Add("1", new List<System.Activities.Activity>()); 
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                int colNumber = 0;
                if (ColumnNumber != null)
                {
                    colNumber = ColumnNumber.Get(context);
                }
                DataRow dataRow = CurrentDataRow.Get(context);
                dynamic currentValue = CurrentRowColumnValue.Get(context);
                dataRow[colNumber] = currentValue;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity DataTableSetCurrentRowCellValue", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
