﻿namespace CryptoPortfolioCalculator.DataContracts
{
    public class PortfolioItemDto
    {
        public decimal Quantity { get; set; }
        public string Symbol { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ChangePercentage { get; set; }
    }
}