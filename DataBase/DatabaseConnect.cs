using Logger;
using System.Activities;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;

namespace Database
{
    [ToolboxBitmap("Resources/DatabaseConnect.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class DatabaseConnect : BaseNativeActivity
    {

      public  DatabaseConnect()
        {

            ConnectionString = new InArgument<string>();
            issqlserver = new bool();
        }

      

       // [RequiredArgument]
        [Category("Connection Configuration")]
        [DefaultValue("System.Data.SqlClient")]
        [DisplayName("Provider Name")]
        [Description("Database Provider Name")]
        [Browsable(false)]
        public InArgument<string> ProviderName { get; set; }

        //[DependsOn("ProviderName")]
        [DefaultValue(null)]
        [Category("Connection Configuration")]
        
        [DisplayName("Database Connection String")]
        [Description("Database Provider Name")]
        public InArgument<string> ConnectionString { get; set; }


        [DefaultValue(null)]
        [Category("Connection Configuration")]
        [Browsable(false)]
        [DisplayName("Database Connection String")]
        [Description("Database Provider Name")]
        public bool issqlserver { get; set; }

        [Category("Output Parameter")]
        [DisplayName("Database Connection")]
        [Description("Database Connection used for Operations within Activity")]
        public OutArgument<SqlConnection> DatabaseConnection { get; set; }

       
        protected override void Execute(NativeActivityContext context)
        {
            string connString = this.ConnectionString.Get((ActivityContext)context);
           // string provName = this.ProviderName.Get((ActivityContext)context);
            SqlConnection connObject = this.DatabaseConnection.Get((ActivityContext)context);
           
            try
            {
                if (issqlserver == true)
                {
                    SqlConnection sqlConnection;
                    sqlConnection = new SqlConnection(connString);
                    //connObject = 
                    sqlConnection.Open();
                    this.DatabaseConnection.Set((ActivityContext)context, sqlConnection);
                    Logger.Log.Logger.LogData("Database Connected Successful." + sqlConnection.ToString(), LogLevel.Info);
                }
              if(issqlserver == false)
                {
                    OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connString);
                    conn.Open();
                    this.DatabaseConnection.Set((ActivityContext)context, conn);
                    Logger.Log.Logger.LogData("Database Connected Successful." + conn.ToString(), LogLevel.Info);
                }

               
               
            }
            catch (SqlException ex)
            {
                Logger.Log.Logger.LogData("Connection to Database Failed. Error : " + ex.Message, LogLevel.Error);
            }
        }
    }
}
