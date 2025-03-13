namespace CryptoPortfolioCalculator.Application
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public static ValidationResult Success() => new() { IsValid = true };
        public static ValidationResult Fail(string error) => new() { IsValid = false, ErrorMessage = error };
    }
}