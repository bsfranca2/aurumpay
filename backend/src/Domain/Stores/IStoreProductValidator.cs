using AurumPay.Domain.Catalog;

namespace AurumPay.Domain.Stores;

public interface IStoreProductValidator
{
    /// <summary>
    /// Validates that all specified products exist and belong to the given store.
    /// </summary>
    /// <param name="storeId">ID of the store</param>
    /// <param name="productIds">IDs of the products to validate</param>
    /// <returns>True if all products exist and belong to the store; otherwise, false</returns>
    Task<bool> ValidateProductsAsync(StoreId storeId, IEnumerable<ProductId> productIds);
}