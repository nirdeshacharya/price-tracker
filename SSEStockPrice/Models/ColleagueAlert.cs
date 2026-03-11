using SSEStockPrice.Models.Enums;

namespace SSEStockPrice.Models
{
    public class ColleagueAlert
    {
        public int Id { get; set; }
        public string ColleagueName { get; set; }
        public string Email { get; set; }
        public string Symbol { get; set; }
        public decimal TargetPrice { get; set; }
        public AlertDirection Direction { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
