using System;
using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Wager
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Team Team1 { get; set; }

        public Team Team2 { get; set; }

        public string Name { get; set; }

        public Odd OddLeft { get; set; }

        public Odd OddRight { get; set; }

        public Dictionary<string, string> AdditionalData { get; set; }
    }

    public class Odd
    {
        public Team Team { get; set; }

        public int Value { get; set; }
    }
}