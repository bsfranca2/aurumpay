using Ardalis.Result;

using AurumPay.Core;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Shared;

namespace AurumPay.Application.Customers.AddAddress;

internal sealed class AddCustomerAddressCommandHandler(
    ICustomerRepository customerRepository
) : ICommandHandler<AddCustomerAddressCommand, Result<CustomerAddressDto>>
{
    public async Task<Result<CustomerAddressDto>> Handle(AddCustomerAddressCommand request,
        CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetByIdWithAddressesAsync(new CustomerId(request.CustomerId));

        if (customer is null)
        {
            return Result.Error("Customer not found");
        }

        CustomerAddress address = new(
            new CustomerAddressId(),
            new Cep(request.Cep),
            request.AddressLine1,
            request.AddressLine2,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.Recipient,
            request.IsMain);

        customer.AddAddress(address);

        await customerRepository.UpdateAsync(customer);

        return Result.Success(new CustomerAddressDto(address.Id.Value, address.Cep.Value, address.AddressLine1,
            address.AddressLine2, address.Number, address.Neighborhood, address.City, address.State, address.Recipient,
            address.IsMain));
    }
}