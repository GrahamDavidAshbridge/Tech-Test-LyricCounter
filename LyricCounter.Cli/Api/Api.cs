using System.Text.Json;

namespace LyricCounter.Cli.Api
{
    internal class Api : IApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Api(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T?> GetAsync<T>(string apiClientName, string uriExtension)
        {
            var httpClient = _httpClientFactory.CreateClient(apiClientName);
            using (var response = await httpClient.GetAsync(uriExtension))
            {
                response.EnsureSuccessStatusCode();
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(responseStream);
            }
        }
    }
}