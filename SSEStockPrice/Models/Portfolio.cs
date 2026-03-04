namespace SSEStockPrice.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string ColleagueName { get; set; }
        public int SharesOwned { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}