using AurumPay.Application.Common.Interfaces;
using AurumPay.Application.Features.Stores;
using AurumPay.Application.Infrastructure.Persistence;
using AurumPay.Ordering.Domain.Catalog;
using AurumPay.Ordering.Domain.Merchants;
using AurumPay.Ordering.Domain.Stores;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Features.DataSeed;

public class Seed
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/seed", Handler).WithTags("Seed");
        }
    }

    private static async Task<IResult> Handler(AppDbContext context)
    {
        var merchantId = Guid.Parse("aa9225a4-f829-439d-a610-7e1ef43d5cee");
        if (!(await context.Merchants.AnyAsync(m => m.Id == merchantId)))
        {
            var newMerchant = new Merchant(merchantId, "Erick Costa Cavalcanti", "75159264574");
            context.Merchants.Add(newMerchant);
        }

        var storeId = StoreConstant.GlassesStoreId;
        if (!(await context.Stores.AnyAsync(s => s.Id == storeId)))
        {
            var newStore = new Store(storeId, merchantId, "Loja de Oculos");
            context.Stores.Add(newStore);
        }

        if (!(await context.Products.AnyAsync(p => p.StoreId == storeId)))
        {
            context.Products.Add(Product.CreateNew(storeId, "Oculos de sol", 100));
            context.Products.Add(Product.CreateNew(storeId, "Oculos de desanso", 120));
            context.Products.Add(Product.CreateNew(storeId, "Oculos de lente", 150));
        }
        
        await context.SaveChangesAsync();
        return Results.Ok();
    }
}