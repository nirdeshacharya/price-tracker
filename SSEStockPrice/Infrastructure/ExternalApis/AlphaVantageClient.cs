using Microsoft.Extensions.Configuration;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;
using System.Net.Http.Json;

namespace SSEStockPrice.Infrastructure.ExternalApis
{
    public class AlphaVantageClient : IAlphaVantageClient
    {

        private readonly HttpClient _httpclient;
        private readonly IConfiguration _configuration;
        public AlphaVantageClient(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpclient = httpClient;
            _configuration = configuration;
        }

        public async Task<SSEPrice> GetPriceAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var apikey = _configuration["AlphaVantageApiKey"];
            var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={apikey}";
            var response = await _httpclient.GetFromJsonAsync<AlphaVantageResponse>(url, cancellationToken);

            if (response?.GlobalQuote == null)
                throw new Exception("No data returned from AlphaVantage.");

            var quote = response.GlobalQuote;

            return new SSEPrice
            {
                Symbol = quote.Symbol,
                Timestamp = DateTimeOffset.UtcNow,
                OpenPrice = decimal.TryParse(quote.Open, out var open) ? open : 0,
                CurrentPrice = decimal.TryParse(quote.Price, out var price) ? price : 0,
                HighPrice = decimal.TryParse(quote.High, out var high) ? high : 0,
                LowPrice = decimal.TryParse(quote.Low, out var low) ? low : 0,
                Volume = long.TryParse(quote.Volume, out var volume) ? volume : 0,
                ChangePercent = ParseChangePercent(quote.ChangePercent)
            };
        }

        private static decimal ParseChangePercent(string percent)
        {
            if (percent?.EndsWith("%") == true)
                percent = percent.TrimEnd('%');
            return decimal.TryParse(percent, out var value) ? value : 0;
        }
    }
}
