using SSEStockPrice.Infrastructure.Data;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;

namespace SSEStockPrice.Infrastructure.Repositories
{
    public class SentAlertRepository : ISentAlertRepository
    {

        private readonly AppDbContext _dbContext;
        public SentAlertRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveToSentAlertAsync(SentAlert sentAlert, CancellationToken cancellationToken = default)
        {
           await _dbContext.SentAlerts.AddAsync(sentAlert, cancellationToken);
           await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
