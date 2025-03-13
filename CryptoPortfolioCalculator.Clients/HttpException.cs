namespace CryptoPortfolioCalculator.Clients
{
    public class HttpException : Exception
    {
        public string Method { get; }

        public string Url { get; }

        public int StatusCode { get; }

        public HttpException(string url, string method, int statusCode, string message)
            : base(message)
        {
            Url = url;
            Method = method;
            StatusCode = statusCode;
        }
    }
}