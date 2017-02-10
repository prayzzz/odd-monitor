using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PlayTheOdds.VPGame
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
        public IEnumerable<JObject> GetMatches()
        {
            var data = _matchLoader.GetMatches();
            return data;
        }
    }
}