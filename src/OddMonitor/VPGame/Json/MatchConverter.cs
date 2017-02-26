using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OddMonitor.Common;
using OddMonitor.Models;

namespace OddMonitor.VPGame.Json
{
    public class MatchConverter : JsonConverter
    {
        private static readonly Type MatchType = typeof(Match);

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == MatchType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var match = new Match();
            match.AdditionalData.Add("scheduleId", obj.ValueAsString("tournament_schedule_id"));
            match.Category = VpEnum.GetCategory(obj.ValueAsString("category"));
            match.Id = obj.Value<int>("id");
            match.MatchFormat = VpEnum.GetMatchFormat(obj.ValueAsString("round"));
            match.Site = Site.VpGame;
            match.StartDate = DateTimeOffset.FromUnixTimeSeconds(obj.Value<int>("game_time")).LocalDateTime;
            match.TeamLeft = obj.SelectToken("team.left").ToObject<Team>(serializer);
            match.TeamRight = obj.SelectToken("team.right").ToObject<Team>(serializer);
            match.TournamentName = obj.SelectToken("tournament").ValueAsString("name");

            return match;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("CanWrite is false");
        }
    }
}