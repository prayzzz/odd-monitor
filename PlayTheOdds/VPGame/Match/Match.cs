using Newtonsoft.Json.Linq;

namespace PlayTheOdds.VPGame.Match
{
    public class Match
    {
        public JObject Body { get; set; }

        public int ScheduleId => Body.Value<int>("tournament_schedule_id");
    }
}