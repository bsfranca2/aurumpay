using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Catalog;

public sealed class Product : IEntity<ProductId>
{
    public ProductId Id { get; }
    public StoreId StoreId { get; }
    public string PublicId { get; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product(ProductId id, StoreId storeId, string publicId, string name, decimal price)
    {
        Id = id;
        StoreId = storeId;
        PublicId = publicId;
        Name = name;
        Price = price;
    }

    public static Product CreateNew(StoreId storeId, string name, decimal price)
    {
        return new Product(new ProductId(), storeId, PublicIdGenerator.GeneratePublicId(), name, price);
    }
}

public readonly record struct ProductId(long Value);