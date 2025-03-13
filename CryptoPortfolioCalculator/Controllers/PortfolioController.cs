using AutoMapper;
using CryptoPortfolioCalculator.Clients.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using CryptoPortfolioCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace CryptoPortfolioCalculator.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPortfolioClient _portfolioClient;
        private readonly PortfolioSettings _settings;

        public PortfolioController(IPortfolioClient portfolioClient, IMapper mapper, IOptions<PortfolioSettings> settings)
        {
            _portfolioClient = portfolioClient;
            _mapper = mapper;
            _settings = settings.Value;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData["PortfolioItems"] == null)
            {
                return BadRequest("No portfolio items data found.");
            }

            var portfolioItems = JsonConvert.DeserializeObject<List<PortfolioItemDto>>(TempData["PortfolioItems"].ToString());
            return await GetPortfolioView(portfolioItems, false);
        }

        [Route("refresh-interval")]
        public IActionResult GetRefreshInterval()
        {
            return Json(new { interval = _settings.RefreshInterval });
        }

        [HttpPost]
        [Route("portfolio-refresh")]
        public async Task<IActionResult> RefreshPortfolio([FromBody] List<PortfolioItemDto> portfolioItems)
        {
            if (!portfolioItems.Any())
            {
                return BadRequest("No portfolio items data found.");
            }

            return await GetPortfolioView(portfolioItems, true);
        }

        private async Task<IActionResult> GetPortfolioView(List<PortfolioItemDto> portfolioItems, bool isPartial)
        {
            var response = await _portfolioClient.CalculatePortfolioFileAsync(portfolioItems);
            var viewModel = new PortfolioViewModel
            {
                Portfolio = _mapper.Map<Portfolio>(response)
            };

            return isPartial
                ? PartialView("Index", viewModel)
                : View(viewModel);
        }
    }
}