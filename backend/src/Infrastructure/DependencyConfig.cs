using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Repositories;
using AurumPay.Infrastructure.PaymentGateways.MercadoPago;
using AurumPay.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AurumPay.Infrastructure;

public static class DependencyConfig
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductMappingService, ProductMappingService>();
        services.AddScoped<IProductValidator, ProductValidator>();

        return services;
    }

    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentProcessingService, PaymentProcessingService>();
        services.AddScoped<IStorePaymentConfigurationService, StorePaymentConfigurationService>();

        services.AddScoped<IPaymentGatewayService, MercadoPagoGatewayService>();

        return services;
    }
}