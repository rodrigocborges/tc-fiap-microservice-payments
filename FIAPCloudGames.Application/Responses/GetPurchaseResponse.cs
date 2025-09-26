namespace FIAPCloudGames.Application.Responses
{
    public class GetPurchaseResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
