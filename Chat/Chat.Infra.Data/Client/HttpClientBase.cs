using Chat.Domain.Interfaces.Client;
using System.Text.Json;

namespace Chat.Infra.Data.Client
{
    public abstract class HttpClientBase : IHttpClientBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public HttpClientBase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(this.GetType().Name);
        }

        public async Task<TResponse> GetAsync<TResponse>(IDictionary<string, string> headers, string uri)
        {
            TResponse obj = default(TResponse);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseStream);
            }
            else
            {
                return obj;
            }
        }
    }
}
