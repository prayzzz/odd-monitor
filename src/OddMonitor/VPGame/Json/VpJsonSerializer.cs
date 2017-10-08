using Newtonsoft.Json;

namespace OddMonitor.VPGame.Json
{
    public sealed class VpJsonSerializer : JsonSerializer
    {
        private static VpJsonSerializer _instance;

        private VpJsonSerializer()
        {
            Converters.Add(new MatchConverter());
            Converters.Add(new TeamConverter());
            Converters.Add(new WagerConverter());
        }

        public static VpJsonSerializer Instance => _instance ?? (_instance = new VpJsonSerializer());
    }
}
