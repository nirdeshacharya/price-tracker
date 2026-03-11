using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;
using SSEStockPrice.Models.Enums;

namespace SSEStockPrice.Function;

public class PriceAlertFunction
{
    private readonly ILogger<PriceAlertFunction> _logger;
    private readonly IAlertRepository _alertRepository;
    private readonly IServiceBusPublisher _serviceBusPublisher;

    public PriceAlertFunction(ILogger<PriceAlertFunction> logger, IAlertRepository alertRepository, IServiceBusPublisher serviceBusPublisher)
    {
        _logger = logger;
        _alertRepository = alertRepository;
        _serviceBusPublisher = serviceBusPublisher;
    }

    [Function("PriceAlertFunction")]
    public async Task Run(
     [ServiceBusTrigger("sse-price-ingested", Connection = "ServiceBusConnection")]
    ServiceBusReceivedMessage message,
     CancellationToken cancellationToken)
    {
        var priceMessage = JsonSerializer.Deserialize<PriceIngestedMessage>(message.Body);
        if (priceMessage == null)
        {
            _logger.LogWarning("Service Bus message failed to deserialise.");
        }
        var activeAlert = await _alertRepository.GetActiveAlertAsync(priceMessage.Symbol, cancellationToken);
        foreach(var item in activeAlert)
        {
            var thresholdBreached = (item.Direction == AlertDirection.Above && priceMessage.StockPrice >= item.TargetPrice) || (item.Direction == AlertDirection.Below && priceMessage.StockPrice <= item.TargetPrice);

            if (thresholdBreached)
            {
                await _alertRepository.DeactivateAlertAsync(item.Id, cancellationToken);
                var priceAlertMsg = new PriceAlertMessage
                {
                    Symbol = priceMessage.Symbol,
                    ColleagueName = item.ColleagueName,
                    CurrentPrice = priceMessage.StockPrice,
                    Direction = item.Direction,
                    Email = item.Email,
                    TargetPrice = item.TargetPrice
                };
                await _serviceBusPublisher.PublishAsync("sse-price-alert", priceAlertMsg, cancellationToken);
                _logger.LogInformation("Price alert triggered for {ColleagueName} - {Symbol} hit {TargetPrice}", item.ColleagueName, item.Symbol, item.TargetPrice);
            }
            else
            {
                _logger.LogInformation("Threshold not hit for {Symbol}", priceMessage.Symbol);
            }
        }

    }
}