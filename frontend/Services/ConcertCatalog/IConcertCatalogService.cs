using frontend.Models.Api;

namespace frontend.Services.ConcertCatalog
{
    public interface IConcertCatalogService
    {
        Task<IEnumerable<Concert>> GetConcertsAsync();
        Task<Concert> GetConcertAsync(Guid concertId);
    }
}
