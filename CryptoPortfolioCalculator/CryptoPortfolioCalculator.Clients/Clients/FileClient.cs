using CryptoPortfolioCalculator.Clients.Abstractions;
using CryptoPortfolioCalculator.DataContracts;
using Microsoft.Extensions.Logging;

namespace CryptoPortfolioCalculator.Clients.Clients
{
    public class FileClient : ClientBase, IFileClient
    {
        public FileClient(IHttpClientFactory httpClientFactory, ILogger<ClientBase> logger) 
            : base(httpClientFactory, logger)
        {
        }

        public async Task<UploadPortfolioResponse> UploadPortfolioFileAsync(MultipartFormDataContent file)
        {
            return await SendMessageAsync<UploadPortfolioResponse>($"api/files/upload", file, HttpMethod.Post);
        }
    }
}