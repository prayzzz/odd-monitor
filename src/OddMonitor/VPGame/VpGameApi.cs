using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OddMonitor.Models;
using OddMonitor.VPGame.Json;
using prayzzz.Common.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OddMonitor.VPGame
{
    public interface IVpGameApi
    {
        Task<List<Match>> GetLiveMatchesAsync();

        Task<List<Match>> GetOpenMatchesAsync();

        Task<List<Wager>> GetWagersAsync(string scheduleId);
    }

    [Inject]
    public class VpGameApi : IVpGameApi
    {
        // Header
        private const string AcceptHeader = "application/json; application/javascript";

        private const string AcceptLanguageHeader = "en-US";

        private const string CacheControlHeader = "no-cache";

        private const string Host = "www.vpgame.com";

        private const string MatchPath = "gateway/v1/match";

        // Uri
        private const string Scheme = "http";

        private const string UserAgentHeader = "https://github.com/prayzzz/odd-monitor";
        private const string WagerPath = "gateway/v1/match/schedule";
        private readonly JsonSerializer _jsonSerializer;
        private readonly ILogger _logger;

        public VpGameApi(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VpGameApi>();

            _jsonSerializer = VpJsonSerializer.Instance;
        }

        public Task<List<Match>> GetLiveMatchesAsync()
        {
            return GetMatchesAsync("start");
        }

        public Task<List<Match>> GetOpenMatchesAsync()
        {
            return GetMatchesAsync("open");
        }

        public Task<List<Wager>> GetWagersAsync(string scheduleId)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Host = Host,
                Path = WagerPath,
                Query = $"tid={scheduleId}&lang=en_US"
            };

            return GetList<Wager>(uriBuilder);
        }

        private async Task<List<T>> GetList<T>(UriBuilder uriBuilder)
        {
            try
            {
                var webRequest = WebRequest.Create(uriBuilder.Uri);
                webRequest.Headers["Accept"] = AcceptHeader;
                webRequest.Headers["Accept-Language"] = AcceptLanguageHeader;
                webRequest.Headers["Cache-Control"] = CacheControlHeader;
                webRequest.Headers["User-Agent"] = UserAgentHeader;

                var response = await webRequest.GetResponseAsync();
                using (var stream = new JsonTextReader(new StreamReader(response.GetResponseStream())))
                {
                    return _jsonSerializer.Deserialize<Envelope<T>>(stream).Body;
                }
            }
            catch (WebException)
            {
                _logger.LogInformation("VpGame not available");
                return new List<T>();
            }
            catch (IOException)
            {
                _logger.LogInformation("Response broken");
                return new List<T>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unknown error while requesting {Uri}", uriBuilder.Uri);
                return new List<T>();
            }
        }

        private Task<List<Match>> GetMatchesAsync(string status)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Host = Host,
                Path = MatchPath,
                Query = $"category=&status={status}&limit=100&page=1"
            };

            return GetList<Match>(uriBuilder);
        }
    }
}
