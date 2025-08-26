using System.Collections.Concurrent;

using AurumPay.Infrastructure.Messaging.Configurations;
using AurumPay.Infrastructure.Messaging.Contracts;

namespace AurumPay.Infrastructure.Messaging.Providers.RabbitMQ;

public class RabbitMQQueueEventHandlerResolver : IQueueEventHandlerResolver
{
    public IEnumerable<Type> GetEventTypesForQueue(string queueName)
    {
        QueueConfiguration? queueConfig = RabbitMQMessagingTopology.QueueConfigurations
            .FirstOrDefault(q => q.Name == queueName);

        if (string.IsNullOrEmpty(queueConfig?.ExchangeName) || string.IsNullOrEmpty(queueConfig.RoutingKey))
        {
            return [];
        }

        IEnumerable<Type> eventTypes = RabbitMQMessagingTopology.EventMappings
            .Where(mapping =>
                mapping.Value.ExchangeName == queueConfig.ExchangeName &&
                mapping.Value.RoutingKey == queueConfig.RoutingKey)
            .Select(mapping => mapping.Key);

        return eventTypes.ToList();
    }
}