namespace CryptoPortfolioCalculator.Models
{
    public class PortfolioViewModel
    {
        public Portfolio Portfolio { get; set; }
    }
    
    public class Portfolio
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<PortfolioItem> Items { get; set; } = new List<PortfolioItem>();
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal OverallChangePercentage { get; set; }
    }

    public class PortfolioItem
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Symbol { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ChangePercentage { get; set; }
    }
}
