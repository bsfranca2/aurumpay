using AurumPay.Domain.Customers;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Payments.Transactions;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> paymentConfiguration)
    {
        paymentConfiguration.ToTable("Payments");

        paymentConfiguration.HasKey(p => p.Id);
        
        paymentConfiguration.Property(p => p.Id)
            .HasConversion(new PaymentIdConverter());
        
        paymentConfiguration.Property(p => p.OrderId)
            .HasConversion(new OrderIdConverter());

        paymentConfiguration.Property(p => p.CustomerId)
            .HasConversion(new CustomerIdConverter());

        paymentConfiguration.Property(p => p.PaymentMethodId)
            .HasConversion(new PaymentMethodIdConverter())
            .IsRequired();

        paymentConfiguration.Property(p => p.PaymentGatewayId)
            .HasConversion(new PaymentGatewayIdConverter())
            .IsRequired();

        paymentConfiguration.Property(p => p.Amount)
            .HasPrecision(12, 2)
            .IsRequired();

        paymentConfiguration.Property(p => p.Status);

        paymentConfiguration.Property(p => p.GatewayTransactionId)
            .HasMaxLength(255);

        paymentConfiguration.Property(p => p.GatewayResponse)
            .HasColumnType("text");

        paymentConfiguration.Property(p => p.CreatedAt)
            .IsRequired();

        paymentConfiguration.Property(p => p.ProcessedAt);

        paymentConfiguration.Property(p => p.FailureReason)
            .HasMaxLength(1000);
        
        // paymentConfiguration.HasOne<Order>()
        //     .WithMany()
        //     .HasForeignKey(p => p.OrderId)
        //     .OnDelete(DeleteBehavior.Restrict);

        paymentConfiguration.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        paymentConfiguration.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        paymentConfiguration.HasOne<PaymentGateway>()
            .WithMany()
            .HasForeignKey(p => p.PaymentGatewayId)
            .OnDelete(DeleteBehavior.Restrict);

        paymentConfiguration.HasIndex(p => p.OrderId);
        paymentConfiguration.HasIndex(p => p.CustomerId);
        paymentConfiguration.HasIndex(p => p.Status);
        paymentConfiguration.HasIndex(p => p.GatewayTransactionId);
        paymentConfiguration.HasIndex(p => p.CreatedAt);
    }
}