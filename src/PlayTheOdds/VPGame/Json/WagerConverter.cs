using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Json
{
    public class WagerConverter : JsonConverter
    {
        private static readonly Type WagerType = typeof(Wager);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var wager = new Wager();
            wager.AdditionalData.Add("handicap", obj.Value<string>("handicap"));
            wager.AdditionalData.Add("handicapTeam", obj.Value<string>("handicap_team"));
            wager.Id = obj.Value<int>("id");
            wager.OddLeft = obj.SelectToken("odd.left").Value<double>("item");
            wager.OddRight = obj.SelectToken("odd.right").Value<double>("item");
            wager.Name = obj.Value<string>("mode_name");
            wager.StartDate = DateTimeOffset.FromUnixTimeSeconds(obj.Value<int>("game_time")).LocalDateTime;

            return wager;
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == WagerType;
        }
    }
}