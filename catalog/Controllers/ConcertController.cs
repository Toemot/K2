using catalog.Model;
using catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConcertController : ControllerBase
    {
        private readonly IConcertRepository _concertRepository;

        public ConcertController(IConcertRepository concertRepository)
        {
            _concertRepository = concertRepository;
        }

        [HttpGet(Name = "GetConcerts")]
        public ActionResult<IEnumerable<Concert>> GetConcerts()
        {
            var concerts = _concertRepository.GetConcerts();
            return Ok(concerts);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult> GetConcertById(Guid id) 
        {
            var concert = await _concertRepository.GetConcertById(id);
            return Ok(concert);
        }

    }
}
