using catalog.DBContext;
using catalog.Model;

namespace catalog.Repositories
{
    public class ConcertRepository : IConcertRepository
    {
        private readonly EventCatalogDbContext _dbContext;
        private readonly ILogger<ConcertRepository> _logger;

        public ConcertRepository(EventCatalogDbContext dbContext, ILogger<ConcertRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<Concert> GetConcertById(Guid concertId)
        {
            var concert = _dbContext.Concerts.FirstOrDefault(c => c.ConcertId == concertId);
            if (concert == null)
                throw new InvalidOperationException("Concert not found!");
            return Task.FromResult(concert);
        }

        public IEnumerable<Concert> GetConcerts()
        {
            return _dbContext.Concerts.ToList();
        }
    }
}
