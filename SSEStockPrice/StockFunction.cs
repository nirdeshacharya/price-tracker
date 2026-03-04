using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SSEStockPrice;

public class StockFunction
{
    private readonly ILogger _logger;

    public StockFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StockFunction>();
    }

    [Function("StockFunction")]
    public void Run([TimerTrigger("0 0 8-17 * * 1-5")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}