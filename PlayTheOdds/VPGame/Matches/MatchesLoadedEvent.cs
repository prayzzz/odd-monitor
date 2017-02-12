using System.Collections.Generic;

namespace PlayTheOdds.VPGame.Matches
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