using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Shared;

namespace AurumPay.Application.Customers.UpdateAddress;

internal sealed class UpdateCustomerAddressCommandHandler(
    ICustomerRepository customerRepository
) : ICommandHandler<UpdateCustomerAddressCommand, Result<CustomerAddressDto>>
{
    public async Task<Result<CustomerAddressDto>> Handle(UpdateCustomerAddressCommand request,
        CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetByIdWithAddressesAsync(new CustomerId(request.CustomerId));

        if (customer is null)
        {
            return Result.Invalid(new ValidationError("Customer not found"));
        }

        CustomerAddress address = new(
            new CustomerAddressId(request.AddressId),
            new Cep(request.Cep),
            request.AddressLine1,
            request.AddressLine2,
            request.Number,
            request.City,
            request.State,
            request.IsMain);
        customer.UpdateAddress(address);

        await customerRepository.UpdateAsync(customer);

        return Result.Success(new CustomerAddressDto(address.Id.Value, address.Cep.Value, address.AddressLine1,
            address.AddressLine2, address.Number, address.City, address.State, address.IsMain));
    }
}