using CryptoPortfolioCalculator.Application.Services;
using CryptoPortfolioCalculator.Domain.Models;

namespace CryptoPortfolioCalculator.Tests
{
    public class FileValidationServiceTests
    {
        private readonly FileValidationService _service;

        public FileValidationServiceTests()
        {
            _service = new FileValidationService();
        }

        [Fact]
        public async void ValidateFile_FileIsNull_ReturnsFail()
        {
            // Act
            var result = await _service.ValidateFile(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("File is empty.", result.ErrorMessage);
        }

        [Fact]
        public async void ValidateFile_FileIsEmpty_ReturnsFail()
        {
            // Arrange
            var file = new PortfolioFile
            {
                FileName = "empty.csv",
                Length = 0,
                FileContent = new MemoryStream()
            };

            // Act
            var result = await _service.ValidateFile(file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("File is empty.", result.ErrorMessage);
        }

        [Fact]
        public async void ValidateFile_FileTooLarge_ReturnsFail()
        {
            // Arrange
            var file = new PortfolioFile
            {
                FileName = "large.csv",
                Length = 6 * 1024 * 1024,
                FileContent = new MemoryStream()
            };

            // Act
            var result = await _service.ValidateFile(file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("File size exceeds 5 MB limit.", result.ErrorMessage);
        }

        [Fact]
        public async void ValidateFile_UnsupportedFileType_ReturnsFail()
        {
            // Arrange
            var file = new PortfolioFile
            {
                FileName = "document.pdf",
                Length = 1024,
                FileContent = new MemoryStream()
            };

            // Act
            var result = await _service.ValidateFile(file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid or unsupported file type.", result.ErrorMessage);
        }

        [Fact]
        public async void ValidateFile_ValidCsvFile_ReturnsSuccess()
        {
            // Arrange
            var memoryStream = new MemoryStream(new byte[] { 0xEF, 0xBB, 0xBF }); 
            memoryStream.Position = 0;

            var file = new PortfolioFile
            {
                FileName = "data.csv",
                Length = 1024,
                FileContent = memoryStream
            };

            // Act
            var result = await _service.ValidateFile(file);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateFile_ValidTxtFile_ReturnsSuccess()
        {
            // Arrange
            var memoryStream = new MemoryStream(new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }); // "Hello" in bytes
            memoryStream.Position = 0;
            var file = new PortfolioFile
            {
                FileName = "note.txt",
                Length = 1024,
                FileContent = memoryStream
            };

            // Act
            var result = await _service.ValidateFile(file);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}