using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Database
{
    [ToolboxBitmap("Resources/Execute_Query.png")]
    [Designer(typeof(ExecuteQueryActivityDesigner))]
    public class Execute_Query : BaseNativeActivity
    {
        private Dictionary<string, Argument> parameters;
        private SqlConnection DbConnection;

        //[DefaultValue(null)]
        [Category("ConnectionConfiguration")]
        //[RequiredArgument]
        //[OverloadGroup("New Database Connection")]
        [DisplayName("Provider Name")]
        [Description("Name of DataBase Provider")]
        public InArgument<string> ProviderName { get; set; }

        //[RequiredArgument]
        //[DependsOn("ProviderName")]
        //[DefaultValue(null)]
        [Category("ConnectionConfiguration")]
        //[OverloadGroup("New Database Connection")]
        [DisplayName("Connection String")]
        [Description("Connection used to Establish a DataBase Connection")]
        public InArgument<string> ConnectionString { get; set; }

       // [RequiredArgument]
        [Category("ConnectionConfiguration")]
        //[OverloadGroup("Existing Database Connection")]
        [DisplayName("Existing DataBase Connection")]
        [Description("Already opened DataBase Connection Object")]
        public InArgument<SqlConnection> ExistingDbConnection { get; set; }

        [DefaultValue(null)]
        [DisplayName("Command Type Display Name")]
        public CommandType CommandType { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [DefaultValue(null)]
        [DisplayName("Sql Query")]
        [Description("SQL Command to be Executed")]
        public InArgument<string> Sql { get; set; }


        [Category("Common")]
        [DisplayName("Timeout MS Display Name")]
        public InArgument<int> TimeoutMS { get; set; }

        [DefaultValue(null)]
        [Category("Input")]
        [Browsable(true)]
        [DisplayName("Parameters Display Name")]
        public Dictionary<string, Argument> Parameters
        {
            get
            {
                if (this.parameters == null)
                    this.parameters = new Dictionary<string, Argument>();
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        [Category("Output")]
        [DisplayName("DataTable Display Name")]
        public OutArgument<System.Data.DataTable> DataTable { get; set; }

        public Execute_Query()
        {
            this.CommandType = CommandType.Text;
            ConnectionString = new InArgument<string>();
        }

        protected override void Execute(NativeActivityContext context)
        {
            {
                this.DataTable.Get((ActivityContext)context);
                string connString = (string)null;
                string provName = (string)null;
                SqlConnection sqlConnection;
                string sql = string.Empty;
                int commandTimeout = this.TimeoutMS.Get((ActivityContext)context);
                if (commandTimeout < 0)
                    Logger.Log.Logger.LogData("Connection Time Out. ", Logger.LogLevel.Info);
                Dictionary<string, Tuple<object, ArgumentDirection>> parameters = (Dictionary<string, Tuple<object, ArgumentDirection>>)null;
                connString = this.ConnectionString.Get((ActivityContext)context);
                provName = this.ProviderName.Get((ActivityContext)context);
                sql = this.Sql.Get((ActivityContext)context);
                try
                {
                    if (this.ExistingDbConnection.Get((ActivityContext)context) != null && connString==null)
                    {
                        this.DbConnection = this.ExistingDbConnection.Get((ActivityContext)context);
                    }
                    if (this.ExistingDbConnection.Get((ActivityContext)context) == null && connString != null)
                    {
                        this.DbConnection = new SqlConnection(connString);
                    }

                    


                    if (this.Parameters != null)
                    {
                        parameters = new Dictionary<string, Tuple<object, ArgumentDirection>>();
                        foreach (KeyValuePair<string, Argument> parameter in this.Parameters)
                            parameters.Add(parameter.Key, new Tuple<object, ArgumentDirection>(parameter.Value.Get((ActivityContext)context), parameter.Value.Direction));
                    }

                    SqlCommand command = new SqlCommand(sql, this.DbConnection);
                    DataTable dt = null;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);

                    this.DbConnection.Close();
                    if (ds.Tables.Count > 0)
                        dt = ds.Tables[0];

                    this.DataTable.Set((ActivityContext)context, dt);
                }
                catch (Exception ex)
                {
                    Logger.Log.Logger.LogData("Exception while getting Data: " + ex.Message, Logger.LogLevel.Info);
                }
            }
        }
    }
}
