using SSEStockPrice.Models;


namespace SSEStockPrice.Interfaces
{
    public interface IPriceRepository
    {
        Task SavePriceAsync(SSEPrice price, CancellationToken cancellationToken = default);
        Task<SSEPrice?> GetLatestPriceAsync(string symbol, CancellationToken cancellationToken = default);
        Task<IEnumerable<SSEPrice>> GetPriceHistoryAsync(string symbol, int days, CancellationToken cancellationToken = default);

    }
}
