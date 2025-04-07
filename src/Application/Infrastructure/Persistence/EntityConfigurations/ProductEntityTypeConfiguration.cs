using AurumPay.Ordering.Domain.Catalog;
using AurumPay.Ordering.Domain.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> productConfiguration)
    {
        productConfiguration.ToTable("products");

        productConfiguration.HasKey(p => p.Id);
        
        productConfiguration.HasOne<Store>().WithMany().HasForeignKey(s => s.StoreId);
    }
}