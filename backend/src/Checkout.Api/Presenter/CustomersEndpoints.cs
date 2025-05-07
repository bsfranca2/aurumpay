using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.Customers.AddAddress;
using AurumPay.Application.Customers.RemoveAddress;
using AurumPay.Application.Customers.UpdateAddress;
using AurumPay.Checkout.Api.Common.Interfaces;
using AurumPay.Checkout.Api.Infrastructure.Endpoints;

using MediatR;

namespace AurumPay.Checkout.Api.Presenter;

public class CustomersEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/customers/address", async (AddCustomerAddressCommand command, ISender sender) =>
        {
            Result<CustomerAddressDto> result = await sender.Send(command);
            return result.ToMinimalApiResult();
        }).WithTags("Customers");

        app.MapPut("/customers/address", async (UpdateCustomerAddressCommand command, ISender sender) =>
        {
            Result<CustomerAddressDto> result = await sender.Send(command);
            return result.ToMinimalApiResult();
        }).WithTags("Customers");

        app.MapDelete("/customers/{customerId:int}/address/{addressId:int}",
            async (long customerId, long addressId, ISender sender) =>
            {
                RemoveCustomerAddressCommand command = new(customerId, addressId);
                Result result = await sender.Send(command);
                return result.ToMinimalApiResult();
            }).WithTags("Customers");
    }
}