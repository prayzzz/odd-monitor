using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prayzzz.Common.Attributes;

namespace PlayTheOdds.VPGame.Match
{
    public interface IMatchService
    {
        Task<List<Match>> GetLiveMatchesAsync();
        Task<List<Match>> GetOpenMatchesAsync();
    }

    [Inject]
    public class MatchService : IMatchService
    {
        private const string Scheme = "http";
        private const string Host = "www.vpgame.com";
        private const string Path = "gateway/v1/match";
        private const string StatusOpen = "open";
        private const string StatusLive = "start";
        private readonly JsonSerializer _jsonSerializer;

        private readonly ILogger _logger;

        public MatchService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MatchService>();
            _jsonSerializer = JsonSerializer.Create();
        }

        public async Task<List<Match>> GetLiveMatchesAsync()
        {
            return await GetMatches(StatusLive);
        }

        public async Task<List<Match>> GetOpenMatchesAsync()
        {
            return await GetMatches(StatusOpen);
        }

        private async Task<List<Match>> GetMatches(string status)
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
                    var body = _jsonSerializer.Deserialize<Envelope<Match>>(stream).Body;
                    return body.Select(b => new Match {Body = b}).ToList();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(-1, e, string.Empty);
                return new List<Match>();
            }
        }
    }
}