using RabbitMQ.Client;

namespace AurumPay.Infrastructure.Messaging.Contracts;

public interface IInfrastructureManager
{
    Task EnsureInfrastructureAsync(IChannel channel, CancellationToken cancellationToken = default);
}