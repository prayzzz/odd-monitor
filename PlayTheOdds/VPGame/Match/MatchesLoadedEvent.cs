using System.Collections.Generic;

namespace PlayTheOdds.VPGame.Match
{
    public class MatchesLoadedEvent
    {
        public MatchesLoadedEvent(List<Match> matches)
        {
            Matches = matches;
        }

        public List<Match> Matches { get; }
    }
}