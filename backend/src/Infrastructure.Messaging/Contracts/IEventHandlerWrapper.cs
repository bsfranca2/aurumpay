namespace AurumPay.Infrastructure.Messaging.Contracts;

internal interface IEventHandlerWrapper
{
    Task HandleAsync(object message, CancellationToken cancellationToken);
}