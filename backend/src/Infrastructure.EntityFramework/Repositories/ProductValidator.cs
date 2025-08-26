using AurumPay.Domain.Catalog;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class ProductValidator(DatabaseContext context) : IProductValidator
{
    public async Task<HashSet<ProductId>> FilterValidProductsAsync(
        StoreId storeId,
        IEnumerable<ProductId> productIds,
        CancellationToken cancellationToken)
    {
        HashSet<ProductId> inputProductIdSet = productIds.ToHashSet();

        if (inputProductIdSet.Count == 0)
        {
            return [];
        }

        List<ProductId> validIds = await context
            .Products
            .Where(p => p.StoreId == storeId && inputProductIdSet.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        return validIds.ToHashSet();
    }
}