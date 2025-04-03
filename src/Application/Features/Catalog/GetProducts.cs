using AurumPay.Application.Common.Interfaces;
using AurumPay.Application.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Features.Catalog;

public static class GetProducts
{
    public record Response(Guid Id, string Name, decimal Price);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", Handler).WithTags("Products");
        }
    }

    public static async Task<IResult> Handler(AppDbContext context)
    {
        var products = await context.Products.ToListAsync();

        var responses = products.Select(p => new Response(p.Id, p.Name, p.Price)).ToList();

        return TypedResults.Ok(responses);
    }
}
