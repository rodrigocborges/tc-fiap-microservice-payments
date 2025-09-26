using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.API.Endpoints;

public static class PurchaseEndpoints
{

    public static WebApplication MapPurchaseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/purchases");

        group.MapGet("/{id:guid}", async (IPurchaseService service, [FromRoute] Guid id) =>
        {
            var data = await service.FindAllByUserId(userId: id);

            if (data == null)
                return Results.NotFound();

            return Results.Ok(data?.Select(m => new GetPurchaseResponse
            {
                Id = m.Id,
                CreatedAt = m.CreatedAt,
                GameId = m.GameId,
                UserId = m.UserId
            }));
        });

        return app;
    }
}
