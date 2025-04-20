using AurumPay.Application.SeedWork;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Options;
using AurumPay.Infrastructure.EntityFramework.Repositories;
using AurumPay.Infrastructure.EntityFramework.ValueGenerators;

using Bogus;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AurumPay.Infrastructure.EntityFramework;

public static class EntityFrameworkServiceCollectionExtensions
{
    public static IServiceCollection SetupEntityFramework(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        
        services.AddDbContext<DatabaseContext>(ConfigureDbContextOptions);

        return services;
    }
    
    private static void ConfigureDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
        
        dbContextOptionsBuilder.UseNpgsql(databaseOptions.ConnectionString, npgsqlDbContextOptionsBuilder =>
        {
            // npgsqlDbContextOptionsBuilder.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
            npgsqlDbContextOptionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
        }).UseAsyncSeeding(async (context, _, ct) => await DatabaseContextSeed.Seed((DatabaseContext)context, ct));
        
        dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
        dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    }
    
    public static IServiceCollection AddEfRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ICheckoutSessionRepository, CheckoutSessionRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IStoreCustomerService, StoreCustomerService>();
        services.AddScoped<IStoreProductValidator, StoreProductValidator>();

        return services;
    }
}