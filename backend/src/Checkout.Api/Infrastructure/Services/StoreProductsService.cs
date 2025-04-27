using AurumPay.Domain.Catalog;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class StoreProductService(
    ILogger<StoreProductService> logger,
    DatabaseContext dbContext,
    IStoreContext storeContext) : IStoreProductService
{
    public async Task<Dictionary<string, ProductId>?> MapPublicIdsToProductIdsAsync(
        HashSet<string> publicIds,
        CancellationToken cancellationToken)
    {
        if (publicIds.Count == 0)
        {
            return [];
        }

        StoreId currentStoreId = storeContext.GetCurrentStoreId();

        var products = await dbContext.Set<Product>()
            .Where(p => p.StoreId == currentStoreId && publicIds.Contains(p.PublicId))
            .Select(p => new { p.PublicId, p.Id })
            .ToListAsync(cancellationToken);

        if (products.Count != publicIds.Count)
        {
            HashSet<string> foundPublicIds = products.Select(p => p.PublicId).ToHashSet();
            List<string> missingPublicIds = publicIds.Where(id => !foundPublicIds.Contains(id)).ToList();
            logger.LogDebug("The following public IDs were not found: {ids}", string.Join(", ", missingPublicIds));

            return null;
        }

        return products.ToDictionary(
            p => p.PublicId,
            p => p.Id
        );
    }
}