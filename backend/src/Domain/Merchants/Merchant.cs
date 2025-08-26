using AurumPay.Core;
using AurumPay.Domain.Shared;

namespace AurumPay.Domain.Merchants;

public sealed class Merchant : IEntity<MerchantId>
{
    public MerchantId Id { get; }
    public string Name { get; }
    public EmailAddress Email { get; }

    public Merchant(MerchantId id, string name, EmailAddress email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}