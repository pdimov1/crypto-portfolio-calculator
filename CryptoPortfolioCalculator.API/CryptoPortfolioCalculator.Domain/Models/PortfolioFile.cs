namespace CryptoPortfolioCalculator.Domain.Models
{
    public class PortfolioFile
    {
        public string FileName { get; set; }
        public long Length { get; set; }
        public Stream FileContent { get; set; }
    }
}