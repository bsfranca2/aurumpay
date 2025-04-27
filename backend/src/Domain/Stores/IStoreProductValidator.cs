using AurumPay.Domain.Catalog;

namespace AurumPay.Domain.Stores;

public interface IStoreProductValidator
{
    /// <summary>
    /// Filters a collection of product IDs, returning only those that exist
    /// and belong to the current store context.
    /// </summary>
    /// <param name="productIds">The collection of product IDs to validate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A HashSet containing only the valid ProductIds from the input collection for the current store.</returns>
    Task<HashSet<ProductId>> GetValidProductsAsync(
        IEnumerable<ProductId> productIds,
        CancellationToken cancellationToken = default);
}