using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.API.Endpoints;

public static class PaymentEndpoints
{

    public static WebApplication MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/payments");

        group.MapGet("/pending", async (IPaymentService service, [FromQuery] Guid? userId = null) => 
        {
            var data = userId != null
                ? await service.FindAllPendingByUserId(userId.Value)
                : await service.FindAllPending();

            return Results.Ok(data?.Select(m => new GetPaymentResponse
            {
                Id = m.Id,
                GameId = m.GameId,
                Status = m.Status,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                UserId = m.UserId
            }));
        });

        group.MapGet("/{id:guid}", async (IPaymentService service, [FromRoute] Guid id) =>
        {
            var data = await service.Find(id: id);

            if (data == null)
                return Results.NotFound();

            return Results.Ok(new GetPaymentResponse
            {
                Id = data.Id,
                GameId = data.GameId,
                Status = data.Status,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                UserId = data.UserId
            });
        });

        group.MapPost("/", async (IPaymentService service, [FromBody] CreatePaymentRequest request) =>
        {
            try
            {
                if (request == null)
                    throw new InvalidOperationException("Invalid body");

                Guid id = await service.Create(new Domain.Entities.Payment(request.GameId, request.UserId));

                return Results.Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new GenericMessageResponse { Message = ex.Message });
            }

        });

        group.MapPatch("/{id:guid}", async (IPaymentService service, [FromRoute] Guid id, [FromBody] UpdateStatusPaymentRequest request) =>
        {
            try
            {
                if (request == null)
                    throw new InvalidOperationException("Invalid body");

                await service.UpdateStatus(id: id, newStatus: request.Status);

                return Results.Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new GenericMessageResponse { Message = ex.Message });
            }

        });

        return app;
    }
}
