using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;
using PlayTheOdds.Heartbeat;

namespace PlayTheOdds.VPGame
{
    public interface IMatchLoader
    {
        List<JObject> GetMatches();
    }

    [Inject(DependencyLifetime.Singleton)]
    public class MatchLoader : IMatchLoader
    {
        private readonly IHeartbeat _heartbeat;
        private readonly IMatchService _matchService;
        private readonly ILogger<MatchLoader> _logger;

        private readonly Timer _loadingTimer;

        private readonly object _matchDataLock;
        private List<JObject> _matchData;

        public MatchLoader(IHeartbeat heartbeat, IMatchService matchService, ILoggerFactory loggerFactory)
        {
            _heartbeat = heartbeat;
            _matchService = matchService;
            _logger = loggerFactory.CreateLogger<MatchLoader>();

            _matchDataLock = new object();
            _matchData = new List<JObject>();

            _loadingTimer = new Timer(LoadMatches, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        }

        public List<JObject> GetMatches()
        {
            lock (_matchDataLock)
            {
                return _matchData;
            }
        }

        private async void LoadMatches(object state)
        {
            if (_heartbeat.TimeSinceLastActivity > TimeSpan.FromMinutes(5))
            {
                return;
            }

            var openMatches = await _matchService.GetOpenMatchesAsync();
            var liveMatches = await _matchService.GetLiveMatchesAsync();

            var matches = liveMatches;
            matches.AddRange(openMatches);

            _logger.LogInformation($"{matches.Count} matches loaded");
            
            lock (_matchDataLock)
            {
                _matchData = matches;
            }
        }
    }

}