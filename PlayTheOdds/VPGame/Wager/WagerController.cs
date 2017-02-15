using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PlayTheOdds.VPGame.Wager
{
    [Route("api/v1/vpgame/wager")]
    public class WagerController : Controller
    {
        private readonly IWagerLoader _loader;

        public WagerController(IWagerLoader loader)
        {
            _loader = loader;
        }

        [HttpGet("{scheduleId}")]
        public IEnumerable<JObject> GetWagers(int scheduleId)
        {
            return _loader.GetWagers(scheduleId).Select(x => x.Body);
        }
    }
}