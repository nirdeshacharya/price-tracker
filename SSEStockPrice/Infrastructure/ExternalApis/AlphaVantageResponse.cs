using System.Text.Json.Serialization;

namespace SSEStockPrice.Infrastructure.ExternalApis
{
    public class AlphaVantageResponse
    {
        [JsonPropertyName("Global Quote")]
        public GlobalQuoteData GlobalQuote { get; set; }

    }

    public class GlobalQuoteData
    {
        [JsonPropertyName("01. symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("02. open")]
        public string Open { get; set; }

        [JsonPropertyName("03. high")]
        public string High { get; set; }

        [JsonPropertyName("04. low")]
        public string Low { get; set; }

        [JsonPropertyName("05. price")]
        public string Price { get; set; }

        [JsonPropertyName("06. volume")]
        public string Volume { get; set; }

        [JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; }
    }
}
