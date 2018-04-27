using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.HttpClientNotFollowingRedirectsSample.Web
{
    public class TrackingHttpClientWrapper
    {
        private HttpClient _httpClient;

        public TrackingHttpClientWrapper()
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            _httpClient = new HttpClient(handler);
        }

        public async Task<string> TrackAsync(string trackingUrl, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(trackingUrl, ct);

            return response.StatusCode == HttpStatusCode.Redirect
                 ? response.Headers.Location.OriginalString
                 : null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }

            _httpClient = null;
        }
        
        ~TrackingHttpClientWrapper()
        {
            Dispose(false);
        }
    }
}