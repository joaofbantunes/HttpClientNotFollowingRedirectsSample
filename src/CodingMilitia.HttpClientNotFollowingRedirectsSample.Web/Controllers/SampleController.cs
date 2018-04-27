using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.HttpClientNotFollowingRedirectsSample.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/sample")]
    public class SampleController : Controller
    {
        private readonly TrackingHttpClientWrapperV2 _tracker;
        private readonly ILogger<SampleController> _logger;

        public SampleController(TrackingHttpClientWrapperV2 tracker, ILogger<SampleController> logger)
        {
            _tracker = tracker;
            _logger = logger;
        }

        //To test redirects use http://localhost:5000/api/sample/thing?trackingUrl=http%3A%2F%2Fhttpstat.us%2F302
        //To test no redirects use http://localhost:5000/api/sample/thing?trackingUrl=http%3A%2F%2Fhttpstat.us%2F200
        [Route("thing")]
        public async Task<IActionResult> TrackAsync(string trackingUrl, CancellationToken ct)
        {
            var targetUrl = await _tracker.TrackAsync(trackingUrl, ct);
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                _logger.LogInformation("No target url -> it wasn't a redirect");
            }
            else
            {
                _logger.LogInformation("Target url: \"{targetUrl}\" -> it was a redirect", targetUrl);
            }
            return Ok(new {targetUrl});
        }
    }
}