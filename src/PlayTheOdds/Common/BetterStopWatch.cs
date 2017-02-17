using System.Diagnostics;

namespace PlayTheOdds.Common
{
    public class BetterStopWatch : Stopwatch
    {
        public new static Stopwatch Start()
        {
            var watch = new Stopwatch();
            watch.Start();
            return watch;
        }
    }
}