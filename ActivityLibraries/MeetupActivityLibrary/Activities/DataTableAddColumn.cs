using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using dt = System.Data;
using System.Data;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/DataTableAddColumn.png")]
    [Designer(typeof(DataTableAddColumn_ActivityDesigner))]
    public class DataTableAddColumn : BaseNativeActivity
    {
       
        
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Data Table Object*")]
        [Description("Enter Data Table Object Name")]
        public InArgument<System.Data.DataTable> DataTableObject { get; set; }

        
        [RequiredArgument]
        [TypeConverter(typeof(Collection2PropertyConverter1))]
        [Category("Input")]
        [DisplayName("Column Type")]
        [Description("Enter Column Type")]
        public string ColumnType { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Column Name")]
        [Description("Enter Column Name")]
        public InArgument<string> ColumnName { get; set; }
     
        protected override void Execute(NativeActivityContext context)
        {
            dt.DataTable db = new System.Data.DataTable();

            System.Data.DataTable dtable = null;
            try
            {
                string strcolumnName = string.Empty;
                strcolumnName = ColumnName.Get(context);
                dtable = DataTableObject.Get(context);
                DataColumn colString = new DataColumn(strcolumnName);
                if (string.IsNullOrEmpty(ColumnType))
                {
                    ColumnType = "System.String";
                }
                colString.DataType = System.Type.GetType(ColumnType);
                dtable.Columns.Add(colString);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity DataTableAddColumn", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

        }
    }
    public class Collection2PropertyConverter1 : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, but allow free-form entry
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> columnTypes = new List<string>();
            columnTypes.Add("System.String");
            columnTypes.Add("System.Int32");
            columnTypes.Add("System.Boolean");
            columnTypes.Add("System.TimeSpan");
            columnTypes.Add("System.DateTime");
            columnTypes.Add("System.Decimal");
            columnTypes.Add("System.Byte[]");
            
            return new StandardValuesCollection(columnTypes);
        }

    }
}



    


