using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AutomationService
{
    public class QuartzHelper
    {

        public static void InitializeQuartz()
        {
            try
            {
                Task.Run(async () =>
                {
                    StdSchedulerFactory factory = new StdSchedulerFactory(LocalConfig());

                    // get a scheduler
                    IScheduler sched = await factory.GetScheduler();
                    // sched.ListenerManager.AddTriggerListener(new TriggerListener("TriggerListner"));
                    //ILogger Logger = new MemoryLogger(1000);
                    //sched.ListenerManager.AddJobListener(Logger);
                    //sched.ListenerManager.AddTriggerListener(Logger);
                    //sched.ListenerManager.AddSchedulerListener(Logger);

                    if (!sched.IsStarted)
                        await sched.Start();

                    //  Console.WriteLine("Local Server has been started..");

                    //while (true)
                    //{
                    //    Thread.Sleep(60 * 60000);
                    //}
                }).Wait();
            }
            catch (Exception e)
            {
                BOTService serviceMethod = new BOTService();
                serviceMethod.insertLog("Issue In Initializing Quartz", "Exception: " + e.Message, 0,0 ); //No Tenant Id and GroupId at this Point so setting it to 0.
            }
  
        }

        public static NameValueCollection LocalConfig()  //Defining Local Configuation and mapping with DataBase
        {
            string connstring = ConfigurationManager.ConnectionStrings["ConnectionStringSql"].ConnectionString;
            NameValueCollection configuration = new NameValueCollection
            {
                 { "quartz.scheduler.instanceName", "RemoteServer" },
                 { "quartz.scheduler.instanceId", "RemoteServer" },
                 { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                 { "quartz.jobStore.useProperties", "true" },
                 { "quartz.jobStore.dataSource", "default" },
                 { "quartz.jobStore.tablePrefix", "QRTZ_" },
                 { "quartz.dataSource.default.connectionString", connstring },
                 { "quartz.dataSource.default.provider", "SqlServer" },
                 { "quartz.threadPool.threadCount", "80" },    //to increase threadCount from 10 to 80 to solve issue "all worker threads were busy running other jobs"
                 { "quartz.serializer.type", "binary" },
            };

            return configuration;
        }


        public async void CreateJob(string serverName, string strQueueName, string strBotName, string strChronExp,int groupid, int tenantid)
        {
            string strTenantid = string.Empty;
            string strGroupid = string.Empty;
            try
            {
                NameValueCollection configuration = LocalConfig();                               //Defining Local Configuation and mapping with DataBase
                StdSchedulerFactory factory = new StdSchedulerFactory(configuration); // Creating Scheduler Factory Object.
                IScheduler sched = await factory.GetScheduler();                                   // Initializing Default Scheduler.

                strTenantid = Convert.ToString(tenantid);
                strGroupid = Convert.ToString(groupid);

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity(strBotName + strQueueName + strChronExp, strBotName)
                    .UsingJobData("strChronExp", strChronExp)
                    .UsingJobData("ServerName", serverName)
                    .UsingJobData("DateTime", DateTime.Now.ToString())
                    .UsingJobData("QueueName", strQueueName)
                    .UsingJobData("BotName", strBotName)
                    .UsingJobData("tenantid", strTenantid)
                    .UsingJobData("strGroupid", strGroupid)
                    .Build();

                // Trigger the job to run now, and then every 40 seconds
                ITrigger trigger = TriggerBuilder.Create()                                              //Trigger the Job and Run.
                    .WithIdentity(strBotName + strQueueName + strChronExp, strBotName)
                    .ForJob(job.Key)                                                                              // To identify job using Key
                    .WithCronSchedule(strChronExp, cron => { cron.InTimeZone(TimeZoneInfo.Utc); }) //"0 0/1 * 1/1 * ? *"
                    .StartNow()
                    .Build();

                await sched.ScheduleJob(job, trigger);                                                 //Schedule the Job using Trigger
                //Quartz.JobKey jobKey = new JobKey(strBotName + strQueueName + strChronExp);
                //await sched.DeleteJob(jobKey);

                //BOTService botService = new BOTService();

            }
            catch (Exception e)
            {
                BOTService serviceMethod = new BOTService();
                serviceMethod.insertLog("CreateJob Exception","Exception: "+e.Message,Convert.ToInt32(strGroupid),Convert.ToInt32(strTenantid));
            }

        }

        public async void CreateJobForProcess(string serverName, string strProcessName, string strBotName, string strChronExp, int groupid, int tenantid)
        {
            string strTenantid = string.Empty;
            string strGroupid = string.Empty;
            try
            {
                NameValueCollection configuration = LocalConfig();                               //Defining Local Configuation and mapping with DataBase
                StdSchedulerFactory factory = new StdSchedulerFactory(configuration); // Creating Scheduler Factory Object.
                IScheduler sched = await factory.GetScheduler();                                   //Initializing Default Scheduler

                strTenantid = Convert.ToString(tenantid);
                strGroupid = Convert.ToString(groupid);

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity(strBotName + strProcessName + strChronExp, strBotName)
                    .UsingJobData("strChronExp", strChronExp)
                    .UsingJobData("ServerName", serverName)
                    .UsingJobData("DateTime", DateTime.Now.ToString())
                    .UsingJobData("ProcessName", strProcessName)
                    .UsingJobData("BotName", strBotName)
                    .UsingJobData("tenantid", strTenantid)
                    .UsingJobData("strGroupid", strGroupid)
                    .Build();

                // Trigger the job to run now, and then every 40 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(strBotName + strProcessName + strChronExp, strBotName)
                    .ForJob(job.Key)
                    .WithCronSchedule(strChronExp, cron => { cron.InTimeZone(TimeZoneInfo.Utc); }) //"0 0/1 * 1/1 * ? *"
                    .StartNow()
                    .Build();

                await sched.ScheduleJob(job, trigger);
            }
            catch (Exception e)
            {
                BOTService serviceMethod = new BOTService();
                serviceMethod.insertLog("CreateJob For Process Exception", "Exception: " + e.Message, Convert.ToInt32(strGroupid), Convert.ToInt32(strTenantid));
            }
        }

        public async void DeleteJob(string serverName, string strQueueName, string strBotName, string strChronExp, int tenantid)
        {
            try
            {
                NameValueCollection configuration = LocalConfig();
                StdSchedulerFactory factory = new StdSchedulerFactory(configuration);
                IScheduler sched = await factory.GetScheduler();
                Quartz.JobKey jobKey = new JobKey(strBotName + strQueueName + strChronExp);
                await sched.DeleteJob(jobKey);
            }
            catch (Exception e)
            {
                BOTService serviceMethod = new BOTService();
                serviceMethod.insertLog("Delete Job Exception", "Exception: " + e.Message, tenantid, tenantid); //No Group Id Found at this point.
            }
        }
    }
}