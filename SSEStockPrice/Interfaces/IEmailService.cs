using SSEStockPrice.Models;

namespace SSEStockPrice.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAlertAsync(PriceAlertMessage priceAlertMessage, CancellationToken cancellationToken = default);
    }
}
