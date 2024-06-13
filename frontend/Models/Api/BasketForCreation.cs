using System.ComponentModel.DataAnnotations;

namespace frontend.Models.Api
{
    public class BasketForCreation
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
