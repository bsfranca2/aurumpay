using AurumPay.Core;
using AurumPay.Infrastructure.Messaging.Configurations;

namespace AurumPay.Infrastructure.Messaging.Contracts;

public interface IEventRoutingRegistry
{
    EventRouting GetRouting(Type eventType);
    EventRouting GetRouting<T>() where T : class, IEvent;
    bool TryGetEventType(string eventTypeName, out Type? eventType);
}