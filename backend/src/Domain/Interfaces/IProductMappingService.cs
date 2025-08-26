using AurumPay.Domain.Catalog;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Interfaces;

public interface IProductMappingService
{
    /// <summary>
    ///     Maps public product identifiers to their corresponding internal product identifiers for a specific store.
    /// </summary>
    /// <param name="storeId">The identifier of the store to search for products.</param>
    /// <param name="publicIds">Collection of public identifiers to be mapped.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A dictionary where each key is a public identifier and its value is the corresponding
    ///     internal ProductId; returns null if any public identifier cannot be found.
    /// </returns>
    /// <remarks>
    ///     This method follows an "all or nothing" approach - either all public identifiers are
    ///     successfully mapped to their internal IDs, or null is returned if any single public
    ///     identifier cannot be mapped.
    /// </remarks>
    Task<Dictionary<string, ProductId>?> MapPublicIdsToProductIdsAsync(
        StoreId storeId,
        HashSet<string> publicIds,
        CancellationToken cancellationToken = default);
}