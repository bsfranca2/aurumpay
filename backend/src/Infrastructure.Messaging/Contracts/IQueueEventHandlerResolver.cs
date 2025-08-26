namespace AurumPay.Infrastructure.Messaging.Contracts;

public interface IQueueEventHandlerResolver
{
    IEnumerable<Type> GetEventTypesForQueue(string queueName);
}