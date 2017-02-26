using System;
using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Match : IEquatable<Match>, IEqualityComparer<Match>
    {
        public Dictionary<string, string> AdditionalData { get; } = new Dictionary<string, string>();

        public Category Category { get; set; }

        public DateTime DateModified { get; set; } = DateTime.Now;

        public int Id { get; set; }

        public MatchFormat MatchFormat { get; set; }

        public Site Site { get; set; }

        public DateTime StartDate { get; set; }

        public Team TeamLeft { get; set; }

        public Team TeamRight { get; set; }

        public string TournamentName { get; set; }

        public List<Wager> Wagers { get; } = new List<Wager>();

        public bool Equals(Match other)
        {
            return Id == other.Id;
        }

        public bool Equals(Match x, Match y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Match obj)
        {
            return obj.GetHashCode();
        }
    }
}