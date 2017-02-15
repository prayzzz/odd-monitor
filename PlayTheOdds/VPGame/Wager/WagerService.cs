using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prayzzz.Common.Attributes;

namespace PlayTheOdds.VPGame.Wager
{
    public interface IWagerService
    {
        Task<List<Wager>> GetOdds(int matchId);
    }

    [Inject]
    public class WagerService : IWagerService
    {
        private const string Scheme = "http";
        private const string Host = "www.vpgame.com";
        private const string Path = "gateway/v1/match/schedule";
        private readonly JsonSerializer _jsonSerializer;

        private readonly ILogger _logger;

        public WagerService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WagerService>();
            _jsonSerializer = JsonSerializer.Create();
        }

        public async Task<List<Wager>> GetOdds(int scheduleId)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Host = Host,
                Path = Path,
                Query = $"tid={scheduleId}&lang=en_US"
            };

            try
            {
                var webRequest = WebRequest.Create(uriBuilder.Uri);
                var response = await webRequest.GetResponseAsync();
                using (var stream = new JsonTextReader(new StreamReader(response.GetResponseStream())))
                {
                    var body = _jsonSerializer.Deserialize<Envelope<Match.Match>>(stream).Body;
                    return body.Select(b => new Wager {Body = b}).ToList();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(-1, e, string.Empty);
                return new List<Wager>();
            }
        }
    }
}