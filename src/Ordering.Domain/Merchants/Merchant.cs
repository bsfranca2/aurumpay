using AurumPay.Ordering.Domain.SeedWork;

namespace AurumPay.Ordering.Domain.Merchants;

public class Merchant : EntityBase
{
    public string Name { get; private set; }
    public string TaxId { get; private set; }

    public Merchant(Guid id, string name, string taxId)
    {
        Id = id;
        Name = name;
        TaxId = taxId;
    }
}