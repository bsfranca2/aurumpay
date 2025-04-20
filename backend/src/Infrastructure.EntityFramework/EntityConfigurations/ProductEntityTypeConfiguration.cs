using AurumPay.Domain.Catalog;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> productConfiguration)
    {
        productConfiguration.ToTable("Products");

        productConfiguration.HasKey(p => p.Id);
        
        productConfiguration.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");
        
        productConfiguration.Property(p => p.Id).HasConversion(new ProductIdConverter());

        productConfiguration.Property(p => p.StoreId).HasConversion(new StoreIdConverter());
        
        productConfiguration.HasOne<Store>().WithMany().HasForeignKey(s => s.StoreId);
        
        productConfiguration.Property(p => p.Name).HasMaxLength(255);
        
        productConfiguration.Property(p => p.Price).HasPrecision(12, 2);
    }
}