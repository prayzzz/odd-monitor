using System;
using System.Collections.Generic;
using System.Threading;
using Easy.MessageHub;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;
using PlayTheOdds.Common;
using PlayTheOdds.Heartbeat;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Matches
{
    public interface IMatchLoader
    {
        List<Match> GetMatches();
    }

    [Inject(DependencyLifetime.Singleton, AutoActivate = true)]
    public class MatchLoader : IMatchLoader, IDisposable
    {
        private static readonly TimeSpan FullLoadTimeout = TimeSpan.FromMinutes(10);

        private readonly IVpGameApi _api;
        private readonly IMessageHub _messageHub;
        private readonly Timer _fullLoadTimer;
        private readonly ILogger<MatchLoader> _logger;
        private readonly object _matchDataLock;

        private List<Match> _matchData;

        public MatchLoader(IVpGameApi api, IMessageHub messageHub, ILoggerFactory loggerFactory)
        {
            _api = api;
            _messageHub = messageHub;
            _logger = loggerFactory.CreateLogger<MatchLoader>();

            _matchDataLock = new object();
            _matchData = new List<Match>();

            _fullLoadTimer = new Timer(LoadMatches, null, TimeSpan.Zero, FullLoadTimeout);
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
            var watch = BetterStopWatch.Start();

            var openMatches = await _api.GetOpenMatchesAsync();
            var liveMatches = await _api.GetLiveMatchesAsync();

            var matches = liveMatches;
            matches.AddRange(openMatches);

            watch.Stop();
            _logger.LogInformation($"{matches.Count} matches loaded in {watch.ElapsedMilliseconds}ms");

            watch.Restart();

            foreach (var match in matches)
            {
                match.Wagers.AddRange(await _api.GetWagersAsync(match.AdditionalData["scheduleId"]));
            }

            watch.Stop();
            _logger.LogInformation($"wagers loaded in {watch.ElapsedMilliseconds}ms");

            if (matches.Count != 0)
            {
                lock (_matchDataLock)
                {
                    _matchData = matches;
                }

                _messageHub.Publish(new MatchesLoadedEvent(_matchData));
            }
        }

        public void Dispose()
        {
            _fullLoadTimer.Dispose();
        }
    }
}