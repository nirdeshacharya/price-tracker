namespace SSEStockPrice.Models
{
    public class SSEPrice
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public long Volume { get; set; }
    }
}