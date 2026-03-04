namespace SSEStockPrice.Models
{
    public class Dividend
    {
        public int Id { get; set; }
        public DateTime ExDividendDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPerShare { get; set; }
        public bool IsPaid { get; set; }
    }
}