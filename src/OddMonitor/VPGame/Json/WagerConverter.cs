using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayTheOdds.Common;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Json
{
    public class WagerConverter : JsonConverter
    {
        private static readonly Type WagerType = typeof(Wager);

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == WagerType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var wager = new Wager();
            wager.AdditionalData.Add("handicap", obj.ValueAsString("handicap"));
            wager.AdditionalData.Add("handicapTeam", obj.ValueAsString("handicap_team"));
            wager.Id = obj.Value<int>("id");
            wager.Name = obj.ValueAsString("mode_name");
            wager.OddLeft = obj.SelectToken("odd.left").Value<double>("item");
            wager.OddRight = obj.SelectToken("odd.right").Value<double>("item");
            wager.StartDate = DateTimeOffset.FromUnixTimeSeconds(obj.Value<int>("game_time")).LocalDateTime;
            wager.Status = VpEnum.GetWagerStatus(obj.ValueAsString("status_name"));

            return wager;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}