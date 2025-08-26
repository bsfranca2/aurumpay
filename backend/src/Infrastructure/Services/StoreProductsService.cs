using AurumPay.Domain.Catalog;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AurumPay.Infrastructure.Services;

public class ProductMappingService(
    ILogger<ProductMappingService> logger,
    DatabaseContext dbContext
) : IProductMappingService
{
    public async Task<Dictionary<string, ProductId>?> MapPublicIdsToProductIdsAsync(
        StoreId storeId,
        HashSet<string> publicIds,
        CancellationToken cancellationToken)
    {
        if (publicIds.Count == 0)
        {
            return [];
        }

        var products = await dbContext.Products
            .Where(p => p.StoreId == storeId && publicIds.Contains(p.PublicId))
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