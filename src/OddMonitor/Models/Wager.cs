using System;
using System.Collections.Generic;

namespace OddMonitor.Models
{
    public class Wager
    {
        public Dictionary<string, string> AdditionalData { get; } = new Dictionary<string, string>();

        public int Id { get; set; }

        public string Name { get; set; }

        public double OddLeft { get; set; }

        public double OddRight { get; set; }

        public DateTime StartDate { get; set; }

        public WagerStatus Status { get; set; }
    }
}