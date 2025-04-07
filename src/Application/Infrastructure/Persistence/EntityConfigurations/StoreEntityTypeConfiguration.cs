using AurumPay.Ordering.Domain.Merchants;
using AurumPay.Ordering.Domain.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> storeConfiguration)
    {
        storeConfiguration.ToTable("stores");

        storeConfiguration.HasKey(s => s.Id);

        storeConfiguration.HasOne<Merchant>().WithMany().HasForeignKey(s => s.MerchantId);
    }
}