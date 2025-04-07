using AurumPay.Ordering.Domain.SeedWork;

namespace AurumPay.Ordering.Domain.Stores;

public class Store : EntityBase
{
    public Guid MerchantId { get; private set; }
    public string Name { get; private set; }

    public Store(Guid id, Guid merchantId, string name)
    {
        Id = id;
        MerchantId = merchantId;
        Name = name;
    }
}