using AurumPay.Application.Domain.Stores;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;

public class MerchantEntityTypeConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> merchantConfiguration)
    {
        merchantConfiguration.ToTable("merchants");

        merchantConfiguration.HasKey(m => m.Id);
    }
}