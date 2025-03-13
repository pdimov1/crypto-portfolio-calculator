using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Application.Abstractions
{
    /// <summary>
    /// Defines a service for calculating portfolio values based on asset data.
    /// </summary>
    public interface IPortfolioCalculatorService
    {
        /// <summary>
        /// Calculates the total portfolio value based on the given portfolio items and their current market prices.
        /// </summary>
        /// <param name="portfolioAssets">A list of portfolio items representing owned assets.</param>
        /// <param name="currentCryptoPrices">A list of current cryptocurrency prices.</param>
        /// <returns>A calculated portfolio containing updated asset values.</returns>
        Portfolio CalculatePortfolio(List<PortfolioItem> portfolioAssets, List<CryptoCurrency> currentCryptoPrices);
    }
}