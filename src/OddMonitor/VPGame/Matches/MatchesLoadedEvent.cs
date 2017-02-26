using System.Collections.Generic;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Matches
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