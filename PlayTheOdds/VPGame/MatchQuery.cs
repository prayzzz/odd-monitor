namespace PlayTheOdds.VPGame
{
    public class MatchQuery
    {
        public string Category { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }

        public string Status { get; set; }
    }
}