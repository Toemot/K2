using frontend.Extensions;
using frontend.Models.Api;
using frontend.Models.View;
using frontend.Models;
using frontend.Services.ShoppingBasket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights;

namespace frontend.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private readonly IShoppingBasketService basketService;
        private readonly Settings settings;
        private readonly ILogger<ShoppingBasketController> logger;
        //private readonly TelemetryClient telemetryClient;

        public ShoppingBasketController(IShoppingBasketService basketService,/* TelemetryClient telemetryClient,*/ Settings settings, ILogger<ShoppingBasketController> logger)
        {
            this.basketService = basketService;
            this.settings = settings;
            this.logger = logger;
            //this.telemetryClient = telemetryClient;
        }

        public async Task<IActionResult> Index()
        {
            var basketLines = await basketService.GetLinesForBasket(Request.Cookies.GetCurrentBasketId(settings));
            var lineViewModels = basketLines.Select(bl => new BasketLineViewModel
            {
                LineId = bl.BasketLineId,
                ConcertId = bl.ConcertId,
                ConcertName = bl.Concert.Name,
                Date = bl.Concert.Date,
                Price = bl.Price,
                Quantity = bl.TicketAmount
            }
            );
            return View(lineViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLine(BasketLineForCreation basketLine)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            var newLine = await basketService.AddToBasket(basketId, basketLine);
            Response.Cookies.Append(settings.BasketIdCookieName, newLine.BasketId.ToString());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLine(BasketLineForUpdate basketLineUpdate)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.UpdateLine(basketId, basketLineUpdate);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveLine(Guid lineId)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.RemoveLine(basketId, lineId);
            return RedirectToAction("Index");
        }

    }
}
