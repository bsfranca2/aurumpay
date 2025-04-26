using AurumPay.Domain.Catalog;
using AurumPay.Domain.Merchants;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.ValueGenerators;

public static class DatabaseContextSeed
{
    public static async Task Seed(DatabaseContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Merchants.AnyAsync(cancellationToken))
        {
            return;
        }
        
        var merchant = new Merchant(new MerchantId(), "João Ap", new EmailAddress("joao@email.com"));
        await context.AddAsync(merchant, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var clothesStore = new Store(new StoreId(), merchant.Id, "Clothes Store");
        await context.AddAsync(clothesStore, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var clothesProduct1 = Product.CreateNew(clothesStore.Id, "Camiseta Básica", 29.99m);
        var clothesProduct2 = Product.CreateNew(clothesStore.Id, "Calça Jeans", 89.90m);
        var clothesProduct3 = Product.CreateNew(clothesStore.Id, "Jaqueta de Couro", 249.99m);
        
        await context.AddAsync(clothesProduct1, cancellationToken);
        await context.AddAsync(clothesProduct2, cancellationToken);
        await context.AddAsync(clothesProduct3, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var shoesStore = new Store(new StoreId(), merchant.Id, "Shoes Store");
        await context.AddAsync(shoesStore, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var shoesProduct1 = Product.CreateNew(shoesStore.Id, "Tênis Esportivo", 149.90m);
        var shoesProduct2 = Product.CreateNew(shoesStore.Id, "Sapato Social", 179.99m);
        var shoesProduct3 = Product.CreateNew(shoesStore.Id, "Sandália de Verão", 59.90m);
        
        await context.AddAsync(shoesProduct1, cancellationToken);
        await context.AddAsync(shoesProduct2, cancellationToken);
        await context.AddAsync(shoesProduct3, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}