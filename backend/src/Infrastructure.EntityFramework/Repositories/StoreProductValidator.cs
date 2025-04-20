using AurumPay.Domain.Catalog;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class StoreProductValidator(DatabaseContext context) : IStoreProductValidator
{
    public async Task<bool> ValidateProductsAsync(StoreId storeId, IEnumerable<ProductId> productIds)
    {
        HashSet<ProductId> productIdSet = productIds.ToHashSet();
        return await context
            .Products
            .CountAsync(p => p.StoreId == storeId && productIdSet.Contains(p.Id)) == productIdSet.Count;
    }
}