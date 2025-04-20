using AurumPay.Application.Data;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Merchants;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;
using AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework;

/// <remarks>
/// Add migrations using the following command inside the 'src\Infrastructure.EntityFramework' project directory:
///
/// dotnet ef migrations add --startup-project ..\Api --context AppDbContext [migration-name]
/// </remarks>
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<Merchant> Merchants { get; init; }
    public DbSet<Store> Stores { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<CheckoutSession> CheckoutSessions { get; init; }
    public DbSet<Customer> Customers { get; init; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MerchantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CheckoutSessionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerAddressEntityTypeConfiguration());
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
    }
}
