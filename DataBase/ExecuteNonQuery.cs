using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Markup;

namespace Database
{
    [ToolboxBitmap("Resources/ExecuteNonQuery.png")]
    [Designer(typeof(ExecuteNonQueryActivityDesigner))]
    public class ExecuteNonQuery : BaseNativeActivity
    {
        private Dictionary<string, Argument> parameters;
        private SqlConnection DbConnection;

        [DefaultValue(null)]
        [Category("ConnectionConfiguration")]
        [RequiredArgument]
        [OverloadGroup("New Database Connection")]
        [DisplayName("ProviderNameDisplayName")]
        [Description("Database Connection name used within Activity")]
        public InArgument<string> ProviderName { get; set; }

        [Category("ConnectionConfiguration")]
        [DependsOn("ProviderName")]
        [DefaultValue(null)]
       // [RequiredArgument]
        [OverloadGroup("New Database Connection")]
        [DisplayName("Connection String")]
        public InArgument<string> ConnectionString { get; set; }

        [Category("ConnectionConfiguration")]
       // [RequiredArgument]
        [OverloadGroup("Existing Database Connection")]
        [DisplayName("Existing DataBase Connection")]
        public InArgument<SqlConnection> ExistingDbConnection { get; set; }

        [DefaultValue(null)]
        [DisplayName("Command Type")]
        public CommandType CommandType { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("SQL Command to be Executed")]
        public InArgument<string> Sql { get; set; }

        [Category("Input")]
        [DisplayName("Timeout(In Milli-Seconds)")]
        [Description("Specify Amount of time in MilliSecond")]
        public InArgument<int> TimeoutMS { get; set; }

        [DefaultValue(null)]
        [Category("Input")]
        [Browsable(true)]
        [DisplayName("ParametersDisplayName")]
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
        [DisplayName("AffectedRecordsDisplayName")]
        public OutArgument<int> AffectedRecords { get; set; }

        public ExecuteNonQuery()
        {
            this.CommandType = CommandType.Text;
            ConnectionString = new InArgument<string>();
        }
        protected override void Execute(NativeActivityContext context)
        {
            string connString = (string)null;
            string provName = (string)null;
            string sql = string.Empty;
            sql = this.Sql.Get((ActivityContext)context);
            this.DbConnection = this.ExistingDbConnection.Get((ActivityContext)context);
            connString = this.ConnectionString.Get((ActivityContext)context);
            provName = this.ProviderName.Get((ActivityContext)context);
            int commandTimeout = this.TimeoutMS.Get((ActivityContext)context);
            if (commandTimeout < 0)
                Logger.Log.Logger.LogData("Timout Occured", Logger.LogLevel.Info);
            Dictionary<string, Tuple<object, ArgumentDirection>> parameters = (Dictionary<string, Tuple<object, ArgumentDirection>>)null;
            try
            {
                if (this.ExistingDbConnection.Get((ActivityContext)context) != null && connString == null)
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
                DbConnection.Open();
                SqlCommand command = new SqlCommand(sql, this.DbConnection);
                
                int resultExecuteQuery = 0;

                resultExecuteQuery = command.ExecuteNonQuery();
                if (0 > resultExecuteQuery)
                {
                    Logger.Log.Logger.LogData("Query Execution Failed ", Logger.LogLevel.Error);
                }
                else
                {
                    {
                        Logger.Log.Logger.LogData("Query Execution Successfull ", Logger.LogLevel.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception While Executing Query: " + ex.Message, Logger.LogLevel.Error);
            }
        }
    }
}
