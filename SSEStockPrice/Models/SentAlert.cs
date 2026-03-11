using SSEStockPrice.Models.Enums;

namespace SSEStockPrice.Models
{
    public class SentAlert
    {
        public int Id { get; set; }
        public string ColleagueName { get; set; }
        public string Email { get; set; }
        public string Symbol { get; set; }
        public decimal TargetPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public AlertDirection Direction { get; set; }
        public DateTimeOffset SentAt { get; set; }
    }
}
