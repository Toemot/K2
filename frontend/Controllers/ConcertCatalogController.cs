using frontend.Extensions;
using frontend.Models;
using frontend.Models.View;
using frontend.Services.ConcertCatalog;
using frontend.Services.ShoppingBasket;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Controllers
{
    public class ConcertCatalogController : Controller
    {
        private readonly IConcertCatalogService _concertCatalogService;
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly Settings _settings;
        public ConcertCatalogController(IConcertCatalogService concertCatalogService, 
            IShoppingBasketService shoppingBasketService, Settings settings)
        {
            _concertCatalogService = concertCatalogService;
            _shoppingBasketService = shoppingBasketService;
            _settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);

            var getBasket = _shoppingBasketService.GetBasket(currentBasketId);
            var getConcerts = _concertCatalogService.GetConcertsAsync();

            await Task.WhenAll(new Task[] { getBasket, getConcerts });  

            var numberOfItems = getBasket.Result.NumberOfItems;

            return View(new ConcertListModel 
            { 
                Concerts = getConcerts.Result, NumberOfItems = numberOfItems 
            });
        }

        public async Task<IActionResult> Detail(Guid concertId)
        {
            var ev = await _concertCatalogService.GetConcertAsync(concertId);
            return View(ev);
        }
    }
}
