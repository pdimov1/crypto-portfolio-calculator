using AutoMapper;
using CryptoPortfolioCalculator.Clients.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CryptoPortfolioCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFileClient _fileClient;

        public HomeController(IFileClient fileClient, IMapper mapper)
        {
            _fileClient = fileClient;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Please select a file to upload." });
            }

            using var formData = new MultipartFormDataContent();
            using var fileContent = BuildContent(file);
            formData.Add(fileContent, "file", file.FileName);

            var response = await _fileClient.UploadPortfolioFileAsync(formData);

            TempData["PortfolioItems"] = JsonConvert.SerializeObject(response.Items);

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Portfolio") });
        }

        public IActionResult Error()
        {
            TempData["ErrorMessage"] = HttpContext.Items["ErrorMessage"];
            return View();
        }

        private StreamContent BuildContent(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;

            var fileContent = new StreamContent(memoryStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            return fileContent;
        }
    }
}