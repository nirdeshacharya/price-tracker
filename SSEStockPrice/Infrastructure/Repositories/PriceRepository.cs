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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SSEPrice>> GetPriceHistoryAsync(string symbol, int days, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

        }

        public async Task SavePriceAsync(SSEPrice price, CancellationToken cancellationToken = default)
        {
            await _dbContext.SSEPrices.AddAsync(price);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
