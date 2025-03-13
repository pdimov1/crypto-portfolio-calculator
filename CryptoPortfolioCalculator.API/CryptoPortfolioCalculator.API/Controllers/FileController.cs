using AutoMapper;
using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using CryptoPortfolioCalculator.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolioCalculator.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileValidationService _fileValidationService;
        private readonly IFileParserService _fileParserService;
        private readonly IMapper _mapper;

        public FileController(
            IFileValidationService fileValidationService,
            IFileParserService fileParserService,
            IMapper mapper)
        {
            _fileValidationService = fileValidationService;
            _fileParserService = fileParserService;
            _mapper = mapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var portfolioFile = new PortfolioFile
            {
                FileName = file.FileName,
                Length = file.Length,
                FileContent = file.OpenReadStream(),
            };

            var validationResult = await _fileValidationService.ValidateFile(portfolioFile);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            var assets = await _fileParserService.ParsePortfolioFileAsync(portfolioFile.FileContent);
            return Ok(new UploadPortfolioResponse { Items = _mapper.Map<List<PortfolioItemDto>>(assets) });
        }
    }
}
