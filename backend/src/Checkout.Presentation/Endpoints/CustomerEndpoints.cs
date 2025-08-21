using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.Customers.AddAddress;
using AurumPay.Application.Customers.UpdateAddress;
using AurumPay.Application.Data;
using AurumPay.Checkout.Presentation.Contracts;
using AurumPay.Checkout.Presentation.Utilities;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Carter;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace AurumPay.Checkout.Presentation.Endpoints;

public class CustomerEndpoints : CarterModule
{
    public CustomerEndpoints() : base("/customer")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/addresses", AddCustomerAddress)
            .WithName(nameof(AddCustomerAddress))
            .RequireAuthorization(Policies.CheckoutSessionCustomer)
            .Produces<CustomerAddressDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        app.MapPut("/addresses/{addressId:long}", UpdateCustomerAddress)
            .WithName(nameof(UpdateCustomerAddress))
            .RequireAuthorization(Policies.CheckoutSessionCustomer)
            .Produces<CustomerAddressDto>()
            .ProducesValidationProblem();
    }

    private static async Task<IResult> AddCustomerAddress(
        CustomerAddressRequest request,
        ISender sender,
        ICheckoutContext checkoutContext)
    {
        CheckoutSession checkoutSession = await checkoutContext.SessionManager.GetRequiredSessionAsync();
        AddCustomerAddressCommand command = new(checkoutSession.GetRequiredCustomerId().Value, request.Cep, request.AddressLine1,
            request.AddressLine2, request.Number, request.Neighborhood, request.City, request.State, request.Recipient, true);
        Result<CustomerAddressDto> result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> UpdateCustomerAddress(
        long addressId,
        CustomerAddressRequest request,
        ISender sender,
        ICheckoutContext checkoutContext)
    {
        CheckoutSession checkoutSession = await checkoutContext.SessionManager.GetRequiredSessionAsync();
        UpdateCustomerAddressCommand command = new(checkoutSession.GetRequiredCustomerId().Value, addressId, request.Cep,
            request.AddressLine1, request.AddressLine2, request.Number, request.Neighborhood, request.City, request.State,
            request.Recipient, true);
        Result<CustomerAddressDto> result = await sender.Send(command);
        return result.ToMinimalApiResult();
    }
}