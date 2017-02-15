using System.Collections.Generic;

namespace PlayTheOdds.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> AdditionalData { get; set; }
    }
}