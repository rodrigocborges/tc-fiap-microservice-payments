namespace FIAPCloudGames.Domain.Events;

public record PaymentCreatedEvent(Guid PaymentId, Guid GameId, Guid UserId, decimal Amount);