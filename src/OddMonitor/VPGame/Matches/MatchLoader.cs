using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using OddMonitor.Common;
using OddMonitor.Common.Extensions;
using OddMonitor.Models;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;

namespace OddMonitor.VPGame.Matches
{
    public interface IMatchLoader
    {
        List<Match> GetMatches();
    }

    [Inject(DependencyLifetime.Singleton, AutoActivate = true)]
    public class MatchLoader : IMatchLoader, IDisposable
    {
        private static readonly TimeSpan FullLoadTimeout = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan RefreshTimeout = TimeSpan.FromMinutes(1);

        private readonly IVpGameApi _api;
        private readonly Timer _fullLoadTimer;
        private readonly ILogger<MatchLoader> _logger;
        private readonly object _matchDataLock;
        private readonly Timer _refreshWagersTimer;

        private List<Match> _matchData;

        public MatchLoader(IVpGameApi api, ILogger<MatchLoader> logger)
        {
            _api = api;
            _logger = logger;

            _matchDataLock = new object();
            _matchData = new List<Match>();

            _fullLoadTimer = new Timer(LoadMatches, null, TimeSpan.Zero, FullLoadTimeout);
            _refreshWagersTimer = new Timer(RefreshWagers, null, TimeSpan.Zero, RefreshTimeout);
        }

        public void Dispose()
        {
            _fullLoadTimer.Dispose();
            _refreshWagersTimer.Dispose();
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
            matches = matches.DistinctBy(x => x.AdditionalData["scheduleId"]).ToList();

            watch.Stop();
            _logger.LogDebug($"{matches.Count} matches loaded in {watch.ElapsedMilliseconds}ms");

            watch.Restart();

            foreach (var match in matches)
            {
                match.Wagers.AddRange(await _api.GetWagersAsync(match.AdditionalData["scheduleId"]));
            }

            watch.Stop();
            _logger.LogDebug($"{matches.Sum(m => m.Wagers.Count)} wagers loaded in {watch.ElapsedMilliseconds}ms");

            if (matches.Count != 0)
            {
                lock (_matchDataLock)
                {
                    _matchData = matches;
                }
            }
        }

        private async void RefreshWagers(object state)
        {
            var watch = BetterStopWatch.Start();

            var matches = new List<Match>(_matchData);

            var now = DateTime.Now;
            var startingMatches = matches.Where(x => x.StartDate > now && x.StartDate - now < TimeSpan.FromMinutes(15))
                                         .ToList();

            if (startingMatches.Count == 0)
            {
                return;
            }

            var matchToWagers = new Dictionary<Match, List<Wager>>();
            foreach (var match in startingMatches)
            {
                var loadedWagers = await _api.GetWagersAsync(match.AdditionalData["scheduleId"]);
                if (loadedWagers.Count > 0)
                {
                    matchToWagers.Add(match, loadedWagers);
                }
            }

            lock (_matchDataLock)
            {
                foreach (var pair in matchToWagers)
                {
                    pair.Key.Wagers.Clear();
                    pair.Key.Wagers.AddRange(pair.Value);
                }
            }

            watch.Stop();
            _logger.LogDebug($"{matchToWagers.Count} matches updated in {watch.ElapsedMilliseconds}ms");
        }
    }
}