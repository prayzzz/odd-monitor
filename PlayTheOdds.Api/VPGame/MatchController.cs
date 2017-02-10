using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PlayTheOdds.Api.VPGame
{
    [Route("api/v1/vpgame/match")]
    public class MatchController : Controller
    {
        private const string Host = "www.vpgame.com";
        private const string Path = "gateway/v1/match";

        [HttpGet]
        public async Task<IActionResult> GetMatchesAsync([FromQuery] MatchQuery q)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = Host,
                Path = Path,
                Query = $"category={q.Category}&status={q.Status}&limit={q.Limit}&page={q.Page}"
            };

            var webRequest = WebRequest.Create(uriBuilder.Uri);
            var response = await webRequest.GetResponseAsync();

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                return Ok(await stream.ReadToEndAsync());
            }
        }
    }

    public class MatchQuery
    {
        public string Category { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }

        public string Status { get; set; }
    }
}