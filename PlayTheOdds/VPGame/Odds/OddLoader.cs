using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Easy.MessageHub;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;
using PlayTheOdds.Heartbeat;
using PlayTheOdds.VPGame.Matches;

namespace PlayTheOdds.VPGame.Odds
{
    public interface IOddLoader
    {
        List<Odd> GetOdds(int scheduleId);
    }

    [Inject(DependencyLifetime.Singleton, AutoActivate = true)]
    public class OddLoader : IOddLoader
    {
        private readonly IHeartbeat _heartbeat;
        private readonly IOddService _oddService;
        private readonly IMessageHub _messageHub;
        private readonly ILogger<MatchLoader> _logger;

        private readonly Timer _loadingTimer;

        private readonly object _oddDataLock;
        private DateTime _oddUpdateTime;
        private Dictionary<int, List<Odd>> _oddData;

        public OddLoader(IHeartbeat heartbeat, IOddService oddService, IMessageHub messageHub, ILoggerFactory loggerFactory)
        {
            _heartbeat = heartbeat;
            _oddService = oddService;
            _messageHub = messageHub;
            _logger = loggerFactory.CreateLogger<MatchLoader>();

            _oddDataLock = new object();
            _oddData = new Dictionary<int, List<Odd>>();

            //_loadingTimer = new Timer(LoadOdds, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            _messageHub.Subscribe<MatchesLoadedEvent>(OnMatchesLoaded);
        }

        private void OnMatchesLoaded(MatchesLoadedEvent ev)
        {
            var idToOdds = new Dictionary<int, List<Odd>>();

            Parallel.ForEach(ev.Matches, async match =>
            {
                var odds = await _oddService.GetOdds(match.ScheduleId);
                idToOdds.Add(match.ScheduleId, odds);
            });


            _logger.LogInformation($"{idToOdds.Values.SelectMany(x => x).Count()} Odds loaded");

            lock (_oddDataLock)
            {
                _oddData = idToOdds;
            }
        }

        //private void LoadOdds(object state)
        //{
        //    // Update every 10Minutes, if there was no heartbeat for 5Minutes
        //    if (_heartbeat.TimeSinceLastActivity > TimeSpan.FromMinutes(5) &&
        //        DateTime.Now - _oddUpdateTime < TimeSpan.FromMinutes(10))
        //    {
        //        return;
        //    }

        //    Parallel.ForEach(_oddData.Keys, async scheduleId =>
        //    {
        //        var odds = await _oddService.GetOdds(scheduleId);

        //        lock (_oddDataLock)
        //        {
        //            _oddData[scheduleId] = odds;
        //        }
        //    });

        //    _oddUpdateTime = DateTime.Now;
        //}

        public List<Odd> GetOdds(int scheduleId)
        {
            if (_oddData.TryGetValue(scheduleId, out List<Odd> odds))
            {
                return odds;
            }

            return new List<Odd>();
        }
    }
}