using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("Orders");

        orderConfiguration.HasKey(o => o.Id);
        
        orderConfiguration.Property(o => o.Id)
            .HasConversion(new OrderIdConverter())
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");

        orderConfiguration.Property(o => o.StoreId)
            .HasConversion(new StoreIdConverter());

        orderConfiguration.HasOne<Store>()
            .WithMany()
            .HasForeignKey(o => o.StoreId);

        orderConfiguration.Property(o => o.CustomerId)
            .HasConversion(new CustomerIdConverter());

        orderConfiguration.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
        
        orderConfiguration.Property(o => o.OrderTotal)
            .HasColumnType("decimal(18,2)");

        orderConfiguration.Property(o => o.PaidAmount)
            .HasColumnType("decimal(18,2)");

        orderConfiguration.Property(o => o.CreatedAtUtc);

        orderConfiguration.Property(o => o.PaidDateUtc);

        orderConfiguration.Property(o => o.Status);

        orderConfiguration.Property(o => o.PaymentStatus);
        
        orderConfiguration.HasOne<Store>()
            .WithMany()
            .HasForeignKey(o => o.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        orderConfiguration.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        orderConfiguration.OwnsMany(o => o.OrderItems, orderItemBuilder =>
        {
            orderItemBuilder.ToTable("OrderItems");

            orderItemBuilder.Property(oi => oi.Id)
                .HasConversion(
                    orderItemId => orderItemId.Value,
                    value => new OrderItemId(value)
                )
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1000, incrementBy: 1)
                .HasColumnType("bigint");;

            orderItemBuilder.HasKey(oi => oi.Id);
            
            orderItemBuilder.Property(oi => oi.ProductId)
                .HasConversion(new ProductIdConverter())
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(startValue: 1000, incrementBy: 1)
                .HasColumnType("bigint");

            orderItemBuilder.Property(oi => oi.ProductName)
                .HasMaxLength(255)
                .IsRequired();

            orderItemBuilder.Property(oi => oi.Quantity)
                .HasColumnType("integer");

            orderItemBuilder.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            orderItemBuilder.Ignore(oi => oi.TotalPrice);

            orderItemBuilder.Property(oi => oi.CreatedOnUtc);
        });
    }
}