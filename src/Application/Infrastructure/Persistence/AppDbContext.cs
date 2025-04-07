using AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;
using AurumPay.Ordering.Domain.Catalog;
using AurumPay.Ordering.Domain.Checkout;
using AurumPay.Ordering.Domain.Merchants;
using AurumPay.Ordering.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Infrastructure.Persistence;

/// <remarks>
/// Add migrations using the following command inside the 'src\Application' project directory:
///
/// dotnet ef migrations add --startup-project ..\Api --context AppDbContext [migration-name]
/// </remarks>
public class AppDbContext : DbContext
{
    public DbSet<Merchant> Merchants { get; init; }
    public DbSet<Store> Stores { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<CheckoutSession> CheckoutSessions { get; init; }
    public DbSet<CartItem> CartItems { get; init; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MerchantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CheckoutSessionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemEntityTypeConfiguration());
    }
}
