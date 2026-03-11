using SSEStockPrice.Models.Enums;

namespace SSEStockPrice.Models
{
    public class PriceAlertMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public string ColleagueName { get; set; }
        public string Email { get; set; }
        public AlertDirection Direction { get; set; }
    }
}
