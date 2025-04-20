using AurumPay.Domain.Merchants;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.Stores;

public sealed class Store : IEntity<StoreId>
{
    public StoreId Id { get; set; }
    public MerchantId MerchantId { get; }
    public string Name { get; }

    public Store(StoreId id, MerchantId merchantId, string name)
    {
        Id = id;
        MerchantId = merchantId;
        Name = name;
    }
}

public readonly record struct StoreId(int Value);
