using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Customers;

public sealed class Customer : IEntity<CustomerId>
{
    private readonly List<CustomerAddress> _addresses = [];

    public CustomerId Id { get; set; }
    public StoreId StoreId { get; }
    public string FullName { get; }
    public EmailAddress Email { get; }
    public Cpf Cpf { get; }
    public Telephone MobilePhone { get; }
    public IReadOnlyCollection<CustomerAddress> Addresses => _addresses.AsReadOnly();

    public CustomerAddress? MainAddress => _addresses.FirstOrDefault(a => a.IsMain);

    private Customer(CustomerId id, StoreId storeId, string fullName, EmailAddress email, Cpf cpf,
        Telephone mobilePhone)
    {
        Id = id;
        StoreId = storeId;
        FullName = fullName;
        Email = email;
        Cpf = cpf;
        MobilePhone = mobilePhone;
    }

    public static Customer Create(StoreId storeId, string fullName, EmailAddress email, Cpf cpf,
        Telephone mobilePhone)
    {
        return new Customer(new CustomerId(), storeId, fullName, email, cpf, mobilePhone);
    }

    public void AddAddress(CustomerAddress address)
    {
        if (_addresses.Count == 0)
        {
            address.SetAsMain();
        }
        else if (address.IsMain)
        {
            CustomerAddress? currentMainAddress = _addresses.FirstOrDefault(a => a.IsMain);
            currentMainAddress?.UnsetAsMain();
        }

        _addresses.Add(address);
    }

    public void UpdateAddress(CustomerAddress address)
    {
        CustomerAddress? existingAddress = _addresses.FirstOrDefault(a => a.Id == address.Id) ??
                                           throw new InvalidOperationException(
                                               $"Address with ID {address.Id} not found.");
        
        if (address.IsMain && !existingAddress.IsMain)
        {
            CustomerAddress? currentMainAddress = _addresses.FirstOrDefault(a => a.IsMain);
            currentMainAddress?.UnsetAsMain();
        }

        _addresses.Remove(existingAddress);
        _addresses.Add(address);

        if (_addresses.Count != 0 && !_addresses.Any(a => a.IsMain))
        {
            _addresses.First().SetAsMain();
        }
    }

    public void RemoveAddress(CustomerAddressId addressId)
    {
        CustomerAddress address = _addresses.FirstOrDefault(a => a.Id == addressId) ??
                                  throw new InvalidOperationException($"Address with ID {addressId} not found.");

        bool wasMain = address.IsMain;
        _addresses.Remove(address);

        if (wasMain && _addresses.Count != 0)
        {
            _addresses.First().SetAsMain();
        }
    }

    public void SetMainAddress(CustomerAddressId addressId)
    {
        CustomerAddress address = _addresses.FirstOrDefault(a => a.Id == addressId) ??
                                  throw new InvalidOperationException($"Address with ID {addressId} not found.");

        CustomerAddress? currentMainAddress = _addresses.FirstOrDefault(a => a.IsMain);
        currentMainAddress?.UnsetAsMain();

        address.SetAsMain();
    }
}

public readonly record struct CustomerId(long Value);