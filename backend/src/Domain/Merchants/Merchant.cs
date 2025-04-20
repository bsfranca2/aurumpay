using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Shared;

namespace AurumPay.Domain.Merchants;

public sealed class Merchant : IEntity<MerchantId>
{
    public MerchantId Id { get; set; }
    public string Name { get; }
    public EmailAddress Email { get; }

    public Merchant(MerchantId id, string name, EmailAddress email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}

public readonly record struct MerchantId(int Value);
