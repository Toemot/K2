using frontend.Extensions;
using frontend.Models;
using frontend.Models.View;
using frontend.Services.Ordering;
using frontend.Services.ShoppingBasket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace frontend.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly IOrderSubmissionService _orderSubmissionService;
        private readonly Settings _settings;
        private readonly ILogger<CheckoutController> _logger;   
        public CheckoutController(IShoppingBasketService shoppingBasketService, 
            IOrderSubmissionService orderSubmissionService, Settings settings, ILogger<CheckoutController> logger)
        {
            _shoppingBasketService = shoppingBasketService;
            _orderSubmissionService = orderSubmissionService;
            _settings = settings;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);

            return View(new CheckoutViewModel { BasketId = currentBasketId });
        }

        public IActionResult Thanks()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(CheckoutViewModel checkout)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            if (ModelState.IsValid)
            {
                var currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);
                checkout.BasketId = currentBasketId;

                _logger.LogInformation($"Received an order from {checkout.Name}");

                var orderId = await _orderSubmissionService.SubmitOrder(checkout);
                await _shoppingBasketService.ClearBasket(currentBasketId);

                return RedirectToAction("Thanks");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
