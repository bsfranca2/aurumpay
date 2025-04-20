using AurumPay.Domain.Merchants;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> storeConfiguration)
    {
        storeConfiguration.ToTable("Stores");

        storeConfiguration.HasKey(s => s.Id);
        
        storeConfiguration.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");
        
        storeConfiguration.Property(s => s.Id).HasConversion(new StoreIdConverter());
        
        storeConfiguration.Property(s => s.MerchantId).HasConversion(new MerchantIdConverter());
        
        storeConfiguration.HasOne<Merchant>().WithMany().HasForeignKey(s => s.MerchantId);

        storeConfiguration.Property(s => s.Name).HasMaxLength(150);
    }
}