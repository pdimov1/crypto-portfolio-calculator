namespace CryptoPortfolioCalculator.DataContracts
{
    public class PortfolioDto
    {
        public List<PortfolioItemDto> Items { get; set; } = new List<PortfolioItemDto>();

        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal OverallChangePercentage { get; set; }
    }
}