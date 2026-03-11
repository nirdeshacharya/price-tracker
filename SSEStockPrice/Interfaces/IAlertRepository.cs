using SSEStockPrice.Models;

namespace SSEStockPrice.Interfaces
{
    public interface IAlertRepository
    {
        Task<IEnumerable<ColleagueAlert>> GetActiveAlertAsync(string symbol, CancellationToken cancellationToken = default);

        Task DeactivateAlertAsync(int id, CancellationToken cancellationToken = default);
    }
}
