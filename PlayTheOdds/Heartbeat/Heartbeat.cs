using System;

namespace PlayTheOdds.Heartbeat
{
    public interface IHeartbeat
    {
        void ResetLastActivity();
    }

    /// <summary>
    /// Stores the date of the last heartbeat occurence
    /// </summary>
    public class Heartbeat : IHeartbeat
    {
        public DateTime LastActivity { get; private set; }

        public void ResetLastActivity()
        {
            LastActivity = DateTime.Now;
        }
    }
}