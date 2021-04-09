//#region Headers
//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using log4net;
//using log4net.Appender;
//using log4net.Repository.Hierarchy;
//using log4net.Layout;
//using log4net.Core;
//using System.Reflection;
//using System.Timers;
//using System.Net;
//using System.Collections.Specialized;
//using System.Data;
//using System.Configuration;
//using System.ComponentModel;
//#endregion

//namespace Logger
//{
//    public class Log : INotifyPropertyChanged
//    {
//        System.Timers.Timer aTimer = null;
//        private static object SysLock = new object();
//        public DataTable DatatableLog { get; set; }
//        // DataTable DatatableLog = null;
//        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
//        MemoryAppender mappender = null;
//        string strLogToDB = string.Empty;
//        string strLogToELK = string.Empty;
//        string strLogstash = string.Empty;
//        string strMode = string.Empty;
//        int groupid = 0;
//        int tenantid = 0;


//        //string strLogLevel = string.Empty;
//        ServiceReference1.BOTServiceClient bOTServiceClient = null; 
//        private Log()
//        {
//            //strLogToDB = ConfigurationManager.AppSettings["LogToDB"];
//            //strLogToELK = ConfigurationManager.AppSettings["LogToELK"];
//            //strLogstash = ConfigurationManager.AppSettings["LogstashUrl"];
//            //strMode = ConfigurationManager.AppSettings["Mode"];
//            ////strLogLevel = ConfigurationManager.AppSettings["LogLevel"];
//            //string strtxtLog = ConfigurationManager.AppSettings["TxtFileLogSwitch"];

//            //Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

//            //PatternLayout patternLayout = new PatternLayout();
//            //patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
//            //patternLayout.ActivateOptions();

//            ////if (string.IsNullOrEmpty(strLogLevel) || strLogLevel.Trim().Length == 0)
//            ////{
//            ////    hierarchy.Root.Level = Level.Error; //default level
//            ////}
//            ////else
//            ////{
//            ////    if (strLogLevel.ToLower().Trim() == "error")
//            ////    {
//            ////        hierarchy.Root.Level = Level.Error;
//            ////    }
//            ////    else if (strLogLevel.ToLower().Trim() == "warning")
//            ////    {
//            ////        hierarchy.Root.Level = Level.Warn;
//            ////    }
//            ////    else if (strLogLevel.ToLower().Trim() == "info")
//            ////    {
//            ////        hierarchy.Root.Level = Level.Info;
//            ////    }
//            ////    else if (strLogLevel.ToLower().Trim() == "fatal")
//            ////    {
//            ////        hierarchy.Root.Level = Level.Fatal;
//            ////    }
//            ////}
//            //if ((!(string.IsNullOrEmpty(strtxtLog))) && (strtxtLog.ToLower().Trim() == "on"))
//            //{
//            //    string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
//            //    filePath = filePath + @"\E2EBot\Logs\AutomationLog.txt";
//            //    RollingFileAppender roller = new RollingFileAppender();
//            //    roller.AppendToFile = false;
//            //    roller.File = filePath;// @"Logs\EventLog.txt";
//            //    roller.Layout = patternLayout;
//            //    roller.MaxSizeRollBackups = 5;
//            //    roller.MaximumFileSize = "1GB";
//            //    roller.RollingStyle = RollingFileAppender.RollingMode.Size;
//            //    roller.StaticLogFileName = true;
//            //    roller.ActivateOptions();
//            //    hierarchy.Root.AddAppender(roller);
//            //}
//            //if (((strLogToDB != string.Empty) && (strLogToDB.ToLower().Trim() == "true")) || (strMode == "Designer"))
//            //{
//            //    DatatableLog = new DataTable();

//            //    DatatableLog.Columns.Add("Machine", typeof(System.String));
//            //    DatatableLog.Columns.Add("UserName", typeof(System.String));
//            //    DatatableLog.Columns.Add("BotName", typeof(System.String));
//            //    DatatableLog.Columns.Add("ProcessName", typeof(System.String));
//            //    DatatableLog.Columns.Add("TimeStampValue", typeof(System.DateTime));
//            //    DatatableLog.TableName = "LogData";
//            //    DatatableLog.Columns.Add("Logger", typeof(System.String));
//            //    DatatableLog.Columns.Add("MessageValue", typeof(System.String));
//            //    DatatableLog.Columns.Add("LogLevel", typeof(System.String));
//            //    DatatableLog.Columns.Add("GroupId", typeof(System.Int32));
//            //    DatatableLog.Columns.Add("TenantId", typeof(System.Int32));
//            //}

//            //hierarchy.Configured = true;

//            //Task taskA = new Task(() => StartLogger());
//            //// Start the task.
//            //taskA.Start();


//        }

//        private static Log logger = null;
//        public static Log Logger
//        {
//            get
//            {
//                if (logger == null)
//                {
//                    lock (SysLock)
//                    {
//                        logger = new Log();
//                    }
//                }
//                return logger;
//            }

//        }
//        private static string botname = null;
//        public static string TenantName { get; set; }
//        public static string BotName { get; set; }
//        public static string ProcessName { get; set; }
//        public static string RequestId { get; set; }
//        public static int GroupId { get; set; }
//        public static int TenantId { get; set; }

//        int interval = 0;
//        public void StartLogger()
//        {

//            try
//            {
//                aTimer = new System.Timers.Timer();

//                string strinterval = ConfigurationManager.AppSettings["LogInterval(ms)"];
//                if ((strinterval != null) && (strinterval != string.Empty) && (strinterval.Length > 0))
//                {
//                    interval = Convert.ToInt32(ConfigurationManager.AppSettings["LogInterval(ms)"]);
//                }
//                else
//                {
//                    interval = 1000;
//                }
//                if (interval != 0)  
//                {
//                    Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
//                    mappender = hierarchy.Root.GetAppender("MemoryAppender") as MemoryAppender;
//                    if (strMode.ToLower() == "designer")
//                    {

//                    }
//                    else
//                    {
//                        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
//                        aTimer.Interval = interval;
//                        aTimer.Enabled = true;
//                    }
//                }

//            }
//            catch (Exception ex)
//            {

//            }
//        }
//        private void OnTimedEvent(object source, ElapsedEventArgs e)
//        {
//            DatatableLog.Rows.Clear();
//            LogDataWitoutOrWithTimer();
//        }

//        private void LogDataWitoutOrWithTimer()
//        {
//            string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
//            try
//            {

//                StringBuilder message = new StringBuilder();
//                foreach (LoggingEvent x in mappender.GetEvents().ToList())
//                {
//                    if (((strLogToDB != string.Empty) && (strLogToDB.ToLower().Trim() == "true")) || (strMode == "Designer"))
//                    {
//                        DataRow dr = DatatableLog.NewRow();
//                        dr[0] = Environment.MachineName;
//                        dr[1] = x.UserName;
//                        dr[2] = BotName;
//                        dr[3] = ProcessName;
//                        //dr[2] = "Demo_Bot";
//                        //dr[3] = "Web";
//                        dr[4] = x.TimeStamp;
//                        dr[5] = x.LoggerName;
//                        dr[6] = x.RenderedMessage;
//                        dr[7] = x.Level;
//                        dr[8] = GroupId;
//                        dr[9] = TenantId;

//                        DatatableLog.Rows.Add(dr);
//                    }
//                    message.AppendLine("Machine: " + Environment.MachineName + " User: " + x.UserName + " TenantName: " + TenantName + 
//                        " BotName: " + BotName + " ProcessName: " + ProcessName + " TimeStamp: " + x.TimeStamp + "Logger: " + x.LoggerName + " Message: " + x.RenderedMessage + " LogLevel" + x.Level + " GroupId" + GroupId + " TenantId " + TenantId );
//                }
//                mappender.Clear();
//                if (message.ToString().Length > 0)
//                {
//                    try
//                    {
//                        //if (!Directory.Exists(filePath + @"\Logs"))
//                        //{
//                        //    Directory.CreateDirectory(filePath + @"\Logs");
//                        //}
//                        //if (!File.Exists(filePath + @"\Logs\AutomationLog.txt"))
//                        //{
//                        //using (FileStream fs = File.Create(filePath + @"\Logs\AutomationLog.txt"))
//                        //{
//                        //    byte[] info = new UTF8Encoding(true).GetBytes(message.ToString());

//                        //    fs.Write(info, 0, info.Length);
//                        //}


//                        //}
//                        //else
//                        //{
//                        //    System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", message.ToString());
//                        //}
//                        if ((strLogToELK != string.Empty) && (strLogToELK.ToLower().Trim() == "true") && (strLogstash != string.Empty))
//                        {
//                            using (var wb = new WebClient())
//                            {
//                                var data = new NameValueCollection();
//                                data["LogData:"] = message.ToString();
//                                var response = wb.UploadValues(strLogstash, "POST", data);
//                            }
//                        }
//                        if ((strLogToDB != string.Empty) && (strLogToDB.ToLower().Trim() == "true"))
//                        {
//                            if (bOTServiceClient == null)
//                            {
//                                bOTServiceClient = new ServiceReference1.BOTServiceClient(); 
//                            }
//                            bOTServiceClient.LogDataToDB(DatatableLog);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", "Error while generating log in Log.cs in OnTimeEvent function");
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", "Error while generating log in Log.cs in OnTimeEvent function");
//            }
//        }

//        public void LogData(string msg, LogLevel level)
//        {
//            if ((RequestId != null) && (RequestId != string.Empty) && (RequestId.Trim().Length > 0))
//            {
//                //msg = msg + " ReqNo : " + RequestId;
//            }

//            //mappender.Clear();
//            try
//            {

//                if (level == LogLevel.Warning)
//                {
//                    // msg = "Warning : " + msg;
//                    LogWarning(msg);
//                }
//                else if (level == LogLevel.Error)
//                {
//                    //msg = "Error : " + msg;
//                    LogError(msg);
//                }
//                else if (level == LogLevel.Debug)
//                {
//                    // msg = "Debug : " + msg;
//                    LogDebug(msg);
//                }
//                else if (level == LogLevel.Info)
//                {
//                    // msg = "Info : " + msg;
//                    LogInfo(msg);
//                }
//                else if (level == LogLevel.Fatal)
//                {
//                    // msg = "Fatal : " + msg;
//                    LogFatal(msg);
//                }
//                if (strMode == "Designer")
//                {
//                    OnLogHandler(null);
//                    LogDataWitoutOrWithTimer();
//                }
//            }
//            catch (Exception ex)
//            {

//            }

//            //mappender.Clear();
//        }

//        void LogDebug(string msg)
//        {
//            log.DebugFormat("{0}", msg);
//        }
//        void LogInfo(string msg)
//        {
//            log.InfoFormat("{0}", msg);
//        }
//        void LogWarning(string msg)
//        {
//            log.WarnFormat("{0}", msg);
//        }
//        void LogError(string msg)
//        {
//            log.ErrorFormat("{0}", msg);

//        }
//        void LogFatal(string msg)
//        {
//            log.FatalFormat("{0}", msg);
//        }

//        public static event EventHandler<EventArgs> LogHandler;
//        public static void OnLogHandler(EventArgs e)
//        {
//            EventHandler<EventArgs> handler = LogHandler;
//            if (handler != null)
//            {
//                handler(null, e);
//            }
//        }

//        #region INotify
//        public event PropertyChangedEventHandler PropertyChanged;
//        private void NotifyPropertyChanged(String propertyName)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (null != handler)
//            {
//                handler(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//        #endregion
//    }
//    //public class LogEventArgs : EventArgs
//    //{
//    //    public DataTable dtLogTable;
//    //}
//    public enum LogLevel
//    {
//        // Commmenting Because of Log Activity in Designer
//        //Debug = 0,
//        //Info = 1,
//        //Warning = 2,
//        //Error = 3,
//        //Fatal = 4,
//        //All = 5
//        Info = 0,
//        Warning = 1,
//        Error = 2,
//        Fatal = 3,
//        Debug = 4,
//        Logger = 5
//    }

//    public class LogInput
//    {
//        public string RequestNumber { get; set; }
//        public string TenantName { get; set; }
//        public string RobotName { get; set; }
//        public string Message { get; set; }
//    }
//}
#region Headers
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Core;
using System.Reflection;
using System.Timers;
using System.Net;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.ComponentModel;
#endregion

namespace Logger
{
    public class Log : INotifyPropertyChanged
    {
        System.Timers.Timer aTimer = null;
        private static object SysLock = new object();
        public DataTable DatatableLog { get; set; }
        // DataTable DatatableLog = null;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        MemoryAppender mappender = null;
        string strLogToDB = string.Empty;
        string strLogToELK = string.Empty;
        string strLogstash = string.Empty;
        string strMode = string.Empty;
      //  int groupid = 0;
      //  int tenantid = 0;



        //string strLogLevel = string.Empty;
        ServiceReference1.BOTServiceClient bOTServiceClient = null;
        private Log()
        {
            strLogToDB = ConfigurationManager.AppSettings["LogToDB"];
            strLogToELK = ConfigurationManager.AppSettings["LogToELK"];
            strLogstash = ConfigurationManager.AppSettings["LogstashUrl"];
            strMode = ConfigurationManager.AppSettings["Mode"];
            //strLogLevel = ConfigurationManager.AppSettings["LogLevel"];
            string strtxtLog = ConfigurationManager.AppSettings["TxtFileLogSwitch"];

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            //if (string.IsNullOrEmpty(strLogLevel) || strLogLevel.Trim().Length == 0)
            //{
            //    hierarchy.Root.Level = Level.Error; //default level
            //}
            //else
            //{
            //    if (strLogLevel.ToLower().Trim() == "error")
            //    {
            //        hierarchy.Root.Level = Level.Error;
            //    }
            //    else if (strLogLevel.ToLower().Trim() == "warning")
            //    {
            //        hierarchy.Root.Level = Level.Warn;
            //    }
            //    else if (strLogLevel.ToLower().Trim() == "info")
            //    {
            //        hierarchy.Root.Level = Level.Info;
            //    }
            //    else if (strLogLevel.ToLower().Trim() == "fatal")
            //    {
            //        hierarchy.Root.Level = Level.Fatal;
            //    }
            //}
            if ((!(string.IsNullOrEmpty(strtxtLog))) && (strtxtLog.ToLower().Trim() == "on"))
            {
                string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                filePath = filePath + @"\E2EBot\Logs\AutomationLog.txt";
                RollingFileAppender roller = new RollingFileAppender();
                roller.AppendToFile = false;
                roller.File = filePath;// @"Logs\EventLog.txt";
                roller.Layout = patternLayout;
                roller.MaxSizeRollBackups = 5;
                roller.MaximumFileSize = "1GB";
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.StaticLogFileName = true;
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }
            if (((!string.IsNullOrEmpty(strLogToDB)) && (strLogToDB.ToLower().Trim() == "true")) || (strMode == "Designer"))
            {
                DatatableLog = new DataTable();

                DatatableLog.Columns.Add("Machine", typeof(System.String));
                DatatableLog.Columns.Add("UserName", typeof(System.String));
                DatatableLog.Columns.Add("BotName", typeof(System.String));
                DatatableLog.Columns.Add("ProcessName", typeof(System.String));
                DatatableLog.Columns.Add("TimeStampValue", typeof(System.DateTime));
                DatatableLog.TableName = "LogData";
                DatatableLog.Columns.Add("Logger", typeof(System.String));
                DatatableLog.Columns.Add("MessageValue", typeof(System.String));
                DatatableLog.Columns.Add("LogLevel", typeof(System.String));
                DatatableLog.Columns.Add("GroupId", typeof(System.Int32));
                DatatableLog.Columns.Add("TenantId", typeof(System.Int32));
            }

            hierarchy.Configured = true;

            Task taskA = new Task(() => StartLogger());
            // Start the task.
            taskA.Start();


        }

        private static Log logger = null;
        public static Log Logger
        {
            get
            {
                if (logger == null)
                {
                    lock (SysLock)
                    {
                        logger = new Log();
                    }
                }
                return logger;
            }

        }
        private static string botname = null;
        public static string TenantName { get; set; }
        public static string BotName { get; set; }
        public static string ProcessName { get; set; }
        public static string RequestId { get; set; }
        public static int GroupId { get; set; }
        public static int TenantId { get; set; }

        int interval = 0;
        public void StartLogger()
        {

            try
            {
                aTimer = new System.Timers.Timer();

                string strinterval = ConfigurationManager.AppSettings["LogInterval(ms)"];
                if ((strinterval != null) && (!string.IsNullOrEmpty(strinterval)) && (strinterval.Length > 0))
                {
                    interval = Convert.ToInt32(ConfigurationManager.AppSettings["LogInterval(ms)"]);
                }
                else
                {
                    interval = 1000;
                }
                if (interval != 0)
                {
                    Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
                    mappender = hierarchy.Root.GetAppender("MemoryAppender") as MemoryAppender;
                    if (strMode.ToLower() == "designer")
                    {

                    }
                    else
                    {
                        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        aTimer.Interval = interval;
                        aTimer.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DatatableLog.Rows.Clear();
            LogDataWitoutOrWithTimer();
        }

        private void LogDataWitoutOrWithTimer()
        {
            string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            try
            {

                StringBuilder message = new StringBuilder();
                foreach (LoggingEvent x in mappender.GetEvents().ToList())
                {
                    if (((!string.IsNullOrEmpty(strLogToDB)) && (strLogToDB.ToLower().Trim() == "true")) || (strMode == "Designer"))
                    {
                        DataRow dr = DatatableLog.NewRow();
                        dr[0] = Environment.MachineName;
                        dr[1] = x.UserName;
                        dr[2] = BotName;
                        dr[3] = ProcessName;
                        //dr[2] = "Demo_Bot";
                        //dr[3] = "Web";
                        dr[4] = x.TimeStamp;
                        dr[5] = x.LoggerName;
                        dr[6] = x.RenderedMessage;
                        dr[7] = x.Level;
                        dr[8] = GroupId;
                        dr[9] = TenantId;

                        DatatableLog.Rows.Add(dr);
                    }
                    message.AppendLine("Machine: " + Environment.MachineName + " User: " + x.UserName + " TenantName: " + TenantName +
                        " BotName: " + BotName + " ProcessName: " + ProcessName + " TimeStamp: " + x.TimeStamp + "Logger: " + x.LoggerName + " Message: " + x.RenderedMessage + " LogLevel" + x.Level + " GroupId" + GroupId + " TenantId " + TenantId);
                }
                mappender.Clear();
                if (message.ToString().Length > 0)
                {
                    try
                    {
                        //if (!Directory.Exists(filePath + @"\Logs"))
                        //{
                        //    Directory.CreateDirectory(filePath + @"\Logs");
                        //}
                        //if (!File.Exists(filePath + @"\Logs\AutomationLog.txt"))
                        //{
                        //using (FileStream fs = File.Create(filePath + @"\Logs\AutomationLog.txt"))
                        //{
                        //    byte[] info = new UTF8Encoding(true).GetBytes(message.ToString());

                        //    fs.Write(info, 0, info.Length);
                        //}


                        //}
                        //else
                        //{
                        //    System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", message.ToString());
                        //}
                        if ((!string.IsNullOrEmpty(strLogToELK)) && (strLogToELK.ToLower().Trim() == "true") && (!string.IsNullOrEmpty(strLogstash)))
                        {
                            using (var wb = new WebClient())
                            {
                                var data = new NameValueCollection();
                                data["LogData:"] = message.ToString();
                                var response = wb.UploadValues(strLogstash, "POST", data);
                            }
                        }
                        if ((!string.IsNullOrEmpty(strLogToDB)) && (strLogToDB.ToLower().Trim() == "true"))
                        {
                            if (bOTServiceClient == null)
                            {
                                bOTServiceClient = new ServiceReference1.BOTServiceClient();
                            }
                            bOTServiceClient.LogDataToDB(DatatableLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", "Error while generating log in Log.cs in OnTimeEvent function" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", "Error while generating log in Log.cs in OnTimeEvent function" + ex.Message);
            }
        }

        public void LogData(string msg, LogLevel level)
        {
            if ((RequestId != null) && (!string.IsNullOrEmpty(RequestId)) && (RequestId.Trim().Length > 0))
            {
                //msg = msg + " ReqNo : " + RequestId;
            }

            //mappender.Clear();
            try
            {

                if (level == LogLevel.Warning)
                {
                    // msg = "Warning : " + msg;
                    LogWarning(msg);
                }
                else if (level == LogLevel.Error)
                {
                    //msg = "Error : " + msg;
                    LogError(msg);
                }
                else if (level == LogLevel.Debug)
                {
                    // msg = "Debug : " + msg;
                    LogDebug(msg);
                }
                else if (level == LogLevel.Info)
                {
                    // msg = "Info : " + msg;
                    LogInfo(msg);
                }
                else if (level == LogLevel.Fatal)
                {
                    // msg = "Fatal : " + msg;
                    LogFatal(msg);
                }
                if (strMode == "Designer")
                {
                    OnLogHandler(null);
                    LogDataWitoutOrWithTimer();
                }
            }
            catch (Exception ex)
            {
                string filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                System.IO.File.AppendAllText(filePath + @"\Logs\AutomationLog.txt", "LogData Level Error" + ex.Message);

            }

            //mappender.Clear();
        }

        void LogDebug(string msg)
        {
            log.DebugFormat("{0}", msg);
        }
        void LogInfo(string msg)
        {
            log.InfoFormat("{0}", msg);
        }
        void LogWarning(string msg)
        {
            log.WarnFormat("{0}", msg);
        }
        void LogError(string msg)
        {
            log.ErrorFormat("{0}", msg);

        }
        void LogFatal(string msg)
        {
            log.FatalFormat("{0}", msg);
        }

        public static event EventHandler<EventArgs> LogHandler;
        public static void OnLogHandler(EventArgs e)
        {
            EventHandler<EventArgs> handler = LogHandler;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
    //public class LogEventArgs : EventArgs
    //{
    //    public DataTable dtLogTable;
    //}
    public enum LogLevel
    {
        // Commmenting Because of Log Activity in Designer
        //Debug = 0,
        //Info = 1,
        //Warning = 2,
        //Error = 3,
        //Fatal = 4,
        //All = 5
        Info = 0,
        Warning = 1,
        Error = 2,
        Fatal = 3,
        Debug = 4,
        Logger = 5
    }

    public class LogInput
    {
        public string RequestNumber { get; set; }
        public string TenantName { get; set; }
        public string RobotName { get; set; }
        public string Message { get; set; }
    }
}
