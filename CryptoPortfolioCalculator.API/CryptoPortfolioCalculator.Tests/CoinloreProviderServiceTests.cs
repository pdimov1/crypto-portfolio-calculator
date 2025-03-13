using CryptoPortfolioCalculator.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CryptoPortfolioCalculator.Tests
{
    public class CoinloreProviderServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly CoinloreProviderService _service;
        private readonly Mock<ILogger<CoinloreProviderService>> _mockLogger;

        public CoinloreProviderServiceTests()
        {
            _mockLogger = new Mock<ILogger<CoinloreProviderService>>();
            _mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_mockHandler.Object) { BaseAddress = new Uri("https://api.coinlore.net/api/") };

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(_ => _.CreateClient("CryptoCurrencyAPI")).Returns(httpClient);

            _service = new CoinloreProviderService(_mockHttpClientFactory.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_ValidApiResponse_ReturnsMappedData()
        {
            // Arrange
            var responseContent = JsonSerializer.Serialize(new CryptoCurrencyResponse
            {
                Data = new List<CryptoCurrencyData>
            {
                new() { Symbol = TestConstants.BitcoinSymbol, Name = "Bitcoin", PriceUsd = TestConstants.BitcoinPrice },
                new() { Symbol = TestConstants.EthereumSymbol, Name = "Ethereum", PriceUsd = TestConstants.EthereumPrice }
            }
            });

            SetupHttpResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

            // Act
            var result = await _service.GetCryptoCurrenciesAsync(new List<string> { TestConstants.BitcoinSymbol, TestConstants.EthereumSymbol });

            // Verify
            Assert.Equal(2, result.Count());
            Assert.Equal(TestConstants.BitcoinPrice, result.First(c => c.Symbol == TestConstants.BitcoinSymbol).CurrentPrice);
            Assert.Equal(TestConstants.EthereumPrice, result.First(c => c.Symbol == TestConstants.EthereumSymbol).CurrentPrice);
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_EmptyApiResponse_ReturnsEmptyList()
        {
            // Arrange
            var responseContent = JsonSerializer.Serialize(new CryptoCurrencyResponse { Data = new List<CryptoCurrencyData>() });
            SetupHttpResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

            // Act
            var result = await _service.GetCryptoCurrenciesAsync(new List<string> { TestConstants.BitcoinSymbol });

            // Verify
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_ApiFailure_ThrowsException()
        {
            // Arrange
            SetupHttpResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            // Act
            // Verify
            await Assert.ThrowsAsync<CryptoProviderException>(() => _service.GetCryptoCurrenciesAsync(new List<string> { TestConstants.BitcoinSymbol }));
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_NullSymbolsList_ReturnsEmptyCollection()
        {
            // Act
            var result = await _service.GetCryptoCurrenciesAsync(null);

            // Verify
            Assert.Empty(result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("null or empty symbols list")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_OperationCanceled_ThrowsOperationCanceledException()
        {
            // Arrange
            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new OperationCanceledException());

            // Act
            // Verify
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _service.GetCryptoCurrenciesAsync(new List<string> { TestConstants.BitcoinSymbol }));
        }

        [Fact]
        public async Task GetCryptoCurrenciesAsync_DeserializationError_ThrowsCryptoProviderException()
        {
            // Arrange
            SetupHttpResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("invalid json content")
            });

            // Act & Verify
            var exception = await Assert.ThrowsAsync<CryptoProviderException>(() =>
                _service.GetCryptoCurrenciesAsync(new List<string> { TestConstants.BitcoinSymbol }));

            Assert.Contains("Invalid data received", exception.Message);
        }

        private void SetupHttpResponse(HttpResponseMessage responseMessage, bool throwException = false)
        {
            _mockHandler.Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(responseMessage);
        }
    }
}