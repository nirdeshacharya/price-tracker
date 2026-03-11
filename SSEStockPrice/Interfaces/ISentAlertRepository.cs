using SSEStockPrice.Models;

namespace SSEStockPrice.Interfaces
{
    public interface ISentAlertRepository
    {
        Task SaveToSentAlertAsync(SentAlert sentAlert, CancellationToken cancellationToken = default);
    }
}
