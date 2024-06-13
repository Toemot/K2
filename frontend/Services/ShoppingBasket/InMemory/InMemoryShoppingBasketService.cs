using frontend.Models;
using frontend.Models.Api;
using frontend.Services.ConcertCatalog;

namespace frontend.Services.ShoppingBasket.InMemory
{
    public class InMemoryShoppingBasketService : IShoppingBasketService
    {
        private readonly Dictionary<Guid, InMemoryBasket> baskets;
        private readonly Dictionary<Guid, Concert> concertCache;
        private readonly IConcertCatalogService _concertCatalogService;
        private readonly Settings _settings;
        public InMemoryShoppingBasketService(IConcertCatalogService concertCatalogService, Settings settings)
        {
            _concertCatalogService = concertCatalogService;
            _settings = settings;
            baskets = new Dictionary<Guid, InMemoryBasket>();
            concertCache = new Dictionary<Guid, Concert>();
        }

        public async Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine)
        {
            if (!baskets.TryGetValue(basketId, out var basket))
            {
                basket = new InMemoryBasket(_settings.UserId);
                baskets.Add(basket.BasketId, basket);
            }

            if (!concertCache.TryGetValue(basketLine.ConcertId, out var concert))
            {
                concert = await _concertCatalogService.GetConcertAsync(basketLine.ConcertId);
                concertCache.Add(basketLine.ConcertId, concert);
            }

            return basket.Add(basketLine, concert);
        }

        public Task ClearBasket(Guid basketId)
        {
            if (baskets.TryGetValue(basketId, out var basket))
            {
                basket.Clear();
            }
            return Task.CompletedTask;
        }

        public async Task<Basket> GetBasket(Guid basketId)
        {
            baskets.TryGetValue(basketId, out var basket);

            return await Task.FromResult(new Basket
            {
                BasketId = basketId,
                NumberOfItems = basket?.Lines?.Count ?? 0,
                UserId = basket?.UserId ?? Guid.Empty,
            });
        }

        public async Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId)
        {
            if (!baskets.TryGetValue(basketId, out var basket))
            {
                return new BasketLine[0];
            }
            return await Task.FromResult(basket.Lines);
        }

        public async Task RemoveLine(Guid basketId, Guid lineId)
        {
            if (baskets.TryGetValue(basketId, out var basket))
            {
                basket.RemoveLine(lineId);
            }
        }

        public async Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate)
        {
            if (baskets.TryGetValue(basketId, out var basket))
            {
                basket.Update(basketLineForUpdate);
            }
        }
    }
}
