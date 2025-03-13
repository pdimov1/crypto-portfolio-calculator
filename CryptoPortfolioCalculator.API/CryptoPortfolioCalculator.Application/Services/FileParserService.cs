using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CryptoPortfolioCalculator.Application.Services
{
    public class FileParserService : IFileParserService
    {
        private readonly ILogger<FileParserService> _logger;

        public FileParserService(ILogger<FileParserService> logger)
        {
            _logger = logger;
        }

        public async Task<List<PortfolioItem>> ParsePortfolioFileAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
            {
                _logger.LogError("File stream was null");
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (!fileStream.CanRead)
            {
                _logger.LogError("File stream is not readable");
                throw new InvalidOperationException("File stream must be readable");
            }

            var portfolioItems = new List<PortfolioItem>();

            using (var streamReader = new StreamReader(fileStream))
            {
                string line;
                int lineNumber = 0;

                var invariantCulture = CultureInfo.InvariantCulture;

                while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue; 
                    }

                    try
                    {
                        var parts = line.Split('|');
                        if (parts.Length != 3)
                        {
                            _logger.LogError($"Invalid format at line {lineNumber}: {line}");
                            continue;
                        }

                        // Use invariant culture and trim any whitespace
                        if (!decimal.TryParse(parts[0].Trim(), NumberStyles.Any, invariantCulture, out var quantity) ||
                            !decimal.TryParse(parts[2].Trim(), NumberStyles.Any, invariantCulture, out var initialPrice))
                        {
                            _logger.LogError($"Invalid number format at line {lineNumber}: {line}");
                            _logger.LogError($"Tried to parse: '{parts[0].Trim()}' and '{parts[2].Trim()}'");
                            continue;
                        }

                        portfolioItems.Add(new PortfolioItem
                        {
                            Quantity = quantity,
                            Symbol = parts[1].Trim(),
                            InitialPrice = initialPrice,
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Portfolio file parsing was canceled");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error parsing line {lineNumber}: {line}");
                    }
                }

                if (portfolioItems.Count == 0)
                {
                    _logger.LogError($"No valid portfolio items found in the file.");
                }

                _logger.LogInformation($"Parsed {portfolioItems.Count} portfolio items from file");

                return portfolioItems;
            }
        }
    }
}