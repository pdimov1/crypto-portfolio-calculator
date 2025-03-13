using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Application.Services
{
    public class PortfolioCalculatorService : IPortfolioCalculatorService
    {
        public Portfolio CalculatePortfolio(List<PortfolioItem> portfolioAssets, List<CryptoCurrency> currentCryptoPrices)
        {
            decimal totalInitialValue = 0;
            decimal totalCurrentValue = 0;
            List<PortfolioItem> updatedPortfolioAssets = new List<PortfolioItem>();

            foreach (var portfolioAsset in portfolioAssets)
            {
                CryptoCurrency crypto = currentCryptoPrices.FirstOrDefault(c => c.Symbol == portfolioAsset.Symbol);
                if (crypto != null)
                {
                    decimal currentValue = CalculateValue(portfolioAsset.Quantity, crypto.CurrentPrice);
                    decimal initialValue = CalculateValue(portfolioAsset.Quantity, portfolioAsset.InitialPrice);

                    PortfolioItem updatedPortfolioItem = new PortfolioItem
                    {
                        Symbol = portfolioAsset.Symbol,
                        Quantity = portfolioAsset.Quantity,
                        InitialPrice = portfolioAsset.InitialPrice,
                        CurrentPrice = crypto.CurrentPrice,
                        InitialValue = initialValue,
                        CurrentValue = currentValue,
                        ChangePercentage = CalculateChangePercentage(initialValue, currentValue)
                    };

                    updatedPortfolioAssets.Add(updatedPortfolioItem);

                    totalInitialValue += updatedPortfolioItem.InitialValue;
                    totalCurrentValue += updatedPortfolioItem.CurrentValue;
                }
            }

            decimal overallChangePercentage = CalculateChangePercentage(totalInitialValue, totalCurrentValue);


            return new Portfolio
            {
                Items = updatedPortfolioAssets,
                InitialValue = totalInitialValue,
                CurrentValue = totalCurrentValue,
                OverallChangePercentage = overallChangePercentage
            };

        }

        private decimal CalculateValue(decimal quantity, decimal value)
        {
            return quantity * value;
        }

        private decimal CalculateChangePercentage(decimal initialValue, decimal currentValue)
        {
            return initialValue == 0 ? 0 : (currentValue / initialValue - 1) * 100;
        }
    }
}