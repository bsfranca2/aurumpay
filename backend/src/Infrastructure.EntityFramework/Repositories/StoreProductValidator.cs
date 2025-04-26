using AurumPay.Domain.Catalog;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class StoreProductValidator(DatabaseContext context, IStoreContext storeContext) : IStoreProductValidator
{
    public async Task<HashSet<ProductId>> GetValidProductsAsync(
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
            .Where(p => p.StoreId == storeContext.GetCurrentStoreId() && inputProductIdSet.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        return validIds.ToHashSet();
    }
}