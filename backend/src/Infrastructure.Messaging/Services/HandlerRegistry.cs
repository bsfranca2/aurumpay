using AurumPay.Infrastructure.Messaging.Contracts;

namespace AurumPay.Infrastructure.Messaging.Services;

public class HandlerRegistry : IHandlerRegistry
{
    private readonly HashSet<Type> _registeredEventTypes = new();

    public void Register(Type eventType)
    {
        _registeredEventTypes.Add(eventType);
    }

    public bool HasHandlerFor(Type eventType)
    {
        return _registeredEventTypes.Contains(eventType);
    }
}