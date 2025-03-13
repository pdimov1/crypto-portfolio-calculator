using CryptoPortfolioCalculator.Domain.Models;
using System.Xml.Linq;

namespace CryptoPortfolioCalculator.Application.Abstractions
{
    /// <summary>
    /// Provides methods to retrieve cryptocurrency data from an external provider.
    /// </summary>
    public interface ICryptoProviderService
    {

          /// <summary>
          /// Fetches cryptocurrency data for the specified symbols.
          /// </summary>
          /// <param name="symbols">A list of cryptocurrency symbols to retrieve information for.</param>
          /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
          /// <returns>A task representing the asynchronous operation, returning a collection of cryptocurrency data.</returns>
          /// <exception cref="CryptoProviderException"> Thrown when unable to retrieve cryptocurrency data due to API issues, network problems, or data deserialization errors. </exception>
          Task<IEnumerable<CryptoCurrency>> GetCryptoCurrenciesAsync(List<string> symbols, CancellationToken cancellationToken = default);
    }
}