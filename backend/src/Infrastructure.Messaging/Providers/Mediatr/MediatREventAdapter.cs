using AurumPay.Core;

using MediatR;

namespace AurumPay.Infrastructure.Messaging.Providers.Mediatr;

public class MediatREventAdapter<TEvent>(TEvent @event) : INotification
    where TEvent : IEvent
{
    public TEvent Event { get; } = @event ?? throw new ArgumentNullException(nameof(@event));
}