using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayTheOdds.VPGame
{
    public class Envelope<TBody>
    {
        [JsonProperty("body")]
        public List<TBody> Body { get; set; }

        [JsonProperty("current_time")]
        public int CurrentTime { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}