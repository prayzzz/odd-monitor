using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlayTheOdds.Models;

namespace PlayTheOdds.VPGame.Matches
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