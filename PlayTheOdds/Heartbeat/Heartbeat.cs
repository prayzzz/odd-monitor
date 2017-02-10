using System;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;

namespace PlayTheOdds.Heartbeat
{
    public interface IHeartbeat
    {
        /// <summary>
        /// Time of the last heartbeat
        /// </summary>
        DateTime LastHeartbeat { get; }

        /// <summary>
        /// Elapsed time since last heartbeat
        /// </summary>
        TimeSpan TimeSinceLastActivity { get; }

        void ResetHeartbeat();
    }

    /// <summary>
    /// Stores the date of the last heartbeat occurence
    /// </summary>
    [Inject(DependencyLifetime.Singleton)]
    public class Heartbeat : IHeartbeat
    {
        public Heartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }

        /// <summary>
        /// Time of the last heartbeat
        /// </summary>
        public DateTime LastHeartbeat { get; private set; }

        public TimeSpan TimeSinceLastActivity => DateTime.Now - LastHeartbeat;

        public void ResetHeartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }
    }
}