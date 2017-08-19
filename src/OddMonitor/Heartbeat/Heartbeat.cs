using System;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;

namespace OddMonitor.Heartbeat
{
    public interface IHeartbeat
    {
        /// <summary>
        ///     Time of the last heartbeat
        /// </summary>
        DateTime LastHeartbeat { get; }

        /// <summary>
        ///     Elapsed time since last heartbeat
        /// </summary>
        TimeSpan TimeSinceLastHeartbeat { get; }

        /// <summary>
        ///     Executes a heartbeat
        /// </summary>
        void ResetHeartbeat();
    }

    /// <summary>
    ///     Stores the date of the last heartbeat
    /// </summary>
    [Inject(DependencyLifetime.Singleton)]
    public class Heartbeat : IHeartbeat
    {
        public Heartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }

        /// <inheritdoc />
        public DateTime LastHeartbeat { get; private set; }

        /// <inheritdoc />
        public TimeSpan TimeSinceLastHeartbeat => DateTime.Now - LastHeartbeat;

        /// <inheritdoc />
        public void ResetHeartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }
    }
}