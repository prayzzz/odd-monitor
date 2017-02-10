using Microsoft.AspNetCore.Mvc;

namespace PlayTheOdds.Heartbeat
{
    [Route("api/v1/heartbeat")]
    public class HeartbeatController : Controller
    {
        private readonly IHeartbeat _heartbeat;

        public HeartbeatController(IHeartbeat heartbeat)
        {
            _heartbeat = heartbeat;
        }

        [HttpGet]
        public void Heartbeat()
        {
            _heartbeat.ResetHeartbeat();
        }
    }
}