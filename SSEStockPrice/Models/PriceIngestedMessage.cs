using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSEStockPrice.Models
{
    public class PriceIngestedMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; }
        public decimal StockPrice { get; set; }
        public DateTimeOffset DateTime { get; set; }

    }
}
