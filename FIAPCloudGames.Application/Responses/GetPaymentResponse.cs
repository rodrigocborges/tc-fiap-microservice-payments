using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Application.Responses
{
    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
