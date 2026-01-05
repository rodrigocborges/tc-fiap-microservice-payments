using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Events;
using FIAPCloudGames.Domain.Interfaces;
using System.Text.Json;
using System.Text;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FIAPCloudGames.Application.Consumers;

public class PaymentProcessingConsumer : IConsumer<PaymentCreatedEvent>
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentProcessingConsumer> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentProcessingConsumer(IPaymentService paymentService, ILogger<PaymentProcessingConsumer> logger, IHttpClientFactory httpClientFactory)
    {
        _paymentService = paymentService;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Consume(ConsumeContext<PaymentCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"[Consumer] Iniciando processamento do pagamento {message.PaymentId}...");

        // Simula tempo de processamento (ex: gateway de cartão)
        await Task.Delay(5000);

        // Lógica Fake de Aprovação (80% de chance de aprovar)
        var isApproved = new Random().Next(0, 10) > 2;
        var newStatus = isApproved ? PaymentStatus.Approved : PaymentStatus.Repproved;

        await _paymentService.UpdateStatus(message.PaymentId, newStatus);

        _logger.LogInformation($"[Consumer] Pagamento {message.PaymentId} processado. Status: {newStatus}");

        await NotifyWebhook(message.PaymentId, newStatus);
    }

    private async Task NotifyWebhook(Guid paymentId, PaymentStatus status)
    {
        try
        {
            //Para carater de estudos, deixei fora do appsettings.json tanto a url quanto o token de webhook!
            var webhookUrl = "https://callback-tc-fiap-payment.free.beeceptor.com?token=523f7db8-cd8f-4c16-ba7b-ee3084dcad83";

            var client = _httpClientFactory.CreateClient();
            var payload = new { PaymentId = paymentId, Status = status.ToString(), UpdatedAt = DateTime.UtcNow };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            await client.PostAsync(webhookUrl, content);
            _logger.LogInformation($"[Webhook] Notificação enviada para {webhookUrl}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[Webhook] Erro ao notificar: {ex.Message}");
        }
    }
}