using System.Collections.Generic;
using OddMonitor.Models;

namespace OddMonitor.VPGame.Matches
{
    public class MatchesLoadedEvent
    {
        public MatchesLoadedEvent(IEnumerable<Match> matches)
        {
            Matches = matches;
        }

        public IEnumerable<Match> Matches { get; }
    }
}