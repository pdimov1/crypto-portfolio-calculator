using AutoMapper;
using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using CryptoPortfolioCalculator.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolioCalculator.API.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileParserService _fileParserService;
        private readonly IPortfolioCalculatorService _portfolioCalculatorService;
        private readonly ICryptoProviderService _cryptoProviderService;

        public PortfolioController(
            IMapper mapper,
            IFileParserService fileParserService,
            IPortfolioCalculatorService portfolioCalculatorService,
            ICryptoProviderService cryptoProviderService)
        {
            _fileParserService = fileParserService;
            _portfolioCalculatorService = portfolioCalculatorService;
            _cryptoProviderService = cryptoProviderService;
            _mapper = mapper;
        }

        [HttpPost("calculate-portfolio")]
        public async Task<IActionResult> CalculatePortfolioAsync(IEnumerable<PortfolioItemDto> cryptoAssets)
        {
            var symbols = cryptoAssets.Select(i => i.Symbol).ToList();
            var cryptos = await _cryptoProviderService.GetCryptoCurrenciesAsync(symbols);

            var portfolio = _portfolioCalculatorService.CalculatePortfolio(_mapper.Map<List<PortfolioItem>>(cryptoAssets), cryptos.ToList());

            return Ok(portfolio);
        }
    }
}