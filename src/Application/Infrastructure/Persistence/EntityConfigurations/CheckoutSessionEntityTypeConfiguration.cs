using AurumPay.Ordering.Domain.Checkout;
using AurumPay.Ordering.Domain.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;

public class CheckoutSessionEntityTypeConfiguration : IEntityTypeConfiguration<CheckoutSession>
{
    public void Configure(EntityTypeBuilder<CheckoutSession> checkoutSessionConfiguration)
    {
        checkoutSessionConfiguration.ToTable("checkout_sessions");

        checkoutSessionConfiguration.HasKey(p => p.Id);

        checkoutSessionConfiguration.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);

        checkoutSessionConfiguration.HasOne<Store>().WithMany().HasForeignKey(s => s.StoreId);

        checkoutSessionConfiguration.HasMany(c => c.CartItems).WithOne().HasForeignKey(ci => ci.CheckoutSessionId);
    }
}