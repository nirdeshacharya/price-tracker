using Microsoft.EntityFrameworkCore;
using SSEStockPrice.Infrastructure.Data;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;


namespace SSEStockPrice.Infrastructure.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly AppDbContext _dbContext;
        public PriceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SSEPrice?> GetLatestPriceAsync(string symbol, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SSEPrices.Where(x => x.Symbol == symbol).OrderByDescending(x =>x.Timestamp).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<SSEPrice>> GetPriceHistoryAsync(string symbol, int days, CancellationToken cancellationToken = default)
        {
            var fromDate = DateTimeOffset.UtcNow.AddDays(-days);

            return await _dbContext.SSEPrices
                .Where(x => x.Symbol == symbol && x.Timestamp >= fromDate)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task SavePriceAsync(SSEPrice price, CancellationToken cancellationToken = default)
        {
            await _dbContext.SSEPrices.AddAsync(price);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
