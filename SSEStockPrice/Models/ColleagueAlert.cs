namespace SSEStockPrice.Models
{
    public class ColleagueAlert
    {
        public int Id { get; set; }
        public string ColleagueName { get; set; }
        public string Email { get; set; }
        public decimal TargetPrice { get; set; }
        public string Direction { get; set; } // "Above" or "Below"
        public bool IsActive { get; set; }
    }
}