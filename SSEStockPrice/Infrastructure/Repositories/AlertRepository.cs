using Microsoft.EntityFrameworkCore;
using SSEStockPrice.Infrastructure.Data;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;

namespace SSEStockPrice.Infrastructure.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext _dbContext;
        public AlertRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeactivateAlertAsync(int id, CancellationToken cancellationToken = default)
        {
            var colleagueAlert = await _dbContext.ColleagueAlerts.Where(x=> x.Id == id).FirstOrDefaultAsync(cancellationToken);
            if (colleagueAlert != null)
            {
                colleagueAlert.IsActive = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<ColleagueAlert>> GetActiveAlertAsync(string symbol, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ColleagueAlerts.Where(x => x.IsActive && x.Symbol == symbol).ToListAsync(cancellationToken);
        }
    }
}
