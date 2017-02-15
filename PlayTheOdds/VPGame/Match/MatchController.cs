using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PlayTheOdds.VPGame.Match
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
            return _matchLoader.GetMatches().Select(m => m.Body);
        }
    }
}