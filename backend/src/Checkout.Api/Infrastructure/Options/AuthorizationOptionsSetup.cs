using AurumPay.Application.Data;
using AurumPay.Infrastructure.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AurumPay.Checkout.Api.Infrastructure.Options;

public class AuthorizationOptionsSetup : IConfigureOptions<AuthorizationOptions>
{
    public void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.CheckoutSessionCustomer, policy => 
            policy.AddRequirements(new CheckoutSessionCustomerRequirement()));
        
        options.AddPolicy(Policies.CheckoutSessionOrder, policy => 
            policy.AddRequirements(new CheckoutSessionOrderRequirement()));
    }
}