using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Passero.Framework.JwtTester
{
    internal static class PasseroDemoPingClient
    {
        public static async Task<string> PingAsync(
            string applicationBaseUrl,
            string? jwtToken = null,
            CancellationToken cancellationToken = default)
        {
            using (var httpClient = new HttpClient())
            {
                return await PingAsync(httpClient, applicationBaseUrl, jwtToken, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public static async Task<string> PingAsync(
            HttpClient httpClient,
            string applicationBaseUrl,
            string? jwtToken = null,
            CancellationToken cancellationToken = default)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (string.IsNullOrWhiteSpace(applicationBaseUrl))
                throw new ArgumentException("The application base url is required.", nameof(applicationBaseUrl));

            var pingUrl = BuildPingUrl(applicationBaseUrl);

            using (var request = new HttpRequestMessage(HttpMethod.Get, pingUrl))
            {
                if (!string.IsNullOrWhiteSpace(jwtToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                using (var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }

        private static string BuildPingUrl(string applicationBaseUrl)
        {
            return applicationBaseUrl.TrimEnd('/') + "/PING";
        }
    }
}