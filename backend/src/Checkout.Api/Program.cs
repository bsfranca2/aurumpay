using System.IdentityModel.Tokens.Jwt;

using AurumPay.Checkout.Api;
using AurumPay.Checkout.Api.Infrastructure.Endpoints;
using AurumPay.Infrastructure.EntityFramework;

using Carter;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddCarter()
    .AddEndpoints()
    .AddEndpointsApiExplorer()
    .AddOpenApi(options =>
    {
        options.AddSchemaTransformer((schema, context, cancellationToken) =>
        {
            if (context.JsonTypeInfo.Type.ToString() == "AurumPay.Checkout.Presentation.CheckoutSessions.CreateCheckoutDto")
            {
                // Certifique-se de que o schema é do tipo objeto
                schema.Type = "object";

                // Inicialize a coleção de propriedades se for nula
                schema.Properties ??= new Dictionary<string, OpenApiSchema>();

                // Adicione a propriedade CartItems com o formato correto
                schema.Properties["cartItems"] = new OpenApiSchema
                {
                    Type = "object", AdditionalProperties = new OpenApiSchema { Type = "integer", Format = "int32" }
                };

                // Se você quiser tornar a propriedade obrigatória
                schema.Required ??= new HashSet<string>();
                schema.Required.Add("cartItems");
            }

            return Task.CompletedTask;
        });
    })
    .AddDefaultCorsPolicy();

builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    DatabaseContext databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await databaseContext.Database.MigrateAsync();
    await databaseContext.Database.EnsureCreatedAsync();
}

app.UseCors();

app.UseForwardedHeaders();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseExceptionHandler();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<StoreTenantMiddleware>();

app.MapDefaultEndpoints()
    .MapEndpoints();

app.MapCarter();

app.UseJwksDiscovery();

app.Run();