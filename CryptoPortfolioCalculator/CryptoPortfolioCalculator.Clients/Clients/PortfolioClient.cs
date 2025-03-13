using CryptoPortfolioCalculator.Clients.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using Microsoft.Extensions.Logging;

namespace CryptoPortfolioCalculator.Clients.Clients
{
    public class PortfolioClient : ClientBase, IPortfolioClient
    {
        public PortfolioClient(IHttpClientFactory httpClientFactory, ILogger<ClientBase> logger)
            : base(httpClientFactory, logger)
        {
        }

        public async Task<PortfolioDto> CalculatePortfolioFileAsync(List<PortfolioItemDto> portfolioItems)
        {
            return await SendMessageAsync<PortfolioDto>($"api/portfolio/calculate-portfolio", portfolioItems, HttpMethod.Post);
        }
    }
}