using System;
using System.Collections.Generic;
using System.Linq;
using Easy.MessageHub;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Attributes;
using prayzzz.Common.Enums;
using PlayTheOdds.Heartbeat;
using PlayTheOdds.VPGame.Match;

namespace PlayTheOdds.VPGame.Wager
{
    public interface IWagerLoader
    {
        List<Wager> GetWagers(int scheduleId);
    }

    [Inject(DependencyLifetime.Singleton, AutoActivate = true)]
    public class WagerLoader : IWagerLoader
    {
        private readonly IHeartbeat _heartbeat;
        private readonly ILogger _logger;
        private readonly IMessageHub _messageHub;
        private readonly IWagerService _wagerService;

        private readonly object _dataLock;
        private Dictionary<int, List<Wager>> _data;

        public WagerLoader(IHeartbeat heartbeat, IWagerService wagerService, IMessageHub messageHub, ILoggerFactory loggerFactory)
        {
            _heartbeat = heartbeat;
            _wagerService = wagerService;
            _messageHub = messageHub;
            _logger = loggerFactory.CreateLogger<WagerLoader>();

            _dataLock = new object();
            _data = new Dictionary<int, List<Wager>>();

            _messageHub.Subscribe<MatchesLoadedEvent>(OnMatchesLoaded);
        }

        public List<Wager> GetWagers(int scheduleId)
        {
            if (_data.TryGetValue(scheduleId, out List<Wager> wagers))
            {
                return wagers;
            }

            return new List<Wager>();
        }

        private async void OnMatchesLoaded(MatchesLoadedEvent ev)
        {
            var startTime = DateTime.Now;

            var idToOdds = new Dictionary<int, List<Wager>>();
            foreach (var scheduleId in ev.Matches.Select(x => x.ScheduleId).Distinct())
            {
                var odds = await _wagerService.GetOdds(scheduleId);
                idToOdds.Add(scheduleId, odds);
            }

            _logger.LogInformation($"{idToOdds.Values.SelectMany(x => x).Count()} wagers loaded in {(DateTime.Now - startTime).Milliseconds}ms");

            lock (_dataLock)
            {
                _data = idToOdds.ToDictionary(x => x.Key, x => x.Value);
            }
        }
    }
}