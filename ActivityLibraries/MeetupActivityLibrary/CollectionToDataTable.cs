using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/CollectionToDataTable.png")]
    [Designer(typeof(CollectionToDataTableDesigner))]
    public class CollectionToDataTable : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Column Name")]
        [Description("Set Column Name")]
        public InArgument<string> ColumnName { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Collection")]
        [Description("Enter the Collection variable")]
        public InArgument<ICollection<string>> CollectionVariable { get; set; }

       
        [Category("Output")]
        [DisplayName("DataTable")]
        [Description("Store in DataTable variable")]
        public OutArgument<DataTable> DataTableVariable { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {  
                ICollection<string> collect = CollectionVariable.Get(context);
                string colname = ColumnName.Get(context); 
                DataTable dt = new DataTable();
                dt.Columns.Add(colname, System.Type.GetType("System.String"));
                foreach (var item in collect)
                {
                  DataRow dr =  dt.NewRow();
                    dr[0] = item;
                    dt.Rows.Add(dr);
                }
                
                DataTableVariable.Set(context, dt);
            }
            catch (Exception ex)
            {

                Logger.Log.Logger.LogData("CollectionToDataTable Activity failed with Exception : "+ex,Logger.LogLevel.Error);
            }
            
        }

     
    }

  
}
