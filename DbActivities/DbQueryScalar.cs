//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

using Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Markup;

//<connectionStrings>    
//    <add name = "DbActivitiesSample"
//         providerName="System.Data.SqlClient"
//         connectionString="Data Source=.\SQLExpress;Initial Catalog=DbActivitiesSample;Integrated Security=true"/>
//  </connectionStrings>

namespace Microsoft.Samples.Activities.Data
{

    /// <summary>
    /// Get a scalar value (of type object) from a Database 
    /// </summary>    
    [ToolboxBitmap("Resources/DbQueryScalar.png")]
    [Designer(typeof(DbQueryScalar_ActivityDesigner))]
    public class DbQueryScalar : AsyncCodeActivity
    {
        // private variables
        IDictionary<string, Argument> parameters;
        DbHelper dbHelper;

        // public arguments
        [RequiredArgument]
        [OverloadGroup("ConnectionString")]
        [DefaultValue(null)]
        public InArgument<string> ProviderName { get; set; }

        [RequiredArgument]
        [OverloadGroup("ConnectionString")]
        [DependsOn("ProviderName")]
        [DefaultValue(null)]
        public InArgument<string> ConnectionString { get; set; }

        [RequiredArgument]
        [OverloadGroup("ConfigFileSectionName")]
        [DefaultValue(null)]
        public InArgument<string> ConfigName { get; set; }

        [DefaultValue(null)]
        public CommandType CommandType { get; set; }

        [RequiredArgument]
        public InArgument<string> Sql { get; set; }

        [DependsOn("Sql")]
        [DefaultValue(null)]

        [RequiredArgument]
        public OutArgument<object> Result { get; set; }
        public IDictionary<string, Argument> Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new Dictionary<string, Argument>();
                }
                return this.parameters;
            }
        }

        public DbQueryScalar()
        {
            this.CommandType = CommandType.Text;
            ProviderName = "System.Data.SqlClient";
        }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            // configure the helper object to access the database
            dbHelper = new DbHelper();
            dbHelper.ConnectionString = this.ConnectionString.Get(context);
            dbHelper.ProviderName = this.ProviderName.Get(context);
            dbHelper.ConfigName = this.ConfigName.Get(context);
            dbHelper.Sql = this.Sql.Get(context);
            dbHelper.CommandType = this.CommandType;
            dbHelper.Parameters = this.parameters;
            dbHelper.Init(context);

            // create the action for doing the actual work
            Func<object> action = () => dbHelper.ExecuteScalar<object>();
            context.UserState = action;

            return action.BeginInvoke(callback, state);
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Func<object> action = (Func<object>)context.UserState;
            object scalar = action.EndInvoke(result);

            // dispose the database connection
            dbHelper.Dispose();

            // return the state
            Result.Set(context, scalar);         	
        }
    }
}
