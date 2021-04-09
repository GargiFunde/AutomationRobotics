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
    /// Execute a query that modifies the database (insert, update, delete, etc.)
    /// </summary>
    [ToolboxBitmap("Resources/DbUpdate.png")]
    [Designer(typeof(DbUpdate_ActivityDesigner))]
    public class DbUpdate: AsyncCodeActivity
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
        [CategoryAttribute("Connection Configuration")]
        [DefaultValue(null)]
        public InArgument<string> ConfigName { get; set; }

        [DefaultValue(null)]
        public CommandType CommandType { get; set; }

        [RequiredArgument]
        public InArgument<string> Sql { get; set; }

        [DependsOn("Sql")]        
        [DefaultValue(null)]
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

        [DependsOn("Parameters")]
        public OutArgument<int> AffectedRecords { get; set; }     

        public DbUpdate()
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
            Func<int> action = () => dbHelper.Execute();
            context.UserState = action;

            return action.BeginInvoke(callback, state);            
        }
        
        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Func<int> action = (Func<int>)context.UserState;            
            int affectedRecords = action.EndInvoke(result);

            // dispose the database connection
            dbHelper.Dispose();

            // set the state
            this.AffectedRecords.Set(context, affectedRecords);
        }
    }
}
