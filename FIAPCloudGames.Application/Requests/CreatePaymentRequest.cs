
namespace FIAPCloudGames.Application.Requests
{
    public class CreatePaymentRequest
    {
        public required Guid GameId { get; set; }
        public required Guid UserId { get; set; }
    }
}
