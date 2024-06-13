using frontend.Models.Api;

namespace frontend.Services.ShoppingBasket.InMemory
{
    public class InMemoryBasket
    {
        public InMemoryBasket(Guid userId)
        {
            BasketId = Guid.NewGuid();
            UserId = userId;
            Lines = new List<BasketLine>();
        }
        public Guid BasketId { get; }
        public List<BasketLine> Lines { get; }
        public Guid UserId { get; }

        public BasketLine Add(BasketLineForCreation line, Concert concert)
        {
            var basketLine = new BasketLine
            {
                ConcertId = line.ConcertId,
                TicketAmount = line.TicketAmount,
                Concert = concert,
                BasketId = this.BasketId,
                BasketLineId = Guid.NewGuid(),
                Price = line.Price,
            };
            Lines.Add(basketLine);
            return basketLine;
        }

        public void RemoveLine(Guid lineId)
        {
            var index = Lines.FindIndex(bl => bl.BasketLineId == lineId);
            if (index >= 0) Lines.RemoveAt(index);
        }

        public void Update(BasketLineForUpdate basketLineForUpdate)
        {
            var index = Lines.FindIndex(bl => bl.BasketLineId == basketLineForUpdate.LineId);
            Lines[index].TicketAmount = basketLineForUpdate.TicketAmount;
        }

        public void Clear()
        {
            Lines.Clear();
        }
    }
}
