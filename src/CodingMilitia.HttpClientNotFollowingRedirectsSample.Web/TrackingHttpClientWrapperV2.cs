using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.HttpClientNotFollowingRedirectsSample.Web
{
    public class TrackingHttpClientWrapperV2
    {
        private HttpClient _httpClient;

        public TrackingHttpClientWrapperV2(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TrackAsync(string trackingUrl, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(trackingUrl, ct);

            return response.StatusCode == HttpStatusCode.Redirect
                 ? response.Headers.Location.OriginalString
                 : null;
        }
    }
}