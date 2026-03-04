namespace SSEStockPrice.Models
{
    public class SSEDailySummary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ChangePercent { get; set; }
    }
}