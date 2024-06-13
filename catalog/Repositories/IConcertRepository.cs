using catalog.Model;

namespace catalog.Repositories
{
    public interface IConcertRepository
    {
        IEnumerable<Concert> GetConcerts();
        Task<Concert> GetConcertById(Guid concertId);
    }
}
