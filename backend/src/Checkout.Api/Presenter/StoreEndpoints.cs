using AurumPay.Checkout.Api.Common.Interfaces;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Checkout.Api.Presenter;

public class StoreEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/store", async (IHttpContextAccessor httpContextAccessor, DatabaseContext dbContext) =>
        {
            var x = httpContextAccessor!.HttpContext!.Items["StoreId"] as StoreId?;
            var store = await dbContext.Stores.FirstAsync(s => s.Id == x!);
            return Results.Ok(store);
        }).WithTags("Store");
    }
}