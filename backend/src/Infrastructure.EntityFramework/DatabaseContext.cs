using AurumPay.Application.Data;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Merchants;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Outbox;
using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;
using AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework;

/// <remarks>
///     Add migrations using the following command inside the 'src\Infrastructure.EntityFramework' project directory:
///     dotnet ef migrations add [migration-name]
/// </remarks>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDatabaseContext
{
    public DbSet<Merchant> Merchants { get; init; }
    public DbSet<Store> Stores { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<CheckoutSession> CheckoutSessions { get; init; }
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<Payment> Payments { get; init; }
    public DbSet<PaymentMethod> PaymentMethods { get; init; }
    public DbSet<PaymentGateway> PaymentGateways { get; init; }
    public DbSet<StorePaymentConfiguration> StorePaymentConfigurations { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MerchantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CheckoutSessionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerAddressEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentGatewayEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StorePaymentConfigurationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
    }
}