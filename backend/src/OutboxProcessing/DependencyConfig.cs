using AurumPay.Infrastructure.Messaging.Extensions;

using Npgsql;

namespace AurumPay.OutboxProcessing;

internal static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddSingleton<NpgsqlDataSource>(_ =>
        {
            string connectionString = configuration.GetConnectionString("Database") ??
                                      throw new InvalidOperationException("Connection string 'Database' not found.");

            NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);
            return dataSourceBuilder.Build();
        });

        services.AddMessaging(context =>
        {
            string connectionString = configuration.GetConnectionString("Messaging")
                                      ?? "amqp://guest:guest@localhost:5672/";
            context.UsingRabbitMq(connectionString);
        });

        services.AddScoped<OutboxProcessor>();

        return services;
    }
}