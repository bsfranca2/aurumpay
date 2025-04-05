using AurumPay.Application.Common;

namespace AurumPay.Application.Domain.Stores;

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