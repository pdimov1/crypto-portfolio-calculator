using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryptoPortfolioCalculator.Clients.Utils
{
    internal static class HttpClientExtensions
    {
        internal static async Task<T> ReadAsJsonAsync<T>(this HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return Deserialize<T>(responseString);
        }

        internal static async Task EnsureSucceeded(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new HttpException(
                    response.RequestMessage.RequestUri?.OriginalString,
                    response.RequestMessage.Method.ToString(),
                    (int)response.StatusCode,
                    content ?? response.ReasonPhrase);
            }
        }

        internal static T Deserialize<T>(this string responseString)
        {
            if (responseString == null)
            {
                throw new ArgumentNullException(nameof(responseString));
            }

            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}