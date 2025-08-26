using AurumPay.Core;
using AurumPay.Infrastructure.Messaging.Configurations;
using AurumPay.Infrastructure.Messaging.Contracts;
using AurumPay.Infrastructure.Messaging.Providers.RabbitMQ;

using Microsoft.Extensions.DependencyInjection;

namespace AurumPay.Infrastructure.Messaging.Extensions;

public static class RabbitMQServiceExtensions
{
    public static IRegistrationContext UsingRabbitMq(
        this IRegistrationContext context,
        string connectionString,
        Action<IRabbitMQRegistrationContext>? configure = null)
    {
        RabbitMQRegistrationContext rabbitMqContext = new();
        configure?.Invoke(rabbitMqContext);

        context.Options.ConnectionString = connectionString;
        ApplyDefaultTopologyConfiguration(context.Options);

        context.AddSingleton<RabbitMQConnectionFactory>();
        context.AddSingleton<IEventRoutingRegistry, RabbitMQEventRoutingRegistry>();
        context.AddSingleton<IInfrastructureManager, RabbitMQInfrastructureManager>();

        context.AddScoped<IEventPublisher, RabbitMQEventPublisher>();
        
        if (rabbitMqContext.SetupInfrastructure)
        {
            context.AddHostedService<InfrastructureInitializationService>();
        }

        if (context.ConsumerEnabled)
        {
            if (!context.IsProcessorConfigured)
            {
                throw new InvalidOperationException(
                    "No message processor was configured. " +
                    "Please call '.UseDirectMessageProcessor()' or '.UseMediatRMessageProcessor()' inside the AddMessaging configuration.");
            }

            context.AddSingleton<IQueueEventHandlerResolver, RabbitMQQueueEventHandlerResolver>();
            context.AddSingleton<IEventConsumer, RabbitMQEventConsumer>();
            context.AddHostedService<RabbitMQConsumerBackgroundService>();
        }

        return context;
    }

    private static MessagingOptions ApplyDefaultTopologyConfiguration(MessagingOptions options)
    {
        foreach (ExchangeConfiguration exchange in RabbitMQMessagingTopology.ExchangeConfigurations)
        {
            if (options.Exchanges.All(e => e.Name != exchange.Name))
            {
                options.Exchanges.Add(exchange);
            }
        }

        List<QueueConfiguration> defaultQueues = RabbitMQMessagingTopology.QueueConfigurations;
        foreach (QueueConfiguration queue in defaultQueues)
        {
            if (options.Queues.All(q => q.Name != queue.Name))
            {
                options.Queues.Add(queue);
            }
        }

        return options;
    }
}