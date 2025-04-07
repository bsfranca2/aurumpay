using AurumPay.Ordering.Domain.Catalog;
using AurumPay.Ordering.Domain.Checkout;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Application.Infrastructure.Persistence.EntityConfigurations;

public class CartItemEntityTypeConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> cartItemConfiguration)
    {
        cartItemConfiguration.ToTable("cart_items");

        cartItemConfiguration.HasKey(m => m.Id);
        
        cartItemConfiguration.HasOne<Product>().WithMany().HasForeignKey(s => s.ProductId);
    }
}