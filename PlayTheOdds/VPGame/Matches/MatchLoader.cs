using System;
using System.Collections.Generic;
using System.Threading;
using Easy.MessageHub;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;
using PlayTheOdds.Heartbeat;

namespace PlayTheOdds.VPGame.Matches
{
    public interface IMatchLoader
    {
        List<Match> GetMatches();
    }

    [Inject(DependencyLifetime.Singleton, AutoActivate = true)]
    public class MatchLoader : IMatchLoader
    {
        private readonly IHeartbeat _heartbeat;
        private readonly IMatchService _matchService;
        private readonly IMessageHub _messageHub;
        private readonly ILogger<MatchLoader> _logger;

        private readonly Timer _loadingTimer;

        private readonly object _matchDataLock;
        private DateTime _matchUpdateTime;
        private List<Match> _matchData;

        public MatchLoader(IHeartbeat heartbeat, IMatchService matchService, IMessageHub messageHub, ILoggerFactory loggerFactory)
        {
            _heartbeat = heartbeat;
            _matchService = matchService;
            _messageHub = messageHub;
            _logger = loggerFactory.CreateLogger<MatchLoader>();

            _matchDataLock = new object();
            _matchData = new List<Match>();

            _loadingTimer = new Timer(LoadMatches, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        }

        public List<Match> GetMatches()
        {
            lock (_matchDataLock)
            {
                return _matchData;
            }
        }

        private async void LoadMatches(object state)
        {
            // Update every 10Minutes, if there was no heartbeat for 5Minutes
            if (_heartbeat.TimeSinceLastActivity > TimeSpan.FromMinutes(5) &&
                DateTime.Now - _matchUpdateTime < TimeSpan.FromMinutes(10))
            {
                return;
            }

            var openMatches = await _matchService.GetOpenMatchesAsync();
            var liveMatches = await _matchService.GetLiveMatchesAsync();

            var matches = liveMatches;
            matches.AddRange(openMatches);

            _logger.LogInformation($"{matches.Count} matches loaded");

            if (matches.Count != 0)
            {
                lock (_matchDataLock)
                {
                    _matchUpdateTime = DateTime.Now;
                    _matchData = matches;
                }

                _messageHub.Publish(new MatchesLoadedEvent(_matchData));
            }
        }
    }
}