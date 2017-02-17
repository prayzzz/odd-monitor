﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Json
{
    public class TeamConverter : JsonConverter
    {
        private static readonly Type TeamType = typeof(Team);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("CanWrite is false");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var team = new Team();
            team.AdditionalData.Add("logoUrl", obj.Value<string>("logo"));
            team.Id = obj.Value<int>("id");
            team.Name = obj.Value<string>("name");
            
            return team;
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == TeamType;
        }
    }
}