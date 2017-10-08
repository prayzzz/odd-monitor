using Microsoft.AspNetCore.Mvc;
using OddMonitor.Models;
using System.Collections.Generic;

namespace OddMonitor.VPGame.Matches
{
    [Route("api/v1/vpgame/match")]
    public class MatchController : Controller
    {
        private readonly IMatchLoader _matchLoader;

        public MatchController(IMatchLoader matchLoader)
        {
            _matchLoader = matchLoader;
        }

        [HttpGet]
        public IEnumerable<Match> GetMatches()
        {
            return _matchLoader.GetMatches();
        }
    }
}
