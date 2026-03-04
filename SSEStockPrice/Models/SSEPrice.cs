using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSEStockPrice.Models
{
    public class SSEPrice
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public long Volume { get; set; }
        public decimal ChangePercent { get; set; }
        public string Symbol { get; set; } = "SSE.LON";
    }
}
