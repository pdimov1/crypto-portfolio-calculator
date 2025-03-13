using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoPortfolioCalculator.Application.Services
{
    public class CoinloreProviderService : ICryptoProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoinloreProviderService> _logger;

        public CoinloreProviderService(IHttpClientFactory httpClientFactory, ILogger<CoinloreProviderService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("CryptoCurrencyAPI");
            _logger = logger;
        }

        public async Task<IEnumerable<CryptoCurrency>> GetCryptoCurrenciesAsync(List<string> symbols, CancellationToken cancellationToken = default)
        {
            if (symbols == null || !symbols.Any())
            {
                _logger.LogWarning("GetCryptoCurrenciesAsync called with null or empty symbols list");
                return Enumerable.Empty<CryptoCurrency>();
            }

            try
            {
                var response = await _httpClient.GetAsync("tickers/", cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadFromJsonAsync<CryptoCurrencyResponse>(cancellationToken);
                var cryptos = content.Data
                    .Where(c => symbols.Contains(c.Symbol))
                    .Select(c => new CryptoCurrency
                    {
                        Symbol = c.Symbol,
                        Name = c.Name,
                        CurrentPrice = c.PriceUsd
                    })
                    .ToList();

                _logger.LogInformation($"Retrieved {cryptos.Count} cryptocurrencies from CoinLore API");
                return cryptos;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation was canceled while retrieving cryptocurrency data");
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while retrieving cryptocurrency data: {Message}", ex.Message);
                throw new CryptoProviderException("Failed to connect to CoinLore API", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response from CoinLore API: {Message}", ex.Message);
                throw new CryptoProviderException("Invalid data received from CoinLore API", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving cryptocurrency data: {Message}", ex.Message);
                throw new CryptoProviderException("An unexpected error occurred while retrieving cryptocurrency data", ex);
            }
        }
    }

    public class CryptoCurrencyResponse
    {
        public List<CryptoCurrencyData> Data { get; set; }
    }

    public class CryptoCurrencyData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("price_usd")]
        public decimal PriceUsd { get; set; }
    }

    public class CryptoProviderException : Exception
    {
        public CryptoProviderException(string message) : base(message) { }
        public CryptoProviderException(string message, Exception innerException) : base(message, innerException) { }
    }
}