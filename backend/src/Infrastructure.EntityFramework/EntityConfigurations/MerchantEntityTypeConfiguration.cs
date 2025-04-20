using AurumPay.Domain.Merchants;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class MerchantEntityTypeConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> merchantConfiguration)
    {
        merchantConfiguration.ToTable("Merchants");

        merchantConfiguration.HasKey(m => m.Id);
        
        merchantConfiguration.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");
        
        merchantConfiguration.Property(m => m.Id).HasConversion(new MerchantIdConverter());
        
        merchantConfiguration.Property(m => m.Name).HasMaxLength(255);
        
        merchantConfiguration.Property(m => m.Email).HasConversion(new EmailAddressConverter());
        
        merchantConfiguration.Property(m => m.Email).HasMaxLength(255);
        
        merchantConfiguration.HasIndex(m => m.Email).IsUnique();
        
        // merchantConfiguration.Property(m => m.TaxId).HasMaxLength(14);
    }
}