using AurumPay.Application;
using AurumPay.Infrastructure;
using AurumPay.Infrastructure.EntityFramework;
using AurumPay.Infrastructure.Messaging.Extensions;

namespace AurumPay.EventsProcessing;

public static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.SetupEntityFramework();
        services.AddEfRepositories();

        services.AddCatalogServices(configuration);

        services.AddPaymentServices(configuration);

        services.AddEventHandlersFromAssemblies(typeof(ApplicationAssemblyReference).Assembly);

        services.AddMessaging(context =>
        {
            context.WithConsumer().UseDirectMessageProcessor();

            string connectionString = configuration.GetConnectionString("Messaging")
                                      ?? "amqp://guest:guest@localhost:5672/";
            context.UsingRabbitMq(connectionString, rabbit => rabbit.WithInfrastructureSetup());
        });

        return services;
    }
}