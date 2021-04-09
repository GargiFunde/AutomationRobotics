using Bot.Activity.ActivityLibrary.Activities;
using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary
{
    [Designer(typeof(DatatableTranspose_ActivityDesigner))]
    [ToolboxBitmap("Resources/DatatableTranspose.png")]
    public class DatatableTranspose : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("DataTable")]
        [Description("Enter input datatable")]
        public InArgument<System.Data.DataTable> dt { get; set; }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("")]
        public OutArgument<System.Data.DataTable> Result { get; set; }


        protected override void Execute(NativeActivityContext context)
        {


            try
            {
                System.Data.DataTable inputTable = dt.Get(context);
                System.Data.DataTable outputTable = new System.Data.DataTable();


                outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

                // Header row's second column onwards, 'inputTable's first column taken
                foreach (DataRow inRow in inputTable.Rows)
                {
                    string newColName = inRow[0].ToString();
                    outputTable.Columns.Add(newColName);
                }

                // Add rows by looping columns        
                for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
                {
                    DataRow newRow = outputTable.NewRow();

                    // First column is inputTable's Header row's second column
                    newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                    for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                    {
                        string colValue = inputTable.Rows[cCount][rCount].ToString();
                        newRow[cCount + 1] = colValue;
                    }
                    outputTable.Rows.Add(newRow);
                }
                Result.Set(context, outputTable);


            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception " + e.Message, LogLevel.Error);
            }

        }
    }
}
