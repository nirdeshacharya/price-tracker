using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;

namespace SSEStockPrice.Function
{
    public class PriceIngestionFunction
    {

        private readonly ILogger<PriceIngestionFunction> _logger;
        private readonly IAlphaVantageClient _alphaVantageClient;
        private readonly IPriceRepository _priceRepository;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public PriceIngestionFunction(ILogger<PriceIngestionFunction> logger, IAlphaVantageClient alphaVantageClient, IPriceRepository priceRepository, IServiceBusPublisher serviceBusPublisher)
        {
            _logger = logger;
            _serviceBusPublisher = serviceBusPublisher;
            _alphaVantageClient = alphaVantageClient;
            _priceRepository = priceRepository;
        }

        [Function("PriceIngestionFunction")]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, CancellationToken cancellationToken)
        {
            var ukTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            var isWeekday = ukTime.DayOfWeek >= DayOfWeek.Monday && ukTime.DayOfWeek <= DayOfWeek.Friday;
            var marketOpen = new TimeSpan(8, 0, 0);
            var marketClose = new TimeSpan(16, 30, 0);
            var isWithinBusinessHours = ukTime.TimeOfDay >= marketOpen && ukTime.TimeOfDay < marketClose;

            if (!(isWeekday && isWithinBusinessHours))
            {
                _logger.LogInformation("Out of Business Hour.");
                return;
            }

            var price = await _alphaVantageClient.GetPriceAsync("SSE.LON", cancellationToken);

            await _priceRepository.SavePriceAsync(price, cancellationToken);

            var priceIngestedMsg = new PriceIngestedMessage 
            {
                EventDateTime = DateTimeOffset.UtcNow,
                StockPrice = price.CurrentPrice,
                Symbol = price.Symbol,
            };
            await _serviceBusPublisher.PublishAsync("sse-price-ingested",priceIngestedMsg, cancellationToken);

            _logger.LogInformation("Price Ingested for {Symbol}: {Price}",price.Symbol, price.CurrentPrice);
        }
    }
}
