using System;
using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Team : IEquatable<Team>
    {
        public Dictionary<string, string> AdditionalData { get; set; } = new Dictionary<string, string>();
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Equals(Team other)
        {
            return Id == other.Id;
        }
    }
}