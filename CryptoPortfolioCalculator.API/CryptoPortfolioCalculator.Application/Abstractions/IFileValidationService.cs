using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Application.Abstractions
{
    /// <summary>
    /// Defines methods for validating file uploads.
    /// </summary>
    public interface IFileValidationService
    {
        /// <summary>
        /// Validates a given file based on predefined settings.
        /// </summary>
        /// <param name="file">The file to validate.</param>
        /// <returns>A ValidationResult indicating success or failure.</returns>
        Task<ValidationResult> ValidateFile(PortfolioFile file);
    }
}