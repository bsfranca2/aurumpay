using AurumPay.Domain.Catalog;

namespace AurumPay.Domain.Interfaces;

public interface IStoreProductService
{
    /// <summary>
    ///     Maps public product identifiers to their corresponding internal product identifiers.
    /// </summary>
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
        HashSet<string> publicIds,
        CancellationToken cancellationToken = default);
}