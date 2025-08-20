using AurumPay.Domain.Payments.Gateways;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class PaymentGatewayEntityTypeConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> paymentGatewayConfiguration)
    {
        paymentGatewayConfiguration.ToTable("PaymentGateways");

        paymentGatewayConfiguration.HasKey(pg => pg.Id);
        
        paymentGatewayConfiguration.Property(pg => pg.Id)
            .HasConversion(new PaymentGatewayIdConverter())
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");

        paymentGatewayConfiguration.Property(pg => pg.Name)
            .HasMaxLength(100)
            .IsRequired();

        paymentGatewayConfiguration.Property(pg => pg.DisplayName)
            .HasMaxLength(150)
            .IsRequired();

        paymentGatewayConfiguration.Property(pg => pg.Type);

        paymentGatewayConfiguration.Property(pg => pg.IsActive)
            .IsRequired();

        paymentGatewayConfiguration.HasIndex(pg => pg.Name).IsUnique();
        paymentGatewayConfiguration.HasIndex(pg => pg.Type);
    }
}