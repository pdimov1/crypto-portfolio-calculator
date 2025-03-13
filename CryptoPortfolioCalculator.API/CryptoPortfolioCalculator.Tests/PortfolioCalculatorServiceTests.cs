using CryptoPortfolioCalculator.Application.Services;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Tests
{
    public class PortfolioCalculatorServiceTests
    {
        private readonly PortfolioCalculatorService _service = new PortfolioCalculatorService();

        [Fact]
        public void CalculatePortfolio_AllCryptoPricesProvided_ReturnsCorrectPortfolioValue()
        {
            // Arrange
            var assets = new List<PortfolioItem>
            {
                new PortfolioItem { Symbol = TestConstants.BitcoinSymbol, Quantity = 2, InitialPrice = 30000 },
                new PortfolioItem { Symbol = TestConstants.EthereumSymbol, Quantity = 5, InitialPrice = 2000 }
            };

            var cryptoCurrencyPrices = new List<CryptoCurrency>
            {
                new CryptoCurrency { Symbol = TestConstants.BitcoinSymbol, CurrentPrice = TestConstants.BitcoinPrice },
                new CryptoCurrency { Symbol = TestConstants.EthereumSymbol, CurrentPrice = TestConstants.EthereumPrice }
            };

            // Act
            var result = _service.CalculatePortfolio(assets, cryptoCurrencyPrices);

            // Verify
            Assert.NotNull(result);
            Assert.Equal(70000, result.InitialValue);
            Assert.Equal(112500, result.CurrentValue);
            Assert.Equal(60.71m, result.OverallChangePercentage, 2);
        }

        [Fact]
        public void CalculatePortfolio_MissingPrices_ReturnsZeroPrice()
        {
            // Arrange
            var assets = new List<PortfolioItem>
            {
                new PortfolioItem { Symbol = TestConstants.BitcoinSymbol, Quantity = 2 },
                new PortfolioItem { Symbol = TestConstants.EthereumSymbol, Quantity = 5 }
            };

            var cryptoCurrencyPrices = new List<CryptoCurrency>
            {
                new CryptoCurrency { Symbol = TestConstants.BitcoinSymbol, CurrentPrice = TestConstants.BitcoinPrice }
            };

            // Act
            var result = _service.CalculatePortfolio(assets, cryptoCurrencyPrices);

            // Verify
            var bitcoinItem = result.Items.FirstOrDefault(i => i.Symbol == TestConstants.BitcoinSymbol);
            var ethereumItem = result.Items.FirstOrDefault(i => i.Symbol == TestConstants.EthereumSymbol);

            Assert.NotNull(bitcoinItem);
            Assert.Equal(TestConstants.BitcoinPrice, bitcoinItem.CurrentPrice);
            Assert.Null(ethereumItem);

            Assert.Equal(100000, result.CurrentValue);
        }

        [Fact]
        public void CalculatePortfolio_NoMatchingSymbols_RetrunZeroPrice()
        {
            // Arrange
            var assets = new List<PortfolioItem>
            {
                new PortfolioItem { Symbol = TestConstants.BitcoinSymbol, Quantity = 2 }
            };

            var cryptoCurrencyPrices = new List<CryptoCurrency>();

            // Act
            var result = _service.CalculatePortfolio(assets, cryptoCurrencyPrices);

            // Verify
            Assert.Empty(result.Items);
            Assert.Equal(0, result.CurrentValue);
        }

        [Fact]
        public void CalculatePortfolio_ContainsZeroOrNegativeQuantities_ExcludesInvalidEntries()
        {
            // Arrange
            var assets = new List<PortfolioItem>
            {
                new PortfolioItem { Symbol = TestConstants.BitcoinSymbol, Quantity = -1 },
                new PortfolioItem { Symbol = TestConstants.EthereumSymbol, Quantity = 0 }
            };

            var prices = new List<CryptoCurrency>
            {
                new CryptoCurrency {Symbol = TestConstants.BitcoinSymbol, CurrentPrice = TestConstants.BitcoinPrice},
                new CryptoCurrency { Symbol = TestConstants.EthereumSymbol, CurrentPrice = TestConstants.EthereumPrice }
            };

            // Act
            var result = _service.CalculatePortfolio(assets, prices);

            // Verify
            Assert.Equal(TestConstants.BitcoinPrice, result.Items.First(i => i.Symbol == TestConstants.BitcoinSymbol).CurrentPrice);
            Assert.Equal(TestConstants.EthereumPrice, result.Items.First(i => i.Symbol == TestConstants.EthereumSymbol).CurrentPrice);
        }
    }
}