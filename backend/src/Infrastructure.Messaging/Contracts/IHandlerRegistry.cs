namespace AurumPay.Infrastructure.Messaging.Contracts;

public interface IHandlerRegistry
{
    void Register(Type eventType);
    bool HasHandlerFor(Type eventType);
}