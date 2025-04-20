using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;
using AurumPay.Infrastructure.EntityFramework.ValueGenerators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class CheckoutSessionEntityTypeConfiguration : IEntityTypeConfiguration<CheckoutSession>
{
    public void Configure(EntityTypeBuilder<CheckoutSession> checkoutSessionConfiguration)
    {
        checkoutSessionConfiguration.ToTable("CheckoutSessions");

        checkoutSessionConfiguration.HasKey(cs => cs.Id);

        checkoutSessionConfiguration.Property(cs => cs.Id)
            .HasConversion(new CheckoutSessionIdConverter());

        checkoutSessionConfiguration.Property(cs => cs.StoreId)
            .HasConversion(new StoreIdConverter());

        checkoutSessionConfiguration.HasOne<Store>()
            .WithMany()
            .HasForeignKey(cs => cs.StoreId);

        checkoutSessionConfiguration.Property(cs => cs.Fingerprint)
            .HasColumnType("text");

        // TODO: Improve
        checkoutSessionConfiguration.Property(cs => cs.Status)
            .HasConversion<string>()
            .HasMaxLength(30);
        
        checkoutSessionConfiguration.OwnsMany(cs => cs.CartItems, cartItemBuilder =>
        {
            cartItemBuilder.ToTable("CartItems");
            
            cartItemBuilder.Property<Guid>("Id")
                .HasValueGenerator<UlidValueGenerator>()
                .ValueGeneratedOnAdd();
            cartItemBuilder.HasKey("Id");
            
            cartItemBuilder.Property<CheckoutSessionId>("CheckoutSessionId")
                .HasConversion(new CheckoutSessionIdConverter());
                
            cartItemBuilder.Property(ci => ci.ProductId)
                .HasConversion(new ProductIdConverter());
            
            cartItemBuilder.HasIndex("CheckoutSessionId", nameof(CartItem.ProductId))
                .IsUnique();
            
            cartItemBuilder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
            
            cartItemBuilder.Property(ci => ci.Quantity).HasColumnType("integer");
        });
        
        checkoutSessionConfiguration.Property(cs => cs.CustomerId)
            .HasConversion(
                customerId => customerId.HasValue ? customerId.Value.Value : (long?)null,
                value => value.HasValue ? new CustomerId(value.Value) : null
            );

        // TODO: Verificar se realmente vai ser necessário esse index de checkoutSession_customer
        checkoutSessionConfiguration.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(cs => cs.CustomerId);
    }
}
