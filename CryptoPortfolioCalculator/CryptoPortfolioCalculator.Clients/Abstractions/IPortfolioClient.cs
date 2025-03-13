using CryptoPortfolioCalculator.DataContracts;

namespace CryptoPortfolioCalculator.Clients.Abstractions
{
    /// <summary>
    /// Defines a client for interacting with the portfolio-related API endpoints.
    /// </summary>
    public interface IPortfolioClient
    {
        /// <summary>
        /// Calculates the portfolio value based on the given list of portfolio items.
        /// </summary>
        /// <param name="portfolioItems">A list of portfolio items to calculate the total portfolio value.</param>
        /// <returns>A task representing the asynchronous operation, returning the calculated portfolio details.</returns>
        Task<PortfolioDto> CalculatePortfolioFileAsync(List<PortfolioItemDto> portfolioItems);
    }
}