using Microsoft.EntityFrameworkCore;
using SSEStockPrice.Models;


namespace SSEStockPrice.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<SSEPrice> SSEPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SSEPrice>(entity =>
            {
                entity.ToTable("SSEPrice");
                entity.HasKey(x => x.Id);
                entity.Property(x =>x.Symbol).HasMaxLength(20).IsRequired();
                entity.Property(x =>x.CurrentPrice).HasColumnType("decimal(10,4)");
                entity.Property(x =>x.OpenPrice).HasColumnType("decimal(10,4)");
                entity.Property(x =>x.HighPrice).HasColumnType("decimal(10,4)");
                entity.Property(x =>x.LowPrice).HasColumnType("decimal(10,4)");
                entity.Property(x =>x.ChangePercent).HasColumnType("decimal(6,4)");

            });
        }
    }
}
