using frontend.Models.Api;

namespace frontend.Models.View
{
    public class ConcertListModel
    {
        public IEnumerable<Concert> Concerts { get; set; } = new List<Concert>();
        public int NumberOfItems { get; set; }
    }
}
