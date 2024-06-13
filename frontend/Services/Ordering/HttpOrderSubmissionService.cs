using frontend.Models.Api;
using frontend.Models.View;
using frontend.Services.ShoppingBasket;
using Prometheus;

namespace frontend.Services.Ordering
{
    public class HttpOrderSubmissionService : IOrderSubmissionService
    {
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly HttpClient _httpClient;
        private static ICounter ticketSold = null;

        public HttpOrderSubmissionService(IShoppingBasketService shoppingBasketService, HttpClient httpClient)
        {
            _shoppingBasketService = shoppingBasketService;
            _httpClient = httpClient;
        }

        public async Task<Guid> SubmitOrder(CheckoutViewModel model)
        {
            var lines = await _shoppingBasketService.GetLinesForBasket(model.BasketId);
            var order = new OrderForCreation
            {
                Date = DateTimeOffset.Now,
                OrderId = Guid.NewGuid(),
                Lines = lines.Select(l => new OrderLine
                {
                    ConcertId = l.ConcertId,
                    Price = l.Price,
                    TicketCount = l.TicketAmount
                }).ToList(),
                CustomerDetails = new CustomerDetails
                {
                    Address = model.Address,
                    CreditCardExpiryDate = model.CreditCardDate,
                    CreditCardNumber = model.CreditCard,
                    Email = model.Email,
                    Name = model.Name,
                    PostalCode = model.PostalCode,
                    Town = model.Town
                }
            };
            SendTelemetryOrderPlaced(lines.Sum(l => l.TicketAmount));
            var response = await _httpClient.PostAsJsonAsync("order", order);
            var success = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return order.OrderId;
        }

        private void SendTelemetryOrderPlaced(int numOfTickets)
        {
            if (ticketSold == null)
                ticketSold = 
                    Metrics.CreateCounter("globoticket_tickets_sold", "Number of tickets in shopping basket on checkout");

            ticketSold.Inc(Convert.ToDouble(numOfTickets));
        }
    }
}
