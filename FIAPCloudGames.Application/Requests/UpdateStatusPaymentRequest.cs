
using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Application.Requests
{
    public class UpdateStatusPaymentRequest
    {
        public required PaymentStatus Status { get; set; }
    }
}
