using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.Domain;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Application.Services
{
    public class FileValidationService : IFileValidationService
    {
        private static readonly Dictionary<string, byte[]> FileSignatures = new()
        {
            { ".csv", new byte[] { 0xEF, 0xBB, 0xBF } },
            { ".txt", new byte[] { } }
        };

        public async Task<ValidationResult> ValidateFile(PortfolioFile file)
        {
            if (file == null || file.Length == 0)
            {
                return ValidationResult.Fail("File is empty.");
            }

            long maxFileSize = 5 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                return ValidationResult.Fail($"File size exceeds 5 MB limit.");
            }

            var isValidSignature = await IsValidFileSignature(file);
            if (!isValidSignature)
            {
                return ValidationResult.Fail("Invalid or unsupported file type.");
            }

            return ValidationResult.Success();
        }

        private async Task<bool> IsValidFileSignature(PortfolioFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!FileSignatures.ContainsKey(extension))
            {
                return false;
            }

            byte[] fileHeader = new byte[FileSignatures[extension].Length];
            file.FileContent.Read(fileHeader, 0, fileHeader.Length);
            file.FileContent.Position = 0;

            return FileSignatures[extension].SequenceEqual(fileHeader);
        }
    }
}
