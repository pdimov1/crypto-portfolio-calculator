namespace CryptoPortfolioCalculator.Domain.Models
{
    public class CryptoCurrency
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}