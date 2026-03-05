using Azure.Messaging.ServiceBus;
using SSEStockPrice.Interfaces;
using System.Text.Json;

namespace SSEStockPrice.Infrastructure.Messaging
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {

        private readonly ServiceBusClient _serviceBusClient;
        public ServiceBusPublisher(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }
        public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default)
        {
            var messageToSend = JsonSerializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(BinaryData.FromString(messageToSend));

            await using var sender = _serviceBusClient.CreateSender(queueName);

            await sender.SendMessageAsync(serviceBusMessage,cancellationToken);
        }
    }
}
