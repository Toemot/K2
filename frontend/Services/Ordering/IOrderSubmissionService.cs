using frontend.Models.View;

namespace frontend.Services.Ordering
{
    public interface IOrderSubmissionService
    {
        Task<Guid> SubmitOrder(CheckoutViewModel model);
    }
}
