using System;
using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Match
    {
        public Category Category { get; set; }

        public DateTime DateTime { get; set; }

        public int Id { get; set; }

        public Team TeamLeft { get; set; }

        public Team TeamRight { get; set; }

        public string TournamentName { get; set; }

        public Dictionary<string, string> AdditionalData { get; set; }

        public Site Site { get; set; }
    }
}