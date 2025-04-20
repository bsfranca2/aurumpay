using Ardalis.Result;

using AurumPay.Application.Data;
using AurumPay.Application.SeedWork;
using AurumPay.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Customers.GetAddresses;

internal sealed class GetCustomerAddressesQueryHandler(IDatabaseContext context)
    : IQueryHandler<GetCustomerAddressesQuery, Result<List<CustomerAddressDto>>>
{
    public async Task<Result<List<CustomerAddressDto>>> Handle(GetCustomerAddressesQuery request,
        CancellationToken cancellationToken)
    {
        bool customerExists = await context
            .Customers
            .AnyAsync(c => c.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
        {
            return Result.Invalid(new ValidationError("Customer not found"));
        }

        List<CustomerAddressDto> customerAddresses = await context
            .Customers
            .Where(c => c.Id == request.CustomerId)
            .SelectMany(c => c.Addresses)
            .Select(a => new CustomerAddressDto(
                a.Id.Value,
                a.Cep.Value,
                a.AddressLine1,
                a.AddressLine2,
                a.Number,
                a.City,
                a.State,
                a.IsMain
            ))
            .ToListAsync(cancellationToken);

        return customerAddresses;
    }
}