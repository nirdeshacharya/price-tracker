using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSEStockPrice.Function;

public class PriceAlertNotificationFunction
{
    private readonly ILogger<PriceAlertNotificationFunction> _logger;
    private readonly IEmailService _emailService;
    private readonly ISentAlertRepository _sentAlertRepo;

    public PriceAlertNotificationFunction(IEmailService emailService, ISentAlertRepository sentAlertRepository, ILogger<PriceAlertNotificationFunction> logger)
    {
        _logger = logger;
        _emailService = emailService;
        _sentAlertRepo = sentAlertRepository;
    }

    [Function("PriceAlertNotificationFunction")]
    public async Task Run(
     [ServiceBusTrigger("sse-price-alert", Connection = "ServiceBusConnection")]
    ServiceBusReceivedMessage message,
     CancellationToken cancellationToken)
    {
        var priceAlertMessage = JsonSerializer.Deserialize<PriceAlertMessage>(message.Body);
        if (priceAlertMessage == null)
        {
            _logger.LogWarning("Service Bus message failed to deserialise.");
            return;
        }

        await _emailService.SendEmailAlertAsync(priceAlertMessage, cancellationToken);
        _logger.LogInformation("Email Sent to {Email}", priceAlertMessage.Email);


        var sentAlert = new SentAlert 
          {
                Email = priceAlertMessage.Email,
                ColleagueName = priceAlertMessage.ColleagueName,
                CurrentPrice = priceAlertMessage.CurrentPrice,
                Direction = priceAlertMessage.Direction,
                SentAt = DateTimeOffset.UtcNow,
                Symbol = priceAlertMessage.Symbol,
                TargetPrice = priceAlertMessage.TargetPrice,
          };


        await _sentAlertRepo.SaveToSentAlertAsync(sentAlert, cancellationToken);
        _logger.LogInformation("Saved to SentAlert for {Email}", priceAlertMessage.Email);

    }
}