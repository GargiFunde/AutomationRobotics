using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace AutomationService
{
    public abstract class AbstractLogger : ILogger
    {
        public virtual void JobToBeExecuted(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public virtual void JobExecutionVetoed(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public virtual void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            throw new NotImplementedException();
        }

        public virtual void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            throw new NotImplementedException();
        }

        string IJobListener.Name
        {
            get { return "QuartzNetWebConsole.Logger"; }
        }

        public virtual void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            return false;
        }

        public virtual void TriggerMisfired(ITrigger trigger)
        {
            throw new NotImplementedException();
        }

        string ITriggerListener.Name
        {
            get { return "QuartzNetWebConsole.Logger"; }
        }

        public abstract IEnumerator<LogEntry> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract Expression Expression { get; }
        public abstract Type ElementType { get; }
        public abstract IQueryProvider Provider { get; }
        public abstract void Add(string msg);

        public virtual void JobScheduled(ITrigger trigger)
        {

        }

        public virtual void JobUnscheduled(TriggerKey triggerKey)
        {
        }

        public virtual void TriggerFinalized(ITrigger trigger)
        {
        }

        public virtual void TriggerPaused(TriggerKey triggerKey)
        {
        }

        public virtual void TriggersPaused(string triggerGroup)
        {
        }

        public virtual void TriggerResumed(TriggerKey triggerKey)
        {
        }

        public virtual void TriggersResumed(string triggerGroup)
        {
        }

        public virtual void JobAdded(IJobDetail jobDetail)
        {
        }

        public virtual void JobDeleted(JobKey jobKey)
        {
        }

        public virtual void JobPaused(JobKey jobKey)
        {
        }

        public virtual void JobsPaused(string jobGroup)
        {
        }

        public virtual void JobResumed(JobKey jobKey)
        {
        }

        public virtual void JobsResumed(string jobGroup)
        {
        }

        public virtual void SchedulerError(string msg, SchedulerException cause)
        {
        }

        public virtual void SchedulerInStandbyMode()
        {
        }

        public virtual void SchedulerStarted()
        {
        }

        public virtual void SchedulerStarting()
        {
        }

        public virtual void SchedulerShutdown()
        {
        }

        public virtual void SchedulerShuttingdown()
        {
        }

        public virtual void SchedulingDataCleared()
        {
        }

        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}