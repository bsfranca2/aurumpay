using AurumPay.Domain.Catalog;

namespace AurumPay.Domain.Stores;

public interface IProductValidator
{
    /// <summary>
    /// Filters a collection of product IDs, returning only those that exist and belong to the specified store.
    /// </summary>
    /// <param name="storeId">The identifier of the store to validate products against.</param>
    /// <param name="productIds">The collection of product IDs to validate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A HashSet containing only the valid ProductIds from the input collection for the specified store.</returns>
    Task<HashSet<ProductId>> FilterValidProductsAsync(
        StoreId storeId,
        IEnumerable<ProductId> productIds,
        CancellationToken cancellationToken = default);
}