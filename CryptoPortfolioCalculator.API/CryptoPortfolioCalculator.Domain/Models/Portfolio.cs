namespace CryptoPortfolioCalculator.Domain.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public List<PortfolioItem> Items { get; set; } = new List<PortfolioItem>();

        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal OverallChangePercentage { get; set; }
    }
}