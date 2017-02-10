using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prayzzz.Common.Attributes;

namespace PlayTheOdds.VPGame
{
    public interface IMatchService
    {
        Task<List<JObject>> GetOpenMatchesAsync();

        Task<List<JObject>> GetLiveMatchesAsync();
    }

    [Inject]
    public class MatchService : IMatchService
    {
        private const string Scheme = "http";
        private const string Host = "www.vpgame.com";
        private const string Path = "gateway/v1/match";
        private const string StatusOpen = "open";
        private const string StatusLive = "start";

        private readonly ILogger<MatchService> _logger;
        private readonly JsonSerializer _jsonSerializer;

        public MatchService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MatchService>();
            _jsonSerializer = JsonSerializer.Create();
        }

        public async Task<List<JObject>> GetOpenMatchesAsync()
        {
            return await GetMatches(StatusOpen);
        }

        public async Task<List<JObject>> GetLiveMatchesAsync()
        {
            return await GetMatches(StatusLive);
        }

        private async Task<List<JObject>> GetMatches(string status)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Host = Host,
                Path = Path,
                Query = $"category=&status={status}&limit=100&page=1"
            };

            try
            {
                var webRequest = WebRequest.Create(uriBuilder.Uri);
                var response = await webRequest.GetResponseAsync();
                using (var stream = new JsonTextReader(new StreamReader(response.GetResponseStream())))
                {
                    return _jsonSerializer.Deserialize<VpGameEnvelope<VpGameMatch>>(stream).Body;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(null, e, string.Empty);
                return new List<JObject>();
            }
        }
    }
}