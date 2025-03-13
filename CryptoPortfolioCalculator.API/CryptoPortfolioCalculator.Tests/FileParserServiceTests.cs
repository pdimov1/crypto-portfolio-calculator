using CryptoPortfolioCalculator.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace CryptoPortfolioCalculator.Tests
{

    public class FileParserServiceTests
    {
        private readonly FileParserService _service;
        private readonly Mock<ILogger<FileParserService>> _mockLogger;

        public FileParserServiceTests()
        {
            _mockLogger = new Mock<ILogger<FileParserService>>();
            _service = new FileParserService(_mockLogger.Object);
        }

        [Fact]
        public async Task ParsePortfolioFileAsync_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = await _service.ParsePortfolioFileAsync(stream);

            // Verify
            Assert.Empty(result);
        }

        [Fact]
        public async Task ParsePortfolioFileAsync_IncorrectFormat_SkipsInvalidLines()
        {
            // Arrange
            var fileContent = "1.5|BTC\nInvalidLineWithoutSeparator\n2.3|ETH|2500.50";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            // Act
            var result = await _service.ParsePortfolioFileAsync(stream);


            // Verify
            Assert.Single(result);
            Assert.Equal(2.3m, result[0].Quantity);
            Assert.Equal(TestConstants.EthereumSymbol, result[0].Symbol);
            Assert.Equal(2500.50m, result[0].InitialPrice);
        }

        [Fact]
        public async Task ParsePortfolioFileAsync_NonNumericValues_SkipsInvalidLines()
        {
            // Arrange
            var fileContent = "ABC|BTC|50000\n2.3|ETH|XYZ";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            // Act
            var result = await _service.ParsePortfolioFileAsync(stream);

            // Verify
            Assert.Empty(result);
        }

        [Fact]
        public async Task ParsePortfolioFileAsync_ValidData_ParsesCorrectly()
        {
            // Arrange
            var fileContent = "1.5|BTC|50000\n2.3|ETH|2500.50";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            // Act
            var result = await _service.ParsePortfolioFileAsync(stream);

            // Verify
            Assert.Equal(2, result.Count);
            Assert.Equal(1.5m, result[0].Quantity);
            Assert.Equal(TestConstants.BitcoinSymbol, result[0].Symbol);
            Assert.Equal(TestConstants.BitcoinPrice, result[0].InitialPrice);

            Assert.Equal(2.3m, result[1].Quantity);
            Assert.Equal(TestConstants.EthereumSymbol, result[1].Symbol);
            Assert.Equal(2500.50m, result[1].InitialPrice);
        }
    }
}