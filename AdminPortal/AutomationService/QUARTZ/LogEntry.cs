using System;

namespace AutomationService
{
    public class LogEntry
    {
        public readonly DateTimeOffset Timestamp;
        public readonly string Description;

        public LogEntry(DateTimeOffset timestamp, string description)
        {
            this.Timestamp = timestamp;
            this.Description = description;
        }

        public LogEntry(string description)
        {
            this.Description = description;
            Timestamp = DateTimeOffset.Now;
        }

    }
}