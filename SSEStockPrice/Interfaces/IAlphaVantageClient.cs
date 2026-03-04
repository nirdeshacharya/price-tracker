using SSEStockPrice.Models;

namespace SSEStockPrice.Interfaces
{
    public interface IAlphaVantageClient
    {
        Task<SSEPrice> GetPriceAsync(string symbol, CancellationToken cancellationToken = default);
    }
}
