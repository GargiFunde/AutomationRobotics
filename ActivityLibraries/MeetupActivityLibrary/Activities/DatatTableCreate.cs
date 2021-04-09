using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using DT = System.Data;
using System.IO;
using System.Drawing;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(DataTableCreate_ActivityDesigner))]
    [ToolboxBitmap("Resources/DataTableCreate.png")]
    public class DataTableCreate : BaseNativeActivity
    {

        [Category("Output Parameters")]
        [DisplayName("Data Table")]
        [Description("Get Result Data Table")]
        [RequiredArgument]
        public OutArgument<DT.DataTable> PropDataTable { get; set; }
       
        //[Category("Output Paramaters")]
        //[DisplayName("String Info")]
        //[Description("Get Result Data Table")]
        //public OutArgument<string> msgString { get; set; }
        [Category("Output Parameters")]
        [DisplayName("Data Table Info")]
        [Description("Get Result Data Table")]
        [Browsable(false)]
        public InArgument<string> TableInfo { get; set; }

        //added by ak
        //public Type TypeArgu { set; get; }
        //public static string aaaa { set; get; }

        //public static DataTableCreate curractivity;
      

        //public static CodeActivityContext currcontext;
        //protected internal bool DesignMode { get; }

        public DataTableCreate()
        {
            //curractivity = this;
           
        }

        //public static DataTableCreate ReturnDataTableCreate()
        //{
        //    return curractivity;
        //    //aaaa = dtb.ToString();

        //}
        //public static void SetProp(DataTable dtb)
        //{

        //    //aaaa = dtb.ToString();

        //}
        //protected override void UpdateInstance(NativeActivityUpdateContext updateContext)
        //{
        //    updateContext.SetValue(TableInfo,"Hello World!!");
          
        //}

        //protected DataTable RebuildDataTable(string dtstring)
        //{
        //    DataTable dt = new DataTable();



        //    return dt;
        //}
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                

                //String ColName = null;
                ////DataType
                //bool allowNull = false;

                

                //if (!ContinueOnError)
                //{
                //    context.Abort();
                //}
                //DataTableCreateForm dataTableCreateForm = new DataTableCreateForm();
                //DT.DataTable TableDataCreateForm = dataTableCreateForm.fromDataCreatedForm();

                //DT.DataTable TableData = TableDataCreateForm.Copy();

                if (context != null)
                {

                    //DT.DataRow firstRow = TableData.NewRow();
                    //List<string> names = new List<string>();
                    //foreach (DT.DataColumn column in TableData.Columns)
                    //{
                    //    names.Add(column.ColumnName);
                    //}
                    //firstRow.ItemArray = names.ToArray();
                    //TableData.Rows.InsertAt(firstRow, 0);
                    //PropDataTable.Set(context, TableData);




                    //TableInfo.Set(context,"Data Table A , Col A , Col B");
                    //TableInfo.Set();
                    string DataTableAsString = TableInfo.Get(context);
                    //DataTable temp2 = JsonConvert.DeserializeObject<DataTable>(DataTableAsString);



                    StringReader reader = null;
                    try
                    {

                        reader = new StringReader(DataTableAsString);
                        DataTable temp = new DataTable();
                        temp.ReadXml(reader);
                        PropDataTable.Set(context, temp);

                    }
                    catch (Exception e)
                    {
                        Logger.Log.Logger.LogData(e.Message + " in activity DataTableCreate", LogLevel.Error);
                        if (!ContinueOnError)
                        {
                            context.Abort();
                        }
                    }
                    finally
                    {
                        reader.Dispose();

                    }



                 
                
                }
       
                //TableData.Dispose();
              
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity DataTableCreate", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }

            }
           
           

        }



       



    }
}
