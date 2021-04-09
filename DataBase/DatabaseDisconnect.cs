//using System;
using System;
using System.Activities;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Logger;

namespace Database
{
    [ToolboxBitmap("Resources/DatabaseDisconnect.png")]
    [Designer(typeof(DatabaseDisconnect_ActivityDesigner))]
    public class DatabaseDisconnect : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input Parameter")]
        [DisplayName("Database Connection")]
        [Description("Database Connection name used within Activity")]
        public InArgument<SqlConnection> DatabaseConnection { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                SqlConnection sqlConn = this.DatabaseConnection.Get((ActivityContext)context);
                Action action = (Action)(() => sqlConn.Dispose());
                //sqlConn.Dispose();
                Logger.Log.Logger.LogData("Database Connection Closed Successfully.", LogLevel.Info);
            }
            catch (SqlException ex)
            {
                Logger.Log.Logger.LogData("Closing Database Connection Error : " + ex.Message, LogLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Closing Database Connection Exception : " + ex.Message, LogLevel.Error);
            }
        }
    }
}
