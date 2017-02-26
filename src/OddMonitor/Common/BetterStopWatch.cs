using System.Diagnostics;

namespace OddMonitor.Common
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