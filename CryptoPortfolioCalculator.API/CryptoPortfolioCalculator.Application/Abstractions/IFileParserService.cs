using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Application.Abstractions
{
    /// <summary>
    /// Defines a service for parsing portfolio data from a file.
    /// </summary>
    public interface IFileParserService
    {
        /// <summary>
        /// Parses a portfolio file and extracts portfolio items from it.
        /// </summary>
        /// <param name="fileStream">The file stream containing the portfolio data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of parsed portfolio items.</returns>
        Task<List<PortfolioItem>> ParsePortfolioFileAsync(Stream fileStream, CancellationToken cancellationToken = default);
    }
}