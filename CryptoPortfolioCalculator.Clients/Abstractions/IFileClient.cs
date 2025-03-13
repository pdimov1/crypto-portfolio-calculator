using CryptoPortfolioCalculator.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoPortfolioCalculator.Clients.Abstractions
{

    /// <summary>
    /// Defines a client responsible for uploading portfolio files.
    /// </summary>
    public interface IFileClient
    {
        /// <summary>
        /// Uploads a portfolio file.
        /// </summary>
        /// <param name="file">The multipart form data content containing the file to be uploaded.</param>
        /// <returns>
        /// The task result contains the response indicating the outcome of the upload process.
        /// </returns>
        Task<UploadPortfolioResponse> UploadPortfolioFileAsync(MultipartFormDataContent file);
    }
}
