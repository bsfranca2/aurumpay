using AurumPay.Application.Common.Interfaces;
using AurumPay.Application.Domain.Catalog;
using AurumPay.Application.Infrastructure.Persistence;

using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AurumPay.Application.Features.Catalog;

public class CreateProduct
{
    public record Request(string Name, decimal Price);
    public record Response(Guid Id, string Name, decimal Price);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products", Handler).WithTags("Products");
        }
    }
    
    public static async Task<IResult> Handler(Request request, AppDbContext context, IValidator<Request> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var product = Product.CreateNew(request.Name, request.Price);

        context.Products.Add(product);

        await context.SaveChangesAsync();

        return Results.Ok(new Response(product.Id, product.Name, product.Price));
    }
}