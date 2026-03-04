

namespace SSEStockPrice.Interfaces
{
    public interface IServiceBusPublisher
    {
        Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default);
    }
}
