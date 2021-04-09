
using Quartz;
using System.Linq;

namespace AutomationService
{
    public interface ILogger : ISchedulerListener, IJobListener, ITriggerListener, IQueryable<LogEntry>
    {
        void Add(string msg);
    }
}
