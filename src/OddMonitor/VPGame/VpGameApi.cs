using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OddMonitor.Models;
using OddMonitor.VPGame.Json;
using prayzzz.Common.Attributes;

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
        private const string Scheme = "http";
        private const string Host = "www.vpgame.com";

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
                Path = "gateway/v1/match/schedule",
                Query = $"tid={scheduleId}&lang=en_US"
            };

            return GetList<Wager>(uriBuilder);
        }

        private async Task<List<T>> GetList<T>(UriBuilder uriBuilder)
        {
            try
            {
                var webRequest = WebRequest.Create(uriBuilder.Uri);
                var response = await webRequest.GetResponseAsync();
                using (var stream = new JsonTextReader(new StreamReader(response.GetResponseStream())))
                {
                    return _jsonSerializer.Deserialize<Envelope<T>>(stream).Body;
                }
            }
            catch (WebException)
            {
                _logger.LogError("VpGame not available");
                return new List<T>();
            }
            catch (Exception e)
            {
                _logger.LogError(-1, e, string.Empty);
                return new List<T>();
            }
        }

        private Task<List<Match>> GetMatchesAsync(string status)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Scheme,
                Host = Host,
                Path = "gateway/v1/match",
                Query = $"category=&status={status}&limit=100&page=1"
            };

            return GetList<Match>(uriBuilder);
        }
    }
}