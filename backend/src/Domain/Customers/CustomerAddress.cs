using Ardalis.GuardClauses;

using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Shared;

namespace AurumPay.Domain.Customers;

public class CustomerAddress : IEntity<CustomerAddressId>
{
    public CustomerAddressId Id { get; init; }
    public Cep Cep { get; private set; }
    public string AddressLine1 { get; private set; }
    public string AddressLine2 { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public bool IsMain { get; private set; }

    public CustomerAddress(CustomerAddressId id, Cep cep, string addressLine1, string addressLine2, string number,
        string city, string state, bool isMain = false)
    {
        Id = id;
        Cep = cep;
        AddressLine1 = Guard.Against.NullOrWhiteSpace(addressLine1);
        AddressLine2 = addressLine2;
        Number = Guard.Against.NullOrWhiteSpace(number);
        City = Guard.Against.NullOrWhiteSpace(city);
        State = Guard.Against.NullOrWhiteSpace(state);
        IsMain = isMain;
    }

    public void SetAsMain()
    {
        IsMain = true;
    }

    public void UnsetAsMain()
    {
        IsMain = false;
    }
}

public readonly record struct CustomerAddressId(long Value);