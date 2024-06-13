using frontend.Extensions;
using frontend.Models.Api;
using System.Text.Json;

namespace frontend.Services.ConcertCatalog
{
    public class ConcertCatalogService : IConcertCatalogService
    {
        private readonly HttpClient _httpClient;

        public ConcertCatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Concert> GetConcertAsync(Guid id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"concert/{id}");
            var yes = await response.ReadContentAs<Concert>();
            return yes;
        }

        public async Task<IEnumerable<Concert>> GetConcertsAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("concert");
            return await responseMessage.ReadContentAs<List<Concert>>();
        }
    }
}
