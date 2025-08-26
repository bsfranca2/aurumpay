using AurumPay.Core;
using AurumPay.Infrastructure.Messaging.Contracts;

using Microsoft.Extensions.DependencyInjection;

namespace AurumPay.Infrastructure.Messaging.Services;

internal class EventHandlerWrapper<TEvent>(IServiceProvider serviceProvider) : IEventHandlerWrapper
    where TEvent : class, IEvent
{
    public async Task HandleAsync(object message, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
        
        var tasks = handlers.Select(handler => 
            handler.HandleAsync((TEvent)message, cancellationToken));
            
        await Task.WhenAll(tasks);
    }
}