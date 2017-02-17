using System;
using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Wager
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public string Name { get; set; }

        public double OddLeft { get; set; }

        public double OddRight { get; set; }

        public Dictionary<string, string> AdditionalData { get; } = new Dictionary<string, string>();
    }
}